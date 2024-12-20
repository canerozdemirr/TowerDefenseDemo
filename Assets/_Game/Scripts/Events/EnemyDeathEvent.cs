using Gameplay.Enemies;
using Interfaces;

namespace Events
{
    public readonly struct EnemyDeathEvent : IEvent
    {
        public readonly BaseEnemy DeadEnemy;
        public EnemyDeathEvent(BaseEnemy deadEnemy)
        {
            DeadEnemy = deadEnemy;
        }
    }
}
