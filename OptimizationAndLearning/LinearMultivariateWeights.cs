using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimizationAndLearning
{
	public class LinearMultivariateWeights
	{
		public double W0 { get; set; }

		public double W1 { get; set; }

		public double W2 { get; set; }

		public double W3 { get; set; }

		public double W4 { get; set; }

		public LinearMultivariateWeights(double w0, double w1, double w2, double w3, double w4)
		{
			W0 = w0;
			W1 = w1;
			W2 = w2;
			W3 = w3;
			W4 = w4;
		}

		public LinearMultivariateWeights()
		{
			W0 = W1 = W2 = W3 = W4 = 0.0;
		}
	}
}
