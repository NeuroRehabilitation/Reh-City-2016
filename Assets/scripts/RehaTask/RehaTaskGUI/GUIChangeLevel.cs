using System;
using System.Collections;
using Assets.scripts.RehaTask.RehaTask_GameLogic;
using Assets.scripts.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.scripts.RehaTask.RehaTaskGUI
{
    public class GUIChangeLevel : MonoBehaviour
    {
        public GameObject mainGUI, valenceGroup;
        public Text completionText, totalScore, warning, congratsLabel, levelChangeLabel, levelChangeTimer, memLabel, attLabel, complLabel, memLevel, attLevel;
        private GameObject _ikTarget;

        public static GameObject[] IapsToggle;
        public static bool[] IapsChoice = new bool[9];//stores the boolean value of the Valence toggles

        public static int Iaps, Temp;
        public static bool Setting = false;

        public static bool ValenceRated = false;
        public static bool LevelFailed = false;

        public static float ChangeLevelTimer = 6.0f;

        public static bool Success = false, LevelFinished;
        public static int LevelStarted = 0;
        private bool _canComplete;

        private void Start()
        {
            _canComplete = true;
            //_ikTarget = GameObject.FindGameObjectWithTag("hand");

            //IComparer myToggleComparer = new MyTogglesSorter();//sorts the toggles by order
            //IapsToggle = GameObject.FindGameObjectsWithTag("IAPSToggle");
            //Array.Sort(IapsToggle, myToggleComparer);
            /*
            foreach (var t in IapsToggle)
            {
                t.GetComponent<Toggle>().isOn = false;//sets all toggles as false
            }*/

            //levelChangeLabel.text = Language.getReady;
            //levelChangeLabel.gameObject.SetActive(false);
            //levelChangeTimer.gameObject.SetActive(false);
            this.gameObject.SetActive(false);//this is set to false and will be set to true in the end of each level
        
        }

        private void Update()
        {
            if (_canComplete)
            {
                ContinueOrQuit(1);
                _canComplete = false;
            }
        }

        public void ContinueOrQuit(int cont)
        {
            if (cont == 1)
            {
                CollisionDetection.ChangeLevel = false;
                //_ikTarget.collider.enabled = true;
                MainGUI.ActivateHint = false;
                SpawnTiles.RenderPointer = false;
                CollisionDetection.X = 0;
                LevelFinished = true;
                gameObject.SetActive(false);
            }
        }
        /*
        public class MyTogglesSorter : IComparer
        {
            int IComparer.Compare(object x, object y)
            {
                return ((new CaseInsensitiveComparer()).Compare(((GameObject)x).name, ((GameObject)y).name));
            }
        }*/

    }
}



