﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="stringBuilderControler.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting.render.contentControl
{
    using imbSCI.Core.extensions.data;
    using imbSCI.Data.data;
    using imbSCI.Data.enums.fields;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Main and subcontent controller - a helper class for <c>ITextRender</c>, <c>IDocumentRender</c>, <c>IRender</c>
    /// </summary>
    /// <seealso cref="imbReportingBindable" />
    public class stringBuilderControler : imbBindable
    {
        public stringBuilderControler()
        {
            switchToActive(templateFieldSubcontent.main);
        }

        ///// <summary>
        ///// Compiles subcontents with the main content or with the specified template.
        ///// </summary>
        ///// <param name="template">The template string. If empty it will use the <c>main</c> content as template</param>
        ///// <param name="log">Optional logger</param>
        ///// <param name="secondary">The secondary dataset - if specified it will run secondary <c>applyToContent</c> call against compiled content. Useful if <c>subcontent</c> contains template placeholders unmatched during prior <c>compile</c> call</param>
        ///// <returns>Compiled content where placeholders are replaced with subcontents and optionally with <c>secondary</c> dataset</returns>
        //public String compile(String template = "", ILogable log = null, PropertyCollection secondary = null)
        //{
        //    String output = "";
        //    Boolean useMain = false;
        //    if (template.isNullOrEmpty())
        //    {
        //        template = this[templateFieldSubcontent.main].ToString();
        //    } else
        //    {
        //        useMain = true;
        //    }

        //    PropertyCollection dataset = getDataset(useMain);

        //    if (dataset.Count == 0)
        //    {
        //        return template;
        //    }

        //    template = template.applyToContent(false, dataset);

        //    if (secondary != null)
        //    {
        //        template = template.applyToContent(false, secondary);
        //    }

        //    return template;
        //}

        /// <summary>
        /// Retrieves all subcontents and optionaly the main subcontent - as PropertyCollection
        /// </summary>
        /// <returns>Key: <see cref="imbSCI.Data.enums.fields.templateFieldSubcontent"/> / Value: <see cref="StringBuilder"/> </returns>
        public PropertyCollection getDataset(Boolean includeMain = false)
        {
            PropertyCollection output = new PropertyCollection();

            foreach (KeyValuePair<String, StringBuilder> de in registry)
            {
                if (includeMain)
                {
                    output.Add(de.Key, de.Value.ToString());

                }
                else
                {
                    if (de.Key != nameof(templateFieldSubcontent.main))
                    {
                        output.Add(de.Key, de.Value.ToString());
                    }
                }
            }

            return output;
        }


        protected List<String> history { get; set; } = new List<string>();


        public void switchToActive(templateFieldSubcontent subfield)
        {
            String k = subfield.ToString();

            if (!registry.ContainsKey(k))
            {
                registry.Add(k, new StringBuilder());
            }
            active = k;
        }

        public void switchToActive(String subfield)
        {
            if (subfield.isNullOrEmpty()) subfield = nameof(templateFieldSubcontent.main);

            if (!registry.ContainsKey(subfield))
            {
                registry.Add(subfield, new StringBuilder());
            }
            active = subfield;
        }

        private String _active = nameof(templateFieldSubcontent.main);

        /// <summary>
        /// Determinates the active StringBuilder
        /// </summary>
        protected String active
        {
            get { return _active; }
            set
            {
                if (_active != value)
                {
                    if (history.Contains(_active))
                    {
                        Int32 _activeIndex = history.IndexOf(_active);
                        if (_activeIndex > 0)
                        {
                            history = history.Take(_activeIndex).ToList();
                        }

                    }
                    else
                    {
                        history.Add(_active);
                    }

                }
                _active = value;
            }
        }

        /// <summary>
        /// Switched to previous subcontent or to the main content if there were no subcontent keys in history
        /// </summary>
        public void switchToBack()
        {
            String lastKey = "";
            if (history.Any())
            {
                lastKey = history.Last();
            }
            if (lastKey.isNullOrEmpty())
            {
                lastKey = nameof(templateFieldSubcontent.main);
            }


            switchToActive(lastKey);
        }

        /// <summary>
        /// Gets the active StringBuilder - equeal to <c>this[active]</c>
        /// </summary>
        /// <value>
        /// Equeal to <c>this[active]</c>
        /// </value>
        public StringBuilder sb
        {
            get
            {
                return registry[active];
            }
        }

        public void Clear()
        {
            registry.Clear();
            history.Clear();
            switchToActive(templateFieldSubcontent.main);
        }

        /// <summary>
        /// Gets the <see cref="System.Text.StringBuilder" /> under the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="System.Text.StringBuilder" />.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        internal StringBuilder this[templateFieldSubcontent key]
        {
            get
            {
                String k = key.ToString();
                if (!registry.ContainsKey(k))
                {
                    registry.Add(k, new StringBuilder());
                }
                return registry[k] as StringBuilder;
            }
        }

        private Dictionary<String, StringBuilder> _registry = new Dictionary<String, StringBuilder>();

        /// <summary>
        /// Container for string builders
        /// </summary>
        protected Dictionary<String, StringBuilder> registry
        {
            get { return _registry; }
            set { _registry = value; }
        }
    }
}