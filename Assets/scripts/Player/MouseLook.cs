using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.scripts.Player
{
    /// MouseLook rotates the transform based on the mouse delta.
    /// Minimum and Maximum values can be used to constrain the possible rotation

    /// To make an FPS style character:
    /// - Create a capsule.
    /// - Add the MouseLook script to the capsule.
    ///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
    /// - Add FPSInputController script to the capsule
    ///   -> A CharacterMotor and a CharacterController component will be automatically added.

    /// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
    /// - Add a MouseLook script to the camera.
    ///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
    [AddComponentMenu("Camera-Control/Mouse Look")]
    public class MouseLook : MonoBehaviour
    {
        public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
        public RotationAxes axes = RotationAxes.MouseXAndY;
        private static float sensitivityX = 0.5F;
        private float sensitivityY = 0.5F;

        //private float minimumX = -360F;
        //private float maximumX = 360F;

        private float minimumY = -5F;
        private float maximumY = 5F;

        public float HorizontalAxis = 0;
        private float axis;
        public bool canRotate;
        float rotationY;
        public bool IsCalibrationRequired = false;
        public bool IsAndroid = false;
        private static float _playerRotY, _angle = 60;
        private static string _speed;

        public Slider SensitSlider;
        
        
        public Text SensitivityValue;
        public Text SpeedValue;
        private static string _tempSpeed;
        private static bool _canUpdateSpeed, _canUpdateAcceleration;
        public static bool CanGetAcceleration;
        private static string _acceleration;
        public GameObject NavigationMenu;

        void Start()
        {
            canRotate = true;
            // Make the rigid body not change rotation
            if (rigidbody)
                rigidbody.freezeRotation = true;
            
            SensitSlider.value = sensitivityX;

            if (_acceleration != "")
                _canUpdateAcceleration = true;

            NavigationMenu.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                if (NavigationMenu.activeSelf)
                    LoadSaveSettings.SaveSettingsInfo("NavigationSettings");
                //else
                //    LoadSaveSettings.LoadSettingsValues();

                CanGetAcceleration = true;
                NavigationMenu.SetActive(!NavigationMenu.activeSelf);
            }

            sensitivityX = SensitSlider.value;
            SensitivityValue.text = sensitivityX.ToString("0.0");
            SpeedValue.text = FPSInputController.minimumforwardspeed.ToString("0.0");
            _speed = SpeedValue.text;
            if (_canUpdateSpeed)
            {
                GetComponent<FPSInputController>().minSpeed.value = float.Parse(_tempSpeed);
                _canUpdateSpeed = false;
            }

            if (_canUpdateAcceleration)
            {
                switch (_acceleration)
                {
                    case "prop":
                        GetComponent<FPSInputController>().exponLength.isOn = false;
                        GetComponent<FPSInputController>().propLength.isOn = true;
                        GetComponent<FPSInputController>().logLength.isOn = false;
                        break;
                    case "log":
                        GetComponent<FPSInputController>().exponLength.isOn = false;
                        GetComponent<FPSInputController>().propLength.isOn = false;
                        GetComponent<FPSInputController>().logLength.isOn = true;
                        break;
                    case "exp":
                        GetComponent<FPSInputController>().exponLength.isOn = true;
                        GetComponent<FPSInputController>().propLength.isOn = false;
                        GetComponent<FPSInputController>().logLength.isOn = false;
                        break;
                }
                _canUpdateAcceleration = false;
            }

            if (CanGetAcceleration)
            {
                if (GetComponent<FPSInputController>().exponLength.isOn)
                    _acceleration = "exp";
                if(GetComponent<FPSInputController>().propLength.isOn)
                    _acceleration = "prop";
                if(GetComponent<FPSInputController>().logLength.isOn)
                    _acceleration = "log";
                //Debug.Log("acceleration: " + _acceleration);
                CanGetAcceleration = false;
            }

            _playerRotY = transform.eulerAngles.y;

            if (!canRotate) return;
            if (IsCalibrationRequired || IsAndroid)
            {
                axis = HorizontalAxis;
            }
            else
            {
                axis = Input.GetAxis("Mouse X");
            }

            if (axes == RotationAxes.MouseXAndY)
            {
                float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX * Time.deltaTime;

                rotationY += Input.GetAxis("Mouse Y") * sensitivityY * Time.deltaTime;
                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

                transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
            }
            else if (axes == RotationAxes.MouseX)
            {
                if (!NavigationSettings.FullFreeze && (axis > NavigationSettings.SidesThreshold || axis < -NavigationSettings.SidesThreshold))
                {
                    
                    var currentRotationAngle = transform.eulerAngles.y;
                    var wantedRotationAngle = transform.eulerAngles.y + (_angle*axis);

                    var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);
                    var wantedRotation = Quaternion.Euler(0, wantedRotationAngle, 0);

                    transform.rotation = Quaternion.Lerp(currentRotation, wantedRotation, Time.deltaTime*sensitivityX);
                    //transform.Rotate(0, axis * sensitivityX, 0);
                }
            }
            else
            {
                rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

                transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
            }

            if (axis > -NavigationSettings.SidesThreshold && axis < NavigationSettings.SidesThreshold)
                FPSInputController.isInsideThresholds = true;
            else
                FPSInputController.isInsideThresholds = false;
        }

        public static string ReturnSpeedValue()
        {
            return _speed;
        }

        public static string ReturnAngleValue()
        {
            return _angle.ToString(CultureInfo.InvariantCulture);
        }

        public static string ReturnSensitValue()
        {
            return sensitivityX.ToString(CultureInfo.InvariantCulture);
        }

        public static void SetSpeedValue(string speed)
        {
            _tempSpeed = speed;
            _canUpdateSpeed = true;
        }

        public static void SetAngleValue(string angle)
        {
            _angle = float.Parse(angle);
        }

        public static void SetSensitValue(string sensi)
        {
            sensitivityX = float.Parse(sensi);
        }

        public static void SetAcceleration(string type)
        {
            _acceleration = type;
            _canUpdateAcceleration = true;
        }

        public static string AccelerationType()
        {
            return _acceleration;
        }

        public static float PlayerRotation()
        {
            return _playerRotY;
        }

  
    }
}