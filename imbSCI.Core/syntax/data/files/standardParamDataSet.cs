// --------------------------------------------------------------------------------------------------------------------
// <copyright file="standardParamDataSet.cs" company="imbVeles" >
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
namespace imbSCI.Core.syntax.data.files
{
    using imbSCI.Core.syntax.data.core;
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Podrzava ucitavanje> bat, e-mac.mch, t-punch.cfg
    /// </summary>
    public class standardParamDataSet : textDataSetWithComments<paramPair>
    {
        public override bool afterLoad()
        {
            // throw new NotImplementedException();
            return true;
        }

        public override bool processToObject(object _target)
        {
            throw new NotImplementedException();
        }

        public override bool processToDictionary()
        {
            throw new NotImplementedException();
        }

        public override bool beforeSave()
        {
            //  throw new NotImplementedException();
            return true;
        }

        public override bool save(string _path = "")
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<paramPair> processLines()
        {
            paramPairs output = new paramPairs();
            int i = 0;
            foreach (String ln in lines)
            {
                paramPair oln = processLine(ln, i);
                output.Add(oln);
                i++;
            }
            loadedParams = output;
            return output;
        }

        public override core.paramPair processLine(string _line, int i)
        {
            if (String.IsNullOrEmpty(_line)) return null;
            _line = _line.Trim();
            if (String.IsNullOrEmpty(_line)) return null;

            paramPair output = new paramPair();

            if (_line.StartsWith("#"))
            {
                output.isComment = true;
                output.name = "#" + i.ToString("D3");
                output.value = _line.Trim('#');
                return output;
            }

            if (paramLine_set.IsMatch(_line))
            {
                Match mc = paramLine_set.Match(_line);
                output.name = mc.Groups[1].Value.Trim();
                if (output.name.StartsWith("set "))
                {
                    output.name = output.name.Substring(3).Trim();
                }
                output.value = mc.Groups[2].Value.Trim();
                return output;
            }

            if (paramLine_tab.IsMatch(_line))
            {
                Match mc = paramLine_tab.Match(_line);
                output.name = mc.Groups[1].Value.Trim();
                output.value = mc.Groups[2].Value.Trim();
                return output;
            }

            return output;
        }
    }
}