using UnityEngine;

namespace Assets.scripts.GUI.Items.PostOffice
{
    public class Stamp : Item {

        public Stamp()
        {
            itemstyle = new GUIStyle();
            base.SetDefaultItemStyle();
            itemstyle.normal.background = Resources.Load("Images/Items/PostOffice/Stamp") as Texture2D;
        }
    }
}
