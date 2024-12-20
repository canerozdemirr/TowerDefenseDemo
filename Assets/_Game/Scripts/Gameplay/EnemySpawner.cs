using System.Collections.Generic;
using Gameplay.Enemies;
using Pools;
using UnityEngine;
using Utilities.TypeUtilities;

namespace Gameplay
{
    public class EnemySpawner : MonoBehaviour
    {
        private Dictionary<Enums.EnemyType, GameObjectPool<BaseEnemy>> _prefabMap;

        public void Initialize()
        {
            _prefabMap = new Dictionary<Enums.EnemyType, GameObjectPool<BaseEnemy>>();
        }

        public BaseEnemy Spawn(Enums.EnemyType enemyType)
        {
            if (_prefabMap.TryGetValue(enemyType, out var pool))
            {
                return pool.Spawn();
            }

            Debug.LogError($"No pool found for ItemType: {enemyType}");
            return null;
        }

        public void DeSpawn(BaseEnemy enemy)
        {
            if (_prefabMap.TryGetValue(enemy.EnemyType, out var pool))
            {
                pool.DeSpawn(enemy);
            }
            else
            {
                Debug.LogError($"No pool found for ItemType: {enemy.EnemyType}");
            }
        }

        private void ClearPools()
        {
            foreach (var pool in _prefabMap.Values)
            {
                pool.ClearObjectReferences();
            }

            _prefabMap.Clear();
        }
    }
}