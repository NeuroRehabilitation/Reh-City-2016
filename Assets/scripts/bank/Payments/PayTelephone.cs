using Assets.scripts.bank.Others;
using Assets.scripts.Manager;
using UnityEngine;

namespace Assets.scripts.bank.Payments
{
    public class PayTelephone : Payments {

        public PayTelephone()
        {
            description = LanguageManager.Instance.GetbankOptionsString("Telephone");
            hasSubOptions = false;
        
        }
        public override System.Collections.Generic.List<MultiBanco> GetSubOptions()
        {
            Debug.Log("Accessed " + this.description + " and it " + this.hasSubOptions + " have suboptions");
            DisplayTransactionText();
            SubOptions = new System.Collections.Generic.List<MultiBanco>();
            SubOptions.Add(new Back());
            return SubOptions;
        }
        public override void DisplayTransactionText()
        {
            transactiontext.text = " Paid " + this.description;
        }
    }
}
