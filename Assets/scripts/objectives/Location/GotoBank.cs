using Assets.scripts.bank;
using Assets.scripts.Locations;
using UnityEngine;

namespace Assets.scripts.objectives.Location
{
    public class GotoBank : LocationObjective {

        public GotoBank()
        {
            description = childlist[3].InnerText;
            name = "Bank";
            RequiredSceneToSpawn = 0;
            Performance = -1;
        }

        public override void SetTargetlocation()
        {
            base.SetTargetlocation();
            targetlocation = LocationHolder.transform.FindChild("Bank").transform.position;
            location = TargetLocation;
        }

        public override void CheckForCompletion()
        {
            if (Application.loadedLevelName == "Bank")
            {
                CheckLocationPerformance();
                completed = true;
                CanAddNextObjective = true;
            }
            base.CheckForCompletion();
        }
    }
}
