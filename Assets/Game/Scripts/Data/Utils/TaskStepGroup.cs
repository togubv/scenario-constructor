using System;
using UnityEngine;

namespace SC
{
    [Serializable]
    public class TaskStepGroup
    {
        [SerializeField] private ExecutionProcedureType _procedureType;
        [SerializeField] private TaskStep[] _steps;

        public ExecutionProcedureType procedureType => _procedureType;
        public TaskStep[] steps => _steps;
    }
}
