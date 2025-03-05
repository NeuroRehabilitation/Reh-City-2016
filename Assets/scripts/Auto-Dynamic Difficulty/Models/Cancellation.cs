using System;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Assets.scripts.Models
{
    public class Cancellation : Model {

        private readonly int _distractors;
        private readonly int _targets;
        private readonly int _desorganization;
        private readonly int _letters;
        private readonly int _numbersValue;
        private readonly int _symbols;

        public Cancellation(int elem, int prob, int numb, int order)
        {
            _distractors = (int) Mathf.Round(elem * (100.0f - prob) / 100.0f);
            _targets = elem - _distractors;
            _desorganization = order;

            _letters = 0;
            _numbersValue = 0;
            _symbols = 1;

            switch (numb)
            {
                case 0:
                    _letters = 1;
                    _numbersValue = 0;
                    _symbols = 0;
                    break;
                case 1:
                    _letters = 0;
                    _numbersValue = 1;
                    _symbols = 0;
                    break;
            }
        }

        public override float[] CalculateModel()
        {
            ExFunctions = (float)Math.Round(3.579 + _distractors * 0.009 - 1.463 * _letters - 0.754 * _numbersValue + _targets * 0.015 + _desorganization * 0.683 + _distractors * _letters * 0.015, 1, MidpointRounding.AwayFromZero);
            Attention = (float)Math.Round(4.314 + _distractors*0.014 + _numbersValue*-0.697 + _targets*0.021, 1, MidpointRounding.AwayFromZero);
            Memory = (float)Math.Round(3.125 + _distractors * 0.009 + _targets * 0.017 + _numbersValue * -0.813, 1, MidpointRounding.AwayFromZero);
            Language = (float)Math.Round(2.433 + _distractors * 0.011 + _symbols * 0.420 + _numbersValue * -0.467, 1, MidpointRounding.AwayFromZero);
            Difficulty = (float)Math.Round(3.610 + _distractors * 0.015 + _targets * 0.012 + _letters * -0.494 + _numbersValue * -1.054, 1, MidpointRounding.AwayFromZero);

            var modelArray = new[] { Attention, Memory, ExFunctions, Language, Difficulty };
            
            return modelArray;
        }

        public static List<string> GenerateTraining(float attention, float memory, float executivefunctions,
            float language, float difficulty, bool closeMatch)
        {
            List<string> closest = null;
            
            var elements = new List<int>();
            for (var i = 3; i < 40; i++)
                elements.Add(i * i);
            
            var probability = new List<int>();
            for (var i = 5; i < 90; i = i + 5)
                probability.Add(i);
            
            const int numbers = 3;
            const int order = 2;
            
            var error = 100000000.0f;

            for (var iElements = 0; iElements < elements.Count; iElements++)
            {
                for (var iProbability = 0; iProbability < probability.Count; iProbability++)
                {
                    for (var iOrder = 0; iOrder < order; iOrder++)
                    {
                        for (var iNumbers = 0; iNumbers < numbers; iNumbers++)
                        {
                            var model = new Cancellation(elements[iElements], probability[iProbability], iNumbers,
                                iOrder);
                            var modelParam = model.CalculateModel();

                            //Debug.Log("model param(" + elements[iElements] + "," + probability[iProbability] + "," + iNumbers + "," + iOrder + " ) " + "Model Attention: " + modelParam[0] + "Model Memory: " + modelParam[1] + "Model ExFunctions: " + modelParam[2] + "Model Language: " + modelParam[3] + "Model Difficulty: " + modelParam[4]);
                            //Debug.Log("elem" + elements[iElements] + " ; " + model._distractors);
                            var sum =
                                (float)
                                    (Math.Pow(modelParam[0] - attention, 2) + Math.Pow(modelParam[1] - memory, 2) +
                                     Math.Pow(modelParam[2] - executivefunctions, 2) +
                                     Math.Pow(modelParam[3] - language, 2) + Math.Pow(modelParam[4] - difficulty, 2));
                            //Debug.Log("sum: " +  sum);
                            if (error > sum)
                            {
                                closest = new List<string>
                                {
                                    elements[iElements].ToString(),
                                    probability[iProbability].ToString(),
                                    iNumbers.ToString(),
                                    iOrder.ToString()
                                };
                                error = sum;
                                //Debug.Log("error: " + error);
                            }
                        }
                    }
                }
            }
            return closest;
        }
    }
}
