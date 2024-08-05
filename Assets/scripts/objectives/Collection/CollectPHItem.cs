using UnityEngine;

namespace Assets.scripts.objectives.Collection
{
    public class CollectPHItem : CollectionObjective {

        public CollectPHItem(int targets, int distractors)
        {
            var itemname = LocationsStock.GetRandomPharmacyItem(targets);

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
            name = "Pharmacy";
        }

        public override void CheckForCompletion()
        {
            if (collectedset.Count == 0) return;
            
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
