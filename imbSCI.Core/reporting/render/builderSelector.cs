// --------------------------------------------------------------------------------------------------------------------
// <copyright file="builderSelector.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting.render
{
    using imbSCI.Core.reporting.format;
    using imbSCI.Core.reporting.render.config;
    using imbSCI.Data.data;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

#pragma warning disable CS1574 // XML comment has cref attribute 'imbBindable' that could not be resolved
    /// <summary>
    /// Builder selector by target format
    /// </summary>
    /// <seealso cref="aceCommonTypes.primitives.imbBindable" />
    public class builderSelector : imbBindable, IEnumerable<KeyValuePair<reportOutputFormatName, ITextRender>>
#pragma warning restore CS1574 // XML comment has cref attribute 'imbBindable' that could not be resolved
    {
        private Dictionary<reportOutputFormatName, ITextRender> registry = new Dictionary<reportOutputFormatName, ITextRender>();

        /// <summary>
        /// Initializes a new instance of the <see cref="builderSelector"/> class.
        /// </summary>
        /// <param name="api">The API.</param>
        /// <param name="formPreset">The form preset.</param>
        public builderSelector(reportAPI api, elementLevelFormPreset formPreset = elementLevelFormPreset.none)
        {
            ITextRender rnd = reportOutputSupport.getRenderFor(api);
            Add(rnd);
            format = registry.Keys.First();
            setActiveElementLevelFormPreset(formPreset);
        }

        public builderSelector()
        {
            //    reportOutputSupport.getRenderFor()
            //ITextRender rnd = reportOutputSupport.getRenderFor(api);
            //Add(rnd);
            //format = registry.Keys.First();
            //setActiveElementLevelFormPreset(formPreset);
        }

        #region --- settings ------- Builder settings

        private builderSettings _settings = new builderSettings();

        /// <summary>
        /// Builder settings
        /// </summary>
        public builderSettings settings
        {
            get
            {
                if (!registry.ContainsKey(format))
                {
                    throw new NotSupportedException("Settings for " + format.ToString() + " are not accessable because builder does not contain renderer for this format");
                    return null;
                }
                return registry[format].settings;
                //return _settings;
            }
            //set
            //{
            //    _settings = value;
            //    OnPropertyChanged("settings");
            //}
        }

        #endregion --- settings ------- Builder settings

        //private builderSessionManager _sessionManager = new builderSessionManager();
        ///// <summary>
        ///// Gets or sets the session manager.
        ///// </summary>
        ///// <value>
        ///// The session manager.
        ///// </value>
        //public builderSessionManager sessionManager
        //{
        //    get { return _sessionManager ; }
        //    set { _sessionManager  = value; }
        //}

        /// <summary>
        /// Automatically sets render and formats
        /// </summary>
        /// <param name="api">The API.</param>
        public void Add(reportAPI api)
        {
            ITextRender rnd = reportOutputSupport.getRenderFor(api);
            Add(rnd);
            //   rnd.settings
        }

        /// <summary>
        /// Adds new builder into selector. If it supports a same format than the one added earlier - the most recently added will manage the format
        /// </summary>
        /// <param name="newBuilder">The new builder.</param>
        public void Add(ITextRender newBuilder)
        {
            foreach (reportOutputFormatName fn in newBuilder.settings.formats.supported)
            {
                if (registry.ContainsKey(fn))
                {
                    registry[fn] = newBuilder;
                }
                else
                {
                    registry.Add(fn, newBuilder);
                }
            }
        }

        /// <summary>
        /// Adds the specified new builder to particular output format
        /// </summary>
        /// <param name="newBuilder">The new builder.</param>
        /// <param name="fn">The function.</param>
        public void Add(ITextRender newBuilder, reportOutputFormatName fn)
        {
            if (registry.ContainsKey(fn))
            {
                registry[fn] = newBuilder;
            }
            else
            {
                registry.Add(fn, newBuilder);
            }
        }

        /// <summary>
        /// Sets the active <see cref="elementLevelFormPreset"/>
        /// </summary>
        /// <param name="preset">The preset.</param>
        public void setActiveElementLevelFormPreset(elementLevelFormPreset preset)
        {
            if (preset != elementLevelFormPreset.none)
            {
                registry[format].settings.forms = new reportOutputFormat(preset);
            }
        }

        /// <summary>
        /// Sets the active output format.
        /// </summary>
        /// <param name="__format">The format.</param>
        /// <param name="formPreset">The form preset.</param>
        /// <exception cref="Exception">Builder instance has no IRender-er for format:" + __format.ToString() + ". call builder.Add() with proper builder before setting this format as active output format.</exception>
        public void setActiveOutputFormat(reportOutputFormatName __format, elementLevelFormPreset formPreset = elementLevelFormPreset.none)
        {
            if (registry.ContainsKey(__format))
            {
                format = __format;

                setActiveElementLevelFormPreset(formPreset);
            }
            else
            {
                throw new NotSupportedException("Builder instance has no IRender-er for format:" + __format.ToString() + ". call builder.Add() with proper builder before setting this format as active output format.");
            }
        }

        private reportOutputFormatName _format;

        /// <summary>
        /// Active format used by i.e. <see cref="ITextRender.saveDocument(System.IO.FileInfo)"/>
        /// </summary>
        public reportOutputFormatName format
        {
            get
            {
                return _format;
            }
            protected set
            {
                _format = value;
            }
        }

        /// <summary>
        /// Clears all builders.
        /// </summary>
        public void clearAllBuilders()
        {
            foreach (var pair in registry)
            {
                pair.Value.Clear();
            }
        }

        public IEnumerator<KeyValuePair<reportOutputFormatName, ITextRender>> GetEnumerator() => registry.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => registry.GetEnumerator();

        /// <summary>
        /// Gets the document builder for the active <c>format</c>
        /// </summary>
        /// <value>
        /// The text builder.
        /// </value>
        public ITextRender documentBuilder
        {
            get
            {
                if (registry.ContainsKey(format))
                {
                    ITextRender textRender = registry[format];

                    return textRender; //registry[format] as IDocumentRender;
                }

                return null;
            }
        }
    }
}