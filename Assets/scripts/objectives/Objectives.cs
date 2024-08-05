using System.Collections.Generic;
using System.Xml;
using Assets.scripts.GUI;
using Assets.scripts.MiniMapScripts.AStar;
using UnityEngine;

namespace Assets.scripts.objectives
{
    public class Objectives {

        /// <summary>
        /// used to store the names of objects that have to be collected in this 
        /// Objective
        /// </summary>
        protected List<string> answerset;
        public List<string> AnswerSet
        {
            get
            {
                return answerset;
            }
        }

        protected Dictionary<string, int> collectedset;
        public Dictionary<string, int> CollectedSet
        {
            get
            {
                return collectedset;
            }
        }

        protected string description;
        protected Vector3 location;
        protected bool completed;
        
        protected TextAsset xmltext;
        protected XmlDocument xmldoc;
        /// <summary>
        /// gets to the node of either english or portuguese
        /// </summary>
        protected XmlNodeList languagelist;
        /// <summary>
        /// gets to the node of eiter LocationObjective, ActionObjective, CollectionObjective
        /// </summary>
        protected static XmlNodeList nodelist; 
        /// <summary>
        /// gets to the node of individual objectives 
        /// </summary>
        protected XmlNodeList childlist;
	
        protected Transform player;
        /// <summary>
        /// used by objindicator to get the texture based on Objective name
        /// </summary>
        public string name;
        /// <summary>
        ///  boolean to check if an Objective can be pushed to the queue
        /// </summary>
        public bool CanbePushed = false ;

        /// <summary>
        /// stores the correct scene number to push this Objective to the queue
        /// </summary>
        public int RequiredSceneToSpawn = -1;
        /// <summary>
        /// This is used when an Objective is Completed and if next Objective is in other scene then, it should wait.
        /// </summary>
        public bool CanAddNextObjective;

        /// <summary>
        /// used by abstract objectives
        /// </summary>
        public int NumberofItemsCollected;
        

        #region UsedbyCollectionObjectives
        public string FirstItemName;
        public string SecondItemName;
        public string ThirdItemName = "NaN";
        public string FourthItemName = "NaN";
        public int NumberofFirstItem;
        public int NumberofSecondItem;
        public int NumberofThirdItem;
        public int NumberofFourthItem;
        #endregion

        #region UsedBy RehaTask objectives 
        public int RehaTaskElements;
        public int RehaTaskTargets, TaskTime;
        public bool DisplayGoal;
        public int RehaTaskLevel;
        public int RehaTaskScenario;
        #endregion

        #region usedBy SM Receipt display
        public List<string> ItemsNames = new List<string>();
        public List<int> ItemsQuantities = new List<int>();
        public List<float> ItemsPrices = new List<float>();
        public bool WrongSum;
        #endregion

        #region used by TipDisplay and Questions
        public int TipType = 0;
        public int TipNumber = 0;
        public List<string> QuestionsList = new List<string>();
        public List<string> SubjectsList = new List<string>();
        #endregion

        public string Abstraction = "NaN";
        public float completedTimer = 0.5f;
        public float Performance;
        
        #region usedBy Bank objectives
        public string NumericSequence;
        public bool CanSetupCode;
        public string ButtonToPress;
        public string Type = "NaN";
        #endregion

        public int Distractors;

        public string Description
        {
            get
            {
                return description;
            }
        }
	
        public Vector3 Location
        {
            get
            {
                return location;
            }
        }

        public bool Completed
        {
            get
            {
                return completed;
            }
        }


        public Objectives()
        {
            if(player == null && Application.loadedLevelName == "City")
                player = GameObject.FindGameObjectWithTag("Player").transform;
            this.description = "None";
            name = "None";
            FirstItemName = "NaN";
            SecondItemName = "NaN";

            NumberofItemsCollected = 0;
        }

        // called from ObjectiveManager
        public void SetLanguage(bool English, bool Portuguese)
        {
            var xmltext = Application.streamingAssetsPath + @"/XMLData/ObjectiveXML.xml";
            xmldoc = new XmlDocument();
            xmldoc.Load(xmltext);
            languagelist = English ? xmldoc.GetElementsByTagName("English"):languagelist;
            languagelist = Portuguese ? xmldoc.GetElementsByTagName("Portuguese"): languagelist;
            foreach (XmlNode node in languagelist)
            {
                nodelist = node.ChildNodes;
            }
        }

        public void CompleteObjective()
        {
            if (completed && DrawObjectiveList.CanGoToNextObj)
            {
                DrawObjectiveList.CanGoToNextObj = false;
                Application.LoadLevel("City");

            }
        }
        
        public virtual void CheckForCompletion()
        {}
    }
}
