using System;
using Data.Configs.EnemyConfigs;
using Events;
using Interfaces;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;
using Utilities.TypeUtilities;
using Zenject;
using IPoolable = Interfaces.IPoolable;

namespace Gameplay.Enemies
{
    public abstract class BaseEnemy : MonoBehaviour, IPoolable, IHittable, IMovable
    {
        #region Attributes

        [ShowNonSerializedField]
        private EnemyConfig _enemyConfig;
        
        private Enums.EnemyType _enemyType;
        private float _health;
        private float _speed;
        private int _baseDamage;
        #endregion

        #region Getters

        public Enums.EnemyType EnemyType => _enemyType;
        
        #endregion

        [Inject] 
        private AllEnemyConfigs _allEnemyConfigs;

        [Foldout("Components")] 
        [SerializeField]
        private NavMeshAgent _navMeshAgent;

        protected virtual void Initialize()
        {
            if (_enemyConfig == null)
            {
                foreach (EnemyConfig config in _allEnemyConfigs.EnemyConfigList)
                {
                    if (config.EnemyType != _enemyType) 
                        continue;
                
                    _enemyConfig = config;
                    break;
                }
            }

            _enemyType = _enemyConfig.EnemyType;
            _health = _enemyConfig.Health;
            _speed = _enemyConfig.Speed;
            _baseDamage = _enemyConfig.BaseDamage;
            _navMeshAgent.speed = _speed;
        }
        
        public virtual void OnCalledFromPool()
        {
            Initialize();
        }

        public virtual void OnReturnToPool()
        {
            _health = 0;
            _navMeshAgent.speed = 0;
            _baseDamage = 0;
            _navMeshAgent.enabled = false;
        }

        public void OnHit(float damage)
        {
            _health -= damage;
        }

        public void OnMovementStart(Vector3 endPosition)
        {
            _navMeshAgent.enabled = true;
            _navMeshAgent.SetDestination(endPosition);
        }

        public void OnMovementEnd()
        {
            EventDispatcher.Instance.Dispatch(new EnemyReachedToBaseEvent(_baseDamage));
        }
    }
}
