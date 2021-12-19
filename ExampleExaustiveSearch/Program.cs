using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimizationToolbox;
using StarMathLib;

namespace ExampleExhaustiveSearch
{
    class Program
    {
        static void Main(string[] args)
        {
            Parameters.Verbosity = VerbosityLevels.Everything;
            // this next line is to set the Debug statements from OOOT to the Console.
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
            /* first a new optimization method in the form of a genetic algorithm is created. */
            var spaceDescriptor = new DesignSpaceDescription
            {
                new VariableDescriptor(1, 20, 1.0),
                new VariableDescriptor(0, 100, 1.241),
                new VariableDescriptor(-180, 180, 36000)
            };
            
            var optMethod = new ExhaustiveSearch(spaceDescriptor, optimize.minimize);
            optMethod.Add(spaceDescriptor);
            optMethod.Add(new MaxIterationsConvergence(spaceDescriptor.SizeOfSpace)); /* stop after 500 iteration (i.e. generations) */
            optMethod.Add(new MaxAgeConvergence(20, 0.000000001)); /*stop after 20 generations of the best not changing */
            optMethod.NumConvergeCriteriaNeeded = 2;

            optMethod.Add(new EfficiencyMeasurement());

            double[] xOptimal;
            var fOptimal = optMethod.Run(out xOptimal);
            Console.WriteLine("Convergence Declared by " + optMethod.ConvergenceDeclaredByTypeString);
            Console.WriteLine("X* = " + xOptimal.MakePrintString());
            Console.WriteLine("F* = " + fOptimal, 1);
            Console.WriteLine("NumEvals = " + optMethod.numEvals);
        }
    }
}
