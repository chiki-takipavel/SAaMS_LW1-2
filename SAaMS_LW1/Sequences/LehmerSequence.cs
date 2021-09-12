using SAaMS_LW1.Helpers;
using System.Collections.Generic;

namespace SAaMS_LW1.Sequences
{
    public class LehmerSequence : ISequence
    {
        private readonly int paramA;
        private readonly int paramM;
        private readonly int paramR0;

        private readonly List<double> randomSequence = new();

        public LehmerSequence(int paramA, int paramR0, int paramM)
        {
            this.paramA = paramA;
            this.paramR0 = paramR0;
            this.paramM = paramM;
        }

        public IEnumerable<double> ProvideSequence()
        {
            LehmerRandom random = new(paramA, paramR0, paramM);
            randomSequence.Add(random.Next());
            double temp = random.Next();

            while (!randomSequence.Contains(temp))
            {
                randomSequence.Add(temp);
                temp = random.Next();
            }

            return randomSequence;
        }
    }
}
