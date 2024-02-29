using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Services.Logging;
using StaticData;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine;
using Newtonsoft.Json;

namespace Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string ConfigEnvironmentId = "test"; // development //"prod"; //production

        private const string StagesList = "StagesList";
        private const string ItemsList = "ItemsList";
        private const string EnemiesList = "EnemiesList";
        private const string HeroConfigKey = "Hero";

        private readonly ILoggingService _logger;

        private Dictionary<EnemyType, EnemyStaticData> _enemies;
        private Dictionary<string, StageStaticData> _stages;
        private Dictionary<string, InventoryItemStaticData> _items;
        private HeroStaticData _heroStaticData;

        #region Attribute struct

        private struct UserAttributes
        {
        }
        
        private struct AppAttributes
        {
        }

        #endregion

        public StaticDataService(ILoggingService logger) => 
            _logger = logger;

        public event Action Initialized;
        
        public async void Initialize()
        {
            var connnection = Application.internetReachability;
            if (connnection != NetworkReachability.NotReachable)
                await InitializeRemoteConfigAsync();

            RemoteConfigService.Instance.FetchCompleted += OnRemoteConfigLoaded;
            RemoteConfigService.Instance.SetEnvironmentID(ConfigEnvironmentId);
            
            await RemoteConfigService.Instance.FetchConfigsAsync(new UserAttributes(), new AppAttributes());
        }

        public StageStaticData ForStage(string stageKey) => 
            _stages.GetValueOrDefault(stageKey);

        public List<StageStaticData> GetAllStages => 
            _stages.Values.ToList();

        public InventoryItemStaticData ForInventoryItem(string itemKey) => 
            _items.GetValueOrDefault(itemKey);

        public List<InventoryItemStaticData> GetAllItems => 
            _items.Values.ToList();

        public HeroStaticData ForHero() => 
            throw new NotImplementedException();

        public EnemyStaticData ForEnemy(EnemyType enemyType) => 
            _enemies.GetValueOrDefault(enemyType);

        public void ForWindow() => 
            throw new NotImplementedException();

        private void OnRemoteConfigLoaded(ConfigResponse configResponse)
        {
            LoadStagesData();
            LoadItemsData();
            LoadHeroData();
            LoadEnemiesData();
            
            LogConfigsResponseResult(configResponse);
            
            Initialized?.Invoke();
        }

        /* TODO:
         * Used in case of EconomyLocalService; when Remote SD provided by UGS Economy
         * Mark methods and props [Obsolete] if needed
         */
        private void LoadStagesData()
        {
            _stages = new Dictionary<string, StageStaticData>();
            
            var list = JsonConvert.DeserializeObject<List<string>>(
                RemoteConfigService.Instance.appConfig.GetJson(StagesList));

            foreach (var stageKey in list)
                _stages[stageKey] = JsonConvert.DeserializeObject<StageStaticData>(
                    RemoteConfigService.Instance.appConfig.GetJson(stageKey));
        }

        private void LoadItemsData() =>
            _items = (JsonConvert.DeserializeObject<List<InventoryItemStaticData>>(
                    RemoteConfigService.Instance.appConfig.GetJson(ItemsList)
                ) ?? new List<InventoryItemStaticData>())
                .ToDictionary(it => it.ItemId, it => it);

        private void LoadHeroData() => 
            _heroStaticData = JsonConvert.DeserializeObject<HeroStaticData>(RemoteConfigService.Instance.appConfig.GetJson(HeroConfigKey));

        private void LoadEnemiesData() =>
            _enemies = (JsonConvert.DeserializeObject<List<EnemyStaticData>>(
                    RemoteConfigService.Instance.appConfig.GetJson(EnemiesList)
                ) ?? new List<EnemyStaticData>())
                .ToDictionary(e => e.EnemyType, e => e);

        private void LogConfigsResponseResult(ConfigResponse configResponse)
        {
            var message = configResponse.requestOrigin switch
            {
                ConfigOrigin.Default => "No configs loaded; using default config",
                ConfigOrigin.Cached  => "No configs loaded; using cached config",
                ConfigOrigin.Remote  => "New configs loaded; updating cached config...",
                _                    => throw new ArgumentOutOfRangeException()
            };
            _logger.LogMessage(message);
        }

        private async Task InitializeRemoteConfigAsync()
        {
            await UnityServices.InitializeAsync();
            if (!AuthenticationService.Instance.IsSignedIn)
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }
}