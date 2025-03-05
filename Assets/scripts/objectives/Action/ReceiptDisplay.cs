using Assets.scripts.GUI;
using UnityEngine;

namespace Assets.scripts.objectives.Action
{
    public class ReceiptDisplay : ActionObjective
    {
        public ReceiptDisplay(bool wrongsum)
        {
            description = childlist[7].InnerText;
            ReceiptsSetup.ReceiptsCompleted = false;
            WrongSum = wrongsum;
            Performance = -1;
            name = "Receipt";
        }

        public override void CheckForCompletion()
        {
            if (ReceiptsSetup.ReceiptsCompleted || Controller.Controller.B6())
            {
                ObjTime = DrawObjectiveList.TaskTime;
                Performance = ReceiptsSetup.Answer == "Correct" ? 100 : 0;
                //Debug.Log("performance at SM receipt: " + Performance);

                int cor;
                int incor;
                if (Performance == 100)
                {
                    cor = 1;
                    incor = 0;
                }
                else
                {
                    cor = 0;
                    incor = 1;
                }
                TaskSummary.SaveTaskSummary(name, 0,0, cor, incor, 0, 0, Performance, ObjTime, 0);

                PerformanceProcessor.ProbSolvPerform = Performance;
                
                LoadSaveSettings.SaveSettingsInfo(name);
                completed = true;
                Application.LoadLevel(LoadReceipts.CurrentScene);
                ReceiptsSetup.ReceiptsCompleted = false;
            }

            if (completed && Application.loadedLevelName == LoadReceipts.CurrentScene)
            {
                CanAddNextObjective = true;
            }
        
            base.CheckForCompletion();
        }
    }
}
