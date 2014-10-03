using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimizationAndLearning
{
    class Program
    {
        static void Main(string[] args)
        {
            List<LinearValuePair> linearData = LinearValuePair.GetValuesFromFile("../../Files/univariate_data-train.csv");
        }

        public static double TestLinearLearn(List<LinearValuePair> linearData, bool learnOnOdds)
        {
            List<LinearValuePair> learningData = new List<LinearValuePair>();
            List<LinearValuePair> devTest = new List<LinearValuePair>();

            for (int i = 0; i < linearData.Count; i += 2)
            {
                if (learnOnOdds)
                {
                    devTest.Add(linearData[i]);
                    if (i + 1 < linearData.Count)
                    {
                        learningData.Add(linearData[i + 1]);
                    }
                }
                else
                {
                    learningData.Add(linearData[i]);
                    if (i + 1 < linearData.Count)
                    {
                        devTest.Add(linearData[i + 1]);
                    }
                }
            }

            double m, b;
            Helpers.CalculateLeastMeans(learningData, out m, out b);

            List<double> expectedValues = new List<double>();
            List<double> actualValues = new List<double>();

            foreach (LinearValuePair point in devTest)
            {
                double actual = m * point.Input + b;
                actualValues.Add(actual);
                expectedValues.Add(point.Output);
            }

            return Helpers.CalculateMeanSquaredError(expectedValues, actualValues);
        }

        public static double TestLinearFinal(List<LinearValuePair> linearData)
        {
            List<LinearValuePair> testData = LinearValuePair.GetValuesFromFile("../../Files/univariate_data-test.csv");

            double m, b;
            Helpers.CalculateLeastMeans(linearData, out m, out b);

            List<double> expectedValues = new List<double>();
            List<double> actualValues = new List<double>();

            foreach (LinearValuePair point in testData)
            {
                double actual = m * point.Input + b;
                actualValues.Add(actual);
                expectedValues.Add(point.Output);
            }

            return Helpers.CalculateMeanSquaredError(expectedValues, actualValues);
        }
    }
}
