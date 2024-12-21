using Gameplay.Towers;
using Interfaces.StateInterfaces;
using UnityEngine;

namespace States.TowerStates
{
    public class TowerAttackState<T> : ITowerState<T> where T : BaseTower
    {
        private Collider[] _detectedEnemies;
        public void Enter(T tower)
        {
            Debug.Log($"{tower.gameObject.name} entered {GetType().Name} state.");
            _detectedEnemies = new Collider[] { };
        }

        public void Execute(T tower)
        {
            if (!CheckForEnemies(tower))
            {
                tower.ChangeToNextState();
                return;
            }
            
            AttackEnemy();
        }

        public void Exit(T tower)
        {
            Debug.Log($"{tower.gameObject.name} exited {GetType().Name} state.");
        }

        private bool CheckForEnemies(T tower)
        {
            return Physics.OverlapSphereNonAlloc(tower.transform.position, tower.TowerCollider.radius, _detectedEnemies, tower.EnemyDetectionLayerMask) > 0;
        }

        private void AttackEnemy()
        {
            
        }
    }
}