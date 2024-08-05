using System.Collections.Generic;
using UnityEngine;

namespace Assets.scripts.GUI.Categories
{
    public class InCity : Category {
     
        public InCity()
        {
            //categorycount++;
            Debug.Log("InCityCategory Created");
            cart = new Dictionary<string, int>(); 
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
