using System;
using System.Collections.Generic;
using UnityEngine;

namespace SC
{
    public class TasksLogger : MonoBehaviour
    {
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
            _taskManager.EventTaskGroupCompleted += TaskGroupCompletedHandler;
        }

        public void OnDestroy()
        {
            _taskManager.EventStepTaskProcessed -= StepTaskProcessedHandler;
            _taskManager.EventTaskGroupStarted -= TaskGroupStartedHandler;
            _taskManager.EventTaskGroupCompleted -= TaskGroupCompletedHandler;
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
            }

            _currentLogs.Add(new TaskLogData
            {
                timeStamp = timeStamp,
                eventId = eventId,
                eventType = eventType,
                taskStepState = taskStepState,
                triesAmount = tries
            });

            if (_showTriggersDebug)
            {
                Debug.Log($"{timeStamp:HH:mm:ss)} {log}{id} {result}, Попыток: {tries}");
            }
        }

        private void TaskGroupStartedHandler(int id, 
            ExecutionProcedureType procedureType, 
            ScenarioBlockModeType scenarioMode, 
            int examMistakesAmount)
        {
            _currentGroup = new TaskGroupLogData() {
                id = id,
                procedureType = procedureType,
                scenarioModeType = scenarioMode,
                examMistakesMaxAmount = examMistakesAmount,
            };
        }

        private void TaskGroupCompletedHandler(int id)
        {
            _currentGroup.taskLogs = _currentLogs.ToArray();
            _taskGroups.Add(_currentGroup);
            _currentLogs.Clear();
            _currentGroup = default;
        }
    }
}
