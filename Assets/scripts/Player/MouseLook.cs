using UnityEngine;

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
        private float sensitivityX = 0.5F;
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
        private static float _playerRotY;

        void Update()
        {
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
                var currentRotationAngle = transform.eulerAngles.y;
                var wantedRotationAngle = transform.eulerAngles.y  +  (60 * axis);
                
                var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);
                var wantedRotation = Quaternion.Euler(0, wantedRotationAngle, 0);

                transform.rotation = Quaternion.Lerp(currentRotation, wantedRotation, Time.deltaTime * sensitivityX);
                //transform.Rotate(0, axis * sensitivityX, 0);
                

                //Debug.Log("rotating");
            }
            else
            {
                rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

                transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
                
            }
        }

        void Start()
        {
            canRotate = true;
            // Make the rigid body not change rotation
            if (GetComponent<Rigidbody>())
                GetComponent<Rigidbody>().freezeRotation = true;
        }

        public static float PlayerRotation()
        {
            return _playerRotY;
        }
    }
}