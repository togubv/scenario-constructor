using System;
using UnityEngine;

namespace SC
{
    [Serializable]
    public class TaskStepGroup
    {
        [SerializeField] private ExecutionProcedureType _procedureType;
        [SerializeField] private int _examMistakesMaxAmount;
        [SerializeField] private TaskStep[] _steps;

        public ExecutionProcedureType procedureType => _procedureType;
        public int examMistakesMaxAmount => _examMistakesMaxAmount;
        public TaskStep[] steps => _steps;
    }
}
