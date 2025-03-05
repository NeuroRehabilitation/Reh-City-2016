using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIClass : MonoBehaviour {

    [HideInInspector]
    public bool ZerotoOne = false;
    [HideInInspector]
    public bool MinusOnetoOne = true;
    [HideInInspector]
    public bool Analog = true;
    [HideInInspector]
    public bool Digital = false;
    [HideInInspector]
    public bool xAxis = true;
    [HideInInspector]
    public bool yAxis = true;
    [HideInInspector]
    public bool zAxis = false;
 
    public string nofActions = "2";
    [HideInInspector]
    public  int NumberofActions;

    public delegate void CalibrationStart();
    public static event CalibrationStart OnCalibrationStart;

    public string portnumber = "1203";

    public delegate void CalibrationStop();
    public static event CalibrationStop OnCalibrationStop;
   
    private static GUIClass _instance;
    public static GUIClass Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(GUIClass)) as GUIClass;
            }
            if (_instance == null) Debug.Log("Could not find GUIClass");
            return _instance;
        }
    }

    public Toggle oneToZeroToggle, oneToMinusOneToggle, analogToggle, digitalToggle;
    public InputField portInput;
    public Text actionsNumber;
    
    void OnGUI()
    {/*
        GUI.BeginGroup(new Rect(10, 10, 700, 700));
        GUI.Box(new Rect(0, 0, 300, 350), "Options");

        GUI.Label(new Rect(10, 20, 80, 20), "Data format");
        ZerotoOne = GUI.Toggle(new Rect(100, 20, 50, 20), ZerotoOne, "1 to 0");
        
        MinusOnetoOne = GUI.Toggle(new Rect(200, 20, 50, 20), MinusOnetoOne, "1 to -1");
        

        GUI.Label(new Rect(10, 60, 80, 20), "Data Type");
        Analog = GUI.Toggle(new Rect(100, 60, 70, 20), Analog, "Analog");
        Digital = GUI.Toggle(new Rect(200, 60, 70, 20), Digital, "Digital");

        

        GUI.Label(new Rect(10, 110, 80, 20), "Port");
        portnumber = GUI.TextField(new Rect(140, 110, 60, 20), portnumber, 4);
        

        GUI.Label(new Rect(10, 160, 120, 20), "Actions ");
        nofActions = GUI.TextField(new Rect(140, 160, 60, 20), nofActions, 2);

        if (GUI.Button(new Rect(30, 200, 110, 50), "Start Calibration"))
        {
            if (OnCalibrationStart != null)
            {
                OnCalibrationStart();
            }
        }
        if (GUI.Button(new Rect(160, 200, 100, 50), "Stop"))
        {
            if (OnCalibrationStop != null)
            {
                OnCalibrationStop();
            }
        }
        GUI.EndGroup();*/
    }
    
    void Start()
    {
        actionsNumber.text = nofActions;
    }

    void Update()
    {
        ZerotoOne = oneToZeroToggle.isOn;
        MinusOnetoOne = oneToMinusOneToggle.isOn;

        Analog = analogToggle.isOn;
        Digital = digitalToggle.isOn;

        portnumber = portInput.text;
    }

    public void IncreaseActions(int plus)
    {
        int actNumber = int.Parse(nofActions);
        actNumber = actNumber + plus;
        nofActions = actNumber.ToString();
        actionsNumber.text = nofActions.ToString();
        
    }

    public void DecreaseActions(int minus)
    {
        int actNumber = int.Parse(nofActions);
        if (actNumber > 1)
        {
            actNumber = actNumber - minus;
            nofActions = actNumber.ToString();
            actionsNumber.text = nofActions.ToString();
        }
    }

    public void StartCalib(bool bCalib)
    {
        if (bCalib)
        {
            if (OnCalibrationStart != null)
            {
                OnCalibrationStart();
            }
        }
    }

    public void StopCalib(bool sCalib)
    {
        if (sCalib)
        {
            if (OnCalibrationStop != null)
            {
                OnCalibrationStop();
            }
        }
    }
}
