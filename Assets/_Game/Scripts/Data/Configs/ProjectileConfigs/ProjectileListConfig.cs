using System.Collections.Generic;
using Data.Configs;
using Data.Configs.ProjectileConfigs;
using UnityEngine;

namespace Data.Configs.ProjectileConfigs
{
    [CreateAssetMenu(fileName = "New Projectile List Config", menuName = "Tower Defense/Configs/Projectile Config/New Projectile List Config")]
    public class ProjectileListConfig : BaseDataConfig
    {
        [SerializeField] 
        private List<ProjectileConfig> _allProjectileConfigs;
        public List<ProjectileConfig> AllProjectileConfigs => _allProjectileConfigs;
    }
}
