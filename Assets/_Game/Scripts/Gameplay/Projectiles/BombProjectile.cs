using Data.Configs.ProjectileConfigs;
using Gameplay.Projectiles;
using Interfaces;
using UnityEngine;

namespace Gameplay.Projectiles
{
    public class BombProjectile : BaseProjectile
    {
        private BombProjectileConfig _bombProjectileConfig;
        public override void PrepareProjectile(LayerMask enemyLayerMask, ProjectileConfig projectileConfig, float damage)
        {
            base.PrepareProjectile(enemyLayerMask, projectileConfig, damage);
            _bombProjectileConfig = projectileConfig as BombProjectileConfig;
        }

        protected override void ApplyDamageToTarget()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, _bombProjectileConfig.explosionRadius);
            foreach (Collider detectedColliders in hitColliders)
            {
                if (detectedColliders.TryGetComponent(out IHittable hittable))
                {
                    hittable.OnHit(_damageAmount);
                }
            }
        }
    }
}
