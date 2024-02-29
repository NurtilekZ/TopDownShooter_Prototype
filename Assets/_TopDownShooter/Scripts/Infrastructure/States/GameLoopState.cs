using Infrastructure.Factories.Interfaces;
using Infrastructure.States.Interfaces;
using Services.LevelProgress;

namespace Infrastructure.States
{
    public class GameLoopState : IState
    {
        private readonly IEnemyFactory _enemyFactory;
        private readonly IHeroFactory _heroFactory;
        private readonly ILevelProgressService _levelProgressService;
        private GameStateMachine _stateMachine;

        public GameLoopState(
            GameStateMachine gameStateMachine,
            IHeroFactory heroFactory,
            IEnemyFactory enemyFactory,
            ILevelProgressService levelProgressService)
        {
            _stateMachine = gameStateMachine;
            _heroFactory = heroFactory;
            _enemyFactory = enemyFactory;
            _levelProgressService = levelProgressService;
        }

        public void Enter()
        {
            _levelProgressService.LevelProgressWatcher.RunLevel();
        }

        public void Exit()
        {
            _enemyFactory.CleanUp();
            _heroFactory.CleanUp();
        }
    }
}