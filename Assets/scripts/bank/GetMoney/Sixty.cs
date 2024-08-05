using Assets.scripts.bank.Others;
using UnityEngine;

namespace Assets.scripts.bank.GetMoney
{
    public class Sixty : global::Assets.scripts.bank.GetMoney.GetMoney {

        public Sixty()
        {
            description = "60";
            hasSubOptions = false;
        }

        public override System.Collections.Generic.List<MultiBanco> GetSubOptions()
        {
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
