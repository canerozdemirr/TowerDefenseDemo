using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlatform : MonoBehaviour
{
    [SerializeField] private Transform _towerPlacementPoint;
    public Transform TowerPlacementPoint() => _towerPlacementPoint;
}
