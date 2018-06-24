// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceAuthorNotation.cs" company="imbVeles" >
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
using imbSCI.Core.collection;
using imbSCI.Core.extensions.data;
using imbSCI.Core.reporting;
using imbSCI.Data;
using imbSCI.Data.data;
using System;

namespace imbSCI.Core.data
{
    using imbSCI.Core.reporting.render;
    using imbSCI.Core.reporting.render.builders;
    using imbSCI.Data.enums.fields;
    using System.ComponentModel;
    using System.Data;
    using System.Reflection;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Data structure with basic information on the author, application, copyright.. etc. Used for reports, log files signatures etc.
    /// </summary>
    /// <seealso cref="imbBindable" />
    public class aceAuthorNotation : imbBindable, IAppendDataFields, IAppendDataFieldsExtended
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="aceAuthorNotation"/>, setting default data by reading <see cref="Assembly.GetCallingAssembly()"/> attributes
        /// </summary>
        public aceAuthorNotation(Assembly assembly = null)
        {
            if (assembly == null) assembly = Assembly.GetCallingAssembly();
            var attributes = assembly.GetCustomAttributes(true);

            foreach (var att in attributes)
            {
                if (att is AssemblyTitleAttribute attTitle)
                {
                    software = attTitle.Title;
                }
                else if (att is AssemblyCompanyAttribute attCompany)
                {
                    organization = attCompany.Company;
                }
                else if (att is AssemblyCopyrightAttribute attCopyright)
                {
                    copyright = attCopyright.Copyright;
                }
                else if (att is AssemblyVersionAttribute attFileversion)
                {
                    version = attFileversion.Version;
                }
                else if (att is AssemblyProductAttribute attProduct)
                {
                    software = attProduct.Product;
                }
                else if (att is AssemblyDescriptionAttribute attDescription)
                {
                    comment = attDescription.Description;
                }
            }
        }

        /// <summary>
        /// Appends its data points into new or existing property collection
        /// </summary>
        /// <param name="data">Property collection to add data into</param>
        /// <returns>Updated or newly created property collection</returns>
        public virtual PropertyCollectionExtended AppendDataFields(PropertyCollectionExtended data = null)
        {
            if (data == null) data = new PropertyCollectionExtended();

            data.Add(templateFieldBasic.meta_author, author, "Author", "");
            data.Add(templateFieldBasic.meta_copyright, copyright, "Copyright", "Copyright statement");
            data.Add(templateFieldBasic.meta_organization, organization, "Organisation", "");
            data.Add(templateFieldBasic.meta_softwareName, software, "Executable", "Name of executable ran");
            data.Add(templateFieldBasic.meta_year, DateTime.Now.Year, "Year", "");
            //data.Add(templateFieldBasic.meta_keywords, "")
            data.Add(templateFieldBasic.meta_softwareComment, comment, "Software info", "");
            return data;
        }

        /// <summary>
        /// Gets the description lines for data defined in this instance
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public String GetDescription(ITextRender builder = null)
        {
            if (builder == null) builder = new builderForMarkdown();

            var l = builder.Length;
            var notation = this;

            if (!notation.author.isNullOrEmpty()) builder.AppendPair("Author", notation.author, true, ": ");
            if (!notation.Email.isNullOrEmpty()) builder.AppendPair("E-mail", notation.Email, true, ": ");
            if (!notation.web.isNullOrEmpty()) builder.AppendPair("Web", notation.web, true, ": ");
            if (!notation.copyright.isNullOrEmpty()) builder.AppendPair("Copyright", notation.copyright, true, ": ");
            if (!notation.license.isNullOrEmpty()) builder.AppendPair("License", notation.license, true, ": ");
            if (!notation.software.isNullOrEmpty()) builder.AppendPair("Software", notation.software, true, ": ");
            if (!notation.organization.isNullOrEmpty()) builder.AppendPair("Organization", notation.organization, true, ": ");
            if (!notation.comment.isNullOrEmpty()) builder.AppendPair("Comment", notation.comment, true, ": ");

            return builder.GetContent(l);
        }

        /// <summary>
        /// Appends its data points into new or existing property collection
        /// </summary>
        /// <param name="data">Property collection to add data into</param>
        /// <returns>
        /// Updated or newly created property collection
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        PropertyCollection IAppendDataFields.AppendDataFields(PropertyCollection data)
        {
            return AppendDataFields(data as PropertyCollectionExtended);
        }

        private String _software = "imbVeles - application";

        /// <summary>
        /// Name of the software application
        /// </summary>
        public String software
        {
            get { return _software; }
            set { _software = value; }
        }

        private String _author = "Goran Grubić";

        /// <summary>
        /// Author of the software
        /// </summary>
        public String author
        {
            get { return _author; }
            set { _author = value; }
        }

        /// <summary>
        /// License information
        /// </summary>
        /// <value>
        /// The licence.
        /// </value>
        public String license { get; set; } = "GNU General Public License v3.0";

        private String _copyright = "All Rights reserved © 2013-2018.";

        /// <summary>
        /// Copyright notice text
        /// </summary>
        public String copyright
        {
            get { return _copyright; }
            set { _copyright = value; }
        }

        private String _comment = "Part of PhD thesis research.";

        /// <summary>
        /// Comment to be placed in footer/meta info of the reports
        /// </summary>
        public String comment
        {
            get { return _comment; }
            set { _comment = value; }
        }

        private String _organization = "imbVeles";

        /// <summary>
        /// Organization behind the software
        /// </summary>
        public String organization
        {
            get { return _organization; }
            set { _organization = value; }
        }

        public String version { get; set; } = "1.0";

        /// <summary> Web site of the project, software, organization or author </summary>
        [Category("Label")]
        [DisplayName("Web")] //[imb(imbAttributeName.measure_letter, "")]
        [Description("Web site of the project, software, organization or author")] // [imb(imbAttributeName.reporting_escapeoff)]
        public String web { get; set; } = "http://blog.veles.rs";

        /// <summary> E-mail address of the project, author, organization... </summary>
        [Category("Label")]
        [DisplayName("E-mail")] //[imb(imbAttributeName.measure_letter, "")]
        [Description("E-mail address of the project, author, organization...")] // [imb(imbAttributeName.reporting_escapeoff)]
        public String Email { get; set; } = "hardy@veles.rs";
    }
}