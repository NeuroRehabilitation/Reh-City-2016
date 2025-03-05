using UnityEngine;
using System.Collections;
using Assets.scripts.MiniMapScripts;
using Assets.scripts.MiniMapScripts.AStar;

/*Attach this class to arrow object
 *this makes the arrow point to the current Objective on minimap
*/

//This script is atached to ArrowObject in City scene

public class ArrowObject : MonoBehaviour {

    ObjectiveIndicator Objindicator;
	GameObject mesh;
	
	void Start()
	{
		Objindicator = GameObject.Find("ObjectiveIndicator").GetComponent<ObjectiveIndicator>();
		mesh = transform.FindChild("Group1").FindChild("Mesh1").gameObject;
	}
	
	void Update(){
		mesh.renderer.enabled = Objindicator.floating ? true:false;
		transform.position = new Vector3(Objindicator.transform.position.x ,7, Objindicator.transform.position.z);
        transform.LookAt(new Vector3(Objindicator.CurrentObjPosition.x,7,Objindicator.CurrentObjPosition.z));
        /*
        Vector3 relativePos = new Vector3();
        //relativePos = transform.position - Objindicator.transform.position;
        relativePos = transform.position - NodeManager.Instance.GetStartNode().position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        //Debug.Log(rotation);
	    transform.rotation = rotation;*/
	}
}
