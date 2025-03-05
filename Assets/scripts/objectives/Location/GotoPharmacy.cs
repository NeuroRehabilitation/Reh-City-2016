using Assets.scripts.Locations;
using UnityEngine;

namespace Assets.scripts.objectives.Location
{
    public class GotoPharmacy : LocationObjective{

        public GotoPharmacy()
        {
            description = childlist[2].InnerText;
            name = "Pharmacy";
            Performance = -1;
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
                CheckLocationPerformance();
                completed = true;
                CanAddNextObjective = true;
            }
            base.CheckForCompletion();
        }
    }
}
