using UnityEngine;

namespace SC
{
    public class ResultAnalyzer : MonoBehaviour
    {
        [SerializeField] private AnalyzeSettingsData _analyzeSettings;

        public void AnalyzeLogs(TaskGroupLogData logs)
        {
            if (_analyzeSettings == null)
            {
                Debug.LogWarning("Not found AnalyzeSettings!");
                return;
            }

            string taskLogs = string.Empty;
            string resultText = string.Empty;

            if (logs.scenarioModeType == ScenarioBlockModeType.Training)
            {
                int overallScore = 0;

                for (int i = 0; i < logs.taskLogs.Length; i++)
                {
                    int score = _analyzeSettings.GetScoreValue(logs.taskLogs[i].triesAmount);
                    overallScore += score;
                    taskLogs += $"{logs.taskLogs[i].description} {score} באככמג.\n\n";
                }

                resultText = $"\nÈעמדמ: {overallScore} באככמג";
            }
            else if (logs.scenarioModeType == ScenarioBlockModeType.Exam)
            {
                int overallPenaltyValue = 0;

                for (int i = 0; i < logs.taskLogs.Length; i++)
                {
                    int penalty = _analyzeSettings.GetPenaltyValue(logs.taskLogs[i].taskStepState);
                    overallPenaltyValue += penalty;
                    taskLogs += $"{logs.taskLogs[i].description} רענאפ {penalty}.\n\n";
                }

                resultText = $"\nÈעמדמ: {_analyzeSettings.penaltySumm - overallPenaltyValue}.";
            }

            Menu.Get<GameController>().SetResult($"{taskLogs}{resultText}", logs.scenarioModeType);
        }
    }
}
