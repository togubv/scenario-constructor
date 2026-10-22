using System;

namespace SC
{
    [Serializable]
    public struct TaskLogData
    {
        public DateTime timeStamp;
        public EventType eventType;
        public string description;
        public string eventId;
        public TaskStepStateType taskStepState;
        public int triesAmount;
    }
}
