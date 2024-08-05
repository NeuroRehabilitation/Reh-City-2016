using System.Collections.Generic;
using UnityEngine;

namespace Assets.scripts.GUI.Categories
{
    public class Utensils : Category {
     
        public Utensils()
        {
            Debug.Log("Utensils Category created");
            cart = new Dictionary<string, int>(); 
            style = new GUIStyle();
            base.SetDefaultStyle();
            style.normal.background = Resources.Load("Images/Inventory/spoon") as Texture2D;
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
            }
        }
    }
}
