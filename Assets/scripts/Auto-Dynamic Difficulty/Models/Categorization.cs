using System;
using System.Collections.Generic;

namespace Assets.scripts.Models
{
    public class Categorization : Model
    {
        private readonly int _categories;
        private readonly int _elements;

        public Categorization(int cat, int elem)
        {
            _categories = cat;
            _elements = elem;
        }

        public override float[] CalculateModel()
        {
            ExFunctions = (float)Math.Round(1.136 + _categories * 0.989, 1, MidpointRounding.AwayFromZero);
            Attention = (float)Math.Round(-3.26 + _categories * 3.75 + _categories * _elements * -0.41, 1, MidpointRounding.AwayFromZero);
            Memory = (float)Math.Round(0.6 + _categories * 0.9, 1, MidpointRounding.AwayFromZero);
            Language = (float)Math.Round(1.914 + _categories * 0.586, 1, MidpointRounding.AwayFromZero);
            Difficulty = (float)Math.Round(0.234 + _categories * 1.165, 1, MidpointRounding.AwayFromZero);

            var modelArray = new[] { Attention, Memory, ExFunctions, Language, Difficulty };

            return modelArray;
        }

        public static List<string> GenerateTraining(float attention, float memory, float executivefunctions,
            float language, float difficulty, bool closeMatch)
        {
            List<string> closest = null;

            const int elements = 10;
            const int categories = 10;
            
            var error = 100000000.0f;

            for (var iElements = 2; iElements < elements; iElements++)
                for (var iCues = 2; iCues < categories; iCues++)
                {
                    var model = new Categorization(iElements, iCues);
                    var modelParam = model.CalculateModel();

                    var sum = (float)(Math.Pow(modelParam[0] - attention, 2) + Math.Pow(modelParam[1] - memory, 2) + Math.Pow(modelParam[2] - executivefunctions, 2) + Math.Pow(modelParam[3] - language, 2) + Math.Pow(modelParam[4] - difficulty, 2));

                    if (error > sum)
                    {
                        closest = new List<string> { iElements.ToString(), iCues.ToString()};
                        error = sum;
                    }
                }

            return closest;
        }
    }
}
