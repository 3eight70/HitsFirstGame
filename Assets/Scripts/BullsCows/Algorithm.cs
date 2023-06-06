using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BullsCows
{
    public class Algorithm
    {
        private const int MaxAttemptsNum = 10;
        public int AttemptsNum { get; set; }
        private string hiddenNumber;
        private static System.Random random;

        public Algorithm()
        {
            AttemptsNum = 0;
            random = new System.Random();
            // hiddenNumber = GenerateRandomNumber().ToString();
            hiddenNumber = "123";
        }

        private static int GenerateRandomNumber()
        {
            List<int> digits = new List<int>();

            while (digits.Count < 3)
            {
                int digit = random.Next(1, 10);

                if (!digits.Contains(digit))
                {
                    digits.Add(digit);
                }
            }

            int number = digits[0] * 100 + digits[1] * 10 + digits[2];

            return number;
        }

        private bool CheckInput(string userNumber)
        {
            if (!Regex.IsMatch(userNumber, "^[\\d]+$")) return false;

            if (userNumber.Length != 3) return false;
            int[] numbers = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            foreach (var symbol in userNumber)
            {
                var number = int.Parse(symbol.ToString());
                numbers[number] += 1;
                if (numbers[number] > 1) return false;
            }

            return true;
        }

        public AlgorithmAnswer Execute(string userNumber)
        {
            AlgorithmAnswer answer = new AlgorithmAnswer();

            if (!CheckInput(userNumber))
            {
                answer.Status = AlgorithmAnswerStatus.Error;
                answer.Text = "Введите трехзначное число без повторений";
            }
            else if (userNumber == hiddenNumber)
            {
                answer.Status = AlgorithmAnswerStatus.Win;
                answer.Text = "Поздравляю с победой";
            }
            else if (AttemptsNum == MaxAttemptsNum)
            {
                answer.Status = AlgorithmAnswerStatus.Lose;
                answer.Text = "Вы проиграли, попробуйте снова";
            }
            else
            {
                answer.Status = AlgorithmAnswerStatus.Incorrect;
                answer.Text = "быки коровы";
            }

            AttemptsNum++;

            return answer;
        }
    }

}
