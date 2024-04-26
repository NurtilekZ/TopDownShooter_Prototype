using System;
using System.Linq;
using System.Threading.Tasks;
using _current.Core.Pawns.Enemy;
using _current.Core.Pawns.LootSystem.LootSpawners;
using _current.Core.Systems.DamageSystem;
using _current.Infrastructure.AssetManagement;
using _current.Infrastructure.Factories.Interfaces;
using _current.Services.StaticData;
using _current.UI.Core;
using _current.UI.Overlays;
using _current.UI.Overlays.HealthBar;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace _current.Infrastructure.Factories
{
    public class EnemyFactory : IEnemyFactory
    {
        private DiContainer _container;
        private IAssetProvider _assetProvider;
        private IHeroFactory _heroFactory;
        private IUIFactory _uiFactory;
        private IStaticDataService _staticDataService;

        public EnemyFactory(DiContainer container, IAssetProvider assetProvider, IHeroFactory heroFactory, IStaticDataService staticDataService, IUIFactory uiFactory)
        {
            _container = container;
            _assetProvider = assetProvider;
            _heroFactory = heroFactory;
            _staticDataService = staticDataService;
            _uiFactory = uiFactory;
        }

        public async Task WarmUp()
        {
            foreach (var enemyTypeId in (EnemyTypeId[])Enum.GetValues(typeof(EnemyTypeId)))
            {
                await _assetProvider.Load<GameObject>(enemyTypeId.ToString());
            }
        }

        public void CleanUp()
        {
            foreach (var enemyTypeId in (EnemyTypeId[])Enum.GetValues(typeof(EnemyTypeId)))
            {
                _assetProvider.Release(enemyTypeId.ToString());
            }
        }

        public async Task<GameObject> Create(EnemyTypeId typeId, Transform parent)
        {
            var enemyData = _staticDataService.ForEnemy(typeId);
            var prefab = await _assetProvider.Load<GameObject>(enemyData.PrefabReference.AssetGUID);
            var enemy = Object.Instantiate(prefab, parent.position, parent.rotation, parent);
            
            _container.InjectGameObject(enemy);
            
            var enemyHealth = enemy.GetComponent<IHealth>(); 
            enemyHealth.Initialize(enemyData.Hp);
            enemy.GetComponent<AggroZone>().Initialize(enemyData.AggroRadius);
            enemy.GetComponent<CheckAttack>().Initialize(enemyData.AttackRadius);

            if (enemy.TryGetComponent<OverlayUIPoint>(out var overlayPawnUI))
            {
                await _uiFactory.GetOrCreateView(new HealthOverlayViewModel(enemyHealth, overlayPawnUI.PointTransform));
            }
            
            var attack = enemy.GetComponent<MeleeAttack>();
            attack.Initialize(enemyData);

            if (enemy.TryGetComponent<EnemyDeathLootSpawner>(out var lootSpawner)) 
                lootSpawner.Initialize(enemyData.MinLoot, enemyData.MaxLoot);

            return enemy;
        }
    }

    public class OverlayUIPoint : MonoBehaviour
    {
        [SerializeField] private Transform _pointTransform;
        public Transform PointTransform => _pointTransform;
    }
}