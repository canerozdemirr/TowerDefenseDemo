using System.Collections.Generic;
using Data.Configs.TowerConfigs;
using Interfaces.StateInterfaces;
using States.StateMachines;
using States.TowerStates;
using Systems;
using UnityEngine;

namespace Gameplay.Towers
{
    public class TurretTower : BaseTower
    {
        public override void Initialize(TowerConfig towerConfig)
        {
            base.Initialize(towerConfig);
            var states = new List<ITowerState<BaseTower>>
            {
                new TowerWaitState<BaseTower>(),
                new TowerAttackState<BaseTower>()
            };

            _towerStateMachine = new TowerStateMachine<BaseTower>(states);
            _towerStateMachine.InitializeState(this);
        }
    }
}
