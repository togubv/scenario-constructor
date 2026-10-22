using UnityEngine;

namespace SC
{
    [CreateAssetMenu(fileName = "ScenarioTraining", menuName = "Data/Scenarios/ScenarioTraining")]
    public class ScenarioTrainingPoolData : ScenarioPoolData
    {
        [SerializeField] private TaskStepGroup[] _stepGroups;

        public override ScenarioBlockModeType modeType { get { return ScenarioBlockModeType.Training; } }
        public override TaskStepGroup[] stepGroups { get { return _stepGroups; } }
    }
}