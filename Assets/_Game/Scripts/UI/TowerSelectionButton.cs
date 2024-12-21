using System;
using System.Text;
using Events;
using Interfaces;
using TMPro;
using UnityEngine;
using Utilities.TypeUtilities;
using Zenject;

namespace UI
{
    public class TowerSelectionButton : MonoBehaviour
    {
        private IEventDispatcher _eventDispatcher;
        
        [SerializeField] 
        private Enums.TowerType _towerType;

        [SerializeField]
        private TextMeshProUGUI _towerTypeText;

        [Inject]
        public void Inject(IEventDispatcher eventDispatcher)
        {
            _eventDispatcher = eventDispatcher;
        }

        private void OnEnable()
        {
            _towerTypeText.SetText(_towerType.ToString());
        }

        public void OnClick()
        {
            _eventDispatcher.Dispatch(new SelectedTowerTypeChangeEvent(_towerType));
        }
    }
}
