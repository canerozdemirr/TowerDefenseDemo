using System.Collections.Generic;
using System.Linq;
using Events;
using Gameplay;
using Gameplay.Towers;
using Interfaces;
using Interfaces.TowerInterfaces;
using UnityEngine;
using Utilities.TypeUtilities;

namespace Systems
{
    public class TowerSystem : BaseSystem, ITowerPlacer
    {
        private readonly ITowerPlatformController _towerPlatformController;
        private readonly ITowerSpawner _towerSpawner;
        private readonly IEventDispatcher _eventDispatcher;
        private Dictionary<TowerPlatform, BaseTower> _spawnedTowerList;

        private Enums.TowerType _currentSelectedTowerType;
        
        public TowerSystem(ITowerPlatformController towerPlatformController, ITowerSpawner towerSpawner, IEventDispatcher eventDispatcher)
        {
            _towerPlatformController = towerPlatformController;
            _towerSpawner = towerSpawner;
            _eventDispatcher = eventDispatcher;
        }

        public override void Initialize()
        {
            base.Initialize();
            _towerPlatformController.Initialize();
            _spawnedTowerList = new Dictionary<TowerPlatform, BaseTower>();
            _eventDispatcher.Subscribe<SelectedTowerTypeChangeEvent>(OnTowerTypeSelectionChange);
            _eventDispatcher.Subscribe<LevelFailedEvent>(OnLevelFail);
        }

        public bool TryToPlaceTower(Vector3 touchPosition)
        {
            if (!_towerPlatformController.CheckForTowerAvailability(touchPosition, out TowerPlatform towerPlatform))
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

                ReplaceTower(towerPlatform, _spawnedTowerList[towerPlatform]);
                return true;
            }
            
            PlaceNewTower(towerPlatform);
            return true;
        }
        
        private void ReplaceTower(TowerPlatform platform, BaseTower existingTower)
        {
            _towerSpawner.DeSpawn(existingTower);
            BaseTower newTower = _towerSpawner.Spawn(_currentSelectedTowerType);
            _spawnedTowerList[platform] = newTower;
            _towerPlatformController.PlaceTheTower(newTower);
        }
        
        private void PlaceNewTower(TowerPlatform platform)
        {
            BaseTower newTower = _towerSpawner.Spawn(_currentSelectedTowerType);
            _spawnedTowerList.Add(platform, newTower);
            _towerPlatformController.PlaceTheTower(newTower);
            Debug.Log($"Placed new tower of type '{_currentSelectedTowerType}' on platform '{platform.name}'.");
        }

        private void OnTowerTypeSelectionChange(SelectedTowerTypeChangeEvent selectedTowerTypeChangeEvent)
        {
            _currentSelectedTowerType = selectedTowerTypeChangeEvent.TowerType;
        }

        public override void Dispose()
        {
            base.Dispose();
            _eventDispatcher.Unsubscribe<SelectedTowerTypeChangeEvent>(OnTowerTypeSelectionChange);
            _eventDispatcher.Unsubscribe<LevelFailedEvent>(OnLevelFail);
            _towerSpawner.ClearPools();
        }

        private void OnLevelFail(LevelFailedEvent levelFailedEvent)
        {
            for (int i = 0; i < _spawnedTowerList.Keys.Count; i++)
            {
                TowerPlatform towerPlatform = _spawnedTowerList.Keys.ElementAt(i);
                _towerSpawner.DeSpawn(_spawnedTowerList[towerPlatform]);
            }
            _spawnedTowerList.Clear();
        }
    }
}
