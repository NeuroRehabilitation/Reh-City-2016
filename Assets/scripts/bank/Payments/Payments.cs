using System.Collections.Generic;
using Assets.scripts.bank.Others;
using Assets.scripts.Manager;
using UnityEngine;

namespace Assets.scripts.bank.Payments
{
    public class Payments : MultiBanco {

   
        public Payments()
        {
            description = LanguageManager.Instance.GetbankOptionsString("Payments");
            hasSubOptions = true;
        }

        public override List<MultiBanco> GetSubOptions()
        {
            Debug.Log(" overriding Accessed " + this.description);
            if (hasSubOptions )
            {
                if (SubOptions != null) SubOptions = null;
                SubOptions = new List<MultiBanco>();
                SubOptions.Add(new PayElectricity());
                SubOptions.Add(new PayTelephone());
                SubOptions.Add(new PayWater());
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
