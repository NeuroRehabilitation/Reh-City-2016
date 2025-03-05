using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CalibrationManager : MonoBehaviour {

    private Vector3 FilteredPosition;
    public bool Calibrating = false;
    public bool CalibarionDone = false;

    public Transform target, leftArrow, rightArrow, upArrow, downArrow;
    public Text instructiontext;

    private static CalibrationManager _instance;
    public static CalibrationManager Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType(typeof(CalibrationManager)) as CalibrationManager;
            if (_instance == null)
            {
                //Debug.Log("Could not find Calibration Manager");
            }
            return _instance;
        }
    }

    private float _elapsedTimeToStart = 5f;

    private bool _donotrunagain;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }   
    
    private void ActivateArrows()
    {
        if (instructiontext.text == "Move Left")
        {
            leftArrow.gameObject.SetActive(true);
            rightArrow.gameObject.SetActive(false);
            upArrow.gameObject.SetActive(false);
            downArrow.gameObject.SetActive(false);
        }
        else if (instructiontext.text == "Move Right")
        {
            leftArrow.gameObject.SetActive(false);
            rightArrow.gameObject.SetActive(true);
            upArrow.gameObject.SetActive(false);
            downArrow.gameObject.SetActive(false);
        }
        else if (instructiontext.text == "Move Top")
        {
            leftArrow.gameObject.SetActive(false);
            rightArrow.gameObject.SetActive(false);
            upArrow.gameObject.SetActive(true);
            downArrow.gameObject.SetActive(false);
        }
        else if (instructiontext.text == "Move Bottom")
        {
            leftArrow.gameObject.SetActive(false);
            rightArrow.gameObject.SetActive(false);
            upArrow.gameObject.SetActive(false);
            downArrow.gameObject.SetActive(true);
        }
        else
        {
            leftArrow.gameObject.SetActive(false);
            rightArrow.gameObject.SetActive(false);
            upArrow.gameObject.SetActive(false);
            downArrow.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (CalibarionDone && !_donotrunagain)
        {
            ActivateArrows();
            _elapsedTimeToStart -= Time.deltaTime;
            instructiontext.text = "Level Loading in " + ((int)_elapsedTimeToStart) + " ";
            if (_elapsedTimeToStart < 0f)
            {
                Application.LoadLevel("City");
                _donotrunagain = true;
            }
        }
    
    }
#region CleanUpStuff

    private void ReCalibrate()
    {
        CalibarionDone = false;
        Calibrating = false;
        target.position = new Vector3(0, 0, target.position.z);
    }

#endregion
}
