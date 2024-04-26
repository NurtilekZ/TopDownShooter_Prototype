using _current.Core.Logic;
using UnityEngine;
using Zenject;

namespace _current.Infrastructure.Installers
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