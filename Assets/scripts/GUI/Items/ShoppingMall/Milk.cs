using UnityEngine;

namespace Assets.scripts.GUI.Items.ShoppingMall
{
    public class Milk : Item {

        public Milk()
        {
            itemstyle = new GUIStyle();
            base.SetDefaultItemStyle();
            itemstyle.normal.background = Resources.Load("Images/Items/Milk") as Texture2D;

        }
	
	
    }
}
