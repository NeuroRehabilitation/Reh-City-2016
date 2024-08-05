using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Assets.scripts.GUI;
using Assets.scripts.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.scripts.objectives.Action
{
    public class LoadTipsContent : MonoBehaviour
    {
        public static List<Sprite> ImagesTipsList = new List<Sprite>();
        public static List<string> ShortTextTipsList = new List<string>();
        public static List<string> MediumTextTipsList = new List<string>();
        public static List<string> LongTextTipsList = new List<string>();
        public static readonly List<string> ShortSubjectsList = new List<string>();
        public static readonly List<string> MediumSubjectsList = new List<string>();
        public static readonly List<string> LongSubjectsList = new List<string>();
        private GameObject _mger;
        private ObjectiveManager _objManager;
        public static bool TipsObj, TipCompleted;
        public static int TipsGroup;
        public static int TipType;

        private void Awake()
        {
            LoadTipsImages();
            LoadShortTipsText();
            LoadMediumTipsText();
            LoadLongTipsText();
        }

        private void Start()
        {
            _mger = GameObject.FindGameObjectWithTag("Manager");
            _objManager = _mger.GetComponent<ObjectiveManager>();   
        }

        private void Update()
        {
            if (_objManager.GetCurrentObjective != null && _objManager.GetCurrentObjective.ToString().Contains("TipDisplay") && DrawObjectiveList.Minimized && !TipCompleted && Application.loadedLevelName == "Kiosk")
            {
                TipsNavigation.ActualTip = _objManager.GetCurrentObjective.TipNumber;
                TipType = _objManager.GetCurrentObjective.TipType;
                TipsObj = true;
            }
            else
            {
                TipsObj = false;
            }
        }

        private static void LoadTipsImages()
        {
            var di = new DirectoryInfo(Application.streamingAssetsPath + @"/Tips/Img/");
/*
#if UNITY_STANDALONE_OSX
            di = new DirectoryInfo(Application.dataPath + @"/Raw/Tips/Img/");
#endif
*/
            var smFiles = di.GetFiles();
            var i = 0;

            foreach (FileInfo fi in smFiles)
            {
                //exclude .meta files
                if (fi.Extension == ".jpg" || fi.Extension == ".png" || fi.Extension == ".JPG" || fi.Extension == ".PNG")
                {
                    {
                        WWW www = new WWW("file://" + Application.streamingAssetsPath + @"/Tips/Img/" + fi.Name);
/*
#if UNITY_STANDALONE_OSX
                        www = new WWW("File://" + Application.dataPath + @"/Raw/Tips/Img/" + fi.Name);

#endif
*/
                        var tempText = www.texture;
                        var tempSprite = Sprite.Create(tempText, new Rect(0, 0, tempText.width, tempText.height),
                            new Vector2(0.5f, 0.5f));
                        ImagesTipsList.Add(tempSprite);
                        ImagesTipsList[i].name = fi.Name;
                        i++;
                    }
                }
            }
        }

        private static void LoadShortTipsText()
        {
            var filepath = Application.streamingAssetsPath + @"/Tips/Txt/ShortTips.xml";
/*
#if UNITY_STANDALONE_OSX
            filepath = Application.dataPath + @"/Raw/Tips/Txt/ShortTips.xml";
#endif
*/
            var xmlDoc = new XmlDocument();
            var tipText = "tip" + Language.langRT;

            if (File.Exists(filepath))
            {
                xmlDoc.Load(filepath);
                var tipsList = xmlDoc.GetElementsByTagName("Tips");
            
                foreach (XmlNode tipNumber in tipsList)
                {
                    var tipContent = tipNumber.ChildNodes;

                    foreach (XmlNode tipLine in tipContent)
                    {
                        foreach (XmlNode xmlsettings in tipLine)
                        {
                            if (xmlsettings.Name == tipText)
                                ShortTextTipsList.Add(xmlsettings.InnerText);
                            if (xmlsettings.Name == "tipSubject")
                                ShortSubjectsList.Add(xmlsettings.InnerText);
                        }
                    }
                }

            }
        }

        private static void LoadMediumTipsText()
        {
            var filepath = Application.streamingAssetsPath + @"/Tips/Txt/MediumTips.xml";
            /*
            #if UNITY_STANDALONE_OSX
                        filepath = Application.dataPath + @"/Raw/Tips/Txt/MediumTips.xml";
            #endif
            */
            var xmlDoc = new XmlDocument();
            var tipText = "tip" + Language.langRT;

            if (File.Exists(filepath))
            {
                xmlDoc.Load(filepath);
                var tipsList = xmlDoc.GetElementsByTagName("Tips");

                foreach (XmlNode tipNumber in tipsList)
                {
                    var tipContent = tipNumber.ChildNodes;

                    foreach (XmlNode tipLine in tipContent)
                    {
                        foreach (XmlNode xmlsettings in tipLine)
                        {
                            if (xmlsettings.Name == tipText)
                                MediumTextTipsList.Add(xmlsettings.InnerText);
                            if (xmlsettings.Name == "tipSubject")
                                MediumSubjectsList.Add(xmlsettings.InnerText);
                        }
                    }
                }

            }
        }

        private static void LoadLongTipsText()
        {
            var filepath = Application.streamingAssetsPath + @"/Tips/Txt/LongTips.xml";
            /*
            #if UNITY_STANDALONE_OSX
                        filepath = Application.dataPath + @"/Raw/Tips/Txt/LongTips.xml";
            #endif
            */
            var xmlDoc = new XmlDocument();
            var tipText = "tip" + Language.langRT;

            if (File.Exists(filepath))
            {
                xmlDoc.Load(filepath);
                var tipsList = xmlDoc.GetElementsByTagName("Tips");

                foreach (XmlNode tipNumber in tipsList)
                {
                    var tipContent = tipNumber.ChildNodes;

                    foreach (XmlNode tipLine in tipContent)
                    {
                        foreach (XmlNode xmlsettings in tipLine)
                        {
                            if (xmlsettings.Name == tipText)
                                LongTextTipsList.Add(xmlsettings.InnerText);
                            if (xmlsettings.Name == "tipSubject")
                                LongSubjectsList.Add(xmlsettings.InnerText);
                        }
                    }
                }

            }
        }
    }
}
