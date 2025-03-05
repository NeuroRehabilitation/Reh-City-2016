using System.Collections.Generic;
using UnityEngine;

namespace Assets.scripts.bank.Others
{
    public class OtherOptions : MultiBanco {

        public OtherOptions()
        {
            description = "OtherOptions";
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
