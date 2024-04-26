using System;
using System.Threading.Tasks;
using _current.Infrastructure.AssetManagement;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace _current.Infrastructure.SceneManagement
{
    public class SceneLoader
    {
        private readonly IAssetProvider _assetProvider;

        public SceneLoader(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public async Task<SceneInstance> Load(SceneName sceneName, Action onLoaded = null)
        {
            var scene = await _assetProvider.LoadScene(sceneName);
            scene.ActivateAsync();
            
            onLoaded?.Invoke();
            return scene;
        }
        
        public void MoveGameObjectToScene(GameObject gameObject, SceneInstance targetScene)
        {
            SceneManager.MoveGameObjectToScene(gameObject, targetScene.Scene);
        }

        public void MoveGameObjectToScene(GameObject gameObject, Scene targetScene)
        {
            SceneManager.MoveGameObjectToScene(gameObject, targetScene);
        }
    }
}