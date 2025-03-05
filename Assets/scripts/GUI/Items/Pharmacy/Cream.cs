using UnityEngine;

namespace Assets.scripts.GUI.Items.Pharmacy
{
    public class Cream : Item {

        public Cream()
        {
            itemstyle = new GUIStyle();
            base.SetDefaultItemStyle();
            itemstyle.normal.background = Resources.Load("Images/Items/Pharmacy/Cream") as Texture2D;
        }
    }
}
