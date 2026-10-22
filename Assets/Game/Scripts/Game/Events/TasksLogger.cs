using System;
using System.Collections.Generic;
using UnityEngine;

namespace SC
{
    public class TasksLogger : MonoBehaviour
    {
        [SerializeField] private ResultAnalyzer _resultAnalyzer;
        [SerializeField] private bool _showTriggersDebug;

        private TasksManager _taskManager;

        private TaskGroupLogData _currentGroup;

        private List<TaskGroupLogData> _taskGroups = new();
        private List<TaskLogData> _currentLogs = new();

        public void Init(TasksManager tasksManager)
        {
            _taskManager = tasksManager;

            _taskManager.EventStepTaskProcessed += StepTaskProcessedHandler;
            _taskManager.EventTaskGroupStarted += TaskGroupStartedHandler;
            _taskManager.EventScenarioCompleted += ScenarioCompletedHandler;
            Menu.Get<GameController>().EventRecalculateAnalyze += RecalculateAnalyzeHandler;
        }

        public void OnDestroy()
        {
            _taskManager.EventStepTaskProcessed -= StepTaskProcessedHandler;
            _taskManager.EventTaskGroupStarted -= TaskGroupStartedHandler;
            _taskManager.EventScenarioCompleted -= ScenarioCompletedHandler;

            if (Menu.Get<GameController>() != null)
            {
                Menu.Get<GameController>().EventRecalculateAnalyze -= RecalculateAnalyzeHandler;
            }
        }

        private void StepTaskProcessedHandler(string eventId, 
            EventType eventType, TaskStepStateType taskStepState, int tries)
        {
            DateTime timeStamp = DateTime.Now;
            string log = string.Empty;
            string id = $", id: {eventId}";
            string result = $"[{taskStepState}]";

            switch (eventType)
            {
                case EventType.EnterCollider:
                    log = $"Вход в Trigger";
                    break;

                case EventType.ExitCollider:
                    log = $"Выход из Trigger";
                    break;

                case EventType.EnterCollision:
                    log = $"Начало контакта с Collider";
                    break;

                case EventType.ExitCollision:
                    log = $"Конец контакта с Collider";
                    break;

                case EventType.ButtonClicked:
                    log = $"Нажатие кнопки";
                    break;

                case EventType.UnityEvent:
                    log = $"Кастомное событие";
                    break;
            }

            _currentLogs.Add(new TaskLogData
            {
                timeStamp = timeStamp,
                eventId = eventId,
                description = log,
                eventType = eventType,
                taskStepState = taskStepState,
                triesAmount = tries
            });

            if (_showTriggersDebug)
            {
                Debug.Log($"{timeStamp:HH:mm:ss)} {log}{id} {result}, Попыток: {tries}");
            }
        }

        private void TaskGroupStartedHandler(int contractVersion, int id, 
            ExecutionProcedureType procedureType, 
            ScenarioBlockModeType scenarioMode, 
            int examMistakesAmount)
        {
            if (contractVersion == 1)
            {
                _currentGroup = new TaskGroupLogData()
                {
                    contractVersion = contractVersion,
                    id = id,
                    procedureType = procedureType,
                    scenarioModeType = scenarioMode,
                    examMistakesMaxAmount = examMistakesAmount,
                };
            }
            else
            {
                Debug.LogWarning("Not found required contract version!");
            }
        }

        private void ScenarioCompletedHandler(int id)
        {
            _currentGroup.taskLogs = _currentLogs.ToArray();

            if (_resultAnalyzer != null)
            {
                _resultAnalyzer.AnalyzeLogs(_currentGroup);
            }

            _taskGroups.Add(_currentGroup);
            _currentLogs.Clear();
        }

        private void RecalculateAnalyzeHandler()
        {
            if (_resultAnalyzer != null)
            {
                if (_taskGroups != null && _taskGroups.Count > 0)
                {
                    _resultAnalyzer.AnalyzeLogs(_taskGroups[^1]);
                }
            }
        }
    }
}
