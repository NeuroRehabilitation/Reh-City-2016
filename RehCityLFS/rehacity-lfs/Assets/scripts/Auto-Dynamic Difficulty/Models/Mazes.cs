using System;
using System.Collections.Generic;

namespace Assets.scripts.Models
{
    public class Mazes : Model {

        private readonly float _elements;

        public Mazes(int elem)
        {
            _elements = elem * 0.5f;
        }

        public override float[] CalculateModel()
        {
            ExFunctions = (float)Math.Round(2.390 + _elements * 1.375, 1, MidpointRounding.AwayFromZero);
            Attention = (float)Math.Round(2.876 + _elements * 1.2, 1, MidpointRounding.AwayFromZero);
            Memory = (float)Math.Round(1.867 + _elements * 1, 1, MidpointRounding.AwayFromZero);
            Language = (float)Math.Round(2.233 + _elements * 0.525, 1, MidpointRounding.AwayFromZero);
            Difficulty = (float)Math.Round(1.733 + _elements * 1.45, 1, MidpointRounding.AwayFromZero);

            var modelArray = new[] { Attention, Memory, ExFunctions, Language, Difficulty };

            return modelArray;
        }

        public static List<string> GenerateTraining(float attention, float memory, float executivefunctions,
            float language, float difficulty, bool closeMatch)
        {
            List<string> closest = null;
            
            var error = 100000000.0f;

            const int elements = 11;

            for (var iElements = 3; iElements < elements; iElements++)
            {
                var model = new Mazes(iElements);
                var modelParam = model.CalculateModel();

                var sum = (float)(Math.Pow(modelParam[0] - attention, 2) + Math.Pow(modelParam[1] - memory, 2) + Math.Pow(modelParam[2] - executivefunctions, 2) + Math.Pow(modelParam[3] - language, 2) + Math.Pow(modelParam[4] - difficulty, 2));
                if (error > sum)
                {
                    closest = new List<string> {iElements.ToString()};
                    error = sum;
                }
            }
            return closest;
        }
    }
}
