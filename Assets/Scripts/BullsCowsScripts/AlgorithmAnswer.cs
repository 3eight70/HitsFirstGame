using System;

namespace BullsCows
{
    public enum AlgorithmAnswerStatus
    {
        Win,
        Lose,
        Default,
        Error,
        Incorrect
    }

    public class AlgorithmAnswer
    {
        public string Text;
        public string ExtraText;
        public AlgorithmAnswerStatus Status;

        public AlgorithmAnswer()
        {
            Text = "";
            ExtraText = "";
            Status = AlgorithmAnswerStatus.Default;
        }

        public void SetWin(string number, bool winFlag, char pinNumber)
        {
            Status = AlgorithmAnswerStatus.Win;
            if (winFlag)
            {
                ExtraText = String.Format("А ты хорош, поздравляю, угадал число {0}.", number);
            } else
            {
                ExtraText = String.Format("А ты оказался умным, число {0} отгадано. Поздравляю тебя, вот твоя цифра: " + pinNumber, number);
            }
        }

        public void SetError()
        {
            Status = AlgorithmAnswerStatus.Error;
            ExtraText = "Введите трехзначное число без повторений";
        }

        public void SetLose(string number)
        {
            Status = AlgorithmAnswerStatus.Lose;
            ExtraText = String.Format("Правильный ответ - {0}, попробуйте снова", number);
        }
    }
}