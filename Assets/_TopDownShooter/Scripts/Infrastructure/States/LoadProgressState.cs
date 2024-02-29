using System.Collections.Generic;
using Data;
using Infrastructure.States.Interfaces;
using Services.Economy;
using Services.PersistentData;
using Services.SaveLoad;
using UniRx;

namespace Infrastructure.States
{
    public class LoadProgressState : IExitableState, IState
    {
        private readonly IEconomyService _economyService;
        private readonly IPersistentDataService _progressService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly GameStateMachine _stateMachine;

        public LoadProgressState(GameStateMachine stateMachine, IPersistentDataService progressService,
            ISaveLoadService saveLoadService, IEconomyService economyService)
        {
            _stateMachine = stateMachine;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
            _economyService = economyService;
        }

        public void Exit()
        {
        }

        public void Enter()
        {
            LoadProgressOnInitNew();
            InitEconomyByProgress();

            _stateMachine.Enter<LoadMetaState>();
        }

        private async void LoadProgressOnInitNew()
        {
            _progressService.Settings =
                await _saveLoadService.LoadSettings()
                ?? NewSettings();

            _progressService.Progress =
                await _saveLoadService.LoadProgress()
                ?? NewProgress();

            _progressService.Economy =
                await _saveLoadService.LoadEconomy()
                ?? NewEconomy();
        }

        private void InitEconomyByProgress()
        {
            _economyService.PlayerCurrency.Value = _progressService.Economy.PlayerCurrency;
            _economyService.PlayerCurrency
                .Subscribe(_ =>
                {
                    _progressService.Economy.PlayerCurrency = _economyService.PlayerCurrency.Value;
                    _saveLoadService.SaveEconomy();
                });
        }

        #region Creation economy, progress and setting stubs

        private PlayerEconomyData NewEconomy()
        {
            return new PlayerEconomyData
            {
                PlayerCurrency = 100,
                InventoryItems = new Dictionary<string, int>()
            };
        }

        private PlayerProgressData NewProgress()
        {
            return new PlayerProgressData
            {
                CompletedStages = new HashSet<string>()
            };
        }

        private PlayerSettingsData NewSettings()
        {
            return new PlayerSettingsData
            {
                MusicVolume = 100,
                SfxVolume = 100,
                DebugEnabled = false
            };
        }

        #endregion
    }
}