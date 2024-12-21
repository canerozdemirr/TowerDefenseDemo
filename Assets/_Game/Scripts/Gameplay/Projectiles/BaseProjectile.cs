using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Interfaces;
using UnityEngine;
using Utilities.TypeUtilities;
using Zenject;
using IPoolable = Interfaces.IPoolable;

namespace Gameplay.Projectiles
{
    public abstract class BaseProjectile : MonoBehaviour, IPoolable, IProjectile
    {
        #region Attributes

        [SerializeField] 
        protected Enums.ProjectileType _projectileType;

        private LayerMask _enemyLayerMask;
        private IProjectileSpawner _projectileSpawner;
        private float _speed;
        protected float _damageAmount;
        private bool _reachedToTarget;

        #endregion

        #region Getters

        public Enums.ProjectileType ProjectileType => _projectileType;
        
        #endregion

        [Inject]
        public void Inject(IProjectileSpawner projectileSpawner)
        {
            _projectileSpawner = projectileSpawner;
        }

        public virtual void PrepareProjectile(LayerMask enemyLayerMask, float speed, float damage)
        {
            _enemyLayerMask = enemyLayerMask;
            _speed = speed;
            _damageAmount = damage;
        }
        
        public virtual async void Launch(Vector3 targetPosition)
        {
            try
            {
                MoveTowardsTarget(targetPosition);
                await UniTask.WaitUntil(() => _reachedToTarget);
                OnReach();
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Projectile movement is canceled.");
            }
        }
        
        private void MoveTowardsTarget(Vector3 targetPosition)
        {
            float duration = Vector3.Distance(transform.position, targetPosition) / _speed;
            transform.DOMove(targetPosition, duration).SetEase(Ease.Linear).OnComplete(delegate
            {
                _reachedToTarget = true;
            });
        }

        protected virtual void ApplyDamageToTarget()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.5f, _enemyLayerMask);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (!hitColliders[i].TryGetComponent(out IHittable hittable)) 
                    continue;
                
                hittable.OnHit(_damageAmount);
                break;
            }
        }

        private void OnReach()
        {
            ApplyDamageToTarget();
            _reachedToTarget = false;
            _projectileSpawner.DeSpawn(this);
        }
        
        public void OnCalledFromPool()
        {
            
        }

        public void OnReturnToPool()
        {
            transform.DOKill();
        }
    }
}
