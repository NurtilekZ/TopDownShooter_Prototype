using _current.Core.Pawns.Components;
using _current.Core.Pawns.Enemy;
using _current.StaticData.ScriptableObjects;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;
using IEnemyFactory = _current.Infrastructure.Factories.Interfaces.IEnemyFactory;

namespace _current.Core.Logic.EnemySpawners
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private SpawnerPawnAnimationHandler _animationHandler;
        [SerializeField] private EnemyDeath _enemyDeath;
        [SerializeField] private float _waitTime;
        
        private IEnemyFactory _enemyFactory;
        private EnemyStaticData _enemyStaticData;
        private ReactiveProperty<int> _activeEnemiesCount = new();
        public IReadOnlyReactiveProperty<int> ActiveEnemiesCount => _activeEnemiesCount;

        [Inject]
        private void Construct(IEnemyFactory factory) => 
            _enemyFactory = factory;

        public void Initialize(EnemyStaticData enemyStaticData, int activeEnemiesCount = 5)
        {
            _activeEnemiesCount.Value = activeEnemiesCount;
            _enemyStaticData = enemyStaticData;
            _enemyDeath.OnDeath += StopSpawning;
            Spawn();
        }

        private void StopSpawning(PawnDeath obj)
        {
            _enemyDeath.OnDeath -= StopSpawning;
        }

        private async void Spawn()
        {
            for (int i = 0; i < _activeEnemiesCount.Value; i++)
            {
                var enemy = await _enemyFactory.Create(_enemyStaticData.EnemyTypeId, transform);
                enemy.GetComponent<EnemyDeath>().OnDeath += Died;
                await UniTask.WaitForSeconds(_waitTime);
            }
        }

        private void Died(PawnDeath pawn)
        {
            pawn.OnDeath -= Died;
            if (--_activeEnemiesCount.Value == 0)
            {
                Spawn();
            }
        }
    }
}