// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorGradient.cs" company="imbVeles" >
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
using imbSCI.Core.collection;
using imbSCI.Core.math;
using imbSCI.Core.math.range.finder;
using imbSCI.Core.math.range.frequency;
using imbSCI.Data.collection.special;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
//using Accord.Imaging;
using System.Xml.Serialization;

namespace imbSCI.Core.style.color
{
    public class ColorGradientForInstanceEnumeration<T> 
    {

        public ColorGradientForInstanceEnumeration(String HexA, String HexB)
        {
            PrepareRoot(HexA, HexB);
        }

        public ColorGradient rootGradient { get; set; } 

        public void PrepareRoot(String HexA, String HexB)
        {
            rootGradient = new ColorGradient(HexA, HexB, ColorGradientFunction.HueAToB | ColorGradientFunction.Saturation);
        }

        ListDictionary<T, String> CompiledColors = new ListDictionary<T, string>();

        Dictionary<T, circularSelector<String>> ColorSelectors = new Dictionary<T, circularSelector<String>>();
        Dictionary<T, String> RootColors = new Dictionary<T, String>();

        public String GetColor(T instance, Boolean getRootColor=false)
        {
            if (getRootColor) return RootColors[instance];
            return ColorSelectors[instance].next();
        }

        

        public void Prepare(IEnumerable<T> instances, float minBrightness=0.6F, float maxBrightness=0.9F)
        {
            ColorSelectors = new Dictionary<T, circularSelector<String>>();

            frequencyCounter<T> counter = new frequencyCounter<T>();

            foreach (var inst in instances)
            {
                counter.Count(inst);
            }
            var types = counter.GetDistinctItems();

            var rootColors = rootGradient.GetColorHSVSteps(types.Count); //GetColorSteps(types.Count);

            foreach (var type in types )
            {
                Int32 ti = types.IndexOf(type);
                var rootColor = rootColors[ti];

                var A = rootColor.Clone(); 
                A.V = minBrightness;
                var B = rootColor.Clone();
                B.V = maxBrightness;

                RootColors.Add(type, A.GetHexColor(true));
                

                ColorGradient typeGradient = new ColorGradient(A.GetHexColor(true), B.GetHexColor(true), ColorGradientFunction.AllAToB);
                
                ColorSelectors.Add(type, new circularSelector<string>(typeGradient.GetColorSteps(counter.GetFrequencyForItem(type))));

            }
        }

    }
}