using System.Threading.Tasks;
using Core.Hero;
using Infrastructure.AssetsManagement;
using Infrastructure.Factories.Interfaces;
using JetBrains.Annotations;
using Services.StaticData;
using UnityEngine;
using Zenject;

namespace Infrastructure.Factories
{
    public class HeroFactory : IHeroFactory
    {
        private const string HeroPrefabId = "HeroPrefab";
        private readonly IAssetProvider _assetProvider;

        private readonly DiContainer _container;
        private readonly IStaticDataService _staticDataService;

        public HeroFactory(DiContainer container, IAssetProvider assetProvider, IStaticDataService staticDataService)
        {
            _container = container;
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
        }

        [CanBeNull] public GameObject Hero { get; private set; }

        public async Task WarmUp()
        {
            await _assetProvider.Load<GameObject>(HeroPrefabId);
        }

        public void CleanUp()
        {
            Hero = null;
            _assetProvider.Release(HeroPrefabId);
        }

        public async Task<GameObject> Create(Vector3 at)
        {
            var config = _staticDataService.ForHero();
            var prefab = await _assetProvider.Load<GameObject>(HeroPrefabId);
            var hero = Object.Instantiate(prefab, at, Quaternion.identity);

            _container.InjectGameObject(hero);

            var health = hero.GetComponent<HeroHealth>();
            health.MaxHP = config.Health;
            health.CurrentHP.Value = health.MaxHP;

            var attack = hero.GetComponent<HeroAttack>();
            attack.AttackDamage.Value = config.AttackDamage;
            attack.Shield = config.Resistance;

            return Hero = hero;
        }
    }
}