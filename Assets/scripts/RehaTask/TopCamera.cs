using UnityEngine;

namespace Assets.scripts.RehaTask
{
    public class TopCamera : MonoBehaviour {
	
        public UnityEngine.Camera top;
        public GameObject gamePlayUI;

        private void Awake () 
        {
            top.enabled=false;
        }
	
	
        private void Update () 
        {
            //shortcut key to Enable/disable top camera
            if(Input.GetKeyDown(KeyCode.F1) && !top.enabled && gamePlayUI.activeSelf)
            {
                top.enabled=true;
            }
            else if (Input.GetKeyDown(KeyCode.F1) && top.enabled && gamePlayUI.activeSelf)
            {
                top.enabled=false;
            }
        }
    }
}
