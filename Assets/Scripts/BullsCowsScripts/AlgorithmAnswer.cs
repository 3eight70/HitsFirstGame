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
    }
}