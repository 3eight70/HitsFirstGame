using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace BullsCows
{
    public class BullsCowsUI : MonoBehaviour
    {
        public InputField UserNumber;
        public Text ErrorText;
        public GameObject TextFieldPrefab;
        public Transform TextFieldContainer;
        private Algorithm Algorithm;

        private Text CreateTextField()
        {
            const int defaultMarginTop = 30;

            float marginTop = (defaultMarginTop * Algorithm.AttemptsNum);

            GameObject newTextField = Instantiate(TextFieldPrefab, TextFieldContainer.transform);
            newTextField.transform.localPosition = Vector3.zero;

            Text textFieldText = newTextField.GetComponentInChildren<Text>();
            textFieldText.transform.localPosition = new Vector3(0f, -marginTop, 0f);

            return textFieldText;
        }

        public void OnUserTryClick()
        {
            AlgorithmAnswer result = Algorithm.Execute(UserNumber.text);

            if (result.Status == AlgorithmAnswerStatus.Error)
            {
                ErrorText.text = result.Text;

                return;
            }

            if (result.Status == AlgorithmAnswerStatus.Incorrect)
            {
                Text textFieldText = CreateTextField();
                textFieldText.text = result.Text;

                ErrorText.text = "";
                UserNumber.text = "";

                return;
            }

            if (result.Status == AlgorithmAnswerStatus.Win)
            {

                Text textFieldText = CreateTextField();
                textFieldText.text = result.Text;

                ErrorText.text = "";
                UserNumber.text = "";
                
                return;
            }

            if (result.Status == AlgorithmAnswerStatus.Lose)
            {
                ResetGame();
                return;
            }
        }

        private void ResetGame()
        {
            for (int i = TextFieldContainer.transform.childCount - 1; i >= 2; i--)
            {
                GameObject textField = TextFieldContainer.transform.GetChild(i).gameObject;

                Destroy(textField);
            }
        }

        void Start()
        {
            Algorithm = new Algorithm();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
