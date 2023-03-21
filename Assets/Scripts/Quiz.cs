using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BDQuizzer
{
    public class Quiz : MonoBehaviour
    {
        [Header("Questions")]
        [SerializeField] TextMeshProUGUI questionText;
        [SerializeField] List<QuestionSO> questions = new List<QuestionSO>();
        QuestionSO currentQuestion;

        [Header("Answers")]
        [SerializeField] GameObject[] answerButtons;
        int correctAnswerIndex;
        bool hasAnsweredEarly = false;

        [Header("Button Colours")]
        [SerializeField] Sprite defaultAnswerSprite;
        [SerializeField] Sprite correctAnswerSprite;

        [Header("Timer")]
        [SerializeField] Image timerImage;
        Timer timer;

        [Header("Scoring")]
        [SerializeField] TextMeshProUGUI scoreText;
        ScoreKeeper scoreKeeper;

        [Header("Progress Bar")]
        [SerializeField] Slider progressBar;

        public bool isComplete = false;

        void Awake()
        {
            timer = FindObjectOfType<Timer>();
            scoreKeeper = FindObjectOfType<ScoreKeeper>();
            progressBar.maxValue = questions.Count;
            progressBar.value = 0;
        }

        void Start()
        {
            timer.isAnsweringQuestion = false;
            timer.loadNextQuestion = true;
        }

        void Update()
        {
            timerImage.fillAmount = timer.fillFraction;
            if (timer.loadNextQuestion && timer.isAnsweringQuestion)
            {
                if (progressBar.value == progressBar.maxValue)
                {
                    isComplete = true;
                    return;
                }

                hasAnsweredEarly = false;
                GetNextQuestion();
                timer.loadNextQuestion = false;
            }
            else if (timer.loadNextQuestion && !timer.isAnsweringQuestion)
            {
                hasAnsweredEarly = false;
                GetFirstQuestion();
                timer.loadNextQuestion = false;
            }
            else if (!hasAnsweredEarly && !timer.isAnsweringQuestion)
            {
                int i = 0;
                while (i == currentQuestion.GetCorrectAnswerIndex())
                {
                    i++;
                }
                DisplayAnswer(i);
                SetButtonState(false);
                scoreText.text = $"Score: {scoreKeeper.CalculateCurrentScore()}%";
            }
        }

        public void OnAnswerSelected(int index)
        {
            hasAnsweredEarly = true;
            DisplayAnswer(index);
            SetButtonState(false);
            timer.CancelTimer();
            scoreText.text = $"Score: {scoreKeeper.CalculateCurrentScore()}%";
        }

        void DisplayAnswer(int index)
        {
            if (index == currentQuestion.GetCorrectAnswerIndex())
            {
                questionText.text = "You are one step closer to becoming a true BatemaDevelopment historian!";
                scoreKeeper.IncrementCorrectAnswers();
            }
            else
            {
                correctAnswerIndex = currentQuestion.GetCorrectAnswerIndex();
                questionText.text = "You aren't a true BatemaDevelopment historian!" +
                    "\n" +
                    $"Answer: {currentQuestion.GetAnswer(currentQuestion.GetCorrectAnswerIndex())}";
            }

            answerButtons[currentQuestion.GetCorrectAnswerIndex()].GetComponent<Image>().sprite = correctAnswerSprite;
        }

        void GetNextQuestion()
        {
            if (questions.Count > 0)
            {
                SetButtonState(true);
                SetDefaultButtonSprites();
                GetRandomQuestion();
                DisplayQuestion();
                progressBar.value++;
                scoreKeeper.IncrementQuestionsSeen();
            }
        }

        void GetFirstQuestion()
        {
            if (progressBar.maxValue == questions.Count)
            {
                questions.Remove(questions[0]);
                GetRandomQuestion();
                DisplayQuestion();
                progressBar.value--;
            }
        }

        void GetRandomQuestion()
        {
            int index = Random.Range(0, questions.Count);
            currentQuestion = questions[index];

            if (questions.Contains(currentQuestion))
            {
                questions.Remove(currentQuestion);
            }
        }

        void DisplayQuestion()
        {
            questionText.text = currentQuestion.GetQuestion();

            for (int i = 0; i < answerButtons.Length; i++)
            {
                TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = currentQuestion.GetAnswer(i);
            }
        }

        void SetButtonState(bool state)
        {
            for (int i = 0; i < answerButtons.Length; i++)
            {
                Button button = answerButtons[i].GetComponent<Button>();
                button.interactable = state;
            }
        }

        void SetDefaultButtonSprites()
        {
            for (int i = 0; i < answerButtons.Length; i++)
            {
                answerButtons[i].GetComponent<Image>().sprite = defaultAnswerSprite;
            }
        }
    }
}
