using System.Threading.Tasks;
using _current.Data.Data;
using _current.Infrastructure.Factories.Interfaces;
using _current.Infrastructure.SceneManagement;
using _current.Services.Input;
using _current.Services.UI;
using _current.StaticData;
using _current.StaticData.ScriptableObjects;
using _current.UI.Screens.GameHUD;
using _current.UI.Screens.Loading;
using UnityEngine;

namespace _current.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<LevelStaticData>
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IUIFactory _uiFactory;
        private readonly IHeroFactory _heroFactory;
        private readonly ICameraFactory _cameraFactory;
        private readonly ILevelFactory _levelFactory;
        private readonly IUIService _iUiService;
        private string _sceneName;

        private LevelStaticData _pendingLevelStaticData;
        private LevelProgressData _levelProgressData;
        private IInputService _inputService;

        public LoadLevelState(GameStateMachine stateMachine,
            SceneLoader sceneLoader,
            IUIFactory uiFactory,
            IHeroFactory heroFactory,
            IUIService iUiService,
            ICameraFactory cameraFactory,
            IInputService inputService,
            ILevelFactory levelFactory)
        {
            _inputService = inputService;
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _heroFactory = heroFactory;
            _iUiService = iUiService;
            _levelFactory = levelFactory;
            _cameraFactory = cameraFactory;
            _uiFactory = uiFactory;
        }

        public async void Enter(LevelStaticData levelStaticData)
        {
            _pendingLevelStaticData = levelStaticData;
            _levelProgressData = new LevelProgressData();

            await _iUiService.Open(new LoadingScreenViewModel(_inputService));
            var sceneInstance = await _sceneLoader.Load(SceneName.Core, OnLoaded);
        }

        public void Exit()
        {
            _levelFactory.CleanUp();
            _pendingLevelStaticData = null;
            _iUiService.Close<LoadingScreenViewModel>();
        }

        private async void OnLoaded()
        {
            await InitSceneCanvas();
            await InitGameWorld();
            await InitUI();
            
            _stateMachine.Enter<GameLoopState>();
        }

        private async Task InitSceneCanvas()
        {
            await _uiFactory.CreateSceneRootCanvas();
        }

        private async Task InitGameWorld()
        {
            await SetupInterestPoints();

            _levelProgressData.Hero = await SetupHero();
            await SetupPlayerCamera(_levelProgressData.Hero);
            await SetupEnemySpawner();
        }

        private async Task InitUI()
        {
            var gameHUDViewModel = new GameHUDViewModel(_levelProgressData, _pendingLevelStaticData);
            await _iUiService.Open(gameHUDViewModel);
        }

        private async Task<GameObject> SetupHero() => 
            await _heroFactory.Create(_pendingLevelStaticData.PlayerSpawnPoint);

        private async Task SetupPlayerCamera(GameObject hero)
        {
            await _cameraFactory.CreateHeroCamera(hero.transform);
        }

        private async Task SetupEnemySpawner()
        {
            foreach (var spawnerData in _pendingLevelStaticData.EnemySpawners)
            {
                var spawner = await _levelFactory.CreateSpawner(spawnerData);
                _levelProgressData.EnemySpawners.Add(spawner);
            } 
        }

        private async Task SetupInterestPoints()
        {
            foreach (var interestPoint in _pendingLevelStaticData.MissionPointSpawners)
            {
                await _levelFactory.CreateMissionPoint(interestPoint);
            }
        }
    }
}