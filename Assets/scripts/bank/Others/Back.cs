using Assets.scripts.Manager;

namespace Assets.scripts.bank.Others
{
    public class Back : MultiBanco
    {
        public Back()
        {
            description = LanguageManager.Instance.GetbankOptionsString("Back");
            hasSubOptions = false;
        }
    }
}
