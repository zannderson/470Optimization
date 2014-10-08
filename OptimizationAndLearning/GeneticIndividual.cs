using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimizationAndLearning
{
    public class GeneticIndividual
    {
        public LinearMultivariateWeights Weights { get; set; }

        public double Fit { get; set; }

        public GeneticIndividual()
        {
            Weights = new LinearMultivariateWeights();
        }

        public GeneticIndividual(GeneticIndividual cloneMe) : this()
        {
            Weights.W0 = cloneMe.Weights.W0;
            Weights.W1 = cloneMe.Weights.W1;
            Weights.W2 = cloneMe.Weights.W2;
            Weights.W3 = cloneMe.Weights.W3;
            Weights.W4 = cloneMe.Weights.W4;
        }

        public static bool AttemptMutate(GeneticIndividual subject, int range)
        {
            if(FakeGaussian(0.5, 0.25) < 0.25)
            {
                int index = Helpers.Rand.Next(5);
                subject.Weights[index] += Helpers.Rand.Next(range) - range;
                return true;
            }
            return false;
        }

        private static double FakeGaussian(double mean, double stdDev)
        {
            double u1 = Helpers.Rand.NextDouble(); //these are uniform(0,1) random doubles
            double u2 = Helpers.Rand.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                         Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
            double randNormal =
                         mean + stdDev * randStdNormal; //random normal(mean,stdDev^2)
            return randNormal;
        }

        public static List<GeneticIndividual> Crossover(GeneticIndividual parentOne, GeneticIndividual parentTwo)
        {
            List<GeneticIndividual> children = new List<GeneticIndividual>();
            GeneticIndividual child = new GeneticIndividual();
            GeneticIndividual otherChild = new GeneticIndividual();

            int cutPoint = Helpers.Rand.Next(3) + 1;
            for (int i = 0; i < cutPoint; i++)
            {
                child.Weights[i] = parentOne.Weights[i];
                otherChild.Weights[i] = parentTwo.Weights[i];
            }
            for (int i = cutPoint; i < 5; i++)
            {
                child.Weights[i] = parentTwo.Weights[i];
                otherChild.Weights[i] = parentOne.Weights[i];
            }

            children.Add(child);
            children.Add(otherChild);

            return children;
        }
    }
}
