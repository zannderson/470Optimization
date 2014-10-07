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
			List<MultivariateDataValues> multivariateData = MultivariateDataValues.GetValuesFromFile("../../Files/multivariate_data-train.csv");
			//TestMultivariateLearn(multivariateData, true);
            var dude = Helpers.GeneticLinearRegression(multivariateData, 20, 50, 5, 10.0);
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

		public static LinearMultivariateWeights TestMultivariateLearn(List<MultivariateDataValues> multivariateData, bool learnOnOdds)
		{
			LinearMultivariateWeights weights = Helpers.PerformGradientDescent(multivariateData, 0.0000036);

			return weights;
		}

        public static double TestMultivariateFinal(List<MultivariateDataValues> multivariateData, LinearMultivariateWeights weights)
        {
            List<MultivariateDataValues> testData = MultivariateDataValues.GetValuesFromFile("../../Files/multivariate_data-test.csv");

            List<double> expected = new List<double>();
            List<double> actual = new List<double>();

            foreach (MultivariateDataValues point in testData)
            {
                expected.Add(point.Y);
                actual.Add(Helpers.GetYForMValues(weights, point));
            }

            return Helpers.CalculateMeanSquaredError(expected, actual);
        }
    }
}
