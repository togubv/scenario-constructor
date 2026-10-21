using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SC
{
    public class ScenarioBlockContainer : MonoBehaviour
    {
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Image checkmarkImage;
        [SerializeField] private Color descriptionAvailableColor;
        [SerializeField] private Color descriptionNotAvailableColor;

        public TaskStep blockData => _blockData;

        private TaskStep _blockData;

        public void Init(TaskStep blockData)
        {
            _blockData = blockData;
            descriptionText.text = $"{blockData.description}";
        }

        public void ToggleAvailableState(bool toggle)
        {
            if (toggle)
            {
                descriptionText.color = descriptionAvailableColor;
            }
            else
            {
                descriptionText.color = descriptionNotAvailableColor;
            }
        }

        public void UpdateState(ScenarioBlockRuntimeData runtimeData)
        {
            checkmarkImage.gameObject.SetActive(true);

            switch (runtimeData.state)
            {
                case TaskStepStateType.Success:
                    checkmarkImage.color = Color.green;
                    break;

                case TaskStepStateType.Failed:
                    checkmarkImage.color = Color.red;
                    break;

                case TaskStepStateType.Skipped:
                    checkmarkImage.color = Color.yellow;
                    break;
            }
        }
    }
}
