using Events;
using Interfaces;
using UnityEngine;
using Zenject;

namespace UI
{
    public class StartButton : MonoBehaviour
    {
        [Inject] private readonly IEventDispatcher _eventDispatcher;
        
        public void OnClick()
        {
            _eventDispatcher.Dispatch(new LevelStartedEvent());
        }

        private void SetButtonVisible()
        {
            
        }
    }
}
