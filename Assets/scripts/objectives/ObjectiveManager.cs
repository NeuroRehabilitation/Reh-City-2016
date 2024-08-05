using System.Collections.Generic;
using Assets.scripts.GUI;
using Assets.scripts.GUI.StartSCene;
using Assets.scripts.Manager;
using UnityEngine;

/* Attach this class to any gameobject
 * This class manages the addition of objectives
 * Check Objectives clas to know what it does
 * New Objectives are added to a queue and removed when Completed 
 * Coder : Kushal
*/
namespace Assets.scripts.objectives
{
    public class ObjectiveManager :MonoBehaviour{
	
        private GameManager _gamemanager;
        private InventoryManager _inventorymanager;
        // Variable for main Objective class which stores the current Objective spawned	
        private  Objectives _obj;
	
        // queue to store the currentobjective 
        // used by drawinventory to display text
        private Queue<Objectives> _objQueue;
	
        // readonly property that gives the entire objectivequeue as an array
        public Objectives[] GetObjQueue
        {
            get
            {
                return _objQueue.ToArray();
            
            }
        }
	
        // readonly property that gives the current Objective in the queue
        public Objectives GetCurrentObjective
        {
            get
            {
                if(_objQueue.Count>0)
                {
                    return _objQueue.ToArray()[_objQueue.Count-1];
                }
                else
                {
                    return null;
                }
            }
        }

        private List<Objectives> _levelList;
	
        // number of objectives Completed
        private int _completedObjectives;
	
        //elapsed time is used to set delay for showing a tickmark after an Objective is Completed
        private float _elapsedTime;
	
        // used to know when the Objective is in transition to other.
        [HideInInspector]
        public bool transition;

        private DrawObjectiveList _drawobjective;

        private int _score;

        public int GetScore
        {
            get
            {
                return _score;
            }
        }
    
        /// <summary>
        /// stores the level Completed
        /// </summary>
        public int PreviousLevel;
        /// <summary>
        /// stores the current level
        /// </summary>
        public int Level = 1;
        /// <summary>
        /// bool to know if a level has been Completed
        /// </summary>
        public bool LevelComplete;
        public int GetLevel
        {
            get { return Level; }
        }
    
        private DisplayScore _scoreDisplay;
        private static ObjectiveManager _sInstance = null;
        public static ObjectiveManager Instance
        {
            get
            {
                if (_sInstance == null)
                {
                    _sInstance = FindObjectOfType(typeof(ObjectiveManager)) as ObjectiveManager;
                    if (_sInstance == null)
                    {
                        Debug.Log("Could not locate ObjectiveManager");
                    }
                }
                return _sInstance;
            }
        }

        private GameObject _objList;

        private void Start()
        {
            _objList = GameObject.FindGameObjectWithTag("InterfacePrefab");
            _obj = new Objectives();
            _objQueue = new Queue<Objectives>();
            _drawobjective = _objList.GetComponent<DrawObjectiveList>();
            _inventorymanager = GetComponent<InventoryManager>();
            _gamemanager = GetComponent<GameManager>();
            _levelList = new List<Objectives>();
            _inventorymanager.AddCategory(InventoryManager.CategoryTypes.PostOffice);
            _inventorymanager.AddCategory(InventoryManager.CategoryTypes.SuperMarket);
            _inventorymanager.AddCategory(InventoryManager.CategoryTypes.Pharmacy);
            _scoreDisplay = DisplayScore.Instance;
        }

        /// <summary>
        /// set from StartSceneSettings
        /// </summary>
        /// <param name="_level"></param>
        public void GetLevelAndAddObjective(int _level)
        {
            
            Level = _level;
            GetNewObjectiveList();
            AddObjective(_completedObjectives);  
        }

        private void GetNewObjectiveList()
        {
            if (Level > 1)
                PerformanceProcessor.ProcessPerformance();
            
            if (PerformanceProcessor.CurrentProfileModel.Difficulty >= 10)
            {
                DrawObjectiveList.Instance.ShowObjectiveWindow = false;
                DrawObjectiveList.TimeToNextStep = 10.0f;
                
            }
            else
            {
                DrawObjectiveList.TimeToNextStep = 8.0f;
                DrawObjectiveList.Instance.ShowObjectiveWindow = true;
            }
            
            GameManager.Instance.ShowArrowObject = PerformanceProcessor.CurrentProfileModel.Difficulty > 8.5f && PerformanceProcessor.CurrentProfileModel.Difficulty < 9.5f;
            GameManager.Instance.ToggleArrowDisplay();
            DrawObjectiveList.ShowMiniMap = PerformanceProcessor.CurrentProfileModel.Difficulty >= 9f && PerformanceProcessor.CurrentProfileModel.Difficulty < 10.5f;
            CitySigns.ShowSigns = !(PerformanceProcessor.CurrentProfileModel.Difficulty >= 11);
            GameManager.ShowLabel = !(PerformanceProcessor.CurrentProfileModel.Difficulty >= 11.5f);

            ObjectiveInstatiation.CalculateTasksParameters(PerformanceProcessor.CurrentProfileModel);
            
            _levelList = ObjectiveInstatiation.GetObjectivesList();

        }

        // main function to add objectives 
        // follow the Sequence in which the objectives have to be spawned
        public void AddObjective(int objectiveNumber)
        {
            _obj = _levelList[objectiveNumber];
        }

        public void AddScore(int score)
        {
            _score += score;
            _scoreDisplay.UpdateScoreDisplay(this._score);
        }

        // set from startscene settings
        public void SetObjectiveLanguage(bool English, bool Portuguese)
        {
            _obj.SetLanguage(English, Portuguese);
        }
        /* checks for completion of objectives
	 * if Completed adds an other Objective to the queue
	*/
        private void UpdateObjectives()
        {
            if (_gamemanager.GameCompleted) return;
            // check for completion on the spawned Objective
            _obj.CheckForCompletion();

            if(_obj.Completed)
            {
                // sets the Objective window back to full screen.
                _drawobjective.MinimizeObjective = false;
                // Example : If you are in supermarket and Objective is to collect 3 milk. After that ojective is Completed
                // the game has to wait until you exit that scene to give an other Objective.
                if (_obj.CanAddNextObjective)
                {
                    _completedObjectives++;
                    AddScore(10);
                    // if the completedobjective is at the end of the list then 
                    // increase the level and get another list
                    
                    if (_completedObjectives > _levelList.Count - 1)
                    {
                        
                        PreviousLevel = Level;
                        LevelComplete = true;
                        Level++;
                        if (Level > _gamemanager.MaxLevel)
                        {
                            Level = _gamemanager.MaxLevel;
                        }
                        _completedObjectives = 0;
                        
                        if(Application.loadedLevelName == "City")
                            GetNewObjectiveList();
                    }
                    AddObjective(_completedObjectives);
                    
                }
                _elapsedTime = 0;
            }

            // Push the current Objective into stack after some delay
            // delay is used to show a tick mark on the Completed Objective
            _elapsedTime += Time.deltaTime;
            transition = (_objQueue.Count!=0 && _elapsedTime<3.0f)?true:false;
            if(_elapsedTime >= 3.0f && _elapsedTime < 3.1f)
            {
                if(!_gamemanager.IsCalibrationRequired)
                    DrawObjectiveList.CanActivateDelayTimer = true;

                if (_objQueue.Count==0 || (GetCurrentObjective!=_obj))
                {
                    _objQueue.Enqueue(_obj);
                    LevelComplete = false;
                }
                if (_objQueue.Count > 4)
                {
                    _objQueue.Dequeue();
                }
            }
        }

        private void Update()
        {
            UpdateObjectives();
            
            DrawObjectiveList.LevelSet = GetCurrentObjective!=null;
        }
    }
}
