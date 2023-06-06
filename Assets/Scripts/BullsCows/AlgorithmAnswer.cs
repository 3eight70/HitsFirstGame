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
        public AlgorithmAnswerStatus Status;

        public AlgorithmAnswer()
        {
            Text = "";
            Status = AlgorithmAnswerStatus.Default;
        }
    }
}