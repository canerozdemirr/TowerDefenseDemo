using Gameplay.Towers;
using Interfaces.StateInterfaces;
using UnityEngine;

namespace States.TowerStates
{
    public class TowerWaitState<T> : ITowerState<T> where T : BaseTower
    {
        private Collider[] _detectedEnemies;
        private bool _enemyFound;
        public void Enter(T tower)
        {
            Debug.Log($"{tower.gameObject.name} entered {GetType().Name} state.");
            tower.SetupTowerCollider();
            _detectedEnemies = new Collider[1];
        }

        public void Execute(T tower)
        {
            _enemyFound = Physics.OverlapSphereNonAlloc(tower.transform.position, tower.TowerCollider.radius, _detectedEnemies, tower.EnemyDetectionLayerMask) > 0;

            if (!_enemyFound) 
                return;
            
            Debug.Log($"Enemy detected within range of {tower.gameObject.name}.");
            tower.ChangeToNextState();
        }

        public void Exit(T tower)
        {
            Debug.Log($"{tower.gameObject.name} exited {GetType().Name} state.");
        }
    }
}
