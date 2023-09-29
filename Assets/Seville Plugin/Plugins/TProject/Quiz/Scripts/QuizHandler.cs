using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Tproject.Quiz
{
    public class QuizHandler : MonoBehaviour
    {
        public QuizController quizController;
        public List<QuizController.MQuizContent> mQuizContentList;

        [SerializeField] bool playOnStart = false;
        public UnityEvent AfterDialogFinish;

        private void Start()
        {
            if (playOnStart) ShowQuiz();
        }

        public void ShowQuiz()
        {
            bool checkquestion = quizController.StartQuiz(mQuizContentList, QuizIsDone);

            if (!checkquestion) AfterDialogFinish?.Invoke();
        }

        public void QuizIsDone()
        {
            AfterDialogFinish?.Invoke();
        }
    }
}