using UnityEngine;

/* Attach this class to the current Objective .
 * If the Objective is out of minimap range then it's icon will
 * float on borders of the minimap.
 * Coder: Kushal
 */

//This script is attached to ObjectiveIndicator in City scene

namespace Assets.scripts.MiniMapScripts
{
    public class ObjectiveIndicator : MonoBehaviour {
	
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
	
        public Vector3 GetContactPoint
        {
            get
            {
                return _hit.point;
            }
        }
	
        [HideInInspector]
        public bool floating;

        private static ObjectiveIndicator _sInstance;
        public static ObjectiveIndicator Instance
        {
            get
            {
                if (_sInstance == null)
                {
                    _sInstance = FindObjectOfType(typeof(ObjectiveIndicator)) as ObjectiveIndicator;
                    if (_sInstance == null)
                    {
                        Debug.Log("Could not locate ObjectiveIndicator");
                    }
                }
                return _sInstance;
            }
        }

        private void Start ()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void Raycast()
        {
            // throw the ray not more than the distance between player and Objective.
            var distance = Vector3.Distance(player.transform.position, CurrentObjPosition);
            //direction in which the ray has to be projected.
            var direction = player.transform.position - CurrentObjPosition;

            Debug.DrawRay(CurrentObjPosition, direction, Color.red);

            //Raycast from CurrentObjectivePosition to the player ignoring all other layers
            if (Physics.Raycast(CurrentObjPosition, direction, out _hit, distance, minimapplayer))
            {
                //checking the Raycast collision with the MinimapBorder 
                // the collider is attached to the MinimapCamera.
                if (_hit.collider.tag == "MapBorder")
                {
                    // change the position of Objective icon to the colliding point on minimap.
                    transform.position = new Vector3(_hit.point.x, 7, _hit.point.z);
                    floating = true;
                }
            }
            else
            {
                if (transform.position != CurrentObjPosition) transform.position = CurrentObjPosition;
                floating = false;
            }
        }

        private void FixedUpdate()
        {
            Raycast();
        }
    }
}
