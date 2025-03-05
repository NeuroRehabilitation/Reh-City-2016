using Assets.scripts.Manager;
using UnityEngine;

namespace Assets.scripts.objectives.LocationandAction
{
    public class LocationandActionObjective : Objectives {

        protected GameObject LocationHolder;
        protected Vector3 targetlocation;
        public Vector3 TargetLocation
        {
            get
            {
                return targetlocation;
            }
        }

        //protected string ButtonToPress;
        protected LanguageManager Language;
        public LocationandActionObjective()
        {
            childlist = nodelist[4].ChildNodes;
            SetTargetlocation();
            Language = LanguageManager.Instance;
        }
        public virtual void SetTargetlocation()
        {
            LocationHolder = GameObject.Find("Objectivelocations");
        }
    }
}
