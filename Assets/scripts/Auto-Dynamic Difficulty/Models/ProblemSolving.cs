using System;
using System.Collections.Generic;

namespace Assets.scripts.Models
{
    public class ProblemSolving : Model {

        private readonly int _size;
        private readonly int _tens;
        private readonly int _explicit;

        public ProblemSolving(int size, int tens, int expl)
        {
            _size = size;
            _tens = tens;
            _explicit = expl;
        }

        public override float[] CalculateModel()
        {
            ExFunctions = (float)Math.Round(7.23, 1, MidpointRounding.AwayFromZero);
            Attention = (float)Math.Round(6.97, 1, MidpointRounding.AwayFromZero);
            Memory = (float)Math.Round(6.10, 1, MidpointRounding.AwayFromZero);
            Language = (float)Math.Round(4.65 + _explicit * 1.1, 1, MidpointRounding.AwayFromZero);
            Difficulty = (float)Math.Round(4.870 + _size * 0.542 + _tens * 0.365, 1, MidpointRounding.AwayFromZero);

            var modelArray = new[] { Attention, Memory, ExFunctions, Language, Difficulty };

            return modelArray;
        }

        public static List<string> GenerateTraining(float attention, float memory, float executivefunctions,
            float language, float difficulty, bool closeMatch)
        {
            List<string> closest = null;

            //const int elements = 10;
            const int elements = 5;

            const int tens = 2;
            const int exp = 2;

            var error = 100000000.0f;

            //for (var iElements = 2; iElements < elements; iElements++)
            for (var iElements = 1; iElements < elements; iElements++)
                for (var iTens = 0; iTens < tens; iTens++)
                    for (var iExplicit = 0; iExplicit < exp; iExplicit++)
                    {
                        var model = new ProblemSolving(iElements, iTens, iExplicit);
                        var modelParam = model.CalculateModel();

                        var sum =
                            (float) (Math.Pow(modelParam[0] - attention, 2) +
                                     Math.Pow(modelParam[1] - memory, 2) +
                                     Math.Pow(modelParam[2] - executivefunctions, 2) +
                                     Math.Pow(modelParam[3] - language, 2) +
                                     Math.Pow(modelParam[4] - difficulty, 2));
                        if (error > sum)
                        {
                            closest = new List<string>
                            {
                                iElements.ToString(),
                                iTens.ToString(),
                                iExplicit.ToString()
                            };
                            error = sum;
                        }
                    }

            return closest;
        }
    }
}
