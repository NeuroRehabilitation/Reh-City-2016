using UnityEngine;
using System.Collections;
using Assets.scripts.Controller;
using Assets.scripts.GUI;
using Assets.scripts.Manager;
using Assets.scripts.Settings;
using Assets.scripts.UDP;
using UnityEngine.UI;

public class NavigationSettings : MonoBehaviour {

    public GameObject ThresholdRight, ThresholdLeft, ThresholdTop, ThresholdBottom, ThresholdsParent, MovingCursor, NavSettPanel;
    private GameObject _controller;
    public Slider TopThresholdSlider, BottomThresholdSlider, SidesThresholdSlider, TimePerNodeSlider;
    public Text TopThresholdText, BottomThresholdText, SidesThresholdText, TimePerNodeValueText;
    public Toggle RotateAndMoveToggle, KeepCursorToggle;
    public static float TopThreshold;
    public static float BottomThreshold = -0.5f;
    public static float SidesThreshold = 0.20f;
    public static bool FullFreeze, KeepCursor;
    public static bool RotateAndMove = true;
    private GameManager _gm;
    private GameObject _mger;

    public Text SpeedTxt,
        ExpTxt,
        LogTxt,
        PropTxt,
        MoveAndRotTxt,
        SensitivityTxt,
        TopThreshTxt,
        BottomThreshTxt,
        SidesThreshTxt,
        CursorTxt,
        TimePerNodeText;

    private void Start ()
    {
        TimePerNodeSlider.value = NavigationTimer.NavigationTime;
        TopThresholdSlider.value = TopThreshold;
        BottomThresholdSlider.value = BottomThreshold;
        SidesThresholdSlider.value = SidesThreshold;
        _controller = GameObject.FindGameObjectWithTag("hand");
        RotateAndMoveToggle.isOn = RotateAndMove;
        KeepCursorToggle.isOn = KeepCursor;
        _mger = GameObject.FindGameObjectWithTag("Manager");
        _gm = _mger.GetComponent<GameManager>();
        SetLanguage();
    }

	private void Update ()
	{
        if (!KeepCursorToggle.isOn)
	    {
	        MovingCursor.GetComponent<Image>().enabled = NavSettPanel.activeSelf;
	        KeepCursor = false;
	    }
        else if(KeepCursor && !Tracking.IsTracking && _gm.IsCalibrationRequired && CalibrationGUI.CalibDone)
            MovingCursor.GetComponent<Image>().enabled = true;
        else
            MovingCursor.GetComponent<Image>().enabled = false;

        if (MovingCursor.GetComponent<RectTransform>().position.y <
	        ThresholdBottom.GetComponent<RectTransform>().position.y)
	        FullFreeze = true;
	    else
	    {
            FullFreeze = false;
        }
        
        FPSInputController.threshold = TopThreshold;
	    FPSInputController.canRotateAndMove = RotateAndMove;
        
        TopThresholdText.text = TopThreshold.ToString("0.00");
	    BottomThresholdText.text = BottomThreshold.ToString("0.00");
        SidesThresholdText.text = SidesThreshold.ToString("0.00");
        var width = ThresholdsParent.GetComponent<RectTransform>().rect.width;
        var height = ThresholdsParent.GetComponent<RectTransform>().rect.height;
        ThresholdRight.GetComponent<RectTransform>().position = new Vector3(width*0.5f + width * SidesThreshold * 0.5f, ThresholdRight.GetComponent<RectTransform>().position.y, 0);
        ThresholdLeft.GetComponent<RectTransform>().position = new Vector3(width * 0.5f - width * SidesThreshold * 0.5f, ThresholdLeft.GetComponent<RectTransform>().position.y, 0);
        ThresholdTop.GetComponent<RectTransform>().position = new Vector3(ThresholdTop.GetComponent<RectTransform>().position.x, height*0.5f + height * TopThreshold * 0.5f, 0);
        ThresholdBottom.GetComponent<RectTransform>().position = new Vector3(ThresholdBottom.GetComponent<RectTransform>().position.x, height * 0.5f + height * BottomThreshold * 0.5f, 0);
	    TimePerNodeValueText.text = NavigationTimer.NavigationTime.ToString("0");

        MovingCursor.GetComponent<RectTransform>().position = new Vector3((width * 0.5f) + width * 0.5f *_controller.transform.position.x, (height * 0.5f) + height * 0.5f *_controller.transform.position.z, 0);
	}

    public void SetNavTimer(float value)
    {
        NavigationTimer.NavigationTime = value;
    }

    public void SetSidesThreshold(float value)
    {
        SidesThreshold = value;
    }

    public void SetTopThreshold(float value)
    {
        TopThreshold = value;
    }

    public void SetBottomThreshold(float value)
    {
        BottomThreshold = value;
    }

    public void UpdateCursorVisibility()
    {
        KeepCursor = KeepCursorToggle.isOn;
        MovingCursor.GetComponent<Image>().enabled = KeepCursor;
    }

    public void UpdateRotateAndMove()
    {
        RotateAndMove = RotateAndMoveToggle.isOn;
    }

    private void SetLanguage()
    {
        SpeedTxt.text = Language.Speed;
        ExpTxt.text = Language.Exp;
        LogTxt.text = Language.Log;
        PropTxt.text = Language.Prop;
        MoveAndRotTxt.text = Language.MoveAndRot;
        SensitivityTxt.text = Language.Sensitivity;
        TopThreshTxt.text = Language.TopThresh;
        BottomThreshTxt.text = Language.BottomThresh;
        SidesThreshTxt.text = Language.SidesThresh;
        CursorTxt.text = Language.Cursor;
        TimePerNodeText.text = Language.NodeTime;
    }
}
