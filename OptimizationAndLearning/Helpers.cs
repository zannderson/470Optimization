using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimizationAndLearning
{
    public class Helpers
    {
        public static readonly Random Rand = new Random();

        public static double CalculateMeanSquaredError(List<double> expected, List<double> actual)
        {
            if (actual.Count != expected.Count)
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

            if (j != 0.0)
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
            LinearMultivariateWeights weights = new LinearMultivariateWeights(0.0, 1.0, 1.0, 1.0, 1.0);
            double delta = 1.0;
            int iteration = 1;
            double iterationError = 0.0;
            while (delta > 0.000001)// && iteration < 500001)
            {
                //Console.Out.Write(iteration + " -- ");
                GradientDescentStep(ref weights, points, learningRate, out delta, out iterationError);
                iteration++;
            }

            return weights;
        }

        public static void GradientDescentStep(ref LinearMultivariateWeights currentWeights, List<MultivariateDataValues> points, double learningRate, out double delta, out double iterationError)
        {
            //LinearMultivariateWeights newWeights = new LinearMultivariateWeights(currentWeights.W0, currentWeights.W1, currentWeights.W2, currentWeights.W3, currentWeights.W4);
            LinearMultivariateWeights iterationWeights = new LinearMultivariateWeights();

            double numPoints = (double)points.Count;
            List<double> expected = new List<double>();
            List<double> actual = new List<double>();

            foreach (MultivariateDataValues point in points)
            {
                double currentY = GetYForMValues(currentWeights, point);
                expected.Add(point.Y);
                actual.Add(currentY);
                double error = point.Y - currentY;
                iterationWeights.W0 += learningRate * error;
                iterationWeights.W1 += learningRate * (error * point.X1);
                iterationWeights.W2 += learningRate * (error * point.X2);
                iterationWeights.W3 += learningRate * (error * point.X3);
                iterationWeights.W4 += learningRate * (error * point.X4);
            }


            currentWeights.W0 += iterationWeights.W0;
            currentWeights.W1 += iterationWeights.W1;
            currentWeights.W2 += iterationWeights.W2;
            currentWeights.W3 += iterationWeights.W3;
            currentWeights.W4 += iterationWeights.W4;

            delta = Math.Abs((iterationWeights.W0 + iterationWeights.W1 + iterationWeights.W2 + iterationWeights.W3 + iterationWeights.W4) / 5);
            iterationError = CalculateMeanSquaredError(expected, actual);
            //Console.Out.WriteLine(CalculateMeanSquaredError(expected, actual) + " -- " + delta);

            //return newWeights;
        }

        public static double GetYForMValues(LinearMultivariateWeights weights,
            MultivariateDataValues point)
        {
            return weights.W0 + weights.W1 * point.X1 + weights.W2 * point.X2 + weights.W3 * point.X3 + weights.W4 * point.X4;
        }

        public static double TestErrorForArbitraryValues(LinearMultivariateWeights weights, List<MultivariateDataValues> points)
        {
            List<double> expected = new List<double>();
            List<double> actual = new List<double>();

            foreach (MultivariateDataValues point in points)
            {
                expected.Add(point.Y);
                actual.Add(GetYForMValues(weights, point));
            }

            return CalculateMeanSquaredError(expected, actual);
        }

        public static GeneticIndividual GeneticLinearRegression(List<MultivariateDataValues> points, int initialPopulationCount, int minimumPopulationCount, int expectedRange, int keep, double cutoff)
        {
            //initial population
            List<GeneticIndividual> population = new List<GeneticIndividual>();
            for (int i = 0; i < initialPopulationCount; i++)
            {
                population.Add(GenerateRandomIndividual(expectedRange));
            }

            double bestFit = double.MaxValue;
            GeneticIndividual bestIndividual = null;
            int generation = 1;
            int shrinkage = (initialPopulationCount - minimumPopulationCount) / 10;

            int populationCount = initialPopulationCount;
            while (bestFit > cutoff)
            {
                //Console.Out.WriteLine(population.Count);
                //fitness function

                foreach (GeneticIndividual individual in population)
                {
                    individual.Fit = TestErrorForArbitraryValues(individual.Weights, points);
                }

                //selection

                population = population.OrderBy(p => p.Fit).ToList();
                List<GeneticIndividual> toRemove = new List<GeneticIndividual>();
                for (int i = keep; i < population.Count; i++)
                {
                    toRemove.Add(population[i]);
                }
                foreach (var removeMe in toRemove)
                {
                    population.Remove(removeMe);
                }
                var best = population.FirstOrDefault();
                bestFit = best.Fit;
                //Console.Out.WriteLine(generation + " " + bestFit);
                bestIndividual = best;

                if (generation % 100 == 0)
                {
                    populationCount += 2;
                    if (generation % 1000 == 0)
                    {
                        Console.Out.WriteLine(generation + " " + bestFit);
                    }
                }

                //crossover - only selected individuals remain at this point

                //mutate pairs
                //for (int i = 0; i < population.Count; i+= 2)
                //{
                //    if (i < population.Count - 1)
                //    {
                //        population.AddRange(GeneticIndividual.Crossover(population[i], population[i + 1]));
                //    }
                //    else
                //    {
                //        population.AddRange(GeneticIndividual.Crossover(population[i], population[0]));
                //    }
                //}

                //stud top x
                int studs = 2;
                int currentCount = population.Count;
                for (int i = 0; i < studs; i++)
                {
                    for (int j = studs; j < currentCount; j++)
                    {
                        population.AddRange(GeneticIndividual.Crossover(population[i], population[j]));
                    }
                }
                //Console.Out.WriteLine(population.Count);

                //mutation
                int mutated = 0;
                for (int i = 0; i < population.Count; i++)
                {
                    if (GeneticIndividual.AttemptMutate(population[i], expectedRange))
                    {
                        mutated++;
                    }
                }
                //Console.Out.WriteLine("Mutations: " + mutated);

                for (int i = population.Count; i < populationCount; i++)
                {
                    //if (i % 2 == 0)
                    //{
                    //    population.Add(MuckAround(bestIndividual, 20));
                    //}
                    //else
                    //{
                        population.Add(ForSureMutate(bestIndividual, 20));
                    //}
                }


                toRemove.Clear();
                for (int i = populationCount; i < population.Count; i++)
                {
                    toRemove.Add(population[i]);
                }
                foreach (var remove in toRemove)
                {
                    population.Remove(remove);
                }

                generation++;
                //Console.Out.WriteLine(population.Count);
            }
            return bestIndividual;
        }

        private static GeneticIndividual GenerateRandomIndividual(int expectedRange)
        {
            double w0 = GetDoubleWithinRange(expectedRange);
            double w1 = GetDoubleWithinRange(expectedRange);
            double w2 = GetDoubleWithinRange(expectedRange);
            double w3 = GetDoubleWithinRange(expectedRange);
            double w4 = GetDoubleWithinRange(expectedRange);

            return new GeneticIndividual
            {
                Weights = new LinearMultivariateWeights(w0, w1, w2, w3, w4)
            };
        }

        private static GeneticIndividual MuckAround(GeneticIndividual startingPoint, int expectedRange)
        {
            double w0 = GetDoubleWithinRange(expectedRange);
            double w1 = GetDoubleWithinRange(expectedRange);
            double w2 = GetDoubleWithinRange(expectedRange);
            double w3 = GetDoubleWithinRange(expectedRange);
            double w4 = GetDoubleWithinRange(expectedRange);

            return new GeneticIndividual
            {
                Weights = new LinearMultivariateWeights(startingPoint.Weights.W0 + w0, startingPoint.Weights.W1 + w1, startingPoint.Weights.W2 + w2, startingPoint.Weights.W3 + w3, startingPoint.Weights.W4 + w4)
            };
        }

        private static GeneticIndividual ForSureMutate(GeneticIndividual startingPoint, int expectedRange)
        {
            double amount = GetDoubleWithinRange(expectedRange);

            int index = Rand.Next(5);

            GeneticIndividual newGuy = new GeneticIndividual(startingPoint);
            newGuy.Weights[index] += amount;
            return newGuy;
        }

        private static double GetDoubleWithinRange(int range)
        {
            return Rand.NextDouble() * Rand.Next(range * 2) - range;
        }
    }
}
