using System.IO;
using System.Xml;
using UnityEngine;

namespace Assets.scripts.Settings
{
    public class Language : MonoBehaviour {

        public static string langRT = "PT";
        string tempLang;

        //main menu
        public static new string name;
        public static string hand, play, categories, instructions, levels, quit, welcome, handWarning, nameWarning, loading, options;
        public static string practice, memory, calibration, emotional, task, attention, selection, info, controls;
        public static string vSpatial, naming, abstraction, delRecall, orient, execFunc, idiom, edu, moca, models;

        //misc
        public static string main, pressKey, sessionEnd, score, playAgain, yes, no, previous, next, finish;

        //main GUI
        public static string timer, remaining, pause, unpause;
    
        //Level change GUI
        public static string level, completed, points, congrats, accumulated, allLevels;
        public static string cont, session, rate, left, right, getReady;

        //calibration GUI
        public static string network, filtering, devices, port, start, stop, cal, local, scale, filtered, copy, close, calibDone;
        public static string id, difficulty, lang, support, signs, labels, minimap, arrow, objective;

        void Start()
        {
            LoadFromXml();
            tempLang = langRT;
        }

        void Update()
        {
            
            if (tempLang != langRT)
            {
                LoadFromXml();
                tempLang = langRT;
            }
        }

        public void ResetLanguage(int reset)
        {
            if (reset == 0)
                langRT = "EN";
            else
                langRT = "PT";

            LoadFromXml();
        }

        public void LoadFromXml()
        {
            string filepath = Application.streamingAssetsPath + @"/Language_Files/" + langRT + ".xml";
/*
#if UNITY_STANDALONE_OSX
            filepath = Application.dataPath + @"/Raw/Language_Files/" + langRT + ".xml";
#endif
*/
            XmlDocument xmlDoc = new XmlDocument();

            if (File.Exists(filepath))
            {
            
                xmlDoc.Load(filepath);

                XmlNodeList menulist = xmlDoc.GetElementsByTagName("MainMenu");
                XmlNodeList misclist = xmlDoc.GetElementsByTagName("Misc");
                XmlNodeList mainGUI = xmlDoc.GetElementsByTagName("MainGUI");
                XmlNodeList levelChange = xmlDoc.GetElementsByTagName("LevelChange");
                XmlNodeList calibr = xmlDoc.GetElementsByTagName("Calibration");

                //main menu
                foreach (XmlNode wordsInfo in menulist)
                {
                    //Debug.Log("menu found");
                    XmlNodeList xmlcontent = wordsInfo.ChildNodes;

                    foreach (XmlNode words in xmlcontent)
                    {
                        if (words.Name == "Name")
                            name = words.InnerText;
                        if (words.Name == "Hand")
                            hand = words.InnerText;
                        if (words.Name == "Play")
                            play = words.InnerText;
                        if (words.Name == "Categories")
                            categories = words.InnerText;
                        if (words.Name == "Instructions")
                            instructions = words.InnerText;
                        if (words.Name == "Levels")
                            levels = words.InnerText;
                        if (words.Name == "Quit")
                            quit = words.InnerText;
                        if (words.Name == "Welcome")
                            welcome = words.InnerText;
                        if (words.Name == "HandWarning")
                            handWarning = words.InnerText;
                        if (words.Name == "NameWarning")
                            nameWarning = words.InnerText;
                        if (words.Name == "Practice")
                            practice = words.InnerText;
                        if (words.Name == "Memory")
                            memory = words.InnerText;
                        if (words.Name == "Calibration")
                            calibration = words.InnerText;
                        if (words.Name == "Emotional")
                            emotional = words.InnerText;
                        if (words.Name == "Task")
                            task = words.InnerText;
                        if (words.Name == "Attention")
                            attention = words.InnerText;
                        if (words.Name == "Selection")
                            selection = words.InnerText;
                        if (words.Name == "Info")
                            info = words.InnerText;
                        if (words.Name == "Loading")
                            loading = words.InnerText;
                        if (words.Name == "Options")
                            options = words.InnerText;
                        if (words.Name == "Controls")
                            controls = words.InnerText;
                        if (words.Name == "Id")
                            id = words.InnerText;
                        if (words.Name == "Difficulty")
                            difficulty = words.InnerText;
                        if (words.Name == "Language")
                            lang = words.InnerText;
                        if (words.Name == "Support")
                            support = words.InnerText;
                        if (words.Name == "Signs")
                            signs = words.InnerText;
                        if (words.Name == "Labels")
                            labels = words.InnerText;
                        if (words.Name == "Minimap")
                            minimap = words.InnerText;
                        if (words.Name == "Arrow")
                            arrow = words.InnerText;
                        if (words.Name == "Objective")
                            objective = words.InnerText;
                        if (words.Name == "Naming")
                            naming = words.InnerText;
                        if (words.Name == "Abstraction")
                            abstraction = words.InnerText;
                        if (words.Name == "VSpatial")
                            vSpatial = words.InnerText;
                        if (words.Name == "DRecall")
                            delRecall = words.InnerText;
                        if (words.Name == "Orientation")
                            orient = words.InnerText;
                        if (words.Name == "EFunctions")
                            execFunc = words.InnerText;
                        if (words.Name == "Idiom")
                            idiom = words.InnerText;
                        if (words.Name == "Edu")
                            edu = words.InnerText;
                        if (words.Name == "Moca")
                            moca = words.InnerText;
                        if (words.Name == "Models")
                            models = words.InnerText;
                    }
                }//main menu

                //misc
                foreach (XmlNode wordsInfo in misclist)
                {
                    XmlNodeList xmlcontent = wordsInfo.ChildNodes;

                    foreach (XmlNode words in xmlcontent)
                    {
                        if (words.Name == "MainMenu")
                            main = words.InnerText;
                        if (words.Name == "pressKey")
                            pressKey = words.InnerText;
                        if (words.Name == "SessionEnd")
                            sessionEnd = words.InnerText;
                        if (words.Name == "Score")
                            score = words.InnerText;
                        if (words.Name == "PlayAgain")
                            playAgain = words.InnerText;
                        if (words.Name == "Yes")
                            yes = words.InnerText;
                        if (words.Name == "No")
                            no = words.InnerText;
                        if (words.Name == "Previous")
                            previous = words.InnerText;
                        if (words.Name == "Next")
                            next = words.InnerText;
                        if (words.Name == "Finish")
                            finish = words.InnerText;
                    }
                }//misc

                //main GUI
                foreach (XmlNode wordsInfo in mainGUI)
                {
                    XmlNodeList xmlcontent = wordsInfo.ChildNodes;

                    foreach (XmlNode words in xmlcontent)
                    {
                        if (words.Name == "Timer")
                            timer = words.InnerText;
                        if (words.Name == "Remaining")
                            remaining = words.InnerText;
                        if (words.Name == "Pause")
                            pause = words.InnerText;
                        if (words.Name == "Unpause")
                            unpause = words.InnerText;
                    }
                }//main GUI

                //Level change GUI
                foreach (XmlNode wordsInfo in levelChange)
                {
                    XmlNodeList xmlcontent = wordsInfo.ChildNodes;

                    foreach (XmlNode words in xmlcontent)
                    {
                        if (words.Name == "Level")
                            level = words.InnerText;
                        if (words.Name == "Completed")
                            completed = words.InnerText;
                        if (words.Name == "Points")
                            points = words.InnerText;
                        if (words.Name == "Congrats")
                            congrats = words.InnerText;
                        if (words.Name == "Accumulated")
                            accumulated = words.InnerText;
                        if (words.Name == "AllLevels")
                            allLevels = words.InnerText;
                        if (words.Name == "Continue")
                            cont = words.InnerText;
                        if (words.Name == "Session")
                            session = words.InnerText;
                        if (words.Name == "Rate")
                            rate = words.InnerText;
                        if (words.Name == "Left")
                            left = words.InnerText;
                        if (words.Name == "Right")
                            right = words.InnerText;
                        if (words.Name == "GetReady")
                            getReady = words.InnerText;
                    }
                }

                foreach (XmlNode wordsInfo in calibr)
                {
                    XmlNodeList xmlcontent = wordsInfo.ChildNodes;

                    foreach (XmlNode words in xmlcontent)
                    {
                        if (words.Name == "Network")
                            network = words.InnerText;
                        if (words.Name == "Filtering")
                            filtering = words.InnerText;
                        if (words.Name == "Devices")
                            devices = words.InnerText;
                        if (words.Name == "Port")
                            port = words.InnerText;
                        if (words.Name == "Start")
                            start = words.InnerText;
                        if (words.Name == "Stop")
                            stop = words.InnerText;
                        if (words.Name == "Cal")
                            cal = words.InnerText;
                        if (words.Name == "Local")
                            local = words.InnerText;
                        if (words.Name == "Scale")
                            scale = words.InnerText;
                        if (words.Name == "Filtered")
                            filtered = words.InnerText;
                        if (words.Name == "Copy")
                            copy = words.InnerText;
                        if (words.Name == "Close")
                            close = words.InnerText;
                        if (words.Name == "CalibDone")
                            calibDone = words.InnerText;
                    }
                }
            }
        }//load from xml
    }
}

