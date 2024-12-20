using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Data.Configs;
using Events;
using Gameplay;
using UnityEngine;
using Utilities.TypeUtilities;

namespace Systems
{
    public class EnemyWaveSystem : BaseSystem
    {
        private LevelWaveConfig _levelWaveConfig;
        private EnemySpawner _enemySpawner;
        
        private CancellationTokenSource _cts;

        public EnemyWaveSystem(LevelWaveConfig levelWaveConfig, EnemySpawner enemySpawner)
        {
            _levelWaveConfig = levelWaveConfig;
            _enemySpawner = enemySpawner;
            _cts = new CancellationTokenSource();
        }
        public override void Initialize()
        {
            if (!IsConfigAssigned()) 
                return;
            
            base.Initialize();
            EventDispatcher.Instance.Subscribe<LevelStartedEvent>(OnLevelStart);
            EventDispatcher.Instance.Subscribe<LevelFailedEvent>(OnLevelFail);
        }

        public override void Dispose()
        {
            base.Dispose();
            EventDispatcher.Instance.Unsubscribe<LevelStartedEvent>(OnLevelStart);
            EventDispatcher.Instance.Unsubscribe<LevelFailedEvent>(OnLevelFail);
        }

        private void OnLevelStart(LevelStartedEvent levelStartedEvent)
        {
            EnemyWaveConfig enemyWaveConfig;
            for (int i = 0; i < _levelWaveConfig.EnemyWaveConfigList.Count; i++)
            {
                enemyWaveConfig = _levelWaveConfig.EnemyWaveConfigList[i];
                EnemyWaveData enemyWaveData;
                for (int j = 0; j < enemyWaveConfig.EnemyWaveDataList.Count; j++)
                {
                    enemyWaveData = enemyWaveConfig.EnemyWaveDataList[j];
                    StartSpawningEnemiesInWave(enemyWaveData, enemyWaveConfig.SpawnDelayBetweenEachUnit).Forget();
                }
            }
        }

        private void OnLevelFail(LevelFailedEvent levelFailedEvent)
        {
            _cts.Cancel();
            _enemySpawner.ClearPools();
        }
        
        private async UniTaskVoid StartSpawningEnemiesInWave(EnemyWaveData enemyWaveData, float enemySpawnInterval)
        {
            try
            {
                for (int i = 0; i < enemyWaveData.enemyAmount; i++)
                {
                    _cts.Token.ThrowIfCancellationRequested();
                    _enemySpawner.Spawn(enemyWaveData.enemyType);
                    await UniTask.Delay((int)(enemySpawnInterval * 1000), cancellationToken: _cts.Token);
                }
            }
            catch (OperationCanceledException)
            {
                Debug.LogWarning("Spawning task canceled.");
            }
            catch (Exception e)
            {
                Debug.LogError($"An error occurred during enemy spawning: {e.Message}");
            }
        }
        
        private bool IsConfigAssigned()
        {
            if (_levelWaveConfig != null && _levelWaveConfig.EnemyWaveConfigList != null) 
                return true;
            
            Debug.LogError("LevelWaveConfig or EnemyWaveConfigList is null. EnemyWaveSystem cannot start.");
            return false;

        }
    }
}

[Serializable]
public struct EnemyWaveData
{
    public Enums.EnemyType enemyType;
    public int enemyAmount;
}

[Serializable]
public struct EnemyWaveDelayData
{
    public int enemyWaveIndex;
    public float enemyWaveDelayAmount;
}
