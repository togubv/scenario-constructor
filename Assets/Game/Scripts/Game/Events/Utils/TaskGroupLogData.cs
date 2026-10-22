using System;

namespace SC
{
    [Serializable]
    public struct TaskGroupLogData
    {
        public int contractVersion;
        public int id;
        public ExecutionProcedureType procedureType;
        public ScenarioBlockModeType scenarioModeType;
        public int examMistakesMaxAmount;
        public TaskLogData[] taskLogs;
    }
}
