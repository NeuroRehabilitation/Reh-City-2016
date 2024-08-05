using Assets.scripts.Controller;
using Assets.scripts.objectives;
using Assets.scripts.objectives.Action;
using Assets.scripts.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.scripts.GUI
{
    public class TipsNavigation : MonoBehaviour {

        public static int ActualTip;
        private static Text _textTip;
        private static Image _imgTip, _tipBg, _magCover;
        private static GameObject _previousBtn;
        private Text _nextBtnText, _previousBtnText;
        private static CP_Controller _playerselect;
        public Sprite JournalBg, HealthCover, GeneralCover, EndBtnSprite, NextBtnSprite;
        public GameObject InWorldCanvas;
        public Image BtnImage;
        public static string ActualSubject;

        private void Start ()
        {
            _textTip = GameObject.Find("tipText").GetComponent<Text>();
            _imgTip = GameObject.Find("tipImage").GetComponent<Image>();
            _previousBtn = GameObject.Find("PreviousButton");
            _nextBtnText = GameObject.Find("NextButton").GetComponentInChildren<Text>();
            _previousBtnText = _previousBtn.GetComponentInChildren<Text>();
            _textTip.gameObject.SetActive(false);
            _imgTip.gameObject.SetActive(false);
            _previousBtn.SetActive(false);
            _tipBg = GameObject.Find("tipBG").GetComponent<Image>();
            _magCover = GameObject.Find("magCover").GetComponent<Image>();
            _playerselect = CP_Controller.Instance;
            InWorldCanvas = GameObject.Find("InWorldCanvas");
            _previousBtnText.text = Language.previous;
            BtnImage.sprite = EndBtnSprite;
            _nextBtnText.text = Language.finish;
            _nextBtnText.rectTransform.offsetMin = new Vector2(69f, 243f);
            _nextBtnText.rectTransform.offsetMax = new Vector2(-49f, -247f);
            InWorldCanvas.SetActive(false);  
        }
	
        private void Update ()
        {
            if (LoadTipsContent.TipsObj)
            {
                InWorldCanvas.SetActive(true);
                if (LoadTipsContent.TipType != 3)
                {
                    _textTip.gameObject.SetActive(true);

                    if (LoadTipsContent.TipType == 0)
                    {
                        _textTip.text = LoadTipsContent.ShortTextTipsList[ActualTip];
                        ActualSubject = LoadTipsContent.ShortSubjectsList[ActualTip];
                    }
                    else if (LoadTipsContent.TipType == 1)
                    {
                        _textTip.text = LoadTipsContent.MediumTextTipsList[ActualTip];
                        ActualSubject = LoadTipsContent.MediumSubjectsList[ActualTip];
                    }
                    else if (LoadTipsContent.TipType == 2)
                    {
                        _textTip.text = LoadTipsContent.LongTextTipsList[ActualTip];
                        ActualSubject = LoadTipsContent.LongSubjectsList[ActualTip];
                    }
                    //TextGenerator tempText = _textTip.cachedTextGenerator;
                    //print("tempText: " + tempText.lineCount);
                    _magCover.gameObject.SetActive(false);
                    _tipBg.sprite = JournalBg;
                    
                }
                else
                {
                    _tipBg.sprite = null;
                    _magCover.gameObject.SetActive(true);
                    _imgTip.gameObject.SetActive(true);
                    _imgTip.sprite = LoadTipsContent.ImagesTipsList[ActualTip];
                    ActualSubject = LoadTipsContent.ImagesTipsList[ActualTip].name;
                    _magCover.sprite = _imgTip.sprite.name.Contains("health") ? HealthCover : GeneralCover;
                }

                

                //if (ActualTip == LoadTipsContent.Tips.Count - 1)
                //{
                //    //_endBtn.SetActive(true);
                //    //_nextBtn.SetActive(false);
                    

                //    BtnImage.sprite = EndBtnSprite;
                //    _nextBtnText.text = Language.finish;
                //    _nextBtnText.rectTransform.offsetMin = new Vector2(69f, 243f);
                //    _nextBtnText.rectTransform.offsetMax = new Vector2(-49f, -247f);
                    
                //}
                //else
                //{
                //    //_endBtn.SetActive(false);
                //    //_nextBtn.SetActive(true);
                //    BtnImage.sprite = NextBtnSprite;
                //    _nextBtnText.text = Language.next;
                //    _nextBtnText.rectTransform.offsetMin = new Vector2(69f, 77f);
                //    _nextBtnText.rectTransform.offsetMax = new Vector2(-49f, -413f);
                //}  
            }
        }

        private static void CompleteTip(bool ok)
        {
            LoadTipsContent.TipCompleted = ok;
            _textTip.gameObject.SetActive(false);
            _imgTip.gameObject.SetActive(false);
        }

        public static void NextTip()
        {
            //if (ActualTip == LoadTipsContent.Tips.Count - 1)
            //{
                CompleteTip(true);
                _magCover.gameObject.SetActive(false);
                _tipBg.gameObject.SetActive(false);
            //}
            //else
            //{
            //    _previousBtn.SetActive(true);
            //    ActualTip ++;
            //}

            _playerselect.Timer.SetActive(false);
            _playerselect._timerActivated = false;
            _playerselect.Selected = false;
        }
        
        //public static void PreviousTip()
        //{
        //    ActualTip--;

        //    if (ActualTip == 0)
        //    {
        //        _playerselect.Timer.SetActive(false);
        //        _playerselect._timerActivated = false;
        //        _playerselect.Selected = false;
        //        _previousBtn.SetActive(false);
        //    }
        //}
    }
}
