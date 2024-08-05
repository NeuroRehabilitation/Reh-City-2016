using System;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;

namespace Assets.scripts.Models
{
    public class ActionSequencing : Model {

        private readonly int _size;
        private readonly int _type;

        public ActionSequencing(int size, int type)
        {
            _size = size;
            _type = type;
        }

        public override float[] CalculateModel()
        {
            Attention = (float)Math.Round(2.9 + _size * 0.75 - _type * 1.1, 1, MidpointRounding.AwayFromZero);
            Memory = (float)Math.Round(1.507 + _size * 0.635, 1, MidpointRounding.AwayFromZero);
            ExFunctions = (float)Math.Round(2.838 + _size * 0.487, 1, MidpointRounding.AwayFromZero);
            Language = (float)Math.Round(3.325 + _size * 0.525 - _type * 1.2, 1, MidpointRounding.AwayFromZero);
            Difficulty = (float)Math.Round(1.95 + _size * 0.862 - _type * 1.325, 1, MidpointRounding.AwayFromZero);

            var modelArray = new[] {Attention, Memory, ExFunctions, Language, Difficulty};

            return modelArray;
        }

        public static List<string> GenerateTraining(float attention, float memory, float executivefunctions, float language, float difficulty, bool closeMatch)
        {
            
                 
            List<string> closest = null;
            const int elements = 7;
            const int categories = 2;

            var error = 100000000.0f;

            for (var iElements = 2; iElements < elements; iElements++)
            {
                for (var iCues = 0; iCues < categories; iCues++)
                {
                    var model = new ActionSequencing(iElements, iCues);
                    var modelParam = model.CalculateModel();
                    //Debug.Log("Model(" + iElements + "," + iCues+ ") = " + "Model Attention: " + modelParam[0] + "Model Memory: " + modelParam[1] + "Model ExFunctions: " + modelParam[2] + "Model Language: " + modelParam[3] + "Model Difficulty: " + modelParam[4]);
                    var sum =
                        (float)
                            (Math.Pow(modelParam[0] - attention, 2) + Math.Pow(modelParam[1] - memory, 2) +
                             Math.Pow(modelParam[2] - executivefunctions, 2) +
                             Math.Pow(modelParam[3] - language, 2) + Math.Pow(modelParam[4] - difficulty, 2));
                    //Debug.Log(Math.Pow(modelParam[0] - attention, 2) + " + " + Math.Pow(modelParam[1] - memory, 2) + " + " + Math.Pow(modelParam[2] - executivefunctions, 2) + " + " + Math.Pow(modelParam[3] - language, 2) + " + " + Math.Pow(modelParam[4] - difficulty, 2) + " = "  + sum);
                    if (error > sum)
                    {
                        closest = new List<string> {iElements.ToString(), iCues.ToString()};
                        error = sum;
                        //Debug.Log("Error:" + error);
                    }
                }
            }
            
            return closest;
        }

    }

}
