using UnityEngine;

namespace Assets.scripts.objectives.Collection
{
    public class CollectMultiplePOItems : CollectionObjective {
   
        public CollectMultiplePOItems(int firstitem, string firstitemname, int seconditem, string seconditemname, int distractors)
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
            name = "PostOffice";   
        }

        public CollectMultiplePOItems(int firstitem, string firstitemname, int seconditem, string seconditemname, int thirditem,
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
            name = "PostOffice";
        }
    
        public override void CheckForCompletion()
        {
            //if (collectedset.Count == 0) return;
        

            if (answerset.Count == 0 || Controller.Controller.B6() && Application.loadedLevelName == "PostOffice")
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
            }

            if (SecondItemName != null && CollectedSet.ContainsKey(SecondItemName) && CollectedSet[SecondItemName] >= NumberofSecondItem)
            {
                answerset.Remove(SecondItemName);
            }

            if (ThirdItemName != null && CollectedSet.ContainsKey(ThirdItemName) && CollectedSet[ThirdItemName] >= NumberofThirdItem)
            {
                answerset.Remove(ThirdItemName);
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
