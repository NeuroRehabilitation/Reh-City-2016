using UnityEngine;

namespace Assets.scripts.GUI.Items.ShoppingMall
{
    public class Kellogs : Item {

        public Kellogs()
        {
   
            itemstyle = new GUIStyle();
            base.SetDefaultItemStyle();
            itemstyle.normal.background = Resources.Load("Images/Items/Kellogs") as Texture2D;
        }
    }
}
