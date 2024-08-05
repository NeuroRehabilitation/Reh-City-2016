using Assets.scripts.GUI;
using UnityEngine;

namespace Assets.scripts.objectives.Action
{
    public class QuestionsDisplay : ActionObjective
    {
        public QuestionsDisplay(int type, int tipNumber, int questions)
        {
            description = childlist[6].InnerText;
            Performance = -1;

            switch (type)
            {
                case 0:
                    do
                    {
                        var rand = Random.Range(0, LoadQuestions.ShortTextQuestions[tipNumber].Count);

                        if (!QuestionsList.Contains(LoadQuestions.ShortTextQuestions[tipNumber][rand]))
                        {
                            QuestionsList.Add(LoadQuestions.ShortTextQuestions[tipNumber][rand]);
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
                            SubjectsList.Add(LoadQuestions.ImageSubjects[tipNumber][rand]);
                        }
                    } while (QuestionsList.Count < questions);
                    break;
            }
        }
    
        public override void CheckForCompletion()
        {
            if (LoadQuestions.QuestionsCompleted && !completed)
            {
                if (QuestionsNavigation.WrongAnswers == 0)
                    Performance = 100.0f;
                else
                {
                    var dif = QuestionsList.Count - QuestionsNavigation.WrongAnswers;
                    Performance = dif * 100.0f / QuestionsList.Count;
                    if (Performance < 0)
                        Performance = 0;
                }

                Debug.Log("Total questions: " + QuestionsList.Count + " ; wrong answers: " + QuestionsNavigation.WrongAnswers + "; collection performance: " + Performance);
                QuestionsList.Clear();
                QuestionsNavigation.ActualQuestion = 0;
                QuestionsNavigation.WrongAnswers = 0;
                Application.LoadLevel(LoadQuestions.CurrentScene);
                completed = true;
            }

            if (Application.loadedLevelName == LoadQuestions.CurrentScene)
            {
                LoadQuestions.QuestionsCompleted = false;
                CanAddNextObjective = true;
            }
        
            base.CheckForCompletion();
        }
    }
}
