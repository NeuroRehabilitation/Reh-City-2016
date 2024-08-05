using Assets.scripts.Controller;
using Assets.scripts.RehaTask.RehaTaskGUI;
using UnityEngine;

namespace Assets.scripts.RehaTask.RehaTask_GameLogic
{
    public class TimerCount : MonoBehaviour {
	
        public static float MyTimer;
        public static float StartTime;//number of Seconds Timer will be displayed
	
        public static bool Timeout, ControllerB1 = false;

        public GameObject timer;

        private void Start () 
        {
            Timeout = false;
            //StartTime = 6f;//comment this when reading from xml
            MyTimer = StartTime;
        }

        private void Update () 
        {

            MyTimer -= Time.deltaTime;

            if (MyTimer <= 0 && MyTimer > -1)
            {
                Timeout = true;
                MyTimer = -1;
                timer.SetActive(false);
            }
            else
            {
                Timeout = false;
            }
       
            GetComponent<Renderer>().material.SetFloat("_Cutoff", Mathf.InverseLerp(0, StartTime, MyTimer));//set float to "cut" texture - from grayscale channel

        }	
    }
}
