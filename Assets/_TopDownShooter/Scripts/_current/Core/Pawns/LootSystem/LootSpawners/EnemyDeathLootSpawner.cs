using _current.Core.Pawns.Components;
using _current.Core.Pawns.Enemy;
using UnityEngine;

namespace _current.Core.Pawns.LootSystem.LootSpawners
{
    public class EnemyDeathLootSpawner : LootSpawner<PawnDeath>
    {
        [SerializeField] private EnemyDeath _enemyDeath;

        private void Start()
        {
            _enemyDeath.OnDeath += SpawnLoot;
        }

        protected override async void SpawnLoot(PawnDeath pawn)
        {
            _enemyDeath.OnDeath -= SpawnLoot;
            var lootItem = GenerateLoot();
            LootView loot = await _lootFactory.Create(_lootTypeId.ToString());
            loot.transform.position = pawn.transform.position;
            loot.Initialize(lootItem);
        }
    }
}