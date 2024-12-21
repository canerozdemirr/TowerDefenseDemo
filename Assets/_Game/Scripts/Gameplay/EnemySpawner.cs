using System.Collections.Generic;
using Data.Configs.EnemyConfigs;
using Events;
using Gameplay.Enemies;
using Interfaces;
using Pools;
using UnityEngine;
using Utilities.TypeUtilities;
using Zenject;

namespace Gameplay
{
    public class EnemySpawner : MonoBehaviour, IEnemySpawner
    {
        [SerializeField] 
        private List<Transform> _spawnPoints;

        [SerializeField] private Transform _endPoint;

        private EnemyConfig _enemyConfig;

        private Dictionary<Enums.EnemyType, GameObjectPool<BaseEnemy>> _prefabMap;
        
        private DiContainer _container;
        private IEventDispatcher _eventDispatcher;

        [Inject]
        public void Inject(DiContainer container, AllEnemyConfigs allEnemyConfigs, IEventDispatcher eventDispatcher)
        {
            _container = container;
            _eventDispatcher = eventDispatcher;
            InitializePool(allEnemyConfigs);
        }

        private void OnEnable()
        {
            _eventDispatcher.Subscribe<EnemyDeathEvent>(OnEnemyDeath);
        }

        private void OnDestroy()
        {
            _eventDispatcher.Unsubscribe<EnemyDeathEvent>(OnEnemyDeath);
        }

        private void InitializePool(AllEnemyConfigs allEnemyConfigs)
        {
            _prefabMap = new Dictionary<Enums.EnemyType, GameObjectPool<BaseEnemy>>();
            foreach (EnemyConfig config in allEnemyConfigs.EnemyConfigList)
            {
                if (!_prefabMap.ContainsKey(config.EnemyType))
                {
                    GameObject poolParent = new GameObject
                    {
                        name = config.EnemyType + " Pool",
                        transform =
                        {
                            parent = transform
                        }
                    };
                    GameObjectPool<BaseEnemy> pool =
                        GameObjectPool<BaseEnemy>.Create(config.EnemyPrefab, poolParent.transform, 5, 50);
                    _prefabMap[config.EnemyType] = pool;
                }
                else
                {
                    Debug.LogWarning($"EnemyType {config.EnemyType} already exists in the map. Skipping.");
                }
            }
        }

        public void Spawn(Enums.EnemyType enemyType)
        {
            if (_prefabMap.TryGetValue(enemyType, out var pool))
            {
                BaseEnemy spawnedEnemy = pool.Spawn();
                _container.Inject(spawnedEnemy);
                spawnedEnemy.Initialize();
                
                spawnedEnemy.transform.position = _spawnPoints[0].position;
                if (spawnedEnemy is IMovable movable)
                {
                    movable.OnMovementStart(_endPoint.transform.position);
                }
            }

            else
            {
                Debug.LogError($"No pool found for EnemyType: {enemyType}");
            }
        }

        public void DeSpawn(BaseEnemy enemy)
        {
            if (_prefabMap.TryGetValue(enemy.EnemyType, out var pool))
            {
                pool.DeSpawn(enemy);
            }
            else
            {
                Debug.LogError($"No pool found for EnemyType: {enemy.EnemyType}");
            }
        }

        public void ClearPools()
        {
            foreach (var pool in _prefabMap.Values)
            {
                pool.ClearObjectReferences();
            }

            _prefabMap.Clear();
        }

        private void OnEnemyDeath(EnemyDeathEvent enemyDeathEvent)
        {
            DeSpawn(enemyDeathEvent.DeadEnemy);
        }
    }
}