using System.Collections.Generic;
using System.Linq;
using Assets.scripts.Camera;
using Assets.scripts.GUI;
using Assets.scripts.objectives;
using Assets.scripts.RehaTask.RehaTaskGUI;
using Assets.scripts.Settings;
using Assets.scripts.UDP;
using UnityEngine;

namespace Assets.scripts.RehaTask.RehaTask_GameLogic
{
    public class ChangeLevel : MonoBehaviour {

        public GameObject GUIchangelevel, IKTarget;
        public static GameObject GamePlayUi;
        public static int Temp, LevelNumber;
        public static bool AllLevels = false, MemoryAllLevels, AttentionAllLevels, LevelSet = false;
        public static float MyTimer = 1.0f;
        public static List<string> CompletedCats = new List<string>();
    
        public static bool ResetOnce;
        public static bool SetOnce, StopLog;

        public static bool ChangeSet = false;

        private GameObject _mger;
        private static ObjectiveManager _objManager;

        private void Awake()
        {
            GamePlayUi = GameObject.Find("GamePlayUI");
            _mger = GameObject.FindGameObjectWithTag("Manager");
            _objManager = _mger.GetComponent<ObjectiveManager>();
        }

        private void Update () 
        {
            if(CollisionDetection.ChangeLevel)//if level has been Completed checks if is the last one and enables the change level GUI
            {
                ReadLevel();
            
                MyTimer -= Time.deltaTime;
            
                if (MyTimer <= 0 && MyTimer > -1)
                {
                    MyTimer = -1;
                    GUIchangelevel.SetActive(true);
                
                    GamePlayUi.SetActive(false);
                }
            }
        }

        private void ReadLevel()
        {
            //reads the actual level and transform it into an int
            var actualLevel = SpawnTiles.LevelName;
            var numberL = actualLevel[1].ToString();//level in SpawnTiles script is a string so needs to be "split" to parse it into an int, the First number corresponds to the position 1 of the array of characters

            if (actualLevel.Length > 2)//if current level is higher than 9, or higher than 99 it adds the next characters posiotened on the array of characters
            {
                for (var i = 2; i < actualLevel.Length; i++)
                {
                    numberL = numberL + actualLevel[i];
                }
            }
            //parses the obtained string number into an int
            Temp = int.Parse(numberL);
        }

        //cleans variables and prepares application to receive a new level
        public static void ResetLevel()
        {
            if (ResetOnce)
            {
                Resources.UnloadUnusedAssets();
                GUIChangeLevel.Iaps = 0;
                Scoring.HalfScore = 0;
                Scoring.FullScore = 0;
                StopLog = true;
                SetOnce = true;

                if(!LevelSet)
                    SetLevel();
            
                Scoring.Audio1.Stop();
                Scoring.HasPlayed = false;
                GUIChangeLevel.ValenceRated = false;
                MainGUI.ActivateHint = false;
                MainGUI.ClearTargets = true;
                CollisionDetection.ActivateTimer = false;

                //deletes all cubes from previous level
                var tiles = GameObject.FindGameObjectsWithTag("Tile");
                if (tiles.Length > 0)
                {
                    foreach (var t in tiles)
                        Destroy(t);
                }
            
                MainGUI.AllImages = false;
                MainGUI.AllTargets = false;
                General_Settings.Enable = true;

                if (SpawnTiles.UseMemory)
                {
                    MainGUI.ImgTimer = SpawnTiles.PicTimer;
                    MainGUI.Picture = true;
                    MainGUI.PictureReady = true;
                }
                else
                {
                    if (!UdpReceive.Calib)
                    {
                        GamePlayUi.SetActive(true);
                        MainGUI.PictureReady = true;
                    }

                    MoveCamera.AnimStatus = true;
                    SpawnTiles.SpawnOnce = true;
                }
                /*
                if (Main_Menu.Uid != "" && CalibrationGUI.CalibDone)
                    CsvDataSave.StartLog = true;
            */
                ResetOnce = false;
            }
        }

        //sets the new level according to level number set on LevelChangeBehavior
        public static void SetLevel()
        {
            if (SetOnce)
            {
                MainGUI.ActivateHint = false;
                MainGUI.SettingLevel = true;
                Scoring.SetCorrect(0);
                Scoring.SetError(0);
                SpawnTiles.SelectionList.Clear();
                SpawnTiles.TexturesToRender.Clear();
                SpawnTiles.Hints.Clear();
                SpawnTiles.Incorrect.Clear();
                SpawnTiles.TexturesToExport.Clear();
                CollisionDetection.SelectedTexture = "NaN";
                
                Hints.HintActive = false;

                foreach (var t in LoadLevels.Levels.Where(t => SpawnTiles.LevelName == t.LevelName))
                {
                    
                    //SpawnTiles.Columns = t.Columns;
                    //SpawnTiles.Rows = t.Rows;
                    SpawnTiles.Columns = AutomaticParameters.CalculateGrid(_objManager.GetCurrentObjective.RehaTaskElements)[0];
                    SpawnTiles.Rows = AutomaticParameters.CalculateGrid(_objManager.GetCurrentObjective.RehaTaskElements)[1];

                    SpawnTiles.TotalToRender = SpawnTiles.Columns * SpawnTiles.Rows;
                    SpawnTiles.Targets = _objManager.GetCurrentObjective.RehaTaskTargets;
                    SpawnTiles.Correctchoices = _objManager.GetCurrentObjective.RehaTaskTargets;
                    
                    SpawnTiles.PicTimer = t.PicTimer;

                    SpawnTiles.Folder = t.Category;
                    if(SpawnTiles.LevelName == "L1")
                        SpawnTiles.Folder = t.Category + "-" + _objManager.GetCurrentObjective.RehaTaskScenario;

                    if (SpawnTiles.Folder == "Park")
                        SpawnTiles.Correctchoices = SpawnTiles.Targets*2;

                    if (SpawnTiles.Folder == "Home")
                        SpawnTiles.Folder = SpawnTiles.Folder + _objManager.GetCurrentObjective.RehaTaskScenario;

                    MainGUI.DisplayMiniGoal = _objManager.GetCurrentObjective.DisplayGoal;

                    SpawnTiles.UseMemory = t.UseMemory;
                    SpawnTiles.ImagesToDisplay = t.ImagesToDisplay;
                    
                    SpawnTiles.UseDistractors = !Main_Menu.CategDistractors && t.UseDistractors;
                    
                    SpawnTiles.Showcorrect = t.ShowCorrect;
                    SpawnTiles.Completed = t.Completed;
                    
                    SpawnTiles.ImageTc = t.ImageToConstruct;
                    SpawnTiles.Sequence = t.Sequence;
                    SpawnTiles.KeepThumbnail = t.KeepThumbnail;
                    //SpawnTiles.Timer = t.LevelTime;
                    SpawnTiles.Timer = _objManager.GetCurrentObjective.TaskTime;
                    SpawnTiles.Pairs = t.Pairs;
                    General_Settings.TaskScenario = _objManager.GetCurrentObjective.RehaTaskScenario;
                }
                MainGUI.TimeToDisplay = SpawnTiles.Timer;

                LoadGame.StartLoadingCatImages = true;

                if (!UdpReceive.Calib)
                {
                    if (SpawnTiles.UseDistractors)
                        SpawnTiles.StartLoadingDistrators = true;
                    else
                        LoadGame.DistractorsSet = true;
                }

                LevelSet = true;
                MainGUI.SettingLevel = false;
                SetOnce = false;
            }
        }
    }
}
