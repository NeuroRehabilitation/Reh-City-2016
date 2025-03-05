using UnityEngine;
using System.Collections;
/* Attach this class to the current Objective .
 * If the Objective is out of minimap range then it's icon will
 * float on borders of the minimap.
 * Coder: Kushal
 */

public class MapObjectiveIndicator : MonoBehaviour {
	
	public Transform player;
	// cast the ray only to the minimap border ignoring all other layers
	public LayerMask minimapplayer;
	// stucture that holds all the info about where the Raycast hits
	private RaycastHit _hit;
	
	private Vector3 _objPos;
	public Vector3 CurrentObjPosition
	{
		get{
			return _objPos;
		}
		set{
			_objPos = value;
		}
	}
	
	private void Start ()
    {
		_objPos = new Vector3(-74,65,-4);
		transform.localPosition = _objPos;
	}
	
	private void Raycast()
	{
		// throw the ray not more than the distance between player and Objective.
		var distance = Vector3.Distance(player.transform.position,CurrentObjPosition); 
		//direction in which the ray has to be projected.
		var direction = player.transform.position-CurrentObjPosition;
	
		//Raycast from CurrentObjectivePosition to the player ignoring all other layers
		if(Physics.Raycast(CurrentObjPosition,direction,out _hit,distance,minimapplayer))
		{
			//checking the Raycast collision with the MinimapBorder 
			// the collider is attached to the MinimapCamera.
			if(_hit.collider.tag == "MapBorder")
			{
				// change the position of Objective icon to the colliding point on minimap.
				transform.position = _hit.point;
			}else
			{
				transform.localPosition = CurrentObjPosition;
			}
		}
	}
	private void FixedUpdate()
	{
		Raycast();
	}
}
