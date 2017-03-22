
using System;
using System.IO;
using Smoothing;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prediction
{
    class Program
    {
        private static void Main(string[] args)
        {
            var data = File
            .ReadAllLines(@"SwordForecasting.csv")
            .Skip(1)
            .Select(x => x.Split(','))
            .ToDictionary(y => double.Parse(y[0]), y => double.Parse(y[1]));

            var points = data.Values.ToArray();
            // var smoothedPoints = DES.Smooth(points, 0.6, 0.6);
            var best = DES.FindBestAlphaBeta(points, 0.1);
            Console.WriteLine("Best Alpha: " + best.Item1 + " Best Beta: " + best.Item2);

            var forecast = DES.ForeCast(points, 37, 48, best.Item1, best.Item2);

            foreach (var num in forecast)
            {
                Console.WriteLine(num);
            }


            var i = 0;
            // foreach (var point in smoothedPoints)
            // {
            //     Console.WriteLine(i + ": " + point);
            //     i++;
            // }

        }
    }
}
