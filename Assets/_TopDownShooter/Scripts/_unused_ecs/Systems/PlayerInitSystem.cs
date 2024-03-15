using Data;
using Leopotam.EcsLite;
using UnityEngine;

namespace Client.Systems
{
    public class PlayerInitSystem : IEcsInitSystem
    {
        private EcsWorld _ecsWorld;
        private Data.StaticData _staticData;
        private SceneData _sceneData;
        
        public void Init(IEcsSystems systems)
        {
            var playerEntity = _ecsWorld.NewEntity();

            ref var player = ref playerEntity;
            ref var inputData = ref playerEntity;

            GameObject playerGo = Object.Instantiate(_staticData.playerPrefab, _sceneData.playerSpawnPoint.position, Quaternion.identity);
        }
    }
}