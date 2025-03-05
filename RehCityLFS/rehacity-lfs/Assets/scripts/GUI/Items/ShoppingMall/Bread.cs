using UnityEngine;

namespace Assets.scripts.GUI.Items.ShoppingMall
{
    public class Bread : Item {

        public Bread()
        {
            itemstyle = new GUIStyle();
            base.SetDefaultItemStyle();
            itemstyle.normal.background = Resources.Load("Images/Items/Bread") as Texture2D;

        }
	
    }
}
