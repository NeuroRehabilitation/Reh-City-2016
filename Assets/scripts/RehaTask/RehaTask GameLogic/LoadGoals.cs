using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Assets.scripts.RehaTask.RehaTask_GameLogic;
using Assets.scripts.Settings;

public class LoadGoals : MonoBehaviour {

    public static List<string> FashionStoreGoalsPT = new List<string>();
    public static List<string> FashionStoreGoalsEN = new List<string>();
    public static List<string> Home1GoalsPT = new List<string>();
    public static List<string> Home1GoalsEN = new List<string>();
    public static List<string> Home2GoalsPT = new List<string>();
    public static List<string> Home2GoalsEN = new List<string>();
    public static List<string> Home3GoalsPT = new List<string>();
    public static List<string> Home3GoalsEN = new List<string>();

    private void Awake()
    {
        LoadPTGoals();
        LoadENGoals();
    }

    private void LoadPTGoals()
    {
        var filepathPT = Application.streamingAssetsPath + @"/Settings/GoalsPT.xml";

        /*
#if UNITY_STANDALONE_OSX
            filepath = Application.dataPath + @"/Raw/Settings/GoalsPT.xml";
#endif
*/
        var xmlDoc = new XmlDocument();

        if (File.Exists(filepathPT))
        {
            xmlDoc.Load(filepathPT);

            var goalsList = xmlDoc.GetElementsByTagName("Goals");

            foreach (XmlNode goalList in goalsList)
            {
                var placesList = goalList.ChildNodes;
                
                foreach (XmlNode place in placesList)
                {
                    if (place.Name == "FashionStore")
                    {
                        var fashionStoreList = place.ChildNodes;
                        foreach (XmlNode goal in fashionStoreList)
                        {
                            FashionStoreGoalsPT.Add(goal.InnerText);
                        }
                    }
                    else if (place.Name == "Home-1")
                    {
                        var homeOneList = place.ChildNodes;
                        foreach (XmlNode goal in homeOneList)
                        {
                            Home1GoalsPT.Add(goal.InnerText);
                        }
                    }
                    else if (place.Name == "Home-2")
                    {
                        var homeTwoList = place.ChildNodes;
                        foreach (XmlNode goal in homeTwoList)
                        {
                            Home2GoalsPT.Add(goal.InnerText);
                        }
                    }
                    else if (place.Name == "Home-3")
                    {
                        var homeThreeList = place.ChildNodes;
                        foreach (XmlNode goal in homeThreeList)
                        {
                            Home3GoalsPT.Add(goal.InnerText);
                        }
                    }
                }
            }
        }
    }

    private void LoadENGoals()
    {
        var filepathEN = Application.streamingAssetsPath + @"/Settings/GoalsEN.xml";

        /*
#if UNITY_STANDALONE_OSX
        filepath = Application.dataPath + @"/Raw/Settings/GoalsEN.xml";
#endif
*/

        var xmlDoc = new XmlDocument();

        if (File.Exists(filepathEN))
        {
            xmlDoc.Load(filepathEN);

            var goalsList = xmlDoc.GetElementsByTagName("Goals");

            foreach (XmlNode goalList in goalsList)
            {
                var placesList = goalList.ChildNodes;

                foreach (XmlNode place in placesList)
                {
                    if (place.Name == "FashionStore")
                    {
                        var fashionStoreList = place.ChildNodes;
                        foreach (XmlNode goal in fashionStoreList)
                        {
                            FashionStoreGoalsEN.Add(goal.InnerText);
                        }
                    }
                    else if (place.Name == "Home-1")
                    {
                        var homeOneList = place.ChildNodes;
                        foreach (XmlNode goal in homeOneList)
                        {
                            Home1GoalsEN.Add(goal.InnerText);
                        }
                    }
                    else if (place.Name == "Home-2")
                    {
                        var homeTwoList = place.ChildNodes;
                        foreach (XmlNode goal in homeTwoList)
                        {
                            Home2GoalsEN.Add(goal.InnerText);
                        }
                    }
                    else if (place.Name == "Home-3")
                    {
                        var homeThreeList = place.ChildNodes;
                        foreach (XmlNode goal in homeThreeList)
                        {
                            Home3GoalsEN.Add(goal.InnerText);
                        }
                    }
                }
            }
        }
    }

    public static string SetGoal()
    {
        string goal = null;

        if (SpawnTiles.Folder == "FashionStore")
        {
            if (Language.langRT == "PT")
            {
                goal = FashionStoreGoalsPT[int.Parse(LoadGame.SubCategory)];
            }
            else
            {
                goal = FashionStoreGoalsEN[int.Parse(LoadGame.SubCategory)];
            }
        }

        if (SpawnTiles.Folder == "Home-1")
        {
            if (Language.langRT == "PT")
            {
                goal = Home1GoalsPT[int.Parse(LoadGame.SubCategory)];
            }
            else
            {
                goal = Home1GoalsEN[int.Parse(LoadGame.SubCategory)];
            }
        }

        if (SpawnTiles.Folder == "Home-2")
        {
            if (Language.langRT == "PT")
            {
                goal = Home2GoalsPT[int.Parse(LoadGame.SubCategory)];
            }
            else
            {
                goal = Home2GoalsEN[int.Parse(LoadGame.SubCategory)];
            }
        }

        if (SpawnTiles.Folder == "Home-3")
        {
            if (Language.langRT == "PT")
            {
                goal = Home3GoalsPT[int.Parse(LoadGame.SubCategory)];
            }
            else
            {
                goal = Home3GoalsEN[int.Parse(LoadGame.SubCategory)];
            }
        }

        return goal;
    }
}
