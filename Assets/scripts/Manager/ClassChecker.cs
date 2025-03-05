using UnityEngine;

//This script is attached to SimpleController under NewPlayer in City scene

namespace Assets.scripts.Manager
{
    public class ClassChecker : MonoBehaviour {

        public GameObject Manager;
        public GameObject Timer;
        public GameObject Points;
        public GameObject InterfacePrefab, controllerPrefab;
        private float _elapsedTime;

        private void Awake ()
        {
        
            if (GameObject.FindGameObjectWithTag("Manager") == null)
            {
                Instantiate(Manager, Vector3.zero, Quaternion.identity);
            }

            if (GameObject.FindGameObjectWithTag("Timer") == null)
            {
                Instantiate(Timer, new Vector3(0.65f, 1,0), Quaternion.identity); 
            }
            /*
            if (GameObject.FindGameObjectWithTag("Score") == null)
            {
                Instantiate(Points, new Vector3(0.36f, 1, 0), Quaternion.identity);
            }
            */
            if (GameObject.FindGameObjectWithTag("InterfacePrefab") == null)
            {
                Instantiate(InterfacePrefab, Vector3.zero, Quaternion.identity);
            }

            if (GameObject.FindGameObjectWithTag("hand") == null)
            {
                Instantiate(controllerPrefab, new Vector3(-92, 1, 0), Quaternion.identity);
            }
        }
    
        private void WriteData()
        {/*
            if (DataCollector.Instance == null||GameTimer.Instance == null) return;
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime >= 0.2f)
            {
                if(DataCollector.Instance != null)DataCollector.Instance.WritePlayerData(GameTimer.Instance.Timetext,transform.position, transform.localEulerAngles);
                _elapsedTime = 0;
            }*/
        }

        private void Update()
        {
            WriteData();
        }

    }
}
