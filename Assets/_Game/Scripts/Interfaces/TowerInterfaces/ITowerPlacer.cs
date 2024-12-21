using Gameplay;
using UnityEngine;

namespace Interfaces.TowerInterfaces
{
    public interface ITowerPlacer
    {
        bool TryToPlaceTower(Vector3 touchPosition);
    }
}
