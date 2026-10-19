using UnityEngine;

namespace SC
{
    public class Game
    {
        private Canvas _canvas;
        private Settings _settings;
        private InputReader _inputReader;
        private Menu menu;

        public void Init(Canvas canvas, PlayerController playerController)
        {
            _canvas = canvas;

            _settings = new Settings();
            _settings.InitializeSettings();

            _inputReader = new InputReader();
            _inputReader.Init();

            playerController.Init(_inputReader);
            playerController.enabled = true;

            CreateMenu();
        }

        public void OnDestroy()
        {
            if (_inputReader != null)
            {
                _inputReader.Dispose();
            }

            menu?.Dispose();
        }

        private void CreateMenu()
        {
            menu?.Dispose();

            menu = new Menu();

            menu.Create<GameController>(_canvas);

            Menu.Get<GameController>().Show();
        }
    }
}
