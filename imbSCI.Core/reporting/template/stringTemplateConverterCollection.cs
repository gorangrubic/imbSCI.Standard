// --------------------------------------------------------------------------------------------------------------------
// <copyright file="stringTemplateConverterCollection.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting.template
{
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.reporting.extensions;
    using imbSCI.Data;

    #region imbVeles using

    using System;
    using System.Collections.Generic;
    using System.Data;

    #endregion imbVeles using

    /// <summary>
    /// Collection of simple templates and container opening and closing tags
    /// </summary>
    public class stringTemplateConverterCollection
    {
        public String Convert(Enum templateKey, PropertyCollection values)
        {
            String temp = templates[templateKey].applyToContent(false, values);
            return temp;
        }

        public String Convert(String templateKey, PropertyCollection values)
        {
            if (!templates.ContainsKey(templateKey))
            {
                return "";
            }

            String temp = templates[templateKey].applyToContent(false, values);
            return temp;
        }

        public Boolean HasTemplate(Object templateKey)
        {
            return templates.ContainsKey(templateKey);
        }

        public Boolean HasContainer(Object templateKey)
        {
            return containers.ContainsKey(templateKey);
        }

        /// <summary>
        /// Gets the opening line for the container.
        /// </summary>
        /// <param name="templateKey">The template key.</param>
        /// <param name="valuesForOpenTag">The values for open tag.</param>
        /// <returns></returns>
        public String GetContainerOpen(Object templateKey, PropertyCollection valuesForOpenTag = null)
        {
            if (!containers.ContainsKey(templateKey))
            {
                throw new ArgumentOutOfRangeException(nameof(templateKey), "Template key:" + templateKey + " - key not found");
                return "";
            }
            if (valuesForOpenTag == null) return containers[templateKey].Item1;
            String temp = containers[templateKey].Item1.applyToContent(false, valuesForOpenTag);
            temp = imbSciStringExtensions.ensureEndsWith(temp.ensureStartsWith(Environment.NewLine + Environment.NewLine), Environment.NewLine + Environment.NewLine);
            return temp;
        }

        /// <summary>
        /// Gets the closing line for the container
        /// </summary>
        /// <param name="templateKey">The template key.</param>
        /// <param name="valuesForCloseTag">The values for close tag.</param>
        /// <returns></returns>
        public String GetContainerClose(Object templateKey, PropertyCollection valuesForCloseTag = null)
        {
            if (!containers.ContainsKey(templateKey))
            {
                throw new ArgumentOutOfRangeException(nameof(templateKey), "Template key:" + templateKey + " - key not found");
                return "";
            }
            if (valuesForCloseTag == null) return containers[templateKey].Item2;
            String temp = containers[templateKey].Item2.applyToContent(false, valuesForCloseTag);
            temp = imbSciStringExtensions.ensureEndsWith(temp.ensureStartsWith(Environment.NewLine + Environment.NewLine), Environment.NewLine + Environment.NewLine);
            return temp;
        }

        /// <summary>
        /// Registers container with opening and closing templates
        /// </summary>
        /// <param name="templateKey">The template key.</param>
        /// <param name="openTemplate">The open template.</param>
        /// <param name="closeTemplate">The close template.</param>
        public void RegisterContainer(Object templateKey, String openTemplate, String closeTemplate)
        {
            containers.Add(templateKey, new Tuple<String, String>(openTemplate, closeTemplate));
        }

        /// <summary>
        /// Registers the specified template under given key
        /// </summary>
        /// <param name="templateKey">The template key.</param>
        /// <param name="template">The template.</param>
        public void Register(Object templateKey, String template)
        {
            templates.Add(templateKey, template);
            if (!(templateKey is String))
            {
                templates.Add(templateKey.toStringSafe(), template);
            }
        }

        private Dictionary<Object, Tuple<String, String>> _containers = new Dictionary<Object, Tuple<String, String>>();

        /// <summary> </summary>
        protected Dictionary<Object, Tuple<String, String>> containers
        {
            get
            {
                return _containers;
            }
            set
            {
                _containers = value;
            }
        }

        private Dictionary<Object, String> _templates = new Dictionary<Object, String>();

        /// <summary> </summary>
        protected Dictionary<Object, String> templates
        {
            get
            {
                return _templates;
            }
            set
            {
                _templates = value;
            }
        }
    }
}