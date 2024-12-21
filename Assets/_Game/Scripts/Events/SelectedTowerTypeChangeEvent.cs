using Interfaces;
using Utilities.TypeUtilities;

namespace Events
{
    public struct SelectedTowerTypeChangeEvent : IEvent
    {
        public Enums.TowerType TowerType;

        public SelectedTowerTypeChangeEvent(Enums.TowerType towerType)
        {
            TowerType = towerType;
        }
    }
}
