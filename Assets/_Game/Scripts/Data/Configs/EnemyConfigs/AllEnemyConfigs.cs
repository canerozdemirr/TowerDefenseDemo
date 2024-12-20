using System.Collections.Generic;
using Data.Configs;
using UnityEngine;

namespace Data.Configs.EnemyConfigs
{
    [CreateAssetMenu(fileName = "New Enemy List Config",
        menuName = "Tower Defense/Configs/Enemy Config/New Enemy List Config")]
    public class AllEnemyConfigs : BaseDataConfig
    {
        [SerializeField] 
        private List<EnemyConfig> _enemyConfigList;

        public List<EnemyConfig> EnemyConfigList => _enemyConfigList;
    }
}
