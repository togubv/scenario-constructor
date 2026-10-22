using UnityEngine;

namespace SC
{
    public class ScenarioEventReceiver : MonoBehaviour
    {
        public void CallCustomEvent(string eventId)
        {
            EventBus.Raise(new GameEvent(EventType.UnityEvent, eventId));
        }
    }
}