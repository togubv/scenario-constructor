using UnityEngine;
using UnityEngine.UI;

namespace SC
{
    public class ButtonRaiseEvent : MonoBehaviour
    {
        public Button button;
        public EventType eventType;
        public string eventId;

        private void OnEnable()
        {
            button.onClick.AddListener(RaiseEventHandler);
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(RaiseEventHandler);
        }

        private void RaiseEventHandler()
        {
            if (eventType != EventType.None && eventId != string.Empty)
            {
                EventBus.Raise(new GameEvent(eventType, eventId));
            }
        }
    }
}
