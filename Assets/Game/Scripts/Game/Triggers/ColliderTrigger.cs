using UnityEngine;

namespace SC
{
    public class ColliderTrigger : MonoBehaviour
    {
        [SerializeField] private string id;
        [SerializeField] private bool registerExit = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                EventBus.Raise(new GameEvent(EventType.EnterCollider, id));
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (registerExit == false)
            {
                return;
            }

            if (other.CompareTag("Player"))
            {
                EventBus.Raise(new GameEvent(EventType.ExitCollider, id));
            }
        }
    }
}
