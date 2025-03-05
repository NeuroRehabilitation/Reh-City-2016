using UnityEngine;

namespace Assets.scripts.RehabCar
{
    public class RehabCar : MonoBehaviour {

        public Transform[] frontWheels;
        public Transform[] rearWheels;
        public Transform WheelFL;
        public Transform WheelFR;
        public Transform WheelRR;
        public Transform WheelRL;

        //private float throttle;
        //private float steer;
  
        public Transform CenterofMass;
  
        public float maxTorque;
        public float decelerationtorque;
        private float currenttorque;

        public float topspeed;
        public float currentspeed;

        private float speefactor;
        public float highSteerangle;
        public float lowestSteerangle;
        private float currentsteerangle;
    
        void Start () {
            rigidbody.centerOfMass = CenterofMass.localPosition;
            topspeed = Convert_Miles_Per_Hour_To_Meters_Per_Second(topspeed);
        }

        void GetInput()
        {
            //throttle = Input.GetAxis("Vertical");
            //steer = Input.GetAxis("Horizontal");
            //Debug.Log(steer);
        }
        void Update () {
            GetInput();
            RotateWheels();
        }
        void FixedUpdate()
        {
            //ApplyThrottle(throttle);
            //ApplySteering(steer);
        }
        public void ApplyThrottle(float throttle)
        {
            currentspeed = Convert_Miles_Per_Hour_To_Meters_Per_Second(2 * Mathf.PI * rearWheels[0].GetComponent<WheelCollider>().radius * rearWheels[0].GetComponent<WheelCollider>().rpm*60/1000);
            foreach (Transform t in rearWheels)
            {
                if (throttle != 0 && currentspeed<topspeed)
                {
                    t.GetComponent<WheelCollider>().brakeTorque = 0;
                    t.GetComponent<WheelCollider>().motorTorque = maxTorque * throttle;
                }
                else if (throttle != 0 && currentspeed >= topspeed)
                {
                    t.GetComponent<WheelCollider>().motorTorque = 0;
                }
                else
                {
                    t.GetComponent<WheelCollider>().brakeTorque = decelerationtorque;
                }
            }
        }
        public void ApplySteering(float steering)
        {
            speefactor = rigidbody.velocity.magnitude / maxTorque;
            currentsteerangle = Mathf.Lerp(lowestSteerangle, highSteerangle, speefactor);
            currentsteerangle *= steering;
            foreach (Transform t in frontWheels)
            {
                t.GetComponent<WheelCollider>().steerAngle = currentsteerangle;
            }
        }
        public void ApplyBrakes(float intensity)
        {
            foreach (Transform t in rearWheels)
            {
                t.GetComponent<WheelCollider>().brakeTorque = decelerationtorque * intensity;
            }
        }
        public void ApplyHandBrakes(float intensity)
        {
            foreach (Transform t in frontWheels)
            {
                t.GetComponent<WheelCollider>().brakeTorque = decelerationtorque * intensity;
            }
            foreach (Transform t in rearWheels)
            {
                t.GetComponent<WheelCollider>().brakeTorque = decelerationtorque * intensity;
            }
        }
        public void ReleaseBrakes()
        {
            foreach (Transform t in frontWheels)
            {
                t.GetComponent<WheelCollider>().brakeTorque = 0;
            }
        }
        void RotateWheels()
        {
            WheelFL.Rotate(frontWheels[0].GetComponent<WheelCollider>().rpm / 60 * 360 * Time.deltaTime, 0, 0);
            WheelFR.Rotate(frontWheels[1].GetComponent<WheelCollider>().rpm / 60 * 360 * Time.deltaTime, 0, 0);
            WheelRL.Rotate(rearWheels[0].GetComponent<WheelCollider>().rpm / 60 * 360 * Time.deltaTime, 0, 0);
            WheelRR.Rotate(rearWheels[1].GetComponent<WheelCollider>().rpm / 60 * 360 * Time.deltaTime, 0, 0);
            WheelFL.localEulerAngles = new Vector3(WheelFL.localEulerAngles.x, frontWheels[0].GetComponent<WheelCollider>().steerAngle - WheelFL.localEulerAngles.z, WheelFL.localEulerAngles.z);
            WheelFR.localEulerAngles = new Vector3(WheelFR.localEulerAngles.x, frontWheels[1].GetComponent<WheelCollider>().steerAngle - WheelFR.localEulerAngles.z, WheelFR.localEulerAngles.z);    
        }
        float Convert_Miles_Per_Hour_To_Meters_Per_Second(float speed)
        {
            return speed * 0.44704f;
        }
    }
}
