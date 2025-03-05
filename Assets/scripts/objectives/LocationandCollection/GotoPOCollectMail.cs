using UnityEngine;

namespace Assets.scripts.objectives.LocationandCollection
{
    public class GotoPOCollectMail : LocationandCollectionObjective {
    
        public GotoPOCollectMail(int numbertocollect, bool moreabstract)
        {
            answerset = new System.Collections.Generic.List<string>();
            if (moreabstract)
            {
                description = childlist[10].InnerText;
                answerset.Add("AdultBook");
                FirstItemName = "AdultBook";
                Abstraction = "1";
            }
            else
            {
                description = childlist[7].InnerText;
                answerset.Add("Letter");
                FirstItemName = "Letter";
                Abstraction = "0";
            }
            description = description.Replace("@",numbertocollect.ToString());
       
            numberofitemsforthisobjective = numbertocollect;
            NumberofFirstItem = numbertocollect;
            name = "PostOffice";
        }
        public override void SetTargetlocation()
        {
            base.SetTargetlocation();
            targetlocation = LocationHolder.transform.FindChild("PostOffice").transform.position;
            location = TargetLocation;
        }
        public override void CheckForCompletion()
        {
            if (Application.loadedLevelName == "PostOffice")
            {
                if (NumberofItemsCollected >= numberofitemsforthisobjective || global::Assets.scripts.Controller.Controller.B6())
                {
                    completedTimer -= Time.deltaTime;
                    if (completedTimer < 0)
                        completed = true;
                }
            }

            CompleteObjective();

            if (completed && Application.loadedLevelName == "City")
            {
                CanAddNextObjective = true;
            }
            base.CheckForCompletion();
        }
	
    }
}
