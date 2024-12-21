using Gameplay;
using Gameplay.Towers;
using UnityEngine;

namespace Interfaces.TowerInterfaces
{
    public interface ITowerPlatformController
    {
        void Initialize();
        TowerPlatform CheckForTowerAvailability(Vector3 touchPosition, out TowerPlatform towerPlatform);
        void PlaceTheTower(BaseTower tower);
    }
}
