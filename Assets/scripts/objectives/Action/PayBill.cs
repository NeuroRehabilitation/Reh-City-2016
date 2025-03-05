using Assets.scripts.bank;
using Assets.scripts.GUI;
using Assets.scripts.Manager;
using UnityEngine;

namespace Assets.scripts.objectives.Action
{
    public class PayBill : ActionObjective {
    
        public PayBill(string BillToPay)
        {
            description = childlist[1].InnerText;
            description = description.Replace("@", LanguageManager.Instance.GetbankOptionsString(BillToPay));
            ButtonToPress = LanguageManager.Instance.GetbankOptionsString(BillToPay);
            name = "Bank";
            CanSetupCode = false;
        }
        public override void CheckForCompletion()
        {
            if (BankManager.ButtonSelected == ButtonToPress || Controller.Controller.B6() && Application.loadedLevelName == "Bank")
            {
                completed = true;
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
