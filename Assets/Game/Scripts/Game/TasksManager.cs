using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SC
{
    public class TasksManager : MonoBehaviour
    {
        private List<ScenarioBlockRuntimeData> _activeTasks = new ();
        private ScenarioBlockModeType _blockModeType;
        private TaskStepGroup _stepData;
        private int _currentScenarioIndex;
        private int _queueIndex;
        private int _currentTryAmount;
        private int _mistakesAmount;
        private int _groupIndex;

        public IEnumerator Start()
        {
            yield return new WaitForSeconds(1f);
            
            StartScenarioBlock(0);
        }

        private void StartScenarioBlock(int scenarioIndex)
        {
            _currentScenarioIndex = scenarioIndex;

            var scenarioData = Settings.Get<ScenarioBlocksPool>().
                GetScenarioBlock(scenarioIndex);

            StartScenarioGroup(scenarioData, 0);
        }

        private void StartScenarioGroup(ScenarioBlockPoolData scenarioData, int scenarioGroupIndex)
        {
            _groupIndex = scenarioGroupIndex;
            _currentTryAmount = 0;
            _activeTasks.Clear();

            _stepData = scenarioData.stepGroups[scenarioGroupIndex];

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
            var scenarioData = Settings.Get<ScenarioBlocksPool>().
                GetScenarioBlock(_currentScenarioIndex);

            if (_groupIndex + 1 >= scenarioData.stepGroups.Length)
            {
                SetScenarioMode(ScenarioBlockModeType.Exam);
                StartScenarioGroup(scenarioData, 0);
                Debug.Log("START EXAM!");
                return;
            }
            else
            {
                StartScenarioGroup(scenarioData, _groupIndex + 1);
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

        private void CompleteActiveTasks(GameEvent eventData)
        {
            if (_activeTasks == null || _activeTasks.Count == 0)
            {
                return;
            }

            for (int i = 0; i < _activeTasks.Count; i++)
            {
                if (_activeTasks[i].task.conditionType == eventData.eventType)
                {
                    if (_activeTasks[i].state != TaskStepStateType.None)
                    {
                        continue;
                    }

                    if (_stepData.procedureType == ExecutionProcedureType.Free &&
                        _activeTasks[i].task.eventId == eventData.id)
                    {
                        SetTaskState(_activeTasks[i], TaskStepStateType.Success);
                    }
                    else if (_stepData.procedureType == ExecutionProcedureType.InRotation)
                    {
                        if (i == _queueIndex && _activeTasks[i].task.eventId == eventData.id)
                        {
                            SetTaskState(_activeTasks[i], TaskStepStateType.Success);
                            _queueIndex += 1;
                            SetCurrentQueueTask(_queueIndex);
                        }
                        else
                        {
                            ShowNotification("ÍÅ ÏÎ Î×ÅÐÅÄÈ!");

                            if (_blockModeType == ScenarioBlockModeType.Exam)
                            {
                                SetTaskState(_activeTasks[_queueIndex], TaskStepStateType.Failed);
                                _queueIndex += 1;
                                SetCurrentQueueTask(_queueIndex);
                                AddMistakesAmount();
                            }    
                            else
                            {
                                AddTryAmount();
                            }
                            break;
                        }
                    }

                    if (CurrentGroupIsDone())
                    {
                        StartNextStepsGroup();
                    }
                }
            }
        }

        private void SetTaskState(ScenarioBlockRuntimeData activeTask, TaskStepStateType state)
        {
            activeTask.state = state;
            UpdateActiveTasksVisual(activeTask);
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
            
            if (_mistakesAmount >= _stepData.examMistakesMaxAmount)
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
                StartNextStepsGroup();
            }
        }
    }
}
