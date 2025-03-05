using System.Collections.Generic;
using System.IO;
using System.Xml;
using Assets.scripts.GUI;
using UnityEngine;

namespace Assets.scripts.objectives.Action
{
    public class LoadReceipts : MonoBehaviour
    {

        //public static List<List<string>> Receipts = new List<List<string>>();
        
        public static string CurrentScene;
        private GameObject _mger;
        private ObjectiveManager _objManager;
        
        private void Start()
        {
            _mger = GameObject.FindGameObjectWithTag("Manager");
            _objManager = _mger.GetComponent<ObjectiveManager>();
            //LoadReceiptsList();
        }

        private void Update()
        {
            if (_objManager.GetCurrentObjective != null && _objManager.GetCurrentObjective.ToString().Contains("ReceiptDisplay") && DrawObjectiveList.Minimized)
            {
                if (Application.loadedLevelName != "SMReceipt")
                {
                    CurrentScene = Application.loadedLevelName;
                    Application.LoadLevel("SMReceipt");
                }
            }
        }
        /*
        private void LoadReceiptsList()
        {
            var filepath = Application.streamingAssetsPath + @"/Receipts/Receipts.xml";
            /*
#if UNITY_STANDALONE_OSX
            filepath = Application.dataPath + @"/Raw/Receipts/Receipts.xml";
#endif
*//*
            var xmlDoc = new XmlDocument();
            
            if (File.Exists(filepath))
            {
                xmlDoc.Load(filepath);
                var receiptsList = xmlDoc.GetElementsByTagName("Receipts");

                foreach (XmlNode rGroup in receiptsList)
                {
                    var groupContent = rGroup.ChildNodes;

                    foreach (XmlNode receipt in groupContent)
                    {
                        var tempList = new List<string>();

                        foreach (XmlNode line in receipt)
                        {
                            tempList.Add(line.InnerText);
                        }

                        Receipts.Add(tempList);
                    }
                }
            }
        }*/
    }
}
