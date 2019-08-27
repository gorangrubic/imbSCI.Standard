// --------------------------------------------------------------------------------------------------------------------
// <copyright file="regexMarkerCollection.cs" company="imbVeles" >
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
// Project: imbSCI.Data
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Data.data.text
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;


    /*
    public class regexMarkerCollectionSet:List<IRegexMarkerCollection>
    {

         /// <summary>
        /// Processes the specified text input into <see cref="regexMarkerResultCollection{T}"/>
        /// </summary>
        /// <param name="input">The input text to be parsed</param>
        /// <returns>Collection with matched results</returns>
        public regexMarkerResultCollection process(String input)
        {
            regexMarkerResultCollection output = new regexMarkerResultCollection();

            String scrambled = input;
            foreach (IRegexMarkerCollection collection in this)
            {
                foreach (IRegexMarker reg in collection.Values)
                {
                    if (reg != null)
                    {
                        MatchCollection mchs = reg.test.Matches(scrambled);
                        foreach (Match m in mchs)
                        {
                            regexMarkerResult res = new regexMarkerResult(m, reg.marker);
                            output.AddResult(res);
                        }
                        scrambled = reg.test.Replace(scrambled, replacementGenerator);
                    }
                }
            }
            

            String[] rest = scrambleCut.Split(scrambled);

            Int32 index = 0;

            regexMarkerResult restResult = null;
            foreach (String rst in rest)
            {
                if (output.byAllocation.ContainsKey(index))
                {
                    index = index + output.byAllocation[index].First().match.Length;
                }
                else
                {
                    index = output.AddResult(rst, index);
                }
            }
            output.length = index;

            return output;
        }

    }

    public interface IRegexMarkerCollection
    {
        Regex scrambleCut { get; set; }

        String replacementGenerator(Match m);
    }*/

    /// <summary>
    /// Collection of Regex markers
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class regexMarkerCollection 
    {
        public List<IRegexMarker> Markers { get; set; } = new List<IRegexMarker>();

        public IRegexMarker GetDefinition(Object marker)
        {
            
            return Index[marker];
            
        }

        protected Dictionary<Object, IRegexMarker> Index { get; set; } = new Dictionary<Object, IRegexMarker>();

        
          public const String REPLACEMENT_PATTERN = "#";
        

            

        /// <summary>
        /// Adds new marker rule
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(IRegexMarker item)
        {
            if (item == null) return;

            if (item.marker == null)
            {
                //item.marker = defaultMarker;
            }
            if (!Index.ContainsKey(item.marker)) Index.Add(item.marker, item);

            Markers.Add(item);
            
        }

        /// <summary>
        /// The default marker to be applied to the unmatched parts of the text
        /// </summary>
        public Object defaultMarker = null;

        public Boolean SerialMode { get; set; } = false;

              public Regex scrambleCut { get; set; } = new Regex("(" + REPLACEMENT_PATTERN + "+)");

        /// <summary>
        /// Processes the specified text input into <see cref="regexMarkerResultCollection{T}"/>
        /// </summary>
        /// <param name="input">The input text to be parsed</param>
        /// <returns>Collection with matched results</returns>
        public regexMarkerResultCollection process(String input)
        {
            regexMarkerResultCollection output = new regexMarkerResultCollection();

            String scrambled = input;
            
               
            if (SerialMode)
            {
                Boolean matchFound = true;
                do
                {
                    matchFound = false;
                    foreach (var reg in Markers)
                    {
                        if (reg != null)
                        {
                            Match m = reg.test.Match(scrambled);
                            if (m.Success)
                            {
                                regexMarkerResult res = new regexMarkerResult(m, reg.marker);
                                output.AddResult(res);
                                scrambled = reg.ReplaceMatch(m, scrambled);
                                matchFound = true;
                            }
                        }
                    }
                } while (matchFound);
            }
            else
            {
                foreach (var reg in Markers)
                {
                    if (reg != null)
                    {
                        MatchCollection mchs = reg.test.Matches(scrambled);
                        foreach (Match m in mchs)
                        {
                            regexMarkerResult res = new regexMarkerResult(m, reg.marker);
                            output.AddResult(res);
                        }
                        scrambled = reg.test.Replace(scrambled, reg.replacementGenerator);
                    }
                }
            }

            

            String[] rest = scrambleCut.Split(scrambled);

            Int32 index = 0;

            regexMarkerResult restResult = null;
            foreach (String rst in rest)
            {
                if (output.byAllocation.ContainsKey(index))
                {
                    index = index + output.byAllocation[index].First().match.Length;
                }
                else
                {
                    index = output.AddResult(rst, index);
                }
            }
            output.length = index;

            return output;
        }
    }
}