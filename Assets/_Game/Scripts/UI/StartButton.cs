using System;
using Events;
using Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class StartButton : MonoBehaviour
    {
        [Inject] private readonly IEventDispatcher _eventDispatcher;

        [SerializeField] private Button _button;
        [SerializeField] private GameObject _towerSelectionButtons;

        private void OnEnable()
        {
            _eventDispatcher.Subscribe<LevelFailedEvent>(OnLevelFailed);
        }

        private void OnDisable()
        {
            _eventDispatcher.Unsubscribe<LevelFailedEvent>(OnLevelFailed);
        }

        public void OnClick()
        {
            _button.interactable = false;
            _eventDispatcher.Dispatch(new LevelStartedEvent());
            _towerSelectionButtons.SetActive(true);
        }

        private void OnLevelFailed(LevelFailedEvent levelFailedEvent)
        {
            _button.interactable = true;
            _towerSelectionButtons.SetActive(false);
        }
    }
}
