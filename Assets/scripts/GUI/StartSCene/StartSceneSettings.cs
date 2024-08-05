using System;
using System.Globalization;
using Assets.scripts.Controller;
using Assets.scripts.Manager;
using Assets.scripts.Models;
using Assets.scripts.objectives;
using Assets.scripts.RehaTask.RehaTaskGUI;
using Assets.scripts.Settings;
using Assets.scripts.UserProfile;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.scripts.GUI.StartSCene
{
    public class StartSceneSettings : MonoBehaviour
    {
        private static float _modAttValue = 5.0f;
        private static float _modMemValue = 5.0f;
        private static float _modExFunctValue = 5.0f;
        private static float _modLangValue = 5.0f;
        private static float _modDiffValue = 5.0f;

        private static int _moCaVsValue = 3;
        private static int _moCaNamValue = 2;
        private static int _moCaAtValue = 3;
        private static int _moCaLangValue = 2;
        private static int _moCaAbsValue = 1;
        private static int _moCaDelRecValue = 3;
        private static int _moCaOrientValue = 3;
        private static int _moCaEduValue;

        private static string _playerId ="";
        private bool _showMiniMap = true, _showCitySigns = true, _showItemsLabels = true;
        private bool _showObjectives = true;
        private bool _showArrow = true;
        private bool _english;
        private bool _portuguese = true;
        private bool _calibrationRequired;
        private string _levelloadedname;
   
        private readonly bool _canGameLoad = false;
    
        private bool _showLoading, _inputOptions;

        public GameObject mainMenu, controls, options, secondMenu, ControlsPT, ControlsEN, MocaProfilePanel, ModelProfilePanel;
        public Text loadingText, playText, quitText, controlsText, optionsText, MainMenuText;

        public Text ModAttText, ModMemText, ModExecText, ModLangText, ModDiffText;
        public Text ModAttValueText, ModMemValueText, ModExecValueText, ModLangValueText, ModDiffValueText;
        public Text MoCaVSText, MoCaNamText, MoCaAtText, MoCaLangText, MoCaAbsText, MoCaDelRecText, MoCaOrientText, MoCaEduText;
        public Text MoCaVSValueText, MoCaNamValueText, MoCaAtValueText, MoCaLangValueText, MoCaAbsValueText, MoCaDelRecValueText, MoCaOrientValueText, MoCaDifValueText;

        public InputField playerIDinput;
        public Toggle pt, en, calib, minimap, navigation, objective, leftHand, rightHand, citySigns, ItemsLabels, MoCaProfileToggle, ModelProfileToggle, EduMoCaToggle;

        //Text UI elements for enabling changing language
        public Text Id,
            EnterId,
            Calibration,
            Lang,
            Support,
            Signs,
            Labels,
            Minimap,
            Arrow,
            Objective,
            MoCaBtn,
            ModBtn;

        public static bool DoCalibration = false;

        private string _btnName;

        private void Start ()
        {
            loadingText.text = " ";

            UpdateModelTexts();
            UpdateMoCaTexts();

            if (Main_Menu.LArm)
                leftHand.isOn = true;
            else if (Main_Menu.RArm)
                rightHand.isOn = true;

            DontDestroyOnLoad(gameObject);
            EnterId.gameObject.SetActive(false);
            DisplayProfilePanel();
        }
    
        private void OnLevelWasLoaded(int level)
        {
            _levelloadedname = Application.loadedLevelName;
            if (_levelloadedname == "City")
            {
                // give some delay for the all the objects to perform start.
                Invoke("SetLevel", 0.5f);
            }
        }
        /// <summary>
        /// this is called after some delay for various reasons
        /// 1.) when City is loaded it searches for it's references(reset function) and this happens in Start also.So,
        /// for the First time if we don't avoid this then search happens twice and that might lead to null reference errors. To avoid this  we are using CanReset which is false by default,
        /// thereby the gamemanager does not run it's reset function and after 0.5 sec we set it back to true . The same goes with DrawObjectiveList.
        /// 2.) All objects have to perform their start and then only we should override their show booleans.
        /// </summary>
        private void SetLevel()
        {
            GameManager.Instance.CanReset = true;
            GameManager.Instance.ShowArrowObject = _showArrow;
            GameManager.Instance.ToggleArrowDisplay();
            GameManager.Instance.IsCalibrationRequired = _calibrationRequired;
            ObjectiveManager.Instance.SetObjectiveLanguage(_english, _portuguese);
            LanguageManager.Instance.English = _english;
            LanguageManager.Instance.Portuguese = _portuguese;
            LanguageManager.Instance.SetLanguage();
            DrawObjectiveList.Instance.CanReset = true;
            DrawObjectiveList.Instance.ShowObjectiveWindow = _showObjectives;

            ObjectiveManager.Instance.GetLevelAndAddObjective(int.Parse("1"));

            DrawObjectiveList.ShowMiniMap = _showMiniMap;
            //PathFindingController.showGreenPath = _showmappath;
            CitySigns.ShowSigns = _showCitySigns;
            GameManager.ShowLabel = _showItemsLabels;
            // destroy this object after it's job is done.
            Destroy(this.gameObject, 0.5f);
        }

        private static void HandleSceneLoad()
        {
            Application.LoadLevel("City");
        }

        public void MainMenuButton(int option)
        {
            if (option == 1)
            {
                mainMenu.SetActive(false);
                _showLoading = true;
                loadingText.text = Language.loading + "...";

                if (_playerId == "")
                    _playerId = "NoName";
                Main_Menu.Uid = _playerId;


                ikLimbLeft.IsEnabled = Main_Menu.LArm;
                ikLimbRight.IsEnabled = Main_Menu.RArm;
                
                if (_calibrationRequired)
                {
                    
                    DoCalibration = true;
                }
                else
                {
                    DoCalibration = false;
                }
                UpdateUserValues();
                if (MoCaProfileToggle.isOn)
                {
                    var profile = new MoCaProfile(_playerId, _moCaVsValue, _moCaNamValue, _moCaAtValue, _moCaLangValue, _moCaAbsValue, _moCaDelRecValue, _moCaOrientValue, _moCaEduValue);
                    if (PerformanceProcessor.CurrentProfileModel.Difficulty < 1)
                        PerformanceProcessor.CurrentProfileModel = profile.CalculateModelFromMoCa();
                }
                else
                {
                    var profile = new Model(_modAttValue, _modMemValue, _modExFunctValue, _modLangValue, _modDiffValue);
                    if (PerformanceProcessor.CurrentProfileModel.Difficulty < 1)
                        PerformanceProcessor.CurrentProfileModel = profile;
                }
                HandleSceneLoad();
                _inputOptions = false;
            }
            if (option == 2)
            {
                mainMenu.SetActive(false);
                secondMenu.SetActive(true);
                options.SetActive(true);
                controls.SetActive(false);
                _inputOptions = true;
            }
            if (option == 3)
            {
                mainMenu.SetActive(false);
                secondMenu.SetActive(true);
                controls.SetActive(true);
                ControlsEN.SetActive(_english);
                ControlsPT.SetActive(_portuguese);
                
                options.SetActive(false);
                _inputOptions = false;
            }
            if (option == 4)
            {
                Application.Quit();
            }
            if (option == 5)
            {
                
                if (options.activeSelf && playerIDinput.text == "")
                    EnterId.gameObject.SetActive(true);
                else
                {
                    UpdateUserValues();
                    EnterId.gameObject.SetActive(false);
                    secondMenu.SetActive(false);
                    mainMenu.SetActive(true);
                    _inputOptions = false;
                }    
            }
        }
        
        private void Update()
        {
            SetLanguageText();

            if (_canGameLoad)
            {
                if (Application.CanStreamedLevelBeLoaded("City"))
                {
                    Application.LoadLevel("City");
                }
            }  
        }

        private void SetLanguageText()
        {
            playText.text = Language.play;
            quitText.text = Language.quit;
            controlsText.text = Language.controls;
            optionsText.text = Language.options;
            Id.text = Language.id;
            EnterId.text = Language.nameWarning;
            Calibration.text = Language.calibration;
            Lang.text = Language.idiom;
            Support.text = Language.support;
            Signs.text = Language.signs;
            Labels.text = Language.labels;
            Minimap.text = Language.minimap;
            Arrow.text = Language.arrow;
            Objective.text = Language.objective;
            ModAttText.text = Language.attention;
            ModMemText.text = Language.memory;
            ModExecText.text = Language.execFunc;
            ModLangText.text = Language.lang;
            ModDiffText.text = Language.difficulty;
            MoCaVSText.text = Language.vSpatial;
            MoCaNamText.text = Language.naming;
            MoCaAtText.text = Language.attention;
            MoCaLangText.text = Language.lang;
            MoCaAbsText.text = Language.abstraction;
            MoCaDelRecText.text = Language.delRecall;
            MoCaOrientText.text = Language.orient;
            MainMenuText.text = Language.main;
            MoCaEduText.text = "> 12 " + Language.edu;
            MoCaBtn.text = Language.moca;
            ModBtn.text = Language.models;

            if (_inputOptions)
            {
                _playerId = playerIDinput.text;
                _english = en.isOn;
                _portuguese = pt.isOn;

                Language.langRT = _portuguese ? "PT" : "EN";

                _calibrationRequired = calib.isOn;
                _showMiniMap = minimap.isOn;
                _showArrow = navigation.isOn;
                _showObjectives = objective.isOn;
                _showCitySigns = citySigns.isOn;
                _showItemsLabels = ItemsLabels.isOn;
                Main_Menu.RArm = rightHand.isOn;
                Main_Menu.LArm = leftHand.isOn;
            }

            if (_showLoading)
            {
                loadingText.text = Language.loading;
            }
        }
        
        public void ButtonName(string button)
        {
            _btnName = button;
        }

        //functions that increases and decreases MoCa values from interface
        public void ChangeMoCa(int value)
        {
            switch (_btnName)
            {
                case "mocaVS":
                    _moCaVsValue += value;
                    if (_moCaVsValue > 5)
                        _moCaVsValue = 5;
                    if (_moCaVsValue < 0)
                        _moCaVsValue = 0;
                    break;
                case "mocaNam":
                    _moCaNamValue += value;
                    if (_moCaNamValue > 3)
                        _moCaNamValue = 3;
                    if (_moCaNamValue < 0)
                        _moCaNamValue = 0;
                    break;
                case "mocaAt":
                    _moCaAtValue += value;
                    if (_moCaAtValue > 6)
                        _moCaAtValue = 6;
                    if (_moCaAtValue < 0)
                        _moCaAtValue = 0;
                    break;
                case "mocaLang":
                    _moCaLangValue += value;
                    if (_moCaLangValue > 3)
                        _moCaLangValue = 3;
                    if (_moCaLangValue < 0)
                        _moCaLangValue = 0;
                    break;
                case "mocaAbs":
                    _moCaAbsValue += value;
                    if (_moCaAbsValue > 2)
                        _moCaAbsValue = 2;
                    if (_moCaAbsValue < 0)
                        _moCaAbsValue = 0;
                    break;
                case "mocaDR":
                    _moCaDelRecValue += value;
                    if (_moCaDelRecValue > 5)
                        _moCaDelRecValue = 5;
                    if (_moCaDelRecValue < 0)
                        _moCaDelRecValue = 0;
                    break;
                case "mocaOrient":
                    _moCaOrientValue += value;
                    if (_moCaOrientValue > 6)
                        _moCaOrientValue = 6;
                    if (_moCaOrientValue < 0)
                        _moCaOrientValue = 0;
                    break;
            }
            UpdateMoCaTexts();
        }

        public void ChangeModel(float value)
        {
            switch (_btnName)
            {
                case "modAtt":
                    _modAttValue += value;
                    if (_modAttValue > 10.0f)
                        _modAttValue = 10;
                    if(_modAttValue < 1)
                        _modAttValue = 1;
                    break;
                case "modMem":
                    _modMemValue += value;
                    if (_modMemValue > 10)
                        _modMemValue = 10;
                    if (_modMemValue < 1)
                        _modMemValue = 1;
                    break;
                case "modExecF":
                    _modExFunctValue += value;
                    if (_modExFunctValue > 10)
                        _modExFunctValue = 10;
                    if (_modExFunctValue < 1)
                        _modExFunctValue = 1;
                    break;
                case "modLang":
                    _modLangValue += value;
                    if (_modLangValue > 10)
                        _modLangValue = 10;
                    if (_modLangValue < 1)
                        _modLangValue = 1;
                    break;
                case "modDiff":
                    _modDiffValue += value;
                    if (_modDiffValue > 10)
                        _modDiffValue = 10;
                    if (_modDiffValue < 1)
                        _modDiffValue = 1;
                    break;
            }
            UpdateModelTexts();
        }

        private void UpdateModelTexts()
        {
            ModAttValueText.text = _modAttValue.ToString(CultureInfo.InvariantCulture);
            ModMemValueText.text = _modMemValue.ToString(CultureInfo.InvariantCulture);
            ModExecValueText.text = _modExFunctValue.ToString(CultureInfo.InvariantCulture);
            ModLangValueText.text = _modLangValue.ToString(CultureInfo.InvariantCulture);
            ModDiffValueText.text = _modDiffValue.ToString(CultureInfo.InvariantCulture);
        }

        private void UpdateMoCaTexts()
        {
            MoCaVSValueText.text = _moCaVsValue.ToString();
            MoCaNamValueText.text = _moCaNamValue.ToString();
            MoCaAtValueText.text = _moCaAtValue.ToString();
            MoCaLangValueText.text = _moCaLangValue.ToString();
            MoCaAbsValueText.text = _moCaAbsValue.ToString();
            MoCaDelRecValueText.text = _moCaDelRecValue.ToString();
            MoCaOrientValueText.text = _moCaOrientValue.ToString();
            var dif = _moCaEduValue + _moCaVsValue + _moCaNamValue + _moCaAtValue + _moCaLangValue + _moCaAbsValue +
                      _moCaDelRecValue + _moCaOrientValue;
            if (dif > 30)
                dif = 30;
            MoCaDifValueText.text = dif.ToString();
        }

        public void UpdateEduMoCaToggle()
        {
            _moCaEduValue = EduMoCaToggle.isOn ? 1 : 0;
            UpdateMoCaTexts();
        }

        public void DisplayProfilePanel()
        {
            MocaProfilePanel.SetActive(MoCaProfileToggle.isOn);
            ModelProfilePanel.SetActive(ModelProfileToggle.isOn);
        }

        public void UpdateUserValues()
        {
            _playerId = playerIDinput.text;
            Main_Menu.Uid = _playerId;
            LoadSaveSettings.LoadSettingsValues();
            if (PerformanceProcessor.CurrentProfileModel.Difficulty > 0)
            {
                ModelProfileToggle.isOn = true;
                MoCaProfileToggle.isOn = false;
                DisplayProfilePanel();
                _modAttValue = PerformanceProcessor.CurrentProfileModel.Attention;
                _modMemValue = PerformanceProcessor.CurrentProfileModel.Memory;
                _modExFunctValue = PerformanceProcessor.CurrentProfileModel.ExFunctions;
                _modLangValue = PerformanceProcessor.CurrentProfileModel.Language;
                _modDiffValue = PerformanceProcessor.CurrentProfileModel.Difficulty;
                UpdateModelTexts();
            }

            if (MoCaProfileToggle.isOn)
            {
                ModelProfile.UpdateMoCAProfile(_playerId, _moCaVsValue, _moCaNamValue, _moCaAtValue, _moCaLangValue, _moCaAbsValue, _moCaDelRecValue, _moCaOrientValue, _moCaEduValue);
            }
            else
            {
                ModelProfile.UpdateModelProfile(_playerId, _modAttValue, _modMemValue, _modExFunctValue, _modLangValue, _modDiffValue);
            }
        }
    }  
}
