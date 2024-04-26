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
        private const string EnemySpawnerPrefabId = "EnemySpawner";
        
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
            await _assetProvider.Load<GameObject>(EnemySpawnerPrefabId);
        }


        public void CleanUp()
        {
            _assetProvider.Release(EnemySpawnerPrefabId);
        }

        public async Task<EnemySpawner> CreateSpawner(EnemySpawnerStaticData spawnerData)
        {
            var config = _staticDataService.ForEnemy(spawnerData.EnemyType);
            var prefab = await _assetProvider.Load<GameObject>(EnemySpawnerPrefabId);
            var spawner = Object.Instantiate(prefab, spawnerData.Position, Quaternion.identity).GetComponent<EnemySpawner>();
            
            _container.Inject(spawner);
            
            spawner.Initialize(config);
            
            return spawner;
        }

        public async Task<IMissionPointBase> CreateMissionPoint(MissionPointSpawnerStaticData missionPointSpawnerStaticData)
        {
            var config = _staticDataService.ForMissionPoint(missionPointSpawnerStaticData.MissionPointType);
            var prefab = await _assetProvider.Load<GameObject>(EnemySpawnerPrefabId);
            var interestPoint = Object.Instantiate(prefab, missionPointSpawnerStaticData.Position, Quaternion.identity).GetComponent<IMissionPointBase>();
            
            _container.Inject(interestPoint);
            
            interestPoint.Initialize(config);
            
            return interestPoint;
        }
    }
}