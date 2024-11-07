using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Seville
{
    public class QuizController : MonoBehaviour
    {
        private QuizData quizDataTemp;

        [Header("Component Dependencies")]
        [Space(7f)]
        [SerializeField] private QuizConfiguration quizConfig;
        [SerializeField] private List<GameObject> listPanelSection;
        private int m_currentActivePanelId = -1;
        [Space(7f)]

        // buttons
        [SerializeField] private GameObject btnOpenQuiz;

        [Header("Panel Validate To Start Quiz")]
        [SerializeField] private Button btnCancleQuiz;
        [SerializeField] private Button btnShowQuestion;

        [Header("Panel Main Quiz Content")]
        private int m_currentQuestionIndex;
        [SerializeField] private Button btnPrevious;
        [SerializeField] private Button btnNext;
        [SerializeField] private Button btnValidateToSubmit;
        [SerializeField] private TextMeshProUGUI textCounter;
        [SerializeField] private TextMeshProUGUI textQuestion;
        [SerializeField] List<GameObject> btnOptions;

        [Header("Panel Validation To Submit")]
        [SerializeField] private Button btnCancleSubmit;
        [SerializeField] private Button btnSubmitQuiz;

        [Header("Panel Score")]
        [SerializeField] private Button btnOpenReview;
        [SerializeField] private Button btnDoneQuiz;
        [SerializeField] private TextMeshProUGUI textScorePercentage;
        [SerializeField] private TextMeshProUGUI textCorrectScore;
        [SerializeField] private TextMeshProUGUI textTotalScore;
        [SerializeField] private Image imgScoreBar;

        [Header("Panel Review")]
        [SerializeField] private Button btnCloseReview;
        [SerializeField] private QuizReviewItem quizReviewItemPrefab;
        [SerializeField] private GameObject reviewItemParent;

        private void Start()
        {
            btnOpenQuiz.GetComponent<Button>().onClick.AddListener(() => OpenOrCloseQuiz(true));

            btnCancleQuiz.onClick.AddListener(() => OpenOrCloseQuiz(false));
            btnShowQuestion.onClick.AddListener(() => ShowQuestion());

            btnPrevious.onClick.AddListener(() => OnClickPervQuestion());
            btnNext.onClick.AddListener(() => OnClickNextQuestion());
            btnValidateToSubmit.onClick.AddListener(() => SwitchingPanelById(2));

            btnCancleSubmit.onClick.AddListener(() => SwitchingPanelById(1));
            btnSubmitQuiz.onClick.AddListener(SubmitAnswer);

            btnOpenReview.onClick.AddListener(ShowReview);
            btnCloseReview.onClick.AddListener(() => SwitchingPanelById(3));

            btnDoneQuiz.onClick.AddListener(() =>
                {
                    if (quizConfig.canRetakeQuiz)
                    {
                        OpenOrCloseQuiz(false);
                        m_currentQuestionIndex = 0;
                    }
                    else
                    {
                        ScaleOut(listPanelSection[3], () =>
                        {
                            quizConfig.UnityEvents.OnQuizClose?.Invoke();
                        });
                    }
                });
        }

        public void SwitchingPanelById(int id)
        {
            if (listPanelSection == null || listPanelSection.Count == 0)
            {
                Debug.LogWarning("List panel section is empty, please fill the panel list.");
                return;
            }

            if (m_currentActivePanelId == id) return;

            if (m_currentActivePanelId >= 0 && m_currentActivePanelId < listPanelSection.Count)
            {
                ScaleOut(listPanelSection[m_currentActivePanelId], () => OpenPanel(id));
            }
            else
            {
                OpenPanel(id);
            }
        }

        private void OpenPanel(int id)
        {
            if (id >= 0 && id < listPanelSection.Count)
            {
                m_currentActivePanelId = id;
                ScaleIn(listPanelSection[id]);
            }
        }

        public void OpenOrCloseQuiz(bool isOpen)
        {
            if (isOpen)
            {
                SwitchingPanelById(0);
                btnOpenQuiz.SetActive(false);
            }
            else
            {
                ScaleOut(listPanelSection[m_currentActivePanelId], () =>
                {
                    btnOpenQuiz.SetActive(true);
                    quizConfig.UnityEvents.OnQuizClose?.Invoke();
                });
                m_currentActivePanelId = -1;
            }
        }

        #region Main-Question-Panel
        private void ShowQuestion()
        {
            quizDataTemp = quizConfig.GetQuizData();

            if (quizDataTemp != null || quizDataTemp.contentList.Count == 0)
            {
                Debug.LogWarning($"Seville Quiz Controller: there is no question in quiz config");
                return;
            }

            if (quizConfig.shuffleAnswerOption)
            {
                quizDataTemp.ShuffleContent();
            }

            SwitchingPanelById(1);
            SetUpContentByIndex(0);

            UpdateQuestionButtonVisibility();
        }

        private void SetUpContentByIndex(int index)
        {
            textCounter.text = $"Question {index + 1}/{quizDataTemp.contentList.Count}";
            textQuestion.text = quizDataTemp.contentList[index].question;

            for (int i = 0; i < quizDataTemp.contentList[index].optionList.Count; i++)
            {
                btnOptions[i].GetComponent<QuizButtonOption>().SetUpButton(
                    _index: i,
                    _validate: quizDataTemp.contentList[index].optionList[i].validate,
                    _text: quizDataTemp.contentList[index].optionList[i].answerText,
                    _stateView: quizDataTemp.contentList[index].isAnswerd && quizDataTemp.contentList[index].answerIndex == i,
                    _buttonAction: OnSelectionOption
                );
            }
        }

        public void OnSelectionOption(int _indexBtn)
        {
            MQuizContent currentQuestionData = quizDataTemp.contentList[m_currentQuestionIndex];
            QuizButtonOption optionData = btnOptions[_indexBtn].GetComponent<QuizButtonOption>();

            currentQuestionData.answerIndex = optionData.GetAnswerIndex;
            currentQuestionData.answerText = optionData.GetTextOption;
            currentQuestionData.isAnswerCorrect = optionData.GetAnswerValidation;
            currentQuestionData.isAnswerd = true;

            for (int i = 0; i < btnOptions.Count; i++)
            {
                btnOptions[i].GetComponent<QuizButtonOption>().UpdateSelectedView(i == _indexBtn);
            }

            UpdateQuestionButtonVisibility();
        }

        private void UpdateQuestionButtonVisibility()
        {
            MQuizContent currentQuestionData = quizDataTemp.contentList[m_currentQuestionIndex];
            bool isFirstIndex = m_currentQuestionIndex == 0;
            bool isLastIndex = m_currentQuestionIndex == quizDataTemp.contentList.Count - 1;

            if (currentQuestionData.isAnswerd)
            {
                btnNext.gameObject.SetActive(!isLastIndex);
                btnPrevious.gameObject.SetActive(!isFirstIndex);
                btnValidateToSubmit.gameObject.SetActive(isLastIndex);
            }
            else
            {
                btnNext.gameObject.SetActive(false);
                btnPrevious.gameObject.SetActive(m_currentQuestionIndex > 0);
                btnValidateToSubmit.gameObject.SetActive(false);
            }
        }

        private void OnClickNextQuestion()
        {
            if (m_currentQuestionIndex < quizDataTemp.contentList.Count - 1)
            {
                m_currentQuestionIndex++;

                FaderAnimation(listPanelSection[m_currentActivePanelId].GetComponent<CanvasGroup>(), () =>
                {
                    SetUpContentByIndex(m_currentQuestionIndex);
                    UpdateQuestionButtonVisibility();
                });
            }
        }

        private void OnClickPervQuestion()
        {
            if (m_currentQuestionIndex > 0)
            {
                m_currentQuestionIndex--;

                FaderAnimation(listPanelSection[m_currentActivePanelId].GetComponent<CanvasGroup>(), () =>
                {
                    SetUpContentByIndex(m_currentQuestionIndex);
                    UpdateQuestionButtonVisibility();
                });
            }
        }
        #endregion

        #region Score-Panel
        public void SubmitAnswer()
        {
            imgScoreBar.fillAmount = 0;
            SwitchingPanelById(3);

            int correctScore = quizDataTemp.contentList.FindAll((x) => x.isAnswerCorrect).Count;

            float percentage = ((float)correctScore / quizDataTemp.contentList.Count) * 100f;
            textScorePercentage.text = $"{Mathf.RoundToInt(percentage)}%";

            textCorrectScore.text = correctScore.ToString();
            textTotalScore.text = quizDataTemp.contentList.Count.ToString();

            LeanTween.value(imgScoreBar.gameObject, imgScoreBar.fillAmount, percentage / 100f, 1f)
                .setDelay(.7f)
                .setOnUpdate((float val) =>
                {
                    imgScoreBar.fillAmount = val;
                });

            quizConfig.UnityEvents.GetCorrectAnswer?.Invoke(correctScore);
        }
        #endregion

        #region Review-Panel
        private void ShowReview()
        {
            SwitchingPanelById(4);

            foreach (Transform child in reviewItemParent.transform)
            {
                UnityEngine.Object.Destroy(child.gameObject);
            }

            foreach (var reviewContent in quizDataTemp.contentList)
            {
                QuizReviewItem rowItem = Instantiate(quizReviewItemPrefab, reviewItemParent.transform);
                rowItem.SetUpReviewItem(
                    _isCorrectAnswer: reviewContent.isAnswerCorrect,
                    _number: (quizDataTemp.contentList.IndexOf(reviewContent) + 1).ToString(),
                    _question: reviewContent.question,
                    _playerAnswer: reviewContent.answerText,
                    _correctAnswer: reviewContent.optionList.First(x => x.validate).answerText
                );
            }

            StartCoroutine(RefreshLayout());
        }

        IEnumerator RefreshLayout()
        {
            reviewItemParent.GetComponent<ContentSizeFitter>().enabled = false;
            yield return new WaitForSeconds(.7f);
            reviewItemParent.GetComponent<ContentSizeFitter>().enabled = true;
            LayoutRebuilder.ForceRebuildLayoutImmediate(reviewItemParent.GetComponent<RectTransform>());
        }
        #endregion

        #region UI-Animation
        private void ScaleIn(GameObject targetObject)
        {
            if (targetObject == null) return;

            targetObject.SetActive(true);
            targetObject.transform.localScale = Vector3.zero;
            LeanTween.scale(targetObject, Vector3.one, 1f)
                .setEase(LeanTweenType.easeOutSine);
        }

        private void ScaleOut(GameObject targetObject, Action callback = null)
        {
            if (targetObject == null || !targetObject.activeSelf) return;

            LeanTween.scale(targetObject, Vector3.zero, .5f)
                .setEase(LeanTweenType.easeInQuad)
                .setOnComplete(() =>
                {
                    targetObject.SetActive(false);
                    callback?.Invoke();
                });
        }

        private void FaderAnimation(CanvasGroup targetObject, Action callback = null)
        {
            LeanTween.alphaCanvas(targetObject, 0f, .5f)
            .setEase(LeanTweenType.easeInOutQuad)
            .setOnComplete(() =>
            {
                callback?.Invoke();

                LeanTween.alphaCanvas(targetObject, 1f, 1f)
                    .setEase(LeanTweenType.easeInOutQuad);
            });
        }
        #endregion
    }
}