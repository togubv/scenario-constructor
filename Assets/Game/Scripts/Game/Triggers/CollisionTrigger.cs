using UnityEngine;

namespace SC
{
    public class CollisionTrigger : MonoBehaviour
    {
        public string id;

        private void OnCollisionEnter(Collision other)
        {
            if (other.collider.CompareTag("Player"))
            {
                EventBus.Raise(new GameEvent(EventType.EnterCollision, id));
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.collider.CompareTag("Player"))
            {
                EventBus.Raise(new GameEvent(EventType.ExitCollision, id));
            }
        }
    }
}
