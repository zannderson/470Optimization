using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimizationAndLearning
{
    public class LinearValuePair
    {
        public double Input { get; set; }

        public double Output { get; set; }

        private LinearValuePair()
        {

        }

        private LinearValuePair(double input, double output)
        {
            Input = input;
            Output = output;
        }

        public static List<LinearValuePair> GetValuesFromFile(string filename)
        {
            List<LinearValuePair> pairs = new List<LinearValuePair>();

            FileStream stream = new FileStream(filename, FileMode.Open);
            StreamReader reader = new StreamReader(stream);
            string contents = reader.ReadToEnd();

            string[] lines = contents.Split(Environment.NewLine.ToCharArray());

            foreach (string line in lines)
            {
                var pair = LinearValuePair.Parse(line);
                if (pair != null)
                {
                    pairs.Add(LinearValuePair.Parse(line));
                }
            }

            return pairs;
        }

        public static LinearValuePair Parse(string input)
        {
            string[] parts = input.Split(',');
            double inputValue, output;
            if(double.TryParse(parts[0], out inputValue) && double.TryParse(parts[1], out output))
            {
                return new LinearValuePair(inputValue, output);
            }
            return null;
        }
    }
}
