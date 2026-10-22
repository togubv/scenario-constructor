using System;
using UnityEngine;

namespace SC
{
    [Serializable]
    public class TaskStepGroupExam : TaskStepGroup
    {
        [SerializeField] private int _examMistakesMaxAmount;

        public int examMistakesMaxAmount => _examMistakesMaxAmount;
    }
}