using Assets.scripts.GUI;
using UnityEngine;

namespace Assets.scripts.objectives.Collection
{
    public class CollectSMItem : CollectionObjective {

        public CollectSMItem(int numberofitems, int distractors)
        {
            var itemName = LocationsStock.GetRandomSuperMarketItem(numberofitems);

            answerset = new System.Collections.Generic.List<string>();
            collectedset = new System.Collections.Generic.Dictionary<string, int>();
            FirstItemName = itemName;
            description = childlist[1].ChildNodes[0].InnerText;
            description = description.Replace("@", numberofitems.ToString());
            
            description = numberofitems > 1 ? description.Replace("!", language.GetObjectText(FirstItemName + "Plural")) : description.Replace("!", language.GetObjectText(FirstItemName));

            answerset.Add(FirstItemName);
            NumberofFirstItem = numberofitems;
            Distractors = distractors;
            ItemsQuantities.Add(numberofitems);
            ItemsNames.Add(itemName);
            ItemsPrices.Add(0.0f);

            name = "SuperMarket";
        }

   
        public override void CheckForCompletion()
        {
            if(Controller.Controller.B6() && Application.loadedLevelName == "SuperMarket")
            {
                completed = true;    
            }
            /*
            if (invmanager.GetCategoryList.Find(Category => Category.ToString() == "SuperMarket").Cart.ContainsKey(Itemname))
            {
                Debug.Log("Completed: " + Completed);
                if (invmanager.GetCategoryList.Find(Category => Category.ToString() == "SuperMarket").Cart[Itemname] ==
                    NumberOfItemstoCollect)
                {
                    completedTimer -= Time.deltaTime;
                    if (completedTimer < 0)
                        Completed = true;
                }
            }*/
            if (answerset.Count == 0)
            {
                completedTimer -= Time.deltaTime;
                if (completedTimer < 0 && !completed)
                {
                    completed = true;
                }
            }

            if (FirstItemName != null && CollectedSet.ContainsKey(FirstItemName) && CollectedSet[FirstItemName] >= NumberofFirstItem)
            {
                answerset.Remove(FirstItemName);
            }

            CompleteObjective();

            if (Application.loadedLevelName == "City")
            {
                CanAddNextObjective = true;
            }
            base.CheckForCompletion();
        }
    }
}
