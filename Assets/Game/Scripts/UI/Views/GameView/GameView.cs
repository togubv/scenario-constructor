using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SC
{
    public class GameView : IView
    {
        public event Action EventRecalculateAnalyze;

        [SerializeField] private ScenarioBlockContainer taskContainerPrefab;
        [SerializeField] private Transform taskContainersParent;
        [SerializeField] private GameObject notificationContainer;
        [SerializeField] private TMP_Text notificationText;
        [SerializeField] private TMP_Text scenarioModeText;
        [SerializeField] private Button recalculateAnalyzeButton;
        [SerializeField] private float notificationDuration = 2f;
        [Header("Result")]
        [SerializeField] private GameObject resultContainer;
        [SerializeField] private TMP_Text resultModeText;
        [SerializeField] private TMP_Text resultText;
        [SerializeField] private Button resultCloseButton;

        private List<ScenarioBlockContainer> _activeTaskContainers = new ();

        private Coroutine _delayedNotifHide;

        public void SetTasksVisual(ScenarioBlockRuntimeData[] tasks, int queueIndex = -1)
        {
            ClearTasksVisual();

            for (int i = 0; i < tasks.Length; i++)
            {
                var spawned = Instantiate(taskContainerPrefab, taskContainersParent);
                spawned.Init(tasks[i].task);
                
                if (queueIndex >= 0)
                {
                    spawned.ToggleAvailableState(i == queueIndex);
                }

                _activeTaskContainers.Add(spawned);
            }
        }

        public void ClearTasksVisual()
        {
            if (_activeTaskContainers.Count < 1)
            {
                return;
            }

            foreach (var a in _activeTaskContainers)
            {
                Destroy(a.gameObject);
            }

            _activeTaskContainers.Clear();
        }

        public void SetCurrentQueueTask(int queueIndex)
        {
            for (int i = 0; i < _activeTaskContainers.Count; i++)
            {
                _activeTaskContainers[i].ToggleAvailableState(i == queueIndex);
            }
        }

        public void UpdateTaskState(ScenarioBlockRuntimeData task)
        {
            foreach (var a in _activeTaskContainers)
            {
                if (a.blockData == task.task)
                {
                    a.UpdateState(task);
                }
            }
        }

        public void ShowNotification(string notifText)
        {
            notificationContainer.SetActive(true);
            notificationText.text = notifText;
            StartDelayedHideNotification();
        }

        public void HideNotification()
        {
            notificationContainer.SetActive(false);
        }

        public void StartDelayedHideNotification()
        {
            if (_delayedNotifHide != null)
            {
                StopCoroutine(_delayedNotifHide);
                _delayedNotifHide = null;
            }

            _delayedNotifHide = StartCoroutine(DelayedHideNotification());
        }

        public void SetScenarioMode(ScenarioBlockModeType mode)
        {
            scenarioModeText.text = GetModeTitle(mode);
        }

        public void SetResult(string result, ScenarioBlockModeType mode)
        {
            resultContainer.SetActive(true);
            resultText.text = result;
            resultModeText.text = GetModeTitle(mode);
        }

        public void CloseResultContainer()
        {
            resultContainer.SetActive(false);
        }

        private void RecalculateAnalyzeHandler()
        {
            EventRecalculateAnalyze?.Invoke();
        }

        private void ResultCloseButtonHandler()
        {
            CloseResultContainer();
        }

        protected override void AddHandlers()
        {
            base.AddHandlers();

            recalculateAnalyzeButton.onClick.AddListener(RecalculateAnalyzeHandler);
            resultCloseButton.onClick.AddListener(ResultCloseButtonHandler);
        }

        protected override void RemoveHandlers()
        {
            base.RemoveHandlers();

            recalculateAnalyzeButton.onClick.RemoveListener(RecalculateAnalyzeHandler);
            resultCloseButton.onClick.RemoveListener(ResultCloseButtonHandler);
        }

        private IEnumerator DelayedHideNotification()
        {
            yield return new WaitForSeconds(notificationDuration);

            HideNotification();
        }

        private string GetModeTitle(ScenarioBlockModeType mode)
        {
            switch (mode)
            {
                case ScenarioBlockModeType.Training:
                    return $"Тренировка";

                case ScenarioBlockModeType.Exam:
                    return $"Экзамен";

                default:
                    return string.Empty;
            }
        }
    }
}
