using Gameplay.Enemies;
using Gameplay.Towers;
using Utilities.TypeUtilities;

namespace Interfaces
{
    public interface ITowerSpawner
    {
        BaseTower Spawn(Enums.TowerType towerType);
        void DeSpawn(BaseTower tower);
        void ClearPools();
    }
}
