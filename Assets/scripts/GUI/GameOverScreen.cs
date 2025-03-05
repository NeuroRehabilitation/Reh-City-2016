using Assets.scripts.Manager;
using Assets.scripts.Settings;
using UnityEngine;
using UnityEngine.UI;

//This script is attached to Main Camera in GameOver scene

namespace Assets.scripts.GUI
{
    public class GameOverScreen : MonoBehaviour
    {

        public static int Score = 0;
        public static int Level = 0;

        public Text scoreText, endText, scoreLblText, quitText;

        private void Start ()
        {
            scoreText.text = Score.ToString();
            endText.text = Language.sessionEnd;
            scoreLblText.text = Language.score;
            quitText.text = Language.quit;

        }

        public void PlayAgain(bool restart)
        {
            if (restart)
            {
                Application.LoadLevel("StartScene");
            }
        }

        public void QuitCity(bool quit)
        {
            if (quit)
            {
                CSVDataSave.StopLog = true;
                Application.Quit();
            }
        }
    }
}
