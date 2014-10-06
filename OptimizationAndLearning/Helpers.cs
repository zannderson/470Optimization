using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimizationAndLearning
{
    public class Helpers
    {
        public static double CalculateMeanSquaredError(List<double> expected, List<double> actual)
        {
            if(actual.Count != expected.Count)
            {
                throw new ArgumentException("Number of values in 'expected' should be the same as number in 'actual'.");
            }

            double error = 0.0;

            for (int i = 0; i < expected.Count; i++)
            {
                var currentError = actual[i] - expected[i];
                error += currentError * currentError;
            }

            return error / actual.Count;
        }

        public static void CalculateLeastMeans(List<LinearValuePair> points, out double m, out double b)
        {
            double x1, y1, xy, x2, j;
            x1 = y1 = xy = x2 = j = 0.0;

            foreach (LinearValuePair point in points)
            {
                x1 += point.Input;
                y1 += point.Output;
                xy += point.Input * point.Output;
                x2 += point.Input * point.Input;
            }

            j = (points.Count * x2) - (x1 * x1);

            if(j != 0.0)
            {
                m = ((points.Count * xy) - (x1 * y1)) / j;
                m = Math.Floor(1.0E3 * m + 0.5) / 1.0E3;
                b = ((y1 * x2) - (x1 * xy)) / j;
                b = Math.Floor(1.0E3 * b + 0.5) / 1.0E3;
            }
            else
            {
                m = b = 0.0;
            }
        }

		public static LinearMultivariateWeights PerformGradientDescent(List<MultivariateDataValues> points, double learningRate)
		{
			LinearMultivariateWeights weights = new LinearMultivariateWeights(1.0, 1.0, 1.0, 1.0, 1.0);
			double delta = 1.0;
			while(delta > 0.001)
			{
				weights = GradientDescentStep(weights, points, learningRate, out delta);
			}

			return weights;
		}

		public static LinearMultivariateWeights GradientDescentStep(LinearMultivariateWeights currentWeights, List<MultivariateDataValues> points, double learningRate, out double delta)
		{
			LinearMultivariateWeights newWeights = new LinearMultivariateWeights(currentWeights.W0, currentWeights.W1, currentWeights.W2, currentWeights.W3, currentWeights.W4);
			LinearMultivariateWeights iterationWeights = new LinearMultivariateWeights();

			double numPoints = (double)points.Count;

			foreach (MultivariateDataValues point in points)
			{
				double currentY = GetYForMValues(currentWeights, point);
				double error = point.Y - currentY;
				iterationWeights.W0 += error;
				iterationWeights.W1 += error * point.X1;
				iterationWeights.W2 += error * point.X2;
				iterationWeights.W3 += error * point.X3;
				iterationWeights.W4 += error * point.X4;
			}

			newWeights.W0 += (learningRate * iterationWeights.W0);
			newWeights.W1 += (learningRate * iterationWeights.W1);
			newWeights.W2 += (learningRate * iterationWeights.W2);
			newWeights.W3 += (learningRate * iterationWeights.W3);
			newWeights.W4 += (learningRate * iterationWeights.W4);

			delta = Math.Abs(learningRate * (iterationWeights.W0 + iterationWeights.W1 + iterationWeights.W2 + iterationWeights.W3 + iterationWeights.W4) / 5);

			return newWeights;
		}

		private static double GetYForMValues(LinearMultivariateWeights weights,
			MultivariateDataValues point)
		{
			return weights.W0 + weights.W1 * point.X1 + weights.W2 * point.X2 + weights.W3 * point.X3 + weights.W4 * point.X4;
		}
    }
}
