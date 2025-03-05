using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Assets.scripts.GUI;
using Assets.scripts.Manager;
using Assets.scripts.RehaTask.RehaTaskGUI;
using Assets.scripts.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.scripts.UDP
{
    public class UdpReceive : MonoBehaviour
    {
        private Thread _receiveThread;// receiving Thread
        private UdpClient _client;// udpclient object

        private GameManager _gm;

        private readonly List<string> _datatypelst = new List<string>();
        private readonly List<string> _devicelst = new List<string>();
        private readonly List<string> _jointslst = new List<string>();
        private readonly List<string> _transformTypelst = new List<string>();
        private readonly List<string> _emulst = new List<string>();

        private string _datatype, _device, _joint, _transformationtype;
        private float _udpx, _udpy, _udpz, _udpw;

        private int _port; // define > Init
        private string _portField = "1202";
        public static bool UdpConnected;

        private string _selection = "n/a";
        private bool _tracking;

        public static float Xmax, Xmin, Ymax, Ymin, Zmax, Zmin;

        private static Vector3 _udppos;
        private Quaternion _udprot;

        public static GameObject Target;//IK Target

        public static List<float> Xvalues = new List<float>();
        public static List<float> Yvalues = new List<float>();
        private readonly List<float> _zvalues = new List<float>();
        private float _scalex, _scaley, _scalez;
        public static bool calibrate;
        public static bool CalibrateU = false;
        public static bool CalibrateD = false;
        public static bool CalibrateR = false;
        public static bool CalibrateL = false;

        //filter parameters
        private static float _alpha = 0.90f;
        private readonly float[] _nInput = new float[2];//xy in/out
        private readonly float[] _nOutput = new float[2];//xy in/out

        public static Vector3 FilteredPosition;
        public static Vector3 NewPosition;
        private float _delta;
        private bool _deltaon;

        private bool _yon = true;
        private bool _zon;

        private bool _firstInit = true;

        public int calibrationTime = 10;//calibration time in sec.

        public float refreshTimer = 5;//refresh list time

        //Target borders for scaling
        private const float Rangemaxx = 1f;
        private const float Rangemaxy = 1f;
        private const float Rangeminx = -1;
        private const float Rangeminy = -1;

        private bool _eyetrack;

        public static bool Calib = true;

        //UI elements
        private GameObject _optionsPanel, _networkPanel, _filteringPanel, _devicesPanel, _calibPanel, _devicesScroll;
        //private Text _devicePrefab;
        private Text /*_portLabel, */_ipLabel, _ipAdress, _maxLabel, _minLabel, _scaleLabel, _filtered, _calibrating, _alphaLabel, _calibrationLabel;
        private InputField _portInputField;
        private Toggle _startStop, _yToggle, _zToggle, _deltaToggle;
        private Button _copy, _close, _networkBtn, _filteringBtn, _devicesBtn;
        //private Button _mainBtn;
        private Slider _alphaSlider;

        public static bool First = true;

        //private GameObject[] _devices;

        public GameObject calibPrefab;
        private bool _firstEnterCalib = true;
        public Color TextColor, SelectedColor;

        public GameObject TrackingPanel;

        private void Awake()
        {
            Target = GameObject.FindGameObjectWithTag("hand");
        }

        private void Start()
        {
            _gm = GameManager.Instance;
            MainGUI.Picture = false;
            if (calibPrefab.activeSelf)
                SetUiObjects();

            if (_gm.IsCalibrationRequired)
            {
                Init();
                
            }
        }

        private void Update()
        {
            if (_gm.IsCalibrationRequired)
            {
                Target = GameObject.FindGameObjectWithTag("hand");
                _calibrationLabel.text = Language.calibration;
                
                UiTransitions();
                
                CalibUi();//sets all UI elements
                
                //update available data list
                refreshTimer -= Time.deltaTime;
                if (refreshTimer <= 0 && !calibrate)
                {
                    _emulst.Clear();
                    refreshTimer = 5;
                }
                /*
                if (Input.GetKeyDown(KeyCode.L))
                { ClearLists(); }
                */
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    CalibrationGUI.CalibProcess = true;
                    calibrate = true;
                    _eyetrack = false;
                    if(Tracking.PerformTracking)
                        TrackingPanel.SetActive(true);
                }

                if (Input.GetKeyDown(KeyCode.KeypadEnter) && !calibrate && !Tracking.IsTracking)
                {
                    CalibrationGUI.SkipCalib = false;
                    ClearLists();
                    CalibrationGUI.CalibProcess = true;
                    calibrate = true;
                    _eyetrack = false;
                }
                //apply Time.deltaTime to the lowpass filter
                if (_deltaToggle.isOn)
                    _delta = Time.deltaTime;
                else
                    _delta = 1.0f;

                //use y or z coordinates for the y axis
                if (_yon)
                    _zon = false;
                else
                {
                    _yon = false;
                    _zon = true;
                }

                //calibration process is enabled from CalibrationGUI and each min/max value is Calibrated individually
                if (_tracking)
                {
                    if (calibrate)
                    { //XYZ MIN/MAX
                        if (CalibrateL)
                        {
                            if (_udppos.x != 0)
                            {
                                Xvalues.Add(-_udppos.x);
                                Xvalues.Sort();
                                Xmin = Xvalues[Xvalues.Count / 2];
                            }
                        }

                        if (CalibrateR)
                        {
                            if (_udppos.x != 0)
                            {
                                Xvalues.Add(-_udppos.x);
                                Xvalues.Sort();
                                Xmax = Xvalues[Xvalues.Count / 2];
                            }
                        }

                        if (CalibrateU)
                        {
                            if (_udppos.y != 0)
                            {
                                Yvalues.Add(_udppos.y);
                                Yvalues.Sort();
                                Ymax = Yvalues[Yvalues.Count / 2];
                            }
                        }

                        if (CalibrateD)
                        {
                            if (_udppos.y != 0)
                            {
                                Yvalues.Add(_udppos.y);
                                Yvalues.Sort();
                                Ymin = Yvalues[Yvalues.Count / 2];
                            }
                        }

                        if (_udppos.z != 0)
                        {
                            _zvalues.Add(-_udppos.z);
                            _zvalues.Sort();
                            Zmin = _zvalues[0];
                            Zmax = _zvalues[_zvalues.Count - 1];
                        }
                    }//end calibrate

                    //scale raw data between -1 to 1
                    _scalex = ((-_udppos.x - Xmin) / (Xmax - Xmin)) * (Rangemaxx - (Rangeminx)) + (Rangeminx);
                    _scaley = ((_udppos.y - Ymin) / (Ymax - Ymin)) * (Rangemaxy - (Rangeminy)) + (Rangeminy);
                    _scalez = ((-_udppos.z - Zmin) / (Zmax - Zmin)) * (Rangemaxy - (Rangeminy)) + (Rangeminy);

                    if (float.IsInfinity(_scalex) || float.IsNaN(_scalex))
                        _scalex = Target.transform.localPosition.x;

                    if (float.IsInfinity(_scaley) || float.IsNaN(_scaley))
                        _scaley = Target.transform.localPosition.y;

                    if (float.IsInfinity(_scalez) || float.IsNaN(_scalez))
                    {
                        if(Target!=null)
                            _scalez = Target.transform.localPosition.z;
                    }
                    
                    //put last X,y coordinates to an array in order to feed the _tracking to the low pass filter
                    _nInput[0] = _scalex;

                    //option for y or z axis to act like y.
                    if (_yon)
                    { _nInput[1] = _scaley; }//y
                    else if(_zon)
                    { _nInput[1] = _scalez; }//z

                    _nOutput[0] = lowPass(_nInput, _nOutput)[0];
                    _nOutput[1] = lowPass(_nInput, _nOutput)[1];

                    //lowPass(_nInput, _nOutput); //[0]for X, [1]for y
                    FilteredPosition = new Vector3(lowPass(_nInput, _nOutput)[0], lowPass(_nInput, _nOutput)[1], lowPass(_nInput, _nOutput)[1]);
                    if (Target != null)
                    {
                        NewPosition = new Vector3(FilteredPosition.x, Target.transform.localPosition.y, FilteredPosition.y);
                        if (Application.loadedLevel == 2/* && !CP_Controller.RenderHand*/)
                            NewPosition = new Vector3(FilteredPosition.x, Target.transform.localPosition.y, FilteredPosition.y);

                        else if (Application.loadedLevelName == "Home" || Application.loadedLevelName == "FashionStore" || Application.loadedLevelName == "Park")
                            NewPosition = new Vector3(FilteredPosition.x * 1.5f, Target.transform.localPosition.y,
                                FilteredPosition.y *0.94f - 2.025f);
                        else
                        {
                            NewPosition = new Vector3(FilteredPosition.x*80f, FilteredPosition.y*45f + 25f,
                                Target.transform.localPosition.z);
                        }
                       
                        //Debug.Log(NewPosition);
                        Target.transform.position = NewPosition;
                    }
                }

                if (_eyetrack)
                {
                    Target.transform.position = new Vector3(-_udppos.x, _udppos.y, Target.transform.position.z);
                }
            }
        }

        //low pass filter X,y
        float[] lowPass(float[] input, float[] output)
        {
            for (var i = 0; i < input.Length; i++)
            {
                if (output == null || output.Length == 0 || float.IsNaN(output[i]) || float.IsInfinity(output[i]) || output[i] == float.NaN)
                    output[i] = input[i];
                else
                    output[i] = output[i] + _alpha * (input[i] - output[i]) * _delta;
            }

            return output;
        }
    


        public void Init()
        {
            if (_firstInit)
            {
                // Local endpoint define (where messages are received).
                // Create a new thread to receive incoming messages.     
                _receiveThread = new Thread(new ThreadStart(ReceiveData));
                _receiveThread.IsBackground = true;
                _receiveThread.Start();
                _firstInit = false;
            }
        }

        // receive thread 
        public void ReceiveData()
        {
            _port = int.Parse(_portField);
            _client = new UdpClient(_port);
        
            while (UdpConnected)
            {
                try
                {
                    // receive Bytes from 127.0.0.1
                    var IP = new IPEndPoint(IPAddress.Loopback, 0);

                    var udpdata = _client.Receive(ref IP);

                    //  UTF8 encoding in the text format.
                    var data = Encoding.UTF8.GetString(udpdata);

                    //PROTOCOL
                    if (data != string.Empty)
                        TranslateData(data);
                }//try

                catch (Exception err)
                {
                    print(err.ToString());
                }
            }//while true		
        }//ReceiveData

        private void TranslateData(string n_data)
        {
            //	[$]<data  Type> , [$$]<_device> , [$$$]<_joint> , <transformation> , <param_1> , <param_2> , .... , <param_N>
            // Decompose incoming data based on the protocol rules
            string[] separators = { "[$]", "[$$]", "[$$$]", ",", ";", " " };

            var words = n_data.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            _datatype = words[0];
            _device = words[1];
            _joint = words[2];
            _transformationtype = words[3];

            var emustring = _device + "_" + _joint;

            //populate lists categorizing the segmented data
            if (!_emulst.Contains(emustring) && emustring != String.Empty)
                _emulst.Add(emustring);//emulation

            if (!_datatypelst.Contains(_datatype) && _datatype != String.Empty)
                _datatypelst.Add(_datatype);//add to _datatype list

            if (!_devicelst.Contains(_device) && _device != String.Empty)
                _devicelst.Add(_device);//add to _device list

            if (!_jointslst.Contains(_joint) && _joint != String.Empty)
                _jointslst.Add(_joint);//add to _joint list	

            if (!_transformTypelst.Contains(_transformationtype) && _transformationtype != String.Empty)
                _transformTypelst.Add(_transformationtype);//add to transformaton Type list

            //apply incoming transformations
            if (_selection == emustring)
            {
                if (_transformationtype == "position")
                    _udppos = new Vector3(float.Parse(words[4]), float.Parse(words[5]), 0.0f);
            }
        }//end of TranslateData()

        private void ClearLists()
        {
            _datatypelst.Clear();
            _devicelst.Clear();
            _jointslst.Clear();
            _transformTypelst.Clear();
            _emulst.Clear();
            Xvalues.Clear();
            Yvalues.Clear();
            _zvalues.Clear();
            Xmax = 0; Xmin = 0;
            Ymax = 0; Ymin = 0;
            Zmax = 0; Zmin = 0;
            _scalex = 0;
            _scaley = 0;
            _scalez = 0;
        }

        private IEnumerator Calibrate()
        {
            yield return new WaitForSeconds(calibrationTime);
            calibrate = false;//stop calibration
            Time.timeScale = 0.0f;
        }

#if UNITY_STANDALONE
        private static string LocalIpAddress()
        {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                return null;

            var localIp = "";
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in host.AddressList.Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork))
            {
                localIp = ip.ToString();
            }

            return localIp;
        }
#endif

        private void OnDisable()
        {
            //PerformanceProcessor.FirstTimeProcessing = false;
            //LoadSaveSettings.SaveSettingsInfo();
            if (UdpConnected)
                _receiveThread.Abort();
        }

        private void OnApplicationQuit()
        {
            //PerformanceProcessor.FirstTimeProcessing = false;
            //LoadSaveSettings.SaveSettingsInfo();
            if (UdpConnected)
                _receiveThread.Abort();
        }

   
        private void SetUiObjects()
        {
            _optionsPanel = GameObject.Find("OptionsPanel");
            _networkPanel = GameObject.Find("NetworkPanel");
            _filteringPanel = GameObject.Find("FilteringPanel");
            _devicesPanel = GameObject.Find("DevicesPanel");
            _calibPanel = GameObject.Find("CalibrationUI");
            _devicesScroll = GameObject.Find("DevicesScrollContent");

            //_portLabel = GameObject.Find("PortLabel").GetComponent<Text>();
            _ipLabel = GameObject.Find("IPLabel").GetComponent<Text>();
            _ipAdress = GameObject.Find("IPAdress").GetComponent<Text>();
            _maxLabel = GameObject.Find("MaxLabel").GetComponent<Text>();
            _minLabel = GameObject.Find("MinLabel").GetComponent<Text>();
            _scaleLabel = GameObject.Find("ScaleLabel").GetComponent<Text>();
            _filtered = GameObject.Find("FilteredLabel").GetComponent<Text>();
            _calibrating = GameObject.Find("Calibrating").GetComponent<Text>();
            _alphaLabel = GameObject.Find("AlphaLabel").GetComponent<Text>();
            _calibrationLabel = GameObject.Find("CalibrationTitle").GetComponent<Text>();

            _portInputField = GameObject.Find("PortInputField").GetComponent<InputField>();
            _portInputField.text = _portField.ToString();

            _alphaSlider = GameObject.Find("AlphaSlider").GetComponent<Slider>();

            _startStop = GameObject.Find("StartStopToggle").GetComponent<Toggle>();
            _yToggle = GameObject.Find("YToggle").GetComponent<Toggle>();
            _zToggle = GameObject.Find("ZToggle").GetComponent<Toggle>();
            _deltaToggle = GameObject.Find("DeltaToggle").GetComponent<Toggle>();

            _copy = GameObject.Find("CopyButton").GetComponent<Button>();
            //_close = GameObject.Find("CloseButton").GetComponent<Button>();
            _networkBtn = GameObject.Find("NetworkButton").GetComponent<Button>();
            _filteringBtn = GameObject.Find("FilteringButton").GetComponent<Button>();
            _devicesBtn = GameObject.Find("DevicesButton").GetComponent<Button>();

            _calibrating.gameObject.SetActive(false);
            _optionsPanel.SetActive(false);

            _deltaToggle.isOn = _deltaon;

            _alphaSlider.value = _alpha;
        }

        private void UiTransitions()
        {
            //_close.onClick.AddListener(() => { SetUiPanel(0); });
            _networkBtn.onClick.AddListener(() => { SetUiPanel(1); });
            _filteringBtn.onClick.AddListener(() => { SetUiPanel(2); });
            _devicesBtn.onClick.AddListener(() => { SetUiPanel(3); });  
        }

        private void SetUiPanel(int panel)
        {
            if(panel != 4)
            {
                _optionsPanel.SetActive(true);
                _networkPanel.SetActive(false);
                _filteringPanel.SetActive(false);
                _devicesPanel.SetActive(false);

                if (panel == 0)
                    _optionsPanel.SetActive(false);
                if (panel == 1)
                    _networkPanel.SetActive(true);
                if (panel == 2)
                    _filteringPanel.SetActive(true);
                if (panel == 3)
                    _devicesPanel.SetActive(true);
            }
        }

        private void CalibUi()
        {
            
            if (Calib)
            {
                _calibPanel.SetActive(true);
                _networkBtn.gameObject.GetComponentInChildren<Text>().text = Language.network;
                _filteringBtn.gameObject.GetComponentInChildren<Text>().text = Language.filtering;
                _devicesBtn.gameObject.GetComponentInChildren<Text>().text = Language.devices;

                if (_firstEnterCalib)
                {
                    SetUiPanel(1);
                    _firstEnterCalib = false;
                }
            }
            
            else
                _calibPanel.SetActive(false);
        
            if (calibrate && _tracking)
            {
                _calibrating.gameObject.SetActive(true);
                _calibrating.text = Language.cal;
            }

            else if (!calibrate && _tracking && !CalibrationGUI.SkipCalib)
                _calibrating.text = " ";

            //filtering window
            if (_filteringPanel.activeInHierarchy)
            {
                _networkBtn.gameObject.GetComponentInChildren<Text>().color = TextColor;
                _networkBtn.gameObject.GetComponentInChildren<Image>().color = Color.white;

                _filteringBtn.gameObject.GetComponentInChildren<Text>().color = Color.green;
                _filteringBtn.gameObject.GetComponentInChildren<Image>().color = Color.green;

                _devicesBtn.gameObject.GetComponentInChildren<Text>().color = TextColor;
                _devicesBtn.gameObject.GetComponentInChildren<Image>().color = Color.white;

                _maxLabel.text = "max: " + Xmax.ToString("0.00") + "," + Ymax.ToString("0.00") + "," + Zmax.ToString("0.00");
                _minLabel.text = "min: " + Xmin.ToString("0.00") + "," + Ymin.ToString("0.00") + "," + Zmin.ToString("0.00");
                _scaleLabel.text = Language.scale + _scalex.ToString("0.00") + "," + _scaley.ToString("0.00") + "," + _scalez.ToString("0.00");
                _filtered.text = Language.filtered + FilteredPosition.x.ToString("0.00") + "," + FilteredPosition.y.ToString("0.00");

                _alpha = _alphaSlider.value;
                _alphaLabel.text = _alpha.ToString("0.00");

                _deltaon = _deltaToggle.isOn;

                _yon = _yToggle.isOn;
                _zon = _zToggle.isOn;
            }

            //network window
            if (_networkPanel.activeInHierarchy)
            {
                _networkBtn.gameObject.GetComponentInChildren<Text>().color = Color.green;
                _networkBtn.gameObject.GetComponentInChildren<Image>().color = Color.green;

                _filteringBtn.gameObject.GetComponentInChildren<Text>().color = TextColor;
                _devicesBtn.gameObject.GetComponentInChildren<Text>().color = TextColor;

                _filteringBtn.gameObject.GetComponentInChildren<Image>().color = Color.white;
                _devicesBtn.gameObject.GetComponentInChildren<Image>().color = Color.white;

                _portField = _portInputField.text;
                _copy.gameObject.GetComponentInChildren<Text>().text = Language.copy;
                //_close.gameObject.GetComponentInChildren<Text>().text = Language.close;

                var btnLabel = _startStop.GetComponentInChildren<Text>();
                
                if (_startStop.isOn)
                {
                    _port = int.Parse(_portField);
                    _firstInit = true;
                    Init();
                    UdpConnected = true;
                    btnLabel.text = Language.stop;
                    btnLabel.color = Color.green;
                }
                else
                {
                    UdpConnected = false;
                    _receiveThread.Abort();
                    _client.Close();
                    ClearLists();
                    _tracking = false;
                    calibrate = false;
                    btnLabel.text = Language.start;
                    btnLabel.color = TextColor;
                    First = true;
                }

                _ipAdress.text = LocalIpAddress();
                _ipLabel.text = Language.local;

                //_copy address to clipboard
                _copy.onClick.AddListener(() => { ClipboardHelper.clipBoard = LocalIpAddress(); });
            }
        
            //_devices window
            if(_devicesPanel.activeInHierarchy)
            {
                _networkBtn.gameObject.GetComponentInChildren<Text>().color = TextColor;
                _networkBtn.gameObject.GetComponentInChildren<Image>().color = Color.white;

                _filteringBtn.gameObject.GetComponentInChildren<Text>().color = TextColor;
                _filteringBtn.gameObject.GetComponentInChildren<Image>().color = Color.white;

                _devicesBtn.gameObject.GetComponentInChildren<Text>().color = Color.green;
                _devicesBtn.gameObject.GetComponentInChildren<Image>().color = Color.green;

                if (_emulst.Count != 0 && First)
                {
                    for (int i=0; i< _emulst.Count; i++)
                    {
                        var newDevice = Instantiate(Resources.Load("Device")) as GameObject;
                        if (newDevice != null)
                        {
                            newDevice.name = _emulst[i];
                            newDevice.transform.SetParent(_devicesScroll.transform, false);

                            var btn = newDevice.GetComponent<Button>();
                            var btnText = btn.GetComponentInChildren<Text>();
                            btnText.text = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(_emulst[i].ToUpper());
                            var temp = i;
                            btn.onClick.AddListener(() => { ChoseDevice(_emulst[temp]); });
                        }
                    }
                    First = false; 
                }
            }
        }

        private void ChoseDevice(string selected)
        {
            _optionsPanel.SetActive(false);
            _selection = selected;//fix to point into a _joint

            _tracking = true;

            if (Xmin != 0f && Xmax != 0f && Ymin != 0f && Ymax != 0f && !CalibrationGUI.CalibDone)
            {
                _calibrating.gameObject.SetActive(true);
                CalibrationGUI.SkipCalib = true;
                _calibrating.text = Language.calibDone;
            }

            else if (Xmin == 0f && Xmax == 0f && Ymin == 0f && Ymax == 0f && !CalibrationGUI.CalibDone)
            {
                ClearLists();
                CalibrationGUI.CalibProcess = true;
                calibrate = true;
                _eyetrack = false;
            }
        }

        public static float[] Rawposition()
        {
            var rawPos = new float[3];

            rawPos[0] = _udppos.x;
            rawPos[1] = _udppos.y;
            rawPos[2] = _udppos.z;

            return rawPos;
        }
    }
}