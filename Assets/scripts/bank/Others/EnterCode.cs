using System.Collections.Generic;
using Assets.scripts.Manager;
using Assets.scripts.objectives.Action;
using UnityEngine;

namespace Assets.scripts.bank.Others
{
    public class EnterCode : ActionObjective
    {
        private readonly List<int> _missing = new List<int>();

        public EnterCode(int size, int missingDigits, int initialDigit, int sumNumber)
        {
            description = childlist[3].InnerText;
            var sequence = new int[size];
            sequence[0] = initialDigit;
            CanSetupCode = true;
            answerset = new List<string>();
            Performance = -1;

            for (var i = 1; i < sequence.Length; i++)
            {
                sequence[i] = sequence[i - 1] + sumNumber;
            }
        
            do
            {
                var temp = Random.Range(0, size);

                if (!_missing.Contains(temp))
                    _missing.Add(temp);

            } while (_missing.Count < missingDigits);
        
            NumericSequence = "";

            for (var c = 0; c < sequence.Length; c++)
            {
                if (_missing.Contains(c))
                {
                    if (sequence[c] > 9 && sequence[c] <= 99)
                    {
                        var digits = sequence[c].ToString();
                        answerset.Add(digits[0].ToString());
                        answerset.Add(digits[1].ToString());
                        NumericSequence = NumericSequence + "__,";
                    }
                    else if (sequence[c] > 99)
                    {
                        var digits = sequence[c].ToString();
                        answerset.Add(digits[0].ToString());
                        answerset.Add(digits[1].ToString());
                        answerset.Add(digits[2].ToString());
                        NumericSequence = NumericSequence + "___,";
                    }
                    else
                    {
                        answerset.Add(sequence[c].ToString());
                        NumericSequence = NumericSequence + "_,";
                    }
                }

                else
                {
                    NumericSequence = NumericSequence + sequence[c] + ",";
                }

            }
        }
    
        public override void CheckForCompletion()
        {
            if (answerset.Count == 0)
            {
                _missing.Clear();
                BankManager.BankOptionsReady = true;
                for(var i= 0; i<10;  i++)
                {
                    GameObject.Find(i.ToString()).GetComponent<BoxCollider>().enabled = false;
                }
                completedTimer -= Time.deltaTime;
                if (completedTimer < 0 && !completed)
                {
                    if (GameManager.WrongChoices() == 0)
                        Performance = 100.0f;
                    else
                    {
                        var dif = GameManager.CorrectChoices() - GameManager.WrongChoices();
                        Performance = dif * 100.0f / GameManager.CorrectChoices();
                        if (Performance < 0)
                            Performance = 0;
                    }

                    Debug.Log("Targets to find: " + GameManager.CorrectChoices() + " ; wrong answers: " + GameManager.WrongChoices() + "; collection performance: " + Performance);

                    completed = true;
                }

                CanAddNextObjective = true;
                
            }
        }
    }
}
