using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Enemy;
using Core.UI;
using Infrastructure.AssetsManagement;
using Infrastructure.Factories.Interfaces;
using Services.StaticData;
using StaticData;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Infrastructure.Factories
{
    public class EnemyFactory : IEnemyFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly DiContainer _container;
        private readonly IHeroFactory _heroFactory;
        private readonly IStaticDataService _staticDataService;

        public EnemyFactory(DiContainer container, IAssetProvider assetProvider, IStaticDataService staticDataService,
            IHeroFactory heroFactory)
        {
            _container = container;
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
            _heroFactory = heroFactory;
        }

        public async Task WarmUp()
        {
            foreach (var enemyType in (EnemyType[])Enum.GetValues(typeof(EnemyType)))
                await _assetProvider.Load<GameObject>(enemyType.ToString());
        }

        public void CleanUp()
        {
            foreach (var enemyType in (EnemyType[])Enum.GetValues(typeof(EnemyType)))
                _assetProvider.Release(enemyType.ToString());
        }

        public async Task<GameObject> Create(EnemyType enemyType, Transform parent)
        {
            var config = _staticDataService.ForEnemy(enemyType);

            var prefab = await _assetProvider.Load<GameObject>(config.EnemyType.ToString());
            var enemy = Object.Instantiate(prefab, parent.position, parent.rotation, parent);

            _container.InjectGameObject(enemy);

            var health = enemy.GetComponent<EnemyHealth>();
            health.MaxHP = config.Health;
            health.CurrentHP.Value = health.MaxHP;

            var actorUi = enemy.GetComponentInChildren<ActorUI>();
            actorUi.Initialize(health);

            enemy
                .GetComponents<EnemyFollowBase>()
                .ToList()
                .ForEach(fc => fc
                    .Initialize(_heroFactory.Hero));

            var attack = enemy.GetComponent<EnemyAttack>();
            attack.Initialize(_heroFactory.Hero);
            attack.AttackType = config.AttackType;
            attack.AttackDamage = config.AttackDamage;
            attack.Cooldown = config.Cooldown;

            return enemy;
        }
    }
}