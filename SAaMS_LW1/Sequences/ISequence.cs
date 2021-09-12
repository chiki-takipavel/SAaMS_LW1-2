using System.Collections.Generic;

namespace SAaMS_LW1.Sequences
{
    public interface ISequence
    {
        public IEnumerable<double> ProvideSequence();
    }
}