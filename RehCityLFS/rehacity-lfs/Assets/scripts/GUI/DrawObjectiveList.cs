using System.Collections.Generic;
using Assets.scripts.Controller;
using Assets.scripts.GUI.StartSCene;
using Assets.scripts.Locations;
using Assets.scripts.Manager;
using Assets.scripts.MiniMapScripts.AStar;
using Assets.scripts.objectives;
using Assets.scripts.objectives.Action;
using Assets.scripts.RehaTask.RehaTaskGUI;
using Assets.scripts.RehaTask.RehaTask_GameLogic;
using Assets.scripts.Settings;
using Assets.scripts.UDP;
using UnityEngine;
using UnityEngine.UI;

/* Attach this class to any object (ATM attached to InterfacePrefab)
 * This class draws the entire objecive window
 * Coders : Kushal & Teresa
*/

namespace Assets.scripts.GUI
{
    public class DrawObjectiveList : MonoBehaviour
    {
        // boolean used to minimize/maximize
        [HideInInspector]
        public bool MinimizeObjective = false;

        //variable to get Objective manager
        private static ObjectiveManager _objManager;
        private GameManager _gm;
        private GameObject _mger;

        // string that will contain the _description of Objective
        private string _description;

        /// <summary>
        /// set by startscenesettings script
        /// </summary>
        public bool ShowObjectiveWindow = true;

        public GameObject ObjectivesWindow;

        public bool CanReset = false;
        private static DrawObjectiveList _sInstance = null;

        public static DrawObjectiveList Instance
        {
            get
            {
                if (_sInstance == null)
                {
                    _sInstance = FindObjectOfType(typeof(DrawObjectiveList)) as DrawObjectiveList;
                    if (_sInstance == null)
                    {
                        Debug.Log("Could not locate DrawObjectiveList");
                    }
                }
                return _sInstance;
            }
        }

        private Transform _miniMap, _objectiveList;
        public static Image ObjectiveIcon, MiniMapRect, Objective;
        public static bool LevelSet = false;
        public static bool Minimized;
        public static bool AllObjectives;

        private GameObject _bg, _objDescription;//, _itemsList, _timerPlusPoints, _continueBtn, _homeGui;
        public static bool FirstTimePlay = true;
        public static bool DrawingList;
        private Image _verticalLayout;
        public GameObject Item, CalibUi, CityCrossHair, Bg;
        private Text _descriptionText;
        public static bool CanGoToNextObj;

        public Text BtnText;
        private float _timer, _continueTimer = 3f;
        public static float TimeToNextStep = 8.0f;
        public static bool CanActivateDelayTimer, ShowMiniMap;

        public GameObject LocationLabel, Tracking;
        public Text LocLabelText, LocationBtnText;
        private Animator _animDescription, _animLocationReached;
        private bool _goToNextStep, _canEnterLocation, _canMinimize;

        public static float TaskTime = 0;

        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);

            CalibUi.SetActive(StartSceneSettings.DoCalibration);

            _bg = GameObject.FindGameObjectWithTag("BG");
            _objDescription = GameObject.FindGameObjectWithTag("objective");
            _miniMap = GameObject.FindGameObjectWithTag("Minimap").transform;
            _objectiveList = GameObject.Find("ObjListBG").transform;
            _verticalLayout = GameObject.FindGameObjectWithTag("ItemsLayout").GetComponent<Image>();
            _descriptionText = GameObject.FindGameObjectWithTag("Description").GetComponent<Text>();
            LocationLabel.SetActive(false);
            _animDescription = _objDescription.GetComponent<Animator>();
            _animLocationReached = LocationLabel.GetComponent<Animator>();
            _animDescription.enabled = false;
            _animLocationReached.enabled = false;
            _objDescription.gameObject.SetActive(false);
            _timer = TimeToNextStep;
        }

        private void Start()
        {
            BtnText.text = " ";
            _mger = GameObject.FindGameObjectWithTag("Manager");
            _gm = _mger.GetComponent<GameManager>();
            _objManager = _mger.GetComponent<ObjectiveManager>();
            _miniMap.gameObject.SetActive(false);
            _description = " ";
        }

        public void ToggleMinimize(int minimize)
        {
            if (minimize == 1)
            {
                if (MiniMapRect != null)
                {
                    MiniMapRect.rectTransform.anchoredPosition = new Vector2(Screen.width * -0.25f, 0);
                    MiniMapRect.rectTransform.localScale = new Vector2(0.25f, 0.25f);
                }
                _objDescription.gameObject.SetActive(false);
                Minimized = true;
            }
        }

        public void Toggleobjective()
        {
            MinimizeObjective = !MinimizeObjective;
        }

        public void MinimizeInterface(bool min)
        {
            if (_canMinimize || _gm.IsCalibrationRequired)
            {
                TaskTime = 0;
                _animDescription.Play("ObjectiveDisplayAnim2");
                _goToNextStep = true;
                _canMinimize = false;
                
                if (!_gm.IsCalibrationRequired)
                {
                    _continueTimer = 3f;
                    CanActivateDelayTimer = false;
                }
            }
        }

        private void MiniMapBig()
        {
            _miniMap.localScale = new Vector2(1f, 1f);
            _miniMap.localPosition = new Vector2(Screen.width * -0.5f, Screen.height * -0.5f);
        }

        private void MiniMapSmall()
        {
            _miniMap.localScale = new Vector2(0.25f, 0.25f);
            _miniMap.localPosition = new Vector2(Screen.width * -0.48f, Screen.height * -0.465f);
        }

        private void InterfaceStates()
        {
            if (Minimized)
            {
                
                _objDescription.SetActive(false);

                if (Application.loadedLevelName != "GameOver")
                {
                    _objectiveList.gameObject.SetActive(true);
                    _verticalLayout.gameObject.SetActive(true);
                }

                FirstTimePlay = false;
            }
            else
            {
                _objectiveList.gameObject.SetActive(false);
                _verticalLayout.gameObject.SetActive(false);
            }

            if (Application.loadedLevel == 2)
            {
                if (Minimized)
                {
                    this.GetComponent<Canvas>().sortingOrder = 3;
                    _miniMap.gameObject.SetActive(ShowMiniMap);

                    if ((!Tracking.activeSelf && _gm.IsCalibrationRequired && CalibrationGUI.CalibDone) || !_gm.IsCalibrationRequired)
                    {
                        MiniMapSmall();
                        _bg.SetActive(false);
                    }
                    else
                        _bg.SetActive(true);
                    
                    CityCrossHair.SetActive(true);
                }
                else
                {
                    if ((!Tracking.activeSelf && _gm.IsCalibrationRequired && CalibrationGUI.CalibDone) || !_gm.IsCalibrationRequired)
                        _miniMap.gameObject.SetActive(true);

                    if ((!Tracking.activeSelf && _gm.IsCalibrationRequired && CalibrationGUI.CalibDone) || (!_gm.IsCalibrationRequired && LevelSet))
                    {
                        MiniMapBig();
                        _bg.SetActive(false);
                    }
                    else
                        _bg.SetActive(true);

                    CityCrossHair.SetActive(false);
                }
            }
            else
            {
                CityCrossHair.SetActive(false);
                _miniMap.gameObject.SetActive(false);
            
                if (!Minimized)
                    Bg.SetActive(true);
                else
                {
                        Bg.SetActive(false);
                }
            }
        }

        private void DrawItemsList()
        {
            if (Application.loadedLevelName == "GameOver")
                Destroy(gameObject);

            if (DrawingList && Application.loadedLevelName != "GameOver")
            {
                var items = GameObject.FindGameObjectsWithTag("Item");

                foreach (var t in items)
                    Destroy(t);
                
                _description = _objManager.GetCurrentObjective == null ? " " : _objManager.GetCurrentObjective.Description;

                var newItem = Instantiate(Item) as GameObject;

                if (newItem != null)
                {
                    newItem.transform.SetParent(_verticalLayout.transform, false);
                    newItem.SetActive(true);

                    var itemText = newItem.GetComponentInChildren<Text>();
                
                    itemText.text = _description;
                }

                DrawingList = false;
            }
        }

        public void EnableB1(bool b1)
        {
            if (b1)
            {
                _animLocationReached.Play("ObjectiveDisplayAnim2");
                
                if (_objManager.GetCurrentObjective.name == "SuperMarket")
                    _timer = 16.0f;
                else
                    _timer = TimeToNextStep;

                _canEnterLocation = true;

                if (!_gm.IsCalibrationRequired)
                {
                    _continueTimer = 3f;
                    CanActivateDelayTimer = false;
                }
            }
        }

        private void Update()
        {
            TaskTime += Time.deltaTime;
            InterfaceStates();

            if (Controller.Controller.B4())
                ShowMiniMap = !ShowMiniMap;

            if (Controller.Controller.B7())
                ShowObjectiveWindow = !ShowObjectiveWindow;

            if (ShowObjectiveWindow)
            {
                ObjectivesWindow.SetActive(true);
                DrawItemsList();
            }
            else
            {
                ObjectivesWindow.SetActive(false);
            }

            if (Application.loadedLevel != 2)
                _miniMap.gameObject.SetActive(false);

            if (!Minimized && LevelSet && Application.loadedLevelName != "GameOver")
            {
                if ((!Tracking.activeSelf && _gm.IsCalibrationRequired && CalibrationGUI.CalibDone) || !_gm.IsCalibrationRequired)
                {
                    _objDescription.gameObject.SetActive(true);
                    _animDescription.enabled = true;
                }

                _description = _objManager.GetCurrentObjective == null ? " " : _objManager.GetCurrentObjective.Description;

                if (_objManager.GetCurrentObjective != null)
                {
                    _description = _objManager.GetCurrentObjective.Completed ? /*_language.WellDoneText()*/ LanguageManager.Instance.WellDoneText() : _objManager.GetCurrentObjective.Description;
                    _description = _objManager.LevelComplete ? LanguageManager.Instance.LevelCompleteString()/*.Replace("@", _objManager.PreviousLevel.ToString())*/ : _description;
                }
                else
                    _description = " ";
            
                _descriptionText.text = _description;
                if (_gm.IsCalibrationRequired && CalibrationGUI.CalibDone && !_canMinimize)
                {
                    BtnText.text = _timer.ToString("0");
                    _timer -= Time.deltaTime;
                    
                    if (_timer <= 0)
                    {
                        _canMinimize = true;
                        
                        MinimizeInterface(true);
                    }
                }
                else if(!_gm.IsCalibrationRequired && !_canMinimize)
                {
                    if (CanActivateDelayTimer)
                        _continueTimer -= Time.deltaTime;

                    if (_continueTimer < 0)
                    {
                        _canMinimize = true;
                        BtnText.text = Language.pressKey;
                    }
                    else
                    {
                        BtnText.text = " ";
                        _canMinimize = false;
                    }
                }
            }

            if (LevelSet)
            {
                if (_objManager.GetCurrentObjective != null && _objManager.GetCurrentObjective.Completed)
                {
                    Minimized = false;
                    AllObjectives = false;
                }
            }

            if (Controller.Controller.B1())
            {
                if (!Minimized)
                {
                    //Debug.Log("Minimizing");
                    MinimizeInterface(true);
                    
                }
            }

            if (EnterRoom.ReachedLocation)
            {
                _objManager.GetCurrentObjective.ObjTime = TaskTime;
                NavigationTimer.NavigationCowntdown = false;
                _animLocationReached.enabled = true;
                LocationLabel.SetActive(true);
                LocLabelText.text = EnterRoom.TextLocation; //"Welcome to " + _objManager.GetCurrentObjective.name;

                if (_gm.IsCalibrationRequired)
                {
                    LocationBtnText.text = _timer.ToString("0");
                    _timer -= Time.deltaTime;
                    if (_timer <= 0)
                    {
                        EnableB1(true);
                    }
                }
                else
                    LocationBtnText.text = Language.pressKey;

            }
            else if (!EnterRoom.ReachedLocation || Application.loadedLevel != 2)
            {
                LocationLabel.SetActive(false);
            }

            if (_canEnterLocation && _animLocationReached.gameObject.GetComponent<RectTransform>().position.x > Screen.width * 1.2f)
            {
                TimerCount.ControllerB1 = true;
                _canEnterLocation = false;
            }

            if (_goToNextStep && _animDescription.gameObject.GetComponent<RectTransform>().position.x > Screen.width * 1.2f)
            {
                DrawingList = true;
                Minimized = true;
                MinimizeObjective = !MinimizeObjective;
                CanGoToNextObj = true;
                
                _timer = TimeToNextStep;

                if (Application.loadedLevelName == "Home" || Application.loadedLevelName == "FashionStore" || Application.loadedLevelName == "Park")
                {
                    SpawnTiles.LevelName = "L" + _objManager.GetCurrentObjective.RehaTaskLevel;
                    General_Settings.TaskScenario = _objManager.GetCurrentObjective.RehaTaskScenario;
                    CalibrationGUI.CalibDone = true;
                    UdpReceive.Calib = false;
                    Main_Menu.StartFromRehabCity = true;
                }
                
                _animDescription.enabled = false;
                _goToNextStep = false;
            }
        }

    }
}
