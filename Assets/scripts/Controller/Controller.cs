using Assets.scripts.RehaTask.RehaTask_GameLogic;
using UnityEngine;

namespace Assets.scripts.Controller
{
    public class Controller : MonoBehaviour
    {
        public static bool B1()
        {
            if (UnityEngine.Input.GetButtonDown("B1") || UnityEngine.Input.GetKeyDown(KeyCode.Space) ||
                TimerCount.ControllerB1)
            {
                if (TimerCount.ControllerB1 && !CP_Controller.Instance.SelectedArrow &&
                    !CP_Controller.Instance.AtmButton && Application.loadedLevel != 2)
                    TimerCount.ControllerB1 = false;
                
                return true;
            }
            return false;
        }
        
        public static bool B2()
        {
            if (UnityEngine.Input.GetButtonDown("B2") || UnityEngine.Input.GetKeyDown(KeyCode.S))
                return true;
            return false;
        }

        public static bool B3()
        {
            if (UnityEngine.Input.GetButtonDown("B3") || UnityEngine.Input.GetKeyDown(KeyCode.L)) return true;
            return false;
        }

        public static bool B4()
        {
            if (UnityEngine.Input.GetButtonDown("B4") || UnityEngine.Input.GetKeyDown(KeyCode.M)) return true;
            return false;
        }

        public static bool B5()
        {
            if (UnityEngine.Input.GetButtonDown("B5") || UnityEngine.Input.GetKeyDown(KeyCode.A)) return true;
            return false;
        }

        public static bool B6()
        {
            if (UnityEngine.Input.GetButtonDown("B6") || UnityEngine.Input.GetKeyDown(KeyCode.C)) return true;
            return false;
        }

        public static bool B7()
        {
            if (UnityEngine.Input.GetButtonDown("B7") || UnityEngine.Input.GetKeyDown(KeyCode.O)) return true;
            return false;
        }

        public static bool B8()
        {
            if (UnityEngine.Input.GetButtonDown("B8")) return true;
            return false;
        }
    }
}

