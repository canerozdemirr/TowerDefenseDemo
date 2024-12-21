using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Data.Configs;
using Events;
using Gameplay;
using Interfaces;
using UnityEngine;
using Utilities.TypeUtilities;

namespace Systems
{
    public class EnemyWaveSystem : BaseSystem
    {
        private LevelWaveConfig _levelWaveConfig;
        private IEnemySpawner _enemySpawner;
        
        private CancellationTokenSource _cts;
        
        private readonly IEventDispatcher _eventDispatcher;

        public EnemyWaveSystem(LevelWaveConfig levelWaveConfig, IEnemySpawner enemySpawner, IEventDispatcher eventDispatcher)
        {
            _levelWaveConfig = levelWaveConfig;
            _enemySpawner = enemySpawner;
            _eventDispatcher = eventDispatcher;
        }
        public override void Initialize()
        {
            if (!IsWaveSpawnReady()) 
                return;
            
            base.Initialize();
            _eventDispatcher.Subscribe<LevelStartedEvent>(OnLevelStart);
            _eventDispatcher.Subscribe<LevelFailedEvent>(OnLevelFail);
            _cts = new CancellationTokenSource();
        }

        public override void Dispose()
        {
            base.Dispose();
            _eventDispatcher.Unsubscribe<LevelStartedEvent>(OnLevelStart);
            _eventDispatcher.Unsubscribe<LevelFailedEvent>(OnLevelFail);
            HandleCancellation();
        }

        private async void OnLevelStart(LevelStartedEvent levelStartedEvent)
        {
            try
            {
                foreach (var enemyWaveConfig in _levelWaveConfig.EnemyWaveConfigList)
                {
                    await SpawnWave(enemyWaveConfig);
                    await UniTask.Delay((int)(enemyWaveConfig.SpawnDelayBetweenEachUnit * 1000), cancellationToken: _cts.Token);
                }
            }
            catch (OperationCanceledException)
            {
                Debug.LogWarning("Wave spawning canceled.");
            }
            catch (Exception e)
            {
                Debug.LogError($"An error occurred during wave spawning: {e.Message}");
            }
        }

        private void OnLevelFail(LevelFailedEvent levelFailedEvent)
        {
            HandleCancellation();
            _enemySpawner.ClearPools();
        }
        
        private async UniTask StartSpawningEnemiesInWave(EnemyWaveData enemyWaveData, float enemySpawnInterval)
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
                Debug.LogWarning("Enemy wave spawning task canceled.");
            }
            catch (Exception e)
            {
                Debug.LogError($"An error occurred during enemy spawning: {e.Message}");
            }
        }
        
        private async UniTask SpawnWave(EnemyWaveConfig enemyWaveConfig)
        {
            foreach (var enemyWaveData in enemyWaveConfig.EnemyWaveDataList)
            {
                await StartSpawningEnemiesInWave(enemyWaveData, enemyWaveConfig.SpawnDelayBetweenEachUnit);
            }
        }
        
        private bool IsWaveSpawnReady()
        {
            if (_levelWaveConfig == null || _levelWaveConfig.EnemyWaveConfigList == null)
            {
                Debug.LogError("Level wave config is not available, wave cannot start.");
                return false;
            } 
                
            
            if (_enemySpawner == null)
            {
                Debug.LogError("EnemySpawner is null. Wave cannot start.");
                return false;
            }

            return true;
        }
        
        private void HandleCancellation()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts.Dispose();
            }
            _cts = new CancellationTokenSource();
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
