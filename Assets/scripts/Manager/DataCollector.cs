using System.IO;
using UnityEngine;

namespace Assets.scripts.Manager
{
    public class DataCollector : MonoBehaviour {/*

        private string foldername;
        private string uniquefoldername;
        private string screenshotsfoldername;
   
        private StreamWriter settings;
        private StreamWriter playerdata;
        private StreamWriter GitterData;
        private StreamWriter FaceAPIData;

        private string timetext;
        private static DataCollector s_Instance = null;
        public static DataCollector Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindObjectOfType(typeof(DataCollector)) as DataCollector;
                    if (s_Instance == null)
                    {
                        //  Debug.Log("Could not locate DataCollector");
                    }
                }
                return s_Instance;
            }
        }
        bool iswebbuild = false;
        // Use this for initialization
        void Start()
        {

            if (iswebbuild) return;
            DontDestroyOnLoad(this.gameObject);
            if (!Directory.Exists("DataCollected"))
            {
                Directory.CreateDirectory("DataCollected");
            }
            foldername = Directory.GetCurrentDirectory()+"/DataCollected";
            uniquefoldername = foldername + "/" + System.DateTime.Now.ToShortDateString().Replace("/", "-") + " " + System.DateTime.Now.ToShortTimeString().Replace(":", "-") + "-" + System.DateTime.Now.TimeOfDay.Seconds.ToString();
            screenshotsfoldername = uniquefoldername + "/Shots";
            if (!Directory.Exists(uniquefoldername))
            {
                Directory.CreateDirectory(uniquefoldername);
                Directory.CreateDirectory(screenshotsfoldername);
            }
            settings = new StreamWriter(uniquefoldername + "/Settings.csv");
            playerdata = new StreamWriter(uniquefoldername + "/PlayerData.csv");
            GitterData = new StreamWriter(uniquefoldername + "/Gitter.csv");
            FaceAPIData = new StreamWriter(uniquefoldername + "/FaceAPI.txt");
            settings.WriteLine("Player ID  ," +"Level Started  ," + " ShowMapPath , " + "  Show Arrow  ," + "  ShowObjectiveList ");
            playerdata.WriteLine("Time   ," + " Position X ,"+" Position Y ,"+" Position Z ," + " Rotation X ,"+" Rotation Y ," +"Rotation Z");
            GitterData.WriteLine("Score ," + " Level ," + " Time ," + " CurrentObjective ," + " ItemName ," + " X ," + " Y ,");
            FaceAPIData.WriteLine("Time ," + " Logging ,");
        }

        void OnDisable()
        {
            if (playerdata == null) return;
            playerdata.Close();
            GitterData.Close();
            settings.Close();
            FaceAPIData.Close();
            settings.Dispose();
            playerdata.Dispose();
            GitterData.Dispose();
            FaceAPIData.Dispose();
        }

        // called from startscenesettings
        public void WriteSettings(string playerid,string Level,bool showMapPath,bool showArrow,bool showList)
        {
            string append = playerid+" , "+Level + " , " + showMapPath.ToString() + " , " + showArrow.ToString() + " , " + showList.ToString();
            settings.WriteLine(append);
        }

        // called from classchecker script
        public void WritePlayerData(string time, Vector3 pos, Vector3 rot)
        {
            string append ="  "+time+" , " + pos.x.ToString()+","+pos.y.ToString()+","+pos.z.ToString()+ " , " + rot.x.ToString()+","+rot.y.ToString()+","+rot.z.ToString();
            playerdata.WriteLine(append);
        }

        //called from selectobject script
        public void WriteGitter(int Score, int Level, string time, string objectivename, string pickeditem, float left,float top)
        {
            string append = " "+Score.ToString()+" , "+Level.ToString()+" , "+time.ToString()+" ,  "+objectivename+" , "+pickeditem+ ", "+left.ToString()+" , "+top.ToString();
            GitterData.WriteLine(append);
        }
        public void WriteFaceAPIData(string data)
        {
            FaceAPIData.WriteLine(timetext+" , "+data);
        }
        //called from select object script
        public void CaptureScreenShot(string levelname , string time)
        {
            Application.CaptureScreenshot(uniquefoldername + "/Shots/"+levelname.ToString()+"   "+time+".png");
        }
        void Update()
        {
            timetext = Time.time.ToString();
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                CaptureScreenShot(Application.loadedLevelName, timetext);
            }
        }*/
    }
}
