using System;
using System.IO;
using System.Xml;
using Assets.scripts.Controller;
using Assets.scripts.objectives.Action;
using Assets.scripts.RehaTask.RehaTaskGUI;
using Assets.scripts.UDP;
using UnityEngine;

namespace Assets.scripts.Settings
{
    public class General_Settings : MonoBehaviour {
	
        public GameObject LArm, RArm, Scenario1, Scenario2, Scenario3;
        public static bool Enable = true;
        public static int TaskScenario;

        private void Start()
        {
            
        }

        private void Update()
        {
            if (Application.loadedLevelName == "Home" || Application.loadedLevelName == "FashionStore" || Application.loadedLevelName == "Park")
            {
                switch (TaskScenario)
                {
                    case 1:
                        Scenario1.SetActive(true);
                        if (Scenario2 != null)
                            Scenario2.SetActive(false);
                        if (Scenario3 != null)
                            Scenario3.SetActive(false);
                        break;
                    case 2:
                        Scenario1.SetActive(false);
                        if (Scenario2 != null)
                            Scenario2.SetActive(true);
                        if (Scenario3 != null)
                            Scenario3.SetActive(false);
                        break;
                    case 3:
                        Scenario1.SetActive(false);
                        if (Scenario2 != null)
                            Scenario2.SetActive(false);
                        if (Scenario3 != null)
                            Scenario3.SetActive(true);
                        break;
                }
            }

            if (Enable)
            {
                ikLimbLeft.IsEnabled = Main_Menu.LArm;
                ikLimbRight.IsEnabled = Main_Menu.RArm;

                if (ikLimbLeft.IsEnabled)
                {
                    LArm.transform.Rotate(0, 90, 0);
                    RArm.transform.Rotate(0, 0, 0);
                }
                else if (ikLimbRight.IsEnabled)
                {
                    RArm.transform.Rotate(0, 270, 0);
                    LArm.transform.Rotate(0, 0, 0);
                }
                else
                {
                    RArm.transform.Rotate(0, 0, 0);
                    LArm.transform.Rotate(0, 0, 0);
                }

                Enable = false;
            }    
        }
   
           
    }
}