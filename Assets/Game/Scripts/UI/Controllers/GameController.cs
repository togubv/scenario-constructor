using System;
using System.Collections;
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

        public void SetTasksVisual(ScenarioBlockRuntimeData[] tasks, int queueIndex = -1)
        {
            _view.SetTasksVisual(tasks, queueIndex);
        }

        public void SetCurrentQueueTask(int queueIndex)
        {
            _view.SetCurrentQueueTask(queueIndex);
        }

        public void UpdateTaskState(ScenarioBlockRuntimeData task)
        {
            _view.UpdateTaskState(task);
        }

        public void ShowNotification(string notifText, float duration = 5f)
        {
            _view.ShowNotification(notifText, duration);
        }

        public void SetScenarioMode(ScenarioBlockModeType mode)
        {
            _view.SetScenarioMode(mode);
        }
    }
}