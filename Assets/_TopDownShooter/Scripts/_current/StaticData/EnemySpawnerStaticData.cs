using System;
using _current.Core.Pawns.Enemy;
using Newtonsoft.Json;
using UnityEngine;

namespace _current.StaticData
{
    [Serializable]
    public class EnemySpawnerStaticData
    {
        public string UnitId { get; }
        public EnemyTypeId EnemyType { get; }

        [JsonConverter(typeof(Vector3))]
        public Vector3 Position { get; }

        public EnemySpawnerStaticData(string unitId, EnemyTypeId argEnemyTypeId, Vector3 transformPosition)
        {
            UnitId = unitId;
            EnemyType = argEnemyTypeId;
            Position = transformPosition;
        }
    }
}