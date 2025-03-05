using Assets.scripts.RehaTask.RehaTaskGUI;
using UnityEngine;

namespace Assets.scripts.RehaTask.RehaTask_GameLogic
{
    public class Scoring : MonoBehaviour {

        private static int _correct;//C
        public static int Wrong;//E
        private static int _score;//final _score in %
        public static int HalfScore;
        public static int FullScore;

        public static bool Countdown = false;
        public static bool HasPlayed;
        public static AudioSource Audio1;

        public static int Points;
        public static int TotalPoints;

        public static int CorrectInTime;
        public static int WrongInTime;

        private void Awake()
        {
            var sounds = gameObject.GetComponents<AudioSource>();
            Audio1 = sounds[0];
        }

        //Correct Answerss
        public static int GetCorrect()
        {
            return _correct;
        }

        public static void SetCorrect(int nCorrect)
        {
            if (nCorrect == 0)
            {
                _correct = 0;
                CorrectInTime = 0;
            }
            else
            {
                _correct = _correct + nCorrect;
                //if it's a _correct answer coming from a hint, only gets 50% of the _score
                if (Hints.HintActive)
                {
                    HalfScore ++;
                }
                else
                {
                    FullScore = FullScore + nCorrect;
                    CorrectInTime = CorrectInTime + nCorrect;
                }
            }
        }
	
        public static void DeleteCorrect(int del_correct){_correct = _correct-del_correct;}
		
        //Error Answers
        public static int GetError(){return Wrong;}
        public static void SetError(int n_error)
        {
            if (n_error == 0)
            {
                Wrong = 0;
                WrongInTime = 0;
            }
            else
            {
                Wrong = Wrong + n_error;
                if (!Hints.HintActive)
                    WrongInTime ++;
            }
        }
	
        public static void DeleteError(int del_error){Wrong = Wrong-del_error;}
		
        //Final _score
        public static int GetScore()
        {
            if (SpawnTiles.LevelName != "L0")
                _score = ((FullScore * 10) + (HalfScore * 5) - (Wrong * 10)) / SpawnTiles.Correctchoices;
            else
                _score = 0;
            return _score; 
        }

        public static int GetPoints()
        {
            if (SpawnTiles.LevelName != "L0")
                Points = (FullScore * 10) + (HalfScore * 5);
            else
                Points = 0;
            return Points;
        }

        public static void SetTotalPoints(int points)
        {
            TotalPoints = TotalPoints + points;
        }

        public static int GetTotalPoints()
        {
            return TotalPoints;
        }

        private void Update()
        {
            if (SpawnTiles.Timer > 0 && MainGUI.TimeToDisplay <= 4.5f && SpawnTiles.ImagesLoaded)
            {
                if (Main_Menu.Sound)
                {
                    if (Audio1.isPlaying)
                    {
                        HasPlayed = true;
                    }
                    else
                    {
                        PlaySound();
                    }
                }
            }
        }

        private static void PlaySound()
        {
            if (!HasPlayed)
            {
                Audio1.Play();
            }
        }
    }
}
