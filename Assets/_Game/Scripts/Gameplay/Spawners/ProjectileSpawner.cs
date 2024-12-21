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
        
        [Inject]
        public void Inject(DiContainer container, ProjectileListConfig projectileListConfig)
        {
            _container = container;
            _projectileListConfig = projectileListConfig;
            InitializePool();
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
                return spawnedProjectile;
            }

            Debug.LogError($"No pool found for projectile type of: {projectileType}");
            return null;
        }

        public void DeSpawn(BaseProjectile projectile)
        {
            if (_prefabMap.TryGetValue(projectile.ProjectileType, out var pool))
            {
                pool.DeSpawn(projectile);
            }
            else
            {
                Debug.LogError($"No pool found for projectile type of: {projectile.ProjectileType}");
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
    }
}
