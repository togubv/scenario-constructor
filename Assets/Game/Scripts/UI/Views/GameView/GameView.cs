using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SC
{
    public class GameView : IView
    {
        [SerializeField] private ScenarioBlockContainer taskContainerPrefab;
        [SerializeField] private Transform taskContainersParent;
        [SerializeField] private GameObject notificationContainer;
        [SerializeField] private TMP_Text notificationText;
        [SerializeField] private TMP_Text scenarioModeText;

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

        public void ShowNotification(string notifText, float duration = 5f)
        {
            notificationContainer.SetActive(true);
            notificationText.text = notifText;
            StartDelayedHideNotification(duration);
        }

        public void HideNotification()
        {
            notificationContainer.SetActive(false);
        }

        public void StartDelayedHideNotification(float delay)
        {
            if (_delayedNotifHide != null)
            {
                StopCoroutine(_delayedNotifHide);
                _delayedNotifHide = null;
            }

            _delayedNotifHide = StartCoroutine(DelayedHideNotification(delay));
        }

        public void SetScenarioMode(ScenarioBlockModeType mode)
        {
            switch (mode)
            {
                case ScenarioBlockModeType.Training:
                    scenarioModeText.text = $"Тренировка";
                    break;

                case ScenarioBlockModeType.Exam:
                    scenarioModeText.text = $"Экзамен";
                    break;
            }
        }

        private IEnumerator DelayedHideNotification(float delay)
        {
            yield return new WaitForSeconds(delay);

            HideNotification();
        }
    }
}
