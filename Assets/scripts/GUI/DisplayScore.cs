using Assets.scripts.objectives.Action;
using Assets.scripts.RehaTask.RehaTaskGUI;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.scripts.GUI
{
    public class DisplayScore : MonoBehaviour {
    
        public static int Score;
        private static DisplayScore _sInstance;
        public static DisplayScore Instance
        {
            get
            {
                if (_sInstance == null)
                {
                    _sInstance = FindObjectOfType(typeof(DisplayScore)) as DisplayScore;
                    if (_sInstance == null)
                    {
                        Debug.Log("Could not locate Score");
                    }
                }
                return _sInstance;
            }
        }

        private Transform _scoreLabel;
        private Text _scoreLabelText;

        private void Start () 
        {
            _scoreLabel = GameObject.FindGameObjectWithTag("score").transform;
            _scoreLabelText = _scoreLabel.GetComponent<Text>();
            UpdateScoreDisplay(0);
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            //_scoreLabelText.text = DrawObjectiveList.Minimized && !MainGUI.Picture ? Score.ToString(): " ";
        }

        public void UpdateScoreDisplay(int score)
        {
            Score = score;

            //_scoreLabelText.text = DrawObjectiveList.Minimized && !MainGUI.Picture ? Score.ToString() : " ";
            _scoreLabelText.text = Score.ToString();
        }

        public static int ActualScore()
        {
            return Score;
        }
    }
}
