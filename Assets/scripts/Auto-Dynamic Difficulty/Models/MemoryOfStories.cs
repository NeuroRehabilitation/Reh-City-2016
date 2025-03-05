using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.scripts.Models
{
    public class MemoryOfStories : Model {

        private readonly int _questions;
        private readonly int _type;
        private readonly int _size;

        public MemoryOfStories(int quest, int type)
        {
            _size = type;
            _questions = quest;
            if (_size == 0)
            {
                if (_questions > 4)
                    _questions = 4;
            }
            if (_size == 1)
            {
                if (_questions > 6)
                    _questions = 6;
            }
            if (_size == 2)
            {
                if (_questions > 10)
                    _questions = 10;
            }

            if (type == 3)
            {
                _size = 1;
                if (_questions > 9)
                    _questions = 9;
            }
            

            _type = type == 3 ? 0 : 1;
        }

        public override float[] CalculateModel()
        {
            ExFunctions = (float)Math.Round(1.9 + _size * 1.45, 1, MidpointRounding.AwayFromZero);
            Attention = (float)Math.Round(2.618 + _type * 1.025 + _questions * 0.454, 1, MidpointRounding.AwayFromZero);
            Memory = (float)Math.Round(1.95 + _type * 1.05 + _questions * 0.5, 1, MidpointRounding.AwayFromZero);
            Language = (float)Math.Round(1.461 + _type * 1.575 + _questions * 0.382, 1, MidpointRounding.AwayFromZero);
            Difficulty = (float)Math.Round(1.298 + _type * 0.937 + _questions * 0.545, 1, MidpointRounding.AwayFromZero);

            var modelArray = new[] { Attention, Memory, ExFunctions, Language, Difficulty };

            return modelArray;
        }

        public static List<string> GenerateTraining(float attention, float memory, float executivefunctions,
            float language, float difficulty, bool closeMatch)
        {
            List<string> closest = null;

            const int elements = 11;
            const int categories = 4;
            
            var error = 100000000.0f;

            for (var iElements = 1; iElements < elements; iElements++)
                for (var iCues = 0; iCues < categories; iCues++)
                {
                    var model = new MemoryOfStories(iElements, iCues);
                    var modelParam = model.CalculateModel();
                    //Debug.Log("Model(" + iElements + "," + iCues+ ") = " + "Model Attention: " + modelParam[0] + "Model Memory: " + modelParam[1] + "Model ExFunctions: " + modelParam[2] + "Model Language: " + modelParam[3] + "Model Difficulty: " + modelParam[4]);
                    var sum = (float)(Math.Pow(modelParam[0] - attention, 2) + Math.Pow(modelParam[1] - memory, 2) + Math.Pow(modelParam[2] - executivefunctions, 2) + Math.Pow(modelParam[3] - language, 2) + Math.Pow(modelParam[4] - difficulty, 2));
                    //Debug.Log(Math.Pow(modelParam[0] - attention, 2) + " + " + Math.Pow(modelParam[1] - memory, 2) + " + " + Math.Pow(modelParam[2] - executivefunctions, 2) + " + " + Math.Pow(modelParam[3] - language, 2) + " + " + Math.Pow(modelParam[4] - difficulty, 2) + " = "  + sum);
                    if (error > sum)
                    {
                        closest = new List<string> { iElements.ToString(), iCues.ToString()};
                        error = sum;
                        //Debug.Log("Error:" + error);
                    }
                }

            return closest;
        }
    }
}
