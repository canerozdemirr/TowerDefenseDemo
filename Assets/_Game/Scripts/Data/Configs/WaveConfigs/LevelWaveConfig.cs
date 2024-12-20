using System.Collections.Generic;
using Data.Configs;
using UnityEngine;

namespace Data.Configs
{
    [CreateAssetMenu(fileName = "New Level Wave Config",
        menuName = "Tower Defense/Configs/Level Config/New Level Wave Config")]
    public class LevelWaveConfig : BaseDataConfig
    {
        [SerializeField] 
        private List<EnemyWaveConfig> _allEnemyWaveConfigs;

        [SerializeField] 
        private float _delayBetweenWaves;

        public List<EnemyWaveConfig> EnemyWaveConfigList => _allEnemyWaveConfigs;

        public float DelayBetweenWaves => _delayBetweenWaves;
    }
}
