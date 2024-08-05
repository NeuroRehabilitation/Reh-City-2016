using Assets.scripts.GUI;
using UnityEngine;

namespace Assets.scripts.objectives.Action
{
    public class TipDisplay : ActionObjective
    {
        public TipDisplay(int type)
        {
            if (type == 3)
            {
                description = childlist[5].InnerText;
                TipNumber = Random.Range(0, LoadTipsContent.ImagesTipsList.Count);
            }
            else
            {
                description = childlist[4].InnerText;
                
                switch (type)
                {
                    case 0:
                        TipNumber = Random.Range(0, LoadTipsContent.ShortTextTipsList.Count);
                        break;
                    case 1:
                        TipNumber = Random.Range(0, LoadTipsContent.MediumTextTipsList.Count);
                        break;
                    case 2:
                        TipNumber = Random.Range(0, LoadTipsContent.LongTextTipsList.Count);
                        break;
                }
            }
            TipType = type;
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
            if (LoadTipsContent.TipCompleted)
            {
                completed = true;
                Application.LoadLevel("City");
                LoadTipsContent.TipCompleted = false;
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