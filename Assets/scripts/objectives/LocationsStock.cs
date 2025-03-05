using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LocationsStock : MonoBehaviour {

    public static Dictionary<string, int> PharmacyItems = new Dictionary<string, int>() {
            {"Cream", 5},
            {"Syrup", 5},
            {"Bandaid", 5},
            {"Aspirin", 5},
            {"Betadin", 3},
            {"Sunscreen", 4},
            {"Benuron", 5},
            {"Brufen", 4},
    };

    public static Dictionary<string, int> PostOfficeItems = new Dictionary<string, int>() {
            {"Stamp", 5},
            {"Letter", 5},
            {"Package", 4},
            {"ChildrenBook", 5},
            {"AdultBook", 5},
            {"Postcard", 5},
            {"CDsBox", 3},
            {"Lottery", 4},
    };

    public static Dictionary<string, int> SuperMarketItems = new Dictionary<string, int>() {
            {"Milk", 6},
            {"Juice", 3},
            {"Chocapic", 2},
            {"Bread", 6},
            {"Water", 6},
            {"Kellogs", 2},
            {"Coke", 3},
            {"Apple", 6},
            {"Orange", 6},
            {"Nutella", 2},
            {"Shampoo", 2},
            {"Coffee", 2},
            {"Sauce", 2},
            {"Yogurt", 5},
            {"Butter", 2},
            {"Pasta", 3},
            {"OliveOil", 2},
    };

    /// <summary>
    /// Get a random item name from Pharmacy
    /// </summary>
    /// <param name="quant">quantity required to check availability</param>
    /// <returns>a random item name wich quantity matches the requirements</returns>
    public static string GetRandomPharmacyItem(int quant)
    {
        var item = "";
        do
        {
            var random = Random.Range(0, PharmacyItems.Count);
            if (PharmacyItems.Values.ElementAt(random) >= quant)
            {
                item = PharmacyItems.Keys.ElementAt(random);   
            }
        } while (item == "");

        return item;
    }

    /// <summary>
    /// Get a random item name from PostOffice
    /// </summary>
    /// <param name="quant">quantity required to check availability</param>
    /// <returns>a random item name wich quantity matches the requirements</returns>
    public static string GetRandomPostOfficeItem(int quant)
    {
        var item = "";
        do
        {
            var random = Random.Range(0, PostOfficeItems.Count);
            if (PostOfficeItems.Values.ElementAt(random) >= quant)
            {
                item = PostOfficeItems.Keys.ElementAt(random);
            }
        } while (item == "");

        return item;
    }

    /// <summary>
    /// Get a random item name from Supermarket
    /// </summary>
    /// <param name="quant">quantity required to check availability</param>
    /// <returns>a random item name wich quantity matches the requirements</returns>
    public static string GetRandomSuperMarketItem(int quant)
    {
        var item = "";
        do
        {
            var random = Random.Range(0, SuperMarketItems.Count);
            if (SuperMarketItems.Values.ElementAt(random) >= quant)
            {
                item = SuperMarketItems.Keys.ElementAt(random);
            }
        } while (item == "");

        return item;
    }

    /// <summary>
    /// Obtain the maximum values of items in scene
    /// lines determines the number of different items (lines in the receipt if SM)
    /// </summary>
    /// <returns>[0] -> targets; [1] -> distractors</returns>
    public static List<int> GetLocationCancellationItems(string location, int elements, int prob, int lines)
    {
        var cancellationValues = new List<int>();
        var maxItem = 0;
        var totalItems = 0;
        
        if (location == "SuperMarket")
        {
            for (var i = 0; i < SuperMarketItems.Count; i++)
            {
                totalItems += SuperMarketItems.Values.ElementAt(i);
                if (SuperMarketItems.Values.ElementAt(i) > maxItem)
                    maxItem = SuperMarketItems.Values.ElementAt(i);
            }
        }
        else if (location == "PostOffice")
        {
            for (var i = 0; i < PostOfficeItems.Count; i++)
            {
                totalItems += PostOfficeItems.Values.ElementAt(i);
                if (PostOfficeItems.Values.ElementAt(i) > maxItem)
                    maxItem = PostOfficeItems.Values.ElementAt(i);
            }
        }
        else if (location == "Pharmacy")
        {
            for (var i = 0; i < PharmacyItems.Count; i++)
            {
                totalItems += PharmacyItems.Values.ElementAt(i);
                if (PharmacyItems.Values.ElementAt(i) > maxItem)
                    maxItem = PharmacyItems.Values.ElementAt(i);
            }
        }

        //scale the returned values from the model to the scene existing ones (model returns elements between 3 and 1521)
        //rehacity can vary from 1 to totalItems in scene
        //529 is the maximum number of elements returned from the model when profile has everything as 10
        
        var locationsRange = totalItems - 3;

        var elementsScaled = (int)Mathf.Round((elements - 3) * locationsRange / 529 + 3);
        if (elementsScaled < 3)
            elementsScaled = 3;

        elementsScaled = elementsScaled * lines;

        if (elementsScaled > totalItems)
            elementsScaled = totalItems;

        var percentage = prob / 100.0f;
        
        var targets = (int) (elementsScaled * percentage);

        if (targets > maxItem)
            targets = maxItem;

        if (targets < 1)
            targets = 1;

        var distractors = elementsScaled - targets;
        var multipleTargets = 0;

        if (lines == 1)
        {
            cancellationValues.Add(targets);
            cancellationValues.Add(distractors);
        }
        else
        {
            for (var i = 0; i < lines; i++)
            {
                var rand = Random.Range(1, targets);
                cancellationValues.Add(rand);
                multipleTargets += rand;
            }
            distractors = elementsScaled - multipleTargets;
            cancellationValues.Add(distractors);
        }

        return cancellationValues;
    }
}
