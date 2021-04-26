﻿// ***********************************************************************
// Assembly         : OptimizationToolbox
// Author           : campmatt
// Created          : 01-28-2021
//
// Last Modified By : campmatt
// Last Modified On : 01-28-2021
// ***********************************************************************
// <copyright file="HillClimbing.cs" company="OptimizationToolbox">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
/*************************************************************************
 *     This file & class is part of the Object-Oriented Optimization
 *     Toolbox (or OOOT) Project
 *     Copyright 2010 Matthew Ira Campbell, PhD.
 *
 *     OOOT is free software: you can redistribute it and/or modify
 *     it under the terms of the MIT X11 License as published by
 *     the Free Software Foundation, either version 3 of the License, or
 *     (at your option) any later version.
 *  
 *     OOOT is distributed in the hope that it will be useful,
 *     but WITHOUT ANY WARRANTY; without even the implied warranty of
 *     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *     MIT X11 License for more details.
 *  


 *     
 *     Please find further details and contact information on OOOT
 *     at http://designengrlab.github.io/OOOT/.
 *************************************************************************/
using System.Collections.Generic;
using System.Linq;
using StarMathLib;

namespace OptimizationToolbox
{
    /// <summary>
    /// Class HillClimbing.
    /// Implements the <see cref="OptimizationToolbox.abstractOptMethod" />
    /// </summary>
    /// <seealso cref="OptimizationToolbox.abstractOptMethod" />
    public class HillClimbing : abstractOptMethod
    {
        /// <summary>
        /// Gets the neighbor generator.
        /// </summary>
        /// <value>The neighbor generator.</value>
        public abstractGenerator neighborGenerator { get; private set; }
        /// <summary>
        /// Gets the selector.
        /// </summary>
        /// <value>The selector.</value>
        public abstractSelector selector { get; private set; }

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="HillClimbing"/> class.
        /// </summary>
        public HillClimbing()
        {
            RequiresObjectiveFunction = true;
            ConstraintsSolvedWithPenalties = false;
            InequalitiesConvertedToEqualities = false;
            RequiresSearchDirectionMethod = false;
            RequiresLineSearchMethod = false;
            RequiresAnInitialPoint = true;
            RequiresConvergenceCriteria = true;
            RequiresFeasibleStartPoint = false;
            RequiresDiscreteSpaceDescriptor = true;
        }

        #endregion

        /// <summary>
        /// Adds the specified object to the optimization routine.
        /// </summary>
        /// <param name="function">The object, function.</param>
        public override void Add(object function)
        {
            if (function is abstractGenerator)
                neighborGenerator = (abstractGenerator)function;
            else if (function is abstractSelector)
                selector = (abstractSelector)function;
            else base.Add(function);
        }

        /// <summary>
        /// Runs the specified x star.
        /// </summary>
        /// <param name="xStar">The x star.</param>
        /// <returns>System.Double.</returns>
        protected override double run(out double[] xStar)
        {
            var candidates = new List<ICandidate> { new Candidate(calc_f(x), x) };
            while (notConverged(k++, numEvals, candidates[0].objectives[0], candidates[0].x))
            {
                SearchIO.output(k + ": f = " + candidates[0].objectives[0], 4);
                SearchIO.output("     x = " + candidates[0].x.MakePrintString(), 4);
                var neighbors = neighborGenerator.GenerateCandidates(candidates[0].x);
                var feasibleAndEvaluatedNeighbors =
                    from neighbor in neighbors
                    where ConstraintsSolvedWithPenalties || feasible(neighbor)
                    let f = calc_f(neighbor)
                    select new Candidate(f, neighbor);
                candidates.AddRange(feasibleAndEvaluatedNeighbors.Cast<ICandidate>());
                selector.SelectCandidates(ref candidates);
            }
            xStar = candidates[0].x;
            return candidates[0].objectives[0];
        }
    }
}