using UnityEngine;

namespace Assets.scripts.objectives.LocationandCollection
{
    public class GotoSMandgetFood : LocationandCollectionObjective
    {
        public GotoSMandgetFood(int numberofitems, string meal)
        {
            answerset = new System.Collections.Generic.List<string>();
            if (meal == "Breakfast")
            {
                description = childlist[3].InnerText;
                answerset.Add("Milk");
                answerset.Add("Bread");
                answerset.Add("Juice");
                FirstItemName = "Milk-Bread-Juice";
            }
            else if (meal == "Lunch")
            {
                description = childlist[4].InnerText;
                answerset.Add("Bread");
                answerset.Add("Pasta");
                answerset.Add("Sauce");
                FirstItemName = "Bread-Pasta-Sauce";
            }
            else if (meal == "Snack")
            {
                description = childlist[5].InnerText;
                answerset.Add("Orange");
                answerset.Add("Apple");
                answerset.Add("Yogurt");
                FirstItemName = "Orange-Apple-Yogurt";
            }
            description = description.Replace("@", numberofitems.ToString());
            numberofitemsforthisobjective = numberofitems;
            NumberofFirstItem = numberofitems;
            Abstraction = meal;
            name = "SuperMarket";
        }
        public override void SetTargetlocation()
        {
            base.SetTargetlocation();
            targetlocation = LocationHolder.transform.FindChild("SuperMarket").transform.position;
            location = TargetLocation;
        }

        public override void CheckForCompletion()
        {
            if (Application.loadedLevelName == "SuperMarket")
            {
                if(NumberofItemsCollected>=numberofitemsforthisobjective || global::Assets.scripts.Controller.Controller.B6())
                {
                    completedTimer -= Time.deltaTime;
                    if (completedTimer < 0)
                        completed = true;
                }

                CompleteObjective();
            }
            if (completed && Application.loadedLevelName == "City")
            {
                CanAddNextObjective = true;
            }
            base.CheckForCompletion();
        }
    }
}
