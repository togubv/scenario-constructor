using UnityEngine;
using System;

namespace SC
{
    [Serializable]
    public class PenaltyValueData
    {
        [SerializeField] private TaskStepStateType _resultType;
        [SerializeField] private int _value;

        public TaskStepStateType resultType => _resultType;
        public int value => _value;
    }
}