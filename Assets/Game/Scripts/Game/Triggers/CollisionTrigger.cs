using UnityEngine;

namespace SC
{
    public class CollisionTrigger : MonoBehaviour
    {
        [SerializeField] private string id;
        [SerializeField] private bool registerExit = false;

        private void OnCollisionEnter(Collision other)
        {
            if (other.collider.CompareTag("Player"))
            {
                EventBus.Raise(new GameEvent(EventType.EnterCollision, id));
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (registerExit == false)
            {
                return;
            }

            if (other.collider.CompareTag("Player"))
            {
                EventBus.Raise(new GameEvent(EventType.ExitCollision, id));
            }
        }
    }
}
