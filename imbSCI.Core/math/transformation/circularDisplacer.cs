// --------------------------------------------------------------------------------------------------------------------
// <copyright file="circularDisplacer.cs" company="imbVeles" >
//
// Copyright (C) 2018 imbVeles
//
// This program is free software: you can redistribute it and/or modify
// it under the +terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// <summary>
// Project: imbSCI.Core
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace imbSCI.Core.math.transformation
{
    /// <summary>
    /// Geometry/translation utility class, representing rotational turret/wheel with given number of <see cref="nestPoint"/>.
    /// </summary>
    /// <remarks>
    /// <para>The class describes X,Y position and angular state (rotation by central Z axis). Provides methods for: central X,Y offset of <see cref="nestPoint"/>s</para>
    /// <para>Use indexer (circularDisplacer[n]) to get <see cref="PointF"/> offset for nest point <c>n</c></para>
    /// </remarks>
    public class circularDisplacer
    {
        /// <summary>
        /// Creates instance with <c>nestCount</c> of <see cref="nestPoint"/>s, placed in full circle, having even angular distances. The first <see cref="nestPoint"/> has <see cref="nestPoint.angularPosition"/> of the <c>zeroOffset</c>. The <see cref="nestPoint"/>s are placed at <c>distance</c> from central point of the wheel
        /// </summary>
        /// <param name="nestCount">Number of <see cref="nestPoint"/>s to create</param>
        /// <param name="zeroOffset">Angular starting point for the first nest point</param>
        /// <param name="distance">Distance from central point - to be set for all <see cref="nestPoint"/>s.</param>
        public circularDisplacer(Int32 nestCount = 6, Double zeroOffset = 30, Double distance = 35.5)
        {
            Double step = 360 / nestCount;

            for (int i = 0; i < nestCount; i++)
            {
                Double a = (step * i) + zeroOffset;
                var n = new nestPoint(a, distance);
                points.Add(n);
            }
        }

        /// <summary>
        /// Gets the <see cref="PointF"/> with central offset of a <see cref="nestPoint"/>, identified by <c>nestID</c>.
        /// </summary>
        /// <value>
        /// The <see cref="PointF"/>, describing X,Y offset from central point of the <see cref="circularDisplacer"/> instance and <see cref="nestPoint"/> identified by <c>nestID</c>.
        /// </value>
        /// <param name="nestID">The nest identifier (from 0 to nest count-1). If negative value, it is interpreted as selecting the nest from end of the list, not from the begining.</param>
        /// <returns>Point with X and Y axis central point offset.</returns>
        /// <exception cref="ArgumentOutOfRangeException">nestID - There are no nestpoints defined in this instance! Seems you are using wrong constructor/instancing approach.</exception>
        public PointF this[Int32 nestID]
        {
            get
            {
                if (!points.Any())
                {
                    throw new ArgumentOutOfRangeException(nameof(nestID), "There are no nestpoints defined in this instance! Seems you are using wrong constructor/instancing approach.");
                }
                if (nestID < 0)
                {
                    nestID = nestID % points.Count;
                    nestID = points.Count + nestID;
                }

                if (nestID >= points.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(nestID), "There are [" + points.Count + "] in the turret, can't select specified [" + nestID + "]. The max. is [" + (points.Count - 1) + "] no nestpoints defined in this instance! Seems you are using wrong constructor/instancing approach.");
                }

                nestPoint np = points[nestID];
                Double d = np.angularPosition.Degrees;
                var _x = Math.Cos(d) * np.distanceFromCenter;
                var _y = Math.Sin(d) * np.distanceFromCenter;

                PointF output = new PointF((float)_x, (float)_y);

                return output;
            }
        }

        /// <summary>
        /// Gets or sets the points.
        /// </summary>
        /// <value>
        /// The points.
        /// </value>
        public List<nestPoint> points { get; protected set; } = new List<nestPoint>();

        /// <summary>
        /// Describes one nest/point, in the <see cref="circularDisplacer"/> turret. It is described by angular position and distance from the center
        /// </summary>
        public class nestPoint
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="nestPoint"/> class.
            /// </summary>
            /// <param name="angle">The angle.</param>
            /// <param name="distance">The distance.</param>
            public nestPoint(Double angle, Double distance)
            {
                angularPosition = new math.angle(angle, math.angle.Type.Degrees);
                distanceFromCenter = distance;
            }

            /// <summary>
            /// Gets or sets the angular position.
            /// </summary>
            /// <value>
            /// The angular position.
            /// </value>
            public angle angularPosition { get; set; } = new angle(angle.Preset.Deg0);

            /// <summary>
            /// Gets or sets the distance from center.
            /// </summary>
            /// <value>
            /// The distance from center.
            /// </value>
            public double distanceFromCenter { get; set; } = 100;
        }
    }
}