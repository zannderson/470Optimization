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

        public double this[int key]
        {
            get
            {
                switch (key)
                {
                    case 0:
                        return W0;
                    case 1:
                        return W1;
                    case 2:
                        return W2;
                    case 3:
                        return W3;
                    case 4:
                        return W4;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
            set
            {
                switch (key)
                {
                    case 0:
                        W0 = value;
                        break;
                    case 1:
                        W1 = value;
                        break;
                    case 2:
                        W2 = value;
                        break;
                    case 3:
                        W3 = value;
                        break;
                    case 4:
                        W4 = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }
	}
}
