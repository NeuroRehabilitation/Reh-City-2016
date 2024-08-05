using System.Collections;
using UnityEngine;

namespace Assets.scripts.RehabCar
{
    public class CarControl : MonoBehaviour {
	
        struct carwheel
        {
            WheelCollider coll;
            bool driveWheel ;
            bool steerWheel ;
            Vector3 wheelVelo ;
            Vector3 groundSpeed ;
        }
	
	
        public float suspensionRange = 0.1f;
        public float suspensionDamper = 50;
        public float suspensionSpringFront = 18500;
        public float suspensionSpringRear = 9000;
	
        private Vector3 dragMultiplier = new Vector3(2, 5, 1);
	
        private float throttle=0;
        private float steer=0;
        private bool handbrake = false;
	
        public GameObject[] frontwheels;
        public GameObject[] rearwheels;
	
        //private carwheel[] wheels = new carwheel[frontwheels.Length + rearwheels.Length];
        public Transform centerOfMass;
	
        private WheelFrictionCurve wfc;
	
        public float topSpeed;
        public int numberOfGears = 5;
	
        public int maximumTurn = 15;
        public int minimumTurn = 10;
	
        private float[] engineForceValues;
        private float[] gearSpeeds;
	
        private int currentGear;
        private float currentEnginePower = 0.0f;
	
        private float handbrakeXDragFactor = 0.5f;
        private float initialDragMultiplierX =10.0f;
        private float handbrakeTime = 0.0f;
        private float handbrakeTimer =1.0f;
        public bool CanDrive=false;
        public bool CanSteer = false;
	
        private float normPower;
        // Use this for initialization
        void Start()
        {
            foreach(GameObject g in frontwheels)
            {
                SetUpWheels(g.transform,true);
            }
            foreach(GameObject g in rearwheels)
            {
                SetUpWheels(g.transform,false);
            }
            SetUpCenterOfMass();
            topSpeed = Convert_Miles_Per_Hour_To_Meters_Per_Second(topSpeed);
            SetUpGears();
            initialDragMultiplierX = dragMultiplier.x;
        }
        void Update()
        {
            Vector3 relativeVelocity = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity);
            GetInput();
            UpdateGear(relativeVelocity);
        }
        void FixedUpdate()
        {
            Vector3 relativeVelocity = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity);
		
            CalculateState();	
	
            UpdateFriction(relativeVelocity);
	
            UpdateDrag(relativeVelocity);
	
            CalculateEnginePower(relativeVelocity);
	
            ApplyThrottle(CanDrive, relativeVelocity);
	
            ApplySteering(CanSteer, relativeVelocity);
		
        }
        void SetUpWheels(Transform wheelTransform,bool isFrontWheel)
        {
            SetUpWheelFrictionCurve();
            wheelTransform.GetComponent<CarWheel>().setwheelcollider();
            WheelCollider wc = wheelTransform.GetComponent<WheelCollider>();
            wc.suspensionDistance = suspensionRange;
            JointSpring js = wc.suspensionSpring;
		
            if(isFrontWheel)
                js.spring = suspensionSpringFront;
            else
                js.spring = suspensionSpringRear;
		
            js.damper = suspensionDamper;
            wc.suspensionSpring = js;
            wc.sidewaysFriction = wfc;
		
            CarWheel wheel = wc.GetComponent<CarWheel>();
            if(isFrontWheel)
                wheel.driveWheel = true;
            else
                wheel.steerWheel = true;
        }
        void SetUpWheelFrictionCurve()
        {
            wfc = new WheelFrictionCurve();
            wfc.extremumSlip = 1;
            wfc.extremumValue = 50;
            wfc.asymptoteSlip = 2;
            wfc.asymptoteValue = 25;
            wfc.stiffness = 1;
        }
	
        void SetUpCenterOfMass()
        {
            if(centerOfMass!=null)
                GetComponent<Rigidbody>().centerOfMass = centerOfMass.localPosition;
        }
	
        void SetUpGears()
        {
            engineForceValues = new float[numberOfGears];
            gearSpeeds = new float[numberOfGears];
		
            float temptopspeed = topSpeed;
            for(int i=0;i<numberOfGears;i++)
            {
                if(i>0)
                    gearSpeeds[i] = temptopspeed/4+gearSpeeds[i-1];
                else
                    gearSpeeds[i] = temptopspeed/4;
			
                temptopspeed -= temptopspeed/4;
            }
		
            float engineFactor = topSpeed / gearSpeeds[gearSpeeds.Length - 1];
		
            for(int i=0;i<numberOfGears;i++)
            {
                float maxLinearDrag = gearSpeeds[i]*gearSpeeds[i];
                engineForceValues[i] = maxLinearDrag * engineFactor;
                Debug.Log(engineForceValues[i]);
            }
        }
	
        void GetInput()
        {
            throttle = Input.GetAxis("Vertical");
            steer = Input.GetAxis("Horizontal");
        }
	
        void CheckHandbrake()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                if(!handbrake)
                {
                    handbrake = true;
                    handbrakeTime = Time.time;
                    dragMultiplier.x = initialDragMultiplierX * handbrakeXDragFactor;
                }
            }else if(handbrake)
            {
                handbrake=false;
                StartCoroutine("StopHandbraking",(Mathf.Min(5,Time.time-handbrakeTime)));	
            }
        }
	
        IEnumerator StopHandbraking(float seconds)
        {
            float diff = initialDragMultiplierX-dragMultiplier.x;
            handbrakeTimer = 1;
		
            while(dragMultiplier.x<initialDragMultiplierX && !handbrake)
            {
                dragMultiplier.x += diff * (Time.deltaTime/seconds);
                handbrakeTimer -= Time.deltaTime/seconds;
                yield return null;
            }
            dragMultiplier.x = initialDragMultiplierX;
            handbrakeTimer = 0;
            yield return null;
        }
	
        void UpdateGear(Vector3 relativeVelocity)
        {
            currentGear = 0;
            for(int i = 0; i < numberOfGears - 1; i++)
            {	
                if(relativeVelocity.z > gearSpeeds[i])
                    currentGear = i + 1;
            }
        }
	
	
        void UpdateDrag(Vector3 relativeVelocity)
        {	
            Vector3 relativeDrag  = new Vector3(	-relativeVelocity.x * Mathf.Abs(relativeVelocity.x), 
                -relativeVelocity.y * Mathf.Abs(relativeVelocity.y), 
                -relativeVelocity.z * Mathf.Abs(relativeVelocity.z) );
	
            Vector3 drag = Vector3.Scale(dragMultiplier, relativeDrag);
		
            if(initialDragMultiplierX > dragMultiplier.x) // Handbrake code
            {			
                drag.x /= (relativeVelocity.magnitude / (topSpeed / ( 1 + 2 * handbrakeXDragFactor ) ) );
                drag.z *= (1 + Mathf.Abs(Vector3.Dot(GetComponent<Rigidbody>().velocity.normalized, transform.forward)));
                drag += GetComponent<Rigidbody>().velocity * Mathf.Clamp01(GetComponent<Rigidbody>().velocity.magnitude / topSpeed);
            }
            else // No handbrake
            {
                drag.x *= topSpeed / relativeVelocity.magnitude;
            }
	
            if(Mathf.Abs(relativeVelocity.x) < 5 && !handbrake)
                drag.x = -relativeVelocity.x * dragMultiplier.x;
		

            GetComponent<Rigidbody>().AddForce(transform.TransformDirection(drag) * GetComponent<Rigidbody>().mass * Time.deltaTime);
        }
	
	

        void UpdateFriction(Vector3 relativeVelocity)
        {
            float sqrVel  = relativeVelocity.x * relativeVelocity.x;
	
            // Add extra sideways friction based on the car's turning velocity to avoid slipping
            wfc.extremumValue = Mathf.Clamp(300 - sqrVel, 0, 300);
            wfc.asymptoteValue = Mathf.Clamp(150 - (sqrVel / 2), 0, 150);
            foreach(GameObject g in frontwheels)
            {
                g.GetComponent<WheelCollider>().sidewaysFriction = wfc;
                g.GetComponent<WheelCollider>().forwardFriction = wfc;
            }
            foreach(GameObject g in rearwheels)
            {
                g.GetComponent<WheelCollider>().sidewaysFriction = wfc;
                g.GetComponent<WheelCollider>().forwardFriction = wfc;
            }
        }
	
	
        void CalculateEnginePower(Vector3 relativeVelocity)
        {
            if(throttle == 0)
            {
                currentEnginePower -= Time.deltaTime * 200;
            }
            else if( HaveTheSameSign(relativeVelocity.z, throttle) )
            {
                normPower = (currentEnginePower / engineForceValues[engineForceValues.Length - 1]) * 2;
                currentEnginePower += Time.deltaTime * 200 * EvaluateNormPower(normPower);
            }
            else
            {
                currentEnginePower -= Time.deltaTime * 300;
            }
	
            if(currentGear == 0)
                currentEnginePower = Mathf.Clamp(currentEnginePower, 0, engineForceValues[0]);
            else
                currentEnginePower = Mathf.Clamp(currentEnginePower, engineForceValues[currentGear - 1], engineForceValues[currentGear]);
        }
	
	
        void CalculateState()
        {
            CanDrive = false;
            CanSteer = false;
            foreach(GameObject g in frontwheels)
            {
                if(g.GetComponent<CarWheel>().coll.isGrounded)
                {
                    if(g.GetComponent<CarWheel>().driveWheel)
                        CanDrive = true;
                    if(g.GetComponent<CarWheel>().steerWheel)
                        CanSteer = true;
                }
            }
        }
	
        void ApplyThrottle(bool canDrive,Vector3 relativeVelocity)
        {
            if(canDrive)
            {
                float throttleforce=0;
                float brakeForce = 0;
                if(HaveTheSameSign(relativeVelocity.z,throttle))
                {
                    if(!handbrake)
                        throttleforce = Mathf.Sign(throttle)*currentEnginePower*GetComponent<Rigidbody>().mass;
                }
                brakeForce = Mathf.Sign(throttle) * engineForceValues[0] * GetComponent<Rigidbody>().mass;
                GetComponent<Rigidbody>().AddForce(transform.forward * Time.deltaTime *(throttleforce+brakeForce));
            }
        }
	
        void ApplySteering(bool canSteer,Vector3 relativeVelocity)
        {
		
            float turnRadius = 3.0f / Mathf.Sin((90 - (steer * 30)) * Mathf.Deg2Rad);
            float minMaxTurn = EvaluateSpeedToTurn(GetComponent<Rigidbody>().velocity.magnitude);
            float turnSpeed  = Mathf.Clamp(relativeVelocity.z / turnRadius, -minMaxTurn / 10, minMaxTurn / 10);
		
            transform.RotateAround(	transform.position + transform.right * turnRadius * steer, 
                transform.up, 
                turnSpeed * Mathf.Rad2Deg * Time.deltaTime * steer);
		
            /*var debugStartPoint = transform.position + transform.right * turnRadius * steer;
		var debugEndPoint = debugStartPoint + Vector3.up * 5;
		
		Debug.DrawLine(debugStartPoint, debugEndPoint, Color.red);
		
		if(initialDragMultiplierX > dragMultiplier.X) // Handbrake
		{
			float rotationDirection  = Mathf.Sign(steer); // rotationDirection is -1 or 1 by default, depending on steering
			if(steer == 0)
			{
				if(rigidbody.angularVelocity.y < 1) // If we are not steering and we are handbraking and not rotating fast, we apply a random rotationDirection
					rotationDirection = Random.Range(-1.0f, 1.0f);
				else
					rotationDirection = rigidbody.angularVelocity.y; // If we are rotating fast we are applying that rotation to the car
			}
			// -- Finally we apply this rotation around a point between the cars front wheels.
			transform.RotateAround( transform.TransformPoint( (	frontwheels[0].transform.localPosition + frontwheels[1].transform.localPosition) * 0.5f), 
																transform.up, 
																rigidbody.velocity.magnitude * Mathf.Clamp01(1 - rigidbody.velocity.magnitude / topSpeed) * rotationDirection * Time.deltaTime * 2);
		}*/
        }
	

	
	
        bool HaveTheSameSign(float first,float second)
        {
            if (Mathf.Sign(first) == Mathf.Sign(second))
                return true;
            else
                return false;
        }
	
        float EvaluateSpeedToTurn(float speed)
        {
            if(speed > topSpeed / 2)
                return minimumTurn;
	
            float speedIndex = 1 - (speed / (topSpeed / 2));
            return minimumTurn + speedIndex * (maximumTurn - minimumTurn);
        }
	
        float Convert_Miles_Per_Hour_To_Meters_Per_Second(float speed)
        {
            return speed * 0.44704f;
        }
        float Convert_Meters_Per_Second_To_Miles_Per_Hour(float speed)
        {
            return speed * 2.23693629f;	
        }
        float EvaluateNormPower(float normPower)
        {
            if(normPower < 1)
                return 10 - normPower * 9;
            else
                return 1.9f - normPower * 0.9f;
        }
        /*float GetGearState()
	{
		Vector3 relativeVelocity = transform.InverseTransformDirection(rigidbody.velocity);
		float lowLimit = (currentGear == 0 ? 0 : gearSpeeds[currentGear-1]);
		return (relativeVelocity.z - lowLimit) / (gearSpeeds[currentGear - lowLimit]) * (1 - currentGear * 0.1f) + currentGear * 0.1f;
	}*/
    }
}
