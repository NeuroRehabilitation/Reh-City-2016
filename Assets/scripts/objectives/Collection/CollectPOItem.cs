using UnityEngine;
using Assets.scripts.Controller;
using Assets.scripts.GUI;

namespace Assets.scripts.objectives.Collection
{
    public class CollectPOItem : CollectionObjective {
    
        public CollectPOItem(int targets, int distractors)
        {
            var itemname = LocationsStock.GetRandomPostOfficeItem(targets);

            answerset = new System.Collections.Generic.List<string>();
            collectedset = new System.Collections.Generic.Dictionary<string, int>();
            FirstItemName = itemname;
            description = childlist[1].ChildNodes[0].InnerText;
            description = description.Replace("@", targets.ToString());
            description = targets > 1 ? description.Replace("!", language.GetObjectText(FirstItemName + "Plural")) : description.Replace("!", language.GetObjectText(FirstItemName));
            Distractors = distractors;
            answerset.Add(FirstItemName);
            NumberofFirstItem = targets;
            Performance = -1;
            name = "PostOffice";

        }
        public override void CheckForCompletion()
        {
            if (Controller.Controller.B6() && Application.loadedLevelName == "PostOffice")
            {
                completed = true;   
            }

            if (answerset.Count == 0)
            {
                completedTimer -= Time.deltaTime;
                if (completedTimer < 0 && !completed)
                {
                    ObjTime = DrawObjectiveList.TaskTime;
                    CheckForCollectionPerformance();
                    completed = true;
                }
            }

            if (FirstItemName != null && CollectedSet.ContainsKey(FirstItemName) && CollectedSet[FirstItemName] >= NumberofFirstItem)
            {
                answerset.Remove(FirstItemName);
            }

            CompleteObjective();

            if (Application.loadedLevelName == "City")
            {
                CanAddNextObjective = true;
            }
            base.CheckForCompletion();
        }
    }
}
