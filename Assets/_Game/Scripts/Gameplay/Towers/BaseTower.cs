using System;
using System.Collections.Generic;
using Data.Configs.TowerConfigs;
using Interfaces;
using States.StateMachines;
using UnityEngine;
using Utilities.TypeUtilities;

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

        protected TowerStateMachine<BaseTower> _towerStateMachine;

        #endregion
        
        #region Getters

        public Enums.TowerType TowerType => _towerType;
        
        #endregion

        #region Components

        [SerializeField] 
        private SphereCollider _towerCollider;

        #endregion

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
        }

        public virtual void Update()
        {
            _towerStateMachine.UpdateState(this);
        }

        public void ChangeToNextState()
        {
            _towerStateMachine.ChangeToNextState(this);
        }

        public void OnCalledFromPool()
        {
            
        }

        public void OnReturnToPool()
        {
            
        }
    }
}
