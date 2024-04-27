using _current.Core.Pawns.Components;
using _current.Core.Pawns.Enemy;
using _current.StaticData;
using UnityEngine;

namespace _current.Core.Logic.MissionPointSpawners
{
    [RequireComponent(typeof(EnemyDeath))]
    public class DestroyMissionPointBase : MissionPointBase
    {
        [SerializeField] private EnemyDeath _enemyDeath;

        public override MissionPointType MissionType => MissionPointType.Destroy;

        public override void Initialize(ObjectiveSpawnerLevelData levelData)
        {
            base.Initialize(levelData);
            _enemyDeath.OnDeath += OnDeath;
        }

        private void OnDeath(PawnDeath obj)
        {
            _isComplete.Value = true;
        }
    }
}