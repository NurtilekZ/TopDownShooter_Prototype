using System.Threading.Tasks;
using _current.Core.Pawns.LootSystem;
using _current.Data;
using _current.Infrastructure.AssetManagement;
using _current.Infrastructure.Factories.Interfaces;
using _current.Services.StaticData;
using UnityEngine;
using Zenject;

namespace _current.Infrastructure.Factories
{
    public class LootFactory : ILootFactory
    {
        private readonly DiContainer _container;
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticDataService;
        private readonly WorldData _worldData;

        private readonly string[] _assetPaths =
        {
            AssetsPath.AmmoSupply,
            AssetsPath.CurrencySimple,
            AssetsPath.CurrencyRare,
            AssetsPath.CurrencySuper,
            
            AssetsPath.AmmoSupply,
            AssetsPath.HealSupply,
            
            AssetsPath.SuppliesSmall,
            AssetsPath.SuppliesBig,
        };

        public LootFactory(DiContainer container, IAssetProvider assetProvider, IStaticDataService staticDataService)
        {
            _container = container;
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
        }

        public async Task WarmUp()
        {
            foreach (var assetPath in _assetPaths) 
                await _assetProvider.Load<GameObject>(assetPath);
        }

        public void CleanUp()
        {
            foreach (var assetPath in _assetPaths) 
                _assetProvider.Release(assetPath);
        }

        public async Task<LootView> Create(string assetPath)
        {
            var prefab = await _assetProvider.Load<GameObject>(assetPath);
            var lootPiece = prefab.GetComponent<LootView>();
            
            return lootPiece;
        }
    }
}