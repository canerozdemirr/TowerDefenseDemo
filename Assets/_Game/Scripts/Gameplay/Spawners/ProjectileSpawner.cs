using System;
using System.Collections.Generic;
using Data.Configs.EnemyConfigs;
using Data.Configs.ProjectileConfigs;
using Events;
using Gameplay.Enemies;
using Gameplay.Projectiles;
using Interfaces;
using Pools;
using UnityEngine;
using Utilities.TypeUtilities;
using Zenject;

namespace Gameplay.Spawners
{
    public class ProjectileSpawner : MonoBehaviour, IProjectileSpawner
    {
        private Dictionary<Enums.ProjectileType, GameObjectPool<BaseProjectile>> _prefabMap;
        private ProjectileListConfig _projectileListConfig;

        private DiContainer _container;
        private IEventDispatcher _eventDispatcher;

        private List<BaseProjectile> _activeProjectileList;
            
        [Inject]
        public void Inject(DiContainer container, ProjectileListConfig projectileListConfig, IEventDispatcher eventDispatcher)
        {
            _container = container;
            _projectileListConfig = projectileListConfig;
            InitializePool();
            _eventDispatcher = eventDispatcher;
            _eventDispatcher.Subscribe<LevelFailedEvent>(OnLevelFail);
            _activeProjectileList = new List<BaseProjectile>();
        }
        
        private void InitializePool()
        {
            _prefabMap = new Dictionary<Enums.ProjectileType, GameObjectPool<BaseProjectile>>();
            foreach (ProjectileConfig projectileReference in _projectileListConfig.AllProjectileConfigs)
            {
                if (!_prefabMap.ContainsKey(projectileReference.projectileType))
                {
                    GameObject poolParent = new GameObject
                    {
                        name = projectileReference.projectileType + " Pool",
                        transform =
                        {
                            parent = transform
                        }
                    };
                    GameObjectPool<BaseProjectile> pool =
                        GameObjectPool<BaseProjectile>.Create(projectileReference.projectilePrefab, poolParent.transform, 5, 50);
                    _prefabMap[projectileReference.projectileType] = pool;
                }
                else
                {
                    Debug.LogWarning($"EnemyType {projectileReference.projectileType} already exists in the map. Skipping.");
                }
            }
        }
        
        public BaseProjectile Spawn(Enums.ProjectileType projectileType)
        {
            if (_prefabMap.TryGetValue(projectileType, out var pool))
            {
                BaseProjectile spawnedProjectile = pool.Spawn();
                _container.Inject(spawnedProjectile);
                _activeProjectileList.Add(spawnedProjectile);
                return spawnedProjectile;
            }

            Debug.LogError($"No pool found for projectile type of: {projectileType}");
            return null;
        }

        public void DeSpawn(BaseProjectile projectile)
        {
            if (_prefabMap.TryGetValue(projectile.ProjectileType, out var pool))
            {
                _activeProjectileList.Remove(projectile);
                pool.DeSpawn(projectile);
            }
            else
            {
                Debug.LogError($"No pool found for projectile type of: {projectile.ProjectileType}");
            }
        }

        public void ClearPools()
        {
            if (_prefabMap == null || _prefabMap.Count == 0)
            {
                Debug.LogWarning("Pools are already empty.");
                return;
            }

            foreach (var pool in _prefabMap.Values)
            {
                pool.ClearObjectReferences();
            }
            _prefabMap.Clear();
        }

        private void OnDestroy()
        {
            _eventDispatcher.Unsubscribe<LevelFailedEvent>(OnLevelFail);
            ClearPools();
        }
        
        private void OnLevelFail(LevelFailedEvent levelFailedEvent)
        {
            foreach (var pool in _prefabMap.Values)
            {
                foreach (var projectile in _activeProjectileList)
                {
                    pool.DeSpawn(projectile);
                }
            }
            
            _activeProjectileList.Clear();
        }
    }
}
