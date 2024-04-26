using System.Threading.Tasks;
using _current.Infrastructure.SceneManagement;
using _current.Services;
using UnityEngine.ResourceManagement.ResourceProviders;
using Zenject;

namespace _current.Infrastructure.AssetManagement
{
    public interface IAssetProvider : IService, IInitializable
    {
        Task<T> Load<T>(string key) where T : class;
        Task<SceneInstance> LoadScene(SceneName sceneName);
        void Release(string key);
        void CleanUp();
    }
}