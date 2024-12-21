using Gameplay.Enemies;
using Utilities.TypeUtilities;

namespace Interfaces
{
    public interface IEnemySpawner
    {
        void Spawn(Enums.EnemyType enemyType);
        void DeSpawn(BaseEnemy enemy);
        void ClearPools();
    }
}
