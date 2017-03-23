using System;
using System.Linq;

namespace Smoothing
{

    public static class SES
    {
        public static double[] ForeCast(double[] values, double alpha, int left, int right) {
            double[] forecast = new double[values.Length + right - left + 1];
            Smooth(values, alpha).CopyTo(forecast, 0);
            double s1 = values.Take(12).Average();
            double Sn = calculateSmoothing(alpha, values, values.Length, s1);

            for(int i = left; i <= right; i++) {
                forecast[i - 1] = Sn;
            }

            return forecast;
        }
        public static double FindBestAlpha(double[] values, double stepSize) {
            double currentBestAlpha = 0;
            double currentBestSquaredError = double.MaxValue;
             
            for (double a = 0; a < 1; a += stepSize)
            {
                var se = SquaredError(values, Smooth(values, a));

               if(se < currentBestSquaredError) {
                   currentBestAlpha = a;
                   currentBestSquaredError = se;
               }     
            }
            return currentBestAlpha;
        }
        private static double[] Smooth(double[] values, double alpha)
        {
            double s1 = values[0];
            double[] smoothedValues = new double[values.Length];

            //Get better initial value if values.Length > 12 (magic number)
            if (values.Length >= 12)
                s1 = values.Take(12).Average();
            
            smoothedValues[0] = s1;
            for (int i = 1; i < values.Length; i++)
            {
                smoothedValues[i] = calculateSmoothing(alpha, values, i, s1);
            }
            SquaredError(values, smoothedValues);
            return smoothedValues;
        }

        private static double SquaredError(double[] points, double[] smoothedPoints) {
            var se = Math.Sqrt(points.Zip(smoothedPoints, (x, y) => Math.Pow(Math.Abs(x - y), 2)).Sum()/(points.Length - 1));

            return se;
        }

        private static double calculateSmoothing(double alpha, double[] values, int x, double s1)
        {
            if (x == 0)
                return s1;
            return alpha * values[x - 1] + (1 - alpha) * calculateSmoothing(alpha, values, x - 1, s1);
        }
    }
}