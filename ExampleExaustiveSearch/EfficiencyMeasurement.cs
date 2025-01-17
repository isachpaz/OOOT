﻿using OptimizationToolbox;

namespace ExampleExhaustiveSearch
{
    public class EfficiencyMeasurement : IObjectiveFunction
    {
        public double calculate(double[] x)
        {
            double numberOfPasses = (x[0] - 5) * (x[0] - 5); /* optimal @ 5 */
            double length = (x[1] - 25) / 25; /* optimal @ 50 */
            double angle = (x[2] + 150) / 30; /* optimal @ -120*/

            /* this is a made up function - basically the banana function again offset*/
            var f = numberOfPasses + ((1 - length) * (1 - length) + 100.0 * (angle - length * length)
                                                                          * (angle - length * length));
            SearchIO.output(f, 10);
            return f;
        }

    }
}