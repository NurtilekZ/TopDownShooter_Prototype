using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using _current.Core.Pawns.Enemy;
using _current.Core.Pawns.LootSystem;
using _current.Core.Systems.WeaponSystem;
using _current.Core.Systems.WeaponSystem.Data;
using _current.Services.Logging;
using _current.StaticData;
using _current.StaticData.ScriptableObjects;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine;
using static Newtonsoft.Json.JsonConvert;

namespace _current.Services.StaticData
{
    public class StaticDataRemoteService : IStaticDataService
    {
        private const string ConfigEnvironmentId = "dev"; // development //"prod"; //production

        private const string StagesList = "StagesList";
        private const string ItemsList = "ItemsList";
        private const string EnemiesList = "EnemiesList";
        private const string WeaponsList = "WeaponsList";
        private const string HeroConfigKey = "Hero";

        private readonly ILoggingService _loggingService;

        private Dictionary<EnemyTypeId, EnemyStaticData> _enemies;
        private Dictionary<string, LevelStaticData> _stages;
        private Dictionary<LootTypeId, ItemStaticData> _items;
        private Dictionary<WeaponTypeId, WeaponStaticData> _weapons;
        private HeroStaticData _heroStaticData;
        private TaskCompletionSource<ConfigResponse> _fetchCompletionSource;
        private Dictionary<MissionPointType, MissionPointSpawnerStaticData> _interestPoints;

        public event Action Initialized;
        
        #region Attribute struct

        private struct UserAttributes
        {
        }
        
        private struct AppAttributes
        {
        }

        #endregion

        public StaticDataRemoteService(ILoggingService loggingService) => 
            _loggingService = loggingService;


        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            _fetchCompletionSource = new TaskCompletionSource<ConfigResponse>();
            
            var connection = Application.internetReachability;
            if (connection != NetworkReachability.NotReachable)
                await InitializeRemoteConfigAsync();

            RemoteConfigService.Instance.FetchCompleted += OnRemoteConfigLoaded;
            RemoteConfigService.Instance.SetEnvironmentID(ConfigEnvironmentId);
            
            await RemoteConfigService.Instance.FetchConfigsAsync(new UserAttributes(), new AppAttributes());
            await _fetchCompletionSource.Task;
            Initialized?.Invoke();
        }

        private static async Task InitializeRemoteConfigAsync()
        {
            await UnityServices.InitializeAsync();
            if (!AuthenticationService.Instance.IsSignedIn)
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        public LevelStaticData ForLevel(string stageKey) => 
            _stages.GetValueOrDefault(stageKey);

        public List<LevelStaticData> GetAllLevels => 
            _stages.Values.ToList();

        public ItemStaticData ForInventoryItem(LootTypeId itemKey) => 
            _items.GetValueOrDefault(itemKey);

        public List<ItemStaticData> GetAllItems => 
            _items.Values.ToList();

        public HeroStaticData ForHero() => 
            _heroStaticData;

        public EnemyStaticData ForEnemy(EnemyTypeId enemyType) => 
            _enemies.GetValueOrDefault(enemyType);

        public MissionPointSpawnerStaticData ForMissionPoint(MissionPointType missionPointType) => 
            _interestPoints.GetValueOrDefault(missionPointType);

        public WeaponStaticData ForWeapon(WeaponTypeId weaponTypeId) => 
            _weapons.GetValueOrDefault(weaponTypeId);

        private void OnRemoteConfigLoaded(ConfigResponse configResponse)
        {
            // LoadStagesData();
            // LoadItemsData();
            // LoadHeroData();
            // LoadEnemiesData();
            
            LogConfigsResponseResult(configResponse);

            RemoteConfigService.Instance.FetchCompleted -= OnRemoteConfigLoaded;
            _fetchCompletionSource.SetResult(configResponse);
        }

        /* TODO:
         * Used in case of EconomyLocalService; when Remote SD provided by UGS Economy
         * Mark methods and props [Obsolete] if needed
         */

        private void LoadStagesData()
        {
            _stages = new Dictionary<string, LevelStaticData>();
            
            var list = DeserializeObject<List<string>>(
                RemoteConfigService.Instance.appConfig.GetJson(StagesList));

            foreach (var stageKey in list)
                _stages[stageKey] = DeserializeObject<LevelStaticData>(
                    RemoteConfigService.Instance.appConfig.GetJson(stageKey));
        }

        private void LoadItemsData() =>
            _items = (DeserializeObject<List<ItemStaticData>>(
                    RemoteConfigService.Instance.appConfig.GetJson(ItemsList)
                ) ?? new List<ItemStaticData>())
                .ToDictionary(it => it.ItemId, it => it);

        private void LoadHeroData() => 
            _heroStaticData = DeserializeObject<HeroStaticData>(RemoteConfigService.Instance.appConfig.GetJson(HeroConfigKey));

        private void LoadEnemiesData() =>
            _enemies = (DeserializeObject<List<EnemyStaticData>>(
                    RemoteConfigService.Instance.appConfig.GetJson(EnemiesList)
                ) ?? new List<EnemyStaticData>())
                .ToDictionary(e => e.EnemyTypeId, e => e);

        private void LoadWeaponsData() =>
            _weapons = (DeserializeObject<List<WeaponStaticData>>(
                    RemoteConfigService.Instance.appConfig.GetJson(WeaponsList)
                ) ?? new List<WeaponStaticData>())
                .ToDictionary(e => e.WeaponTypeId, e => e);

        private void LogConfigsResponseResult(ConfigResponse configResponse)
        {
            var message = configResponse.requestOrigin switch
            {
                ConfigOrigin.Default => "No configs loaded; using default config",
                ConfigOrigin.Cached  => "No configs loaded; using cached config",
                ConfigOrigin.Remote  => "New configs loaded; updating cached config...",
                _                    => throw new ArgumentOutOfRangeException()
            };
            _loggingService.LogMessage(message);
        }

        public void Dispose()
        {
            // TODO release managed resources here
        }
    }
}