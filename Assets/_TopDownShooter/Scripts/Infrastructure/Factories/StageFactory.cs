using System.Threading.Tasks;
using Core.Logic;
using Infrastructure.AssetsManagement;
using Infrastructure.Factories.Interfaces;
using Services.StaticData;
using StaticData;
using UnityEngine;
using Zenject;

namespace Infrastructure.Factories
{
    public class StageFactory : IStageFactory
    {
        private const string EnemySpawnerPrefabId = "EnemySpawnerPrefab";
        private const string MapPrefabId = "BoardPrefabId";
        private readonly IAssetProvider _assetProvider;

        private readonly DiContainer _container;
        private readonly IStaticDataService _staticDataService;

        public StageFactory(DiContainer container, IAssetProvider assetProvider, IStaticDataService staticDataService)
        {
            _container = container;
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
        }

        public async Task WarmUp()
        {
            await _assetProvider.Load<GameObject>(EnemySpawnerPrefabId);
        }

        public void CleanUp()
        {
            _assetProvider.Release(EnemySpawnerPrefabId);
            _assetProvider.Release(MapPrefabId);
        }

        public async Task<EnemySpawner> CreateEnemySpawner(EnemyType enemyType, Vector3 at)
        {
            var config = _staticDataService.ForEnemy(enemyType);
            var prefab = await _assetProvider.Load<GameObject>(EnemySpawnerPrefabId);
            var spawner = Object.Instantiate(prefab, at, Quaternion.identity).GetComponent<EnemySpawner>();

            _container.Inject(spawner);

            spawner.Initialize(config);

            return spawner;
        }
    }
}