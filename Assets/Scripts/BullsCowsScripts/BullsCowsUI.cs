using UnityEngine;
using UnityEngine.UI;

namespace BullsCows
{
    public class BullsCowsUI : MonoBehaviour
    {
        public Animator WinAnimator;
        public Animator ErrorAnimator;
        public InputField UserNumber;
        public GameObject TextFieldPrefab;
        public Text ErrorText;
        public Text WinText;
        public Transform AnswersContainer;
        private Algorithm Algorithm;

        private void AddAnswer(string answerText)
        {
            Text[] textFields = AnswersContainer.GetComponentsInChildren<Text>();

            var currentAnswerIndex = 2 * Algorithm.AttemptsNum - 1;

            Text textField = textFields[currentAnswerIndex];

            if (textField != null && textField.name != "Index")
            {
                textField.text = answerText;
            }
        }

        private void OnUserWin(string text)
        {
            WinText.text = text;
            WinAnimator.SetTrigger("OpenWinPopup");

            UserNumber.text = "";
        }

        private void OnTriggerErrorOrLose(string text)
        {
            ErrorText.text = text;
            ErrorAnimator.SetTrigger("OnErrorUserNumber");
        }

        public void OnUserTryClick()
        {
            AlgorithmAnswer result = Algorithm.Execute(UserNumber.text);

            if (result.Status == AlgorithmAnswerStatus.Error)
            {
                OnTriggerErrorOrLose(result.ExtraText);

                return;
            }

            if (result.Status == AlgorithmAnswerStatus.Incorrect)
            {
                AddAnswer(result.Text);
                UserNumber.text = "";

                return;
            }

            if (result.Status == AlgorithmAnswerStatus.Win)
            {
                OnUserWin(result.ExtraText);

                return;
            }

            if (result.Status == AlgorithmAnswerStatus.Lose)
            {
                AddAnswer(result.Text);
                OnTriggerErrorOrLose(result.ExtraText);

                return;
            }
        }

        public void ResetGame()
        {
            Text[] textFields = AnswersContainer.GetComponentsInChildren<Text>();

            foreach (var textField in textFields)
            {
                if (textField != null && textField.name != "Index")
                {
                    textField.text = "";
                }
            }

            Algorithm = new Algorithm();
        }

        public void OnErrorOrLosePopupClose()
        {
            if (Algorithm.isLose()) {
                ResetGame();
                UserNumber.text = "";
            }
        }

        void Start()
        {
            Algorithm = new Algorithm();
        }
    }
}
