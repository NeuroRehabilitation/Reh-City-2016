using UnityEngine;

namespace Assets.scripts.GUI.Items.ShoppingMall
{
    public class Chocapic : Item {
	
        public Chocapic()
        {
            itemstyle = new GUIStyle();
            base.SetDefaultItemStyle();
            itemstyle.normal.background = Resources.Load("Images/Items/Chocapic") as Texture2D;

        }
	
    }
}
