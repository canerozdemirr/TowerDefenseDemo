using Gameplay.Projectiles;
using Interfaces;
using UnityEngine;

namespace Gameplay.Projectiles
{
    public class BombProjectile : BaseProjectile
    {
        protected override void ApplyDamageToTarget()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5f);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].TryGetComponent(out IHittable hittable))
                {
                    hittable.OnHit(_damageAmount);
                }
            }
        }
    }
}
