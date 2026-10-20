using UnityEngine;

namespace SC
{
    public class EventsListener : MonoBehaviour
    {
        private void OnEnable()
        {
            EventBus.Subscribe<GameEvent>(
                PlayerEnterColliderEventHandler);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<GameEvent>(
                PlayerEnterColliderEventHandler);
        }

        private void PlayerEnterColliderEventHandler(
            GameEvent eventData)
        {

            switch (eventData.eventType)
            {
                case EventType.EnterCollider:
                    Debug.Log($"Player entered the collider with id {eventData.id}");
                    break;

                case EventType.ExitCollider:
                    Debug.Log($"Player exited the collider with id {eventData.id}");
                    break;

                case EventType.EnterCollision:
                    Debug.Log($"Player start collision the collider with id {eventData.id}");
                    break;

                case EventType.ExitCollision:
                    Debug.Log($"Player end collision the collider with id {eventData.id}");
                    break;

                case EventType.ButtonClicked:
                    Debug.Log($"Button clicked with id {eventData.id}");
                    break;
            }
            
        }
    }
}
