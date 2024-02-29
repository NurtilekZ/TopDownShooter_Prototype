using Core.Enemy;
using Infrastructure.Factories.Interfaces;
using StaticData;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core.Logic
{
    public class EnemySpawner : MonoBehaviour
    {
        private EnemyStaticData _enemyStaticData;
        private IEnemyFactory _enemyFactory;

        public IntReactiveProperty enemiesRemainder = new();

        [Inject]
        private void Construct(IEnemyFactory enemyFactory) =>
            _enemyFactory = enemyFactory;

        public void Initialize(EnemyStaticData enemyStaticData, int enemiesCount = 1)
        {
            enemiesRemainder.Value = enemiesCount;
            _enemyStaticData = enemyStaticData;
            Spawn();
        }

        private async void Spawn()
        {
            var enemy = await _enemyFactory.Create(_enemyStaticData.EnemyType, transform);
            enemy.GetComponent<EnemyDeath>().EnemyDied += Slain;
        }

        private void Slain()
        {
            if (--enemiesRemainder.Value > 0)
            {
                Spawn();
            }
        }
    }
}