using System.Collections.Generic;
using Assets.scripts.GUI.Items.PostOffice;
using UnityEngine;

namespace Assets.scripts.GUI.Categories
{
    public class PostOffice : Category {

        public PostOffice()
        {
            cart = new Dictionary<string, int>();
            cartlist = new List<Item>();
            style = new GUIStyle();
            base.SetDefaultStyle();
            style.normal.background = Resources.Load("Images/Inventory/PostOffice") as Texture2D;
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
                switch (key)
                {
                    case "Package":
                        item = new Package();
                        cartlist.Add(item);
                        break;
                    case "Letter":
                        item = new Letter();
                        cartlist.Add(item);
                        break;
                    case "Stamp":
                        item = new Stamp();
                        cartlist.Add(item);
                        break;
                    default:
                        break;
                }
            }
        }	
    }
}
