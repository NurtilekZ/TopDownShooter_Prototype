using _current.Infrastructure.Factories.Interfaces;
using _current.Services.LevelProgress;

namespace _current.Infrastructure.States
{
    public class GameLoopState : IState
    {
        private readonly IHeroFactory _heroFactory;
        private readonly IWeaponFactory _weaponFactory;
        private readonly IEnemyFactory _enemyFactory;
        private readonly ILootFactory _lootFactory;
        private readonly ILevelProgressService _levelProgressService;
        private readonly GameStateMachine _stateMachine;

        public GameLoopState(
            GameStateMachine stateMachine,
            IHeroFactory heroFactory,
            IWeaponFactory weaponFactory,
            IEnemyFactory enemyFactory,
            ILootFactory lootFactory,
            ILevelProgressService levelProgressService)
        {
            _stateMachine = stateMachine;
            _heroFactory = heroFactory;
            _lootFactory = lootFactory;
            _enemyFactory = enemyFactory;
            _levelProgressService = levelProgressService;
            _weaponFactory = weaponFactory;
        }

        public void Enter()
        {
            
        }

        public void Exit()
        {
            _heroFactory.CleanUp();
            _weaponFactory.CleanUp();
            _enemyFactory.CleanUp();
            _lootFactory.CleanUp();
        }
    }
}