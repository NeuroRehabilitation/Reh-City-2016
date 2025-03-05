using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Assets.scripts.Models
{
    public class ImagePairs : Model {

        private readonly int _size;

        public ImagePairs(int size)
        {
            _size = size;
        }

        public override float[] CalculateModel()
        {
            ExFunctions = (float)Math.Round(3.491 + _size * 0.412, 1, MidpointRounding.AwayFromZero);
            Attention = (float)Math.Round(3.811 + _size * 0.587, 1, MidpointRounding.AwayFromZero);
            Memory = (float)Math.Round(3.792 + _size * 0.637, 1, MidpointRounding.AwayFromZero);
            Language = (float)Math.Round(2.716 + _size * 0.388, 1, MidpointRounding.AwayFromZero);
            Difficulty = (float)Math.Round(2.54 + _size * 0.762, 1, MidpointRounding.AwayFromZero);

            var modelArray = new[] { Attention, Memory, ExFunctions, Language, Difficulty };

            return modelArray;
        }

        public static List<string> GenerateTraining(float attention, float memory, float executivefunctions,
            float language, float difficulty, bool closeMatch)
        {
            List<string> closest = null;
            
            float error = 100000000;
            //const int elements = 7;
            const int elements = 20;
            //changed iElements starting on 1 to 2, doesn't make sense having just one pair in Reh@City
            for (var iElements = 2; iElements <= elements; iElements++)
            {
                var model = new ImagePairs(iElements);
                var modelParam = model.CalculateModel();

                var sum = (float) (Math.Pow(modelParam[0] - attention, 2) + Math.Pow(modelParam[1] - memory, 2) + Math.Pow(modelParam[2] - executivefunctions, 2) + Math.Pow(modelParam[3] - language, 2) + Math.Pow(modelParam[4] - difficulty, 2));
                if (error > sum)
                {
                    closest = new List<string> { iElements.ToString()};
                    error = sum;
                }
            }

            return closest;
        }
    }
}
