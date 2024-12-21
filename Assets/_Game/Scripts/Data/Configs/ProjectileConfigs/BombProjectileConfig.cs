using UnityEngine;

namespace Data.Configs.ProjectileConfigs
{
    [CreateAssetMenu(fileName = "New Bomb Projectile Config",
        menuName = "Tower Defense/Configs/Projectile Config/New Bomb Projectile Config")]
    public class BombProjectileConfig : ProjectileConfig
    {
        public float explosionRadius;
    }
}
