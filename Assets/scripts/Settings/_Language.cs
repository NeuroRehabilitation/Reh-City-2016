using System.IO;
using System.Xml;
using UnityEngine;

////this script is attached to GameSettings in MainMenu scene

namespace Assets.scripts.Settings
{
    public class _Language : MonoBehaviour {
	
        string lang = "";
	
        //main menu
        public static string play;
        public static string settings;
        public static string quit;
        //misc
        public static string loading;
	
        void Start () 
        {
            lang = GeneralSettings.Language;
            LoadFromXml();
        }
	
        public void LoadFromXml()
        {
            string filepath = Application.streamingAssetsPath + @"/Language_Files/" + lang+".xml";
/*
#if UNITY_STANDALONE_OSX
            filepath = Application.dataPath + @"/Raw/Language_Files/" + lang + ".xml";
#endif
*/
            XmlDocument xmlDoc = new XmlDocument();
	  
            if(File.Exists (filepath))
            {
                xmlDoc.Load(filepath);
		    
                XmlNodeList menulist = xmlDoc.GetElementsByTagName("MainMenu");
                XmlNodeList misclist = xmlDoc.GetElementsByTagName("Misc");
			
                foreach (XmlNode wordsInfo in menulist)
                {
                    XmlNodeList xmlcontent = wordsInfo.ChildNodes;
			    
                    foreach (XmlNode words in xmlcontent)
                    {
                        if(words.Name == "Play")
                        {
                            play = words.InnerText; 
                        }
                        if(words.Name == "Settings")
                        {
                            settings = words.InnerText; 
                        }
                        if(words.Name == "Quit")
                        {
                            quit = words.InnerText; 
                        }
                        if(words.Name == "Loading")
                        {
                            loading = words.InnerText; 
                        }						
                    }
                }//menu list
			
                foreach (XmlNode wordsInfo in misclist)
                {
                    XmlNodeList xmlcontent = wordsInfo.ChildNodes;
			    
                    foreach (XmlNode words in xmlcontent)
                    {
                        if(words.Name == "Loading")
                        {
                            loading = words.InnerText; 
                        }						
                    }
                }//misc list
            }
        }//load from xml
				
    }
}
