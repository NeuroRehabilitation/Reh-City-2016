using Assets.scripts.GUI;
using UnityEngine;

namespace Assets.scripts.objectives.Action
{
    public class TipDisplay : ActionObjective
    {
        public TipDisplay(int type)
        {
            TipNumber = -1;
            if (type == 3)
            {
                description = childlist[5].InnerText;
                do
                {
                    var rand = Random.Range(0, LoadTipsContent.ImagesTipsList.Count);

                    if (!LoadTipsContent.SessionImagesTips.Contains(rand))
                    {
                        TipNumber = rand;
                    }

                } while (TipNumber == -1);
            }
            else
            {
                description = childlist[4].InnerText;
                
                switch (type)
                {
                    case 0:
                        do
                        {
                            var rand = Random.Range(0, LoadTipsContent.ShortTextTipsList.Count);

                            if (!LoadTipsContent.SessionShortTextTips.Contains(rand))
                            {
                                TipNumber = rand;
                            }
                        } while (TipNumber == -1);
                        break;
                    case 1:
                        do
                        {
                            var rand = Random.Range(0, LoadTipsContent.MediumTextTipsList.Count);

                            if (!LoadTipsContent.SessionMediumTextTips.Contains(rand))
                            {
                                TipNumber = rand;
                            }
                        } while (TipNumber == -1);
                        break;
                    case 2:
                        do
                        {
                            var rand = Random.Range(0, LoadTipsContent.LongTextTipsList.Count);

                            if (!LoadTipsContent.SessionLongTextTips.Contains(rand))
                            {
                                TipNumber = rand;
                            }
                        } while (TipNumber == -1);
                        break;
                }
            }
            
            TipType = type;
            //Debug.Log("TipType: " + TipType + " ; TipNumber: " + TipNumber);
        }
        /*
        public TipDisplay(string Type, int content, float time, int qGroup)
        {
            if (Type == "text")
            {
                description = childlist[4].InnerText;
                LoadTipsContent.TextTip.gameObject.SetActive(true);
                LoadTipsContent.TextTip.text = LoadTipsContent.TextTipsList[content];
            }
            else if (Type == "image")
            {
                description = childlist[5].InnerText;
                LoadTipsContent.ImgTip.gameObject.SetActive(true);
                LoadTipsContent.ImgTip.sprite = LoadTipsContent.ImagesTipsList[content];
            }

            LoadQuestions.QuestionsDisplayTimer = time;
            LoadQuestions.QuestionsGroup = qGroup;
        }*/
    
        public override void CheckForCompletion()
        {
            if (LoadTipsContent.TipCompleted || Controller.Controller.B6())
            {
                completed = true;
                Application.LoadLevel("City");
                LoadTipsContent.TipCompleted = false;
                switch (TipType)
                {
                    case 0:
                        LoadTipsContent.SessionShortTextTips.Add(TipNumber);
                        break;
                    case 1:
                        LoadTipsContent.SessionMediumTextTips.Add(TipNumber);
                        break;
                    case 2:
                        LoadTipsContent.SessionLongTextTips.Add(TipNumber);
                        break;
                    case 3:
                        LoadTipsContent.SessionImagesTips.Add(TipNumber);
                        break;
                }
                //Debug.Log("SessionShortTextTips: " + LoadTipsContent.SessionShortTextTips.Count);
                //Debug.Log("SessionMediumTextTips: " + LoadTipsContent.SessionMediumTextTips.Count);
                //Debug.Log("SessionLongTextTips: " + LoadTipsContent.SessionLongTextTips.Count);
                //Debug.Log("SessionImagesTips: " + LoadTipsContent.SessionImagesTips.Count);
            }
            if (Application.loadedLevelName == "City")
            {
                CanAddNextObjective = true;
                //LoadQuestions.ActivateQuestionsTimer = true;
            }
            base.CheckForCompletion();
        }
    }
}