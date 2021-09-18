using SAaMS_LW1.Randoms.Interfaces;
using System;

namespace SAaMS_LW1.Randoms
{
    public class LehmerRandom : IRandom
    {
        private readonly int paramA;
        private readonly int paramM;
        private double previousR;

        public LehmerRandom(int paramA, double paramR0, int paramM)
        {
            if (paramR0 is <= 0 or int.MaxValue)
            {
                throw new ArgumentException("Bad seed.");
            }

            this.previousR = paramR0;
            this.paramA = paramA;
            this.paramM = paramM;
        }

        public double NextValue()
        {
            double currentR = paramA * previousR % paramM;
            previousR = currentR;
            return currentR / paramM;
        }
    }
}