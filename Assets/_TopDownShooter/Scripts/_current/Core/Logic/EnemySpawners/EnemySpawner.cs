using _current.Core.Pawns.Components;
using _current.Core.Pawns.Enemy;
using _current.StaticData.ScriptableObjects;
using UniRx;
using UnityEngine;
using Zenject;
using IEnemyFactory = _current.Infrastructure.Factories.Interfaces.IEnemyFactory;

namespace _current.Core.Logic.EnemySpawners
{
    public class EnemySpawner : MonoBehaviour
    {
        private IEnemyFactory _enemyFactory;
        private EnemyStaticData _enemyStaticData;
        private ReactiveProperty<int> _remainedEnemiesCount = new();

        public IReadOnlyReactiveProperty<int> RemainedEnemiesCount => _remainedEnemiesCount;

        [Inject]
        private void Construct(IEnemyFactory factory) => 
            _enemyFactory = factory;

        public void Initialize(EnemyStaticData enemyStaticData, int enemiesCount = 1)
        {
            _remainedEnemiesCount.Value = enemiesCount;
            _enemyStaticData = enemyStaticData;
            Spawn();
        }

        private async void Spawn()
        {
            var enemy = await _enemyFactory.Create(_enemyStaticData.EnemyTypeId, transform);
            enemy.GetComponent<EnemyDeath>().OnDeath += Died;
        }

        private void Died(PawnDeath pawn)
        {
            pawn.OnDeath -= Died;
            if (--_remainedEnemiesCount.Value > 0)
            {
                Spawn();
            }
        }
    }
}