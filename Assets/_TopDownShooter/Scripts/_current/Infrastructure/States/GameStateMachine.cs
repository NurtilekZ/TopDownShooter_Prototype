using System;
using System.Collections.Generic;
using _current.Infrastructure.Factories;
using _current.Services.Logging;
using UnityEngine;
using Zenject;

namespace _current.Infrastructure.States
{
    public class GameStateMachine : IInitializable
    {
        private readonly StateFactory _stateFactory;
        private readonly ILoggingService _loggingService;

        private Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        public GameStateMachine(StateFactory stateFactory, ILoggingService loggingService)
        {
            _stateFactory = stateFactory;
            _loggingService = loggingService;
        }

        public void Initialize()
        {
            _states = new Dictionary<Type, IExitableState>()
            {
                [typeof(BootstrapState)] = _stateFactory.CreateState<BootstrapState>(),
                [typeof(LoadProgressState)] = _stateFactory.CreateState<LoadProgressState>(),
                [typeof(MainMenuState)] = _stateFactory.CreateState<MainMenuState>(),
                [typeof(LoadLevelState)] = _stateFactory.CreateState<LoadLevelState>(),
                [typeof(BootstrapState)] = _stateFactory.CreateState<BootstrapState>(),
                [typeof(GameLoopState)] = _stateFactory.CreateState<GameLoopState>(),
            };
            
            Enter<BootstrapState>();
        }

        public void Enter<TState>() where TState : class, IState => 
            ChangeState<TState>().Enter();

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload> => 
            ChangeState<TState>().Enter(payload);

        private TState GetState<TState>() where TState : class, IExitableState => 
            _states[typeof(TState)] as TState;

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();
            var state = GetState<TState>();
            _loggingService.LogMessage($"State Changed from {(_activeState?.GetType().Name != null ? _activeState?.GetType().Name : "Entry Point")} to {state?.GetType().Name}", this, LoggingTag.StateMachine);
            _activeState = state;
            return state;
        }

        public void Dispose()
        {
            
        }
    }
}