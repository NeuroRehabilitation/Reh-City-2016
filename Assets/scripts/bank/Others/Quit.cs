using System.Collections.Generic;
using Assets.scripts.Manager;

namespace Assets.scripts.bank.Others
{
    public class Quit : MultiBanco {

        public Quit()
        {
            description = LanguageManager.Instance.GetbankOptionsString("Quit");
            hasSubOptions = false;
        }
    }
}
