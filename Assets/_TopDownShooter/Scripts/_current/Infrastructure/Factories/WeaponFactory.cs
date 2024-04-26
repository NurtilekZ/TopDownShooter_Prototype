using System.Collections.Generic;
using System.Threading.Tasks;
using _current.Core.Pawns.Player;
using _current.Core.Systems.DamageSystem;
using _current.Core.Systems.WeaponSystem;
using _current.Core.Systems.WeaponSystem.Data;
using _current.Data;
using _current.Infrastructure.AssetManagement;
using _current.Infrastructure.Factories.Interfaces;
using _current.Services.StaticData;
using UnityEngine;
using Zenject;

namespace _current.Infrastructure.Factories
{
    public class WeaponFactory : IWeaponFactory
    {
        private readonly DiContainer _container;
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticDataService;

        private readonly List<string> _cachedWeaponAssetPaths = new();

        public WeaponFactory(DiContainer container, IAssetProvider assetProvider, IStaticDataService staticDataService)
        {
            _container = container;
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
        }

        public Task WarmUp()
        {
            return null;
        }

        public void CleanUp()
        {
            foreach (var assetPath in _cachedWeaponAssetPaths) 
                _assetProvider.Release(assetPath);
        }

        public async Task<WeaponPawn> Create(IDamageSender owner, Transform holder, WeaponData weaponData)
        {
            var config = _staticDataService.ForWeapon(weaponData.typeId);
            var prefab = await _assetProvider.Load<GameObject>(config.PrefabAsset.AssetGUID);
            if (!_cachedWeaponAssetPaths.Contains(config.PrefabAsset.AssetGUID)) 
                _cachedWeaponAssetPaths.Add(config.PrefabAsset.AssetGUID);
            
            var weapon = Object.Instantiate(prefab, holder).GetComponent<WeaponPawn>();
            weapon.Initialize(owner, weaponData, config);
            
            _container.Inject(weapon);
            
            return weapon;
        }
    }
}