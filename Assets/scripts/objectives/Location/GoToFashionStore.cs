using Assets.scripts.Locations;
using UnityEngine;

namespace Assets.scripts.objectives.Location
{
    public class GoToFashionStore : LocationObjective
    {
        public GoToFashionStore()
        {
            description = childlist[5].InnerText;
            name = "FashionStore";
            RequiredSceneToSpawn = 0;
            Performance = -1;
        }
        public override void SetTargetlocation()
        {
            base.SetTargetlocation();
            targetlocation = LocationHolder.transform.FindChild("FashionStore").transform.position;
            location = TargetLocation;
        }
        public override void CheckForCompletion()
        {
            if (Application.loadedLevelName == "FashionStore")
            {
                CheckLocationPerformance();
                completed = true;
                CanAddNextObjective = true;
            }
            base.CheckForCompletion();
        }
    }
}
