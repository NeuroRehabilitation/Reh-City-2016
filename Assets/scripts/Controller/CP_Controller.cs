using Assets.scripts.GUI;
using Assets.scripts.Manager;
using Assets.scripts.objectives;
using Assets.scripts.RehaTask.RehaTask_GameLogic;
using UnityEngine;

namespace Assets.scripts.Controller
{
    public class CP_Controller : MonoBehaviour
    {
        public float MinXLimit, MaxXLimit, MinZLimit, MaxZLimit, MinYLimit, MaxYLimit;
        public static float TargetPosSlide;

        private readonly Color _selectedColor = new Color(0.23f, 0.95f, 0.21f, 0.35f);
        private readonly Color _deSelectedColor = new Color(0, 0, 0, 0);

        public GameObject SelectedObject;
        public bool Selected;

        public bool SelectedArrow, AtmButton;
        public string WriteGitterObjectName;

        public GameObject CrossHair, Timer;

        private GameManager gm;

        private float _speed;
        private float _horizontalAxis;
        private float _verticalAxis;
        private Vector3 _rotationVector;
        public bool _timerActivated;

        private static CP_Controller s_Instance = null;
        private static string _name = "NaN";

        private ObjectiveManager _objManager;

        public static CP_Controller Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindObjectOfType(typeof (CP_Controller)) as CP_Controller;
                    if (s_Instance == null)
                    {
                        Debug.Log("Could not locate CP_Controller");
                    }
                }
                return s_Instance;
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(this.gameObject);
            gm = GameManager.Instance;
            _objManager = gm.GetComponent<ObjectiveManager>();
            _rotationVector = this.gameObject.transform.rotation.eulerAngles;
        }

        private void Update()
        {
            if (Selected && SelectedObject != null)
            {
                if (Application.loadedLevelName == "Bank" && _objManager.GetCurrentObjective!= null && !_objManager.GetCurrentObjective.GetType().ToString().Contains("Code"))
                {
                    if (SelectedObject.GetComponentInChildren<TextMesh>() != null)
                    {
                        _name = SelectedObject.GetComponentInChildren<TextMesh>().name;
                    }
                }
                else
                {
                    _name = SelectedObject.name;
                }
            }
            else
                _name = "NaN";
            
            if (Controller.B6())
                SelectedObject = null;

            if (Selected && SelectedObject != null && SelectedObject.GetComponent<BoxCollider>().enabled == false)
                SelectedObject.transform.FindChild("Cylinder").GetComponent<Renderer>().material.color = _deSelectedColor;

            if (!DrawObjectiveList.Minimized)
            {
                this.GetComponent<Collider>().enabled = false;
                this.transform.position = new Vector3(0, 0, 0);
            }
            else
                this.GetComponent<Collider>().enabled = true;

            if (Application.loadedLevelName == "Home" || Application.loadedLevelName == "FashionStore" || Application.loadedLevelName == "Park")
            {
                this.gameObject.transform.localScale = new Vector3(0.04f, 0.04f, 0.8f);
                CrossHair.transform.localScale = new Vector3(2, 2, 0.001f);
                CrossHair.transform.localPosition = new Vector3(CrossHair.transform.localPosition.x, CrossHair.transform.localPosition.y, 0.04f);

                _rotationVector.x = 90f;
                this.gameObject.transform.rotation = Quaternion.Euler(_rotationVector);

                CrossHair.SetActive(SpawnTiles.RenderPointer);
            }
            else
            {
                _rotationVector.x = 0;
                this.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 30f);
                //Timer.transform.localScale = new Vector3();
                CrossHair.transform.localScale = new Vector3(8, 8, 0.001f);
                CrossHair.transform.localPosition = new Vector3(CrossHair.transform.localPosition.x, CrossHair.transform.localPosition.y, 0.14f);
                this.gameObject.transform.rotation = Quaternion.Euler(_rotationVector);
            }

            if (!gm.IsCalibrationRequired)
            {
                _horizontalAxis = UnityEngine.Input.GetAxis("Horizontal");
                _verticalAxis = UnityEngine.Input.GetAxis("Vertical");

                if (Application.loadedLevelName == "Home" || Application.loadedLevelName == "FashionStore" || Application.loadedLevelName == "Park")
                    _speed = Time.deltaTime*1f;
                else
                    _speed = Time.deltaTime*60f;

                var h = _speed*_horizontalAxis;
                var v = _speed*_verticalAxis;

                transform.Translate(h, v, 0);
            }

            if (Application.loadedLevel == 2)
            {
                MinXLimit = -1;
                MaxXLimit = 1;
                MinZLimit = -1;
                MaxZLimit = 1;
                MinYLimit = -1;
                MaxYLimit = 1;
                CrossHair.SetActive(false);
                Selected = false;
                SelectedArrow = false;
                AtmButton = false;
            }
            else if (Application.loadedLevelName == "Home" || Application.loadedLevelName == "FashionStore" || Application.loadedLevelName == "Park")
            {
                MinXLimit = -1.5f;
                MaxXLimit = 1.50f;
                MinZLimit = -2.7f;
                MaxZLimit = -1.45f;
                this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, 0.893f,
                    this.gameObject.transform.position.z);
            }
            else
            {
                CrossHair.SetActive(true);

                var zero = new Vector3(0, 0, 82);
                //zero.z = UnityEngine.Camera.main.nearClipPlane;
                MinXLimit = UnityEngine.Camera.main.ScreenToWorldPoint(zero).x;

                var width = new Vector3(Screen.width, 0, 82);
                //width.z = UnityEngine.Camera.main.nearClipPlane;
                MaxXLimit = UnityEngine.Camera.main.ScreenToWorldPoint(width).x;

                MinYLimit = UnityEngine.Camera.main.ScreenToWorldPoint(zero).y;

                var height = new Vector3(0, Screen.height, 82);
                //height.z = UnityEngine.Camera.main.nearClipPlane;
                MaxYLimit = UnityEngine.Camera.main.ScreenToWorldPoint(height).y;
              
                TargetPosSlide = MaxXLimit - MinXLimit;
            }

            if (this.gameObject.transform.position.x > MaxXLimit)
                this.gameObject.transform.position = new Vector3(MaxXLimit, this.gameObject.transform.position.y,
                    this.gameObject.transform.position.z);

            if (this.gameObject.transform.position.x < MinXLimit)
                this.gameObject.transform.position = new Vector3(MinXLimit, this.gameObject.transform.position.y,
                    this.gameObject.transform.position.z);

            if (Application.loadedLevelName != "Home" && Application.loadedLevelName != "FashionStore" && Application.loadedLevelName != "Park")
            {
                if (this.gameObject.transform.position.y < MinYLimit)
                    this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, MinYLimit,
                        this.gameObject.transform.position.z);

                if (this.gameObject.transform.position.y > MaxYLimit)
                    this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, MaxYLimit,
                        this.gameObject.transform.position.z);
            }

            if (this.gameObject.transform.position.z < MinZLimit)
                this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x,
                    this.gameObject.transform.position.y, MinZLimit);

            if (this.gameObject.transform.position.z > MaxZLimit)
                this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x,
                    this.gameObject.transform.position.y, MaxZLimit);

            if (!Selected && SelectedObject != null && !SelectedArrow)
                SelectedObject.transform.FindChild("Cylinder").GetComponent<Renderer>().material.color = _deSelectedColor;
        }

        private void OnTriggerEnter()
        {
            TimerCount.MyTimer = TimerCount.StartTime;
        }
        
        private void OnTriggerStay(Component cl)
        {
            if (SelectedObject != null)
                SelectedObject.transform.FindChild("Cylinder").GetComponent<Renderer>().material.color = _deSelectedColor;
                
            if (cl.tag == "item_1" || cl.tag == "directionalarrow" || cl.tag == "SelectionArrows" || cl.tag == "keyNumber" || cl.tag == "continueButton")
            {
                if (gm.IsCalibrationRequired)
                {
                    Timer.SetActive(true);
                    if (!_timerActivated)
                    {
                        TimerCount.Timeout = false;
                        TimerCount.MyTimer = TimerCount.StartTime;
                        _timerActivated = true;
                    }
                }

                switch (cl.tag)
                {
                    case "item_1":
                        Selected = true;
                        SelectedObject = cl.gameObject;
                        SelectedObject.transform.FindChild("Cylinder").GetComponent<Renderer>().material.color = _selectedColor;
                        break;
                    case "directionalarrow":
                        SelectedArrow = true;
                        SelectedObject = cl.gameObject;
                        SelectedObject.transform.FindChild("Cylinder").GetComponent<Renderer>().material.color = _selectedColor;
                        break;
                    case "SelectionArrows":
                        Selected = true;
                        SelectedArrow = true;
                        SelectedObject = cl.gameObject;
                        SelectedObject.transform.FindChild("Cylinder").GetComponent<Renderer>().material.color = _selectedColor;
                        AtmButton = true;
                        break;
                    case "keyNumber":
                        Selected = true;
                        SelectedObject = cl.gameObject;
                        SelectedObject.transform.FindChild("Cylinder").GetComponent<Renderer>().material.color = _selectedColor;
                        break;
                    case "continueButton":
                        Selected = true;
                        SelectedObject = cl.gameObject;
                        SelectedObject.transform.FindChild("Cylinder").GetComponent<Renderer>().material.color = _selectedColor;
                        break;
                    default:
                        Selected = false;
                        SelectedArrow = false;
                        AtmButton = false;
                        break;
                }
            }
            
            if ((gm.IsCalibrationRequired && TimerCount.MyTimer == -1))
            {
                TimerCount.ControllerB1 = true;
                TimerCount.MyTimer = -2;
            }
        }

        private void OnTriggerExit(Component cl)
        {
            _timerActivated = false;
            Timer.SetActive(false);
            Selected = false;
            SelectedArrow = false;
            AtmButton = false;
            

            if (SelectedObject != null)
                SelectedObject.transform.FindChild("Cylinder").GetComponent<Renderer>().material.color = _deSelectedColor;
        }

        public static string SelectedObjectName()
        {
            return _name;
        }

        private void OnApplicationQuit()
        {
            PerformanceProcessor.ProcessPerformance();
        }
    }
}
