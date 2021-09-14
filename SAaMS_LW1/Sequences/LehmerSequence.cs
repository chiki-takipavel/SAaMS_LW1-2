using SAaMS_LW1.Helpers.Interfaces;
using SAaMS_LW1.Sequences.Interfaces;
using System.Collections.Generic;

namespace SAaMS_LW1.Sequences
{
    public class LehmerSequence : ISequence
    {
        private readonly IRandom random;

        private readonly List<double> randomSequence = new();

        public LehmerSequence(IRandom random)
        {
            this.random = random;
        }

        public IEnumerable<double> ProvideSequence()
        {
            randomSequence.Add(random.NextValue());
            double temp = random.NextValue();

            while (!randomSequence.Contains(temp))
            {
                randomSequence.Add(temp);
                temp = random.NextValue();
            }

            return randomSequence;
        }
    }
}
