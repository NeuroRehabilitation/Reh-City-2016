using System;
using System.Collections.Generic;

namespace Assets.scripts.Models
{
    public class Context : Model {

        private readonly int _size;

        public Context(int size)
        {
            _size = size;
        }

        public override float[] CalculateModel()
        {
            ExFunctions = (float)Math.Round(0.25f + _size * 1.2f, 1, MidpointRounding.AwayFromZero);
            Attention = (float)Math.Round(3.40f, 1, MidpointRounding.AwayFromZero);
            Memory = (float)Math.Round(2.63f, 1, MidpointRounding.AwayFromZero);
            Language = (float)Math.Round(1.45f + _size * 1, 1, MidpointRounding.AwayFromZero);
            Difficulty = (float)Math.Round(1.05f + _size * 0.75f, 1, MidpointRounding.AwayFromZero);
            //Language = float.Parse(string.Format("{0:F1}", Math.Truncate((1.45f + _size * 1) * 10) / 10));
            //Difficulty = float.Parse(string.Format("{0:F1}", Math.Truncate((1.05f + _size * 0.75f) * 10) / 10));

            var modelArray = new[] { Attention, Memory, ExFunctions, Language, Difficulty };

            return modelArray;
        }

        public static List<string> GenerateTraining(float attention, float memory, float executivefunctions,
            float language, float difficulty, bool closeMatch)
        {
            List<string> closest = null;
            
            var error = 100000000.0f;
            const int elements = 10;

            for (var iElements = 3; iElements < elements; iElements++)
            {
                var model = new Context(iElements);
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
