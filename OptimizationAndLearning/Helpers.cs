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

		public static void GradientDescent(double currentM1, double currentM2, double currentM3, double currentM4, double currentB, List<MultivariateDataValues> points, double learningRate)
		{
			double iterationM1 = 0.0;
			double iterationM2 = 0.0;
			double iterationM3 = 0.0;
			double iterationM4 = 0.0;
			double iterationB = 0.0;

			double numPoints = (double)points.Count;

			foreach (MultivariateDataValues point in points)
			{
				iterationB = (-2 / numPoints) * (point.Y - (currentM * point.X));
			}

			double newM1 = currentM1 - (learningRate * iterationM1);
			double newM2 = currentM2 - (learningRate * iterationM2);
			double newM3 = currentM3 - (learningRate * iterationM3);
			double newM4 = currentM4 - (learningRate * iterationM4);
		}
    }
}
