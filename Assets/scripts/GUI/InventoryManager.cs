using System.Collections.Generic;
using Assets.scripts.GUI.Categories;
using UnityEngine;

/* Attach this class to any gameobject 
 * This class will add the different categories that has its own cart.
 * Look into Category class to see what it does after spawning
 * Coder:Kushal
*/

namespace Assets.scripts.GUI
{
    public class InventoryManager : MonoBehaviour
    {

        // all the Category types which will be used when adding Category to inventory
        public enum CategoryTypes
        {
            SuperMarket,
            Pharmacy,
            InCity,
            PostOffice
        }

        // variables for all the categories
        public SuperMarket mallcategory;
        public Pharmacy pharmacycategory;
        public InCity incitycategory;
        public PostOffice postofficecategory;

        // list to contain the Sequence in which categories are spawned
        private List<Category> CategoryList;

        // readonly property to get Category list
        public List<Category> GetCategoryList
        {
            get
            {
                return CategoryList;
            }
        }

        private static InventoryManager s_Instance = null;
        public static InventoryManager Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindObjectOfType(typeof(InventoryManager)) as InventoryManager;
                    if (s_Instance == null)
                    {
                        Debug.Log("Could not locate InventoryManager");
                    }
                }
                return s_Instance;
            }
        }
        void Start()
        {
            CategoryList = new List<Category>();
        }


        // use this method to add any Type of Category to inventory
        // this also adds the Category to categorylist
        // which will contain the Sequence in which they were added.
        public void AddCategory(CategoryTypes categoryname)
        {
            switch (categoryname)
            {
                case CategoryTypes.SuperMarket:
                    if (mallcategory == null)
                    {
                        mallcategory = new SuperMarket();
                        CategoryList.Add(mallcategory);
                    }
                    break;
                case CategoryTypes.Pharmacy:
                    if (pharmacycategory == null)
                    {
                        pharmacycategory = new Pharmacy();
                        CategoryList.Add(pharmacycategory);
                    }
                    break;
                case CategoryTypes.InCity:
                    if (incitycategory == null)
                    {
                        incitycategory = new InCity();
                        CategoryList.Add(incitycategory);
                    }
                    break;
                case CategoryTypes.PostOffice:
                    if (postofficecategory == null)
                    {
                        postofficecategory = new PostOffice();
                        CategoryList.Add(postofficecategory);
                    }
                    break;
                default:
                    break;
            }

        }

        public bool hasAtleastOne(string itemtype, string objname)
        {
            switch (itemtype)
            {
                case "SuperMarket":
                    if (mallcategory != null)
                    {
                        return mallcategory.Cart[objname] == 1;
                    }
                    return false;
                case "Utensils":
                    if (pharmacycategory != null)
                    {

                    }
                    return false;
                case "InCity":
                    if (incitycategory != null)
                    {

                    }
                    return false;
                case "Post":
                    if (postofficecategory != null)
                    {

                    }
                    return false;
                default:
                    return false;
            }

        }

        public void AddItem(string itemtype, string objname)
        {
            switch (itemtype)
            {
                case "SuperMarket":
                    if (mallcategory != null)
                    {
                        mallcategory.AddItemToCart(objname);
                    }
                    else
                    {
                        Debug.Log("Cannot be shopped without SuperMarket Category");
                    }
                    break;
                case "Pharmacy":
                    if (pharmacycategory != null)
                    {
                        pharmacycategory.AddItemToCart(objname);
                    }
                    else
                    {
                        Debug.Log("Cannot be Shopped without pharmacy Category");
                    }
                    break;
                case "InCity":
                    if (incitycategory != null)
                    {

                    }
                    else
                    {

                    }
                    break;
                case "PostOffice":
                    if (postofficecategory != null)
                    {
                        postofficecategory.AddItemToCart(objname);
                    }
                    else
                    {
                        Debug.Log("Cannot be Shopped without postoffice Category");
                    }
                    break;
                default:
                    break;
            }
        }


        void Update()
        {
            // Hot Keys
            if (Input.GetKeyDown(KeyCode.M))
            {
                AddCategory(CategoryTypes.SuperMarket);
            }
            if (Input.GetKeyDown(KeyCode.T))
            {

            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                mallcategory.AddItemToCart("Juice");
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                mallcategory.AddItemToCart("Chocapic");
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                AddCategory(CategoryTypes.Pharmacy);

            }
        }


    }
}
