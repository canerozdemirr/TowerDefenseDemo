using UnityEngine;

namespace Gameplay
{
    public class TowerPlatform : MonoBehaviour
    {
        [SerializeField] private Transform _towerPlacementPoint;
        public Transform TowerPlacementPoint() => _towerPlacementPoint;
    }
}
