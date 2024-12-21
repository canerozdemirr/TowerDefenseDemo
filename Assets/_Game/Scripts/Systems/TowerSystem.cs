using Interfaces.TowerInterfaces;
using Systems;

namespace Systems
{
    public class TowerSystem : BaseSystem, ITowerPlacer
    {
        public bool TryToPlaceTower(TowerPlatform towerPlatform)
        {
            return true;
        }
    }
}
