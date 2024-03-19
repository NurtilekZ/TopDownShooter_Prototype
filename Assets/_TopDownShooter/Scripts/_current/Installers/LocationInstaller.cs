using System.Collections.Generic;
using _current.Components;
using _current.Player;
using UnityEngine;
using Zenject;

namespace _current.Installers
{
    public class LocationInstaller : MonoInstaller
    {
        [SerializeField] private PlayerPawn _playerPawnPrefab;
        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private TargetPawn _playerTargetPoint;

        [SerializeField] private List<Transform> _enemiesSpawnPoint = new();

        public override void InstallBindings()
        {
            Application.targetFrameRate = 300;
            
            Container.Bind<TargetPawn>().FromInstance(_playerTargetPoint).AsSingle();

            var playerPawn = Container.InstantiatePrefabForComponent<PlayerPawn>(
                _playerPawnPrefab,
                _playerSpawnPoint.position,
                _playerSpawnPoint.rotation,
                null);

            Container.BindInstance(playerPawn).AsSingle();
        }
    }
}