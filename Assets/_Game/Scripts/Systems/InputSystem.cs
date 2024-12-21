using Events;
using Interfaces;
using Interfaces.TowerInterfaces;
using UnityEngine;
using Zenject;

namespace Systems
{
    public class InputSystem : BaseSystem, ITickable
    {
        private ITowerPlacer _towerPlacer;

        private Camera _mainCamera;

        private bool _isInputAllowed;
        
        private readonly IEventDispatcher _eventDispatcher;
        
        public InputSystem(ITowerPlacer towerPlacer, Camera mainCamera, IEventDispatcher eventDispatcher)
        {
            _towerPlacer = towerPlacer;
            _mainCamera = mainCamera;
            _eventDispatcher = eventDispatcher;
        }

        public override void Initialize()
        {
            base.Initialize();
            _eventDispatcher.Subscribe<LevelStartedEvent>(OnLevelStart);
            _eventDispatcher.Subscribe<LevelFailedEvent>(OnLevelFail);
        }

        public override void Dispose()
        {
            base.Dispose();
            _eventDispatcher.Unsubscribe<LevelStartedEvent>(OnLevelStart);
            _eventDispatcher.Unsubscribe<LevelFailedEvent>(OnLevelFail);
            _isInputAllowed = false;
        }
        
        private void OnLevelFail(LevelFailedEvent levelFailedEvent)
        {
            _isInputAllowed = false;
        }

        private void OnLevelStart(LevelStartedEvent levelStartedEvent)
        {
            _isInputAllowed = true;
        }

        public void Tick()
        {
            if (!_isInputAllowed)
                return;
            
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
                bool hasHit = Physics.Raycast(ray, out RaycastHit hit);
                if (hasHit)
                {
                    if (hit.collider.gameObject.TryGetComponent(out TowerPlatform towerPlatform))
                    {
                        _towerPlacer.TryToPlaceTower(towerPlatform);
                    }
                }
            }
        }
    }
}
