using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/* Base Class for all the different categories
 * Each Category has its own cart where items collected can be placed
 * Each Category has its own style (Image)
 * Coder: Kushal
*/

namespace Assets.scripts.GUI.Categories
{
    public class Category{
	
        // used to change the images of each Category
        protected GUIStyle style;
	
        // read only property to get style
        public GUIStyle CategoryStyle
        {
            get
            {
                return style;
            }
        }
	
        // cart where the items will be placed
        // for every Category there will be individual carts spawning
        // this contains the number of items placed i.e 4 Milk containers, 2 Sandwiches etc..
        protected Dictionary<string,int> cart;
        // readonly property of the cart
        public Dictionary<string,int> Cart
        {
            get
            {
                return cart;
            }
        }
	
        //variable to store the Item spawned
        protected Item item;
	
	
        // this converts cart dictionary to a list
        // used by drawinventory class to get the number of objects collected of one Item (i.e 4 milk etc..)
        public List<KeyValuePair<string,int>> GetNumberOfItems
        {
            get
            {
                return cart.ToList();
            }
        }
	
        // list that contains the different Type of items added to the cart
        // i.e Milk,Sandwich etc..
        protected List<Item> cartlist;
        // readonly property for the cartlist
        public List<Item> GetCartList
        {
            get
            {
                return cartlist;
            }
        }
	

	
        // each Category adds items to its own cart
        // implemented in child classes
        public void AddItemToCart(string key)
        {}
	
        // default style for all the categories
        public virtual void SetDefaultStyle()
        {
            style.alignment = TextAnchor.LowerRight;
            style.fontSize = 10;
            style.fontStyle = FontStyle.Bold;
            style.stretchWidth = true;
            style.stretchHeight = true;
        }
	
    }
}
