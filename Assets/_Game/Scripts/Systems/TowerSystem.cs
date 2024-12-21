using System.Collections.Generic;
using Data.Configs.TowerConfigs;
using Gameplay.Towers;
using Interfaces;
using Interfaces.TowerInterfaces;
using Systems;
using UnityEngine;
using Utilities.TypeUtilities;

namespace Systems
{
    public class TowerSystem : BaseSystem, ITowerPlacer
    {
        private readonly TowerConfigList _towerConfigList;
        private readonly ITowerPlatformController _towerPlatformController;
        private readonly ITowerSpawner _towerSpawner;
        private Dictionary<TowerPlatform, BaseTower> _spawnedTowerList;

        private Enums.TowerType _currentSelectedTowerType;
        
        public TowerSystem(TowerConfigList towerConfigList, ITowerPlatformController towerPlatformController, ITowerSpawner towerSpawner)
        {
            _towerConfigList = towerConfigList;
            _towerPlatformController = towerPlatformController;
            _towerSpawner = towerSpawner;
        }

        public override void Initialize()
        {
            base.Initialize();
            _towerPlatformController.Initialize();
            _spawnedTowerList = new Dictionary<TowerPlatform, BaseTower>();
        }

        public bool TryToPlaceTower(TowerPlatform towerPlatform)
        {
            if (!_towerPlatformController.CheckForTowerAvailability(towerPlatform))
            {
                Debug.LogError("This tower platform is not defined within the available platforms! You can't spawn a tower in that platform!");
                return false;
            }

            if (_spawnedTowerList.ContainsKey(towerPlatform))
            {
                if (_spawnedTowerList[towerPlatform].TowerType == _currentSelectedTowerType)
                {
                    Debug.LogError("This tower platform is already has the same tower type!");
                    return false;
                }

                _towerSpawner.DeSpawn(_spawnedTowerList[towerPlatform]);
                BaseTower replacedTower = _towerSpawner.Spawn(_currentSelectedTowerType);
                _spawnedTowerList[towerPlatform] = replacedTower;
                _towerPlatformController.PlaceTheTower(replacedTower);
                return true;
            }
            
            BaseTower newTower = _towerSpawner.Spawn(_currentSelectedTowerType);
            _spawnedTowerList.Add(towerPlatform, newTower);
            _towerPlatformController.PlaceTheTower(newTower);
            return true;
        }
    }
}
