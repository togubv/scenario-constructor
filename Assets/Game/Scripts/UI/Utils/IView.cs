using UnityEngine;

namespace SC
{
    public class IView : MonoBehaviour
    {
        public virtual void Init()
        {

        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
            AddHandlers();
        }

        public virtual void Hide()
        {
            RemoveHandlers();
            gameObject.SetActive(false);
        }

        protected virtual void AddHandlers()
        {

        }

        protected virtual void RemoveHandlers()
        {

        }
    }
}
