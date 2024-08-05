using System.Collections.Generic;
using Assets.scripts.bank.Others;
using Assets.scripts.Manager;
using UnityEngine;

namespace Assets.scripts.bank.Consults
{
    public class Consults : MultiBanco {

        public Consults()
        {
            description = LanguageManager.Instance.GetbankOptionsString("Consults");
            hasSubOptions = true;
        }

        public override List<MultiBanco> GetSubOptions()
        {
            Debug.Log(" overriding Accessed " + this.description);
            if (hasSubOptions)
            {
                if (SubOptions != null) SubOptions = null;
                SubOptions = new List<MultiBanco>();
                SubOptions.Add(new Back());
                Debug.Log(SubOptions.Count);
                return SubOptions;
            }
            else
            {
                return base.GetSubOptions();
            }
        }


    }
}
