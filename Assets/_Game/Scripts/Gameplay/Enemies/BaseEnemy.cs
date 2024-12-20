using System;
using Data.Configs.EnemyConfigs;
using Interfaces;
using NaughtyAttributes;
using UnityEngine;
using Utilities.TypeUtilities;
using Zenject;
using IPoolable = Interfaces.IPoolable;

namespace Gameplay.Enemies
{
    public abstract class BaseEnemy : MonoBehaviour, IPoolable, IHittable
    {
        [Foldout("Enemy Stats")] 
        [SerializeField]
        private Enums.EnemyType _enemyType;

        [Foldout("Enemy Stats")] 
        [SerializeField]
        private float _health;
        
        [Foldout("Enemy Stats")] 
        [SerializeField]
        private float _speed;

        private EnemyConfig _enemyConfig;

        [Inject] 
        private AllEnemyConfigs _allEnemyConfigs;

        protected virtual void Initialize()
        {
            foreach (EnemyConfig config in _allEnemyConfigs.EnemyConfigList)
            {
                if (config.EnemyType != _enemyType) 
                    continue;
                
                _enemyConfig = config;
                break;
            }
            
            _health = _enemyConfig.Health;
            _speed = _enemyConfig.Speed;
        }
        
        public virtual void OnCalledFromPool()
        {
            Initialize();
        }

        public virtual void OnReturnToPool()
        {
            
        }

        public void OnHit(float damage)
        {
            
        }
    }
}
