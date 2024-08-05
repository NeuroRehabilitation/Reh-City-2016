using UnityEngine;

namespace Assets.scripts.GUI.Items.ShoppingMall
{
    public class Juice : Item {

        public Juice()
        {
            itemstyle = new GUIStyle();
            base.SetDefaultItemStyle();
            itemstyle.normal.background = Resources.Load("Images/Items/Juice") as Texture2D;
        }
    }
}
