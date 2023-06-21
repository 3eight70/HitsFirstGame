using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BullsCows
{
    public class Algorithm
    {
        private const int MaxAttemptsNum = 5;
        public int AttemptsNum { get; set; }
        private string hiddenNumber;
        private static System.Random random;

        public Algorithm()
        {
            AttemptsNum = 0;
            random = new System.Random();
            hiddenNumber = GenerateRandomNumber().ToString();
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


        private string CorrectCowsWriting(int cowsNum)
        {
            if (cowsNum == 0) return "0 коров";
            if (cowsNum == 1) return "1 корова";
            return String.Format("{0} корова", cowsNum);
        }

        private string CorrectBullsWriting(int bullsNum)
        {
            if (bullsNum == 0) return "0 быков";
            if (bullsNum == 1) return "1 бык";
            return String.Format("{0} быка", bullsNum);
        }


        private string GetBullsCows(string userNumber)
        {
            var bullsNum = 0;
            var cowsNum = 0;

            for (int i = 0; i < 3; i++)
            {
                if (userNumber[i] == hiddenNumber[i]) bullsNum++;
                else if (hiddenNumber.Contains(userNumber[i])) cowsNum++;
            }

            return userNumber + String.Format(" - {0} и {1} ", CorrectBullsWriting(bullsNum), CorrectCowsWriting(cowsNum));
        }

        public bool isLose()
        {
            return AttemptsNum == MaxAttemptsNum;
        }

        public AlgorithmAnswer Execute(string userNumber)
        {
            AlgorithmAnswer answer = new AlgorithmAnswer();

            if (!CheckInput(userNumber))
            {
                answer.SetError();

                return answer;
            }

            answer.Text = GetBullsCows(userNumber);
            AttemptsNum++;

            if (userNumber == hiddenNumber)
            {
                answer.SetWin(hiddenNumber);
            }
            else if (AttemptsNum == MaxAttemptsNum)
            {
                answer.SetLose(hiddenNumber);
            }
            else
            {
                answer.Status = AlgorithmAnswerStatus.Incorrect;
            }

            return answer;
        }
    }

}
