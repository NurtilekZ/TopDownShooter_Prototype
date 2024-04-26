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

        public override void Initialize(MissionPointSpawnerStaticData staticData)
        {
            base.Initialize(staticData);
            _enemyDeath.OnDeath += OnDeath;
        }

        private void OnDeath(PawnDeath obj)
        {
            _isComplete.Value = true;
        }
    }
}