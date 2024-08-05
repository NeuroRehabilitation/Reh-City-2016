using Assets.scripts.RehaTask.RehaTask_GameLogic;
using UnityEngine;

namespace Assets.scripts.RehaTask.RehaTaskGUI
{
    public class Main_Menu : MonoBehaviour {
	
        public static string Uid = "1";

        public static bool LArm = false;
        public static bool RArm = true;

        public static bool Sound = true, Music = true;
        public static bool TextField = true;

        public static bool Training = false;

        public static bool GameStarted = false;
     
        public static bool CategDistractors = false, StartFromRehabCity;
    
        private void Update()
        {
            if (StartFromRehabCity)
            {
                ButtonEvent(5);
                StartFromRehabCity = false;
            }
        }
        
        public void ButtonEvent(int btnEvent)
        {
            if (btnEvent == 5 || btnEvent == 8 || btnEvent == 3) //Play, practice and Calibration buttons
            {
                if (!LoadGame.GameLoaded && btnEvent != 3)
                    LoadGame.LoadingGame = true;

                SpawnTiles.SaveActualLevelNumber();

                if (LoadGame.GameLoaded && btnEvent != 3)
                {
                    ChangeLevel.LevelSet = false;
                    ChangeLevel.ResetOnce = true;
                    ChangeLevel.ResetLevel();
                }
            }
        }
    }
}
