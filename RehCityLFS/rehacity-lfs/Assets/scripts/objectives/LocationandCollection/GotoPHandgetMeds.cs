using UnityEngine;

namespace Assets.scripts.objectives.LocationandCollection
{
    public class GotoPHandgetMeds : LocationandCollectionObjective {
    
        public GotoPHandgetMeds(int numberofitems,bool moreabstract)
        {
            answerset = new System.Collections.Generic.List<string>();
            if (moreabstract)
            {
                description = childlist[9].InnerText;
                answerset.Add("Benuron");
                answerset.Add("Aspirin");
                FirstItemName = "Benuron-Aspirin";
                Abstraction = "1";
            }
            else
            {
                description = childlist[6].InnerText;
                answerset.Add("Bandaid");
                FirstItemName = "Bandaid";
                Abstraction = "0";
            }
            description = description.Replace("@", numberofitems.ToString());
       
            numberofitemsforthisobjective = numberofitems;
            NumberofFirstItem = numberofitems;
            
            name = "Pharmacy";
        }

        public override void SetTargetlocation()
        {
            base.SetTargetlocation();
            targetlocation = LocationHolder.transform.FindChild("Pharmacy").transform.position;
            location = TargetLocation;
        }
        public override void CheckForCompletion()
        {
            if (Application.loadedLevelName == "Pharmacy")
            {
                if (Controller.Controller.B6())
                {
                    completed = true;
                }

                if (NumberofItemsCollected >= numberofitemsforthisobjective)
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
