using Assets.scripts.GUI;
using UnityEngine;

namespace Assets.scripts.objectives.Action
{
    public class BillDisplay : ActionObjective
    {
        public BillDisplay()
        {
            description = "Verifique se a conta está correta";
        }

        public override void CheckForCompletion()
        {
            if (completed && DrawObjectiveList.CanGoToNextObj)
            {
                Application.LoadLevel("City");
                DrawObjectiveList.CanGoToNextObj = false;
            }
            if (Application.loadedLevelName == "City")
            {
                CanAddNextObjective = true;
            }
            base.CheckForCompletion();
        }
    }
}
