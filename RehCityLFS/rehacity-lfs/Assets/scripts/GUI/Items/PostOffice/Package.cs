using UnityEngine;

namespace Assets.scripts.GUI.Items.PostOffice
{
    public class Package : Item {

        public Package()
        {
            itemstyle = new GUIStyle();
            base.SetDefaultItemStyle();
            itemstyle.normal.background = Resources.Load("Images/Items/PostOffice/Package") as Texture2D;
        }
    }
}
