using System;
using System.Collections.Generic;
using Infrastructure.Factories;
using Infrastructure.States.Interfaces;
using Services.Logging;
using Zenject;

namespace Infrastructure.States
{
    public class GameStateMachine : IInitializable
    {
        private readonly ILoggingService _logger;
        private readonly StateFactory _stateFactory;
        private IExitableState _currentState;

        private Dictionary<Type, IExitableState> _states;

        public GameStateMachine(StateFactory stateFactory, ILoggingService loggingService)
        {
            _logger = loggingService;
            _stateFactory = stateFactory;
        }

        public void Initialize()
        {
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootstrapState)] = _stateFactory.CreateState<BootstrapState>(),
                [typeof(LoadProgressState)] = _stateFactory.CreateState<LoadProgressState>(),
                [typeof(LoadMetaState)] = _stateFactory.CreateState<LoadMetaState>(),
                [typeof(LoadLevelState)] = _stateFactory.CreateState<LoadLevelState>(),
                [typeof(GameLoopState)] = _stateFactory.CreateState<GameLoopState>()
            };
            Enter<BootstrapState>();
        }

        public void Enter<TState>() where TState : class, IState
        {
            ChangeState<TState>().Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            ChangeState<TState>().Enter(payload);
        }

        private TState GetState<TState>() where TState : class, IExitableState
        {
            return _states[typeof(TState)] as TState;
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _currentState?.Exit();

            var state = GetState<TState>();
            _currentState = state;

            _logger.LogMessage($"State Changed to {_currentState.GetType().Name}", this);

            return state;
        }
    }
}