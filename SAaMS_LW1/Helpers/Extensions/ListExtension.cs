using System;
using System.Collections.Generic;

namespace SAaMS_LW1.Helpers.Extensions
{
    public static class ListExtension
    {
        public static IEnumerable<double> GetExponentialDistribution(this IList<double> sequence, double lambda)
        {
            List<double> resultSequence = new();

            foreach (double value in sequence)
            {
                resultSequence.Add(Math.Abs(Math.Log(value) / lambda));
            }

            return resultSequence;
        }

        public static IEnumerable<double> GetGammaDistribution(this IList<double> sequence, double eta, double lambda)
        {
            List<double> resultSequence = new();
            Random random = new();
            int maxRandomValue = sequence.Count - 1;

            foreach (double _ in sequence)
            {
                double r = 1;
                for (int index = 0; index <= Math.Floor(eta); index++)
                {
                    r *= sequence[random.Next(maxRandomValue)];
                }

                resultSequence.Add(-Math.Abs(Math.Log(r) / lambda));
            }

            return resultSequence;
        }

        public static IEnumerable<double> GetGaussDistribution(this IList<double> sequence, double expc, double mdev)
        {
            List<double> resultSequence = new();
            Random random = new();
            int maxRandomValue = sequence.Count - 1;

            foreach (double _ in sequence)
            {
                double r = 0;
                for (int index = 0; index < 6; index++)
                {
                    r += sequence[random.Next(maxRandomValue)];
                }

                resultSequence.Add(expc + (mdev * Math.Sqrt(2) * (r - 3)));
            }

            return resultSequence;
        }

        public static IEnumerable<double> GetSimpsonDistribution(this IList<double> sequence, double a, double b)
        {
            List<double> tempSequence = new();
            foreach (double value in sequence)
            {
                tempSequence.Add((a / 2) + (((b / 2) - (a / 2)) * value));
            }

            List<double> resultSequence = new();
            Random random = new();
            int maxRandomValue = tempSequence.Count - 1;

            foreach (double _ in sequence)
            {
                double r1 = tempSequence[random.Next(maxRandomValue)];
                double r2 = tempSequence[random.Next(maxRandomValue)];
                resultSequence.Add(r1 + r2);
            }

            return resultSequence;
        }

        public static IEnumerable<double> GetTriangularDistribution(this IList<double> sequence, double a, double b)
        {
            List<double> resultSequence = new();
            Random random = new();
            int maxRandomValue = sequence.Count - 1;

            foreach (double _ in sequence)
            {
                double r1 = sequence[random.Next(maxRandomValue)];
                double r2 = sequence[random.Next(maxRandomValue)];
                resultSequence.Add(a + ((b - a) * Math.Max(r1, r2)));
            }

            return resultSequence;
        }

        public static IEnumerable<double> GetUniformDistribution(this IList<double> sequence, double a, double b)
        {
            List<double> resultSequence = new();

            foreach (double value in sequence)
            {
                resultSequence.Add(a + ((b - a) * value));
            }

            return resultSequence;
        }
    }
}
