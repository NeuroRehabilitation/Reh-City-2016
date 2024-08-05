using Assets.scripts.Settings;
using UnityEngine;

namespace Assets.scripts.Player
{
    public class PlayerOptions : MonoBehaviour {
	
        public Texture2D crosshairImage;
	
        public GameObject normalPlayer;
        public GameObject oculusPlayer;

        // Use this for initialization
        void Start () 
        {
            if(!GeneralSettings.useOculus)
            {
                normalPlayer.SetActive(true);
                //oculusPlayer.SetActive(false);
            }
            else
            {
                normalPlayer.SetActive(false);
                //oculusPlayer.SetActive(true);			
            }
        }

	
        void OnGUI()
        {
            float xMin = (Screen.width / 2) - (crosshairImage.width/4);
            float yMin = (Screen.height / 2) - (crosshairImage.height / 2);
            UnityEngine.GUI.DrawTexture(new Rect(xMin, yMin, crosshairImage.width/4, crosshairImage.height/4), crosshairImage);
        }
	
    }
}
