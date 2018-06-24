// --------------------------------------------------------------------------------------------------------------------
// <copyright file="nestedParamDataSet.cs" company="imbVeles" >
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
namespace imbSCI.Core.syntax.data
{
    using imbSCI.Core.syntax.data.core;
    using imbSCI.Core.syntax.data.files;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Text.RegularExpressions;

    public class nestedParamDataSet : textDataSetWithComments<paramPair>
    {
        protected Regex paramLine_nested = new Regex(@"([\w]+)\s");

        public override bool afterLoad()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public override bool save(string _path = "")
        {
            throw new NotImplementedException();
        }

        #region -----------  groups  -------  [lista pronadjenih grupa nestovanih parametara]

        private List<paramNestedPairs> _groups = new List<paramNestedPairs>(); // = new List<paramNestedPairs>();

        /// <summary>
        /// lista pronadjenih grupa nestovanih parametara
        /// </summary>
        // [XmlIgnore]
        [Category("nestedParamDataSet")]
        [DisplayName("groups")]
        [Description("lista pronadjenih grupa nestovanih parametara")]
        public List<paramNestedPairs> groups
        {
            get
            {
                return _groups;
            }
            set
            {
                // Boolean chg = (_groups != value);
                _groups = value;
                OnPropertyChanged("groups");
                // if (chg) {}
            }
        }

        #endregion -----------  groups  -------  [lista pronadjenih grupa nestovanih parametara]

        #region -----------  currentGroup  -------  [trenutna grupa]

        private paramNestedPairs _currentGroup; // = new paramNestedPairs();

        /// <summary>
        /// trenutna grupa
        /// </summary>
        // [XmlIgnore]
        [Category("nestedParamDataSet")]
        [DisplayName("currentGroup")]
        [Description("trenutna grupa")]
        public paramNestedPairs currentGroup
        {
            get
            {
                return _currentGroup;
            }
            set
            {
                // Boolean chg = (_currentGroup != value);
                _currentGroup = value;
                OnPropertyChanged("currentGroup");
                // if (chg) {}
            }
        }

        #endregion -----------  currentGroup  -------  [trenutna grupa]

        /// <summary>
        /// Obradjuje sve linije
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<paramPair> processLines()
        {
            paramPairs output = new paramPairs();
            currentGroup = null;

            int i = 0;
            foreach (String ln in lines)
            {
                paramPair oln = processLine(ln, i);
                if (currentGroup == null)
                {
                    output.Add(oln);
                }
                else
                {
                    currentGroup.Add(oln);
                }
                if (oln != null)
                {
                    if (oln.isComment) headerComments.Add(oln.value);
                }
                i++;
            }

            return output;
        }

        /// <summary>
        /// Obradjuje jednu liniju
        /// </summary>
        /// <param name="_line"></param>
        /// <param name="i"> </param>
        /// <returns></returns>
        public override paramPair processLine(string _line, int i)
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

            if (!(_line.Contains("=") || _line.Contains(";")))
            {
                _line = _line.Replace("{", "");
                _line = _line.Trim();

                if (String.IsNullOrEmpty(_line))
                {
                }
                else
                {
                    if (_line.Contains("}"))
                    {
                        currentGroup = null;
                    }
                    else
                    {
                        paramNestedPairs newGroup = new paramNestedPairs(_line);
                        currentGroup = newGroup;
                        groups.Add(newGroup);
                    }
                }
            }

            output = null;

            return output;
        }
    }
}