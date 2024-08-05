using UnityEngine;

namespace Assets.scripts.GUI.Items.Pharmacy
{
    public class Syrup : Item {

        public Syrup()
        {
            itemstyle = new GUIStyle();
            base.SetDefaultItemStyle();
            itemstyle.normal.background = Resources.Load("Images/Items/Pharmacy/Syrup") as Texture2D;
        }
    }
}
