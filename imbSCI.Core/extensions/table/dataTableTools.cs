// --------------------------------------------------------------------------------------------------------------------
// <copyright file="dataTableTools.cs" company="imbVeles" >
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

namespace imbSCI.Core.extensions.table
{
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.reporting.lowLevelApi;
    using imbSCI.Data;
    using System.Data;
    using System.Text;

    /// <summary>
    /// 
    /// </summary>
    public static class dataTableTools
    {
        public static Boolean validationDisabled = false;

        /// <summary>
        /// Renders the table into text
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static String GetTextTable(this DataTable source)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Title \t\t\\tt [" + source.GetTitle() + "]");
            sb.AppendLine("Description \t\t\t [" + source.GetDescription() + "]");
            sb.AppendLine("---------------------------------------------------------");

            sb.AppendLine(source.markdownTable(true));

            sb.AppendLine("---------------------------------------------------------");

            var extra = source.GetExtraDesc();
            foreach (String ln in extra)
            {
                sb.AppendLine(ln);
            }

            return sb.ToString();
        }


        /// <summary>
        /// Validates the table.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static Boolean validateTable(this DataTable source)
        {
            if (validationDisabled) return true;
            if (source == null) return false;
            if (source.Columns.Count == 0) return false;
            if (source.Rows.Count == 0) return false;

            if (source.TableName.isNullOrEmpty())
            {
                source.TableName = source.GetTitle().getFilename();
                if (source.TableName.isNullOrEmpty())
                {
                    source.TableName = "DataTable_" + imbStringGenerators.getRandomString(8);
                    return true;
                }
                else
                {
                    return true;
                }
            }
            return true;
        }
    }
}