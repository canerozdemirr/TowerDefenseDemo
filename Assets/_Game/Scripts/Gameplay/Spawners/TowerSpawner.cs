using System.Collections.Generic;
using Data.Configs.TowerConfigs;
using Gameplay.Towers;
using Interfaces;
using Pools;
using UnityEngine;
using Utilities.TypeUtilities;
using Zenject;

namespace Gameplay.Spawners
{
    public class TowerSpawner : MonoBehaviour, ITowerSpawner
    {
        private Dictionary<Enums.TowerType, GameObjectPool<BaseTower>> _prefabMap;
        private DiContainer _container;
        private IEventDispatcher _eventDispatcher;
        private TowerConfigList _towerConfigList;

        [Inject]
        public void Inject(DiContainer container, TowerConfigList towerConfigList, IEventDispatcher eventDispatcher)
        {
            _container = container;
            _eventDispatcher = eventDispatcher;
            _towerConfigList = towerConfigList;
            InitializePool();
        }
        
        private void InitializePool()
        {
            _prefabMap = new Dictionary<Enums.TowerType, GameObjectPool<BaseTower>>();
            foreach (TowerConfig config in _towerConfigList.AllTowerConfigs)
            {
                if (!_prefabMap.ContainsKey(config.TowerType))
                {
                    GameObject poolParent = new GameObject
                    {
                        name = config.TowerType + " Pool",
                        transform =
                        {
                            parent = transform
                        }
                    };
                    GameObjectPool<BaseTower> pool =
                        GameObjectPool<BaseTower>.Create(config.TowerPrefab, poolParent.transform, 5, 10);
                    _prefabMap[config.TowerType] = pool;
                }
                else
                {
                    Debug.LogWarning($"EnemyType {config.TowerType} already exists in the map. Skipping.");
                }
            }
        }
        
        public BaseTower Spawn(Enums.TowerType towerType)
        {
            if (_prefabMap.TryGetValue(towerType, out var pool))
            {
                BaseTower spawnedTower = pool.Spawn();
                _container.Inject(spawnedTower);
                for (int i = 0; i < _towerConfigList.AllTowerConfigs.Count; i++)
                {
                    if (_towerConfigList.AllTowerConfigs[i].TowerType == towerType)
                    {
                        spawnedTower.Initialize(_towerConfigList.AllTowerConfigs[i]);
                        break;
                    }
                }

                return spawnedTower;
            }

            Debug.LogError($"No pool found for EnemyType: {towerType}");
            return null;
        }

        public void DeSpawn(BaseTower tower)
        {
            if (_prefabMap.TryGetValue(tower.TowerType, out var pool))
            {
                pool.DeSpawn(tower);
            }
            else
            {
                Debug.LogError($"No pool found for EnemyType: {tower.TowerType}");
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
