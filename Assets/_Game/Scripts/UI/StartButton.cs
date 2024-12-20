using Events;
using UnityEngine;

namespace UI
{
    public class StartButton : MonoBehaviour
    {
        public void OnClick()
        {
            EventDispatcher.Instance.Dispatch(new LevelStartedEvent());
        }

        private void SetButtonVisible()
        {
            
        }
    }
}
