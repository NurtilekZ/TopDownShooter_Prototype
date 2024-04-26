using System.Collections.Generic;
using UnityEngine;

namespace _current.Utils
{
    public class GameObjectPool
    {
        private readonly GameObject _prefab;
        private readonly Transform _parent;
        private readonly Stack<GameObject> _pool = new();
 
        public GameObjectPool(GameObject prefab, Transform parent)
        {
            _prefab = prefab;
            _parent = parent;
        }

        public GameObject Get()
        {
            if (_pool.Count > 0)
            {
                var cachedGo = _pool.Pop();
                cachedGo.SetActive(true);
                return cachedGo;
            }

            var go = Object.Instantiate(_prefab, _parent);
            return go;
        }

        public void Release(GameObject go)
        {
            if (_pool.Contains(go)) return;
            go.SetActive(false);
            _pool.Push(go);
        }
    }
}