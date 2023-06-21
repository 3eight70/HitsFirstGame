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

        public void SetWin(string number)
        {
            Status = AlgorithmAnswerStatus.Win;
            ExtraText = String.Format("Поздравляю, число {0} отгадано.", number);
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