using Assets.scripts.GUI;
using Assets.scripts.Manager;
using Assets.scripts.RehaTask.RehaTaskGUI;
using Assets.scripts.RehaTask.RehaTask_GameLogic;
using UnityEngine;

namespace Assets.scripts.objectives.Action
{
    public class PlayRehatask : ActionObjective
    {
        public PlayRehatask(int targets, int distractors, int levelTime)
        {
            //Reh@Task Fashion Store
            RehaTaskLevel = 0;
            RehaTaskScenario = 1;
            RehaTaskElements = targets + distractors;
            RehaTaskTargets = targets;
            Type = "Category";
            DisplayGoal = true;
            TaskTime = levelTime;
            description = LanguageManager.Instance.GetRehaTaskTaskString(Type);
            Performance = -1;
            name = "Rehatask-FashionStore";
            
        }

        public PlayRehatask(int steps, bool displayGoal, int levelTime)
        {
            //Reh@Task Home
            RehaTaskLevel = 1;
            var rand = Random.Range(1, 4);
            RehaTaskScenario = rand;
            RehaTaskElements = steps;
            RehaTaskTargets = steps;
            DisplayGoal = displayGoal;
            TaskTime = levelTime;
            Type = "Sequence";
            description = LanguageManager.Instance.GetRehaTaskTaskString(Type);
            Performance = -1;
            name = "Rehatask-Home";
        }

        public PlayRehatask(int pairs, int levelTime)
        {
            if (pairs == 11 || pairs == 13 || pairs == 17 )
                pairs = pairs + 1;
            else if (pairs > 18)
                pairs = 20;
            
            //Reh@Task Park
            DisplayGoal = false;
            RehaTaskLevel = 2;
            RehaTaskScenario = 1;
            RehaTaskElements = pairs * 2;
            RehaTaskTargets = pairs;
            TaskTime = levelTime;
            Type = "Pairs";
            description = LanguageManager.Instance.GetRehaTaskTaskString(Type);
            Performance = -1;
            name = "Rehatask-Park";
        }

        private void CheckForRehaTaskPerformance()
        {
            var correct = RehaTaskTargets;
            if (RehaTaskLevel == 2)
            {
                correct = RehaTaskTargets*2;
                if (Scoring.CorrectInTime == correct)
                    Performance = 100.0f;
                else
                {
                    Performance = Scoring.CorrectInTime * 100.0f / correct;
                }
            }
            else
            {
                if (Scoring.WrongInTime == 0 && Scoring.CorrectInTime == correct)
                    Performance = 100.0f;
                else
                {
                    var dif = Scoring.CorrectInTime - Scoring.WrongInTime;
                    Performance = dif * 100.0f / correct;    
                }
            }
            if (Performance < 0)
                Performance = 0;

            Debug.Log("Targets to find: " + correct + " : correct answers: " + Scoring.CorrectInTime + " ; wrong answers: " + Scoring.Wrong + "; RehaTask performance: " + Performance);
        }

        public override void CheckForCompletion()
        {
            if (GUIChangeLevel.LevelFinished || Controller.Controller.B6())
            {
                CheckForRehaTaskPerformance();
                completed = true;
                GUIChangeLevel.LevelFinished = false;
            }

            if (completed && DrawObjectiveList.CanGoToNextObj && Application.loadedLevelName != "City")
            {
                DrawObjectiveList.CanGoToNextObj = false;
                Application.LoadLevel("City");
            }

            if (Application.loadedLevelName == "City")
            {
                CanAddNextObjective = true;
            }
            base.CheckForCompletion();
        }
    }
}
