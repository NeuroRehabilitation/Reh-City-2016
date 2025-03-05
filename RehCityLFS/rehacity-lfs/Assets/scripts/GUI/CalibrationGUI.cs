using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Xml;
using Assets.scripts.RehaTask.RehaTaskGUI;
using Assets.scripts.Settings;
using Assets.scripts.UDP;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.scripts.GUI
{
    public class CalibrationGUI : MonoBehaviour {

        public GameObject calibPrefab, calibText;
        private GameObject _stepsInterface;

        private GameObject[] _steps = new GameObject[5];
        private Text _instruction;

        public static bool CalibProcess;//if true, calibration is in process
        public static bool CalibSteps;//boolean that helps to indicate when to display _steps
        public static bool CalibDone;//boolean that indicates if calibration has been already done

        private bool _centralCalib = true; 

        private float _myTimer = 54.0f/*, delayTimer = 1.0f*/;
        private float _time;

        private int _step;
        private bool _active = true;

        public string[] instructionsEN = new String[5];//english instructions
        public string[] instructionsPT = new String[5];//portuguese instructions

        public static bool Calibrated = false, SkipCalib = false;

        public GameObject TrackingPanel;

        private void Start () 
        {
            if (calibPrefab.activeSelf)
            {
                instructionsEN = new[] { "Place your hand on the center", "Move your hand to the right", "Move your hand to the left", "Move your hand forward", "Move your hand backward" };
                instructionsPT = new[] { "Coloque a sua mão no centro", "Mova a sua mão para a direita", "Mova a sua mão para a esquerda", "Mova a sua mão para a frente", "Mova a sua mão para trás" };
                _time = _myTimer - 6.0f;

                _instruction = GameObject.Find("Instruction").GetComponent<Text>();
                _stepsInterface = GameObject.FindGameObjectWithTag("Steps");

                IComparer myComparer = new MyStepsSorter();//sorts _steps by order
                _steps = GameObject.FindGameObjectsWithTag("Step");
                Array.Sort(_steps, myComparer);

                _stepsInterface.SetActive(false);
                DisableSteps();
            }
        }
	
        private void Update () 
        {
            if (CalibProcess)
            {
                if (SkipCalib)
                    _myTimer = 0;
                _stepsInterface.SetActive(true);
                CalibSteps = true;
                StartTimer();
            }   
        }

        //enabling and disabling sequencial _steps and consequently enabling individual calibration on UDPreceive script
        private void StartTimer()
        {
            _myTimer -= Time.deltaTime;
            if (_myTimer > _time)
            {
                if (_active)
                {
                    if (_step <= 4 && !_centralCalib)
                    {
                        _steps[_step].SetActive(true);

                        _instruction.text = Language.langRT == "EN" ? instructionsEN[_step] : instructionsPT[_step];

                        if (_step == 1)
                            UdpReceive.CalibrateR = true;

                        if (_step == 2)
                            UdpReceive.CalibrateL = true;

                        if (_step == 3)
                            UdpReceive.CalibrateU = true;

                        if (_step == 4)
                            UdpReceive.CalibrateD = true;
                    }

                    //when returning to central circle disables calibration
                    if(_centralCalib)
                    {
                        _steps[0].SetActive(true);
                    
                        _instruction.text = Language.langRT == "EN" ? instructionsEN[0] : instructionsPT[0];
                    
                        UdpReceive.CalibrateU = false;
                        UdpReceive.CalibrateD = false;
                        UdpReceive.CalibrateL = false;
                        UdpReceive.CalibrateR = false;
                        UdpReceive.Xvalues.Clear();
                        UdpReceive.Yvalues.Clear();
                        _step++;
                    }
                }
                _active = false;   
            }

            else if(!SkipCalib)
            {
                if (_centralCalib)
                    _centralCalib = false;
                else if(_myTimer > 0)
                    _centralCalib = true;

                _time -= 6.0f;//decreasing _time for next circle
                DisableSteps();
                _active = true;
            }

            //in the end of the complete process, calibration is complete and main menu is loaded
            if (_myTimer <= 0)
            {
                calibText.SetActive(false);
                if (!CalibDone)
                {
                    _steps[0].SetActive(false);

                    CalibProcess = false;
                    CalibSteps = false;
                    UdpReceive.Calib = false;
                    calibPrefab.SetActive(false);

                    if(Tracking.PerformTracking)
                        TrackingPanel.SetActive(true);
                    else
                        CalibDone = true;

                    UdpReceive.Calib = false;
                    UdpReceive.calibrate = false;
                    _instruction.text = " ";
                        
                    
                }
            }
        }

        private void DisableSteps()
        {
            foreach (var t in _steps)
            {
                t.SetActive(false);
            }
        }

        public class MyStepsSorter : IComparer
        {
            int IComparer.Compare(object x, object y)
            {
                return ((new CaseInsensitiveComparer()).Compare(((GameObject)x).name, ((GameObject)y).name));
            }
        }    
    }
}
  

