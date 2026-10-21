using UnityEngine;

namespace SC
{
    [CreateAssetMenu(fileName = "TaskStep", menuName = "Data/TaskStep")]
    public class TaskStep : ScriptableObject
    {
        [SerializeField] private string _eventId;
        [SerializeField] private EventType _conditionType;
        [SerializeField] private string _description;

        public string eventId => _eventId;
        public EventType conditionType => _conditionType;
        public string description => _description;
    }
}
