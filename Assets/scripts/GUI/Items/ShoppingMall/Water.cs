using UnityEngine;

namespace Assets.scripts.GUI.Items.ShoppingMall
{
    public class Water : Item {

        public Water()
        {
            itemstyle = new GUIStyle();
            base.SetDefaultItemStyle();
            itemstyle.normal.background = Resources.Load("Images/Items/Water") as Texture2D;

        }
	
    }
}
