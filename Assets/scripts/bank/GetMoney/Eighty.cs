using Assets.scripts.bank.Others;
using UnityEngine;

namespace Assets.scripts.bank.GetMoney
{
    public class Eighty : global::Assets.scripts.bank.GetMoney.GetMoney {

        public Eighty()
        {
            description = "80";
            hasSubOptions = false;
        }

        public override System.Collections.Generic.List<MultiBanco> GetSubOptions()
        {
            Debug.Log("Accessed" + this.description);
            DisplayTransactionText();
            SubOptions = new System.Collections.Generic.List<MultiBanco>();
            SubOptions.Add(new Back());
            return SubOptions;
        }
        public override void DisplayTransactionText()
        {
            transactiontext.text = "Transaction Complete for " + this.description + " Euros";
        }
    }
}
