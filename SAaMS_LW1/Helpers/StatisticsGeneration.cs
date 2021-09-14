using SAaMS_LW1.Helpers.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

namespace SAaMS_LW1.Helpers
{
    public class StatisticsGeneration
    {
        private const int groupsNumber = 20;
        private const string filePath = "values.csv";

        private readonly List<double> randomSequence;

        public StatisticsGeneration(IEnumerable<double> randomSequence)
        {
            this.randomSequence = new List<double>(randomSequence) ?? new List<double>();
        }

        public double GetExpectedValue()
        {
            return randomSequence.Average();
        }

        public double GetDispersion()
        {
            double expectedValue = GetExpectedValue();
            double temp = 0;

            foreach (double i in randomSequence)
            {
                temp += Math.Pow(i - expectedValue, 2);
            }

            return temp / randomSequence.Count;
        }

        public double GetStandartDeviation()
        {
            return Math.Sqrt(GetDispersion());
        }

        public IEnumerable<float> GetDistribution()
        {
            List<float> distribution = new();
            List<double> tempRandomSequence = new(randomSequence);
            tempRandomSequence.Sort();

            int counter = 0;
            int index = 0;

            double firstValue = tempRandomSequence.First();
            double lastValue = tempRandomSequence.Last();
            double interval = (lastValue - firstValue) / groupsNumber;

            for (int i = 0; i < groupsNumber; i++)
            {
                while ((index <= tempRandomSequence.Count - 1) && (tempRandomSequence[index] <= firstValue + interval * (i + 1)))
                {
                    if (tempRandomSequence[index] >= firstValue)
                    {
                        counter++;
                    }

                    index++;
                }

                distribution.Add((float)counter / randomSequence.Count);
                counter = 0;
            }

            return distribution;
        }

        public int GetPeriod()
        {
            return randomSequence.Count;
        }

        public double GetChecked()
        {
            int temp = 0;
            for (int index = 0; index < randomSequence.Count - 2; index++)
            {
                if (Math.Pow(randomSequence[index], 2) + Math.Pow(randomSequence[index + 1], 2) <= 1)
                {
                    temp++;
                }
            }

            return (double)temp / randomSequence.Count;
        }

        public void ShowValues()
        {
            try
            {
                using (StreamWriter streamWriter = new(filePath))
                {
                    foreach (double value in randomSequence)
                    {
                        streamWriter.WriteLine(Convert.ToString(value, CultureInfo.CurrentCulture));
                    }
                }

                Process process = new();
                process.StartInfo.FileName = filePath;
                process.StartInfo.UseShellExecute = true;
                _ = process.Start();
            }
            catch
            {
                throw new ShowValuesException("Please, close Excel.");
            }
        }
    }
}
