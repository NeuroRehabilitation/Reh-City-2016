using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.scripts.Models
{
    public class NumericSequencing : Model {
        
        private readonly int _step;
        private readonly float _ascendent;
        private readonly int _missing;
        private readonly int _position;

        public NumericSequencing(int size, int step, int order, int where, int miss)
        {
            _ascendent = 0.5f;

            switch (order)
            {
                case 0:
                    _ascendent = 1;
                    break;
                case 1:
                    _ascendent = 0;
                    break;
            }

            _step = step;
            _missing = miss;
            _position = 0;

            if (where == 1)
                _position = (int) Mathf.Round((size - _missing) / 2.0f);

            
        }

        public override float[] CalculateModel()
        {
            ExFunctions = (float)Math.Round(6.682 + _missing * -0.014 + _position * -0.002, 1, MidpointRounding.AwayFromZero);
            Attention = (float)Math.Round(6.923 + _missing * -0.020 + _position * -0.003, 1, MidpointRounding.AwayFromZero);
            Memory = (float)Math.Round(5.364 + _missing * -0.027 + _position * -0.003, 1, MidpointRounding.AwayFromZero);
            Language = float.Parse((4.722 + _missing * -0.020 + _position * -0.003).ToString("F1"));
            Difficulty = (float)Math.Round(1.290 + _step * 1.232 + _ascendent * -0.841, 1, MidpointRounding.AwayFromZero);

            var modelArray = new[] { Attention, Memory, ExFunctions, Language, Difficulty };
            
            return modelArray;
        }

        public static List<string> GenerateTraining(float attention, float memory, float executivefunctions,
            float language, float difficulty, bool closeMatch)
        {
            List<string> closest = null;

            const int elements = 12;

            const int step = 9;

            const int missing = 6;
            const int order = 0;
            const int where = 0;

            float error = 100000000;

            for (var iElements = 0; iElements < 1; iElements++)
                for (var iStep = 1; iStep < step; iStep++)
                    for (var iWhere = 1; iWhere >= where; iWhere--)
                        for (var iOrder = 2; iOrder >= order; iOrder--)
                            for (var iMissing = 1; iMissing < missing; iMissing++)
                            {
                                var model = new NumericSequencing(elements, iStep, iOrder, iWhere, iMissing);
                                var modelParam = model.CalculateModel();
                                //Debug.Log("model param(" + elements + "," + iStep + "," + iOrder + "," + iWhere + "," + iMissing + " ) " + "Model Attention: " + modelParam[0] + "Model Memory: " + modelParam[1] + "Model ExFunctions: " + modelParam[2] + "Model Language: " + modelParam[3] + "Model Difficulty: " + modelParam[4]);

                                var sum = (float)(Math.Pow(modelParam[0] - attention, 2) + Math.Pow(modelParam[1] - memory, 2) + Math.Pow(modelParam[2] - executivefunctions, 2) + Math.Pow(modelParam[3] - language, 2) + Math.Pow(modelParam[4] - difficulty, 2));

                                //Debug.Log("sum: " + sum);

                                if (error > sum)
                                {
                                    closest = new List<string> { elements.ToString(), iStep.ToString(), iOrder.ToString(), iWhere.ToString(), iMissing.ToString()};
                                    error = sum;
                                    //Debug.Log("error: " + error);
                                }
                            }
            return closest;
        }
    }
}
