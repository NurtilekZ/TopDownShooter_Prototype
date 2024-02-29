using System.Threading.Tasks;
using Infrastructure.Factories.Interfaces;
using Infrastructure.SceneManagement;
using Infrastructure.States.Interfaces;

namespace Infrastructure.States
{
    public class LoadMetaState : IState
    {
        private readonly SceneLoader _sceneLoader;
        private readonly GameStateMachine _stateMachine;
        private readonly IUIFactory _uiFactory;

        public LoadMetaState(GameStateMachine stateMachine, IUIFactory uiFactory, SceneLoader sceneLoader)
        {
            _stateMachine = stateMachine;
            _uiFactory = uiFactory;
            _sceneLoader = sceneLoader;
        }

        public async void Enter()
        {
            await _uiFactory.WarmUp();

            var sceneInstance = await _sceneLoader.Load(SceneName.Meta, OnLoaded);
        }

        public void Exit()
        {
            _uiFactory.CleanUp();
        }

        private async void OnLoaded(SceneName sceneName)
        {
            await InitUIRoot();
        }

        private async Task InitUIRoot()
        {
            await _uiFactory.CreateUIRoot();
        }
    }
}