using Interfaces;
using UnityEngine;
using UnityEngine.Pool;

namespace Pools
{
    public class GameObjectPool<T> where T : MonoBehaviour, IPoolable
    {
        private readonly int _defaultSize;
        private readonly GameObject _prefab;
        private readonly IObjectPool<T> _pool;
        private readonly Transform _poolParent;
        private readonly bool _dontDestroyOnLoad;

        private GameObjectPool(GameObject prefab, Transform poolParent = null, int defaultSize = 1, int maxSize = 100,
            bool dontDestroyOnLoad = false)
        {
            _prefab = prefab;
            _poolParent = poolParent;
            _defaultSize = defaultSize;
            _dontDestroyOnLoad = dontDestroyOnLoad;

            ValidatePrefab();

            _pool = new ObjectPool<T>(CreatePoolObject, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true,
                _defaultSize, maxSize);

            WarmUp();
        }

        #region Pool Events

        private void OnTakeFromPool(T takenObject)
        {
            takenObject.OnCalledFromPool();
            takenObject.gameObject.SetActive(true);
        }

        private void OnReturnedToPool(T returnedObject)
        {
            returnedObject.OnReturnToPool();
            returnedObject.gameObject.SetActive(false);
            returnedObject.transform.parent = _poolParent;
        }

        private void OnDestroyPoolObject(T destroyedObject)
        {
            Object.Destroy(destroyedObject.gameObject);
        }

        #endregion

        public T Spawn()
        {
            return _pool.Get();
        }

        public static GameObjectPool<T> Create(GameObject prefab, Transform parent, int defaultSize = 10,
            int maxSize = 100, bool dontDestroyOnLoad = false)
        {
            return new GameObjectPool<T>(prefab, parent, defaultSize, maxSize, dontDestroyOnLoad);
        }

        public void DeSpawn(T targetObject)
        {
            _pool.Release(targetObject);
        }

        public void ClearObjectReferences()
        {
            _pool.Clear();
        }

        private T CreatePoolObject()
        {
            GameObject pooledObject = Object.Instantiate(_prefab, _poolParent);
            T component = pooledObject.GetComponent<T>();
            if (component == null)
            {
                Debug.LogWarning($"Prefab {_prefab.name} does not contain a {typeof(T)} component. Adding it dynamically.");
                component = pooledObject.AddComponent<T>();
            }

            if (_dontDestroyOnLoad)
            {
                Object.DontDestroyOnLoad(pooledObject);
            }

            return component;
        }

        private void ValidatePrefab()
        {
            if (_prefab.GetComponent<T>() == null)
            {
                Debug.LogWarning($"Prefab {_prefab.name} does not contain a {typeof(T)} component. It will be added dynamically.");
            }
        }

        private void WarmUp()
        {
            for (int i = 0; i < _defaultSize; i++)
            {
                var obj = _pool.Get();
                _pool.Release(obj);
            }
        }
    }
}
