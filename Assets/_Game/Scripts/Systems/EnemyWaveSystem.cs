using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Data.Configs;
using Events;
using Gameplay;
using Gameplay.Enemies;
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
        private List<BaseEnemy> _spawnedEnemies;

        public EnemyWaveSystem(LevelWaveConfig levelWaveConfig, IEnemySpawner enemySpawner,
            IEventDispatcher eventDispatcher)
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
            _eventDispatcher.Subscribe<EnemyDeathEvent>(OnEnemyDeath);
            _cts = new CancellationTokenSource();
            _spawnedEnemies = new List<BaseEnemy>();
        }

        public override void Dispose()
        {
            base.Dispose();
            _eventDispatcher.Unsubscribe<LevelStartedEvent>(OnLevelStart);
            _eventDispatcher.Unsubscribe<LevelFailedEvent>(OnLevelFail);
            _eventDispatcher.Unsubscribe<EnemyDeathEvent>(OnEnemyDeath);
            HandleCancellation();
            _enemySpawner.ClearPools();
        }

        private void OnEnemyDeath(EnemyDeathEvent enemyDeathEvent)
        {
            _spawnedEnemies.Remove(enemyDeathEvent.DeadEnemy);
        }

        private async void OnLevelStart(LevelStartedEvent levelStartedEvent)
        {
            try
            {
                while (!_cts.Token.IsCancellationRequested)
                {
                    for (int i = 0; i < _levelWaveConfig.EnemyWaveConfigList.Count; i++)
                    {
                        EnemyWaveConfig enemyWaveConfig = _levelWaveConfig.EnemyWaveConfigList[i];
                        _eventDispatcher.Dispatch(new WaveStartedEvent(i + 1));
                        await SpawnWave(enemyWaveConfig);
                        await UniTask.Delay((int) (_levelWaveConfig.DelayBetweenWaves * 1000),
                            cancellationToken: _cts.Token);
                    }
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
            foreach (BaseEnemy enemy in _spawnedEnemies)
            {
                _enemySpawner.DeSpawn(enemy);
            }
        }

        private async UniTask StartSpawningEnemiesInWave(EnemyWaveData enemyWaveData, float enemySpawnInterval)
        {
            for (int i = 0; i < enemyWaveData.enemyAmount; i++)
            {
                BaseEnemy spawnedEnemy = _enemySpawner.Spawn(enemyWaveData.enemyType);
                _spawnedEnemies.Add(spawnedEnemy);
                await UniTask.Delay((int) (enemySpawnInterval * 1000), cancellationToken: _cts.Token);
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
            if (_cts is {IsCancellationRequested: false})
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