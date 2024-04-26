using System.Collections.Generic;
using _current.Infrastructure.Factories.Interfaces;
using _current.Services;
using _current.Services.UI;
using _current.UI.Screens.Loading;

namespace _current.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly List<IInitializableAsync> _initializableServices;
        private readonly IUIFactory _uiFactory;
        private IUIService _uiService;

        public BootstrapState(
            GameStateMachine stateMachine,
            IUIFactory uiFactory,
            IUIService uiService,
            List<IInitializableAsync> initializableServices)
        {
            _uiService = uiService;
            _stateMachine = stateMachine;
            _initializableServices = initializableServices;
            _uiFactory = uiFactory;
        }

        public async void Enter()
        {
            foreach (var service in _initializableServices) 
                await service.InitializeAsync();
            
            await _uiFactory.WarmUp();
            InitRootUI();
            
            _stateMachine.Enter<LoadProgressState>();
        }

        private void InitRootUI()
        {
            _uiFactory.CreateRootCanvas();
            _uiService.Open(new LoadingScreenViewModel(null));
        }

        public void Exit()
        {
            
        }
    }
}