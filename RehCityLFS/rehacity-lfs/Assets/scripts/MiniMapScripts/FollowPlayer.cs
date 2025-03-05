using Assets.scripts.GUI;
using Assets.scripts.objectives;
using UnityEngine;

/* This script is attached to MiniMapCamera in City scene
 * Camera is placed at certain Height from the player.
 * Camera rotates based on players Horizontal rotation.
 * Coder : Kushal
*/

namespace Assets.scripts.MiniMapScripts
{
    public class FollowPlayer : MonoBehaviour {
	
        public Transform player;
        public float Height = 10;

        private DrawObjectiveList _drawobjectivelist;
        private ObjectiveManager _objmanager;

        public int MaxMapLength = 508;
        public int MaxCameraRange = 500;

        private void Start()
        {
            if(player==null){
                Debug.LogWarning("Attach the player transform to minimap camera");
            }
            _drawobjectivelist = DrawObjectiveList.Instance;
            _objmanager = ObjectiveManager.Instance;
        }
	
        void Update () 
        {
            //if minimap camera is maximized then show the entire Path from player to destination.
            if (_objmanager.GetCurrentObjective != null && !_drawobjectivelist.MinimizeObjective)
            {
                var dist = (player.transform.position - _objmanager.GetCurrentObjective.Location).magnitude;
            
                float range = (MaxCameraRange * (int)dist) / MaxMapLength;
                //Debug.Log(range);
                if (range < 20)
                    range = range * 20f;

                else if (range < 140 && range >= 20)
                    range = range * 1.5f;

                else if (range < 150 && range >= 140)
                    range = range * 1.1f;

                else
                    range = range * 0.9f;

                //Debug.Log("Updated Range: " + range);
                // we are disabling boxcollider to stop the Objective from floating on the minimap borders
                //gameObject.GetComponent<BoxCollider>().enabled = false;
                gameObject.GetComponent<UnityEngine.Camera>().orthographicSize = range;//180;//range>20?range:20;
                // change position to midpoint (b/w player and Objective)

                float posZ = 0;
                float posX = 0;
            
                var actualZ = ((player.transform.position + _objmanager.GetCurrentObjective.Location) * 0.5f).z;
                var actualX = ((player.transform.position + _objmanager.GetCurrentObjective.Location) * 0.5f).x;
                //Debug.Log("actaulZ: " + actualZ + " actualX: " + actualX);
                if (actualX < -100f)
                    posX = 25;
                else if (actualX > 100f)
                    posX = -25;
                
                if (actualZ < -45f)
                    posZ = 55;
                else if(actualZ > 45)
                    posZ = -55;

                if (actualZ < -65f || actualZ > 65)
                    posZ = 0;
                

                transform.position = (player.transform.position + _objmanager.GetCurrentObjective.Location) * 0.5f + new Vector3(posX,Height,posZ);

                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, player.localEulerAngles.y, 0);  
            }
            else
            {
                gameObject.GetComponent<UnityEngine.Camera>().orthographicSize = 60;
                // Camera is placed at a height from the player.
                transform.position = player.transform.position + new Vector3(0, Height, 0);
                // Camera rotates based on players horizontal rotation
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, player.localEulerAngles.y, 0);
            }
        }
    }
}
