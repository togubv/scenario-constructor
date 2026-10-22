using UnityEngine;

namespace SC
{
    [CreateAssetMenu(fileName = "ScenarioExam", menuName = "Data/ScenarioExam")]
    public class ScenarioExamPoolData : ScenarioPoolData
    {
        [SerializeField] private TaskStepGroupExam[] _stepGroups;

        public override ScenarioBlockModeType modeType { get { return ScenarioBlockModeType.Exam; } }
        public override TaskStepGroup[] stepGroups { get { return _stepGroups; } }
    }
}