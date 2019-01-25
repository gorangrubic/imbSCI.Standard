// --------------------------------------------------------------------------------------------------------------------
// <copyright file="classificationReport.cs" company="imbVeles" >
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
using imbSCI.Core.attributes;
using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Core.files;
using imbSCI.Core.files.folders;
using imbSCI.Core.reporting;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace imbSCI.Core.math.classificationMetrics
{
    /// <summary>
    /// Collection of custom data entries
    /// </summary>
    /// <seealso cref="System.Collections.Generic.List{imbSCI.Core.math.classificationMetrics.reportExpandedDataPair}" />
    public class reportExpandedData : List<reportExpandedDataPair>
    {
        /// <summary>
        /// Gets the dictionary of the data pairs
        /// </summary>
        /// <returns></returns>
        public Dictionary<String, reportExpandedDataPair> GetDictionary()
        {
            Dictionary<String, reportExpandedDataPair> output = new Dictionary<string, reportExpandedDataPair>();
            foreach (var ent in this)
            {
                if (!output.ContainsKey(ent.key))
                {
                    output.Add(ent.key, ent.CloneViaXML());
                }
                else
                {
                    if (ent.value != "")
                    {
                        if (output[ent.key].value != "")
                        {
                            output[ent.key].value += ", " + ent.value;
                        }
                        else
                        {
                            output[ent.key].value = ent.value;
                        };
                    }
                }

            }
            return output;
        }

        public Double GetMeanValue(String _key)
        {
            var pair = this.FirstOrDefault(x => x.key == _key);
            if (pair == null) return 0;

            var ps = pair.value.SplitSmart(",");
            List<Double> dbl = new List<double>();

            foreach (var s in ps)
            {
                String so = s.Trim(" ,".ToArray());

                dbl.Add(Double.Parse(so));
            }
            if (!dbl.Any()) return 0;
            return dbl.Average();
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="_key">The key.</param>
        /// <param name="_value">The value.</param>
        /// <param name="_description">The description.</param>
        public void Add(String _key, String _value, String _description = "")
        {
            reportExpandedDataPair pair = new reportExpandedDataPair()
            {
                key = _key,
                value = _value,
                description = _description

            };
            Add(pair);
        }




        /// <summary>
        /// Merges the specified pairs.
        /// </summary>
        /// <param name="pairs">The pairs.</param>
        public void Merge(IEnumerable<reportExpandedDataPair> pairs)
        {
            foreach (reportExpandedDataPair pair in pairs)
            {
                if (this.Any(x => x.key == pair.key))
                {
                    var match = this.First(x => x.key == pair.key);
                    match.Merge(pair);
                }
                else
                {
                    Add(pair);
                }
            }

        }


        /// <summary>
        /// Initializes a new instance of the <see cref="reportExpandedData"/> class.
        /// </summary>
        public reportExpandedData()
        {

        }
    }
}