using UnityEngine;
using Utilities.TypeUtilities;

namespace Data.Configs.ProjectileConfigs
{
    [CreateAssetMenu(fileName = "New Projectile Config", menuName = "Tower Defense/Configs/Projectile Config/New Projectile Config")]
    public class ProjectileConfig : BaseDataConfig
    {
        public Enums.ProjectileType projectileType;
        public GameObject projectilePrefab;
        public float speed;
    }
}
