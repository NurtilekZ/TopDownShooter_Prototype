using _current.Infrastructure.AssetManagement;
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
        private IUIFactory _uiFactory;

        public GameLoopState(
            GameStateMachine stateMachine,
            IHeroFactory heroFactory,
            IWeaponFactory weaponFactory,
            IEnemyFactory enemyFactory,
            ILootFactory lootFactory,
            IUIFactory uiFactory,
            ILevelProgressService levelProgressService)
        {
            _uiFactory = uiFactory;
            _stateMachine = stateMachine;
            _heroFactory = heroFactory;
            _lootFactory = lootFactory;
            _enemyFactory = enemyFactory;
            _levelProgressService = levelProgressService;
            _weaponFactory = weaponFactory;
        }

        public async void Enter()
        {
            await _uiFactory.WarmUpForState(new []
            {
                AssetsPath.GameHUDScreen,
                AssetsPath.GameOverScreen,
            });
        }

        public void Exit()
        {
            _uiFactory.CleanUpForState();
            _heroFactory.CleanUp();
            _weaponFactory.CleanUp();
            _enemyFactory.CleanUp();
            _lootFactory.CleanUp();
        }
    }
}