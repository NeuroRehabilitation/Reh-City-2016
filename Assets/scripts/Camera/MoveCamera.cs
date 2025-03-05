using Assets.scripts.RehaTask.RehaTask_GameLogic;
using Assets.scripts.UDP;
using UnityEngine;

namespace Assets.scripts.Camera
{
    public class MoveCamera : MonoBehaviour {
	
        public static bool AnimStatus;
        public GameObject dana;


        private void Start()
        {
            AnimStatus = false;
        }

        private void Update () 
        {
            if(AnimStatus)
            {
                SpawnTiles.ReadyToSpawn = false;
                dana.SetActive(false);
            
                animation.wrapMode = WrapMode.Once;
                animation.Play();
                AnimStatus = false;
                CollisionDetection.ActivateTimer = false;        
            }

            else
            {
                if (!animation.isPlaying)
                {
                    dana.SetActive(true);
                
                    if (!UdpReceive.calibrate)
                    {
                        CollisionDetection.ActivateTimer = true;
                    }

                    SpawnTiles.ReadyToSpawn = true;
                }
            }   
        }
    }
}
