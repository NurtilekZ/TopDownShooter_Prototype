using System.Collections.Generic;
using System.Threading.Tasks;
using _current.Infrastructure.AssetManagement;
using _current.Infrastructure.Factories.Interfaces;
using _current.Services.StaticData;
using _current.UI;
using _current.UI.Core;
using _current.UI.Core.MVVM;
using _current.UI.Popups;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace _current.Infrastructure.Factories
{
    public class UIFactory : IUIFactory
    {
        private readonly DiContainer _container;
        private readonly IAssetProvider _assetsProvider;
        private readonly IStaticDataService _staticDataService;

        private readonly Dictionary<IViewModel, IView> _screens = new();
        private RootCanvas _rootCanvas;
        private RootCanvas _sceneCanvas;

        private readonly string[] _uiAssetPaths = {
            AssetsPath.RootCanvas,
            
            AssetsPath.MainMenuScreen,
            AssetsPath.GameHUDScreen,
            AssetsPath.PauseScreen,
            AssetsPath.GameOverScreen,
            AssetsPath.LoadingScreen,
            
            AssetsPath.OneButtonPopup,
            AssetsPath.TwoButtonPopup,
        };

        public UIFactory(DiContainer container, IAssetProvider assetsProvider, IStaticDataService staticDataService)
        {
            _container = container;
            _assetsProvider = assetsProvider;
            _staticDataService = staticDataService;
        }

        public async Task WarmUp()
        {
            foreach (var assetPath in _uiAssetPaths)
            {
                await _assetsProvider.Load<GameObject>(assetPath);
            }
        }

        public void CleanUp()
        {
            foreach (var assetPath in _uiAssetPaths) 
                _assetsProvider.Release(assetPath);

            foreach (var view in _screens) 
                view.Value.HideAndUnbind();
            
            _screens.Clear();
        }

        public async Task CreateRootCanvas()
        {
            var prefab = await _assetsProvider.Load<GameObject>(AssetsPath.RootCanvas);
            _rootCanvas = Object.Instantiate(prefab).GetComponent<RootCanvas>();
            Object.DontDestroyOnLoad(_rootCanvas);
        }

        public async Task CreateSceneRootCanvas()
        {
            var prefab = await _assetsProvider.Load<GameObject>(AssetsPath.RootCanvas);
            _sceneCanvas = Object.Instantiate(prefab).GetComponent<RootCanvas>();
        }
        
        public async Task<IView> GetOrCreateView(IViewModel viewModel)
        {
            var cachedView = GetView(viewModel);
            if (cachedView != null)
            {
                _container.Inject(cachedView);
                return cachedView;
            }

            var targetCanvas = viewModel.IsInSceneCanvas ? _sceneCanvas : _rootCanvas;
            var prefab = await _assetsProvider.Load<GameObject>(viewModel.AssetPath);
            var parent = viewModel switch
            {
                IPopupViewModel => targetCanvas.PopupsGroup.transform,
                IOverlayViewModel => targetCanvas.OverlaysGroup.transform,
                _ => targetCanvas.ScreensGroup.transform
            };
            var newView = Object.Instantiate(prefab, parent).GetComponent<IView>();
            _screens[viewModel] = newView;
            
            _container.Inject(newView);
            return newView;
        }

        private IView GetView(IViewModel viewModel)
        {
            return _screens.GetValueOrDefault(viewModel);
        }
    }
}