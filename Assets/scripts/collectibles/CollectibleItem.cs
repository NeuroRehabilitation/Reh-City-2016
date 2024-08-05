using Assets.scripts.Controller;
using Assets.scripts.GUI;
using Assets.scripts.Manager;
using Assets.scripts.objectives;
using Assets.scripts.objectives.Action;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.scripts.collectibles
{
    public class CollectibleItem : MonoBehaviour {
	
        // Set by GameManager
        public Vector3 MovePoint;
        public bool CanMove=false;

        public enum Type
        {
            SuperMarket,
            Utensils,
            PostOffice,
            Pharmacy
        };

        public enum Itemname
        {
            Milk,
            Juice,
            Chocapic,
            Bread,
            Water,
            Kellogs,
            Coke,
            Apple,
            Orange,
            Nutella,
            Shampoo,
            Coffee,
            Sauce,
            Yogurt,
            Butter,
            Pasta,
            OliveOil,
            Letter,
            Package,
            Stamp,
            ChildrenBook,
            AdultBook,
            Postcard,
            CDsBox,
            Lottery,
            Cream,
            Syrup,
            Bandaid,
            Aspirin,
            Betadin,
            Sunscreen,
            Benuron,
            Brufen
        };
        
        public Type ItemType;
        public Itemname ItemName;
        public float Price;
        private LanguageManager language;
        public Text Label;
        public int randomnumber;

        private GameObject _mger;
        private ObjectiveManager _objManager;
        private GameObject _label;

        private bool _filledLists;
        

        void Start()
        {
            foreach (Transform child in transform)
            {
                if (child.gameObject.name == "Label")
                    _label = child.gameObject;
            }
            //Debug.Log(_label.name);

            _mger = GameObject.FindGameObjectWithTag("Manager");
            _objManager = _mger.GetComponent<ObjectiveManager>();
            // if (ItemType == CollectibleItem.Type.PostOffice)
            // {
            language = LanguageManager.Instance;
                Label.text = language.GetObjectText(ItemName.ToString());
           // }
           _label.SetActive(false);
            
        }

        /// <summary>
        /// used by Item shuffler. Each object get a random number and Item shuffler sorts it
        /// </summary>
        public void GenerateRandomNumber()
        {
            randomnumber = Random.Range(0, 100);
        }

        void Update()
        {
            if (CanMove)
            {
                var width = new Vector3(Screen.width, Screen.height, 55);
                MovePoint = UnityEngine.Camera.main.ScreenToWorldPoint(width);
                MovePoint.x += 10;
                transform.position = Vector3.MoveTowards(transform.position, MovePoint, 3f);
            }

            //Debug.Log(CP_Controller.Instance.SelectedObject);
            
            if (CP_Controller.Instance.SelectedObject != null && CP_Controller.Instance.Selected)
            {
                if (gameObject == CP_Controller.Instance.SelectedObject && GameManager.ShowLabel)
                    _label.SetActive(true);
                else
                    _label.SetActive(false);
            }
            else
                _label.SetActive(false);

            if (Application.loadedLevelName == "SuperMarket" && !_filledLists && _objManager.GetCurrentObjective.GetType().ToString().Contains("Collect"))
            {
                for (var i = 0; i < _objManager.GetCurrentObjective.ItemsNames.Count; i++)
                {
                    if (_objManager.GetCurrentObjective.ItemsNames[i] == ItemName.ToString())
                    {
                        _objManager.GetCurrentObjective.ItemsPrices[i] = Price;
                    }
                }

                ReceiptsSetup.ItemsQuantities = _objManager.GetCurrentObjective.ItemsQuantities;
                ReceiptsSetup.ItemsNames = _objManager.GetCurrentObjective.ItemsNames;
                ReceiptsSetup.ItemsPrices = _objManager.GetCurrentObjective.ItemsPrices;
               
                _filledLists = true;
            }
            
        }
        
    }
}
