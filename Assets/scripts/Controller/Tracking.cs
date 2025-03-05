using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using Assets.scripts.GUI;

public class Tracking : MonoBehaviour {

    private List<GameObject> _targets = new List<GameObject>();
    public Text MessageText;
    public GameObject MovingCursor;
    
    public static bool  CanFinishTask;
    public static bool CanShowNextCircle, CanHideCircle, TargetSet;
    private int _randTarget;
    private GameObject _controller/*, _target*/;

    public static bool IsTracking;

    public static Vector3 CursorPosition;
    public static Vector3 CirclePosition;

    public static Vector3 ScaledCirclePosition;

    public static bool PerformTracking, FirstCircleSet;

    private float _width, _height;

    private void Start ()
    {
        //_target = Resources.Load("Target") as GameObject;
        
        _width = GetComponent<RectTransform>().rect.width;
        _height = GetComponent<RectTransform>().rect.height;
        
        _targets = GameObject.FindGameObjectsWithTag("TrackingTarget").ToList();

        SetTargetsPositions();

        //if (_target != null) Debug.Log(_target.tag);
        for (var i = 0; i < _targets.Count; i++)
	    {
	        _targets[i].SetActive(false);
	    }

        _controller = GameObject.FindGameObjectWithTag("hand");

        MessageText.text = "Pressione 'Enter' para começar";
        MessageText.gameObject.SetActive(true);
    }

    private void SetTargetsPositions()
    {
        _targets[0].transform.position = new Vector3(_width / 8, _height - _height / 8, 0);
        _targets[1].transform.position = new Vector3(_width / 8, _height*0.5f - _height / 8, 0);
        _targets[2].transform.position = new Vector3(_width / 8, _height / 8, 0);
        _targets[3].transform.position = new Vector3(_width * 0.5f - _width / 8, _height - _height / 8, 0);
        _targets[4].transform.position = new Vector3(_width * 0.5f - _width / 8, _height * 0.5f - _height / 8, 0);
        _targets[5].transform.position = new Vector3(_width * 0.5f - _width / 8, _height / 8, 0);
        _targets[6].transform.position = new Vector3(_width * 0.5f + _width / 8, _height - _height / 8, 0);
        _targets[7].transform.position = new Vector3(_width * 0.5f + _width / 8, _height * 0.5f - _height / 8, 0);
        _targets[8].transform.position = new Vector3(_width * 0.5f + _width / 8, _height / 8, 0);
        _targets[9].transform.position = new Vector3(_width - _width / 8, _height - _height / 8, 0);
        _targets[10].transform.position = new Vector3(_width - _width / 8, _height * 0.5f - _height / 8, 0);
        _targets[11].transform.position = new Vector3(_width - _width / 8, _height / 8, 0);
    }


    private void Update ()
	{
	    if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
	    {
	        if (!CanShowNextCircle)
	        {
	            CanHideCircle = false;
	            CanShowNextCircle = true;
	        }
	        else
	        {
                CanShowNextCircle = false;
                CanHideCircle = true;
	        }
	    }

	    IsTracking = gameObject.activeSelf;
        
        MovingCursor.GetComponent<RectTransform>().position = new Vector3((_width * 0.5f) + _width * 0.5f * _controller.transform.position.x, (_height * 0.5f) + _height * 0.5f * _controller.transform.position.z, 0);
        
        CursorPosition = MovingCursor.transform.position;
        
	    if(CanShowNextCircle && _targets.Count > 0)
        {
            if (!TargetSet)
            {
                MessageText.text = "Preste atenção ao círculo verde que está a ser apresentado";
                _randTarget = Random.Range(0, _targets.Count);
                _targets[_randTarget].SetActive(true);
                MovingCursor.SetActive(false);
                TargetSet = true;
            }
            else
            {
                CirclePosition = _targets[_randTarget].transform.position;
                ScaledCirclePosition = ScaleCirclePosition(CirclePosition);
                FirstCircleSet = true;
            } 
        }
        else if (CanHideCircle)
        {
            Debug.Log(CanHideCircle);
            MovingCursor.SetActive(true);
            MessageText.text =  "Mova o cursor laranja para o sítio onde o círculo verde foi apresentado";
            TargetSet = false;
            _targets[_randTarget].SetActive(false);
            _targets.RemoveAt(_randTarget);
            CanHideCircle = false;
            //CirclePosition = Vector3.zero;
        }

	    if (CanShowNextCircle && _targets.Count == 0 && !CanFinishTask)
	    {
	        MessageText.text = "Pressione 'Enter' para terminar";
            MovingCursor.SetActive(true);
	        CanShowNextCircle = false;
	        CanFinishTask = true;
	    }

	    if (CanFinishTask && CanShowNextCircle)
	    {
            gameObject.SetActive(false);
            IsTracking = false;
            CalibrationGUI.CalibDone = true;
            LoadSaveSettings.SaveSettingsInfo("Tracking");
        }
	}

    private Vector3 ScaleCirclePosition(Vector3 circlePos)
    {
        var width = GetComponent<RectTransform>().rect.width;
        var height = GetComponent<RectTransform>().rect.height;


        var scalex = circlePos.x * (2.0f / width) - 1;
        var scaley = circlePos.y *(2.0f / height) -1;

        var scaledPos = new Vector3(scalex, scaley, 0);

        return scaledPos;
    }
}
