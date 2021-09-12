using System;

namespace SAaMS_LW1.Helpers
{
    public class LehmerRandom
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

        public double Next()
        {
            double currentR = paramA * previousR % paramM;
            double temp = paramA * previousR * 1d / paramM;
            previousR = currentR;
            return temp - Math.Floor(temp);
        }
    }
}