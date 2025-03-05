using System;
using System.Collections.Generic;

namespace Assets.scripts.Models
{
    public class Association : Model
    {
        private readonly int _size;

        public Association(int size)
        {
            _size = size;
        }

        public override float[] CalculateModel()
        {
            ExFunctions = (float)Math.Round(2.729 + _size*0.238, 1, MidpointRounding.AwayFromZero);
            Attention = (float)Math.Round(1.512 + _size*0.487, 1, MidpointRounding.AwayFromZero);
            Memory = (float)Math.Round(1.378 + _size*0.4, 1, MidpointRounding.AwayFromZero);
            Language = (float)Math.Round(3.28, 1, MidpointRounding.AwayFromZero);
            Difficulty = (float)Math.Round(1.435 + _size*0.45, 1, MidpointRounding.AwayFromZero);

            var modelArray = new[] { Attention, Memory, ExFunctions, Language, Difficulty };

            return modelArray;
        }

        public static List<string> GenerateTraining(float attention, float memory, float executivefunctions,
            float language, float difficulty, bool closeMatch)
        {
            List<string> closest = null;

            var error = 100000000.0f;
            const int elements = 20;

            for (var iElements = 2; iElements < elements; iElements++)
            {
                var model = new Association(iElements);
                var modelParam = model.CalculateModel();

                var sum =
                        (float)
                            (Math.Pow(modelParam[0] - attention, 2) + Math.Pow(modelParam[1] - memory, 2) +
                             Math.Pow(modelParam[2] - executivefunctions, 2) +
                             Math.Pow(modelParam[3] - language, 2) + Math.Pow(modelParam[4] - difficulty, 2));
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
