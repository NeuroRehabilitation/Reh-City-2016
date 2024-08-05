using System;
using UnityEngine;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Xml;
using Assets.scripts;
using Assets.scripts.Models;
using Assets.scripts.RehaTask.RehaTaskGUI;
using Assets.scripts.UDP;

public class LoadSaveSettings : MonoBehaviour {

    private static string _filepath = string.Empty;
    private static XmlWriter _writer;
    private static string _path;

    public static void SaveSettingsInfo()
    {
        //PerformanceProcessor.ProcessPerformance();

        _path = Application.dataPath + "/RehaCity_Log/" + Main_Menu.Uid + "/";
        
        if (!Directory.Exists(_path))
        {
            Directory.CreateDirectory(_path);
        }

        _filepath = _path + Main_Menu.Uid  + "_Session_Data.xml";

        var settings = new XmlWriterSettings
        {
            Indent = true,
            NewLineOnAttributes = true
        };

        var hand = "Right";
        if (Main_Menu.LArm)
            hand = "Left";

        _writer = XmlWriter.Create(_filepath, settings);
        _writer.WriteStartDocument();

        _writer.WriteStartElement("Config");

        _writer.WriteElementString("CalibXmin", UdpReceive.Xmin.ToString(CultureInfo.InvariantCulture));
        _writer.WriteElementString("CalibXmax", UdpReceive.Xmax.ToString(CultureInfo.InvariantCulture));
        _writer.WriteElementString("CalibYmin", UdpReceive.Ymin.ToString(CultureInfo.InvariantCulture));
        _writer.WriteElementString("CalibYmax", UdpReceive.Ymax.ToString(CultureInfo.InvariantCulture));
        _writer.WriteElementString("Hand", hand);
        _writer.WriteElementString("Attention", PerformanceProcessor.CurrentProfileModel.Attention.ToString(CultureInfo.InvariantCulture));
        _writer.WriteElementString("Memory", PerformanceProcessor.CurrentProfileModel.Memory.ToString(CultureInfo.InvariantCulture));
        _writer.WriteElementString("ExecFunctions", PerformanceProcessor.CurrentProfileModel.ExFunctions.ToString(CultureInfo.InvariantCulture));
        _writer.WriteElementString("Language", PerformanceProcessor.CurrentProfileModel.Language.ToString(CultureInfo.InvariantCulture));
        _writer.WriteElementString("Difficulty", PerformanceProcessor.CurrentProfileModel.Difficulty.ToString(CultureInfo.InvariantCulture));
        _writer.WriteElementString("ActionSeq", PerformanceProcessor.ActionSeq.ToString());
        _writer.WriteElementString("Cancellation", PerformanceProcessor.Cancellation.ToString());
        _writer.WriteElementString("CatPer", PerformanceProcessor.Cat.ToString());
        _writer.WriteElementString("Pairs", PerformanceProcessor.Pairs.ToString());
        _writer.WriteElementString("MemoryStories", PerformanceProcessor.MemoryStories.ToString());
        _writer.WriteElementString("NumSeq", PerformanceProcessor.NumSeq.ToString());
        _writer.WriteElementString("ProbSolv", PerformanceProcessor.ProbSolv.ToString());
        _writer.WriteElementString("ActionSeqPerform", PerformanceProcessor.ActionSeqPerform.ToString(CultureInfo.InvariantCulture));
        _writer.WriteElementString("CancellationPerform", PerformanceProcessor.CancellationPerform.ToString(CultureInfo.InvariantCulture));
        _writer.WriteElementString("CatPerform", PerformanceProcessor.CatPerform.ToString(CultureInfo.InvariantCulture));
        _writer.WriteElementString("PairsPerform", PerformanceProcessor.PairsPerform.ToString(CultureInfo.InvariantCulture));
        _writer.WriteElementString("MazesPerform", PerformanceProcessor.MazesPerform.ToString(CultureInfo.InvariantCulture));
        _writer.WriteElementString("MemoryStoriesPerform", PerformanceProcessor.MemoryStoriesPerform.ToString(CultureInfo.InvariantCulture));
        _writer.WriteElementString("NumSeqPerform", PerformanceProcessor.NumSeqPerform.ToString(CultureInfo.InvariantCulture));
        _writer.WriteElementString("ProbSolvPerform", PerformanceProcessor.ProbSolvPerform.ToString(CultureInfo.InvariantCulture));
        _writer.WriteElementString("DomainsDivisor", PerformanceProcessor.DomainsDivisor.ToString());
        _writer.WriteElementString("NumberOfLocations", PerformanceProcessor.NumberOfLocations.ToString());
        _writer.WriteElementString("Performance", PerformanceProcessor.LevelPerformance.ToString(CultureInfo.InvariantCulture));

        _writer.WriteEndElement();//Calibration

        _writer.WriteEndDocument();
        _writer.Flush();
        _writer.Close();

        File.Copy(_filepath, Application.dataPath + "/RehaCity_Log/" + Main_Menu.Uid + "/" + Main_Menu.Uid + "_" + DateTime.Now.ToString("yyyyMMdd") + "/" + Main_Menu.Uid + DateTime.Now.ToString("HHmmss") + "_Session_Data.xml", false);
    }

    public static void LoadSettingsValues()
    {
        var filepath2 = Application.dataPath + "/RehaCity_Log/" + Main_Menu.Uid + "/" + Main_Menu.Uid + "_Session_Data.xml";

        var xmlDoc = new XmlDocument();

        var profile = new Model(0, 0, 0, 0, 0);

        if (File.Exists(filepath2))
        {
            xmlDoc.Load(filepath2);

            var configList = xmlDoc.GetElementsByTagName("Config");
            var att = 0.0f;
            var mem = 0.0f;
            var execF = 0.0f;
            var lang = 0.0f;
            var diff = 0.0f;

            foreach (XmlNode configInfo in configList)
            {
                var xmlcontent = configInfo.ChildNodes;

                foreach (XmlNode xmlsettings in xmlcontent)
                {
                    if (xmlsettings.Name == "CalibXmin")
                        UdpReceive.Xmin = float.Parse(xmlsettings.InnerText);

                    if (xmlsettings.Name == "CalibXmax")
                        UdpReceive.Xmax = float.Parse(xmlsettings.InnerText);

                    if (xmlsettings.Name == "CalibYmin")
                        UdpReceive.Ymin = float.Parse(xmlsettings.InnerText);

                    if (xmlsettings.Name == "CalibYmax")
                        UdpReceive.Ymax = float.Parse(xmlsettings.InnerText);

                    if (xmlsettings.Name == "CalibYmax")
                        UdpReceive.Ymax = float.Parse(xmlsettings.InnerText);

                    if (xmlsettings.Name == "Attention")
                        att = float.Parse(xmlsettings.InnerText);

                    if (xmlsettings.Name == "Memory")
                        mem = float.Parse(xmlsettings.InnerText);

                    if (xmlsettings.Name == "ExecFunctions")
                        execF = float.Parse(xmlsettings.InnerText);

                    if (xmlsettings.Name == "Language")
                        lang = float.Parse(xmlsettings.InnerText);

                    if (xmlsettings.Name == "Difficulty")
                        diff = float.Parse(xmlsettings.InnerText);

                    if (xmlsettings.Name == "ActionSeq")
                        PerformanceProcessor.ActionSeq = bool.Parse(xmlsettings.InnerText);

                    if (xmlsettings.Name == "Cancellation")
                        PerformanceProcessor.Cancellation = bool.Parse(xmlsettings.InnerText);

                    if (xmlsettings.Name == "Cat")
                        PerformanceProcessor.Cat = bool.Parse(xmlsettings.InnerText);

                    if (xmlsettings.Name == "Pairs")
                        PerformanceProcessor.Pairs = bool.Parse(xmlsettings.InnerText);

                    if (xmlsettings.Name == "MemoryStories")
                        PerformanceProcessor.MemoryStories = bool.Parse(xmlsettings.InnerText);

                    if (xmlsettings.Name == "NumSeq")
                        PerformanceProcessor.NumSeq = bool.Parse(xmlsettings.InnerText);

                    if (xmlsettings.Name == "ProbSolv")
                        PerformanceProcessor.ProbSolv = bool.Parse(xmlsettings.InnerText);

                    if (xmlsettings.Name == "ActionSeqPerform")
                        PerformanceProcessor.ActionSeqPerform = float.Parse(xmlsettings.InnerText);

                    if (xmlsettings.Name == "CancellationPerform")
                        PerformanceProcessor.CancellationPerform = float.Parse(xmlsettings.InnerText);

                    if (xmlsettings.Name == "CatPerform")
                        PerformanceProcessor.CatPerform = float.Parse(xmlsettings.InnerText);

                    if (xmlsettings.Name == "PairsPerform")
                        PerformanceProcessor.PairsPerform = float.Parse(xmlsettings.InnerText);

                    if (xmlsettings.Name == "MazesPerform")
                        PerformanceProcessor.MazesPerform = float.Parse(xmlsettings.InnerText);

                    if (xmlsettings.Name == "MemoryStoriesPerform")
                        PerformanceProcessor.MemoryStoriesPerform = float.Parse(xmlsettings.InnerText);
                    
                    if (xmlsettings.Name == "NumSeqPerform")
                        PerformanceProcessor.NumSeqPerform = float.Parse(xmlsettings.InnerText);

                    if (xmlsettings.Name == "ProbSolvPerform")
                        PerformanceProcessor.ProbSolvPerform = float.Parse(xmlsettings.InnerText);

                    if (xmlsettings.Name == "DomainsDivisor")
                        PerformanceProcessor.DomainsDivisor = int.Parse(xmlsettings.InnerText);

                    if (xmlsettings.Name == "NumberOfLocations")
                        PerformanceProcessor.NumberOfLocations = int.Parse(xmlsettings.InnerText);
                }

                profile = new Model(att, mem, execF, lang, diff);
                
            }
        }
        Debug.Log("Loaded data from file: NumberOfLocations: " + PerformanceProcessor.NumberOfLocations + "MazesPerform: " + PerformanceProcessor.MazesPerform + " ; MemoryStoriesPerform: " + PerformanceProcessor.MemoryStoriesPerform + " ; ActionSeqPerform: " + PerformanceProcessor.ActionSeqPerform + " ; CatPerform: " + PerformanceProcessor.CatPerform + " ; PairsPerform: " + PerformanceProcessor.PairsPerform + " ; CancellationPerform: " +
                               PerformanceProcessor.CancellationPerform + " ; NumSeqPerform: " + PerformanceProcessor.NumSeqPerform + " ; ProbSolvPerform: " + PerformanceProcessor.ProbSolvPerform + " ; DomainsDivisor: " + PerformanceProcessor.DomainsDivisor);
        PerformanceProcessor.CurrentProfileModel = profile;
    }
}
