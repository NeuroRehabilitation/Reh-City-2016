using Assets.scripts.GUI;
using Assets.scripts.Manager;
using Assets.scripts.Models;
using UnityEngine;

namespace Assets.scripts
{
    public class PerformanceProcessor : MonoBehaviour {

        public static float ActionSeqPerform;
        public static float CancellationPerform;
        public static float CatPerform;
        public static float PairsPerform;
        public static float MazesPerform;
        public static float MemoryStoriesPerform;
        public static float NumSeqPerform;
        public static float ProbSolvPerform;

        public static bool ActionSeq, Cancellation, Cat, Pairs, MemoryStories, NumSeq, ProbSolv;

        public static float LevelPerformance;

        public static int NumberOfLocations, DomainsDivisor;
        public static Model CurrentProfileModel;

        public static bool FirstTimeProcessing = true;

        public static void ProcessPerformance()
        {
            if(!FirstTimeProcessing || NumberOfLocations == 7)
                ResetPerformanceVariables();
            else if (NumberOfLocations > 0 && NumberOfLocations < 7)
                DomainsDivisor -= 1;

            Debug.Log("Loaded data from file: NumberOfLocations: " + NumberOfLocations + "MazesPerform: " + MazesPerform + " ; MemoryStoriesPerform: " + MemoryStoriesPerform + " ; ActionSeqPerform: " + ActionSeqPerform + " ; CatPerform: " + CatPerform + " ; PairsPerform: " + PairsPerform + " ; CancellationPerform: " +
                               CancellationPerform + " ; NumSeqPerform: " + NumSeqPerform + " ; ProbSolvPerform: " + ProbSolvPerform + " ; DomainsDivisor: " + DomainsDivisor);
            LevelPerformance = MemoryStoriesPerform + ActionSeqPerform + CatPerform + PairsPerform + CancellationPerform +
                               NumSeqPerform + ProbSolvPerform;

            for (var i = 0; i < ObjectiveInstatiation.GetObjectivesList().Count; i++)
            {
                if (ObjectiveInstatiation.GetObjectivesList()[i].Performance > -1)
                {
                    if (ObjectiveInstatiation.GetObjectivesList()[i].ToString().Contains("Location"))
                    {
                        NumberOfLocations ++;
                        MazesPerform += ObjectiveInstatiation.GetObjectivesList()[i].Performance;
                    }
                    else if (ObjectiveInstatiation.GetObjectivesList()[i].ToString().Contains("Question"))
                    {
                        MemoryStoriesPerform = ObjectiveInstatiation.GetObjectivesList()[i].Performance;
                        LevelPerformance += MemoryStoriesPerform;
                        MemoryStories = true;
                        DomainsDivisor++;
                    }
                    else if (ObjectiveInstatiation.GetObjectivesList()[i].name.Contains("Home"))
                    {
                        ActionSeqPerform = ObjectiveInstatiation.GetObjectivesList()[i].Performance;
                        LevelPerformance += ActionSeqPerform;
                        ActionSeq = true;
                        DomainsDivisor++;
                    }
                    else if (ObjectiveInstatiation.GetObjectivesList()[i].name.Contains("FashionStore"))
                    {
                        CatPerform = ObjectiveInstatiation.GetObjectivesList()[i].Performance;
                        LevelPerformance += CatPerform;
                        Cat = true;
                        DomainsDivisor++;
                    }
                    else if (ObjectiveInstatiation.GetObjectivesList()[i].name.Contains("Park"))
                    {
                        PairsPerform = ObjectiveInstatiation.GetObjectivesList()[i].Performance;
                        LevelPerformance += PairsPerform;
                        Pairs = true;
                        DomainsDivisor++;
                    }
                    else if (ObjectiveInstatiation.GetObjectivesList()[i].ToString().Contains("Collection") &&
                             !ObjectiveInstatiation.GetObjectivesList()[i].ToString().Contains("SMItem"))
                    {
                        CancellationPerform = ObjectiveInstatiation.GetObjectivesList()[i].Performance;
                        LevelPerformance += CancellationPerform;
                        Cancellation = true;
                        DomainsDivisor++;
                    }
                    else if (ObjectiveInstatiation.GetObjectivesList()[i].ToString().Contains("EnterCode"))
                    {
                        NumSeqPerform = ObjectiveInstatiation.GetObjectivesList()[i].Performance;
                        LevelPerformance += NumSeqPerform;
                        NumSeq = true;
                        DomainsDivisor++;
                    }
                    else if (ObjectiveInstatiation.GetObjectivesList()[i].ToString().Contains("Receipt"))
                    {
                        ProbSolvPerform = ObjectiveInstatiation.GetObjectivesList()[i].Performance;
                        LevelPerformance += ProbSolvPerform;
                        ProbSolv = true;
                        DomainsDivisor++;
                    }
                }
            }
            if (NumberOfLocations > 0)
            {
                MazesPerform = MazesPerform/NumberOfLocations;
                LevelPerformance += MazesPerform;
                DomainsDivisor++;
            }
            
            if (DomainsDivisor > 0)
            {
                Debug.Log("LevelPerformance before dividing by " + DomainsDivisor + ": " + LevelPerformance);
                LevelPerformance = LevelPerformance/DomainsDivisor;
            }
            Debug.Log("NumberOfLocations: " + NumberOfLocations + "MazesPerform: " + MazesPerform + " ; MemoryStoriesPerform: " + MemoryStoriesPerform + " ; ActionSeqPerform: " + ActionSeqPerform + " ; CatPerform: " + CatPerform + " ; PairsPerform: " + PairsPerform + " ; CancellationPerform: " +
                               CancellationPerform + " ; NumSeqPerform: " + NumSeqPerform + " ; ProbSolvPerform: " + ProbSolvPerform + " ; DomainsDivisor: " + DomainsDivisor);

            Debug.Log("LevelPerformance after dividing: " + LevelPerformance);

            if (DomainsDivisor >= 8)
            {
                if (LevelPerformance >= 70.0f)
                    CurrentProfileModel.Difficulty += 0.5f;
                else if (LevelPerformance <= 50.0f)
                {
                    CurrentProfileModel.Difficulty -= 0.5f;
                    if (CurrentProfileModel.Difficulty < 1)
                        CurrentProfileModel.Difficulty = 1;
                }
                FirstTimeProcessing = false;
            }
            LoadSaveSettings.SaveSettingsInfo();
            
        }

        private static void ResetPerformanceVariables()
        {
            Debug.Log("reseting performances");
            ActionSeq = false;
            Cancellation = false;
            Cat = false;
            Pairs = false;
            MemoryStories = false;
            NumSeq = false;
            ProbSolv = false;

            ActionSeqPerform = 0;
            CancellationPerform = 0;
            MazesPerform = 0;
            PairsPerform = 0;
            CatPerform = 0;
            MemoryStoriesPerform = 0;
            NumSeqPerform = 0;
            ProbSolvPerform = 0;

            DomainsDivisor = 0;
            NumberOfLocations = 0;
        }
    }
}
 