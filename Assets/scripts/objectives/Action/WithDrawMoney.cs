using Assets.scripts.bank;
using Assets.scripts.GUI;
using UnityEngine;

namespace Assets.scripts.objectives.Action
{
    public class WithDrawMoney : ActionObjective {
    
        public WithDrawMoney(int amount)
        {
            description = childlist[0].InnerText;
            description = description.Replace("@", amount.ToString());
            RequiredSceneToSpawn = 4;
            ButtonToPress = amount.ToString();
            name = "Bank";
            CanSetupCode = false;
        }

        public override void CheckForCompletion()
        {
            if (BankManager.ButtonSelected == ButtonToPress || (Controller.Controller.B6() && Application.loadedLevelName == "Bank"))
            {
                completed = true;
                BankManager.ButtonSelected = null;
            }

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
