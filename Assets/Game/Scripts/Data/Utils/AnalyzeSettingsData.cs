using UnityEngine;

namespace SC
{
    [CreateAssetMenu(fileName = "AnalyzeSettings", menuName = "Data/AnalyzeSettings")]
    public class AnalyzeSettingsData : ScriptableObject
    {
        [SerializeField] private int _penaltySumm = 100;
        [SerializeField] private PenaltyValueData[] _penaltyValues;
        [SerializeField] private int[] _scoreValues;

        public int penaltySumm => _penaltySumm;

        public int GetPenaltyValue(TaskStepStateType resultType)
        {
            foreach (var p in _penaltyValues)
            {
                if (p.resultType == resultType)
                {
                    return p.value;
                }
            }

            return 0;
        }

        public int GetScoreValue(int tries)
        {
            if (tries < _scoreValues.Length)
            {
                return _scoreValues[tries];
            }

            return 0;
        }
    }
}
