using System;
using System.Collections.Generic;
using System.Reflection;
using Assets.scripts.objectives;
using Assets.scripts.RehaTask.RehaTaskGUI;
using Assets.scripts.UDP;
using UnityEngine;

namespace Assets.scripts.RehaTask.RehaTask_GameLogic
{
    public class CollisionDetection : MonoBehaviour
    {
        public static bool IsColided;
        public Transform ImageDisplay;
        public GameObject Frame, Timer, NumberDisplay;
        public Color SelectedNumber;
        private GameObject _ikTarget;
        public Color LightGreen = new Color32(254, 254, 254, 255);
        public Texture2D FrameCorrect, FrameIncorrect, FrameSelected;
        public static Texture SelectedCube;
        private bool _isSelected;
        private bool _active;
        public static bool ActivateTimer;
        public static float TimerPosition;//Timer changes position wether the left or right hand is being used
        public static string SelectedTexture = "NaN";//actual texture being selected by the "hand"
        public static bool ChangeLevel;
        private bool _keepCube, _canPlaySound = true, _canPlayNegSound = true;
        public static int X;
        private ObjectiveManager _objmanager;

        private AudioSource[] _sounds = new AudioSource[4];
        private AudioSource _audio1;
        private AudioSource _audio2;
        private AudioSource _audio3;

        public static readonly List<GameObject> OpenCards = new List<GameObject>();

        private bool _cardsChecked, _hideCards;
        private static bool _playedAnimation;
        //private static GameObject _tempCard;
        private float _timeToHide = 2.0f;

        private void Start()
        {
            OpenCards.Clear();
            _ikTarget = GameObject.FindGameObjectWithTag("hand");
            _sounds = GetComponents<AudioSource>();
            _audio1 = _sounds[0];
            _audio2 = _sounds[1];
            _audio3 = _sounds[2];
            NumberDisplay.SetActive(false);
            _objmanager = ObjectiveManager.Instance;
            IsColided = false;
            ActivateTimer = false;
            X = 0;
            _playedAnimation = false;
        }

        private void Update()
        {
            if (_hideCards && !transform.animation.isPlaying)
            {
                DisplayWrongFrames();
                _timeToHide -= Time.deltaTime;
                if (_timeToHide < 0)
                {
                    CloseCards();
                    _playedAnimation = false;
                    _hideCards = false;    
                }
            }
        }
        
        //if "hand" collides with a cube
        private void OnTriggerEnter(Component cl)
        {
            _keepCube = false;
            TimerCount.Timeout = false;
            TimerCount.MyTimer = TimerCount.StartTime;
            
            if (cl.tag == "hand")
            {
                //plays animation and sets _active the orange Frame
                if (ImageDisplay.renderer.material.color != LightGreen)
                {
                    SelectedTexture = ImageDisplay.renderer.material.mainTexture.name;
                    SelectedCube = ImageDisplay.renderer.material.mainTexture;
                    //_cardsChecked = false;

                    //displays Timer
                    if (!UdpReceive.calibrate)
                        ActivateTimer = true;

                    if (Main_Menu.Sound)
                        _audio1.Play();

                    IsColided = true;

                    if (SpawnTiles.Pairs == 0)
                    {
                        transform.animation.Play();    
                    }
                    Frame.SetActive(true);
                    Timer.SetActive(true);
                }
                else
                    transform.gameObject.collider.enabled = false;
            }
        }

        private void OnTriggerExit(Component cl)
        {
            if (cl.tag == "hand")
            {
                _playedAnimation = false;
                _cardsChecked = false;
                SelectedTexture = "NaN";
                _canPlaySound = true;
                IsColided = false;
                Timer.SetActive(false);
                if (SpawnTiles.Pairs == 0)
                {
                    if (SpawnTiles.Xxl)
                        transform.transform.localScale = new Vector3(0.57f, 0.57f, 0.57f);
                    else if (SpawnTiles.Xl)
                        transform.transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);
                    else if (SpawnTiles.Big)
                        transform.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
                    else if (SpawnTiles.Medium)
                        transform.transform.localScale = new Vector3(0.27f, 0.27f, 0.27f);
                    else
                        transform.transform.localScale = new Vector3(0.20f, 0.20f, 0.20f);

                    transform.animation.Stop();
                }

                if (ImageDisplay.renderer.material.color == Color.white)
                {
                    Frame.renderer.material.mainTexture = FrameSelected;
                    Frame.SetActive(false);
                }

                ImageDisplay.renderer.material.color = ImageDisplay.renderer.material.color == LightGreen ? LightGreen : Color.white;

                _isSelected = false;
            }
        }
    
        //"hand" keeps over the same cube
        private void OnTriggerStay(Component c)
        {
            if (c.tag == "hand")
            {
                //if(!Hints.CanDisplayHint)
                    SelectedTexture = ImageDisplay.renderer.material.mainTexture.name;
                SelectedCube = ImageDisplay.renderer.material.mainTexture;
                TimerPosition = transform.position.x;
                if (!_isSelected)
                {
                    if (!_active && TimerCount.Timeout)
                    {
                        
                        if (!_playedAnimation)
                        {
                            transform.animation.Play();
                            _playedAnimation = true;
                        }

                        if (SpawnTiles.SelectionList.Contains(ImageDisplay.renderer.material.mainTexture))
                        {
                            
                            if (SpawnTiles.Sequence)
                            {
                                if (SpawnTiles.SelectionList[X].name == ImageDisplay.renderer.material.mainTexture.name)
                                {
                                    Scoring.SetCorrect(1);
                                    _objmanager.AddScore(1);
                                    Frame.renderer.material.mainTexture = FrameCorrect;
                                    NumberDisplay.SetActive(true);
                                    NumberDisplay.GetComponent<TextMesh>().color = SelectedNumber;
                                    Hints.BlinkTimer = 6.0f;
                                    if (Main_Menu.Sound)
                                        _audio2.Play();

                                    ImageDisplay.renderer.material.color = LightGreen;

                                    if (SpawnTiles.LevelName != "L0")
                                        Scoring.SetTotalPoints(Hints.HintActive ? 5 : 10);
                                
                                    for (var i = 0; i < SpawnTiles.Hints.Count; i++)
                                    {

                                        if (SpawnTiles.Hints[i].gameObject == this.gameObject)
                                        {
                                            Hints.BlinkTimer = 6.0f;
                                            SpawnTiles.Hints.RemoveAt(i);
                                        }
                                    }
                                    _active = true;
                                    _isSelected = true;

                                    X++;
                                }
                                else
                                {
                                    _keepCube = true;
                                    Scoring.SetError(1);
                                    _objmanager.AddScore(-1);
                                    if (Main_Menu.Sound && _canPlaySound)
                                    {
                                        _audio3.Play();
                                        _canPlaySound = false;
                                    }
                                    Frame.renderer.material.mainTexture = FrameIncorrect;
                                    _isSelected = true;
                                }
                            }
                            else if (SpawnTiles.Pairs > 0 && !_cardsChecked)
                            {
                                for (var d = 0; d < SpawnTiles.Hints.Count; d++)
                                {
                                    if (SpawnTiles.Hints[d].gameObject == gameObject)
                                    {
                                        Hints.BlinkTimer = 6.0f;
                                        SpawnTiles.Hints.RemoveAt(d);
                                    }
                                }
                                ImageDisplay.renderer.material.color = LightGreen;
                                OpenCards.Add(gameObject);
                                _canPlayNegSound = true;
                                Scoring.SetCorrect(1);
                                
                                if (OpenCards.Count > 1)
                                {
                                    for (var i = 0; i < OpenCards.Count - 1; i++)
                                    {
                                        if (OpenCards[i].transform.FindChild("ImageDisplay").renderer.material.mainTexture.name ==
                                            ImageDisplay.renderer.material.mainTexture.name)
                                        {
                                            if (i == SpawnTiles.Pairs - 2)
                                            {
                                                for (var b = 0; b < OpenCards.Count; b++)
                                                {
                                                    OpenCards[b].transform.FindChild("frame").renderer.material.mainTexture = FrameCorrect;
                                                }

                                                var hintsActive = GameObject.FindGameObjectsWithTag("Hint");
                                                if (hintsActive != null)
                                                {
                                                    foreach (var fr in hintsActive)
                                                    {
                                                        fr.tag = "Untagged";
                                                    }
                                                }

                                                _objmanager.AddScore(SpawnTiles.Pairs);
                                                _audio2.Play();
                                                OpenCards.Clear();
                                            }
                                        }
                                        else
                                        {
                                            Scoring.SetError(1);
                                            _timeToHide = 2.0f;
                                            _hideCards = true;
                                        }
                                    }
                                }
                                Hints.CanDisplayHint = true;
                                SelectedTexture = "NaN";
                                _cardsChecked = true;
                                
                            }
                            else if(SpawnTiles.Pairs == 0)
                            {
                                Scoring.SetCorrect(1);
                                _objmanager.AddScore(1);
                                Frame.renderer.material.mainTexture = FrameCorrect;

                                var hintsActive = GameObject.FindGameObjectsWithTag("Hint");
                                

                                if (Main_Menu.Sound)
                                    _audio2.Play();

                                ImageDisplay.renderer.material.color = LightGreen;

                                if (SpawnTiles.LevelName != "L0")
                                    Scoring.SetTotalPoints(Hints.HintActive ? 5 : 10);

                                //removes the tile from the Hints list
                                for (var i = 0; i < SpawnTiles.Hints.Count; i++)
                                {
                                    if (SpawnTiles.Hints[i].gameObject == this.gameObject)
                                        SpawnTiles.Hints.RemoveAt(i);
                                }

                                _active = true;
                                _isSelected = true;
                            }
                        }
                        else
                        {
                            if (SpawnTiles.Pairs == 0)
                            {
                                Scoring.SetError(1);
                                _objmanager.AddScore(-1);
                                Frame.renderer.material.mainTexture = FrameIncorrect;

                                if (Main_Menu.Sound)
                                    _audio3.Play();

                                ImageDisplay.renderer.material.color = LightGreen;
                                _isSelected = true;
                            }
                        }
                        if (SpawnTiles.Pairs == 0)
                        {
                            if (SpawnTiles.Xxl && !_keepCube)
                            {
                                transform.position = new Vector3(transform.position.x, 0.18f, transform.position.z);
                                transform.localScale = new Vector3(0.57f, 0.57f, 0.57f);
                            }
                            else if (SpawnTiles.Xl && !_keepCube)
                            {
                                transform.position = new Vector3(transform.position.x, 0.20f, transform.position.z);
                                transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);
                            }
                            else if (SpawnTiles.Big && !_keepCube)
                            {
                                transform.position = new Vector3(transform.position.x, 0.4f, transform.position.z);
                                transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
                            }
                            else if (SpawnTiles.Medium && !_keepCube)
                            {
                                transform.position = new Vector3(transform.position.x, 0.45f, transform.position.z);
                                transform.localScale = new Vector3(0.27f, 0.27f, 0.27f);
                            }
                            else if (!SpawnTiles.Xxl && !SpawnTiles.Xl && !SpawnTiles.Big && !SpawnTiles.Medium &&
                                     !_keepCube)
                            {
                                transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
                                transform.localScale = new Vector3(0.20f, 0.20f, 0.20f);
                            }
                        }
                        if (Scoring.GetCorrect() == SpawnTiles.Correctchoices)
                        {
                            RehaTask_GameLogic.ChangeLevel.MyTimer = 1.0f;
                            _ikTarget.collider.enabled = false;

                            GUIChangeLevel.Setting = false;

                            GUIChangeLevel.LevelFailed = Hints.HintActive;

                            GUIChangeLevel.ChangeLevelTimer = 6.0f;
                            ChangeLevel = true;
                        }
                    }
                    else if (_active)
                    {
                        if (SpawnTiles.SelectionList.Contains(ImageDisplay.renderer.material.mainTexture))
                            Scoring.DeleteCorrect(1);
                        else
                            Scoring.DeleteError(1);
                    
                        ImageDisplay.renderer.material.color = Color.white;

                        if (SpawnTiles.Pairs == 0)
                        {
                            if (SpawnTiles.Xxl)
                                transform.position = new Vector3(transform.position.x, 0.18f, transform.position.z);
                            else if (SpawnTiles.Xl)
                                transform.position = new Vector3(transform.position.x, 0.20f, transform.position.z);
                            else if (SpawnTiles.Big)
                                transform.position = new Vector3(transform.position.x, 0.4f, transform.position.z);
                            else if (SpawnTiles.Medium)
                                transform.position = new Vector3(transform.position.x, 0.45f, transform.position.z);
                            else
                                transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
                        }

                        _isSelected = true;
                    }
                    TimerCount.Timeout = false; //reset Timer
                    _active = false;
                    
                }
            }
        }

        private void DisplayWrongFrames()
        {
            if (_canPlayNegSound)
            {
                _audio3.Play();
                _canPlayNegSound = false;
            }
            
            for (var i = 0; i < OpenCards.Count; i++)
            {
                OpenCards[i].transform.FindChild("frame").renderer.material.mainTexture = FrameIncorrect;
            }

            var hintsActive = GameObject.FindGameObjectsWithTag("Hint");
            if (hintsActive != null)
            {
                foreach (var fr in hintsActive)
                {
                    fr.tag = "Untagged";
                    fr.transform.parent.renderer.material.color = Color.white;
                    fr.SetActive(false);
                }
            }
        }

        private void CloseCards()
        {
            Scoring.SetError(OpenCards.Count);
            _objmanager.AddScore(-1);
            Scoring.SetCorrect(OpenCards.Count * -1);
            for (var i = 0; i < OpenCards.Count; i++)
            {
                OpenCards[i].transform.rotation = Quaternion.Euler(0,0,0);
                OpenCards[i].transform.FindChild("ImageDisplay").renderer.material.color = Color.white;
                OpenCards[i].transform.FindChild("frame").renderer.material.mainTexture = FrameSelected;
                OpenCards[i].transform.FindChild("frame").gameObject.SetActive(false);
                SpawnTiles.Hints.Add(OpenCards[i]);
                OpenCards[i].collider.enabled = true;
            }
            _isSelected = false;
            //Hints.CanDisplayHint = true;
            OpenCards.Clear();
        }   
    }
}
