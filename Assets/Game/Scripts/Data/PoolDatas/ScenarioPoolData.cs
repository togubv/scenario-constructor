using UnityEngine;

namespace SC
{
    public class ScenarioPoolData : ScriptableObject
    {
        [SerializeField] private int _id;

        public int id => _id;
        public virtual TaskStepGroup[] stepGroups { get; }
        public virtual ScenarioBlockModeType modeType { get; }
    }
}
