using Infrastructure.AssetsManagement;
using Infrastructure.Factories;
using Infrastructure.Factories.Interfaces;
using Infrastructure.SceneManagement;
using Services.Input;
using UnityEngine;
using Zenject;

namespace Infrastructure.Intallers
{
    public class InfrastructureInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _curtainServicePrefab;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<AddressableProvider>().AsSingle();
            Container.Bind<SceneLoader>().AsSingle();

            BindServices();
            BindFactories();
        }

        private void BindServices()
        {
            // Container.Bind<ILoggingService>().To<LoggingService>().AsSingle().NonLazy();
            Container.Bind<IInputService>().To<InputService>().AsSingle().NonLazy();
            // Container.BindInterfacesAndSelfTo<StaticDataService>().AsSingle().NonLazy();
            // Container.BindInterfacesAndSelfTo<PersistentDataService>().AsSingle().NonLazy();
            // Container.BindInterfacesAndSelfTo<SaveLoadLocalService>().AsSingle().NonLazy();
            // Container.BindInterfacesAndSelfTo<EconomyLocalService>().AsSingle().NonLazy();


            // Container.BindInterfacesAndSelfTo<LevelProgressServiceResolver>()
            //     .AsSingle()
            //     .CopyIntoDirectSubContainers();
            // Container.BindInterfacesAndSelfTo<LevelProgressService>().AsSingle().NonLazy();
        }

        private void BindFactories()
        {
            Container.BindInterfacesAndSelfTo<StateFactory>().AsSingle();
            Container.Bind<IHeroFactory>().To<HeroFactory>().AsSingle();
            // Container.Bind<IStageFactory>().To<StageFactory>().AsSingle();
            // Container.Bind<IEnemyFactory>().To<EnemyFactory>().AsSingle();
            // Container.Bind<IUIFactory>().To<UIFactory>().AsSingle();
        }
    }
}