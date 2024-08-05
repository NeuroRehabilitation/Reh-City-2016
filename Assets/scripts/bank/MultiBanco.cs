using System.Collections.Generic;
using Assets.scripts.bank.Others;
using UnityEngine;

namespace Assets.scripts.bank
{
    public class MultiBanco {

        /// <summary>
        /// SubOptions is one static list where
        /// all the options under a choosen option will be added
        /// </summary>
        public static List<MultiBanco> SubOptions;
        protected string description;
        public bool hasSubOptions;
        protected GUIText transactiontext;

        public string Description
        {
            get
            {
                return description;
            }
        }

        public MultiBanco()
        {
            description = "None";
            hasSubOptions = true;
            transactiontext = GameObject.Find("Transactiontext").GetComponent<GUIText>();
        }

        public void SetUpMainScreenOptions()
        {
            if (SubOptions != null) SubOptions = null;
            SubOptions = new List<MultiBanco>();
            SubOptions.Add(new GetMoney.GetMoney());
            SubOptions.Add(new Consults.Consults());
            SubOptions.Add(new Payments.Payments());
            //  SubOptions.Add(new Transfers());
            SubOptions.Add(new Quit());
        }

        /// <summary>
        /// Used to retrieve suboptions under a choosen option
        /// </summary>
        /// <returns>SubOptions</returns>
        public virtual List<MultiBanco> GetSubOptions()
        {
            if (!hasSubOptions)
            {
                SetUpMainScreenOptions();
            }
            DisplayTransactionText();
            return SubOptions;
        }

        public virtual void DisplayTransactionText()
        {
            transactiontext.text = " ";
        }

    }
}
