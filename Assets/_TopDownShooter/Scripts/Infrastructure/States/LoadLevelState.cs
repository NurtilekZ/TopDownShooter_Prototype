using System.Threading.Tasks;
using Data;
using Infrastructure.Factories.Interfaces;
using Infrastructure.SceneManagement;
using Infrastructure.States.Interfaces;
using StaticData;
using UnityEngine;

namespace Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<StageStaticData>
    {
        private readonly IHeroFactory _heroFactory;
        private readonly SceneLoader _sceneLoader;
        private readonly IStageFactory _stageFactory;
        private readonly GameStateMachine _stateMachine;
        private readonly IUIFactory _uiFactory;

        private StageStaticData _pendingStageStaticData;
        private StageProgressData _stageProgressData;

        public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader, IUIFactory uiFactory,
            IHeroFactory heroFactory, IStageFactory stageFactory)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _uiFactory = uiFactory;
            _heroFactory = heroFactory;
            _stageFactory = stageFactory;
        }

        public async void Enter(StageStaticData stageStaticData)
        {
            _pendingStageStaticData = stageStaticData;
            _stageProgressData = new StageProgressData();

            //TODO: show curtain & warm-up enemy factory

            await _heroFactory.WarmUp();
            await _stageFactory.WarmUp();

            var sceneInstance = await _sceneLoader.Load(SceneName.Core, OnLoaded);
        }

        public void Exit()
        {
            _stageFactory.CleanUp();
            _pendingStageStaticData = null;
        }

        private async void OnLoaded(SceneName obj)
        {
            await InitUIRoot();
            await InitGameWorld();
            await InitUI();

            _stateMachine.Enter<GameLoopState>();
        }

        private async Task InitUIRoot()
        {
            await _uiFactory.CreateUIRoot();
        }

        private async Task InitGameWorld()
        {
            _stageProgressData.Hero = await SetupHero();
            SetupCamera(_stageProgressData.Hero);

            await SetupEnemySpawners();
        }

        private async Task InitUI()
        {
            await _uiFactory
                .CreateHud()
                .ContinueWith(
                    m => m.Result.Initialize(_pendingStageStaticData, _stageProgressData),
                    TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async Task<GameObject> SetupHero()
        {
            return await _heroFactory.Create(_pendingStageStaticData.PlayerSpawnPoint);
        }

        private void SetupCamera(GameObject hero)
        {
            //TODO: Set up Camera flow
        }

        private async Task SetupEnemySpawners()
        {
            foreach (var spawnerData in _pendingStageStaticData.EnemySpawners)
            {
                var spawner = await _stageFactory.CreateEnemySpawner(spawnerData.EnemyType, spawnerData.Position);
                _stageProgressData.EnemySpawners.Add(spawner);
            }
        }
    }
}