using Assets.scripts.GUI;
using Assets.scripts.Manager;
using Assets.scripts.RehaTask.RehaTask_GameLogic;
using UnityEngine;

namespace Assets.scripts.objectives.Collection
{
    public class CollectionObjective : Objectives {

        protected InventoryManager invmanager;
        /// <summary>
        /// total number of items to collect to complete this Objective
        /// </summary>
        
        protected LanguageManager language;
        

        public CollectionObjective()
        {
            childlist = nodelist[2].ChildNodes;
            invmanager = InventoryManager.Instance;
            language = LanguageManager.Instance;
        }

        public void CheckForCollectionPerformance()
        {
            if (GameManager.WrongChoices() == 0)
                Performance = 100.0f;
            else
            {
                var dif = GameManager.CorrectChoices() - GameManager.WrongChoices();
                Performance = dif * 100.0f / GameManager.CorrectChoices();
                if (Performance < 0)
                    Performance = 0;
            }
            TaskSummary.SaveTaskSummary(name, 0, 0, GameManager.CorrectChoices(), GameManager.WrongChoices(), 0, 0, Performance, ObjTime, 0);
            PerformanceProcessor.CancellationPerform = Performance;
            LoadSaveSettings.SaveSettingsInfo(name);
            Debug.Log("Targets to find: " + GameManager.CorrectChoices() + " ; wrong answers: " + GameManager.WrongChoices() + "; collection performance: " + Performance);
        }
    }
}
