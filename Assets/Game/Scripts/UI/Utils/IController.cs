using UnityEngine;

namespace SC
{
    public interface IController
    {
        void Create(Canvas canvas);
        void Init();

        void Show();
        void Hide();
    }
}
