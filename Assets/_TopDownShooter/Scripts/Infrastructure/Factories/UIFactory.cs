using System.Threading.Tasks;
using Infrastructure.AssetsManagement;
using Infrastructure.Factories.Interfaces;
using Meta.Hud;
using Services.Economy;
using Services.StaticData;
using UnityEngine;
using Zenject;

namespace Infrastructure.Factories
{
    public class UIFactory : IUIFactory
    {
        private const string UIRootPrefabId = "UIRootPrefab";
        private const string HudPrefabId = "HudPrefab";
        private const string MenuPrefabId = "MenuPrefab";
        private const string ShopPrefabId = "ShopPrefab";
        private const string StageCardPrefabId = "StageCardPrefab";
        private const string ShopItemCardPrefabId = "ShopItemCardPrefab";
        private readonly IAssetProvider _assetProvider;

        private readonly DiContainer _container;
        private readonly IEconomyService _economyService;
        private readonly IStaticDataService _staticDataService;

        private Canvas _uiRoot;

        public UIFactory(DiContainer container, IAssetProvider assetProvider, IStaticDataService staticDataService,
            IEconomyService economyService)
        {
            _container = container;
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
            _economyService = economyService;
        }

        public async Task WarmUp()
        {
            await _assetProvider.Load<GameObject>(UIRootPrefabId);
            await _assetProvider.Load<GameObject>(HudPrefabId);
            await _assetProvider.Load<GameObject>(MenuPrefabId);
            await _assetProvider.Load<GameObject>(ShopPrefabId);
        }

        public void CleanUp()
        {
            _assetProvider.Release(UIRootPrefabId);
            _assetProvider.Release(HudPrefabId);
            _assetProvider.Release(StageCardPrefabId);
            _assetProvider.Release(ShopItemCardPrefabId);
        }

        public async Task CreateUIRoot()
        {
            var prefab = await _assetProvider.Load<GameObject>(UIRootPrefabId);
            _uiRoot = Object.Instantiate(prefab).GetComponent<Canvas>();
        }

        public async Task<HUDController> CreateHud()
        {
            var prefab = await _assetProvider.Load<GameObject>(HudPrefabId);
            var hud = Object.Instantiate(prefab, _uiRoot.transform).GetComponent<HUDController>();

            _container.Inject(hud);
            return hud;
        }
    }
}