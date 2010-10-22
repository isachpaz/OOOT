﻿/*************************************************************************
 *     This file & class is part of the Object-Oriented Optimization
 *     Toolbox (or OOOT) Project
 *     Copyright 2010 Matthew Ira Campbell, PhD.
 *
 *     OOOT is free software: you can redistribute it and/or modify
 *     it under the terms of the GNU General Public License as published by
 *     the Free Software Foundation, either version 3 of the License, or
 *     (at your option) any later version.
 *  
 *     OOOT is distributed in the hope that it will be useful,
 *     but WITHOUT ANY WARRANTY; without even the implied warranty of
 *     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *     GNU General Public License for more details.
 *  
 *     You should have received a copy of the GNU General Public License
 *     along with OOOT.  If not, see <http://www.gnu.org/licenses/>.
 *     
 *     Please find further details and contact information on OOOT
 *     at http://ooot.codeplex.com/.
 *************************************************************************/
using System;
using System.Collections.Generic;

namespace OptimizationToolbox
{
    /// <summary>
    /// 
    /// </summary>
    public class MaxTimeConvergence : abstractConvergence
    {
        private readonly DateTime startTime;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MaxTimeConvergence"/> class.
        /// </summary>
        public MaxTimeConvergence()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaxTimeConvergence"/> class.
        /// </summary>
        /// <param name="maxTime">The max time.</param>
        public MaxTimeConvergence(TimeSpan maxTime)
        {
            this.maxTime = maxTime;
            startTime = DateTime.Now;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaxTimeConvergence"/> class.
        /// </summary>
        /// <param name="timeToStop">The time to stop.</param>
        public MaxTimeConvergence(DateTime timeToStop)
        {
            startTime = DateTime.Now;
            maxTime = timeToStop - startTime;
        }

        #endregion

        /// <summary>
        /// Gets or sets the maximum time span for the process.
        /// </summary>
        /// <value>The max time.</value>
        public TimeSpan maxTime { get; set; }

        /// <summary>
        /// Given a value for maxTime, this criteria will return true, when the process reaches
        /// this length of time. It does not use any of the arguments below, but in some ways
        /// is the most user-friendly criteria. Use wisely. Use often.
        /// </summary>
        /// <param name="iteration">The number of iterations (not used).</param>
        /// <param name="numFnEvals">The number of function evaluations.</param>
        /// <param name="fBest">The best f.</param>
        /// <param name="xBest">The best x.</param>
        /// <param name="population">The population of candidates.</param>
        /// <param name="gradF">The gradient of F.</param>
        /// <returns>
        /// true or false - has the process converged?
        /// </returns>
        public override bool converged(long iteration = -1, long numFnEvals = -1, double fBest = double.NaN, IList<double> xBest = null, IList<double[]> population = null, IList<double> gradF = null)
        {
            return ((DateTime.Now - startTime) >= maxTime);
        }
    }
}