﻿using System;
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
            //Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
            /* first a new optimization method in the form of a genetic algorithm is created. */
            var spaceDescriptor = new DesignSpaceDescription
            {
                new VariableDescriptor(1, 20, 0.01),
                new VariableDescriptor(0, 100, 0.01),
                new VariableDescriptor(-180, 180, 10000)
            };

            var optMethod = new HillClimbing();
            //var optMethod = new NelderMead();
            //var optMethod = new ExhaustiveSearch(spaceDescriptor, optimize.minimize);
            optMethod.Add(spaceDescriptor);
            optMethod.Add(
                new MaxIterationsConvergence(spaceDescriptor
                    .SizeOfSpace)); /* stop after 500 iteration (i.e. generations) */
            optMethod.Add(new MaxAgeConvergence(20,
                0.000000001)); /*stop after 20 generations of the best not changing */
            optMethod.NumConvergeCriteriaNeeded = 1;
            //optMethod.Add(new squaredExteriorPenalty(optMethod, 8));
            optMethod.Add(new RandomNeighborGenerator(spaceDescriptor));
            optMethod.Add(new KeepSingleBest(optimize.minimize));
            optMethod.Add(new ExhaustiveNeighborGenerator(spaceDescriptor));


            optMethod.Add(new EfficiencyMeasurement());


            double[] xOptimal1;
            var fOptimal1 = optMethod.Run(out xOptimal1);


            Random r = new Random();

            double[] bestSolution = null;
            double fBest = Double.MaxValue;

            RandomSampling randomSampling = new RandomSampling(spaceDescriptor);


            List<ICandidate> candidates = new List<ICandidate>();
            randomSampling.GenerateCandidates(ref candidates,100);

            var samples =
                randomSampling.GenerateCandidates(candidates.FirstOrDefault()?.x, 100);


            foreach (var xInit in samples)
            {
                double[] xOptimal;
                var fOptimal = optMethod.Run(out xOptimal, xInit);

                if (fBest > fOptimal)
                {
                    fBest = fOptimal;
                    bestSolution = xOptimal;
                }

                Console.WriteLine($"Init values = {xInit.MakePrintString()}");
                Console.WriteLine("Convergence Declared by " + optMethod.ConvergenceDeclaredByTypeString);
                Console.WriteLine("X* = " + xOptimal.MakePrintString());
                Console.WriteLine("F* = " + fOptimal, 1);
                Console.WriteLine("NumEvals = " + optMethod.numEvals);
                Console.WriteLine("=======================================");
            }
        }
    }
}