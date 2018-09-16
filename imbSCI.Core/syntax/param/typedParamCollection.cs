// --------------------------------------------------------------------------------------------------------------------
// <copyright file="typedParamCollection.cs" company="imbVeles" >
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
namespace imbSCI.Core.syntax.param
{
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.text;
    using imbSCI.Data.enums.fields;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Kolekcija parametara sa dodeljenom vrednoscu
    /// </summary>
    public class typedParamCollection : List<typedParam>
    {
        /// <summary>
        /// Information about params
        /// </summary>
        private typedParamInfoCollection _info;

        /// <summary>
        /// Information about params
        /// </summary>
        internal typedParamInfoCollection info
        {
            get
            {
                return _info;
            }
        }

        /// <summary>
        /// Each Match is one parameter declaration
        /// </summary>
        public static Regex PARAMFORMAT_DoubleDot = new Regex("([\\w]*)[\\s=]?[\\\"]?([\\w\\d\\.\\*,:\\?\\+\\-_\\!`&\\$\\%\\s]*)[\\\"]?:([\\w]*)");

        private static DataColumn Add(DataTable output, String name, String description)
        {
            var dc = output.Columns.Add(name);
            dc.ExtendedProperties[templateFieldDataTable.col_desc] = description;
            return dc;
        }

        public DataTable getParameterTable()
        {
            DataTable output = new DataTable("Arguments");

            var col_id = Add(output, "ID", "Parameter ordinal number");
            var col_name = Add(output, "Name", "Parameter name");
            var col_type = Add(output, "Type", "Data type");
            var col_default = Add(output, "Default", "Default value");
            //var col_example = Add(output,"Example", "Default value");
            var col_comment = Add(output, "Comment", "Additional info");

            Int32 c = 1;
            foreach (var cmdPar in this)
            {
                var dr = output.NewRow();

                dr[col_id] = c.ToString("D2");
                dr[col_name] = cmdPar.info.name;
                dr[col_type] = cmdPar.info.type.Name;
                dr[col_default] = cmdPar?.value.toStringSafe("[null]");
                //dr[col_example] = cmdPar.getString(false, true, true);

                dr[col_comment] = cmdPar.info?.sPE?.description.toStringSafe("Example: " + cmdPar.getString(false, true, true));
                if (cmdPar.info.sPE != null)
                {
                    if (cmdPar.info.sPE.acceptableValues.Any())
                    {
                        String __l = "Values: ";

                        __l += cmdPar.info.sPE.acceptableValues.toCsvInLine(); //.ForEach(x => __l + ", " + x.toStringSafe());
                        dr[col_comment] += __l;
                    }
                }

                output.Rows.Add(dr);
                c++;
            }
            return output;
        }

        /// <summary>
        /// Sets from declaration line like: word:String;steps=5:Integer;
        /// </summary>
        /// <param name="declaration">The declaration.</param>
        public void setFromDeclaration(String declaration)
        {
            if (declaration == "param:type;paramb:type;") return;

            var matches = PARAMFORMAT_DoubleDot.Matches(declaration);
            _info = new typedParamInfoCollection();

            foreach (Match mch in matches)
            {
                typedParamInfo tPI = new typedParamInfo(mch.Groups[1].Value, mch.Groups[3].Value);
                info.Add(tPI);
                typedParam tP = new typedParam(tPI, mch.Groups[2].Value);
                Add(tP);
            }
        }

        public void setFromMethodInfo(MethodInfo methodInfo)
        {
            ParameterInfo[] pars = methodInfo.GetParameters();
            _info = new typedParamInfoCollection();

            foreach (ParameterInfo par in pars)
            {
                if (!par.IsRetval)
                {
                    typedParamInfo tPI = new typedParamInfo(par);
                    info.Add(tPI);
                    typedParam tP = new typedParam(tPI, par.DefaultValue.toStringSafe(""));
                    Add(tP);
                }
            }
        }

        public String ToString(Boolean declarationForm = true, Boolean addSeparator = true, Boolean explicitForm = true)
        {
            StringBuilder sb = new StringBuilder();
            foreach (typedParam pr in this)
            {
                sb.Append(pr.getString(declarationForm, addSeparator, explicitForm));
            }
            return sb.ToString();
        }

        public Object[] GetValues()
        {
            List<Object> output = new List<object>();
            foreach (typedParam pr in this)
            {
                output.Add(pr.value);
            }
            return output.ToArray();
        }

        public typedParamCollection()
        {
        }

        public typedParamCollection(typedParamInfoCollection __info)
        {
            _info = __info;
        }

        /// <summary>
        /// Deklaracija parametara na osnovu string opisa
        /// </summary>
        /// <param name="declaration">The declaration.</param>
        public typedParamCollection(String declaration)
        {
            setFromDeclaration(declaration);
        }

        public typedParamCollection(MethodInfo methodInfo)
        {
            setFromMethodInfo(methodInfo);
        }

        public Object this[String paramName]
        {
            get
            {
                typedParam pr = getParam(paramName);
                if (pr != null)
                {
                    return pr.value;
                }
                return null;
            }
        }

        public typedParam getParam(String paramName)
        {
            foreach (var pr in this)
            {
                if (pr.info.name == paramName)
                {
                    return pr;
                }
            }
            return null;
        }

        /// <summary>
        /// Sets the value for param according the name
        /// </summary>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="valueString">The value string.</param>
        public void setValue(String paramName, String valueString)
        {
            var param = getParam(paramName);
            if (param != null)
            {
                param.setValue(valueString);
            }
        }

        /// <summary>
        /// Sets the values - ordinarly
        /// </summary>
        /// <param name="valueStrings">The value strings.</param>
        public String setValues(List<String> valueStrings)
        {
            String errorMessage = "";
            Int32 c = 0;
            foreach (String vl in valueStrings)
            {
                if (c < Count)
                {
                    this[c].setValue(vl);
                    c++;
                }
                else
                {
                    errorMessage = "Parameter [" + c.ToString() + "] not expected. Command requires [" + Count + "] parameters";
                    break;
                }
            }

            return errorMessage;
        }

        public String errorMessage { get; protected set; }

        /// <summary>
        /// Sets the values, returns error message if error occured
        /// </summary>
        /// <param name="paramNames">The parameter names.</param>
        /// <param name="valueStrings">The value strings.</param>
        /// <returns>Empty string if param input passed the validation</returns>
        public String setValues(List<String> paramNames, List<String> valueStrings)
        {
            Int32 c = 0;
            String errorMessage = "";
            Int32 p = paramNames.Count() - valueStrings.Count();
            try
            {
                if (p > 0)
                {
                    for (int i = 0; i < p; i++)
                    {
                        valueStrings.Add(this[valueStrings.Count() + i].value.toStringSafe());
                    }
                }
                else if (paramNames.Count() < valueStrings.Count())
                {
                    valueStrings = valueStrings.Take(paramNames.Count()).ToList();
                }

                foreach (String pn in paramNames)
                {
                    if (valueStrings.Count < c)
                    {
                        break;
                    }
                    setValue(pn, valueStrings[c]);
                    c++;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return errorMessage;
            }
            return "";
        }

        public Boolean addFromString(String input)
        {
            if (Count < info.Count())
            {
                var tpi = info[Count];
                var tp = new typedParam(tpi, input);
                Add(tp);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}