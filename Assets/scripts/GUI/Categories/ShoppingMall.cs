using System.Collections.Generic;
using UnityEngine;

namespace Assets.scripts.GUI.Categories
{
    public class SuperMarket : Category {

        public SuperMarket()
        {
            //Debug.Log("SuperMarket Category created");
            cart = new Dictionary<string, int>(); 
            cartlist = new List<Item>();
            style = new GUIStyle();
            base.SetDefaultStyle();
            style.normal.background = Resources.Load("Images/Inventory/Supermarket") as Texture2D;
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
                /*switch(key)
			{
			case "Milk":
				Item = new Milk();
				cartlist.Add(Item);
				break;
			case "Juice":
				Item = new Juice();
				cartlist.Add(Item);
				break;
			case "Chocapic":
				Item = new Chocapic();
				cartlist.Add(Item);
				break;
            case "Water":
                Item = new Water();
                cartlist.Add(Item);
                break;
            case "Bread":
                Item = new Bread();
                cartlist.Add(Item);
                break;
            case "Kellogs":
                Item = new Kellogs();
                cartlist.Add(Item);
                break;
			default:
				break;
			}*/
            }
        }
	
	
    }
}
