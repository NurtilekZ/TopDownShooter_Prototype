using System.Threading.Tasks;
using _current.Core.Logic.EnemySpawners;
using _current.Core.Logic.MissionPointSpawners;
using _current.Infrastructure.AssetManagement;
using _current.Infrastructure.Factories.Interfaces;
using _current.Services.StaticData;
using _current.StaticData;
using UnityEngine;
using Zenject;

namespace _current.Infrastructure.Factories
{
    public class LevelFactory : ILevelFactory
    {
        private readonly DiContainer _container;
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticDataService;

        public LevelFactory(DiContainer container,IAssetProvider assetProvider, IStaticDataService staticDataService)
        {
            _container = container;
            _staticDataService = staticDataService;
            _assetProvider = assetProvider;
        }


        public async Task WarmUp()
        {
            await _assetProvider.Load<GameObject>(AssetsPath.SpawnPoint);
        }


        public void CleanUp()
        {
            _assetProvider.Release(AssetsPath.SpawnPoint);
        }

        public async Task<EnemySpawner> CreateSpawner(EnemySpawnerLevelData spawnerData)
        {
            var config = _staticDataService.ForEnemy(spawnerData.EnemyType);
            var prefab = await _assetProvider.Load<GameObject>(config.EnemyTypeId.ToString());
            var spawner = Object.Instantiate(prefab, spawnerData.Position, Quaternion.identity).GetComponent<EnemySpawner>();
            
            _container.Inject(spawner);
            
            spawner.Initialize(config);
            
            return spawner;
        }

        public async Task<IMissionPointBase> CreateMissionPoint(ObjectiveSpawnerLevelData objectiveSpawnerLevelData)
        {
            var config = _staticDataService.ForMissionPoint(objectiveSpawnerLevelData.MissionPointType);
            var prefab = await _assetProvider.Load<GameObject>(config.MissionPointType.ToString());
            var interestPoint = Object.Instantiate(prefab, objectiveSpawnerLevelData.Position, Quaternion.identity).GetComponent<IMissionPointBase>();
            
            _container.Inject(interestPoint);
            
            interestPoint.Initialize(config);
            
            return interestPoint;
        }
    }
}