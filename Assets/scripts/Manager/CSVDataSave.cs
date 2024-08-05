using System;
using System.Globalization;
using System.IO;
using System.Xml;
using Assets.scripts.bank;
using Assets.scripts.Controller;
using Assets.scripts.GUI;
using Assets.scripts.Locations;
using Assets.scripts.objectives;
using Assets.scripts.objectives.Action;
using Assets.scripts.Player;
using Assets.scripts.RehaTask.RehaTaskGUI;
using Assets.scripts.RehaTask.RehaTask_GameLogic;
using Assets.scripts.UDP;
using UnityEngine;

namespace Assets.scripts.Manager
{
    public class CSVDataSave : MonoBehaviour
    {
        private string[] _generalheader = new string[12];
        private string[] _navigationHeader = new string[5];
        private string[] _bankHeader = new string[5];
        private string[] _rehaTaskHeader = new string[12];
        private string[] _collectionHeader = new string[13];
        private string[] _selectionHeader = new string[4];

        private string[] _generalValues = new string[12];
        private string[] _navigationValues = new string[5];
        private string[] _bankValues = new string[5];
        private string[] _rehaTaskValues = new string[12];
        private string[] _collectionValues = new string[13];
        private string[] _selectionValues = new string[4];

        private static ObjectiveManager _objManager;

        private TextWriter _file;
        private string _filepath = string.Empty;
        private string _path;
        public static bool Islogging;
        public static bool StartLog, StopLog;

        private string _objType = "NaN", _tipFile, _tempObjFile = "NaN", _stringObjTypeFull = "NaN";
        private bool _resetted;

        private void Start()
        {
            _objManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<ObjectiveManager>();

            _generalheader = new string[] {
                "TimeInSeconds",
                "TimeToFinishSession",
                "Scene",
                "PlayerPositionX",
                "PlayerPositionY",
                "PlayerPositionZ",
                "RawInputPositionX",
                "RawInputPositionY",
                "ObjectiveType",
                "ObjectiveCompletion",
                "Score",
                "ObjectiveWindow"
            };

            _navigationHeader = new string[]
            {
                "CitySigns",
                "MiniMap",
                "NavigationArrow",
                "PlayerRotation",
                "ReachedLocation",
            };

            _bankHeader = new string[]
            {
                "DigitsToFind",
                "SelectedKey",
                "CorrectKeys",
                "WrongKeys",
                "Amount/Bill/Sequence",
            };

            _rehaTaskHeader = new string[]
            {
                "LevelNumber",
                "Type",
                "Scenario",
                "Category",
                "ImagesToFind",
                "Columns",
                "Rows",
                "ObjectiveVisible",
                "HintsActive",
                "SelectedImage",
                "CorrectImages",
                "Wrong Images"
            };

            _collectionHeader = new string[]
            {
                "ItemsLabels",
                "SelectedObject",
                "CorrectObjects",
                "WrongObjects",
                "Quantity1stItem",
                "1stItemName",
                "Quantity2ndItem",
                "2ndItemName",
                "Quantity3rdItem",
                "3rdItemName",
                "Quantity4rdItem",
                "4rdItemName",
                "Abstraction"
            };

            _selectionHeader = new string[]
            {
                "Buttonselected",
                "Tips/QuestionsSubject",
                "Tips/QuestionsNumber",
                "CurrentTip",
                "Answer_ChosenReceipt"
            };

        }

        private void Update()
        {
            _generalValues = GetGeneralValues();
            
            if (_stringObjTypeFull.Contains("Location"))
                _navigationValues = GetNavigationValues();

            if (_objType.Contains("With") || _objType == "EnterCode" || _objType == "PayBill")
            {
                _bankValues = GetBankValues();
            }

            if (_objType == "PlayRehatask")
                _rehaTaskValues = GetRehaTaskValues();

            if (_objType.Contains("Collect") || _objType.Contains("andget"))
            {
                _collectionValues = GetCollectionValues();
            }

            if (_objType.Contains("Display"))
                _selectionValues = GetSelectionValues();

            if (StartLog)
            {
                if(!Islogging)
                    LogInit();
                else
                {
                    StopLog = true;
                }
            }

            if (StopLog && Islogging)
            {
                Islogging = false;
                _file.Close();
                StopLog = false;
            }

            if (Islogging)
                CSVWrite();
        }

        private void ResetForNewFile()
        {
            if (!_resetted)
            {
                Scoring.SetCorrect(0);
                Scoring.SetError(0);
                SpawnTiles.LevelName = "NaN";
                SpawnTiles.Folder = "NaN";
                SpawnTiles.Columns = 0;
                SpawnTiles.Rows = 0;
                SpawnTiles.Correctchoices = 0;
                Hints.HintActive = false;
                CollisionDetection.SelectedTexture = "NaN";
                MainGUI.ObjectiveVisible = "NaN";
                ReceiptsSetup.Answer = "NaN";
                QuestionsNavigation.Answer = "NaN";
                StartLog = true;
                _resetted = true;
            }
        }

        private string[] GetGeneralValues()
        {
            var generalValues = new string[12];
            var hours = int.Parse(DateTime.Now.ToString("HH")) * 60 * 60;
            var minutes = int.Parse(DateTime.Now.ToString("mm")) * 60;
            var seconds = int.Parse(DateTime.Now.ToString("ss"));
            var totalSeconds = hours + minutes + seconds;
            var temp = totalSeconds + "." + DateTime.Now.Millisecond.ToString("000");

            var completed = 0;
            if (_objManager.GetCurrentObjective != null)
                completed = _objManager.GetCurrentObjective.Completed ? 1 : 0;
            
            if (_objManager.GetCurrentObjective != null)
            {
                _stringObjTypeFull = _objManager.GetCurrentObjective.GetType().ToString();
                var objTypeFull = _stringObjTypeFull.Split("." [0]);
                _objType = objTypeFull[objTypeFull.Length - 1];
                if (_tempObjFile != _objType)
                {
                    if ((GameManager.Instance.IsCalibrationRequired && CalibrationGUI.CalibDone) ||
                        !GameManager.Instance.IsCalibrationRequired)
                    {
                        _resetted = false;
                        ResetForNewFile();
                        _tempObjFile = _objType;
                    }
                }
            }

            generalValues[0] = temp;//actual time in seconds
            generalValues[1] = GameTimer.SessionTime().ToString();//missing session time
            generalValues[2] = Application.loadedLevelName;

            if (Application.loadedLevelName == "City")
            {//game position of the player
                generalValues[3] = GameObject.FindGameObjectWithTag("Player").transform.position.x.ToString(CultureInfo.InvariantCulture);//player position x
                generalValues[4] = GameObject.FindGameObjectWithTag("Player").transform.position.y.ToString(CultureInfo.InvariantCulture);//player position y
                generalValues[5] = GameObject.FindGameObjectWithTag("Player").transform.position.z.ToString(CultureInfo.InvariantCulture);//player position z
            }
            else
            {//in other scenes the actual player position coresponds to the controller position
                generalValues[3] = CP_Controller.Instance.transform.position.x.ToString(CultureInfo.InvariantCulture);//player position x
                generalValues[4] = CP_Controller.Instance.transform.position.y.ToString(CultureInfo.InvariantCulture);//player position y
                generalValues[5] = CP_Controller.Instance.transform.position.y.ToString(CultureInfo.InvariantCulture);//player position z
            }
            
            generalValues[6] = UdpReceive.Rawposition()[0].ToString(CultureInfo.InvariantCulture);//raw position x
            generalValues[7] = UdpReceive.Rawposition()[1].ToString(CultureInfo.InvariantCulture);//raw position y
            generalValues[8] = _objType;//ObjectiveName
            generalValues[9] = completed.ToString();//objectiveCompletion(true = 1; false = 0)
            generalValues[10] = DisplayScore.ActualScore().ToString();//score
            generalValues[11] = DrawObjectiveList.Instance.ShowObjectiveWindow ? "1" : "0";//Objective Window (true = 1; false = 0)

            return generalValues;
        }

        private string[] GetNavigationValues()
        {
            var navigationValues = new string[5];
            
            navigationValues[0] = CitySigns.ShowSigns ? "1" : "0";//CitySigns (true = 1; false = 0)
            navigationValues[1] = DrawObjectiveList.ShowMiniMap ? "1" : "0";//Minimap (true = 1; false = 0)
            navigationValues[2] = GameManager.Instance.ShowArrowObject ? "1" : "0";//NavigationArrow (true = 1; false = 0)
            navigationValues[3] = MouseLook.PlayerRotation().ToString(CultureInfo.InvariantCulture);//PlayerRotation
            navigationValues[4] = EnterRoom.LocationReached().ToString();//reached location (true = 1; false = 0)

            return navigationValues;
        }

        private string[] GetBankValues()
        {
            var bankValues = new string[5];

            var answerSet = "NaN";
            if (_objManager.GetCurrentObjective != null && _objManager.GetCurrentObjective.AnswerSet != null && _objManager.GetCurrentObjective.AnswerSet.Count > 0)
            {
                answerSet = _objManager.GetCurrentObjective.AnswerSet[0];
                for (var i = 1; i < _objManager.GetCurrentObjective.AnswerSet.Count; i++)
                {
                    answerSet = answerSet + "-" + _objManager.GetCurrentObjective.AnswerSet[i];
                }
            }
            
            var bankAction = "NaN";
            if (_objManager.GetCurrentObjective != null && !_objManager.GetCurrentObjective.Description.Contains("Code"))
            {
                if(_objManager.GetCurrentObjective.ButtonToPress != "")
                    bankAction = _objManager.GetCurrentObjective.ButtonToPress;
            }
            else if (_objManager.GetCurrentObjective != null && _objManager.GetCurrentObjective.NumericSequence != "")
            {
                bankAction = _objManager.GetCurrentObjective.NumericSequence;
            }

            bankValues[0] = answerSet;//DigitsToFind split by "-"
            bankValues[1] = CP_Controller.SelectedObjectName();//SelectedKey
            bankValues[2] = GameManager.CorrectChoices().ToString();//CorrectKeys
            bankValues[3] = GameManager.WrongChoices().ToString();//WrongKeys 
            bankValues[4] = bankAction;//Amount to withdraw/Bill to pay
            
            return bankValues;
        }

        private string[] GetRehaTaskValues()
        {
            var rehaTaskValues = new string[12];
 
            var hintsActive = Hints.HintActive ? 1 : 0;
            
            rehaTaskValues[0] = SpawnTiles.LevelName;//LevelNumber
            rehaTaskValues[1] = _objManager.GetCurrentObjective.Type;//Type
            rehaTaskValues[2] = _objManager.GetCurrentObjective.RehaTaskScenario.ToString();// Scenario
            rehaTaskValues[3] = SpawnTiles.Folder;//Category
            rehaTaskValues[4] = SpawnTiles.Correctchoices.ToString();//ImagesToFind
            rehaTaskValues[5] = SpawnTiles.Columns.ToString();//Columns
            rehaTaskValues[6] = SpawnTiles.Rows.ToString();//Rows
            rehaTaskValues[7] = MainGUI.ObjectiveVisible;//ObjectiveVisible(BigDisplay or NaN - if use memory, else, Thumbnail or SmallTextDisplay)
            rehaTaskValues[8] = hintsActive.ToString();//HintsActive
            rehaTaskValues[9] = CollisionDetection.SelectedTexture; //SelectedImage
            rehaTaskValues[10] = Scoring.GetCorrect().ToString();//CorrectImages
            rehaTaskValues[11] = Scoring.GetError().ToString();//Wrong Images

            return rehaTaskValues;
        }

        private string[] GetCollectionValues()
        {
            var collectionValues = new string[13];

            var firstQuantity = _objManager.GetCurrentObjective.NumberofFirstItem;
            var firstItem = _objManager.GetCurrentObjective.FirstItemName;
            

            collectionValues[0] = GameManager.ShowLabel ? "1" : "0";//ItemsLabels (true = 1; false = 0)
            collectionValues[1] = CP_Controller.SelectedObjectName();//SelectedObject
            collectionValues[2] = GameManager.CorrectChoices().ToString();//CorrectObjects
            collectionValues[3] = GameManager.WrongChoices().ToString();//WrongObjects
            collectionValues[4] = firstQuantity.ToString();//Quantity1stItem
            collectionValues[5] = firstItem;//1stItemName
            collectionValues[6] = _objManager.GetCurrentObjective.NumberofSecondItem.ToString();//Quantity2ndItem
            collectionValues[7] = _objManager.GetCurrentObjective.SecondItemName;//2ndItemName
            collectionValues[8] = _objManager.GetCurrentObjective.NumberofThirdItem.ToString();//Quantity3rdItem
            collectionValues[9] = _objManager.GetCurrentObjective.ThirdItemName;//3rdItemName
            collectionValues[10] = _objManager.GetCurrentObjective.NumberofFourthItem.ToString();//Quantity4rdItem
            collectionValues[11] = _objManager.GetCurrentObjective.FourthItemName;//4rdItemName
            collectionValues[12] = _objManager.GetCurrentObjective.Abstraction;//Abstraction (Meal or true = 1 or false = 0)

            return collectionValues;
        }

        private string[] GetSelectionValues()
        {
            var selectionValues = new string[6];

            var currentSubj = "NaN";
            var currentTip = "NaN";
            var answer = "NaN";

            if (_objManager.GetCurrentObjective.GetType().ToString().Contains("Tip") && TipsNavigation.ActualSubject != null)
            {
                currentSubj = TipsNavigation.ActualSubject;
                currentTip = TipsNavigation.ActualTip.ToString();
            }
            else if (_objManager.GetCurrentObjective.GetType().ToString().Contains("Question") && QuestionsNavigation.ActualSubject != null)
            {
                currentSubj = QuestionsNavigation.ActualSubject;
                answer = QuestionsNavigation.Answer;
            }
            else
            {
                answer = ReceiptsSetup.Answer;
            }
            
            selectionValues[0] = CP_Controller.SelectedObjectName();//Buttonselected
            selectionValues[1] = currentSubj;//Tips_QuestionsSubject
            selectionValues[3] = currentTip;//CurrentTip -> xml file
            selectionValues[4] = answer;//Answer_ChosenReceipt

            return selectionValues;
        }
        
        //initiates the csv File
        private void LogInit()
        {
            _path = Application.dataPath + "/RehaCity_Log/" + Main_Menu.Uid + "/" + Main_Menu.Uid + "_" + DateTime.Now.ToString("yyyyMMdd") + "/";
            if (!Directory.Exists(_path))
            {
                System.IO.Directory.CreateDirectory(_path);
            }

            _filepath = _path + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + _objType + ".csv";

            if (_objType.Contains("TipDisplay"))
                _tipFile = _filepath;
            
            _file = _objType.Contains("Question") && _tipFile != null? new StreamWriter(_tipFile, true) : new StreamWriter(_filepath, true);
            
            //builds the string that will be the header of the csv File
            var header = _generalheader[0];

            for (var i = 1; i < _generalheader.Length; i++)
            {
                 header = header + "," + _generalheader[i];
            }

            var sceneHeader = new string[] {};
            string[] aditionalHeader;

            if (!_stringObjTypeFull.Contains("Locationand"))
            {
                if (_stringObjTypeFull.Contains("Location"))
                {
                    sceneHeader = _navigationHeader;
                }
                else if (_objType.Contains("With") || _objType == "EnterCode" || _objType == "PayBill")
                {
                    sceneHeader = _bankHeader;
                }
                else if (_objType == "PlayRehatask")
                {
                    sceneHeader = _rehaTaskHeader;
                }
                else if (_objType.Contains("Collect"))
                {
                    sceneHeader = _collectionHeader;
                }
                else if (_objType.Contains("Display"))
                {
                    sceneHeader = _selectionHeader;
                }
                aditionalHeader = null;
            }

            else
            {
                sceneHeader = _navigationHeader;
                if(_stringObjTypeFull.Contains("andCollection"))
                    aditionalHeader = _collectionHeader;
                else
                    aditionalHeader = _bankHeader;
            }

            for (var i = 0; i < sceneHeader.Length; i++)
            {
                header = header + "," + sceneHeader[i];
            }

            if (aditionalHeader != null)
            {
                for (var i = 0; i < aditionalHeader.Length; i++)
                {
                    header = header + "," + aditionalHeader[i];
                }
            }
            //writes the First line of the File (header)
            _file.WriteLine(header);

            
            StartLog = false;
            Islogging = true;
        }
        
    //writes new line in the csv File
        private void CSVWrite()
        {
            var newLine = _generalValues[0];

            for (var i = 1; i < _generalValues.Length; i++)
            {
                newLine = newLine + "," + _generalValues[i];
            }

            var sceneValues = new string[] {};
            string[] aditionalValues;

            if (!_stringObjTypeFull.Contains("Locationand"))
            {
                if (_stringObjTypeFull.Contains("Location"))
                {
                    sceneValues = _navigationValues;
                }
                else if (_objType.Contains("With") || _objType == "EnterCode" || _objType == "PayBill")
                {
                    
                    sceneValues = _bankValues;
                }
                else if (_objType == "PlayRehatask")
                {
                    sceneValues = _rehaTaskValues;
                }
                else if (_objType.Contains("Collect"))
                {
                    sceneValues = _collectionValues;
                }
                else if (_objType.Contains("Display"))
                {
                    sceneValues = _selectionValues;
                }
                aditionalValues = null;
            }
            else
            {
                sceneValues = _navigationValues;
                if (_stringObjTypeFull.Contains("andCollection"))
                    aditionalValues = _collectionValues;
                else
                    aditionalValues = _bankValues;
            }

            

            for (var i = 0; i < sceneValues.Length; i++)
            {
                newLine = newLine + "," + sceneValues[i];
            }

            if (aditionalValues != null)
            {
                for (var i = 0; i < aditionalValues.Length; i++)
                {
                    newLine = newLine + "," + aditionalValues[i];
                }
            }
            _file.Write(newLine);
            _file.WriteLine("");
        }
    }
}