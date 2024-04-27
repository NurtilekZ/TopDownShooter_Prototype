using _current.Core.Pawns.Enemy;
using UnityEngine;

namespace _current.StaticData
{
    public class EnemySpawnerLevelData
    {
        public EnemyTypeId EnemyType { get; }
        public Vector3 Position { get; }

        public EnemySpawnerLevelData(EnemyTypeId enemyTypeId, Vector3 position)
        {
            EnemyType = enemyTypeId;
            Position = position;
        }
    }
}