using TMPro;
using UnityEngine;

namespace BDQuizzer
{
    public class EndScreen : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI finalScoreText;
        ScoreKeeper scoreKeeper;

        void Awake()
        {
            scoreKeeper = FindObjectOfType<ScoreKeeper>();
        }

        public void ShowFinalScore()
        {
            if (scoreKeeper.CalculateCurrentScore() == 100)
            {
                finalScoreText.text = $"You have completed the quiz!\nYour score was {scoreKeeper.CalculateCurrentScore()}%.\nYou passed, but how do you know this much? Did you stalk me? As a precaution, this will count as a failure for the reason of stalking me >:)";
            }
            else
            {
                finalScoreText.text = $"You have completed the quiz!\nYour score was {scoreKeeper.CalculateCurrentScore()}%.\nYou failed, as you don't know much about me >:)";
            }
        }
    }
}
