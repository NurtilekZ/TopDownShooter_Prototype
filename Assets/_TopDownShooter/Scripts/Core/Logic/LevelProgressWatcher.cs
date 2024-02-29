using Infrastructure.States;
using Services.Logging;
using UnityEngine;
using Zenject;

namespace Core.Logic
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
            _loggingService.LogMessage("Level Ran", this);
        }

        public void CompleteLevel()
        {
        }
    }
}