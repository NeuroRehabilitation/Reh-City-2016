using UnityEngine;

namespace Assets.scripts.RehabCar
{
    public class CarWheel : MonoBehaviour {
	
        public WheelCollider coll;

        public bool driveWheel = false;
	
        public bool steerWheel = false;
	
        public Vector3 wheelVelocity = Vector3.zero;
	
        public Vector3 groundSpeed = Vector3.zero;

        // Use this for initialization
        void Start () {
		
        }
	
        public void setwheelcollider()
        {
            coll = GetComponent<WheelCollider>();
        }
        // Update is called once per Frame
        void Update () {
	
        }
    }
}
