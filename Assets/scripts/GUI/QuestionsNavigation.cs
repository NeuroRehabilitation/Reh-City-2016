using System.Collections.Generic;
using Assets.scripts.Controller;
using Assets.scripts.objectives;
using Assets.scripts.objectives.Action;
using Assets.scripts.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.scripts.GUI
{
    public class QuestionsNavigation : MonoBehaviour {
        
        public Sprite JournalBg;
        public Sprite HealthCover;
        public Sprite GeneralCover;
        private static Text _textTipQuestion, _imgTipQuestion;
        private static Image  _questionsBg, _magCover;
        public static int ActualQuestion;
        public static string ActualSubject;
        public static string Answer = "NaN";
        private static bool _questionReady;
        private Text _yesBtnText, _noBtnText;
        private GameObject _mger;
        private static ObjectiveManager _objManager;
        public static int WrongAnswers;

        private void Start ()
        {
            ActualQuestion = 0;
            _mger = GameObject.FindGameObjectWithTag("Manager");
            _objManager = _mger.GetComponent<ObjectiveManager>();
            _textTipQuestion = GameObject.Find("textTipQuestion").GetComponent<Text>();
            _imgTipQuestion = GameObject.Find("imgTipQuestion").GetComponent<Text>();
            _textTipQuestion.gameObject.SetActive(false);
            _imgTipQuestion.gameObject.SetActive(false);
            _questionsBg = GameObject.Find("questionsBG").GetComponent<Image>();
            _magCover = GameObject.Find("Cover").GetComponent<Image>();
            _yesBtnText = GameObject.Find("TrueButton").GetComponentInChildren<Text>();
            _noBtnText = GameObject.Find("FalseButton").GetComponentInChildren<Text>();
            _yesBtnText.text = Language.yes;
            _noBtnText.text = Language.no;
            _questionReady = false;
        }
	
        private void Update ()
        {
            if (!_questionReady && !LoadQuestions.QuestionsCompleted)
            {
                DisplayQuestion();
                Answer = "NaN";
            }   
        }

        public void DisplayQuestion()
        {
            ActualSubject = _objManager.GetCurrentObjective.SubjectsList[ActualQuestion];
            //Debug.Log(ActualSubject);
            
            if (ActualSubject.Contains("text"))
            {
                _textTipQuestion.gameObject.SetActive(true);
                _questionsBg.sprite = JournalBg;
                _textTipQuestion.text = _objManager.GetCurrentObjective.QuestionsList[ActualQuestion];
                
                _magCover.gameObject.SetActive(false);
            }
            else
            {
                _imgTipQuestion.gameObject.SetActive(true);
                _textTipQuestion.gameObject.SetActive(false);
                _questionsBg.gameObject.SetActive(false);
                _magCover.sprite = ActualSubject.Contains("health") ? HealthCover : GeneralCover;
                _imgTipQuestion.text = _objManager.GetCurrentObjective.QuestionsList[ActualQuestion];
            }
            _questionReady = true;
        }

        public static void SaveAnswer(string answer)
        {
            Answer = ActualSubject.Contains(answer) ? "Correct" : "Wrong";
            if(Answer == "Wrong")
                WrongAnswers++;

            if (_objManager.GetCurrentObjective.QuestionsList.Count > ActualQuestion + 1)
            {
                GameObject.Find("Book").GetComponent<AutoFlip>().FlipRightPage();
                ActualQuestion ++;
            }
            else
            {
                ActualQuestion = 0;
                LoadQuestions.QuestionsCompleted = true;
            }
            _questionReady = false;
            
        }

        public static string ReturnAnswer()
        {
            return Answer;
        }
    }
}
