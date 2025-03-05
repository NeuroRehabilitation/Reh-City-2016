using UnityEngine;
using System.Collections.Generic;
using Assets.scripts.bank.Others;
using Assets.scripts.bank.Payments;
using Assets.scripts.Locations;
using Assets.scripts.Manager;
using Assets.scripts.objectives;
using Assets.scripts.objectives.Action;
using Assets.scripts.objectives.Collection;
using Assets.scripts.objectives.Location;
using Assets.scripts.objectives.LocationandAction;
using Assets.scripts.objectives.LocationandCollection;

public class LevelListManager {
    public static Dictionary<int, List<Objectives>> LevelList;
    public static void AddLevelsToList(int Level)
    {
        LevelList = new Dictionary<int, List<Objectives>>();
        switch (Level)
        {
            case 1:
               // LevelList.Add(0, GetLevel1ListA());
                //LevelList.Add(1, GetLevel1ListB());
                LevelList.Add(0, GetLevel1ListC());
                //LevelList.Add(0, GetLevel1ListD());
                break;
            case 2:
                LevelList.Add(0, GetLevel2ListA());
                //LevelList.Add(0, GetLevel2ListB());
                //LevelList.Add(2, GetLevel2ListC());
                //LevelList.Add(3, GetLevel2ListD());
                break;
            case 3:
                LevelList.Add(0, GetLevel3ListA());
                //LevelList.Add(1, GetLevel3ListB());
                //LevelList.Add(2, GetLevel3ListC());
                //LevelList.Add(3, GetLevel3ListD());
                break;
            case 4:
                LevelList.Add(0, GetLevel4ListA());
                //LevelList.Add(1, GetLevel4ListB());
                //LevelList.Add(2, GetLevel4ListC());
                //LevelList.Add(3, GetLevel4ListD());
                break;
            case 5:
                LevelList.Add(0, GetLevel5ListA());
                //LevelList.Add(1, GetLevel5ListB());
                break;
            default:
                break;
        }
    }
    static List<Objectives> GetLevel1ListA()
    {
        List<Objectives> list = new List<Objectives>();
        return list;
    }
    static List<Objectives> GetLevel1ListB()
    {
        List<Objectives> list = new List<Objectives>();
        return list;
    }
    static List<Objectives> GetLevel1ListC()
    {
        List<Objectives> list = new List<Objectives>();
        
        list.Add(new GotoBank());
        list.Add(new EnterCode(6, 1, 3, 3));
        list.Add(new WithDrawMoney(20));

        //list.Add(new GotoPostoffice());
        //list.Add(new CollectPOItem(2, "Stamp", 10));
        
        //list.Add(new GoToHome());
        //list.Add(new PlayRehatask(6, true));
        /*
        list.Add(new GoToFashionStore());
        list.Add(new PlayRehatask(2, 1, "Category"));

        list.Add(new GotoPharmacy());
        list.Add(new CollectPHItem(2, "Cream", 20));

        list.Add(new GotoShopping());
        list.Add(new CollectSMItem(3, "Water", 30));
        list.Add(new ReceiptDisplay(0));
        
        list.Add(new GoToKiosk());
        list.Add(new TipDisplay("image", 1));
        */
        //list.Add(new GoToPark());
        //list.Add(new QuestionsDisplay(0));
        //list.Add(new GoToFashionStore());
        //list.Add(new PlayRehatask(3, 12));

        //list.Add(new GoToHome());
        //list.Add(new PlayRehatask(1, 1, "Sequence"));

        //list.Add(new PayBill("Electricity"));

        //list.Add(new CollectMultipleSMItems(3, "Water", 2, "Juice",10));
        //list.Add(new CollectMultipleSMItems(3, "Water", 2, "Juice", 1, "Bread", 1, "Shampoo",10));
        //list.Add(new ReceiptDisplay(2));


        //list.Add(new CollectMultiplePOItems(3, "Package", 2, "Stamp"));
        //list.Add(new CollectMultiplePOItems(3, "Package", 2, "Stamp", 1, "AdultBook", 1, "ChildrenBook"));


        //list.Add(new CollectMultiplePHItems(2, "Aspirin", 1, "Sunscreen", 10));
        //list.Add(new CollectMultiplePHItems(2, "Bandaid", 2, "Syrup", 2, "Cream", 1, "Betadin",15));


        //list.Add(new GoToFashionStore());
        //list.Add(new PlayRehatask(2, 1, "Category"));

        //list.Add(new GotoPHandgetMeds(2, false));
        //list.Add(new QuestionsDisplay(0));
        //list.Add(new GotoBankandWithdraw(20));
        //list.Add(new GotoSMandgetFood(2, "Lunch"));
        //list.Add(new GotoPOCollectMail(1, false));
        //list.Add(new GotoPHandgetMeds(2, true));
        //list.Add(new GotoPOCollectMail(1, true));
        //list.Add(new GotoSMandgetFood(2, "Snack"));

        return list;
    }

    private static List<Objectives> GetLevel1ListD()
    {
        List<Objectives> list = new List<Objectives>();
        return list;
    }
    

    static List<Objectives> GetLevel2ListA()
     {
        List<Objectives> list = new List<Objectives>();

        //list.Add(new GoToPark());
        //list.Add(new PlayRehatask(5, 1, "Pairs"));
        
        list.Add(new GotoPostoffice());
        list.Add(new CollectMultiplePOItems(3, "Package", 2, "Stamp", 10));

        list.Add(new GotoBank());
        list.Add(new EnterCode(12, 2, 3, 3));
        list.Add(new WithDrawMoney(40));

        list.Add(new GotoPharmacy());
        list.Add(new CollectMultiplePHItems(2, "Aspirin", 1, "Sunscreen", 10));
        
        //list.Add(new GoToHome());
        //list.Add(new PlayRehatask(4, 3, "Sequence"));
       
        return list;
     }

    private static List<Objectives> GetLevel2ListB()
     {
         List<Objectives> list = new List<Objectives>();
         return list;
     }
     private static List<Objectives> GetLevel2ListC()
     {
         List<Objectives> list = new List<Objectives>();
         return list;
     }
     private static List<Objectives> GetLevel2ListD()
     {
         List<Objectives> list = new List<Objectives>();
         return list;
     }
  
 
    private static List<Objectives> GetLevel3ListA()
     {
         List<Objectives> list = new List<Objectives>();

        list.Add(new GotoBank());
        list.Add(new PayBill(LanguageManager.Instance.GetbankOptionsString("Electricity")));
        /*
        list.Add(new GoToFashionStore());
        list.Add(new PlayRehatask(2, 1, "Category"));

        list.Add(new GoToHome());
        list.Add(new PlayRehatask(3, 2, "Sequence"));
        */
        //list.Add(new GoToKiosk());
        //list.Add(new TipDisplay("image", 0));

        list.Add(new GotoPharmacy());
       // list.Add(new QuestionsDisplay(0));
        list.Add(new CollectMultiplePHItems(2, "Bandaid", 2, "Syrup", 2, "Cream", 1, "Betadin", 10));

        //list.Add(new GotoShopping());
        //list.Add(new CollectMultipleSMItems(2, "Bread", 1, "Kellogs", 3, "Apple", 1, "Shampoo", 10));
        //list.Add(new ReceiptDisplay(1));

        

        return list;
     }

    private static List<Objectives> GetLevel3ListB()
     {
         List<Objectives> list = new List<Objectives>();
         return list;
     }

    private static List<Objectives> GetLevel3ListC()
     {
         List<Objectives> list = new List<Objectives>();
        return list;
     }

    private static List<Objectives> GetLevel3ListD()
     {
         List<Objectives> list = new List<Objectives>();
        return list;
     }

    private static List<Objectives> GetLevel4ListA()
     {
        List<Objectives> list = new List<Objectives>();
        list.Add(new GotoPHandgetMeds(2, false));
        list.Add(new GotoBankandWithdraw(20));
        list.Add(new GotoSMandgetFood(2, "Lunch"));
        list.Add(new GotoPOCollectMail(1, false));
         return list;
     }

    private static List<Objectives> GetLevel4ListB()
     {
         List<Objectives> list = new List<Objectives>();
         return list;
     }

    private static List<Objectives> GetLevel4ListC()
     {
         List<Objectives> list = new List<Objectives>();
        return list;
     }

    private static List<Objectives> GetLevel4ListD()
     {
         List<Objectives> list = new List<Objectives>();
        return list;
     }

    private static List<Objectives> GetLevel5ListA()
     {
        List<Objectives> list = new List<Objectives>();
        list.Add(new GotoPHandgetMeds(2, true));
        list.Add(new GotoPOCollectMail(1, true));
        list.Add(new GotoSMandgetFood(2, "Snack"));
        return list;
     }

    private static List<Objectives> GetLevel5ListB()
     {
         List<Objectives> list = new List<Objectives>();
        return list;
     }

}
