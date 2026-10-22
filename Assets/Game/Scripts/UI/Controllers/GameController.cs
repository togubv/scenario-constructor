using System;
using UnityEngine;

namespace SC
{
    public class GameController : IController
    {
        public event Action EventRecalculateAnalyze;

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
            AddHandlers();
        }

        public void Hide()
        {
            RemoveHandlers();
            _view.Hide();
        }

        public void SetTasksVisual(ScenarioBlockRuntimeData[] tasks, int queueIndex = -1)
        {
            _view.SetTasksVisual(tasks, queueIndex);
        }

        public void ClearTasksVisual()
        {
            _view.ClearTasksVisual();
        }

        public void SetCurrentQueueTask(int queueIndex)
        {
            _view.SetCurrentQueueTask(queueIndex);
        }

        public void UpdateTaskState(ScenarioBlockRuntimeData task)
        {
            _view.UpdateTaskState(task);
        }

        public void ShowNotification(string notifText)
        {
            _view.ShowNotification(notifText);
        }

        public void SetScenarioMode(ScenarioBlockModeType mode)
        {
            _view.SetScenarioMode(mode);
        }

        public void SetResult(string result, ScenarioBlockModeType mode)
        {
            _view.SetResult(result, mode);
        }

        private void RecalculateAnalyzeHandler()
        {
            EventRecalculateAnalyze?.Invoke();
        }

        private void AddHandlers()
        {
            _view.EventRecalculateAnalyze += RecalculateAnalyzeHandler;
        }

        private void RemoveHandlers()
        {
            _view.EventRecalculateAnalyze -= RecalculateAnalyzeHandler;
        }
    }
}