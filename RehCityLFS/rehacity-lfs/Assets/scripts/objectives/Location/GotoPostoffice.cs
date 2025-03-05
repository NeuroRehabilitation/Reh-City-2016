using Assets.scripts.Locations;
using UnityEngine;

namespace Assets.scripts.objectives.Location
{
    public class GotoPostoffice : LocationObjective {

        public GotoPostoffice()
        {
            description = childlist[1].InnerText;
            name = "PostOffice";
            RequiredSceneToSpawn = 0;
            Performance = -1;
        }
	
        public override void SetTargetlocation()
        {
            base.SetTargetlocation();
            targetlocation = LocationHolder.transform.FindChild("PostOffice").transform.position;
            location = TargetLocation;
        }
	
        public override void CheckForCompletion ()
        {
            if (Application.loadedLevelName == "PostOffice")
            {
                CheckLocationPerformance();
                completed = true;
                CanAddNextObjective = true;
            }
            base.CheckForCompletion();
        }	
    }
}
