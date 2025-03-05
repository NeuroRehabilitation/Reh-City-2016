using Assets.scripts.GUI;
using Assets.scripts.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.scripts.Controller
{
    public class NavigationTimer : MonoBehaviour
    {

        public static float NavigationTime = 60;
        public static bool NavigationCowntdown, JumpToDestination, CanJump = true;
        public static float NavTimer;

        public GameObject TeleportPanel, NavSettings;
        public Text SecondsText, TeleportTxt;

        private void Start ()
        {
            NavigationCowntdown = true;
            NavTimer = 100000;
            TeleportPanel.SetActive(false);
            TeleportTxt.text = Language.Teleport;
        }
	
        private void Update ()
        {
            if (NavigationCowntdown && DrawObjectiveList.Minimized && !NavSettings.activeSelf && CanJump)
                NavTimer -= Time.deltaTime;
            else
            {
                TeleportPanel.SetActive(false);
            }

            if (NavTimer < 6 && CanJump)
            {
                TeleportPanel.SetActive(true);
                SecondsText.text = NavTimer.ToString("0");
            }
            else if (NavTimer < 6 && !CanJump)
            {
                NavTimer = 6.0f;
            }
            
            if (NavTimer < 0)
            {
                TeleportPanel.SetActive(false);
                JumpToDestination = true;
                NavTimer = 100000;
            }
        }
    }
}
