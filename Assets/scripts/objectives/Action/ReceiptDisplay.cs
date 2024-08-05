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
        }

        public override void CheckForCompletion()
        {
            if (ReceiptsSetup.ReceiptsCompleted)
            {
                Performance = ReceiptsSetup.Answer == "Correct" ? 100 : 0;
                Debug.Log("performance at SM receipt: " + Performance);
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
