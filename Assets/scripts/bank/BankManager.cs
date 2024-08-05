using Assets.scripts.objectives;
using UnityEngine;
using UnityEngine.UI;

//This script is attached to SelectionArrowObject in Bank scene

namespace Assets.scripts.bank
{
    public class BankManager : MonoBehaviour {

        private GameObject _mger;
        private ObjectiveManager _objManager;
        private MultiBanco _multibanco;
        public GameObject[] selectionarrows;
        public GameObject sequenceChar, horLayout;
        public static string ButtonSelected = "NaN";
        private GameObject _code, _lerpingChar;
        //private TextMesh _sequenceText;
        
        public static bool BankOptionsReady;
        public static bool SequenceInstantiated;
    
        private void Start ()
        {
            BankOptionsReady = true;
            _mger = GameObject.FindGameObjectWithTag("Manager");
            _objManager = _mger.GetComponent<ObjectiveManager>();
            _multibanco = new MultiBanco();
            _multibanco.SetUpMainScreenOptions();
            _code = GameObject.Find("SequenceBG");
            SequenceInstantiated = false;
            
        }

        /* called from Selectobject when player places the cursor on 
     * bank options.
    */
        private void Update()
        {
            if (_objManager.GetCurrentObjective.CanSetupCode)
            {
                SetupScreenCode();
            }
            
            if(!_objManager.GetCurrentObjective.CanSetupCode && BankOptionsReady)
                SetupOptions();
        }

        public void SetupScreenCode()
        {
            _code.SetActive(true);
            for (var i = 0; i <= MultiBanco.SubOptions.Count; i++)
            {
                selectionarrows[i].SetActive(false);
            }
            
            var lerp = Mathf.PingPong(Time.time, 0.6f) / 0.6f;
            if(_lerpingChar != null)
                _lerpingChar.GetComponent<Text>().color = Color.Lerp(Color.red, Color.white, lerp);

            for (var i = 0; i < 10; i++)
            {
                var tempKey = i.ToString();
                GameObject.Find(tempKey).GetComponent<BoxCollider>().enabled = true;
            }

            if (!SequenceInstantiated)
            {
                var changedColor = false;
                for (var i = 0; i < _objManager.GetCurrentObjective.NumericSequence.Length; i++)
                {
                    var newChar = Instantiate(sequenceChar) as GameObject;
                    if (newChar != null)
                    {
                        newChar.transform.SetParent(horLayout.transform, false);
                        newChar.GetComponent<Text>().text = _objManager.GetCurrentObjective.NumericSequence[i].ToString();
                        var tempChar = _objManager.GetCurrentObjective.NumericSequence[i].ToString();
                        if(tempChar == "_" && !changedColor)
                        {
                            _lerpingChar = newChar;
                            changedColor = true;
                        }
                    }
                }
                SequenceInstantiated = true;
            } 
        }

        public void SelectedButton(int id)
        {
            MultiBanco.SubOptions[id].GetSubOptions();
            BankOptionsReady = true;
        }

        private void SetupOptions()
        {
            _code.SetActive(false);

            for (var i = 0; i < 10; i++)
            {
                var tempKey = i.ToString();
                var tempKeyObject = GameObject.Find(tempKey);
                tempKeyObject.GetComponent<BoxCollider>().enabled = false;
                tempKeyObject.transform.FindChild("Cylinder").GetComponent<Renderer>().material.color = new Color(0, 0, 0, 0);
            }

            // set the available options active
            for (var i = 0; i < MultiBanco.SubOptions.Count; i++)
            {
                selectionarrows[i].SetActive(true);
                //selectionarrows[i].transform.FindChild("New Text").GetComponent<TextMesh>().text = MultiBanco.SubOptions[i].Description;
                selectionarrows[i].GetComponentInChildren<TextMesh>().text = MultiBanco.SubOptions[i].Description;
                selectionarrows[i].GetComponentInChildren<TextMesh>().name = MultiBanco.SubOptions[i].Description;
            }

            // disable the unavailable options
            for (var i = MultiBanco.SubOptions.Count; i < selectionarrows.Length; i++)
            {
                selectionarrows[i].SetActive(false);
            }
            BankOptionsReady = false;
        }
    
    }
}
