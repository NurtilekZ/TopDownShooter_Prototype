using _current.Data;
using _current.Infrastructure.Factories.Interfaces;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace _current.Core.Pawns.LootSystem.LootSpawners
{
    public abstract class LootSpawner<T> : MonoBehaviour
    {
        [SerializeField] protected LootTypeId _lootTypeId;
        [SerializeField] private int _maxValue = 1;
        [SerializeField] private int _minValue = 1;

        protected ILootFactory _lootFactory;

        [Inject]
        private void Construct(ILootFactory factory)
        {
            _lootFactory = factory;
        }

        public void Initialize(int minValue, int maxValue)
        {
            _minValue = minValue;
            _maxValue = maxValue;
        }

        protected abstract void SpawnLoot(T data);

        protected Loot GenerateLoot()
        {
            return new Loot
            {
                lootTypeId = _lootTypeId,
                value = Random.Range(_minValue, _maxValue + 1)
            };
        }
    }
}