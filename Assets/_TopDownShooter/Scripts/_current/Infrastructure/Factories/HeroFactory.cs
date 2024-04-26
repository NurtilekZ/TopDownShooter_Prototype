using System.Threading.Tasks;
using _current.Core.Pawns.Player;
using _current.Infrastructure.AssetManagement;
using _current.Infrastructure.Factories.Interfaces;
using _current.Services.StaticData;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace _current.Infrastructure.Factories
{
    public class HeroFactory : IHeroFactory
    {
        private readonly DiContainer _container;
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticDataService;

        [CanBeNull] public GameObject Hero { get; private set; }

        public HeroFactory(DiContainer container,IAssetProvider assetProvider, IStaticDataService staticDataService)
        {
            _container = container;
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
        }

        public async Task WarmUp()
        {
            await _assetProvider.Load<GameObject>(AssetsPath.Player);
        }

        public void CleanUp()
        { 
            Hero = null;
            _assetProvider.Release(AssetsPath.Player);
        }

        public async Task<GameObject> Create(Vector3 at)
        {
            var heroData = _staticDataService.ForHero();
            var prefab = await _assetProvider.Load<GameObject>(AssetsPath.Player);
            var hero  = Object.Instantiate(prefab, at, Quaternion.identity);

            _container.InjectGameObject(hero);

            var health = hero.GetComponent<PlayerHealth>();
            health.Initialize(heroData.Hp);

            return Hero = hero;
        }
    }
}