using System.Collections.Generic;
using Gameplay.Towers;
using Interfaces.StateInterfaces;

namespace States.StateMachines
{
    public class TowerStateMachine<T> where T : BaseTower
    {
        private readonly List<ITowerState<T>> _allStates;
        private ITowerState<T> _currentState;
        
        private int _currentStateIndex;

        public TowerStateMachine(List<ITowerState<T>> states)
        {
            if (states == null || states.Count == 0)
            {
                throw new System.ArgumentException("State list cannot be null or empty.");
            }

            _allStates = states;
            _currentStateIndex = 0;
        }

        public void ChangeToNextState(T tower)
        {
            _currentState?.Exit(tower);
            _currentStateIndex = (_currentStateIndex + 1) % _allStates.Count;
            _currentState = _allStates[_currentStateIndex];
            _currentState.Enter(tower);
        }

        public void UpdateState(T tower)
        {
            _currentState?.Execute(tower);
        }

        public void InitializeState(T tower)
        {
            _currentState = _allStates[_currentStateIndex];
            _currentState.Enter(tower);
        }
    }
}