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
                NodeManager.PlayerPath.Add(PathFindingController.PathArray[PathFindingController.PathArray.Count - 1].ToString());
            if (NodeManager.PlayerPath.Count == PathFindingController.PathArray.Count)
                Performance = 100.0f;
            else
            {
                var dif = PathFindingController.PathArray.Count -
                          Mathf.Abs(NodeManager.PlayerPath.Count - PathFindingController.PathArray.Count);
                Performance = dif * 100.0f / PathFindingController.PathArray.Count;
                if (Performance < 0)
                    Performance = 0;
            }
            Debug.Log("Player path: " + NodeManager.PlayerPath.Count + " ; objectve path: " + PathFindingController.PathArray.Count + " ;Performance: " + Performance);
            NodeManager.PlayerPath.Clear();
        }

        public virtual void SetTargetlocation()
        {
            LocationHolder = GameObject.Find("Objectivelocations");
        }    
    }
}
