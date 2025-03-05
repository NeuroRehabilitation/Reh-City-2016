using UnityEngine;

namespace Assets.scripts.GUI.Items.PostOffice
{
    public class Letter : Item {
        public Letter()
        {
            itemstyle = new GUIStyle();
            base.SetDefaultItemStyle();
            itemstyle.normal.background = Resources.Load("Images/Items/PostOffice/Letter") as Texture2D;
        }
    }
}
