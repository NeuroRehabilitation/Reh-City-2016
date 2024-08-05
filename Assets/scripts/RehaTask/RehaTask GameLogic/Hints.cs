using System.Linq;
using Assets.scripts.RehaTask.RehaTaskGUI;
using UnityEngine;

namespace Assets.scripts.RehaTask.RehaTask_GameLogic
{
    public class Hints : MonoBehaviour {

        public static float TimeToHint;
        public static float CurrentTime;
        private GameObject _hintTile;
        private Transform _imageDisplay, _numberDisplay;
        private GameObject _incorrectTile;
        public Transform tile;
        public static bool HintActive = false;
        public static float Timer;
        public static bool TimeCounter = false;
        public static bool Blink = true;
        public static float BlinkTimer = 8.0f;

        public Texture2D frameSelected;
        public GameObject frame;

        public Color32 orange;
        public static bool Ordered;

        private int _hintToDisplay;
        public static bool CanDisplayHint;
        private bool _canAnimateHint;

        private void Start()
        {
            CanDisplayHint = false;
        }

        private void Update () 
        {
            if (Application.loadedLevelName == "Home" || Application.loadedLevelName == "FashionStore" || Application.loadedLevelName == "Park")
            {
                OrderHints();
                if (MainGUI.ActivateHint)
                    ShowHint();
            }
        }

        private void OrderHints()
        {
            if(!Ordered && SpawnTiles.RenderPointer && SpawnTiles.Sequence)
            {
                for(var i = 0; i < SpawnTiles.SelectionList.Count; i++)
                {
                    for(var b = 0; b < SpawnTiles.Hints.Count; b++)
                    {
                        if(SpawnTiles.Hints[b].transform.Find("ImageDisplay").GetComponent<Renderer>().material.mainTexture == SpawnTiles.SelectionList[i])
                        {
                            var tempHint = SpawnTiles.Hints[i];
                            SpawnTiles.Hints[i] = SpawnTiles.Hints[b];
                            SpawnTiles.Hints[b] = tempHint;
                        }
                    }
                }
                Ordered = true;
            }
        }
    
        /// <summary>
        /// display correct answers
        /// </summary>
        private void ShowHint()
        {
            if (SpawnTiles.Hints.Count > 0)
            {
                foreach (var t in SpawnTiles.Incorrect)
                {
                    _incorrectTile = t;
                    if (SpawnTiles.Xxl)
                        _incorrectTile.transform.position = new Vector3(_incorrectTile.transform.position.x, 0.10f, _incorrectTile.transform.position.z);
                    else if (SpawnTiles.Xl)
                        _incorrectTile.transform.position = new Vector3(_incorrectTile.transform.position.x, 0.20f, _incorrectTile.transform.position.z);
                    else if (SpawnTiles.Big)
                        _incorrectTile.transform.position = new Vector3(_incorrectTile.transform.position.x, 0.4f, _incorrectTile.transform.position.z);
                    else if (SpawnTiles.Medium)
                        _incorrectTile.transform.position = new Vector3(_incorrectTile.transform.position.x, 0.45f, _incorrectTile.transform.position.z);
                    else
                        _incorrectTile.transform.position = new Vector3(_incorrectTile.transform.position.x, 0.5f, _incorrectTile.transform.position.z);

                    _incorrectTile.gameObject.GetComponent<Collider>().enabled = false;
                }

                if (SpawnTiles.Sequence)
                {
                    _hintTile = SpawnTiles.Hints[0];
                    _imageDisplay = SpawnTiles.Hints[0].transform.Find("ImageDisplay");
                    _numberDisplay = SpawnTiles.Hints[0].transform.Find("NumberDisplay");
                    _numberDisplay.gameObject.SetActive(true);
                    AnimateCube();
                }
                else if (SpawnTiles.Pairs > 0)
                {
                    if (CollisionDetection.OpenCards.Count > 0 && CanDisplayHint)
                    {
                        for (var i = 0; i < SpawnTiles.Hints.Count; i++)
                        {
                            for (var b = 0; b < CollisionDetection.OpenCards.Count; b++)
                            {
                                if (SpawnTiles.Hints[i].transform.Find("ImageDisplay").GetComponent<Renderer>().material.mainTexture ==
                                    CollisionDetection.OpenCards[b].transform.Find("ImageDisplay").GetComponent<Renderer>().material.mainTexture)
                                {
                                    _hintToDisplay = i;
                                }
                            }
                        }
                        _canAnimateHint = true;
                        CanDisplayHint = false;
                        _hintTile = SpawnTiles.Hints[_hintToDisplay];
                        _imageDisplay = SpawnTiles.Hints[_hintToDisplay].transform.Find("Cover");
                        
                    }
                    if(_canAnimateHint)
                        AnimateCube();
                    /*
                    else if(CanDisplayHint)
                    {
                        _hintToDisplay = 0;
                        CanDisplayHint = false;
                    }*/

                }
                else
                {
                    foreach (var t in SpawnTiles.Hints)
                    {
                        _hintTile = t;
                        _imageDisplay = t.transform.Find("ImageDisplay");
                        AnimateCube();
                    }
                }

                if (SpawnTiles.SelectionList.Contains(CollisionDetection.SelectedCube))
                    HintActive = true;
            }
        }  

        private void AnimateCube()
        {
            foreach (var child in _hintTile.transform.Cast<Transform>().Where(child => child.name == "frame"))
            {
                child.gameObject.SetActive(true);
            }

            //cube color animation to point out where correct answers are
            BlinkTimer -= Time.deltaTime;
            var lerp = Mathf.PingPong(Time.time, 0.6f) / 0.6f;
            _imageDisplay.gameObject.GetComponent<Renderer>().material.color = Color.Lerp(orange, Color.white, lerp);

            if (BlinkTimer < 0)
            {
                _canAnimateHint = false;
                _imageDisplay.gameObject.GetComponent<Renderer>().material.color = Color.white;
            }
        }
    }
}
