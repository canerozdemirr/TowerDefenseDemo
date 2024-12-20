using System.Collections.Generic;
using Data.Configs;
using NaughtyAttributes;
using UnityEngine;

namespace Data.Configs
{
    [CreateAssetMenu(fileName = "New Enemy Wave Config",
        menuName = "Tower Defense/Configs/Enemy Config/New Enemy Wave Config")]
    public class EnemyWaveConfig : BaseDataConfig
    {
        [SerializeField] 
        private List<EnemyWaveData> _enemyWaveDataList;
        
        [Foldout("Spawn Attributes")] 
        [SerializeField]
        private float _spawnDelayBetweenEachUnit;

        public List<EnemyWaveData> EnemyWaveDataList => _enemyWaveDataList;
        public float SpawnDelayBetweenEachUnit => _spawnDelayBetweenEachUnit;
    }
}
