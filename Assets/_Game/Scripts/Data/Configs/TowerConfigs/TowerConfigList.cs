using System.Collections.Generic;
using UnityEngine;

namespace Data.Configs.TowerConfigs
{
    [CreateAssetMenu(fileName = "New Tower List Config",
        menuName = "Tower Defense/Configs/Tower Config/New Tower List Config")]
    public class TowerConfigList : ScriptableObject
    {
        [SerializeField] private List<TowerConfig> _allTowerConfigs;

        public List<TowerConfig> AllTowerConfigs => _allTowerConfigs;
    }
}
