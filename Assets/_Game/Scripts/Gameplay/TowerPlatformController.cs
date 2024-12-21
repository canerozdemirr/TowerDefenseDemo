using System.Collections.Generic;
using Interfaces.TowerInterfaces;
using UnityEngine;

namespace Gameplay
{
    public class TowerPlatformController : MonoBehaviour, ITowerPlatformController
    {
        private HashSet<TowerPlatform> _allPlatforms;
    
        public void Initialize()
        {
            _allPlatforms = new HashSet<TowerPlatform>(transform.childCount);
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).TryGetComponent(out TowerPlatform towerPlatform))
                {
                    _allPlatforms.Add(towerPlatform);
                }
            }
        }

        public bool CheckForTowerAvailability(TowerPlatform towerPlatform)
        {
            if (!_allPlatforms.Contains(towerPlatform))
            {
                Debug.LogError("That platform is not available");
                return false;
            }

            return true;
        }
    }
}
