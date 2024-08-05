using UnityEngine;

namespace Assets.scripts.objectives.Collection
{
    public class CollectMultiplePHItems : CollectionObjective {

        public CollectMultiplePHItems(int firstitem, string firstitemname, int seconditem, string seconditemname, int distractors)
        {
            answerset = new System.Collections.Generic.List<string>();
            collectedset = new System.Collections.Generic.Dictionary<string, int>();
            description = childlist[2].InnerText;
            description = description.Replace("@", firstitem.ToString());
            description = description.Replace("#", firstitem == 1 ? language.GetObjectText(firstitemname) : language.GetObjectText(firstitemname + "Plural"));
            description = description.Replace("!", seconditem.ToString());
            description = description.Replace("$", seconditem == 1 ? language.GetObjectText(seconditemname) : language.GetObjectText(seconditemname + "Plural"));
            FirstItemName = firstitemname;
            SecondItemName = seconditemname;
            answerset.Add(firstitemname);
            answerset.Add(seconditemname);

            NumberofFirstItem = firstitem;
            NumberofSecondItem = seconditem;
            Distractors = distractors;
            name = "Pharmacy";

        }

        public CollectMultiplePHItems(int firstitem, string firstitemname, int seconditem, string seconditemname, int thirditem,
            string thirditemname, int fourthitem, string fourthitemname, int distractors)
        {
            answerset = new System.Collections.Generic.List<string>();
            collectedset = new System.Collections.Generic.Dictionary<string, int>();
            description = childlist[3].InnerText;
            description = description.Replace("@", firstitem.ToString());
            description = description.Replace("#", firstitem == 1 ? language.GetObjectText(firstitemname) : language.GetObjectText(firstitemname + "Plural"));
            description = description.Replace("!", seconditem.ToString());
            description = description.Replace("$", seconditem == 1 ? language.GetObjectText(seconditemname) : language.GetObjectText(seconditemname + "Plural"));
            description = description.Replace("%", thirditem.ToString());
            description = description.Replace("^", thirditem == 1 ? language.GetObjectText(thirditemname) : language.GetObjectText(thirditemname + "Plural"));
            description = description.Replace("|", fourthitem.ToString());
            description = description.Replace("*", fourthitem == 1 ? language.GetObjectText(fourthitemname) : language.GetObjectText(fourthitemname + "Plural"));

            FirstItemName = firstitemname;
            SecondItemName = seconditemname;
            ThirdItemName = thirditemname;
            FourthItemName = fourthitemname;
            answerset.Add(firstitemname);
            answerset.Add(seconditemname);
            answerset.Add(thirditemname);
            answerset.Add(fourthitemname);
            NumberofFirstItem = firstitem;
            NumberofSecondItem = seconditem;

            NumberofThirdItem = thirditem;
            NumberofFourthItem = fourthitem;
            Distractors = distractors;
            name = "Pharmacy"; 
        }

        public override void CheckForCompletion()
        {
            if (collectedset.Count == 0) return;
        
            if (FirstItemName != null && CollectedSet.ContainsKey(FirstItemName))
                Debug.Log("NumberofFirstItem: " + NumberofFirstItem  + " _Collected so far: " + CollectedSet[FirstItemName] + " - " + FirstItemName);

            if (SecondItemName != null && CollectedSet.ContainsKey(SecondItemName))
                Debug.Log("NumberofSecondItem: " + NumberofSecondItem + " _Collected so far: " + CollectedSet[SecondItemName] + " - " + SecondItemName);

            if (ThirdItemName != null && CollectedSet.ContainsKey(ThirdItemName))
                Debug.Log("NumberofThirdItem: " + NumberofThirdItem + " _Collected so far: " + CollectedSet[ThirdItemName] + " - " + ThirdItemName);

            if (FourthItemName != null && CollectedSet.ContainsKey(FourthItemName))
                Debug.Log("NumberofFourthItem: " + NumberofFourthItem + " _Collected so far: " + CollectedSet[FourthItemName] + " - " + FourthItemName);

            
            if (answerset.Count == 0 || Controller.Controller.B6() && Application.loadedLevelName == "Pharmacy")
            {
                completedTimer -= Time.deltaTime;
                if (completedTimer < 0 && !completed)
                {
                    CheckForCollectionPerformance();
                    completed = true;
                }
            }

            if (FirstItemName != null && CollectedSet.ContainsKey(FirstItemName) && CollectedSet[FirstItemName] >= NumberofFirstItem)
            {
                answerset.Remove(FirstItemName);
                Debug.Log("removing First Itemname");
            }

            if (SecondItemName != null && CollectedSet.ContainsKey(SecondItemName) && CollectedSet[SecondItemName] >= NumberofSecondItem)
            {
                answerset.Remove(SecondItemName);
                Debug.Log("removing second Itemname");
            }

            if (ThirdItemName != null && CollectedSet.ContainsKey(ThirdItemName) && CollectedSet[ThirdItemName] >= NumberofThirdItem)
            {
                answerset.Remove(ThirdItemName);
                Debug.Log("removing third Itemname");
            }

            if (FourthItemName != null && CollectedSet.ContainsKey(FourthItemName) && CollectedSet[FourthItemName] >= NumberofFourthItem)
            {
                answerset.Remove(FourthItemName);
            }

            CompleteObjective();

            if (completed && Application.loadedLevelName == "City")
            {
                CanAddNextObjective = true;
            }
            base.CheckForCompletion();
        }
    }
}
