using Gameplay.Enemies;
using Interfaces;

namespace Events
{
    public struct EnemyDeathEvent : IEvent
    {
        public readonly BaseEnemy DeadEnemy;
        public EnemyDeathEvent(BaseEnemy deadEnemy)
        {
            DeadEnemy = deadEnemy;
        }
    }
}
