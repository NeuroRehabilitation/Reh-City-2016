using System.Collections.Generic;

namespace Assets.scripts.Models
{
    public class Model
    {
        public float Attention;
        public float Memory;
        public float ExFunctions;
        public float Language;
        public float Difficulty;

        protected List<string> Closest;

        public Model(float att, float mem, float exec, float lang, float diff)
        {
            Attention = att;
            Memory = mem;
            ExFunctions = exec;
            Language = lang;
            Difficulty = diff;
        }

        protected Model()
        {
        }

        public virtual float[] CalculateModel()
        {
            return null;
        }
    }
}
