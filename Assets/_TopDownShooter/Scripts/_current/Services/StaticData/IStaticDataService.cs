using System;
using System.Collections.Generic;
using _current.Core.Pawns.Enemy;
using _current.Core.Pawns.LootSystem;
using _current.Core.Systems.WeaponSystem;
using _current.Core.Systems.WeaponSystem.Data;
using _current.StaticData;
using _current.StaticData.ScriptableObjects;

namespace _current.Services.StaticData
{
    public interface IStaticDataService : IInitializableAsync, IService
    {
        event Action Initialized;

        LevelStaticData ForLevel(string stageKey);
        List<LevelStaticData> GetAllLevels { get; }

        ItemStaticData ForInventoryItem(LootTypeId itemKey);
        List<ItemStaticData> GetAllItems { get; }

        MissionPointSpawnerStaticData ForMissionPoint(MissionPointType missionPointType);
        HeroStaticData ForHero();
        EnemyStaticData ForEnemy(EnemyTypeId enemyType);
        WeaponStaticData ForWeapon(WeaponTypeId weaponTypeId);
    }
}