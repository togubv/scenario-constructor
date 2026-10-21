namespace SC
{
    public class ScenarioBlockRuntimeData
    {
        public TaskStep task;
        public TaskStepStateType state;

        public ScenarioBlockRuntimeData(TaskStep task)
        {
            this.task = task;
        }
    }
}
