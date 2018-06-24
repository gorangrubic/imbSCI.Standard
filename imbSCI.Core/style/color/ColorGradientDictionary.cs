// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorGradientDictionary.cs" company="imbVeles" >
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
using System.Linq;

//using Accord.Imaging;

namespace imbSCI.Core.style.color
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="System.Collections.Generic.Dictionary{System.String, System.Double}" />
    public class ColorGradientDictionary : Dictionary<String, Double>
    {
        public List<String> GetColors()
        {
            return Keys.ToList();
        }

        public List<String> GetColors(Double rStart, Double rEnd)
        {
            List<String> output = new List<string>();
            foreach (var pair in this)
            {
                if (pair.Value >= rStart && pair.Value < rEnd)
                {
                    output.Add(pair.Key);
                }
            }
            return output;
        }

        public String GetColor(Double r)
        {
            String first = "";
            foreach (var pair in this)
            {
                if (first == "") first = pair.Key;
                if (pair.Value > r)
                {
                    return pair.Key;
                }
            }
            return first;
        }
    }
}