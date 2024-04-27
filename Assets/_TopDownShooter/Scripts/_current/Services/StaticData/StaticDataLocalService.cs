using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using _current.Core.Pawns.Enemy;
using _current.Core.Pawns.LootSystem;
using _current.Core.Systems.WeaponSystem;
using _current.Core.Systems.WeaponSystem.Data;
using _current.Infrastructure.AssetManagement;
using _current.Services.Logging;
using _current.StaticData;
using _current.StaticData.ScriptableObjects;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _current.Services.StaticData
{
    public class StaticDataLocalService : IStaticDataService
    {
        private const string LevelsDataPath = "StaticData/LevelsData";
        private const string ItemsDataPath = "StaticData/ItemsData";
        private const string EnemiesDataPath = "StaticData/EnemiesData";
        private const string WeaponsDataPath = "StaticData/WeaponsData";
        private const string HeroDataPath = "StaticData/HeroData/HeroData";
        
        private readonly ILoggingService _loggingService;
        private readonly IAssetProvider _assetProvider;
        
        private HeroStaticData _heroStaticData;
        private Dictionary<string, LevelStaticData> _levels;
        private Dictionary<PrimaryWeaponTypeId, WeaponStaticData> _weapons;
        private Dictionary<LootTypeId, ItemStaticData> _items;
        private Dictionary<EnemyTypeId, EnemyStaticData> _enemies;
        private Dictionary<MissionPointType, ObjectiveSpawnerLevelData> _interestPoints;

        public event Action Initialized;

        public List<LevelStaticData> GetAllLevels => _levels.Values.ToList();
        public List<ItemStaticData> GetAllItems => _items.Values.ToList();

        public StaticDataLocalService(ILoggingService loggingService, IAssetProvider assetProvider)
        {
            _loggingService = loggingService;
            _assetProvider = assetProvider;
        }

        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            await LoadHero();
            LoadItems();
            LoadEnemies();
            LoadLevels();
            LoadWeapons();
            
            _loggingService.LogMessage("Static Data Loaded", this, LoggingTag.Infrastructure);
        }

        public LevelStaticData ForLevel(string stageKey) => 
            _levels.GetValueOrDefault(stageKey);

        public ItemStaticData ForInventoryItem(LootTypeId itemKey) => 
            _items.GetValueOrDefault(itemKey);

        public ObjectiveSpawnerLevelData ForMissionPoint(MissionPointType missionPointType) => 
            _interestPoints.GetValueOrDefault(missionPointType);

        public HeroStaticData ForHero() => 
            _heroStaticData;

        public EnemyStaticData ForEnemy(EnemyTypeId enemyType) => 
            _enemies.GetValueOrDefault(enemyType);

        public WeaponStaticData ForWeapon(PrimaryWeaponTypeId primaryWeaponTypeId) => 
            _weapons.GetValueOrDefault(primaryWeaponTypeId);

        private async Task LoadHero()
        {
            _heroStaticData = (HeroStaticData) await Resources.LoadAsync<HeroStaticData>(HeroDataPath);
        }

        private void LoadLevels() =>
            _levels = Resources.LoadAll<LevelStaticData>(LevelsDataPath)
                .ToDictionary(x => x.LevelKey, x => x);

        private void LoadEnemies() =>
            _enemies = Resources.LoadAll<EnemyStaticData>(EnemiesDataPath)
                .ToDictionary(x => x.EnemyTypeId, x => x);

        private void LoadWeapons() =>
            _weapons = Resources.LoadAll<WeaponStaticData>(WeaponsDataPath)
                .ToDictionary(x => x.primaryWeaponTypeId, x => x);

        private void LoadItems() =>
            _items = Resources.LoadAll<ItemStaticData>(ItemsDataPath)
                .ToDictionary(x => x.ItemId, x => x);

        public void Dispose()
        {
            _assetProvider?.Dispose();
        }
    }
}