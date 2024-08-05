using Assets.scripts.GUI;
using Assets.scripts.Manager;
using Assets.scripts.objectives;
using Assets.scripts.RehaTask.RehaTask_GameLogic;
using UnityEngine;

namespace Assets.scripts.Locations
{
    public class EnterRoom : MonoBehaviour {

        private GameObject _player;
        /// <summary>
        /// used to check if player is near the room
        /// </summary>
        private bool _canDisplay = false;
        private DrawObjectiveList _drawobjective;
        public string LevelToLoad;
        private string _objectiveRequiredToEnter;
        private LanguageManager _language;
        private ObjectiveManager _objmanager;
        public static bool ReachedLocation = false;
        public static string TextLocation;
        private Transform _hand;
        private GameObject _arrow;
	
        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _drawobjective = GameObject.FindGameObjectWithTag("InterfacePrefab").GetComponent<DrawObjectiveList>();
            _language = LanguageManager.Instance;
            _objmanager = ObjectiveManager.Instance;
            _arrow = GameObject.Find("arrow");
        }

        private void Update()
        {
            if (Application.loadedLevelName == "City" && Controller.Controller.B6())
            {
                switch (_objmanager.GetCurrentObjective.name)
                {
                    case "SuperMarket":
                        _player.transform.position = new Vector3(-2.78f, 3f, 28.51f);
                        _player.transform.rotation = Quaternion.Euler(new Vector3(0, 270, 0));
                        break;
                    case "PostOffice":
                        _player.transform.position = new Vector3(145.5f, 3f, 100f);
                        _player.transform.rotation = Quaternion.Euler(new Vector3(0, 90f, 0));
                        break;
                    case "Pharmacy":
                        _player.transform.position = new Vector3(81f, 3f, -91.9f);
                        _player.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                        break;
                    case "Bank":
                        _player.transform.position = new Vector3(-50f, 3f, -76.64f);
                        _player.transform.rotation = Quaternion.Euler(new Vector3(0, 270f, 0));
                        break;
                    case "Home":
                        _player.transform.position = new Vector3(2.175f, 3f, 101.21f);
                        _player.transform.rotation = Quaternion.Euler(new Vector3(0, 90f, 0));
                        break;
                    case "FashionStore":
                        _player.transform.position = new Vector3(-29.5f, 3f, -2.4f);
                        _player.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                        break;
                    case "Kiosk":
                        _player.transform.position = new Vector3(-8f, 3f, -136f);
                        _player.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                        break;
                    case "Park":
                        _player.transform.position = new Vector3(98f, 3f, 45f);
                        _player.transform.rotation = Quaternion.Euler(new Vector3(0, 90f, 0));
                        break;
                    default:
                        break;
                }
            }

            if (_objmanager.GetCurrentObjective == null) return;

            if (_objmanager.GetCurrentObjective.name == _objectiveRequiredToEnter && !_canDisplay)
                ReachedLocation = false;

            if (_objmanager.GetCurrentObjective.name == _objectiveRequiredToEnter && _canDisplay &&
                _drawobjective.MinimizeObjective)
                TextLocation = _language.EnterRoomtext(this.name);

            if (Vector3.Distance(this.gameObject.transform.position, _player.transform.position) < 5.0f)
                _objectiveRequiredToEnter = this.gameObject.name;

            _canDisplay = Vector3.Distance(gameObject.transform.position, _player.transform.position) < 5.0f ? true : false;
            
            if (_canDisplay && _drawobjective.MinimizeObjective &&
                _objmanager.GetCurrentObjective.name == _objectiveRequiredToEnter)
            {
                ReachedLocation = true;
                _arrow.SetActive(false);

                if (Controller.Controller.B1())
                {
                    switch (LevelToLoad)
                    {
                        case "SuperMarket":
                            GameManager.PlayerData.Playerposition = new Vector3(this.transform.position.x + 3, 3,
                                this.transform.position.z + 0.6f);
                            GameManager.PlayerData.Playerrotation = new Vector3(this.transform.localEulerAngles.x,
                                90, this.transform.localEulerAngles.z);
                            break;
                        case "PostOffice":
                            GameManager.PlayerData.Playerposition = new Vector3(this.transform.position.x-2, 3,
                                this.transform.position.z-5);
                            GameManager.PlayerData.Playerrotation = new Vector3(this.transform.localEulerAngles.x,
                                270, this.transform.localEulerAngles.z);
                            break;
                        case "Pharmacy":
                            GameManager.PlayerData.Playerposition = new Vector3(this.transform.position.x + 3, 3,
                                this.transform.position.z + 1);
                            GameManager.PlayerData.Playerrotation = new Vector3(this.transform.localEulerAngles.x,
                                0, this.transform.localEulerAngles.z);
                            break;
                        case "Bank":
                            GameManager.PlayerData.Playerposition = new Vector3(this.transform.position.x + 2.2f, 3,
                                this.transform.position.z);
                            GameManager.PlayerData.Playerrotation = new Vector3(this.transform.localEulerAngles.x,
                                90, this.transform.localEulerAngles.z);
                            break;
                        case "Home":
                            GameManager.PlayerData.Playerposition = new Vector3(this.transform.position.x, 3,
                                this.transform.position.z + 2.1f);
                            GameManager.PlayerData.Playerrotation = new Vector3(this.transform.localEulerAngles.x,
                                -90, this.transform.localEulerAngles.z);
                            break;
                        case "FashionStore":
                            GameManager.PlayerData.Playerposition = new Vector3(this.transform.position.x - 4, 3,
                                this.transform.position.z +2.2f);
                            GameManager.PlayerData.Playerrotation = new Vector3(this.transform.localEulerAngles.x,
                                0, this.transform.localEulerAngles.z);
                            break;
                        case "Kiosk":
                            GameManager.PlayerData.Playerposition = new Vector3(this.transform.position.x, 3,
                                this.transform.position.z);
                            GameManager.PlayerData.Playerrotation = new Vector3(this.transform.localEulerAngles.x,
                                0, this.transform.localEulerAngles.z);
                            break;
                        case "Park":
                            GameManager.PlayerData.Playerposition = new Vector3(this.transform.position.x, 3,
                                this.transform.position.z);
                            GameManager.PlayerData.Playerrotation = new Vector3(this.transform.localEulerAngles.x,
                                -90, this.transform.localEulerAngles.z);
                            break;
                        default:
                            break;
                    }

                    TimerCount.ControllerB1 = false;
                    ReachedLocation = false;
                    _hand = GameObject.FindGameObjectWithTag("hand").transform;
                    _hand.parent = null;
                    _arrow.SetActive(true);
                    Application.LoadLevel(LevelToLoad);
                }
            }   
        }

        public static int LocationReached()
        {
            var reached = 0;
            if (ReachedLocation)
                reached = 1;
            return reached;
        } 
    }
}
