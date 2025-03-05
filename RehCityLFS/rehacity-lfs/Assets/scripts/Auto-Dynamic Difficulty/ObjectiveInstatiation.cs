using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Assets.scripts.bank.Others;
using Assets.scripts.MiniMapScripts.AStar;
using Assets.scripts.Models;
using Assets.scripts.objectives;
using Assets.scripts.objectives.Action;
using Assets.scripts.objectives.Collection;
using Assets.scripts.objectives.Location;
using Assets.scripts.RehaTask.RehaTask_GameLogic;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace Assets.scripts
{
    public class ObjectiveInstatiation : MonoBehaviour {

        private static readonly List<Objectives> ObjectivesList = new List<Objectives>();
        private static GameObject _initialPoint;
        private static readonly List<string> LocationsOrder = new List<string>();
        public static int RehaTaskExtraTime, RehaTaskTargetTime;
        private static NodeManager nodemanager;
        private static Objectives SavedQuestions;

        private void Start()
        {
            nodemanager = NodeManager.Instance;  
        }

        public static void GenerateNewLocationsSet(int maze)
        {
            var locations = GameObject.FindGameObjectsWithTag("Location").ToList();

            //checking if there are settings from a previous unfinished session and removes the completed tasks
            if (PerformanceProcessor.FirstTimeProcessing)
            {
                if (PerformanceProcessor.ActionSeq)
                    locations.Remove(GameObject.Find("Home"));
                if (PerformanceProcessor.Cancellation)
                {
                    locations.Remove(GameObject.Find("PostOffice"));
                    locations.Remove(GameObject.Find("Pharmacy"));
                }
                if (PerformanceProcessor.Cat)
                    locations.Remove(GameObject.Find("FashionStore"));
                if (PerformanceProcessor.Pairs)
                    locations.Remove(GameObject.Find("Park"));
                if (PerformanceProcessor.MemoryStories)
                    locations.Remove(GameObject.Find("Kiosk"));
                if (PerformanceProcessor.NumSeq)
                    locations.Remove(GameObject.Find("Bank"));
                if (PerformanceProcessor.ProbSolv)
                    locations.Remove(GameObject.Find("SuperMarket"));

                if (locations.Count == 0 || (locations.Count == 1 && locations[0].name == "Kiosk"))
                {
                    locations = GameObject.FindGameObjectsWithTag("Location").ToList();
                    PerformanceProcessor.ResetPerformanceVariables();
                }
            }

            //Debug.Log("Locations for the objectives set: " + locations.Count);

            _initialPoint = GameObject.FindGameObjectWithTag("Player");
            
            do
            {
                var loc = new List<Location>();
                
                for (var i = 0; i < locations.Count; i++)
                {
                    if (!LocationsOrder.Contains(locations[i].name) && _initialPoint!= null)
                    {
                        var one = nodemanager.CalculateStartNode(_initialPoint.transform.position);
                        var two = nodemanager.CalculateGoalNode(locations[i].transform.position);
                        
                        var crosses = PathFindingController.CrossingNodes(one, two);
                        var dist = Vector3.Distance(one.position, two.position);

                        if(Mathf.Abs(Vector3.Distance(_initialPoint.transform.position, two.position))>20)
                            loc.Add(new Location(locations[i].name, crosses, dist));
                    }
                }

                //orders the available locations by the number of crosses to easily get the range for comparison
                loc = loc.OrderBy(x => x.Crosses).ToList();

                var max = loc[loc.Count - 1].Crosses;
                var min = loc[0].Crosses;
                var locationsRange = max - min;
                var step = (int) Mathf.Round((maze - 3.0f)*locationsRange/7.0f + min);
                bool stepFound = false;
                
                //making sure that the calculated step exists in the list
                for (var i = 0; i < loc.Count; i++)
                {
                    if (loc[i].Crosses == step)
                    {
                        stepFound = true;
                    }
                }
                if (!stepFound)
                {
                    if (loc.Count == 1)
                    {
                        step = loc[0].Crosses;
                    }
                    else
                    {
                        var dif = 100;
                        var pos = 0;

                        for (var i = 0; i < loc.Count; i++)
                        {
                            if (Mathf.Abs(loc[i].Crosses - step) < dif)
                            {
                                dif = Mathf.Abs(loc[i].Crosses - step);
                                pos = i;
                            }
                        }

                        step = loc[pos].Crosses;
                    }
                }
                
                //removing locations that doesn't match the step (crossing nodes)
                loc.RemoveAll(s => s.Crosses != step);

                if (loc.Count == 1)
                {
                    LocationsOrder.Add(loc[0].Name);
                }
                else if (loc.Count>1)
                {
                    loc = loc.OrderBy(x => x.Distance).ToList();

                    var distRange = loc[loc.Count - 1].Distance - loc[0].Distance;

                    var stepScale = (int)Mathf.Round((step - min) * distRange / max + loc[0].Distance);
                    var distDif = 10000.0f;
                    var disPos = 0;

                    for (var i = 0; i < loc.Count; i++)
                    {
                        if (Mathf.Abs(loc[i].Distance - stepScale) < distDif)
                        {
                            distDif = Mathf.Abs(loc[i].Distance - stepScale);
                            disPos = i;
                        }    
                    }
                    LocationsOrder.Add(loc[disPos].Name);
                }
                
                _initialPoint = locations.SingleOrDefault(x => x.name == LocationsOrder[LocationsOrder.Count - 1]);
              
            } while (LocationsOrder.Count < locations.Count);     
        }
        
        public static void CalculateTasksParameters(Model profile)
        {
            //checking if the last objective before concluding the previous level has the questions objective
            ObjectivesList.Clear();

            if (SavedQuestions != null && SavedQuestions.ToString().Contains("QuestionsDisplay"))
                ObjectivesList.Add(SavedQuestions);

            LocationsOrder.Clear();

            //Debug.Log("Generating training for profile: attention: " + profile.Attention + " , memory" + profile.Memory +
            //          " , ExecFunctions: " + profile.ExFunctions + " , Lang: " + profile.Language + " , Difficulty: " +
            //          profile.Difficulty);

            var actionTraining = ActionSequencing.GenerateTraining(profile.Attention, profile.Memory, profile.ExFunctions, profile.Language, profile.Difficulty, false);
            //var associationTraining = Association.GenerateTraining(profile.Attention, profile.Memory, profile.ExFunctions, profile.Language, profile.Difficulty, false);
            var cancellation = Cancellation.GenerateTraining(profile.Attention, profile.Memory, profile.ExFunctions, profile.Language, profile.Difficulty, false);
            var categorization = Categorization.GenerateTraining(profile.Attention, profile.Memory, profile.ExFunctions, profile.Language, profile.Difficulty, false);
            //var context = Context.GenerateTraining(profile.Attention, profile.Memory, profile.ExFunctions, profile.Language, profile.Difficulty, false);
            var pairs = ImagePairs.GenerateTraining(profile.Attention, profile.Memory, profile.ExFunctions, profile.Language, profile.Difficulty, false);
            var mazesTraining = Mazes.GenerateTraining(profile.Attention, profile.Memory, profile.ExFunctions, profile.Language, profile.Difficulty, false);
            var memoryStories = MemoryOfStories.GenerateTraining(profile.Attention, profile.Memory, profile.ExFunctions, profile.Language, profile.Difficulty, false);
            var numSeq = NumericSequencing.GenerateTraining(profile.Attention, profile.Memory, profile.ExFunctions, profile.Language, profile.Difficulty, false);
            var probSolv = ProblemSolving.GenerateTraining(profile.Attention, profile.Memory, profile.ExFunctions, profile.Language, profile.Difficulty, false);
            /*
            var actionTrainingParam = "Action Sequencing parameters: " + actionTraining[0];
            //var associationParam = "association parameters: " + associationTraining[0];
            var cancellationParam = "cancellation parameters: " + cancellation[0];
            var categorizationParam = "categorization parameters: " + categorization[0];
            //var contextParam = "context parameters: " + context[0];
            var pairsParam = "pairs parameters: " + pairs[0];
            var mazesParam = "mazes parameters: " + mazesTraining[0];
            var memoryStoriesParam = "memoryStories parameters: " + memoryStories[0];
            var numSeqParam = "numSeq parameters: " + numSeq[0];
            var probSolvParam = "probSolv parameters: " + probSolv[0];

            //2 parameters
            for (var i = 1; i < actionTraining.Count; i++)   
            {
                actionTrainingParam = actionTrainingParam + "; "  + actionTraining[i];
                categorizationParam = categorizationParam + "; " + categorization[i];
                memoryStoriesParam = memoryStoriesParam + "; " + memoryStories[i];
            }
            
            //3 parameters
            for (var i = 1; i < probSolv.Count; i++)
            {
                probSolvParam = probSolvParam + "; " + probSolv[i];
            }
            //4 parameters
            for (var i = 1; i < cancellation.Count; i++)
            {
                cancellationParam = cancellationParam + "; " + cancellation[i];
            }
            //5 parameters (all others only receive one parameter)
            for (var i = 1; i < numSeq.Count; i++)
            {
                numSeqParam = numSeqParam + "; " + numSeq[i];
            }

            //Debug.Log(actionTrainingParam);
            //Debug.Log(associationParam);
            //Debug.Log(cancellationParam);
            //Debug.Log(categorizationParam);
            //Debug.Log(contextParam);
            //Debug.Log(pairsParam);
            //Debug.Log(mazesParam);
            //Debug.Log(memoryStoriesParam);
            //Debug.Log(numSeqParam);
            //Debug.Log(probSolvParam);
            /*
            var actionModel = new ActionSequencing(int.Parse(actionTraining[0]), int.Parse(actionTraining[1]));
            //var associationModel = new Association(int.Parse(associationTraining[0]));
            var cancellationModel = new Cancellation(int.Parse(cancellation[0]), int.Parse(cancellation[1]), int.Parse(cancellation[2]), int.Parse(cancellation[3]));
            var categorizationModel = new Categorization(int.Parse(categorization[0]), int.Parse(categorization[1]));
            //var contextModel = new Context(int.Parse(context[0]));
            var pairsModel = new ImagePairs(int.Parse(pairs[0]));
            var mazesModel = new Mazes(int.Parse(mazesTraining[0]));
            var memoryStoriesModel = new MemoryOfStories(int.Parse(memoryStories[0]), int.Parse(memoryStories[1]));
            var numSeqModel = new NumericSequencing(int.Parse(numSeq[0]), int.Parse(numSeq[1]), int.Parse(numSeq[2]), int.Parse(numSeq[3]), int.Parse(numSeq[4]));
            var probSolvModel = new ProblemSolving(int.Parse(probSolv[0]), int.Parse(probSolv[1]), int.Parse(probSolv[2]));
            
            var actionModelParameters = actionModel.CalculateModel();
            //var associationModelParameters = associationModel.CalculateModel();
            var cancellationModelParameters = cancellationModel.CalculateModel();
            var categorizationModelParameters = categorizationModel.CalculateModel();
            //var contextModelParameters = contextModel.CalculateModel();
            var pairsModelParameters = pairsModel.CalculateModel();
            var mazesModelParameters = mazesModel.CalculateModel();
            //var memoryStoriesModelParameters = memoryStoriesModel.CalculateModel();
            //var numSeqModelParameters = numSeqModel.CalculateModel();
            var probSolvModelParameters = probSolvModel.CalculateModel();
            */
            //var actionModelParam = "Action Sequencing model: " + actionModelParameters[0];
            //var associationModelParam = "association Model: " + associationModelParameters[0];
            //var cancellationModelParam = "cancellation model: " + cancellationModelParameters[0];
            //var categorizationModelParam = "categorization model: " + categorizationModelParameters[0];
            //var contextModelParam = "context model: " + contextModelParameters[0];
            //var pairsModelParam = "pairs model: " + pairsModelParameters[0];
            //var mazesModelParam = "mazes model: " + mazesModelParameters[0];
            //var memoryStoriesModelParam = "memoryStories model: " + memoryStoriesModelParameters[0];
            //var numSeqModelParam = "numSeq model: " + numSeqModelParameters[0];
            //var probSolvModelParam = "probSolv model: " + probSolvModelParameters[0];

            //for (var i = 1; i < actionModelParameters.Length; i++)
            // {
            //    actionModelParam = actionModelParam + "; " + actionModelParameters[i];
            //    associationModelParam = associationModelParam + "; "  + associationModelParameters[i];
            // cancellationModelParam = cancellationModelParam + "; "   + cancellationModelParameters[i];
            //    categorizationModelParam = categorizationModelParam + "; "   + categorizationModelParameters[i];
            //    contextModelParam = contextModelParam + "; "   + contextModelParameters[i];
            //    pairsModelParam = pairsModelParam + "; "   + pairsModelParameters[i];
            //mazesModelParam = mazesModelParam + "; " + mazesModelParameters[i];
            //    memoryStoriesModelParam = memoryStoriesModelParam + "; " + memoryStoriesModelParameters[i];
            //    numSeqModelParam = numSeqModelParam + "; " + numSeqModelParameters[i];
            //probSolvModelParam = probSolvModelParam + "; " + probSolvModelParameters[i];
            //}

            //Debug.Log(actionModelParam);
            //Debug.Log(associationModelParam);
            //Debug.Log(cancellationModelParam);
            //Debug.Log(categorizationModelParam);
            //Debug.Log(contextModelParam);
            //Debug.Log(pairsModelParam);
            //Debug.Log(mazesModelParam);
            //Debug.Log(memoryStoriesModelParam);
            //Debug.Log(numSeqModelParam);
            //Debug.Log(probSolvModelParam);

            GenerateNewLocationsSet(int.Parse(mazesTraining[0]));
            var cancellationSet = false;
            for (var i = 0; i < LocationsOrder.Count; i++)
            {
                switch (LocationsOrder[i])
                {
                    case "Home":
                        SetActionSequencingObjective(int.Parse(actionTraining[0]), int.Parse(actionTraining[1]));
                        break;
                    case "Park":
                        SetPairsObjective(int.Parse(pairs[0]));
                        break;
                    case "FashionStore":
                        SetCategorizationObjective(int.Parse(categorization[0]), int.Parse(categorization[1]));
                        break;
                    case "Kiosk":
                        SetMemOfStoriesObjective(int.Parse(memoryStories[0]), int.Parse(memoryStories[1]));
                        break;
                    case "SuperMarket":
                        SetProblemSolvingObjective(int.Parse(probSolv[0]), int.Parse(probSolv[1]), int.Parse(cancellation[0]), int.Parse(cancellation[1]));
                        break;
                    case "Pharmacy":
                        if (!cancellationSet)
                        {
                            SetCancellationObjective(int.Parse(cancellation[0]), int.Parse(cancellation[1]), 0);
                            cancellationSet = true;
                        }
                        break;
                    case "PostOffice":
                        if (!cancellationSet)
                        {
                            SetCancellationObjective(int.Parse(cancellation[0]), int.Parse(cancellation[1]), 1);
                            cancellationSet = true;
                        }
                        break;
                    case "Bank":
                        SetNumericSequencigObjective(int.Parse(numSeq[0]), int.Parse(numSeq[4]), int.Parse(numSeq[01]), int.Parse(numSeq[02]));
                        break;
                }
            }

            SavedQuestions = new Objectives();

            if (ObjectivesList[ObjectivesList.Count - 1].ToString().Contains("QuestionsDisplay"))
            {
                SavedQuestions = ObjectivesList[ObjectivesList.Count - 1];
                ObjectivesList.RemoveAt(ObjectivesList.Count - 1);
            }
        }

        private static void SetProblemSolvingObjective(int size, int tens, int elements, int prob)
        {
            Objectives task = null;

            var targets = new List<int>();

            Objectives loc = new GotoShopping();

            if (size == 1)
            {
                targets.Add(LocationsStock.GetLocationCancellationItems("SuperMarket", elements, prob, 1)[0]);
                var distractors = LocationsStock.GetLocationCancellationItems("SuperMarket", elements, prob, 1)[1];
                task = new CollectSMItem(targets[0], distractors);
            }
            else
            {
                var smParam = LocationsStock.GetLocationCancellationItems("SuperMarket", elements, prob, size);
                
                switch (size)
                {
                    case 2:
                        task = new CollectMultipleSMItems(smParam[0], smParam[1], smParam[2]);
                        break;
                    case 3:
                        task = new CollectMultipleSMItems(smParam[0], smParam[1], smParam[2], smParam[3]);
                        break;
                    case 4:
                        task = new CollectMultipleSMItems(smParam[0], smParam[1], smParam[2], smParam[3], smParam[4]);
                        break;
                }
            }

            CheckForQuestionsObjective(loc, task);
            var wrongSum = tens == 0;
            ObjectivesList.Add(new ReceiptDisplay(wrongSum));
        }

        private static void SetCancellationObjective(int elements, int prob, int locat)
        {
            var location = locat;
            
            Objectives loc;
            Objectives task;

            int targets;
            int distractors;

            if (location == 0)
            {
                loc = new GotoPharmacy();
                targets = LocationsStock.GetLocationCancellationItems("Pharmacy", elements, prob, 1)[0];
                distractors = LocationsStock.GetLocationCancellationItems("Pharmacy", elements, prob, 1)[1];
                task = new CollectPHItem(targets, distractors);
            }
            else
            {
                loc = new GotoPostoffice();
                targets = LocationsStock.GetLocationCancellationItems("PostOffice", elements, prob, 1)[0];
                distractors = LocationsStock.GetLocationCancellationItems("PostOffice", elements, prob, 1)[1];
                task = new CollectPOItem(targets, distractors);
            }

            CheckForQuestionsObjective(loc, task);
        }

        private static void SetNumericSequencigObjective(int size, int missingNumbers, int step, int order)
        {
            var multiplier = Random.Range(0.0f,1.0f);

            if (multiplier > 0.5f)
                multiplier = 1;
            else
                multiplier = -1;

            if (order == 0)
                multiplier = 1;
            else if (order == 1)
                multiplier = -1;

            var initialDigit = Random.Range(1, 100);

            if(multiplier < 0 && initialDigit < size * step)
                initialDigit = Random.Range(size * step, 100);

            Objectives loc = new GotoBank(); ;
            Objectives task = new EnterCode(size, missingNumbers, initialDigit, (int)(step * multiplier));

            CheckForQuestionsObjective(loc, task);

            SetNewBankObjective();
        }

        private static void SetNewBankObjective()
        {
            var action = Random.Range(0, 2);

            if (action == 0)
            {
                var moneyTask = Random.Range(0, 4);
                var amount = 20;

                switch (moneyTask)
                {
                    case 0:
                        amount = 20;
                        break;
                    case 1:
                        amount = 40;
                        break;
                    case 2:
                        amount = 60;
                        break;
                    case 3:
                        amount = 80;
                        break;
                }

                ObjectivesList.Add(new WithDrawMoney(amount));
            }
            else
            {
                var payTask = Random.Range(0, 3);
                var billToPay = "Electricity";

                switch (payTask)
                {
                    case 0:
                        billToPay = "Electricity";
                        break;
                    case 1:
                        billToPay = "Water";
                        break;
                    case 2:
                        billToPay = "Telephone";
                        break;
                }

                ObjectivesList.Add(new PayBill(billToPay));
            }
            
        }

        private static void SetActionSequencingObjective(int size, int type)
        {
            var levelTime = (int)Mathf.Round(RehaTaskExtraTime + size * TimerCount.StartTime + RehaTaskTargetTime * size / PerformanceProcessor.CurrentProfileModel.Difficulty);
            var display = type != 0;

            Objectives loc = new GoToHome();
            Objectives task = new PlayRehatask(size, display, levelTime);

            CheckForQuestionsObjective(loc, task);
        }

        private static void SetCategorizationObjective(int elements, int categories)
        {
            var levelTime = (int)Mathf.Round(RehaTaskExtraTime + elements * TimerCount.StartTime + RehaTaskTargetTime * elements / PerformanceProcessor.CurrentProfileModel.Difficulty);
            var distractors = elements*categories - elements;
            if(distractors + elements > 45)
                distractors = distractors - (distractors + elements - 45);

            Objectives loc = new GoToFashionStore();
            Objectives task = new PlayRehatask(elements, distractors, levelTime);

            CheckForQuestionsObjective(loc, task);
        }

        private static void SetPairsObjective(int pairs)
        {
            var levelTime = (int)Mathf.Round(RehaTaskExtraTime + pairs * 2 * TimerCount.StartTime + RehaTaskTargetTime * pairs * 2 / PerformanceProcessor.CurrentProfileModel.Difficulty);

            Objectives loc = new GoToPark();
            Objectives task = new PlayRehatask(pairs, levelTime);

            CheckForQuestionsObjective(loc, task);
        }

        private static void SetMemOfStoriesObjective(int quest, int type)
        {

            Objectives loc = new GoToKiosk();
            Objectives task = new TipDisplay(type);

            CheckForQuestionsObjective(loc, task);

            ObjectivesList.Add(new QuestionsDisplay(type, task.TipNumber, quest));  
        }

        private static void CheckForQuestionsObjective(Objectives loc, Objectives task)
        {
            if (ObjectivesList.Count > 0 &&
                ObjectivesList[ObjectivesList.Count - 1].ToString().Contains("QuestionsDisplay"))
            {
                var temp = ObjectivesList[ObjectivesList.Count - 1];
                ObjectivesList[ObjectivesList.Count - 1] = loc;
                ObjectivesList.Add(temp);
            }
            else
            {
                ObjectivesList.Add(loc);
            }
            ObjectivesList.Add(task);
        }

        public static List<Objectives> GetObjectivesList()
        {

            return ObjectivesList;
        } 
    }
}
