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
        private static GameObject _tempCard;
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
        }

        private void Update()
        {
            if (_hideCards && !transform.GetComponent<Animation>().isPlaying)
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
                if (ImageDisplay.GetComponent<Renderer>().material.color != LightGreen)
                {
                    SelectedTexture = ImageDisplay.GetComponent<Renderer>().material.mainTexture.name;
                    SelectedCube = ImageDisplay.GetComponent<Renderer>().material.mainTexture;
                    //_cardsChecked = false;

                    //displays Timer
                    if (!UdpReceive.calibrate)
                        ActivateTimer = true;

                    if (Main_Menu.Sound)
                        _audio1.Play();

                    IsColided = true;

                    if (SpawnTiles.Pairs == 0)
                    {
                        transform.GetComponent<Animation>().Play();    
                    }
                    Frame.SetActive(true);
                    Timer.SetActive(true);
                }
                else
                    transform.gameObject.GetComponent<Collider>().enabled = false;
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

                    transform.GetComponent<Animation>().Stop();
                }

                if (ImageDisplay.GetComponent<Renderer>().material.color == Color.white)
                {
                    Frame.GetComponent<Renderer>().material.mainTexture = FrameSelected;
                    Frame.SetActive(false);
                }

                ImageDisplay.GetComponent<Renderer>().material.color = ImageDisplay.GetComponent<Renderer>().material.color == LightGreen ? LightGreen : Color.white;

                _isSelected = false;
            }
        }
    
        //"hand" keeps over the same cube
        private void OnTriggerStay(Component c)
        {
            if (c.tag == "hand")
            {
                if(!Hints.CanDisplayHint)
                    SelectedTexture = ImageDisplay.GetComponent<Renderer>().material.mainTexture.name;
                SelectedCube = ImageDisplay.GetComponent<Renderer>().material.mainTexture;
                TimerPosition = transform.position.x;
                if (!_isSelected)
                {
                    if (!_active && TimerCount.Timeout)
                    {
                        
                        if (!_playedAnimation)
                        {
                            transform.GetComponent<Animation>().Play();
                            _playedAnimation = true;
                        }

                        if (SpawnTiles.SelectionList.Contains(ImageDisplay.GetComponent<Renderer>().material.mainTexture))
                        {
                            
                            if (SpawnTiles.Sequence)
                            {
                                if (SpawnTiles.SelectionList[X].name == ImageDisplay.GetComponent<Renderer>().material.mainTexture.name)
                                {
                                    Scoring.SetCorrect(1);
                                    _objmanager.AddScore(1);
                                    Frame.GetComponent<Renderer>().material.mainTexture = FrameCorrect;
                                    NumberDisplay.SetActive(true);
                                    NumberDisplay.GetComponent<TextMesh>().color = SelectedNumber;
                                    Hints.BlinkTimer = 6.0f;
                                    if (Main_Menu.Sound)
                                        _audio2.Play();

                                    ImageDisplay.GetComponent<Renderer>().material.color = LightGreen;

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
                                    Frame.GetComponent<Renderer>().material.mainTexture = FrameIncorrect;
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
                                ImageDisplay.GetComponent<Renderer>().material.color = LightGreen;
                                OpenCards.Add(gameObject);
                                _canPlayNegSound = true;
                                Scoring.SetCorrect(1);
                                
                                if (OpenCards.Count > 1)
                                {
                                    for (var i = 0; i < OpenCards.Count - 1; i++)
                                    {
                                        if (OpenCards[i].transform.FindChild("ImageDisplay").GetComponent<Renderer>().material.mainTexture.name ==
                                            ImageDisplay.GetComponent<Renderer>().material.mainTexture.name)
                                        {
                                            if (i == SpawnTiles.Pairs - 2)
                                            {
                                                for (var b = 0; b < OpenCards.Count; b++)
                                                {
                                                    OpenCards[b].transform.FindChild("frame").GetComponent<Renderer>().material.mainTexture = FrameCorrect;
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
                                Frame.GetComponent<Renderer>().material.mainTexture = FrameCorrect;

                                if (Main_Menu.Sound)
                                    _audio2.Play();

                                ImageDisplay.GetComponent<Renderer>().material.color = LightGreen;

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
                                Frame.GetComponent<Renderer>().material.mainTexture = FrameIncorrect;

                                if (Main_Menu.Sound)
                                    _audio3.Play();

                                ImageDisplay.GetComponent<Renderer>().material.color = LightGreen;
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
                            _ikTarget.GetComponent<Collider>().enabled = false;

                            GUIChangeLevel.Setting = false;

                            GUIChangeLevel.LevelFailed = Hints.HintActive;

                            GUIChangeLevel.ChangeLevelTimer = 6.0f;
                            ChangeLevel = true;
                        }
                    }
                    else if (_active)
                    {
                        if (SpawnTiles.SelectionList.Contains(ImageDisplay.GetComponent<Renderer>().material.mainTexture))
                            Scoring.DeleteCorrect(1);
                        else
                            Scoring.DeleteError(1);
                    
                        ImageDisplay.GetComponent<Renderer>().material.color = Color.white;

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
                OpenCards[i].transform.FindChild("frame").GetComponent<Renderer>().material.mainTexture = FrameIncorrect;
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
                OpenCards[i].transform.FindChild("ImageDisplay").GetComponent<Renderer>().material.color = Color.white;
                OpenCards[i].transform.FindChild("frame").GetComponent<Renderer>().material.mainTexture = FrameSelected;
                OpenCards[i].transform.FindChild("frame").gameObject.SetActive(false);
                OpenCards[i].GetComponent<Collider>().enabled = true;
                SpawnTiles.Hints.Add(OpenCards[i]);
            }
            //Hints.CanDisplayHint = true;
            OpenCards.Clear();
        }   
    }
}
