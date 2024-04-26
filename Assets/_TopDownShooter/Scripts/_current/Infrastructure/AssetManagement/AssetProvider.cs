using System.Collections.Generic;
using System.Threading.Tasks;
using _current.Infrastructure.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace _current.Infrastructure.AssetManagement
{
    public class AssetProvider : IAssetProvider
    {
        private readonly Dictionary<string, AsyncOperationHandle> _completedCache = new();
        private readonly Dictionary<string, List<AsyncOperationHandle>> _handles = new();

        public void Initialize()
        {
            Addressables.InitializeAsync();
        }

        public async Task<T> Load<T>(string key) where T : class
        {
            if (_completedCache.TryGetValue(key, out var completedHandle)) 
                return completedHandle.Result as T;
            
            var handle = Addressables.LoadAssetAsync<T>(key);

            return await RunWithCacheOnComplete(handle, key);
        }

        public async Task<SceneInstance> LoadScene(SceneName sceneName)
        {
            var operationHandle = Addressables.LoadSceneAsync(sceneName.ToSceneString());
            return await operationHandle.Task;
        }

        public void Release(string key)
        {
            if (!_handles.ContainsKey(key))
                return;

            foreach (var handle in _handles[key]) 
                Addressables.Release(handle);

            _completedCache.Remove(key);
            _handles.Remove(key);
        }

        public void CleanUp()
        {
            if (_handles.Count == 0)
                return;
            
            foreach (var resourceHandles in _handles.Values)
            foreach (var handle in resourceHandles) 
                Addressables.Release(handle);
            
            _completedCache.Clear();
            _handles.Clear();
        }

        private async Task<T> RunWithCacheOnComplete<T>(AsyncOperationHandle<T> handle, string key) where T : class
        {
            handle.Completed += completeHandle =>
                _completedCache[key] = completeHandle;
            
            AddHandle(key, handle);
            return await handle.Task;
        }

        private void AddHandle<T>(string key, AsyncOperationHandle<T> handle) where T : class
        {
            if (!_handles.TryGetValue(key, out var resourceHandles))
            {
                resourceHandles = new List<AsyncOperationHandle>();
                _handles[key] = resourceHandles;
            }

            resourceHandles.Add(handle);
        }

        public void Dispose()
        {
            CleanUp();
        }
    }
}