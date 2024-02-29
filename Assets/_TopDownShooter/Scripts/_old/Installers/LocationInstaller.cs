using System.Collections.Generic;
using _old.Player;
using UnityEngine;
using Zenject;

namespace _old.Installers
{
    public class LocationInstaller : MonoInstaller
    {
        [SerializeField] private PlayerPawn _playerPawnPrefab;
        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private Transform _playerTargetPoint;

        [SerializeField] private List<Transform> _enemiesSpawnPoint = new();

        public override void InstallBindings()
        {
            Application.targetFrameRate = 300;

            var playerPawn = Container.InstantiatePrefabForComponent<PlayerPawn>(
                _playerPawnPrefab,
                _playerSpawnPoint.position,
                _playerSpawnPoint.rotation,
                null);

            Container.BindInstance(playerPawn).AsSingle();
        }
    }
}