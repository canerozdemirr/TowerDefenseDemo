using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Events;
using Gameplay.Enemies;
using Gameplay.Projectiles;
using Gameplay.Towers;
using Interfaces.StateInterfaces;
using UnityEngine;

namespace States.TowerStates
{
    public class TowerAttackState<T> : ITowerState<T> where T : BaseTower
    {
        private BaseTower _tower;
        private CancellationTokenSource _cts;
        public void Enter(T tower)
        {
            Debug.Log($"{tower.gameObject.name} entered {GetType().Name} state.");
            _cts = new CancellationTokenSource();
            _tower = tower;
            _tower.EventDispatcher.Subscribe<LevelFailedEvent>(OnLevelFailed);
            AttackEnemy();
        }

        public void Execute(T tower)
        {
            if (!CheckForEnemies())
            {
                tower.ChangeToNextState();
            }
        }

        public void Exit(T tower)
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _tower.EventDispatcher.Unsubscribe<LevelFailedEvent>(OnLevelFailed);
            Debug.Log($"{tower.gameObject.name} exited {GetType().Name} state.");
        }

        private void OnLevelFailed(LevelFailedEvent levelFailedEvent)
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }

        private bool CheckForEnemies()
        {
            return _tower.EnemyListInRange.Count > 0;
        }

        private void AttackEnemy()
        {
            _ = FiringRoutine();
        }

        private async UniTask FiringRoutine()
        {
            try
            {
                while (!_cts.IsCancellationRequested)
                {
                    BaseProjectile projectile = _tower.ProjectileSpawner.Spawn(_tower.ProjectileConfig.projectileType);
                    if (projectile == null)
                    {
                        Debug.LogError("Failed to spawn projectile.");
                        return;
                    }

                    BaseEnemy targetEnemy = null;
                    for (int i = 0; i < _tower.EnemyListInRange.Count; i++)
                    {
                        if (_tower.EnemyListInRange[i] != null)
                        {
                            targetEnemy = _tower.EnemyListInRange[i];
                            break;
                        }
                    }

                    if (targetEnemy != null)
                    {
                        projectile.transform.position = _tower.ProjectileSpawnPoint.position;
                        projectile.PrepareProjectile(_tower.EnemyDetectionLayerMask, _tower.ProjectileConfig.speed, _tower.TowerBaseDamage);
                        projectile.Launch(targetEnemy.transform.position);
                    }
                    
                    await UniTask.Delay((int)(_tower.DelayBetweenEachFiring * 1000), cancellationToken: _cts.Token);
                }
            }
            catch (OperationCanceledException)
            {
                Debug.LogWarning("Tower attacking canceled.");
            }
            catch (Exception e)
            {
                Debug.LogError($"An error occurred during tower attacking: {e.Message}");
            }
        }
    }
}