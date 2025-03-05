using System.Collections.Generic;
using System.IO;
using System.Xml;
using Assets.scripts.GUI;
using Assets.scripts.RehaTask.RehaTaskGUI;
using Assets.scripts.RehaTask.RehaTask_GameLogic;
using Assets.scripts.Settings;
using Assets.scripts.UDP;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.scripts.objectives.Action
{
    public class LoadQuestions : MonoBehaviour
    {
        public static List<List<string>> ImageQuestions = new List<List<string>>();
        public static List<List<string>> ShortTextQuestions = new List<List<string>>();
        public static List<List<string>> MediumTextQuestions = new List<List<string>>();
        public static List<List<string>> LongTextQuestions = new List<List<string>>();

        public static List<List<string>> ImageSubjects = new List<List<string>>();
        public static List<List<string>> ShortTextSubjects = new List<List<string>>();
        public static List<List<string>> MediumTextSubjects = new List<List<string>>();
        public static List<List<string>> LongTextSubjects = new List<List<string>>();

        public static float QuestionsDisplayTimer;
        public static bool ActivateQuestionsTimer;

        public static bool Answer, QuestionsCompleted, DisplayingQuestions;
        private GameObject _mger;
        private ObjectiveManager _objManager;
        public static string CurrentScene;
        
        private void Start ()
        {
            
            _mger = GameObject.FindGameObjectWithTag("Manager");
            _objManager = _mger.GetComponent<ObjectiveManager>();
            LoadImageQuestionsList();
            LoadShortTextQuestionsList();
            LoadMediumTextQuestionsList();
            LoadLongTextQuestionsList();
        }
        
        private void Update()
        {
            if (_objManager.GetCurrentObjective != null && _objManager.GetCurrentObjective.ToString().Contains("QuestionsDisplay") && DrawObjectiveList.Minimized)
            {
                if (Application.loadedLevelName != "Questions")
                {
                    CurrentScene = Application.loadedLevelName;
                    Application.LoadLevel("Questions");
                    
                }
            }
        }

        private static void LoadImageQuestionsList()
        {
            var filepath = Application.streamingAssetsPath + @"/Questions/ImageQuestions.xml";
/*
#if UNITY_STANDALONE_OSX
            filepath = Application.dataPath + @"/Raw/Questions/ImageQuestions.xml";
#endif
*/
            var xmlDoc = new XmlDocument();
            var questionText = "question" + Language.langRT;

            if (File.Exists(filepath))
            {
                xmlDoc.Load(filepath);
                var questionsList = xmlDoc.GetElementsByTagName("Questions");
                
                foreach (XmlNode qGroup in questionsList)
                {
                    var groupContent = qGroup.ChildNodes;
                
                    foreach (XmlNode question in groupContent)
                    {
                        var imgQuestions = new List<string>();
                        var imgSubj = new List<string>();
                        foreach (XmlNode quest in question)
                        {
                            foreach (XmlNode line in quest)
                            {
                                if (line.Name == questionText)
                                    imgQuestions.Add(line.InnerText);
                                if (line.Name == "questionSubject")
                                    imgSubj.Add(line.InnerText);
                            }
                        }
                        ImageQuestions.Add(imgQuestions);
                        ImageSubjects.Add(imgSubj);
                    }
                }
            }
        }

        private static void LoadShortTextQuestionsList()
        {
            var filepath = Application.streamingAssetsPath + @"/Questions/ShortQuestions.xml";
            /*
            #if UNITY_STANDALONE_OSX
                        filepath = Application.dataPath + @"/Raw/Questions/ShortQuestions.xml";
            #endif
            */
            var xmlDoc = new XmlDocument();
            var questionText = "question" + Language.langRT;

            if (File.Exists(filepath))
            {
                xmlDoc.Load(filepath);
                var questionsList = xmlDoc.GetElementsByTagName("Questions");
                
                foreach (XmlNode qGroup in questionsList)
                {
                    var groupContent = qGroup.ChildNodes;

                    foreach (XmlNode question in groupContent)
                    {
                        var shrtQuestions = new List<string>();
                        var shrtSubj = new List<string>();
                        foreach (XmlNode quest in question)
                        {
                            foreach (XmlNode line in quest)
                            {
                                if (line.Name == questionText)
                                    shrtQuestions.Add(line.InnerText);
                                if (line.Name == "questionSubject")
                                    shrtSubj.Add(line.InnerText);
                            }
                        }
                        ShortTextQuestions.Add(shrtQuestions);
                        ShortTextSubjects.Add(shrtSubj);
                    }
                }
            }
        }

        private static void LoadMediumTextQuestionsList()
        {
            var filepath = Application.streamingAssetsPath + @"/Questions/MediumQuestions.xml";
            /*
            #if UNITY_STANDALONE_OSX
                        filepath = Application.dataPath + @"/Raw/Questions/MediumQuestions.xml";
            #endif
            */
            var xmlDoc = new XmlDocument();
            var questionText = "question" + Language.langRT;

            if (File.Exists(filepath))
            {
                xmlDoc.Load(filepath);
                var questionsList = xmlDoc.GetElementsByTagName("Questions");
                
                foreach (XmlNode qGroup in questionsList)
                {
                    var groupContent = qGroup.ChildNodes;

                    foreach (XmlNode question in groupContent)
                    {
                        var mdmQuestions = new List<string>();
                        var mdmSubj = new List<string>();
                        foreach (XmlNode quest in question)
                        {
                            foreach (XmlNode line in quest)
                            {
                                if (line.Name == questionText)
                                    mdmQuestions.Add(line.InnerText);
                                if (line.Name == "questionSubject")
                                    mdmSubj.Add(line.InnerText);
                            }
                        }
                        MediumTextQuestions.Add(mdmQuestions);
                        MediumTextSubjects.Add(mdmSubj);
                    }
                }
            }
        }

        private static void LoadLongTextQuestionsList()
        {
            var filepath = Application.streamingAssetsPath + @"/Questions/LongQuestions.xml";
            /*
            #if UNITY_STANDALONE_OSX
                        filepath = Application.dataPath + @"/Raw/Questions/LongQuestions.xml";
            #endif
            */
            var xmlDoc = new XmlDocument();
            var questionText = "question" + Language.langRT;

            if (File.Exists(filepath))
            {
                xmlDoc.Load(filepath);
                var questionsList = xmlDoc.GetElementsByTagName("Questions");
                
                foreach (XmlNode qGroup in questionsList)
                {
                    var groupContent = qGroup.ChildNodes;

                    foreach (XmlNode question in groupContent)
                    {
                        var lngQuestions = new List<string>();
                        var lngSubj = new List<string>();
                        foreach (XmlNode quest in question)
                        {
                            foreach (XmlNode line in quest)
                            {
                                if (line.Name == questionText)
                                    lngQuestions.Add(line.InnerText);
                                if (line.Name == "questionSubject")
                                    lngSubj.Add(line.InnerText);
                            }
                        }
                        LongTextQuestions.Add(lngQuestions);
                        LongTextSubjects.Add(lngSubj);
                    }
                }
            } 
        }
    }
}
