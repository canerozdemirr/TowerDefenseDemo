namespace Interfaces.TowerInterfaces
{
    public interface ITowerPlatformController
    {
        void Initialize();
        bool CheckForTowerAvailability(TowerPlatform towerPlatform);
    }
}
