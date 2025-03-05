using Assets.scripts.GUI;
using Assets.scripts.objectives.Action;
using Assets.scripts.RehaTask.RehaTaskGUI;
using Assets.scripts.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.scripts.Manager
{
    public class GameTimer : MonoBehaviour {
    
        public string Timetext= " ";
       
        private string _minutestring;
        private string _secondstring;
        public static int GameTime;
        private static float _seconds = 0.0f;

        private Transform _gameInfoBg;
        public Text TimerText;
        public Text PointsText;
    
        private static GameTimer _sInstance;
        public static GameTimer Instance
        {
            get
            {
                if(_sInstance == null)
                {
                    _sInstance = FindObjectOfType(typeof(GameTimer)) as GameTimer;
                    if(_sInstance==null)
                    {
                        Debug.Log("Could not locate Game Timer");
                    }
                }
                return _sInstance;
            }
        }

        private void Start()
        {
            _gameInfoBg = GameObject.FindGameObjectWithTag("GameInfoBG").transform;
            DontDestroyOnLoad(this.gameObject);
            GameTime = GeneralSettings.SessionTime;
        }
        
        private void Update ()
        {
            if (GameTime >= 0)
            {
                if (!DrawObjectiveList.FirstTimePlay)
                {
                    _seconds -= Time.deltaTime;
                    if (_seconds <= 0)
                    {
                        GameTime--;
                        _seconds = 60;

                    }
                    PointsText.text = Language.points;

                    var sec = _seconds.ToString("00");
                    if (sec == "60")
                        sec = "00";

                    _minutestring = GameTime.ToString("00");
                    _secondstring = sec;
                }
            }
            else
            {
                _minutestring = "00";
                _secondstring = "00";
            }
            if (DrawObjectiveList.Minimized && !MainGUI.Picture)
            {
                _gameInfoBg.gameObject.SetActive(true);
                TimerText.text = _minutestring + " : " + _secondstring;
              
            }
            else
            {
                _gameInfoBg.gameObject.SetActive(false);
                TimerText.text = " ";
            }
        }

        public static float SessionTime()
        {
            float sessionTime = GameTime * 60.0f + _seconds;
            return sessionTime;
        }
    }
}
