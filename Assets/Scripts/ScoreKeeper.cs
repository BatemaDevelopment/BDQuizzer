using static System.Math;
using UnityEngine;

namespace BDQuizzer
{
    public class ScoreKeeper : MonoBehaviour
    {
        int correctAnswers = 0;
        int questionsSeen = 0;

        public int GetCorrectAnswers()
        {
            return correctAnswers;
        }

        public int GetQuestionsSeen()
        {
            return questionsSeen;
        }

        public void IncrementCorrectAnswers()
        {
            correctAnswers++;
        }

        public void IncrementQuestionsSeen()
        {
            questionsSeen++;
        }

        public int CalculateCurrentScore()
        {
            return (int) Round((float) correctAnswers / questionsSeen * 100);
        }
    }
}