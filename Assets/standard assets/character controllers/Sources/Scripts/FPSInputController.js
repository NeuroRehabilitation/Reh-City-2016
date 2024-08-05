import UnityEngine.UI;

private var 	motor : 	CharacterMotor;
public  var 	canMove : 	boolean;
private var 	axis:		float;

private var 	motormove:	CharacterMotorMovement;
private var 	elapsedTime:float;
public  var 	maximumforwardspeed:float;
public static var 		minimumforwardspeed:float = 4;
public static var 		threshold:float = 0;
public static var 		canRotateAndMove:boolean;
public static var 		isInsideThresholds:boolean;
public var 		holdtime:float;
public var		SpeedChangeDelta:float;
public var      pausegame:boolean;
public var      IsCalibrationRequired:boolean;
public var      IsAndroid : boolean;

public var      VerticalAxis : float;

public var      exponLength : UI.Toggle;
public var      logLength : UI.Toggle;
public var      propLength : UI.Toggle;
public var      dirVectorText : UI.Text;
public var      axisText : UI.Text;
private var      dv : String;
private var      ax : String;
public var       minSpeed : UnityEngine.UI.Slider;



//This script is attached to SimpleController under NewPlayer in NewCity scene

// Use this for initialization
function Awake () {
    canMove = true;
	motor = GetComponent(CharacterMotor);
	motormove = motor.movement;
    //Debug.Log(motormove.maxForwardSpeed);
} 

function Start() {
    minSpeed.value = minimumforwardspeed;
}

// Update is called once per frame
function Update () {

    
    
    // Get the input vector from kayboard or analog stick
    //var directionVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    //sp = minimumforwardspeed.toString();
    
	if(canMove){
	   if(IsCalibrationRequired||IsAndroid)
	   {
	       axis = VerticalAxis;
	   }
	   else
 	   {
	       axis = Input.GetAxisRaw("Vertical");
	   }
	   if (threshold < 0 && axis > threshold)
	       axis = axis + (-threshold);
	   
	    if(axis <= 2f)var directionVector = new Vector3(0, 0, axis);
	    if((axis<2 && axis > threshold && canRotateAndMove) || (!canRotateAndMove && isInsideThresholds && axis<2 && axis > threshold)){
	        motor.enabled = true;
	        minimumforwardspeed = minSpeed.value;
	        maximumforwardspeed = minimumforwardspeed + 4;
	   	     elapsedTime += Time.deltaTime;
	   		 if(!audio.isPlaying && !pausegame)
	   		 {
	   		     audio.Play();
	   		 } 
	    }
	    else
	    {
	        motor.enabled = false;
	        maximumforwardspeed = 0;
	    	elapsedTime=0;
	    	audio.Stop();
	    }
	    if(elapsedTime>=holdtime) {
	        
	        motormove.maxForwardSpeed = Lerp(motormove.maxForwardSpeed,maximumforwardspeed,SpeedChangeDelta);
	    }
	    else{
	        
	        motormove.maxForwardSpeed = minimumforwardspeed;
	    	}
	}
	ax = "axis: " + axis.ToString();
    axisText.text = ax;
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
		    directionLength = 1 +Mathf.Log(directionLength);
		}
	    
		// Multiply the normalized direction vector by the modified length
		directionVector = directionVector * directionLength ;
	    dv = "direction vector: " + directionVector.ToString();
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