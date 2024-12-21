using System.Collections.Generic;
using Gameplay.Towers;
using Interfaces.TowerInterfaces;
using UnityEngine;

namespace Gameplay
{
    public class TowerPlatformController : MonoBehaviour, ITowerPlatformController
    {
        private List<TowerPlatform> _allPlatforms;

        private TowerPlatform _currentlySelectedTowerPlatform;
    
        public void Initialize()
        {
            _allPlatforms = new List<TowerPlatform>(transform.childCount);
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).TryGetComponent(out TowerPlatform towerPlatform))
                {
                    _allPlatforms.Add(towerPlatform);
                }
            }
        }

        public TowerPlatform CheckForTowerAvailability(Vector3 position, out TowerPlatform towerPlatform)
        {
            foreach (TowerPlatform visitedTowerPlatform in _allPlatforms)
            {
                if (Vector3.Distance(position, visitedTowerPlatform.transform.position) < .5f)
                {
                    _currentlySelectedTowerPlatform = visitedTowerPlatform;
                    towerPlatform = _currentlySelectedTowerPlatform;
                    return visitedTowerPlatform;
                }
            }

            towerPlatform = null;
            return null;
        }

        public void PlaceTheTower(BaseTower tower)
        {
            tower.transform.position = _currentlySelectedTowerPlatform.TowerPlacementPoint().position;
        }
    }
}
