using System.Collections.Generic;
using Assets.scripts.GUI.Items.Pharmacy;
using UnityEngine;

namespace Assets.scripts.GUI.Categories
{
    public class Pharmacy : Category{
    
        public Pharmacy()
        {
            //Debug.Log("Pharmacy Category created");
            cart = new Dictionary<string, int>(); 
            cartlist = new List<Item>();
            style = new GUIStyle();
            base.SetDefaultStyle();
            style.normal.background = Resources.Load("Images/Inventory/PharmacyIcon") as Texture2D;
        }
	
        public new void AddItemToCart (string key)
        {
            if(cart==null) return;
            if(cart.ContainsKey(key))
            {
                cart[key] += 1;
            }else
            {
                cart.Add(key,1);
                switch(key)
                {
                    case "Cream":
                        item = new Cream();
                        cartlist.Add(item);
                        break;
                    case "Syrup":
                        item = new Syrup();
                        cartlist.Add(item);
                        break;
                    case "Bandaid":
                        item = new Bandaid();
                        cartlist.Add(item);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
