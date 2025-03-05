using Assets.scripts.Controller;
using Assets.scripts.RehaTask.RehaTask_GameLogic;
using UnityEngine;

//This script is attached to SelectionArrow_01, _02, _03, _04, _05, _06 in Bank scene

namespace Assets.scripts.bank
{
    public class BankSelection : MonoBehaviour
    {
        public int Id;

        private BankManager _bankmanager;
        private CP_Controller _playerselect;
        // set from bank manager
        public string ButtonDescription;
	
        private void Start ()
        {
            _bankmanager = transform.parent.GetComponent<BankManager>();
            _playerselect = CP_Controller.Instance;
        }
    
        private void Update()
        {
            

            if (Controller.Controller.B1() && _playerselect.AtmButton)
            {
                for (var i = 0; i < _bankmanager.selectionarrows.Length; i++)
                {
                    if (_playerselect.SelectedObject == _bankmanager.selectionarrows[i])
                    {
                        Id = i;
                    }

                }

                ButtonDescription = MultiBanco.SubOptions[Id].Description;

                BankManager.ButtonSelected = ButtonDescription;

                _bankmanager.SelectedButton(Id);
                _playerselect.WriteGitterObjectName = ButtonDescription;
                _playerselect.AtmButton = false;
                _playerselect.Selected = false;
                //_playerselect.Timer.SetActive(false);
                //_playerselect._timerActivated = false;
                _playerselect.SelectedArrow = false;
                TimerCount.ControllerB1 = false;
            }
        }
        
    }
}
