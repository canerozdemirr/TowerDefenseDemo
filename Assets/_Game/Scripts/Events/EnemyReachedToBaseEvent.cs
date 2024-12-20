using Interfaces;

namespace Events
{
    public struct EnemyReachedToBaseEvent : IEvent
    {
        public int EnemyDamage;

        public EnemyReachedToBaseEvent(int enemyDamage)
        {
            EnemyDamage = enemyDamage;
        }
    }
}
