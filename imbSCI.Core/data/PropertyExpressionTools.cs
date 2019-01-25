// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyExpressionTools.cs" company="imbVeles" >
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
namespace imbSCI.Core.data
{
    using imbSCI.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Tools to work with <see cref="PropertyExpression"/> graphs, to evaluate String path... See: <see cref="GetExpressionResolved(object, string)"/>
    /// </summary>
    public static class PropertyExpressionTools
    {
        public const String EXPRESSION_PATH_DELIMITER = ".";

        /// <summary>
        /// Resolves the specified expresion <c>path</c>, having <c>host</c> as starting node
        /// </summary>
        /// <param name="host">The host object - the starting point for expression path interpretation</param>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static PropertyExpression GetExpressionResolved(this Object host, String path)
        {
            PropertyExpression output = null;

            List<String> pathPart = imbSciStringExtensions.SplitSmart(path, EXPRESSION_PATH_DELIMITER);
            String root = "";
            if (pathPart.Any())
            {
                root = pathPart.First();
                output = new PropertyExpression(host, root);

                pathPart = pathPart.GetRange(1, pathPart.Count() - 1);
            }
            // pathPart.Remove(pathPart.First());
            
            Object head = host;
            if (output.property == null)
            {
                output.state = PropertyExpressionStateEnum.nothingResolved;
                output.undesolvedPart = path;
                return output;
            }
            String currentPart = root;
            foreach (String pp in pathPart)
            {
                currentPart = pp;

                var tmp = output.Add(pp);
                if (tmp.host == null)
                {
                    output.state = PropertyExpressionStateEnum.resolvedSome;
                    break;
                }
                else
                {
                    output = tmp;
                }
            }

            if (pathPart.Any() && currentPart != pathPart.LastOrDefault())
            {
                String unp = "";
                Int32 i = pathPart.IndexOf(currentPart);
                for (int j = i; j < pathPart.Count; j++)
                {
                    unp = imbSciStringExtensions.add(unp, pathPart[j], EXPRESSION_PATH_DELIMITER);
                }
                output.undesolvedPart = unp;
            }
            else
            {
                if (output.property != null)
                {
                    output.state = PropertyExpressionStateEnum.resolvedAll;
                }
            }
            return output;
        }
    }
}