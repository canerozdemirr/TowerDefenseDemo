using Gameplay.Towers;
using Interfaces.StateInterfaces;
using UnityEngine;

namespace States.TowerStates
{
    public class TowerWaitState<T> : ITowerState<T> where T : BaseTower
    {
        public void Enter(T tower)
        {
            Debug.Log($"{tower.gameObject.name} entered {GetType().Name} state.");
            tower.SetupTowerCollider();
        }

        public void Execute(T tower)
        {
            if (tower.EnemyListInRange.Count == 0) 
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
