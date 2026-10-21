using UnityEngine;

namespace SC
{
    [CreateAssetMenu(fileName = "ScenarioBlock", menuName = "Data/ScenarioBlock")]
    public class ScenarioBlockPoolData : ScriptableObject
    {
        [SerializeField] private int _id;
        [SerializeField] private TaskStepGroup[] _stepGroups;

        public int id => _id;
        public TaskStepGroup[] stepGroups => _stepGroups;

    }
}
