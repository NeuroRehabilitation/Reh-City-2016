using Assets.scripts.Controller;
using Assets.scripts.Manager;
using Assets.scripts.MiniMapScripts.AStar;
using UnityEngine;

namespace Assets.scripts.objectives.Location
{
    public class LocationObjective : Objectives {

        protected GameObject LocationHolder;
        protected Vector3 targetlocation;
        protected LanguageManager Language;
  
        public Vector3 TargetLocation
        {
            get
            {
                return targetlocation;
            }
        }
	
        public LocationObjective()
        {
            childlist = nodelist[0].ChildNodes;  
            //invmanager = InventoryManager.Instance;
            Language = LanguageManager.Instance;
            SetTargetlocation();
        }

        public void CheckLocationPerformance()
        {
            if (NodeManager.PlayerPath[NodeManager.PlayerPath.Count - 1] != PathFindingController.GoalNodeName)
                NodeManager.PlayerPath.Add(PathFindingController.InitialObjPath[PathFindingController.InitialObjPath.Count - 1].ToString());

            if (NodeManager.PlayerPath.Count <= PathFindingController.InitialObjPath.Count)
                Performance = 100.0f;
            else
            {
                var dif = PathFindingController.InitialObjPath.Count -
                          Mathf.Abs(NodeManager.PlayerPath.Count - PathFindingController.InitialObjPath.Count);
                Performance = dif * 100.0f / PathFindingController.InitialObjPath.Count;
                if (Performance < 0)
                    Performance = 0;
            }

            if(NavigationTimer.NavTimer > 10000)
                Performance = 0;
            //Debug.Log("Player path: " + NodeManager.PlayerPath.Count + " ; objectve path: " + PathFindingController.InitialObjPath.Count + " ;Performance: " + Performance);

            TaskSummary.SaveTaskSummary(name, PathFindingController.InitialObjPath.Count,
                NodeManager.PlayerPath.Count, 0, 0, 0, 0, Performance, ObjTime, 0);
            LoadSaveSettings.LastLocationPerformance = Performance;
            LoadSaveSettings.SaveSettingsInfo("GoTo" + name);
            NodeManager.PlayerPath.Clear();
        }

        public virtual void SetTargetlocation()
        {
            LocationHolder = GameObject.Find("Objectivelocations");
        }    
    }
}
