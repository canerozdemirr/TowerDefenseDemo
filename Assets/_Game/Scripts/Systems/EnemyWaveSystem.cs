using Cysharp.Threading.Tasks;
using Data.Configs;
using Events;
using UnityEngine;
using Utilities.TypeUtilities;

namespace Systems
{
    public class EnemyWaveSystem : BaseSystem
    {
        private LevelWaveConfig _levelWaveConfig;
        public EnemyWaveSystem(LevelWaveConfig levelWaveConfig)
        {
            _levelWaveConfig = levelWaveConfig;
        }
        public override void Initialize()
        {
            base.Initialize();
            EventDispatcher.Instance.Subscribe<LevelStartedEvent>(OnLevelStart);
        }

        public override void Dispose()
        {
            base.Dispose();
            EventDispatcher.Instance.Unsubscribe<LevelStartedEvent>(OnLevelStart);
        }

        private void OnLevelStart(LevelStartedEvent levelStartedEvent)
        {
            for (int i = 0; i < _levelWaveConfig.EnemyWaveConfigList.Count; i++)
            {
                //TODO: Start Enemy Spawn Routine
            }
        }
    }
}

[System.Serializable]
public struct EnemyWaveData
{
    public Enums.EnemyType enemyType;
    public int enemyAmount;
}

[System.Serializable]
public struct EnemyWaveDelayData
{
    public int enemyWaveIndex;
    public float enemyWaveDelayAmount;
}
