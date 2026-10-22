using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SC
{
    public class TasksManager : MonoBehaviour
    {
        public event Action<string, EventType, TaskStepStateType, int> EventStepTaskProcessed;
        public event Action<int, int, ExecutionProcedureType, ScenarioBlockModeType, int> EventTaskGroupStarted;
        public event Action<int> EventScenarioCompleted;

        [SerializeField] private TasksLogger _gameEventsLogger;
        [SerializeField] private bool _showTriggersDebug;
        [SerializeField] private float _nextGroupStartDelay = 1f;
        [SerializeField] private ScenarioPoolData[] _defaultScenarios;

        private List<ScenarioBlockRuntimeData> _activeTasks = new ();
        private ScenarioBlockModeType _blockModeType;
        private TaskStepGroup _stepData;
        private int _currentScenarioIndex;
        private int _queueIndex;
        private int _currentTryAmount;
        private int _mistakesAmount;
        private int _groupIndex;
        private bool _isLoading;

        private void Awake()
        {
            _gameEventsLogger.Init(this);
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(1f);
            
            StartScenario(0);
        }

        private void StartScenario(int scenarioIndex)
        {
            _currentScenarioIndex = scenarioIndex;

            var scenarioData = _defaultScenarios[scenarioIndex];
            SetScenarioMode(scenarioData.modeType);

            StartScenarioGroup(scenarioData, 0);
        }

        private void StartScenarioGroup(ScenarioPoolData scenarioData, int scenarioGroupIndex)
        {
            _groupIndex = scenarioGroupIndex;
            _currentTryAmount = 0;
            _activeTasks.Clear();

            if (scenarioData is ScenarioExamPoolData exam)
            {
                _stepData = exam.stepGroups[scenarioGroupIndex];
            }
            else
            {
                _stepData = scenarioData.stepGroups[scenarioGroupIndex];
            }

            ProcessingStartingGroup(scenarioData);

            if (_stepData.procedureType != ExecutionProcedureType.InRotation)
            {
                _queueIndex = -1;
            }
            else
            {
                _queueIndex = 0;
            }


            for (int i = 0; i < scenarioData.stepGroups[_groupIndex].steps.Length; i++)
            {
                var newRuntimeData = 
                    new ScenarioBlockRuntimeData(scenarioData.stepGroups[_groupIndex].steps[i]);

                _activeTasks.Add(newRuntimeData);
            }

            Menu.Get<GameController>().SetTasksVisual(_activeTasks.ToArray(), _queueIndex);
        }

        private void StartNextStepsGroup()
        {
            var currentScenario = _defaultScenarios[_currentScenarioIndex];

            if (_groupIndex + 1 >= currentScenario.stepGroups.Length)
            {
                EventScenarioCompleted?.Invoke(_currentScenarioIndex);

                if (_currentScenarioIndex + 1 < _defaultScenarios.Length)
                {
                    StartScenario(_currentScenarioIndex + 1);
                }
                else
                {
                    _activeTasks.Clear();
                    Menu.Get<GameController>().ClearTasksVisual();
                }
            }
            else
            {
                _groupIndex += 1;
                StartScenarioGroup(currentScenario, _groupIndex);                
            }
        }

        private void OnEnable()
        {
            EventBus.Subscribe<GameEvent>(
                GameEventProcessing);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<GameEvent>(
                GameEventProcessing);
        }

        private void GameEventProcessing(GameEvent eventData)
        {
            CompleteActiveTasks(eventData);
        }

        private void UpdateActiveTasksVisual(ScenarioBlockRuntimeData runtimeData)
        {
            Menu.Get<GameController>().UpdateTaskState(runtimeData);
        }

        private void ShowNotification(string notifText)
        {
            Menu.Get<GameController>().ShowNotification(notifText);
        }

        private void SetCurrentQueueTask(int queueIndex)
        {
            Menu.Get<GameController>().SetCurrentQueueTask(queueIndex);
        }

        private void SetScenarioMode(ScenarioBlockModeType scenarioMode)
        {
            _blockModeType = scenarioMode;
            Menu.Get<GameController>().SetScenarioMode(_blockModeType);
        }

        private void ProcessingStartingGroup(ScenarioPoolData scenarioData)
        {
            if (scenarioData.stepGroups is TaskStepGroupExam[] groupExam)
            {
                EventTaskGroupStarted?.Invoke(
                    1,
                    scenarioData.id,
                    _stepData.procedureType,
                    ScenarioBlockModeType.Exam,
                    groupExam[_groupIndex].examMistakesMaxAmount);
            }
            else
            {
                EventTaskGroupStarted?.Invoke(
                    1,
                    scenarioData.id,
                    _stepData.procedureType,
                    ScenarioBlockModeType.Training,
                    0);
            }
        }

        private void CompleteActiveTasks(GameEvent eventData)
        {
            if (_activeTasks == null || _activeTasks.Count == 0)
            {
                return;
            }

            bool hasMistake = true;

            for (int i = 0; i < _activeTasks.Count; i++)
            {
                if (_activeTasks[i].task.conditionType == eventData.eventType)
                {
                    //if (_activeTasks[i].state != TaskStepStateType.None)
                    //{
                    //    continue;
                    //}

                    string taskEventId = _activeTasks[i].task.eventId;

                    if (_stepData.procedureType == ExecutionProcedureType.Free &&
                        taskEventId == eventData.id)
                    {
                        hasMistake = false;
                        SetTaskState(_activeTasks[i], TaskStepStateType.Success);
                    }
                    else if (_stepData.procedureType == ExecutionProcedureType.InRotation)
                    {
                        if (taskEventId == eventData.id)
                        {
                            if (i == _queueIndex)
                            {
                                hasMistake = false;
                                SetTaskState(_activeTasks[i], TaskStepStateType.Success);
                                _queueIndex += 1;
                                _currentTryAmount = 0;
                                SetCurrentQueueTask(_queueIndex);
                            }
                            else
                            {
                                hasMistake = false;
                                AddMistakeAmount();
                                ShowNotification("ÍÅ ÏÎ Î×ÅÐÅÄÈ!");
                            }
                        }
                    }
                }
            }

            if (hasMistake)
            {
                AddMistakeAmount();
                ShowNotification("ÎØÈÁÊÀ!");
            }

            if (CurrentGroupIsDone())
            {
                if (_isLoading == false)
                {
                    StartCoroutine(DelayedStartNextStepsGroup());
                }
            }
        }

        private void AddMistakeAmount()
        {
            if (_blockModeType == ScenarioBlockModeType.Exam)
            {
                SetTaskState(_activeTasks[_queueIndex], TaskStepStateType.Failed);
                _queueIndex += 1;
                _currentTryAmount = 0;
                SetCurrentQueueTask(_queueIndex);
                AddMistakesAmount();
            }
            else
            {
                AddTryAmount();
            }
        }

        private void SetTaskState(ScenarioBlockRuntimeData activeTask, TaskStepStateType state)
        {
            activeTask.state = state;
            UpdateActiveTasksVisual(activeTask);

            if (state != TaskStepStateType.None)
            {
                EventStepTaskProcessed?.Invoke(
                    activeTask.task.eventId,
                    activeTask.task.conditionType,
                    activeTask.state,
                    _currentTryAmount
                    );
            }

            _currentTryAmount = 0;
        }

        private void AddTryAmount()
        {
            _currentTryAmount += 1;
        }

        private bool CurrentGroupIsDone()
        {
            return GetDoneStepsAmount() >= _stepData.steps.Length;
        }

        private int GetDoneStepsAmount()
        {
            int amount = 0;

            for (int i = 0; i < _activeTasks.Count; i++)
            {
                if (_activeTasks[i].state != TaskStepStateType.None)
                {
                    amount += 1;
                }
            }

            return amount;
        }

        private void AddMistakesAmount()
        {
            _mistakesAmount += 1;

            if (_stepData is TaskStepGroupExam examSteps)
            {
                if (_mistakesAmount >= examSteps.examMistakesMaxAmount)
                {
                    for (int i = 0; i < _activeTasks.Count; i++)
                    {
                        if (_activeTasks[i].state == TaskStepStateType.None)
                        {
                            SetTaskState(_activeTasks[i], TaskStepStateType.Skipped);
                        }
                    }

                    SetCurrentQueueTask(-1);
                    ShowNotification("ÍÅÓÄÀ×À!");

                    if (_isLoading == false)
                    {
                        StartCoroutine(DelayedStartNextStepsGroup());
                    }
                }
            }
        }

        private IEnumerator DelayedStartNextStepsGroup()
        {
            _isLoading = true;

            yield return new WaitForSeconds(_nextGroupStartDelay);

            StartNextStepsGroup();
            _isLoading = false;
        }
    }
}
