// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ncLineExtensions.cs" company="imbVeles" >
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
namespace imbSCI.Core.syntax.nc.line
{
    using imbSCI.Core.extensions.text;
    using imbSCI.Data;
    using System;
    using System.Linq;

    public static class ncLineExtensions
    {
        public static String writeLineCriteriaInline(this ncLineCriteria criteria)
        {
            String output = "";
            output = output.add("Criteria type[" + criteria.GetType().Name + "]");

            if (criteria.commandCriteria == ncLineCommandPredefined.custom)
            {
                output.add("cust.command[" + criteria.customCommand + "]");
            }
            else
            {
                output.add("command[" + criteria.commandCriteria.ToString() + "]");
            }

            if (criteria is ncLineRelativeCriteria)
            {
                ncLineRelativeCriteria rc = criteria as ncLineRelativeCriteria;

                output.addVal("relation type: ", rc.relativeType.ToString());

                output.addVal("relation offset: ", rc.relativePosition.ToString());
            }

            if (criteria is ncLineSelector)
            {
                ncLineSelector sc = criteria as ncLineSelector;

                output.addVal("included relative criterias: ", sc.Count().ToString());

                foreach (ncLineRelativeCriteria rc in sc)
                {
                    output.log(rc.writeLineCriteriaInline());
                }
            }
            return output;
        }
    }
}