using Core.Logic;
using UnityEngine;
using Zenject;

namespace Infrastructure.Intallers
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private LevelProgressWatcher _levelProgressWatcher;

        public override void InstallBindings()
        {
            Container.BindInstance(_levelProgressWatcher);
        }
    }
}