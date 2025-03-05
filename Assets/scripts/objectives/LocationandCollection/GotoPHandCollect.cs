using UnityEngine;

namespace Assets.scripts.objectives.LocationandCollection
{
    public class GotoPHandCollect : LocationandCollectionObjective {
    
        public GotoPHandCollect(int firstitem, string firstitemname, int seconditem, string seconditemname, int distractors)
        {
            answerset = new System.Collections.Generic.List<string>();
            description = childlist[1].InnerText;
            description = description.Replace("@", firstitem.ToString());

            description = description.Replace("#", firstitem == 1 ? language.GetObjectText(firstitemname) : language.GetObjectText(firstitemname + "Plural"));

            description = description.Replace("!", seconditem.ToString());

            description = description.Replace("$", seconditem == 1 ? language.GetObjectText(seconditemname) : language.GetObjectText(seconditemname + "Plural"));

            FirstItemName = firstitemname;
            SecondItemName = seconditemname;
            Distractors = distractors;
            answerset.Add(firstitemname);
            answerset.Add(seconditemname);
            if (invmanager.GetCategoryList.Find(Category => Category.ToString() == "Pharmacy").Cart.ContainsKey(firstitemname))
            {
                NumberofFirstItem = invmanager.GetCategoryList.Find(Category => Category.ToString() == "Pharmacy").Cart[firstitemname] + firstitem;
            }
            else
            {
                NumberofFirstItem = firstitem;
            }
            if (invmanager.GetCategoryList.Find(Category => Category.ToString() == "Pharmacy").Cart.ContainsKey(seconditemname))
            {
                NumberofSecondItem = invmanager.GetCategoryList.Find(Category => Category.ToString() == "Pharmacy").Cart[seconditemname] + seconditem;
            }
            else
            {
                NumberofSecondItem = seconditem;
            }
            name = "Pharmacy";
        }

        public override void SetTargetlocation()
        {
            base.SetTargetlocation();
            targetlocation = LocationHolder.transform.FindChild("Pharmacy").transform.position;
            location = TargetLocation;
        }

        public override void CheckForCompletion()
        {
            if (Application.loadedLevelName == "Pharmacy")
            {
                if(Controller.Controller.B6())
                {
                    completed = true;  
                }

                if (invmanager.GetCategoryList.Find(Category => Category.ToString() == "Pharmacy").Cart.ContainsKey(FirstItemName) &&
                    invmanager.GetCategoryList.Find(Category => Category.ToString() == "Pharmacy").Cart.ContainsKey(SecondItemName))
                {
                    if (invmanager.GetCategoryList.Find(Category => Category.ToString() == "Pharmacy").Cart[FirstItemName] >= NumberofFirstItem &&
                        invmanager.GetCategoryList.Find(Category => Category.ToString() == "Pharmacy").Cart[SecondItemName] >= NumberofSecondItem)
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
