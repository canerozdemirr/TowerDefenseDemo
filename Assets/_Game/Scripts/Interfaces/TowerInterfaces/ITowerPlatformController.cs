using Gameplay.Towers;

namespace Interfaces.TowerInterfaces
{
    public interface ITowerPlatformController
    {
        void Initialize();
        bool CheckForTowerAvailability(TowerPlatform towerPlatform);
        void PlaceTheTower(BaseTower tower);
    }
}
