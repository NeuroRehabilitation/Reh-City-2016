using UnityEngine;

namespace Assets.scripts.GUI.Items.Pharmacy
{
    public class Bandaid : Item {

        public Bandaid()
        {
            itemstyle = new GUIStyle();
            base.SetDefaultItemStyle();
            itemstyle.normal.background = Resources.Load("Images/Items/Pharmacy/band") as Texture2D;
        }
    }
}
