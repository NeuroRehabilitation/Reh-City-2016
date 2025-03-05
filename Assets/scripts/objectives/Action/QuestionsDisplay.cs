using Assets.scripts.GUI;
using Assets.scripts.RehaTask.RehaTaskGUI;
using Assets.scripts.RehaTask.RehaTask_GameLogic;
using Assets.scripts.Settings;
using Assets.scripts.UDP;
using UnityEngine;

namespace Assets.scripts.objectives.Action
{
    public class QuestionsDisplay : ActionObjective
    {
        public QuestionsDisplay(int type, int tipNumber, int questions)
        {
            description = childlist[6].InnerText;
            Performance = -1;
            name = "Questions";

            switch (type)
            {
                case 0:
                    do
                    {
                        var rand = Random.Range(0, LoadQuestions.ShortTextQuestions[tipNumber].Count);

                        if (!QuestionsList.Contains(LoadQuestions.ShortTextQuestions[tipNumber][rand]))
                        {
                            QuestionsList.Add(LoadQuestions.ShortTextQuestions[tipNumber][rand]);
                            QuestionsNumbersList.Add(rand);
                            SubjectsList.Add(LoadQuestions.ShortTextSubjects[tipNumber][rand]);
                        }
                        
                    } while (QuestionsList.Count < questions);
                    break;
                case 1:
                    do
                    {
                        var rand = Random.Range(0, LoadQuestions.MediumTextQuestions[tipNumber].Count);
                        if (!QuestionsList.Contains(LoadQuestions.MediumTextQuestions[tipNumber][rand]))
                        {
                            QuestionsList.Add(LoadQuestions.MediumTextQuestions[tipNumber][rand]);
                            QuestionsNumbersList.Add(rand);
                            SubjectsList.Add(LoadQuestions.MediumTextSubjects[tipNumber][rand]);
                        }
                    } while (QuestionsList.Count < questions);
                    break;
                case 2:
                    do
                    {
                        var rand = Random.Range(0, LoadQuestions.LongTextQuestions[tipNumber].Count);
                        if (!QuestionsList.Contains(LoadQuestions.LongTextQuestions[tipNumber][rand]))
                        {
                            QuestionsList.Add(LoadQuestions.LongTextQuestions[tipNumber][rand]);
                            QuestionsNumbersList.Add(rand);
                            SubjectsList.Add(LoadQuestions.LongTextSubjects[tipNumber][rand]);
                        }
                    } while (QuestionsList.Count < questions);
                    break;
                case 3:
                    do
                    {
                        var rand = Random.Range(0, LoadQuestions.ImageQuestions[tipNumber].Count);
                        if (!QuestionsList.Contains(LoadQuestions.ImageQuestions[tipNumber][rand]))
                        {
                            QuestionsList.Add(LoadQuestions.ImageQuestions[tipNumber][rand]);
                            QuestionsNumbersList.Add(rand);
                            SubjectsList.Add(LoadQuestions.ImageSubjects[tipNumber][rand]);
                        }
                    } while (QuestionsList.Count < questions);
                    break;
            }
        }
    
        public override void CheckForCompletion()
        {
            if ((LoadQuestions.QuestionsCompleted || Controller.Controller.B6()) && !completed)
            {
                ObjTime = DrawObjectiveList.TaskTime;
                if (QuestionsNavigation.WrongAnswers == 0)
                    Performance = 100.0f;
                else
                {
                    var dif = QuestionsList.Count - QuestionsNavigation.WrongAnswers;
                    Performance = dif * 100.0f / QuestionsList.Count;
                    if (Performance < 0)
                        Performance = 0;
                }

                PerformanceProcessor.MemoryStoriesPerform = Performance;
                TaskSummary.SaveTaskSummary(name, 0,0, QuestionsList.Count - QuestionsNavigation.WrongAnswers, QuestionsNavigation.WrongAnswers, 0, 0, Performance, ObjTime, 0);

                //Debug.Log("Total questions: " + QuestionsList.Count + " ; wrong answers: " + QuestionsNavigation.WrongAnswers + "; collection performance: " + Performance);
                QuestionsList.Clear();
                QuestionsNumbersList.Clear();
                QuestionsNavigation.ActualQuestion = 0;
                QuestionsNavigation.WrongAnswers = 0;
                Application.LoadLevel(LoadQuestions.CurrentScene);
                
                completed = true;
                LoadSaveSettings.SaveSettingsInfo(name);
            }

            if (Application.loadedLevelName == LoadQuestions.CurrentScene)
            {
                if (Application.loadedLevelName == "Home" || Application.loadedLevelName == "FashionStore" ||
                    Application.loadedLevelName == "Park")
                {
                    LoadGame.SessionSubCategory.RemoveAt(LoadGame.SessionSubCategory.Count - 1);
                }
                LoadQuestions.QuestionsCompleted = false;
                CanAddNextObjective = true;
                LoadQuestions.CurrentScene = "";
            }
        
            base.CheckForCompletion();
        }
    }
}
