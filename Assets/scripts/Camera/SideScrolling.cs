using Assets.scripts.Controller;
using Assets.scripts.RehaTask.RehaTask_GameLogic;
using Assets.scripts.Settings;
using UnityEngine;
using UnityEngine.UI;

/* Attach this class to SideScroll Object
 * this scrolls the camera left/right
 */
namespace Assets.scripts.Camera
{
    public class SideScrolling : MonoBehaviour {

        private CP_Controller _cpc;
        public GameObject ctrler;
        private Transform _point;
        public static int ChangeSide;

        private void Start()
        {
            _cpc = CP_Controller.Instance;
            ChangeSide = 0;
            _cpc.gameObject.GetComponent<Collider>().enabled = false;

            var navBtns = GameObject.FindGameObjectsWithTag("directionalarrow");

            for (var i = 0; i < navBtns.Length; i++)
            {
                if(navBtns[i].name == "NextButton")
                    navBtns[i].GetComponentInChildren<Text>().text = Language.next;
                else if(navBtns[i].name == "PreviousButton")
                    navBtns[i].GetComponentInChildren<Text>().text = Language.previous;
            }
            /*
            _nextBtnText = GameObject.Find("NextButton").GetComponentInChildren<Text>();
            _previousBtnText = GameObject.Find("PreviousButton").GetComponentInChildren<Text>();
            _previousBtnText.text = Language.previous;
            _nextBtnText.text = Language.next;*/
        }

        private void SideScroll()
        {
            if (Controller.Controller.B1() && _cpc.SelectedArrow)
            {
                switch (_cpc.SelectedObject.name)
                {
                    case "PreviousButton":
                        if (ChangeSide > 0) ChangeSide -= 1;
                        _cpc.WriteGitterObjectName = "PreviousButton";
                        break;
                    case "NextButton":
                        ChangeSide += 1;
                        _cpc.WriteGitterObjectName = "NextButton";
                        break;
                    default:
                        break;
                }
                _cpc.SelectedArrow = false;
                var position = ChangeSide.ToString();
                _point = GameObject.Find(position).transform;
                TimerCount.ControllerB1 = false;
            }
            if (_point != null)
            {
                transform.position = Vector3.MoveTowards(this.transform.position, _point.position, 10);
            }
        }

        private void Update () {
            SideScroll();
        }
    }
}
