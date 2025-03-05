using Assets.scripts.GUI;
using Assets.scripts.Manager;
using UnityEngine;

namespace Assets.scripts.objectives.LocationandCollection
{
    public class LocationandCollectionObjective : Objectives {

        protected GameObject LocationHolder;
        protected Vector3 targetlocation;
        public Vector3 TargetLocation
        {
            get
            {
                return targetlocation;
            }
        }

        protected int numberofitemsforthisobjective = 0;
        protected LanguageManager language;
        //protected int NumberofFirstItem = 0;
        //protected int NumberofSecondItem = 0;
        protected InventoryManager invmanager;
        public LocationandCollectionObjective()
        {
            childlist = nodelist[3].ChildNodes;  
            SetTargetlocation();
            language = LanguageManager.Instance;
            invmanager = InventoryManager.Instance;
        }
        public virtual void SetTargetlocation()
        {
            LocationHolder = GameObject.Find("Objectivelocations");
        }
    }
}
