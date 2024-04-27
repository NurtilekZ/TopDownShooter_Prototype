using System.Linq;
using System.Threading.Tasks;
using _current.Infrastructure.AssetManagement;
using _current.Infrastructure.Factories.Interfaces;
using _current.Infrastructure.SceneManagement;
using _current.Services.StaticData;
using _current.Services.UI;
using _current.UI.Screens.MainMenu;

namespace _current.Infrastructure.States
{
    internal class MainMenuState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IUIService _uiService;
        private readonly IUIFactory _uiFactory;
        private readonly SceneLoader _sceneLoader;
        private IStaticDataService _staticDataService;

        public MainMenuState(
            GameStateMachine gameStateMachine,
            IUIService uiService,
            SceneLoader sceneLoader,
            IUIFactory uiFactory,
            IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
            _gameStateMachine = gameStateMachine;
            _uiService = uiService;
            _sceneLoader = sceneLoader;
            _uiFactory = uiFactory;
        }

        public async void Enter()
        {
            await _uiFactory.WarmUpForState(new []{AssetsPath.MainMenuScreen});
            var sceneInstance = await _sceneLoader.Load(SceneName.Meta, OnLoaded);
        }

        public void Exit()
        {
            _uiFactory.CleanUpForState();
            _uiService.Close<MenuScreenViewModel>();
        }

        private async void OnLoaded()
        {
            await InitUI();
            await InitMainMenu();
        }

        private async Task InitUI()
        {
            await _uiFactory.CreateSceneRootCanvas();
        }

        private async Task InitMainMenu()
        {
            await _uiService.Open(new MenuScreenViewModel(_gameStateMachine, _staticDataService));
        }
    }
}