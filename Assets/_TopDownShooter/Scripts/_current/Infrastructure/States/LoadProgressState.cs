using System.Collections.Generic;
using System.Linq;
using _current.Core.Pawns.LootSystem;
using _current.Core.Systems.WeaponSystem.Data;
using _current.Data;
using _current.Data.Data;
using _current.Services.Economy;
using _current.Services.PersistentData;
using _current.Services.SaveLoad;
using _current.Services.StaticData;
using _current.Services.UI;
using _current.UI.Screens.Loading;
using UniRx;

namespace _current.Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly IEconomyService _economyService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IPersistentDataService _persistentDataService;
        private readonly IUIService _uiService;
        private IStaticDataService _staticDataService;


        public LoadProgressState(
            GameStateMachine stateMachine,
            IEconomyService economyService,
            ISaveLoadService saveLoadService,
            IUIService uiService, 
            IStaticDataService staticDataService,
            IPersistentDataService persistentDataService)
        {
            _staticDataService = staticDataService;
            _uiService = uiService;
            _stateMachine = stateMachine;
            _economyService = economyService;
            _saveLoadService = saveLoadService;
            _persistentDataService = persistentDataService;
        }

        public void Enter()
        {
            LoadProgressOrInitNew();
            InitEconomyByProgress();
            
            _stateMachine.Enter<MainMenuState>();
        }

        public void Exit()
        {
            _uiService.Close<LoadingScreenViewModel>();
        }

        private async void LoadProgressOrInitNew()
        {
            _persistentDataService.Settings = await _saveLoadService.LoadSettings() ?? NewSettings();
            _persistentDataService.Progress = await _saveLoadService.LoadProgress() ?? NewProgress();
            _persistentDataService.Economy = await _saveLoadService.LoadEconomy() ?? NewEconomy();
        }

        private void InitEconomyByProgress()
        {
            _economyService.InitializeCurrency(_persistentDataService.Economy.PlayerCurrency);
            _economyService.PlayerCurrency.ObserveReplace().Subscribe(x =>
            { 
                _persistentDataService.Economy.PlayerCurrency[x.Key] = x.NewValue; 
                _saveLoadService.SaveEconomy();
            });
        }

        private PlayerEconomyData NewEconomy() =>
            new()
            {
                PlayerCurrency = new Dictionary<CurrencyTypeId, int>()
                {
                    { CurrencyTypeId.Ordinary, 0},
                    { CurrencyTypeId.Rare, 0},
                    { CurrencyTypeId.SuperRare, 0},
                },
                InventoryItems = new Dictionary<LootTypeId, int>()
            };

        private PlayerSettingsData NewSettings() =>
            new()
            {
                MusicVolume = 100,
                SfxVolume = 100,
                DebugEnabled = true,
            };

        private PlayerProgressData NewProgress()
        {
            return new PlayerProgressData
            {
                AvailableWeapons = new WeaponData[] { new(PrimaryWeaponTypeId.AssaultRifle, 30, 120) },
                LevelsProgress = new LevelData[_staticDataService.GetAllLevels.Count],
            };
        }
    }
}