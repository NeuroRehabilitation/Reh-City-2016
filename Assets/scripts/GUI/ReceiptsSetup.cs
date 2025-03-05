using System.Collections.Generic;
using System.Globalization;
using Assets.scripts.Manager;
using Assets.scripts.objectives;
using Assets.scripts.objectives.Action;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.scripts.GUI
{
    public class ReceiptsSetup : MonoBehaviour
    {
        public static List<string> ItemsNames = new List<string>();
        public static List<int> ItemsQuantities = new List<int>();
        public static List<float> ItemsPrices = new List<float>();

        public GameObject ItemLinePrefab;
        public Transform ItemsLayout1, ItemsLayout2, Total1, Total2;

        private static string _correctReceipt;
        public static string Answer = "NaN";

        protected LanguageManager language;

        private GameObject _mger;
        private ObjectiveManager _objManager;

        public static bool Choice, ReceiptsCompleted;

        private void Start()
        {
            _mger = GameObject.FindGameObjectWithTag("Manager");
            _objManager = _mger.GetComponent<ObjectiveManager>();
            language = LanguageManager.Instance;
            SetCorrectAndIncorrect();
            FillReceipts();
        }

        private void SetCorrectAndIncorrect()
        {
            var randomNumber = Random.Range(-1, 1);
            _correctReceipt = randomNumber < 0 ? "Receipt1" : "Receipt2";
        }

        private void FillReceipts()
        {
            Transform correctLayout = null;
            Transform incorrectLayout = null;
            RectTransform correctContainer;
            RectTransform incorrectContainer;
            Text correctTotal = null;
            Text wrongTotal = null;
            var correctSum = 0.0f;
            var wrongSum = 0.0f;

            if (_correctReceipt == "Receipt1")
            {
                correctLayout = ItemsLayout1;
                incorrectLayout = ItemsLayout2;
                correctTotal = Total1.transform.GetComponent<Text>();
                wrongTotal = Total2.transform.GetComponent<Text>();
            }
            else
            {
                correctLayout = ItemsLayout2;
                incorrectLayout = ItemsLayout1;
                correctTotal = Total2.transform.GetComponent<Text>();
                wrongTotal = Total1.transform.GetComponent<Text>();
            }

            

            for (var i = 0; i < ItemsQuantities.Count; i++)
            {
                var newLineC = Instantiate(ItemLinePrefab) as GameObject;
                var newLineW = Instantiate(ItemLinePrefab) as GameObject;

                if (newLineC != null)
                {
                    newLineC.transform.SetParent(correctLayout, false);

                    var description = newLineC.transform.FindChild("ItemDescription").GetComponentInChildren<Text>();
                    var quantity = newLineC.transform.FindChild("Quantity").GetComponentInChildren<Text>();
                    var price = newLineC.transform.FindChild("UnitPrice").GetComponentInChildren<Text>();
                    var lineTotal = newLineC.transform.FindChild("SubTotal").GetComponentInChildren<Text>();

                    description.text = language.GetObjectText(ItemsNames[i]); 
                    quantity.text = ItemsQuantities[i] + "x";
                    price.text = ItemsPrices[i].ToString("0.00") + "€";
                    var lineTot = ItemsQuantities[i] * ItemsPrices[i];
                    lineTotal.text = lineTot.ToString("0.00") + "€";
                    correctSum += lineTot;
                }

                if (newLineW != null)
                {
                    newLineW.transform.SetParent(incorrectLayout, false);

                    var description = newLineW.transform.FindChild("ItemDescription").GetComponentInChildren<Text>();
                    var quantity = newLineW.transform.FindChild("Quantity").GetComponentInChildren<Text>();
                    var price = newLineW.transform.FindChild("UnitPrice").GetComponentInChildren<Text>();
                    var lineTotal = newLineW.transform.FindChild("SubTotal").GetComponentInChildren<Text>();

                    description.text = language.GetObjectText(ItemsNames[i]);
                    quantity.text = ItemsQuantities[i] + "x";
                    price.text = ItemsPrices[i].ToString("0.00") + "€";
                    var lineTot = ItemsQuantities[i] * ItemsPrices[i];

                    if (!_objManager.GetCurrentObjective.WrongSum)
                    {
                        var error = Random.Range(-0.05f, 0.99f);
                        if (error == 0)
                            error = 0.01f;
                        lineTot += error;
                    }
                    wrongSum += lineTot;
                    lineTotal.text = lineTot.ToString("0.00") + "€";
                    // ->lineTotal.text = LoadReceipts.Receipts[_objManager.GetCurrentObjective.Wrong][i] + "€";
                }
            }
            correctContainer = correctLayout.GetComponent<RectTransform>();
            incorrectContainer = incorrectLayout.GetComponent<RectTransform>();

            var height = 100 * ItemsQuantities.Count;
            var width = correctContainer.sizeDelta.x;

            correctContainer.sizeDelta = new Vector2(width, height);
            incorrectContainer.sizeDelta = new Vector2(width, height);
            correctContainer.anchoredPosition = new Vector2(width, correctContainer.anchoredPosition.y);
            correctContainer.offsetMax = new Vector2(correctContainer.offsetMax.x, 0);
            correctContainer.offsetMax = new Vector2(0, correctContainer.offsetMax.y);
            incorrectContainer.anchoredPosition = new Vector2(width, incorrectContainer.anchoredPosition.y);
            incorrectContainer.offsetMax = new Vector2(incorrectContainer.offsetMax.x, 0);
            incorrectContainer.offsetMax = new Vector2(0, incorrectContainer.offsetMax.y);

            correctTotal.text = correctSum.ToString("0.00") + "€";

            if (_objManager.GetCurrentObjective.WrongSum)
            {
                var error = Random.Range(-0.05f, 0.99f);
                if (error == 0)
                    error = 0.01f;
                wrongSum += error;
            }

            wrongTotal.text = wrongSum.ToString("0.00") + "€";
            // ->wrongTotal.text = LoadReceipts.Receipts[_objManager.GetCurrentObjective.Wrong][LoadReceipts.Receipts[_objManager.GetCurrentObjective.Wrong].Count - 1] + "€";

        }

        public static void SaveAnswer(string answer)
        {
            Answer = _correctReceipt == answer ? "Correct" : "Wrong";
            ReceiptsCompleted = true;
        }
    }
}
