using UnityEngine;

namespace SC
{
    public class ColliderTrigger : MonoBehaviour
    {
        public string id;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                EventBus.Raise(new GameEvent(EventType.EnterCollider, id));
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                EventBus.Raise(new GameEvent(EventType.ExitCollider, id));
            }
        }
    }
}
