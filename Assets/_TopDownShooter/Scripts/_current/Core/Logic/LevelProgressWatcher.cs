using _current.Infrastructure.States;
using _current.Services.Logging;
using UnityEngine;
using Zenject;

namespace _current.Core.Logic
{
    public class LevelProgressWatcher : MonoBehaviour
    {
        private GameStateMachine _gameStateMachine;
        private ILoggingService _loggingService;

        [Inject]
        private void Construct(GameStateMachine gameStateMachine, ILoggingService loggingService)
        {
            _gameStateMachine = gameStateMachine;
            _loggingService = loggingService;
        }

        public void RunLevel()
        {
            _loggingService.LogMessage("Level Ran", this, LoggingTag.Game);
        }

        public void CompleteLevel()
        {
            
        }
    }
}