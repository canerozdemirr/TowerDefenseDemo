using System;
using System.Collections.Generic;
using Data.Configs.ProjectileConfigs;
using Data.Configs.TowerConfigs;
using Events;
using Gameplay.Enemies;
using Interfaces;
using States.StateMachines;
using UnityEngine;
using Utilities.TypeUtilities;
using Zenject;
using IPoolable = Interfaces.IPoolable;

namespace Gameplay.Towers
{
    public abstract class BaseTower : MonoBehaviour, IPoolable
    {
        #region Attributes

        private TowerConfig _towerConfig;
        private Enums.TowerType _towerType;
        private float _delayBetweenEachFiring;
        private float _towerCheckRadius;
        private float _towerBaseDamage;
        private ProjectileConfig _projectileConfig;
        private IProjectileSpawner _projectileSpawner;
        private IEventDispatcher _eventDispatcher;
        protected TowerStateMachine<BaseTower> _towerStateMachine;
        private List<BaseEnemy> _enemyListInRange;
        private bool _isTowerActive;

        #endregion
        
        #region Components

        [SerializeField] 
        private SphereCollider _towerCollider;
        
        [SerializeField]
        private LayerMask _enemyDetectionLayerMask;

        [SerializeField] 
        private Transform _projectileSpawnPoint;

        #endregion
        
        #region Getters
        public Enums.TowerType TowerType => _towerType;
        public SphereCollider TowerCollider => _towerCollider;
        public LayerMask EnemyDetectionLayerMask => _enemyDetectionLayerMask;
        public float DelayBetweenEachFiring => _delayBetweenEachFiring;
        public ProjectileConfig ProjectileConfig => _projectileConfig;
        public IProjectileSpawner ProjectileSpawner => _projectileSpawner;
        public float TowerBaseDamage => _towerBaseDamage;
        public float TowerCheckRadius => _towerCheckRadius;
        public Transform ProjectileSpawnPoint => _projectileSpawnPoint;
        public List<BaseEnemy> EnemyListInRange => _enemyListInRange;
        public IEventDispatcher EventDispatcher => _eventDispatcher;
        #endregion

        [Inject]
        public void Inject(IProjectileSpawner projectileSpawner, IEventDispatcher eventDispatcher)
        {
            _projectileSpawner = projectileSpawner;
            _eventDispatcher = eventDispatcher;
        }

        public virtual void Initialize(TowerConfig towerConfig)
        {
            if (_towerConfig == null)
            {
                _towerConfig = towerConfig;
            }

            _towerType = _towerConfig.TowerType;
            _towerBaseDamage = _towerConfig.TowerBaseDamage;
            _delayBetweenEachFiring = _towerConfig.DelayBetweenEachFiring;
            _towerCheckRadius = _towerConfig.TowerCheckRadius;
            _projectileConfig = _towerConfig.ProjectileConfig;
            _enemyListInRange = new List<BaseEnemy>();
            _eventDispatcher.Subscribe<EnemyDeathEvent>(OnEnemyDeath);
        }

        public virtual void Update()
        {
            if (!_isTowerActive)
                return;
            
            _towerStateMachine.UpdateState(this);
        }

        public void ChangeToNextState()
        {
            _towerStateMachine.ChangeToNextState(this);
        }

        public void SetupTowerCollider()
        {
            _towerCollider.enabled = true;
            _towerCollider.radius = _towerCheckRadius;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out BaseEnemy baseEnemy))
            {
                if ((_enemyDetectionLayerMask.value & (1 << other.gameObject.layer)) != 0)
                {
                    _enemyListInRange.Add(baseEnemy);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out BaseEnemy baseEnemy) && _enemyListInRange.Contains(baseEnemy))
            {
                _enemyListInRange.Remove(baseEnemy);
            }
        }

        public void OnCalledFromPool()
        {
            _isTowerActive = true;
            _eventDispatcher?.Subscribe<EnemyDeathEvent>(OnEnemyDeath);
        }

        public void OnReturnToPool()
        {
            _towerCollider.enabled = false;
            _isTowerActive = false;
            _enemyListInRange.Clear();
            _towerStateMachine.Reset(this);
            _eventDispatcher.Unsubscribe<EnemyDeathEvent>(OnEnemyDeath);
        }
        
        private void OnEnemyDeath(EnemyDeathEvent enemyDeathEvent)
        {
            if (_enemyListInRange.Contains(enemyDeathEvent.DeadEnemy))
            {
                _enemyListInRange.Remove(enemyDeathEvent.DeadEnemy);
            }
        }
    }
}
