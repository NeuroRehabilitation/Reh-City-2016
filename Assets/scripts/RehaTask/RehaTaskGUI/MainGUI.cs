using Assets.scripts.Camera;
using Assets.scripts.RehaTask.RehaTask_GameLogic;
using Assets.scripts.Settings;
using Assets.scripts.UDP;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.scripts.RehaTask.RehaTaskGUI
{
    public class MainGUI : MonoBehaviour {
	
        public GameObject  gamePlayUI, imageDisplay, cover, targetBig, targetSmall, changeLevelUI, timer, miniObjText, ObjectiveBG;
        public Transform layoutBig, layoutSmall;
        public Text time, cubes, objText;
        public Text timerLabel, cubesLabel;
        private GameObject _ikTarget;
        public static float TimeToDisplay;
        public static bool Picture;

        public static float ImgTimer;
        public static bool RemoveCover;//screen to avoid distractions and give some time before Big Picture be displayed
        public static bool PictureReady;
        public static bool AllImages, AllTargets;
        public static bool ClearTargets;
        public static bool ActivateHint, SettingLevel = false;
        public static string ObjectiveVisible = "NaN";

        private int _cubesToFind;

        public static bool DisplayMiniGoal;

        private void Start ()
        {
            ActivateHint = false;
            imageDisplay.SetActive(false);
            gamePlayUI.SetActive(false);
            cover.SetActive(false);
            RemoveCover = false;
            _ikTarget = GameObject.FindGameObjectWithTag("hand");
        }

        private void Update()
        {
            ObjectiveBG.SetActive(!SpawnTiles.UseMemory);

            timerLabel.text = Language.timer;
            cubesLabel.text = Language.remaining;
  
            if (Input.GetKeyDown(KeyCode.Escape) && gamePlayUI.activeSelf)
            {
                Time.timeScale = Time.timeScale == 1 ? 0 : 1;
            }

            _cubesToFind = SpawnTiles.Correctchoices - Scoring.GetCorrect();
            cubes.text = _cubesToFind.ToString();
            

            if (gamePlayUI.activeSelf && SpawnTiles.Timer>0)//SpawnTiles.Showcorrect)
            {
                timer.gameObject.SetActive(true);
                time.text = TimeToDisplay.ToString("0");

                if (CollisionDetection.ActivateTimer)
                {
                    TimeToDisplay -= Time.deltaTime;
                    if (TimeToDisplay <= 0)
                    {
                        
                        TimeToDisplay = 0;

                        if(CollisionDetection.SelectedTexture == "NaN")
                            TimerCount.Timeout = true;

                        if ((TimerCount.Timeout && SpawnTiles.Hints.Count >0) || (TimerCount.Timeout && !SpawnTiles.Showcorrect))
                            ActivateHint = true;
                    }
                }
            }

            else
                timer.gameObject.SetActive(false);
        
            if(CollisionDetection.ChangeLevel || ClearTargets)
            {
                var tempTargets = GameObject.FindGameObjectsWithTag("DisplayTarget");

                foreach (var t in tempTargets)
                {
                    Destroy(t);
                }
            }

            //displaying Targets to memorize before level starts
            if (Picture && !UdpReceive.Calib && PictureReady && !SettingLevel)
            {
                
                ObjectiveVisible = "BigDisplay";
                if (!AllImages)
                {
                    if (SpawnTiles.ObjectiveText != "")
                        objText.text = SpawnTiles.ObjectiveText;
                    else
                        objText.gameObject.SetActive(false);

                    var tempTargets = GameObject.FindGameObjectsWithTag("DisplayTarget");
                    if (!SpawnTiles.KeepThumbnail)
                    {
                        foreach (var t in tempTargets)
                        {
                            Destroy(t);
                        }
                    }

                    if (SpawnTiles.ConstructFigure)
                    {
                        var newImage = Instantiate(targetBig) as GameObject;
                        if (newImage != null)
                        {
                            newImage.name = SpawnTiles.ToConstruct.name;
                            newImage.transform.SetParent(layoutBig, false);
                            var tempText = SpawnTiles.ToConstruct as Texture2D;
                            var sprite = Sprite.Create(tempText, new Rect(0, 0, tempText.width, tempText.height), new Vector2(0, 0), 100.0f);
                            newImage.GetComponent<Image>().sprite = sprite;
                        }
                    }
                    else
                    {
                        foreach (var t in SpawnTiles.SelectionList)
                        {
                            var newImage = Instantiate(targetBig) as GameObject;
                            newImage.name = t.name;
                            newImage.transform.SetParent(layoutBig, false);
                            var tempText = t as Texture2D;
                            var sprite = Sprite.Create(tempText, new Rect(0, 0, tempText.width, tempText.height), new Vector2(0, 0), 100.0f);
                            newImage.GetComponent<Image>().sprite = sprite;
                        }
                    }

                    _ikTarget.SetActive(false);
                    imageDisplay.SetActive(true);
                    
                    cover.SetActive(true);
                    AllImages = true;
                }
               
                ImgTimer -= Time.deltaTime;
                
                if (ImgTimer <= SpawnTiles.PicTimer)
                {
                    cover.SetActive(false);
                    
                }

                if (ImgTimer < 0)
                {
                    
                    var displays = GameObject.FindGameObjectsWithTag("DisplayBig");
                    if (displays.Length > 0)
                    {
                        foreach (var t in displays)
                            Destroy(t);
                    }

                    imageDisplay.SetActive(false);
                    gamePlayUI.SetActive(true);
                    _ikTarget.SetActive(true);
                    ImgTimer = SpawnTiles.PicTimer;
                    MoveCamera.AnimStatus = true;
                    SpawnTiles.SpawnOnce = true;
                    ImgTimer = SpawnTiles.PicTimer;
                    Picture = false;
                    if(!SpawnTiles.KeepThumbnail)
                        PictureReady = false;

                    ObjectiveVisible = "NaN";

                }
            }
            //displaying the Targets on the top left side of the screen
            if ((SpawnTiles.KeepThumbnail || !SpawnTiles.UseMemory) && PictureReady && !SettingLevel)
            {
                ClearTargets = false;
                
                ObjectiveVisible = "Thumbnail";

                if (!AllTargets)
                {
                    if (SpawnTiles.ConstructFigure)
                    {
                        var newTarget = Instantiate(targetSmall) as GameObject;
                        newTarget.transform.SetParent(layoutSmall, false);
                        var tempText = SpawnTiles.ToConstruct as Texture2D;
                    
                        var sprite = Sprite.Create(tempText, new Rect(0, 0, tempText.width, tempText.height), new Vector2(0, 0), 100.0f);

                        foreach (Transform child in newTarget.transform)
                        {
                            child.GetComponent<Image>().sprite = sprite;
                        }
                    }
                    else
                    {
                        foreach (var t in SpawnTiles.SelectionList)
                        {
                            var newTarget = Instantiate(targetSmall) as GameObject;
                            newTarget.transform.SetParent(layoutSmall, false);
                            var tempText = t as Texture2D;
                            var sprite = Sprite.Create(tempText, new Rect(0, 0, tempText.width, tempText.height), new Vector2(0, 0), 100.0f);

                            var uiSprites = newTarget.GetComponentsInChildren<Image>();

                            foreach (var uiSprite in uiSprites)
                            {
                                if (uiSprite.gameObject.transform.parent.name == newTarget.name)
                                    uiSprite.sprite = sprite; //this gameObject is a child, because its transform.parent is not null  
                            }
                        }
                    }

                    AllTargets = true;
                }
            }

            if (!SpawnTiles.KeepThumbnail)
            {
                miniObjText.SetActive(true);
                miniObjText.GetComponent<Text>().text = SpawnTiles.ObjectiveText;
                var tempTarget = GameObject.FindGameObjectWithTag("DisplayTarget");
                Destroy(tempTarget);
                ObjectiveVisible = "TextSmallDisplay";
                if (SpawnTiles.ObjectiveText == "" || !DisplayMiniGoal)
                    ObjectiveBG.SetActive(false);
                
                
            }
            else
            {
                miniObjText.SetActive(false);
            }
        }

        public void SetButtonOption(int option)
        {
            if (option == 0)
            {
                ClearTargets = true;
                gamePlayUI.SetActive(false);
            }
        }  
    }
}
