using SAaMS_LW1.Helpers.Enums;
using SAaMS_LW1.Helpers.Extensions;
using SAaMS_LW1.Sequences.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SAaMS_LW1.Helpers
{
    public class DistributionHelper
    {
        public ISequence StartSequence { get; set; }
        public double Parameter1Value { get; set; }
        public double Parameter2Value { get; set; }

        public DistributionHelper() { }

        public async Task<IEnumerable<double>> GetDistribution(Distribution selectedDistribution)
        {
            List<double> startSequenceValue = await Task.Run(() => new List<double>(StartSequence.ProvideSequence()));

            return await Task.Run(() => selectedDistribution switch
            {
                Distribution.None => startSequenceValue,
                Distribution.Exponential => startSequenceValue.GetExponentialDistribution(Parameter1Value),
                Distribution.Gauss => startSequenceValue.GetGaussDistribution(Parameter1Value, Parameter2Value),
                Distribution.Gamma => startSequenceValue.GetGammaDistribution(Parameter1Value, Parameter2Value),
                Distribution.Simpson => startSequenceValue.GetSimpsonDistribution(Parameter1Value, Parameter2Value),
                Distribution.Triangular => startSequenceValue.GetTriangularDistribution(Parameter1Value, Parameter2Value),
                Distribution.Uniform => startSequenceValue.GetUniformDistribution(Parameter1Value, Parameter2Value),
                _ => startSequenceValue,
            });
        }
    }
}
