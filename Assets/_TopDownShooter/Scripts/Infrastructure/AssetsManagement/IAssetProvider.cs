using System.Threading.Tasks;
using Infrastructure.SceneManagement;
using UnityEngine.ResourceManagement.ResourceProviders;
using Zenject;

namespace Infrastructure.AssetsManagement
{
    public interface IAssetProvider : IInitializable
    {
        public Task<T> Load<T>(string key) where T : class;
        public Task<SceneInstance> LoadScene(SceneName sceneName);
        public void Release(string key);
        public void Cleanup();
    }
}