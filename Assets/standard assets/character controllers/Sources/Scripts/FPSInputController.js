private var 	motor : 	CharacterMotor;
public  var 	canMove : 	boolean;
private var 	axis:		float;

private var 	motormove:	CharacterMotorMovement;
private var 	elapsedTime:float;
public  var 	maximumforwardspeed:float;
public var 		minimumforwardspeed:float;
public var 		holdtime:float;
public var		SpeedChangeDelta:float;
public var      pausegame:boolean;
public var      IsCalibrationRequired:boolean;
public var      IsAndroid : boolean;

public var      VerticalAxis : float;

private var      exponLength : UnityEngine.UI.Toggle;
private var      logLength : UnityEngine.UI.Toggle;
private var      propLength : UnityEngine.UI.Toggle;
private var      dirVectorText : UnityEngine.UI.Text;

//This script is attached to SimpleController under NewPlayer in NewCity scene

// Use this for initialization
function Awake () {
    canMove = true;
	motor = GetComponent(CharacterMotor);
	motormove = motor.movement;
	//Debug.Log(motormove.maxForwardSpeed);
} 

function Start() {
    exponLength = GameObject.Find("ExpToggle").GetComponent(UnityEngine.UI.Toggle) as UnityEngine.UI.Toggle;
    logLength = GameObject.Find("LogToggle").GetComponent(UnityEngine.UI.Toggle) as UnityEngine.UI.Toggle;
    propLength = GameObject.Find("PropToggle").GetComponent(UnityEngine.UI.Toggle) as UnityEngine.UI.Toggle;
    dirVectorText = GameObject.Find("DirVectorText").GetComponent(UnityEngine.UI.Text) as UnityEngine.UI.Text;
}

// Update is called once per frame
function Update () {
	// Get the input vector from kayboard or analog stick
    //var directionVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    

	if(canMove){
	   if(IsCalibrationRequired||IsAndroid)
	   {
	       axis = VerticalAxis;
	       Debug.Log("axis IsCalibrationRequired: " + axis);
	   }
	   else
 	   {
	       axis = Input.GetAxisRaw("Vertical");
	       //Debug.Log("axis !IsCalibrationRequired: " + axis);
	   }
	    if(axis <= 2f)var directionVector = new Vector3(0, 0, axis);
	    if(axis<2 && axis > 0){
	   	     elapsedTime += Time.deltaTime;
	   		 if(!GetComponent.<AudioSource>().isPlaying && !pausegame)
	   		 {
	   		     GetComponent.<AudioSource>().Play();
	   		 } 
	    }
	    else
	    {
	    	 elapsedTime=0;
	    	 GetComponent.<AudioSource>().Stop();
	    }
	    if(elapsedTime>=holdtime){
	    	motormove.maxForwardSpeed = Lerp(motormove.maxForwardSpeed,maximumforwardspeed,SpeedChangeDelta);
	    }
        else{
	    	motormove.maxForwardSpeed = minimumforwardspeed;
	    	}
	}
	//print("raw directionVector: " + directionVector);
	if (directionVector != Vector3.zero) {
		// Get the length of the directon vector and then normalize it
		// Dividing by the length is cheaper than normalizing when we already have the length anyway
		var directionLength = directionVector.magnitude;
		//print("directionLength magnitude: " + directionLength);
		directionVector = directionVector / directionLength;
		//print("raw directionVector divided by length(or normalized): " + directionVector);
		// Make sure the length is no bigger than 1
		directionLength = Mathf.Min(1, directionLength);
		//print("directionLength after forced to max 1: " + directionLength);
		// Make the input vector more sensitive towards the extremes and less sensitive in the middle
	    // This makes it easier to control slow speeds when using analog sticks
		if (exponLength.isOn) {
		    directionLength = directionLength * directionLength;
		}
        else if (logLength.isOn) {
		    directionLength = -Mathf.Log(directionLength);
		}
		
		// Multiply the normalized direction vector by the modified length
		directionVector = directionVector * directionLength;
	    var dv: String = directionVector.toLocaleString();
	    dirVectorText.text = dv;
		//print(directionVector);

	}
	// Apply the direction to the CharacterMotor
	motor.inputMoveDirection = transform.rotation * directionVector;
	//motor.inputJump = Input.GetButton("Jump");
}
function Lerp( start:float,  end:float,  duration:float)
{
	if(start >= end) return end;
	
	return start + Mathf.Sign(end-start) * duration;
}

// Require a character controller to be attached to the same game object
@script RequireComponent (CharacterMotor)
@script AddComponentMenu ("Character/FPS Input Controller")