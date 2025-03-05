using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

namespace Assets.scripts.RehaTask.RehaTask_GameLogic
{
    public class LoadLevels : MonoBehaviour {

        private int _columns, _rows, _targets, _correctchoices, _imgConstruct, _pairs;
        private bool _showcorrect, _useDistractors, _useMemory, _complete, _sequence, _keepThumbnail;
        private float _picTimer;
        private string _folder, _imagesToDisplay;

        public static List<Level> Levels = new List<Level>();

        private void Awake()
        {
            if(LoadGame.CanSetRehaTask)
                LoadLevelConfig();
        }

        //reads all the Levels from xml File and stores info in Levels list
        private void LoadLevelConfig()
        {
            var filepath = Application.streamingAssetsPath + @"/Settings/Levels.xml";
/*
#if UNITY_STANDALONE_OSX
            filepath = Application.dataPath + @"/Raw/Settings/Levels.xml";
#endif
*/
            var xmlDoc = new XmlDocument();

            if (File.Exists(filepath))
            {
                xmlDoc.Load(filepath);
                var levelsList = xmlDoc.GetElementsByTagName("Levels");

                foreach (XmlNode levelNumber in levelsList)
                {
                    var levelcontent = levelNumber.ChildNodes;

                    foreach (XmlNode levelnum in levelcontent)
                    {
                        var level = levelnum.Name;

                        foreach (XmlNode xmlsettings in levelnum)
                        {
                            if (xmlsettings.Name == "Columns")
                                _columns = int.Parse(xmlsettings.InnerText);
                            if (xmlsettings.Name == "Rows")
                                _rows = int.Parse(xmlsettings.InnerText);
                            if (xmlsettings.Name == "Targets")
                                _targets = int.Parse(xmlsettings.InnerText);
                            if (xmlsettings.Name == "Correctchoices")
                                _correctchoices = int.Parse(xmlsettings.InnerText);
                            if (xmlsettings.Name == "Showcorrect")
                                _showcorrect = bool.Parse(xmlsettings.InnerText);
                            if (xmlsettings.Name == "PicTimer")
                                _picTimer = float.Parse(xmlsettings.InnerText);
                            if (xmlsettings.Name == "Category")
                                _folder = xmlsettings.InnerText;
                            if (xmlsettings.Name == "ImagesToDisplay")
                                _imagesToDisplay = xmlsettings.InnerText;
                            if (xmlsettings.Name == "UseDistractors")
                                _useDistractors = bool.Parse(xmlsettings.InnerText);
                            if (xmlsettings.Name == "UseMemory")
                                _useMemory = bool.Parse(xmlsettings.InnerText);
                            if (xmlsettings.Name == "ImageToConstruct")
                            {
                                if(xmlsettings.InnerText != "")
                                    _imgConstruct = int.Parse(xmlsettings.InnerText);
                                else
                                    _imgConstruct = -1;  
                            }
                            if (xmlsettings.Name == "Sequence")
                                _sequence = bool.Parse(xmlsettings.InnerText);
                            if (xmlsettings.Name == "KeepThumbnail")
                                _keepThumbnail = bool.Parse(xmlsettings.InnerText);
                            if (xmlsettings.Name == "Pairs")
                                _pairs = int.Parse(xmlsettings.InnerText);

                            _complete = false;
                        } //foreach xmlcontent
                        Levels.Add(new Level(level, _columns, _rows, _targets, _correctchoices, _picTimer,
                            _folder, _imagesToDisplay, _useDistractors, _useMemory, _showcorrect, _complete, _imgConstruct, _sequence, _keepThumbnail, _pairs));
                    } //foreach level
                } //foreach levelnum
            } //foreach configList	

            SpawnTiles.TotalLevels = LoadLevels.Levels.Count;
        } //loadconfig
    }
}
