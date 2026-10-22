using UnityEngine;
using UnityEngine.Events;

namespace SC
{
    public class InteractableContainer : MonoBehaviour
    {
        [SerializeField] private UnityEvent EventInteractableComplete;

        public void CompleteInteractable()
        {
            EventInteractableComplete?.Invoke();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                CompleteInteractable();
            }
        }
    }
}