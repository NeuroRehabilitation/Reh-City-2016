using Assets.scripts.Camera;
using UnityEngine;

namespace Assets.scripts.Controller
{
    public class RenderHands : MonoBehaviour {
	
        public GameObject dana;
	
        private bool _activeHands;
	
        // Use this for initialization
        private void Start () 
        {
            dana.SetActive(false);
        }
	
        // Update is called once per Frame
        private void Update () 
        {
            if(MoveCamera.AnimStatus)
            {
                dana.SetActive(false);
            }
            else if(!MoveCamera.AnimStatus && !_activeHands)
            {
                dana.SetActive(true); 
                _activeHands =true;
            }
        }	
    }
}
