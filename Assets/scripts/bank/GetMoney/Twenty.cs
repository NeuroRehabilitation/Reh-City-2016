using Assets.scripts.bank.Others;

namespace Assets.scripts.bank.GetMoney
{
    public class Twenty : global::Assets.scripts.bank.GetMoney.GetMoney {

        public Twenty()
        {
            description = "20";
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
            transactiontext.text = "Transaction Complete for "+this.description+" Euros";
        }
    }
}
