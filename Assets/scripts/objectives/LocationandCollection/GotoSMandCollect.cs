using UnityEngine;

namespace Assets.scripts.objectives.LocationandCollection
{
    public class GotoSMandCollect : LocationandCollectionObjective {

        public GotoSMandCollect(int firstitem, string firstitemname, int seconditem, string seconditemname, int distractors)
        {
            answerset = new System.Collections.Generic.List<string>();
            description = childlist[0].InnerText;
            description = description.Replace("@", firstitem.ToString());
            description = description.Replace("#", firstitem == 1 ? language.GetObjectText(firstitemname) : language.GetObjectText(firstitemname + "Plural"));
            description = description.Replace("!", seconditem.ToString());
            description = description.Replace("$", seconditem == 1 ? language.GetObjectText(seconditemname) : language.GetObjectText(seconditemname + "Plural"));
            FirstItemName = firstitemname;
            SecondItemName = seconditemname;
            answerset.Add(firstitemname);
            answerset.Add(seconditemname);
            Distractors = distractors;
            if (invmanager.GetCategoryList.Find(Category => Category.ToString() == "SuperMarket").Cart.ContainsKey(firstitemname))
            {
                NumberofFirstItem = invmanager.GetCategoryList.Find(Category => Category.ToString() == "SuperMarket").Cart[firstitemname] + firstitem;
            }
            else
            {
                NumberofFirstItem = firstitem;
            }
            if (invmanager.GetCategoryList.Find(Category => Category.ToString() == "SuperMarket").Cart.ContainsKey(seconditemname))
            {
                NumberofSecondItem = invmanager.GetCategoryList.Find(Category => Category.ToString() == "SuperMarket").Cart[seconditemname] + seconditem;
            }
            else
            {
                NumberofSecondItem = seconditem;
            }
            name = "SuperMarket";
        }
        public override void SetTargetlocation()
        {
            base.SetTargetlocation();
            targetlocation = LocationHolder.transform.FindChild("SuperMarket").transform.position;
            location = TargetLocation;
        }
        public override void CheckForCompletion()
        {
            if (Application.loadedLevelName == "SuperMarket")
            {
                if(Controller.Controller.B6())
                {
                    completed = true;
                }

                if (invmanager.GetCategoryList.Find(Category => Category.ToString() == "SuperMarket").Cart.ContainsKey(FirstItemName) &&
                    invmanager.GetCategoryList.Find(Category => Category.ToString() == "SuperMarket").Cart.ContainsKey(SecondItemName))
                {
                    if (invmanager.GetCategoryList.Find(Category => Category.ToString() == "SuperMarket").Cart[FirstItemName] >= NumberofFirstItem &&
                        invmanager.GetCategoryList.Find(Category => Category.ToString() == "SuperMarket").Cart[SecondItemName] >= NumberofSecondItem)
                    {
                        completedTimer -= Time.deltaTime;
                        if (completedTimer < 0)
                            completed = true;
                    }
                }

                CompleteObjective();
            }
            if (completed && Application.loadedLevelName == "City")
            {
                CanAddNextObjective = true;
            }
            base.CheckForCompletion();
        }

    }
}
