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
using imbSCI.Core.enums;
using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.table;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Core.files;
using imbSCI.Core.files.folders;
using imbSCI.Core.reporting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace imbSCI.Core.math.classificationMetrics
{
    /// <summary>
    /// 
    /// </summary>
    public class reportDataFlagDictionary
    {
        public reportDataFlagDictionary()
        {

        }

        /// <summary>
        /// Processes the name of the run, and assignes data entries for matched flags
        /// </summary>
        /// <param name="_name">The name.</param>
        /// <returns></returns>
        public String ProcessRunName(String _name, reportExpandedData data)
        {
            String __name = _name;
            foreach (reportDataFlag flag in items)
            {
                List<String> match = new List<string>();
                foreach (String fl in flag.needles)
                {

                    if (__name.Contains(fl))
                    {
                        if (flag.replacement != "*")
                        {
                            __name = __name.Replace(fl, flag.replacement);
                        }
                        match.Add(fl);
                    }

                }
                if (match.Any())
                {
                    data.Add(flag.name, match.toCsvInLine(), flag.needles.ToString());
                }

            }
            return __name;
        }




        /// <summary>
        /// Deploys the specified name.
        /// </summary>
        /// <param name="_name">The name.</param>
        /// <param name="_needles">The needles.</param>
        /// <param name="_replacement">The replacement.</param>
        public reportDataFlag Add(String _name, String _needles, String _replacement, String _description)
        {
            reportDataFlag flag = new reportDataFlag();
            flag.Deploy(_name, _needles, _replacement, _description);
            items.Add(flag);
            return flag;
        }

        public List<reportDataFlag> items { get; set; } = new List<reportDataFlag>();
    }
}