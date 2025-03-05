using Assets.scripts.Locations;
using UnityEngine;

namespace Assets.scripts.objectives.Location
{
    public class GotoShopping : LocationObjective {

        public GotoShopping()
        {
            description = childlist[0].InnerText;
            name = "SuperMarket";
            RequiredSceneToSpawn = 0;
            Performance = -1;
        }
	
        public override void SetTargetlocation()
        {
            base.SetTargetlocation();
            targetlocation = LocationHolder.transform.FindChild("SuperMarket").transform.position;
            location = TargetLocation;
        }
        public override void CheckForCompletion ()
        {
            if (Application.loadedLevelName == "SuperMarket")
            {
                CheckLocationPerformance();
                completed = true;
                CanAddNextObjective = true;
            }
            base.CheckForCompletion();
        }
    }
}
