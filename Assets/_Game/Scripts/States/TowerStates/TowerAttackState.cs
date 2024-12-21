using Gameplay.Towers;
using Interfaces.StateInterfaces;
using UnityEngine;

namespace States.TowerStates
{
    public class TowerAttackState<T> : ITowerState<T> where T : BaseTower
    {
    public void Enter(T tower)
    {
        Debug.Log($"{tower.gameObject.name} entered {GetType().Name} state.");
    }

    public void Execute(T tower)
    {

    }

    public void Exit(T tower)
    {
        Debug.Log($"{tower.gameObject.name} exited {GetType().Name} state.");
    }
    }
}
