using _current.Infrastructure.AssetManagement;
using _current.Infrastructure.Factories;
using _current.Infrastructure.Factories.Interfaces;
using _current.Infrastructure.SceneManagement;
using _current.Services.Economy;
using _current.Services.Input;
using _current.Services.LevelProgress;
using _current.Services.Logging;
using _current.Services.PersistentData;
using _current.Services.SaveLoad;
using _current.Services.StaticData;
using _current.Services.UI;
using Zenject;

namespace _current.Infrastructure.Installers
{
    public class InfrastructureInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<AssetProvider>().AsSingle();
            Container.Bind<SceneLoader>().AsSingle();

            BindServices();
            BindFactories();
        }

        private void BindServices()
        {
            Container.Bind<ILoggingService>().To<LoggingService>().AsSingle().NonLazy();
            Container.Bind<IInputService>().To<InputService>().AsSingle().NonLazy();
            Container.Bind<IUIService>().To<UIService>().AsSingle();
            Container.BindInterfacesAndSelfTo<StaticDataLocalService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PersistentDataService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SaveLoadLocalService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<EconomyLocalService>().AsSingle().NonLazy();
            
            
            Container.BindInterfacesAndSelfTo<LevelProgressServiceResolver>()
                .AsSingle()
                .CopyIntoDirectSubContainers();
            Container.BindInterfacesAndSelfTo<LevelProgressService>().AsSingle().NonLazy();
        }

        private void BindFactories()
        {
            Container.BindInterfacesAndSelfTo<StateFactory>().AsSingle();
            Container.Bind<IHeroFactory>().To<HeroFactory>().AsSingle();
            Container.Bind<ICameraFactory>().To<CameraFactory>().AsSingle();
            Container.Bind<IWeaponFactory>().To<WeaponFactory>().AsSingle();
            Container.Bind<ILevelFactory>().To<LevelFactory>().AsSingle();
            Container.Bind<IEnemyFactory>().To<EnemyFactory>().AsSingle();
            Container.Bind<ILootFactory>().To<LootFactory>().AsSingle();
            Container.Bind<IUIFactory>().To<UIFactory>().AsSingle();
        }
    }
}