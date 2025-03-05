using System.Collections.Generic;
using Assets.scripts.Manager;
using UnityEngine;

namespace Assets.scripts.bank.Services
{
    public class Services : MultiBanco {

        public Services()
        {
            description = LanguageManager.Instance.GetbankOptionsString("Services");
            hasSubOptions = true;
        }

        public override List<MultiBanco> GetSubOptions()
        {
            Debug.Log(" overriding Accessed " + this.description);
            if (hasSubOptions )
            {
                if (SubOptions != null) SubOptions = null;
                SubOptions = new List<MultiBanco>();
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
