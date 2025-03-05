using System.Collections.Generic;
using Assets.scripts.bank.Others;
using Assets.scripts.Manager;

namespace Assets.scripts.bank.GetMoney
{
    public class GetMoney : MultiBanco {

        public GetMoney()
        {
            description = LanguageManager.Instance.GetbankOptionsString("WithDraw");
            hasSubOptions = true;
      
        }
        public override List<MultiBanco> GetSubOptions()
        {
            if (hasSubOptions )
            {
                if (SubOptions != null) SubOptions = null;
                SubOptions = new List<MultiBanco>();
                SubOptions.Add(new Twenty());
                SubOptions.Add(new Fourty());
                SubOptions.Add(new Sixty());
                SubOptions.Add(new Eighty());
                SubOptions.Add(new Back());
                return SubOptions;
            }
            else
            {
                return base.GetSubOptions();
            }
        }   
    }
}
