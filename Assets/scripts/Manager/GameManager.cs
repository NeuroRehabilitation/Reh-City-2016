using Assets.scripts.bank;
using Assets.scripts.collectibles;
using Assets.scripts.Controller;
using Assets.scripts.GUI;
using Assets.scripts.Locations;
using Assets.scripts.MiniMapScripts;
using Assets.scripts.MiniMapScripts.AStar;
using Assets.scripts.objectives;
using Assets.scripts.Player;
using Assets.scripts.RehaTask.RehaTask_GameLogic;
using Assets.scripts.UDP;
using UnityEngine;

/* Game Manager is the heart of this game
 * Main actions in the game are controlled by this class
*/

namespace Assets.scripts.Manager
{
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// use this to stop reset function 
        /// for the First time game loads City and continue after the game has loaded.
        /// </summary>
        public static bool Pausegame = false;

        private bool _findEmpty;
        private InventoryManager _inventorymanager;
        private DrawObjectiveList _drawobjective;
        private ObjectiveManager _objmanager;
        private ObjectiveIndicator _objindicator;
        private CP_Controller _playerselect;
        private PathFindingController _pathfinding;
        private Vector3 _screenpoint;
        private Vector3 _objectFloatingPoint;
        private Transform _player;
        private FPSInputController _playermovementinput;
        private MouseLook _playerrotationinput;
        private CalibrationManager _calibmanager;
        [HideInInspector] public bool CanReset = false;

        public struct PlayerData
        {
            public static Vector3 Playerposition;
            public static Vector3 Playerrotation;
        };

        public GameObject PlusOne;
        public GameObject WrongMark;
        public int MaxLevel;
        [HideInInspector] public bool GameCompleted = false;

        private GameObject _points;
        private GameObject _timer;
        private GameObject _datacollector;

        /// <summary>
        /// set from start scene Setting
        /// </summary>
        [HideInInspector] public bool ShowArrowObject = true;

        [HideInInspector] public bool IsCalibrationRequired;

        public bool IsAndroid = false;

        private AudioClip _correctClip;
        private AudioClip _errorClip;

        public Transform AnalogBG;
        public Transform Analog;
        public Transform DigitalButtonKey;
        public Transform DigitalIKey;
        public Transform DigitalSKey;

        private static GameManager _sInstance = null;

        public static GameManager Instance
        {
            get
            {
                if (_sInstance == null)
                {
                    _sInstance = FindObjectOfType(typeof (GameManager)) as GameManager;
                    if (_sInstance == null)
                    {
                        Debug.Log("Could not locate GameManager");
                    }
                }
                return _sInstance;
            }
        }

        private static int _correctChoices;
        private static int _wrongChoices;
        public static bool ShowLabel;

        private void Start()
        {
            DontDestroyOnLoad(this.gameObject);
            MaxLevel = 5;
            _player = GameObject.Find("NewPlayer").transform;
            _playermovementinput = _player.GetComponentInChildren<FPSInputController>();
            _playerrotationinput = _player.GetComponentInChildren<MouseLook>();
            _inventorymanager = GetComponent<InventoryManager>();
            _objmanager = ObjectiveManager.Instance;
            _drawobjective = DrawObjectiveList.Instance;
            _objindicator = ObjectiveIndicator.Instance;
            _playerselect = CP_Controller.Instance;
            _pathfinding = PathFindingController.Instance;
            _calibmanager = CalibrationManager.Instance;
            _objectFloatingPoint = new Vector3(Screen.width - 100, Screen.height - 200, 0);
            GetComponent<AudioSource>().volume = 0.5f;
            _correctClip = Resources.Load("Sounds/Effects/Correct") as AudioClip;
            _errorClip = Resources.Load("Sounds/Effects/Error") as AudioClip;
            _points = DisplayScore.Instance.gameObject;
            _timer = GameTimer.Instance.gameObject;
            _datacollector = GameObject.Find("DataCollector");
        }

        /// <summary>
        /// initially called from start scene Setting
        /// </summary>
        public void ToggleArrowDisplay()
        {
            _player.FindChild("SimpleController")
                .FindChild("arrow")
                .GetComponent<CompassArrow>()
                .ToggleArrowDisplay(ShowArrowObject);
        }

        private void ResetAfterLevelLoads()
        {
            _player = GameObject.Find("NewPlayer").transform;
            ToggleArrowDisplay();
            _playermovementinput = _player.GetComponentInChildren<FPSInputController>();
            _playerrotationinput = _player.GetComponentInChildren<MouseLook>();
            _calibmanager = CalibrationManager.Instance;
            _objindicator = GameObject.Find("ObjectiveIndicator").GetComponent<ObjectiveIndicator>();
            _playerselect = CP_Controller.Instance;
            _pathfinding = PathFindingController.Instance;
            SetPlayerPosition();
            _correctClip = Resources.Load("Sounds/Effects/Correct") as AudioClip;
            _errorClip = Resources.Load("Sounds/Effects/Error") as AudioClip;
            //_correctChoices = 0;
            //_wrongChoices = 0;
        }

        // Everytime the New City is loaded the player is set to data in Playerdata that is accessed before 
        // player enters into postoffice, supermarket , pharmacy etc...
        public void SetPlayerPosition()
        {
            //print(PlayerData.Playerrotation);
            _player.position = PlayerData.Playerposition;
            _player.FindChild("SimpleController").transform.localEulerAngles = PlayerData.Playerrotation;
            //_player.FindChild("SimpleController").transform.rotation = Quaternion.LookRotation(NodeManager.Instance.GetStartNode().position);
            //_player.FindChild("SimpleController").transform.rotation = Quaternion.LookRotation(PathFindingController.NextNode());
            //_player.position = new Vector3(NodeManager.Instance.GetNearestNode().position.x, 3, NodeManager.Instance.GetNearestNode().position.z);

            //_player.FindChild("SimpleController").transform.localEulerAngles = new Vector3(0, 0, 0);
        }

        private void PausePlayer()
        {
            if (_drawobjective == null) return;
            if (!EnterRoom.ReachedLocation)
            {
                _playermovementinput.canMove = _drawobjective.MinimizeObjective ? true : false;
                _playerrotationinput.canRotate = _drawobjective.MinimizeObjective ? true : false;
            }
            else
            {
                _playermovementinput.canMove = false;
                _playerrotationinput.canRotate = false;
            }
        }

        private string _levelloadedname = null;
        //private char _;

        private void OnLevelWasLoaded(int level)
        {

            _levelloadedname = Application.loadedLevelName;

            if (_levelloadedname == "City")
            {
                // if it game just started do not reset
                if (!CanReset) return;
                ResetAfterLevelLoads();
            }
        }

        // Everytime a new Objective is added by the Objective manager it has to be updated on minimap
        // this method does that.
        // Ask pathfinder for a Path
        private void SetObjectivePosition()
        {
            if (_objmanager.GetCurrentObjective == null) return;
            if (_objindicator == null || _objmanager == null) return;
            if (_objindicator.CurrentObjPosition != _objmanager.GetCurrentObjective.Location)
            {
                _objindicator.CurrentObjPosition = _objmanager.GetCurrentObjective.Location;
                _pathfinding.FindPath();
            }
        }

        private void UpdateNumericSequence()
        {
            var chars = GameObject.FindGameObjectsWithTag("char");
            foreach (var t in chars)
            {
                Destroy(t);
            }
            BankManager.SequenceInstantiated = false;

            var tempNumericSequence = new string[_objmanager.GetCurrentObjective.NumericSequence.Length];

            for (var i = 0; i < _objmanager.GetCurrentObjective.NumericSequence.Length; i++)
            {
                tempNumericSequence[i] = _objmanager.GetCurrentObjective.NumericSequence[i].ToString();
            }

            for (var i = 0; i < _objmanager.GetCurrentObjective.NumericSequence.Length; i++)
            {
                var tempChar = _objmanager.GetCurrentObjective.NumericSequence[i].ToString();

                if (tempChar == "_" && !_findEmpty)
                {
                    tempNumericSequence[i] = _objmanager.GetCurrentObjective.AnswerSet[0];
                    _findEmpty = true;
                }
            }

            _objmanager.GetCurrentObjective.NumericSequence = tempNumericSequence[0];

            for (var i = 1; i < tempNumericSequence.Length; i++)
            {
                _objmanager.GetCurrentObjective.NumericSequence = _objmanager.GetCurrentObjective.NumericSequence +
                                                                  tempNumericSequence[i];
            }
        }

        private void SelectObjects()
        {
            if (Controller.Controller.B1() && _playerselect.Selected &&
                !_objmanager.GetCurrentObjective.ToString().Contains("With") &&
                !_objmanager.GetCurrentObjective.ToString().Contains("Pay"))
            {
                if (Application.loadedLevelName == "Kiosk")
                {
                    switch (_playerselect.SelectedObject.name)
                    {
                        //case "PreviousButton":
                        //    TipsNavigation.PreviousTip();
                        //    break;
                        case "NextButton":
                            TipsNavigation.NextTip();
                            break;
                        case "EndButton":
                            TipsNavigation.NextTip();
                            break;
                    }
                }
                else if (Application.loadedLevelName == "Questions")
                {
                    switch (_playerselect.SelectedObject.name)
                    {
                        case "YesButton":
                            QuestionsNavigation.SaveAnswer("true");
                            break;
                        case "NoButton":
                            QuestionsNavigation.SaveAnswer("false");
                            break;
                    }
                }
                else if (Application.loadedLevelName == "SMReceipt")
                {
                    switch (_playerselect.SelectedObject.name)
                    {
                        case "Receipt1":
                            ReceiptsSetup.SaveAnswer("Receipt1");
                            break;
                        case "Receipt2":
                            ReceiptsSetup.SaveAnswer("Receipt2");
                            break;
                    }
                }
                else if (Application.loadedLevelName == "Bank" &&
                         _objmanager.GetCurrentObjective.ToString().Contains("EnterCode"))
                {
                    _playerselect.AtmButton = false;
                    _playerselect.Selected = false;
                    _playerselect.Timer.SetActive(false);
                    _playerselect._timerActivated = false;
                    _playerselect.SelectedArrow = false;
                    TimerCount.MyTimer = TimerCount.StartTime;
                    TimerCount.Timeout = false;

                    if (_objmanager.GetCurrentObjective.AnswerSet.Count != 0)
                    {
                        if (_objmanager.GetCurrentObjective.AnswerSet[0] == _playerselect.SelectedObject.name)
                        {
                            _correctChoices ++;
                            _findEmpty = false;

                            if (!BankManager.BankOptionsReady)
                                UpdateNumericSequence();

                            _objmanager.GetCurrentObjective.AnswerSet.RemoveAt(0);

                            Instantiate(PlusOne,
                                new Vector3(_playerselect.SelectedObject.transform.position.x,
                                    _playerselect.SelectedObject.transform.position.y + 3,
                                    _playerselect.SelectedObject.transform.position.z), Quaternion.identity);
                            GetComponent<AudioSource>().PlaySound(_correctClip);
                            _objmanager.AddScore(1);
                        }
                        else
                        {
                            _wrongChoices ++;
                            Instantiate(WrongMark,
                                new Vector3(_playerselect.SelectedObject.transform.position.x,
                                    _playerselect.SelectedObject.transform.position.y,
                                    _playerselect.SelectedObject.transform.position.z - 5), Quaternion.identity);
                            GetComponent<AudioSource>().PlaySound(_errorClip);
                            _playerselect.WriteGitterObjectName = _playerselect.SelectedObject.name;
                            _objmanager.AddScore(-1);
                            return;
                        }
                        _playerselect.SelectedObject = null;
                    }
                }
                TimerCount.ControllerB1 = false;
                //_playerselect.Selected = false;
                _playerselect.Timer.SetActive(false);
                //_playerselect._timerActivated = false;
                //TimerCount.MyTimer = TimerCount.StartTime;
            }
        }

        // after player approaches near a collectible Item he has to collect the Item
        private void CollectObjects()
        {
            if (_playerselect == null) _playerselect = CP_Controller.Instance;

            if (_objmanager.GetCurrentObjective != null)
            {
                // if the player's cursor is on object and if b1 button is pressed
                if (Controller.Controller.B1() && _playerselect.SelectedObject != null &&
                    _playerselect.SelectedObject.tag == "item_1")
                {

                    if (_objmanager.GetCurrentObjective.AnswerSet.Count != 0 &&
                        !_objmanager.GetCurrentObjective.AnswerSet.Contains(
                            _playerselect.SelectedObject.GetComponent<CollectibleItem>().ItemName.ToString()))
                    {

                        _wrongChoices ++;
                        Instantiate(WrongMark,
                            new Vector3(_playerselect.SelectedObject.transform.position.x,
                                _playerselect.SelectedObject.transform.position.y,
                                _playerselect.SelectedObject.transform.position.z - 5), Quaternion.identity);
                        GetComponent<AudioSource>().PlaySound(_errorClip);
                        _playerselect.WriteGitterObjectName = _playerselect.SelectedObject.name;
                        _objmanager.AddScore(-1);
                        return;
                    }

                    _inventorymanager.AddItem(
                        _playerselect.SelectedObject.GetComponent<CollectibleItem>().ItemType.ToString(),
                        _playerselect.SelectedObject.GetComponent<CollectibleItem>().ItemName.ToString());

                    if (_objmanager.GetCurrentObjective != null && _objmanager.GetCurrentObjective.CollectedSet != null)
                    {
                        if (
                            _objmanager.GetCurrentObjective.CollectedSet.ContainsKey(
                                _playerselect.SelectedObject.GetComponent<CollectibleItem>().ItemName.ToString()))
                        {
                            _objmanager.GetCurrentObjective.CollectedSet[
                                _playerselect.SelectedObject.GetComponent<CollectibleItem>().ItemName.ToString()] += 1;
                        }
                        else
                        {
                            _objmanager.GetCurrentObjective.CollectedSet.Add(
                                _playerselect.SelectedObject.GetComponent<CollectibleItem>().ItemName.ToString(), 1);
                        }
                    }

                    // get the screen point
                    _screenpoint = UnityEngine.Camera.main.ScreenToWorldPoint(_objectFloatingPoint);

                    // set the screen point
                    _playerselect.WriteGitterObjectName = _playerselect.SelectedObject.name;
                    _playerselect.SelectedObject.GetComponent<CollectibleItem>().MovePoint = _screenpoint;
                    _playerselect.SelectedObject.GetComponent<CollectibleItem>().CanMove = true;
                    if (_objmanager.GetCurrentObjective != null)
                        _objmanager.GetCurrentObjective.NumberofItemsCollected += 1;
                    Instantiate(PlusOne, _playerselect.SelectedObject.transform.position, Quaternion.identity);

                    GetComponent<AudioSource>().PlaySound(_correctClip);
                    _objmanager.AddScore(1);
                    _correctChoices ++;
                    // destroy that object after collecting  
                    Destroy(_playerselect.SelectedObject, 3.0f);
                    TimerCount.ControllerB1 = false;
                }
            }
        }

        private void CleanUpObjects()
        {
            Destroy(gameObject);
            Destroy(_points);
            Destroy(_timer);
            Destroy(_datacollector);
            Destroy(_calibmanager);

#if UNITY_ANDROID
        Destroy(_analogtrans.gameObject);
#endif
            ResumeGame();
        }

        private void PauseGame()
        {
            Pausegame = true;
            Time.timeScale = 0;
            _playermovementinput.pausegame = true;
            Debug.Log("Game Paused");

        }

        private void ResumeGame()
        {
            Pausegame = false;
            Time.timeScale = 1;
            _playermovementinput.pausegame = false;
        }

        private void SendPlayerInput()
        {
            if (IsAndroid && _playermovementinput != null && _playerrotationinput != null && !IsCalibrationRequired)
            {
                _playermovementinput.IsAndroid = true;
                _playerrotationinput.IsAndroid = true;
            }

            if (IsCalibrationRequired == false) return;

            // if we are using calibration then set it true in input classes so they
            // will not use the controller input.
            if (_playermovementinput != null && !_playermovementinput.IsCalibrationRequired)
            {
                _playermovementinput.IsCalibrationRequired = true;
                if (_playerrotationinput != null) _playerrotationinput.IsCalibrationRequired = true;
            }

            if (_playermovementinput != null && _playerrotationinput != null && CalibrationGUI.CalibDone)
            {
                _playermovementinput.VerticalAxis = UdpReceive.Target.transform.position.z;

                var rot = UdpReceive.Target.transform.position.x;

                if (rot < 0.1f && rot > -0.1f)
                    rot = 0;

                _playerrotationinput.HorizontalAxis = rot;
            }
        }

        private void CheckGameOver()
        {
            if ((GameTimer.GameTime < 0 && Application.loadedLevelName == "City") || Input.GetKeyDown(KeyCode.F2))
            {
                GameOverScreen.Score = _objmanager.GetScore;
                GameOverScreen.Level = _objmanager.GetLevel;
                Application.LoadLevel("GameOver");

                CleanUpObjects();
                DrawObjectiveList.Minimized = false;
            }
        }

        private void Update()
        {
            if (Controller.Controller.B5())
            {
                ShowArrowObject = !ShowArrowObject;
                ToggleArrowDisplay();
            }

            if (_objmanager.GetCurrentObjective != null && _objmanager.GetCurrentObjective.Completed)
            {
                _correctChoices = 0;
                _wrongChoices = 0;
            }

            SetObjectivePosition();

            //if (Application.loadedLevelName != "Bank" && Application.loadedLevelName != "Kiosk" &&
            //    Application.loadedLevelName != "Questions" && Application.loadedLevelName != "SMReceipt")
            if (Application.loadedLevelName == "SuperMarket" || Application.loadedLevelName == "Pharmacy" ||
                Application.loadedLevelName == "PostOffice")
            {

                CollectObjects();
            }


            SelectObjects();
            PausePlayer();
            SendPlayerInput();
            CheckGameOver();

            if (!Application.isEditor && !Pausegame)
            {
                //Screen.showCursor = false;
            }
            else
            {
                Cursor.visible = true;
            }

            if (Input.GetKeyDown(KeyCode.F1))
            {
                if (!Pausegame)
                    PauseGame();
                else
                    ResumeGame();
            }
        }

        public static int CorrectChoices()
        {
            return _correctChoices;
        }

        public static int WrongChoices()
        {
            return _wrongChoices;
        }

        
    }
}

