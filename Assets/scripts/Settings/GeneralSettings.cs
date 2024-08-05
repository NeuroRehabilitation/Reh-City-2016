using System.IO;
using System.Xml;
using Assets.scripts.RehaTask.RehaTask_GameLogic;
using UnityEngine;

//this script is attached to GameSettings in MainMenu scene

namespace Assets.scripts.Settings
{
    public class GeneralSettings : MonoBehaviour {
	
//	public GameObject Player;
	
        public static string Language;
        public static int SessionTime = 30;
	
        public static bool useOculus = false;
	
        void Awake () 
        {
            LoadFromXml();
//		Player.SetActive(false);
        }

        // Use this for initialization
        void Start () 
        {
            //DontDestroyOnLoad(this.gameObject);
            //	DontDestroyOnLoad(Player);
		
        }
	
        // Update is called once per Frame
        void Update () 
        {
            if(Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
		
//		if(Application.loadedLevel == 2)//city
//		{
//			Player.SetActive(true);
//		}
//		if(Application.loadedLevel == 3)//supermarket
//		{
//			Player.SetActive(true);
//		}

        }
	
        public void LoadFromXml()
        {
            string filepath = Application.streamingAssetsPath + @"/Settings/_config.xml";
/*
#if UNITY_STANDALONE_OSX
            filepath = Application.dataPath + @"/Raw/Settings/_config.xml";
#endif
*/
            XmlDocument xmlDoc = new XmlDocument();
	  
            if(File.Exists (filepath))
            {
                xmlDoc.Load(filepath);
		    
                XmlNodeList menulist = xmlDoc.GetElementsByTagName("General");
			
                foreach (XmlNode wordsInfo in menulist)
                {
                    XmlNodeList xmlcontent = wordsInfo.ChildNodes;
			    
                    foreach (XmlNode words in xmlcontent)
                    {
                        if(words.Name == "Language")
                        {
                            Language = words.InnerText; 
                        }
                        if (words.Name == "Session")
                        {
                            SessionTime = int.Parse(words.InnerText);
                        }
                        if (words.Name == "RehaTaskExtraTime")
                        {
                            ObjectiveInstatiation.RehaTaskExtraTime = int.Parse(words.InnerText);
                        }
                        if (words.Name == "RehaTaskSelectionTime")
                        {
                            TimerCount.StartTime = int.Parse(words.InnerText);
                        }
                        if (words.Name == "RehaTaskTargetTime")
                        {
                            ObjectiveInstatiation.RehaTaskTargetTime = int.Parse(words.InnerText);
                        }


                    }
                }//menu list
			
			
            }
	
		
        }//load from xml
	
    }
}
