using System;
using System.Linq;
using System.Collections.Generic;

namespace Smoothing
{

    public static class DES
    {
        private static Dictionary<int, double> s = new Dictionary<int, double>();
        private static Dictionary<int, double> b = new Dictionary<int, double>();

        public static double[] ForeCast(double[] values, int start, int end, double alpha, double beta) {
            // var smoothedValues = Smooth(values, alpha, beta);
            int n = values.Length;
            double[] forecast = new double[n + end-start + 1];
            Smooth(values, alpha, beta).CopyTo(forecast, 0);                        
            double Sn = CalculateSmoothing(values, alpha, beta, n - 1);
            double Bn = CalculateBeta(beta, alpha, values, n - 1);


            for(int i = start; i <= end; i++) {
                forecast[i - 1] = Sn + (i - n) * Bn;
            }

            return forecast;

        }
        public static Tuple<double, double> FindBestAlphaBeta(double[] values, double stepSize)
        {
            var BestAlphaBeta = new Tuple<double, double>(0, 0);
            var BestSquaredError = double.MaxValue;

            for (double a = stepSize; a < 1; a += stepSize)
            {
                for (double b = stepSize; b < 1; b += stepSize)
                {
                    var sse = SquaredError(values, a, b);
                    if (sse < BestSquaredError)
                    {
                        BestSquaredError = sse;
                        BestAlphaBeta = new Tuple<double, double>(a, b);
                    }
                }
            }

            return BestAlphaBeta;
        }

        private static double[] Smooth(double[] values, double alpha, double beta)
        {
            double[] smoothedValues = new double[values.Length];

            for (int i = 0; i < values.Length; i++)
            {
                smoothedValues[i] = CalculateSmoothing(values, alpha, beta, i);
            }



            return smoothedValues;
        }

        private static double CalculateSmoothing(double[] values, double alpha, double beta, int x)
        {
            // Console.WriteLine(x);
            if (x <= 1)
                return values[x];

            if (s.ContainsKey(x))
                return s[x];

            s[x] = alpha *
                    values[x] +
                    (1 - alpha) *
                    (CalculateSmoothing(values, alpha, beta, x - 1) +
                    CalculateBeta(beta, alpha, values, x - 1));

            return s[x];
        }

        private static double CalculateBeta(double beta, double alpha, double[] values, int x)
        {
            // Console.WriteLine(x);            
            if (x <= 1)
                return values[x] - values[x - 1];

            if (b.ContainsKey(x))
                return b[x];

            b[x] = beta *
                    (CalculateSmoothing(values, alpha, beta, x) -
                    CalculateSmoothing(values, alpha, beta, x - 1)) +
                    (1 - beta) *
                    CalculateBeta(beta, alpha, values, x - 1);

            return b[x];
        }

        private static double SquaredError(double[] points, double alpha, double beta)
        {
            var se = points
            .Skip(2)
            .Select(
                (x, i) => Math.Pow(
                    Math.Abs(
                        x - (CalculateSmoothing(points, alpha, beta, (i + 2) - 1) - CalculateBeta(beta, alpha, points, (i+2) - 1))
                        ),
                    2)
                    )
            .Sum() / (points.Length - 2);

            s.Clear();
            b.Clear();
            
            return Math.Sqrt(se);
        }

    }

}