using UnityEngine;

namespace SC
{
    public class GameController : IController
    {
        private GameView _view;

        public void Create(Canvas canvas)
        {
            _view = ObjectFactory.Create<GameView>("UI/Views/GameView/GameView", canvas.transform);
            _view.Hide();
        }

        public void Init()
        {
            _view.Init();
        }

        public void Show()
        {
            _view.Show();
        }

        public void Hide()
        {
            _view.Hide();
        }
    }
}