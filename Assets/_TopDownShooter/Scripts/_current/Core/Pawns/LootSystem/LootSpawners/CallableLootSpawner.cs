using System.Collections;
using UnityEngine;

namespace _current.Core.Pawns.LootSystem.LootSpawners
{
    public class CallableLootSpawner : LootSpawner<Vector3>
    {
        [SerializeField] private float _delayTime;
        
        public void Initialize(Vector3 at)
        {
            StartCoroutine(SpawnLootDelay(at));
        }

        private IEnumerator SpawnLootDelay(Vector3 at)
        {
            yield return new WaitForSeconds(_delayTime);
            SpawnLoot(at);
        }

        protected override async void SpawnLoot(Vector3 at)
        {
            var lootItem = GenerateLoot();
            LootView loot = await _lootFactory.Create(_lootTypeId.ToString());
            loot.transform.position = at;
            loot.Initialize(lootItem);
        }
    }
}