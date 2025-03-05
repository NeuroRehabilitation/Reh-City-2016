using Assets.scripts.bank;
using Assets.scripts.GUI;
using UnityEngine;

namespace Assets.scripts.objectives.LocationandAction
{
    public class GotoBankandWithdraw : LocationandActionObjective {

        public GotoBankandWithdraw(int amount)
        {
            description = childlist[0].InnerText;
            ButtonToPress = amount.ToString();
            name = "Bank";
        }
        public override void SetTargetlocation()
        {
            base.SetTargetlocation();
            targetlocation = LocationHolder.transform.FindChild("Bank").transform.position;
            location = TargetLocation;
        }

        public override void CheckForCompletion()
        {
            if (Application.loadedLevelName == "Bank")
            {
                if (BankManager.ButtonSelected == ButtonToPress || Controller.Controller.B6())
                {
                    completed = true;
                    BankManager.ButtonSelected = null;
                }

                if (completed && DrawObjectiveList.CanGoToNextObj)
                {
                    Application.LoadLevel("City");
                    DrawObjectiveList.CanGoToNextObj = false;
                }
            }
            if (completed && Application.loadedLevelName == "City")
            {
                CanAddNextObjective = true;
            }
            base.CheckForCompletion();
        }
    }
}
