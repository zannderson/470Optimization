using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimizationAndLearning
{
    public class MultivariateDataValues
    {
        public double X1 { get; set; }

        public double X2 { get; set; }

        public double X3 { get; set; }

        public double X4 { get; set; }

        public double Y { get; set; }

        private MultivariateDataValues()
        {

        }

        private MultivariateDataValues(double x1, double x2, double x3, double x4, double y)
        {
            X1 = x1;
            X2 = x2;
            X3 = x3;
            X4 = x4;
            Y = y;
        }

        private static MultivariateDataValues Parse(string input)
        {
            string[] parts = input.Split(',');
            if(parts.Length != 5)
            {
                return null;
            }
            else
            {
                double x1, x2, x3, x4, y;
                if(double.TryParse(parts[0], out x1) &&
                    double.TryParse(parts[1], out x2) &&
                    double.TryParse(parts[2], out x3) &&
                    double.TryParse(parts[3], out x4) &&
                    double.TryParse(parts[4], out y))
                {
                    return new MultivariateDataValues(x1, x2, x3, x4, y);
                }
            }
            return null;
        }

        public static List<MultivariateDataValues> GetValuesFromFile(string filename)
        {
            List<MultivariateDataValues> pairs = new List<MultivariateDataValues>();

            FileStream stream = new FileStream(filename, FileMode.Open);
            StreamReader reader = new StreamReader(stream);
            string contents = reader.ReadToEnd();

            string[] lines = contents.Split(Environment.NewLine.ToCharArray());

            foreach (string line in lines)
            {
                var pair = MultivariateDataValues.Parse(line);
                if (pair != null)
                {
                    pairs.Add(MultivariateDataValues.Parse(line));
                }
            }

            return pairs;
        }
    }
}
