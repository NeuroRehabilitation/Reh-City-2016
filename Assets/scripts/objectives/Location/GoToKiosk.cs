using Assets.scripts.Locations;
using UnityEngine;

namespace Assets.scripts.objectives.Location
{
    public class GoToKiosk : LocationObjective
    {
        public GoToKiosk()
        {
            description = childlist[6].InnerText;
            name = "Kiosk";
            RequiredSceneToSpawn = 0;
            Performance = -1;
        }
        public override void SetTargetlocation()
        {
            base.SetTargetlocation();
            targetlocation = LocationHolder.transform.FindChild("Kiosk").transform.position;
            location = TargetLocation;
        }
        public override void CheckForCompletion()
        {
            if (Application.loadedLevelName == "Kiosk")
            {
                CheckLocationPerformance();
                completed = true;
                CanAddNextObjective = true;
            }
            base.CheckForCompletion();
        }
    }
}
