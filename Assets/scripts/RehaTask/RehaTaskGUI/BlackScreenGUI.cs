using Assets.scripts.RehaTask.RehaTask_GameLogic;
using Assets.scripts.UDP;
using UnityEngine;

namespace Assets.scripts.RehaTask.RehaTaskGUI
{
    public class BlackScreenGUI : MonoBehaviour {

        public static bool CoverActive;
        public GameObject targetImage, cover;

        private void Start ()
        {
            CoverActive = SpawnTiles.UseMemory;
        }
    
        private void Update () 
        {
            if (MainGUI.RemoveCover)
            {
                CoverActive = false;
                cover.SetActive(false);
            }

            else
            {
                CoverActive = true;
            }

            if (!UdpReceive.Calib && CoverActive)
            {
                cover.SetActive(true);
            }
        }
    }
}
