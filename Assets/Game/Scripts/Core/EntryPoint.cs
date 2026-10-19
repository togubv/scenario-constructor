using System.Collections;
using UnityEngine;

namespace SC
{
    [DefaultExecutionOrder(-9999)]
    public class EntryPoint : MonoBehaviour
    {
        public Canvas canvas;
        public PlayerController playerController;

        private Game _game;

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            _game = new Game();
            _game.Init(canvas, playerController);
        }

        private IEnumerator Start()
        {
            yield return null;

            var test = Settings.Get<ScenarioBlocksPool>().GetScenarioBlock(0);
            Debug.Log(test.name);
        }

        private void OnDestroy()
        {
            _game.OnDestroy();
        }
    }
}
