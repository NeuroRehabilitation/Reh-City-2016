using System.Xml;
using UnityEngine;

namespace Assets.scripts.Manager
{
    public class LanguageManager : MonoBehaviour {

        [HideInInspector]
        public bool English = false;
        [HideInInspector]
        public bool Portuguese = false;

        private XmlDocument xmldoc;
        /// <summary>
        /// this gets to the node of either english or portuguese
        /// </summary>
        private XmlNodeList languagelist;
        /// <summary>
        /// this gets the child nodes of languagelist
        /// </summary>
        private XmlNodeList objectivelist;

        private static LanguageManager s_Instance = null;
        public static LanguageManager Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindObjectOfType(typeof(LanguageManager)) as LanguageManager;
                    if (s_Instance == null)
                    {
                        Debug.Log("Could not locate ObjectiveManager");
                    }
                }
                return s_Instance;
            }
        }
    
        public void SetLanguage()
        {
            var xmltext = Application.streamingAssetsPath + @"/XMLData/DialoguesXML.xml";
            xmldoc = new XmlDocument();
            xmldoc.Load(xmltext);
            if(English&&Portuguese){
                Debug.LogError("Cannot Choose both languages. Choose only one");
                Debug.Log("Two Languages are chosen. So, changing Language to English");
                Portuguese = false;
            }
            languagelist = English  ? xmldoc.GetElementsByTagName("English") : languagelist;
            languagelist = Portuguese ? xmldoc.GetElementsByTagName("Portuguese") : languagelist;
            foreach (XmlNode node in languagelist)
            {
                objectivelist = node.ChildNodes;
            }
        
        }

        /*Look into DialoguesXML to understand next text
     * objectivelist[0] gives EnterRoom
     * objectivelist[0].childnodes[0] gives SuperMarket and innertext gives all the text inside that node
    */
        public string EnterRoomtext(string name)
        {
            switch (name)
            {
                case "SuperMarket":
                    return objectivelist[0].ChildNodes[0].ChildNodes[0].InnerText;
                case "PostOffice":
                    return objectivelist[0].ChildNodes[2].ChildNodes[0].InnerText;
                case "Pharmacy":
                    return objectivelist[0].ChildNodes[1].ChildNodes[0].InnerText;
                case "Bank":
                    return objectivelist[0].ChildNodes[3].ChildNodes[0].InnerText;
                case "Home":
                    return objectivelist[0].ChildNodes[4].ChildNodes[0].InnerText;
                case "FashionStore":
                    return objectivelist[0].ChildNodes[5].ChildNodes[0].InnerText;
                case "Kiosk":
                    return objectivelist[0].ChildNodes[6].ChildNodes[0].InnerText;
                case "Park":
                    return objectivelist[0].ChildNodes[7].ChildNodes[0].InnerText;
                default:
                    return " Language Was Not Chosen";
            }
        }
        
        // used for objects that needs name
        // both by Postoffice
        public string GetObjectText(string name)
        {
            switch (name)
            {
                case "Stamp":
                    return objectivelist[0].ChildNodes[2].ChildNodes[1].InnerText;
                case "StampPlural":
                    return objectivelist[2].ChildNodes[2].ChildNodes[0].InnerText;
                case "Letter":
                    return objectivelist[0].ChildNodes[2].ChildNodes[2].InnerText;
                case "LetterPlural":
                    return objectivelist[2].ChildNodes[2].ChildNodes[1].InnerText;
                case "Package":
                    return objectivelist[0].ChildNodes[2].ChildNodes[3].InnerText;
                case "PackagePlural":
                    return objectivelist[2].ChildNodes[2].ChildNodes[2].InnerText;
                case "ChildrenBook":
                    return objectivelist[0].ChildNodes[2].ChildNodes[4].InnerText;
                case "ChildrenBookPlural":
                    return objectivelist[2].ChildNodes[2].ChildNodes[3].InnerText;
                case "AdultBook":
                    return objectivelist[0].ChildNodes[2].ChildNodes[5].InnerText;
                case "AdultBookPlural":
                    return objectivelist[2].ChildNodes[2].ChildNodes[4].InnerText;
                case "Postcard":
                    return objectivelist[0].ChildNodes[2].ChildNodes[6].InnerText;
                case "PostcardPlural":
                    return objectivelist[2].ChildNodes[2].ChildNodes[5].InnerText;
                case "CDsBox":
                    return objectivelist[0].ChildNodes[2].ChildNodes[7].InnerText;
                case "CDsBoxPlural":
                    return objectivelist[2].ChildNodes[2].ChildNodes[6].InnerText;
                case "Lottery":
                    return objectivelist[0].ChildNodes[2].ChildNodes[8].InnerText;
                case "LotteryPlural":
                    return objectivelist[2].ChildNodes[2].ChildNodes[7].InnerText;
                case "Milk":
                    return objectivelist[0].ChildNodes[0].ChildNodes[1].InnerText;
                case "MilkPlural":
                    return objectivelist[2].ChildNodes[0].ChildNodes[0].InnerText;
                case "Juice":
                    return objectivelist[0].ChildNodes[0].ChildNodes[2].InnerText;
                case "JuicePlural":
                    return objectivelist[2].ChildNodes[0].ChildNodes[1].InnerText;
                case "Bread":
                    return objectivelist[0].ChildNodes[0].ChildNodes[3].InnerText;
                case "BreadPlural":
                    return objectivelist[2].ChildNodes[0].ChildNodes[2].InnerText;
                case "Water":
                    return objectivelist[0].ChildNodes[0].ChildNodes[4].InnerText;
                case "WaterPlural":
                    return objectivelist[2].ChildNodes[0].ChildNodes[3].InnerText;
                case "Apple":
                    return objectivelist[0].ChildNodes[0].ChildNodes[5].InnerText;
                case "ApplePlural":
                    return objectivelist[2].ChildNodes[0].ChildNodes[4].InnerText;
                case "Orange":
                    return objectivelist[0].ChildNodes[0].ChildNodes[6].InnerText;
                case "OrangePlural":
                    return objectivelist[2].ChildNodes[0].ChildNodes[5].InnerText;
                case "Nutella":
                    return objectivelist[0].ChildNodes[0].ChildNodes[7].InnerText;
                case "NutellaPlural":
                    return objectivelist[2].ChildNodes[0].ChildNodes[6].InnerText;
                case "Shampoo":
                    return objectivelist[0].ChildNodes[0].ChildNodes[8].InnerText;
                case "ShampooPlural":
                    return objectivelist[2].ChildNodes[0].ChildNodes[7].InnerText;
                case "Coffee":
                    return objectivelist[0].ChildNodes[0].ChildNodes[9].InnerText;
                case "CoffeePlural":
                    return objectivelist[2].ChildNodes[0].ChildNodes[8].InnerText;
                case "Sauce":
                    return objectivelist[0].ChildNodes[0].ChildNodes[10].InnerText;
                case "SaucePlural":
                    return objectivelist[2].ChildNodes[0].ChildNodes[9].InnerText;
                case "Yogurt":
                    return objectivelist[0].ChildNodes[0].ChildNodes[11].InnerText;
                case "YogurtPlural":
                    return objectivelist[2].ChildNodes[0].ChildNodes[10].InnerText;
                case "Butter":
                    return objectivelist[0].ChildNodes[0].ChildNodes[12].InnerText;
                case "ButterPlural":
                    return objectivelist[2].ChildNodes[0].ChildNodes[11].InnerText;
                case "Kellogs":
                    return objectivelist[0].ChildNodes[0].ChildNodes[13].InnerText;
                case "KellogsPlural":
                    return objectivelist[2].ChildNodes[0].ChildNodes[12].InnerText;
                case "Coke":
                    return objectivelist[0].ChildNodes[0].ChildNodes[14].InnerText;
                case "CokePlural":
                    return objectivelist[2].ChildNodes[0].ChildNodes[13].InnerText;
                case "Pasta":
                    return objectivelist[0].ChildNodes[0].ChildNodes[15].InnerText;
                case "PastaPlural":
                    return objectivelist[2].ChildNodes[0].ChildNodes[14].InnerText;
                case "OliveOil":
                    return objectivelist[0].ChildNodes[0].ChildNodes[16].InnerText;
                case "OliveOilPlural":
                    return objectivelist[2].ChildNodes[0].ChildNodes[15].InnerText;
                case "Chocapic":
                    return objectivelist[0].ChildNodes[0].ChildNodes[17].InnerText;
                case "ChocapicPlural":
                    return objectivelist[2].ChildNodes[0].ChildNodes[16].InnerText;
                case "Cream":
                    return objectivelist[0].ChildNodes[1].ChildNodes[1].InnerText;
                case "CreamPlural":
                    return objectivelist[2].ChildNodes[1].ChildNodes[0].InnerText;
                case "Syrup":
                    return objectivelist[0].ChildNodes[1].ChildNodes[2].InnerText;
                case "SyrupPlural":
                    return objectivelist[2].ChildNodes[1].ChildNodes[1].InnerText;
                case "Bandaid":
                    return objectivelist[0].ChildNodes[1].ChildNodes[3].InnerText;
                case "BandaidPlural":
                    return objectivelist[2].ChildNodes[1].ChildNodes[2].InnerText;
                case "Aspirin":
                    return objectivelist[0].ChildNodes[1].ChildNodes[4].InnerText;
                case "AspirinPlural":
                    return objectivelist[2].ChildNodes[1].ChildNodes[3].InnerText;
                case "Betadin":
                    return objectivelist[0].ChildNodes[1].ChildNodes[5].InnerText;
                case "BetadinPlural":
                    return objectivelist[2].ChildNodes[1].ChildNodes[4].InnerText;
                case "Sunscreen":
                    return objectivelist[0].ChildNodes[1].ChildNodes[6].InnerText;
                case "SunscreenPlural":
                    return objectivelist[2].ChildNodes[1].ChildNodes[5].InnerText;
                case "Benuron":
                    return objectivelist[0].ChildNodes[1].ChildNodes[7].InnerText;
                case "BenuronPlural":
                    return objectivelist[2].ChildNodes[1].ChildNodes[6].InnerText;
                case "Brufen":
                    return objectivelist[0].ChildNodes[1].ChildNodes[8].InnerText;
                case "BrufenPlural":
                    return objectivelist[2].ChildNodes[1].ChildNodes[7].InnerText;
                default:
                    return "No Lang";
            }
        }

        public string GetRehaTaskTaskString(string name)
        {
            switch (name)
            {
                case "Category":
                    return objectivelist[0].ChildNodes[4].ChildNodes[1].InnerText;
                case "Sequence":
                    return objectivelist[0].ChildNodes[5].ChildNodes[1].InnerText;
                case "Pairs":
                    return objectivelist[0].ChildNodes[7].ChildNodes[1].InnerText;
                default:
                    return "No Lang";
            }
        }

        public string GetbankOptionsString(string name)
        {
            switch (name)
            {
                case "WithDraw":
                    return objectivelist[0].ChildNodes[3].ChildNodes[1].InnerText;
                case "Consults":
                    return objectivelist[0].ChildNodes[3].ChildNodes[2].InnerText;
                case "Services":
                    return objectivelist[0].ChildNodes[3].ChildNodes[4].InnerText;
                case "Back":
                    return objectivelist[0].ChildNodes[3].ChildNodes[5].InnerText;
                case "Quit":
                    return objectivelist[0].ChildNodes[3].ChildNodes[6].InnerText;
                case "Payments":
                    return objectivelist[0].ChildNodes[3].ChildNodes[3].ChildNodes[0].InnerText;
                case "Electricity":
                    return objectivelist[0].ChildNodes[3].ChildNodes[3].ChildNodes[1].InnerText;
                case "Water":
                    return objectivelist[0].ChildNodes[3].ChildNodes[3].ChildNodes[2].InnerText;
                case "Telephone":
                    return objectivelist[0].ChildNodes[3].ChildNodes[3].ChildNodes[3].InnerText;
                default:
                    return "No Lang";
            }
        }
        public string WellDoneText()
        {
            return objectivelist[1].ChildNodes[0].InnerText;
        }
        public string ScoreText()
        {
            return objectivelist[1].ChildNodes[1].InnerText;
        }
        public string LevelCompleteString()
        {
            return objectivelist[1].ChildNodes[2].InnerText;
        }
    }
}
