using System.Collections.Generic;
using Assets.scripts.GUI;
using UnityEngine;

namespace Assets.scripts.objectives.Collection
{
    public class CollectMultipleSMItems : CollectionObjective {
    
        public CollectMultipleSMItems(int firstitem, int seconditem,int distractors)
        {
            var items = new List<string>();
            var firstitemname = LocationsStock.GetRandomSuperMarketItem(firstitem);

            items.Add(firstitemname);
            
            string seconditemname;
            do
            {
                seconditemname = LocationsStock.GetRandomSuperMarketItem(seconditem);
            } while (items.Contains(seconditemname));

            answerset = new System.Collections.Generic.List<string>();
            collectedset = new System.Collections.Generic.Dictionary<string, int>();
            description = childlist[2].InnerText;
            description = description.Replace("@", firstitem.ToString());
            description = firstitem > 1 ? description.Replace("#", language.GetObjectText(firstitemname + "Plural")) : description.Replace("#", language.GetObjectText(firstitemname));
            description = description.Replace("!", seconditem.ToString());
            description = seconditem > 1 ? description.Replace("$", language.GetObjectText(seconditemname + "Plural")) : description.Replace("$", language.GetObjectText(seconditemname));
            Distractors = distractors;
            FirstItemName = firstitemname;
            SecondItemName = seconditemname;
            answerset.Add(firstitemname);
            answerset.Add(seconditemname);

            NumberofFirstItem = firstitem;

            NumberofSecondItem = seconditem;


            ItemsQuantities.Add(NumberofFirstItem);
            ItemsQuantities.Add(NumberofSecondItem);

            ItemsNames.Add(FirstItemName);
            ItemsNames.Add(SecondItemName);

            for (var i = 0; i < 2; i++)
            {
                ItemsPrices.Add(0.0f);
            }

            name = "SuperMarket";
        }

        public CollectMultipleSMItems(int firstitem, int seconditem, int thirditem, int distractors)
        {
            var items = new List<string>();
            var firstitemname = LocationsStock.GetRandomSuperMarketItem(firstitem);
            items.Add(firstitemname);

            string seconditemname;
            do
            {
                seconditemname = LocationsStock.GetRandomSuperMarketItem(seconditem);
            } while (items.Contains(seconditemname));
            items.Add(seconditemname);

            string thirditemname;
            do
            {
                thirditemname = LocationsStock.GetRandomSuperMarketItem(thirditem);
            } while (items.Contains(thirditemname));
            
            answerset = new System.Collections.Generic.List<string>();
            collectedset = new System.Collections.Generic.Dictionary<string, int>();
            description = childlist[3].InnerText;
            description = description.Replace("@", firstitem.ToString());

            Distractors = distractors;
            description = firstitem > 1 ? description.Replace("#", language.GetObjectText(firstitemname + "Plural")) : description.Replace("#", language.GetObjectText(firstitemname));

            description = description.Replace("!", seconditem.ToString());


            description = seconditem > 1 ? description.Replace("$", language.GetObjectText(seconditemname + "Plural")) : description.Replace("$", language.GetObjectText(seconditemname));

            description = description.Replace("%", thirditem.ToString());
            
            description = thirditem > 1 ? description.Replace("^", language.GetObjectText(thirditemname + "Plural")) : description.Replace("^", language.GetObjectText(thirditemname));

            FirstItemName = firstitemname;
            SecondItemName = seconditemname;
            ThirdItemName = thirditemname;

            answerset.Add(firstitemname);
            answerset.Add(seconditemname);
            answerset.Add(thirditemname);

            NumberofFirstItem = firstitem;
            NumberofSecondItem = seconditem;
            NumberofThirdItem = thirditem;


            ItemsQuantities.Add(NumberofFirstItem);
            ItemsQuantities.Add(NumberofSecondItem);
            ItemsQuantities.Add(NumberofThirdItem);

            ItemsNames.Add(FirstItemName);
            ItemsNames.Add(SecondItemName);
            ItemsNames.Add(ThirdItemName);

            for (var i = 0; i < 3; i++)
            {
                ItemsPrices.Add(0.0f);
            }

            name = "SuperMarket";
        }

        public CollectMultipleSMItems(int firstitem, int seconditem, int thirditem, int fourthitem, int distractors)
        {
            var items = new List<string>();
            var firstitemname = LocationsStock.GetRandomSuperMarketItem(firstitem);
            items.Add(firstitemname);

            string seconditemname;
            do
            {
                seconditemname = LocationsStock.GetRandomSuperMarketItem(seconditem);
            } while (items.Contains(seconditemname));
            items.Add(seconditemname);

            string thirditemname;
            do
            {
                thirditemname = LocationsStock.GetRandomSuperMarketItem(thirditem);
            } while (items.Contains(thirditemname));
            items.Add(thirditemname);

            string fourthitemname;
            do
            {
                fourthitemname = LocationsStock.GetRandomSuperMarketItem(fourthitem);
            } while (items.Contains(fourthitemname));
            
            answerset = new System.Collections.Generic.List<string>();
            collectedset = new System.Collections.Generic.Dictionary<string, int>();
            description = childlist[3].InnerText;
            description = description.Replace("@", firstitem.ToString());

            Distractors = distractors;
            description = firstitem > 1 ? description.Replace("#", language.GetObjectText(firstitemname + "Plural")) : description.Replace("#", language.GetObjectText(firstitemname));

            description = description.Replace("!", seconditem.ToString());
            

            description = seconditem > 1 ? description.Replace("$", language.GetObjectText(seconditemname + "Plural")) : description.Replace("$", language.GetObjectText(seconditemname));
            
            description = description.Replace("%",thirditem.ToString());
            

            description = thirditem > 1 ? description.Replace("^", language.GetObjectText(thirditemname + "Plural")) : description.Replace("^", language.GetObjectText(thirditemname));

            description = description.Replace("|",fourthitem.ToString());
            

            description = fourthitem >1 ? description.Replace("*", language.GetObjectText(fourthitemname + "Plural")) : description.Replace("*", language.GetObjectText(fourthitemname));

            FirstItemName = firstitemname;
            SecondItemName = seconditemname;
            ThirdItemName = thirditemname;
            FourthItemName = fourthitemname;

            answerset.Add(firstitemname);
            answerset.Add(seconditemname);
            answerset.Add(thirditemname);
            answerset.Add(fourthitemname);
        
            NumberofFirstItem = firstitem;
            NumberofSecondItem = seconditem;
            NumberofThirdItem = thirditem;
            NumberofFourthItem = fourthitem;
            

            ItemsQuantities.Add(NumberofFirstItem);
            ItemsQuantities.Add(NumberofSecondItem);
            ItemsQuantities.Add(NumberofThirdItem);
            ItemsQuantities.Add(NumberofFourthItem);

            ItemsNames.Add(FirstItemName);
            ItemsNames.Add(SecondItemName);
            ItemsNames.Add(ThirdItemName);
            ItemsNames.Add(FourthItemName);

            for (var i = 0; i < 4; i++)
            {
                ItemsPrices.Add(0.0f);
            }

            name = "SuperMarket";
        }

        public override void CheckForCompletion()
        {
            if (collectedset.Count == 0) return;
            
            if (answerset.Count == 0 || Controller.Controller.B6() && Application.loadedLevelName == "SuperMarket")
            {
                completedTimer -= Time.deltaTime;
                if (completedTimer < 0 && !completed)
                {
                    completed = true;
                }
            }
            if (FirstItemName!=null && CollectedSet.ContainsKey(FirstItemName) &&CollectedSet[FirstItemName] >= NumberofFirstItem)
            {
                answerset.Remove(FirstItemName);
            }
            if (SecondItemName != null && CollectedSet.ContainsKey(SecondItemName) && CollectedSet[SecondItemName] >= NumberofSecondItem)
            {
                answerset.Remove(SecondItemName);
            }
            if (ThirdItemName != null && CollectedSet.ContainsKey(ThirdItemName) && CollectedSet[ThirdItemName] >= NumberofThirdItem)
            {
                answerset.Remove(ThirdItemName);
            }
            if (FourthItemName != null && CollectedSet.ContainsKey(FourthItemName) && CollectedSet[FourthItemName] >= NumberofFourthItem)
            {
                answerset.Remove(FourthItemName);
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
