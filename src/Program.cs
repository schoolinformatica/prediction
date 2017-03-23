
using System;
using System.IO;
using Smoothing;
using System.Collections.Generic;
using System.Linq;
// using Gtk;
using System.Threading.Tasks;
using MyClient;


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
            var bestDES = DES.FindBestAlphaBeta(points, 0.1);
            var forecastDES = DES.ForeCast(points, 37, 48, bestDES.Item1, bestDES.Item2);
            var besSES = SES.FindBestAlpha(points, 0.01);
            var forecastSES = SES.ForeCast(points, besSES, 37, 48);

            string original = "[";

            foreach (var num in points)
            {
                original+= num + ",";
            }
            original += "]";
            
            string desforecastDES = "[";

            foreach (var num in forecastDES)
            {
                desforecastDES+= num + ",";
            }
            desforecastDES += "]";

            string desforecastSES = "[";

            foreach (var num in forecastSES)
            {
                desforecastSES+= num + ",";
            }
            desforecastSES += "]";

            var list = "[" + original + "," + desforecastDES + ","+desforecastSES+ "]";
            // var i = 0;
            // // foreach (var point in smoothedPoints)
            // // {
            // //     Console.WriteLine(i + ": " + point);
            // //     i++;
            // // }
            var socket = new Client_Socket();
            socket.Publish(list);
        

        }
    }
}
