using Assets.scripts.Locations;
using Assets.scripts.MiniMapScripts.AStar;
using UnityEngine;

namespace Assets.scripts.objectives.Location
{
    public class GoToHome : LocationObjective
    {
        public GoToHome()
        {
            description = childlist[4].InnerText;
            name = "Home";
            RequiredSceneToSpawn = 0;
            Performance = -1;
        }
        public override void SetTargetlocation()
        {
            base.SetTargetlocation();
            targetlocation = LocationHolder.transform.FindChild("Home").transform.position;
            location = TargetLocation;
        }
        public override void CheckForCompletion()
        {
            if (Application.loadedLevelName == "Home")
            {
                CheckLocationPerformance();
                completed = true;
                CanAddNextObjective = true;
            }
            base.CheckForCompletion();
        }
    }
}
