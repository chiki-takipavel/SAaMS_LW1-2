using System;

namespace SAaMS_LW1.Helpers
{
    public class LehmerRandom
    {
        private readonly int paramA;
        private readonly int paramM;
        private int previousR;

        public LehmerRandom(int paramA, int paramR0, int paramM)
        {
            if (paramR0 <= 0 || paramR0 == int.MaxValue)
            {
                throw new ArgumentException("Bad seed.");
            }

            this.previousR = paramR0;
            this.paramA = paramA;
            this.paramM = paramM;
        }

        public double Next()
        {
            int currentR = paramA * previousR % paramM;
            double temp = paramA * previousR * 1d / paramM;
            previousR = currentR;
            return temp - Math.Floor(temp);
        }
    }
}
