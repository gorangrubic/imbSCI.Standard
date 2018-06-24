// --------------------------------------------------------------------------------------------------------------------
// <copyright file="metaNotation.cs" company="imbVeles" >
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
// Project: imbSCI.Reporting
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Reporting.meta.blocks
{
    using imbSCI.Core.reporting.colors;
    using imbSCI.Data.enums.appends;
    using imbSCI.Data.enums.fields;
    using imbSCI.Data.interfaces;
    using imbSCI.Reporting.script;
    using System;
    using System.ComponentModel;
    using System.Data;

    /// <summary>
    /// Notation for referencing
    /// </summary>
    /// \ingroup_disabled docBlocks_common
    public class metaNotation : MetaContainerNestedBase, IObjectWithNameAndDescription
    {
        /// <summary>
        /// Appends its data points into new or existing property collection
        /// </summary>
        /// <param name="data">Property collection to add data into</param>
        /// <returns>Updated or newly created property collection</returns>
        public PropertyCollection AppendDataFields(PropertyCollection data = null)
        {
            if (data == null) data = new PropertyCollection();

            data[templateFieldBasic.meta_author] = author;
            data[templateFieldBasic.meta_copyright] = copyright;
            data[templateFieldBasic.meta_organization] = organization;
            data[templateFieldBasic.meta_softwareName] = softwareName;
            data[templateFieldBasic.meta_year] = DateTime.Now.Year;

            // data[target.target_description] = description;
            // data[target.target_id] = id;
            // data[target.target_url] = url;
            return data;
        }

        public override void construct(object[] resources)
        {
            colors = acePaletteRole.colorA;
            width = blockWidth.full;
        }

        /// <summary>
        /// Composes a set of <c>docScriptInstruction</c> into supplied <c>docScript</c> instance or created blank new instance with <c>name</c> of this metaContainer
        /// </summary>
        /// <param name="script">The script.</param>
        /// <returns></returns>
        public override docScript compose(docScript script)
        {
            if (script == null) script = new docScript(name);
            script.x_scopeIn(this);

            script.add(appendType.s_palette).arg(colors);

            script.add(appendType.c_pair).arg(docScriptArguments.dsa_key, "Author").arg(docScriptArguments.dsa_value, author).arg(docScriptArguments.dsa_separator, ": ");
            script.add(appendType.c_pair).arg(docScriptArguments.dsa_key, "Software").arg(docScriptArguments.dsa_value, softwareName).arg(docScriptArguments.dsa_separator, ": ");
            script.add(appendType.c_pair).arg(docScriptArguments.dsa_key, "Copyright").arg(docScriptArguments.dsa_value, copyright).arg(docScriptArguments.dsa_separator, ": ");
            script.add(appendType.c_pair).arg(docScriptArguments.dsa_key, "Organization").arg(docScriptArguments.dsa_value, organization).arg(docScriptArguments.dsa_separator, ": ");
            script.add(appendType.blockquote).arg(docScriptArguments.dsa_content, description);

            //   script.add(appendType.c_section, docScriptArguments.dsa_name, docScriptArguments.dsa_content, docScriptArguments.dsa_description, docScriptArguments.dsa_class_attribute, docScriptArguments.dsa_id_attribute).set(name, content, description, "header", "#" + name);

            script.add(appendType.s_palette).arg(acePaletteRole.colorDefault);

            script.x_scopeOut(this);
            return script;
        }

        public metaNotation()
        {
        }

        /// <summary>
        ///
        /// </summary>
        public string description { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string url { get; set; }

        #region -----------  softwareName  -------  [Name of software generated report]

        private string _softwareName = "imbVeles::imbSCI.Reporting lib"; // = new String();

        /// <summary>
        /// Name of software generated report
        /// </summary>
        // [XmlIgnore]
        [Category("metaNotation")]
        [DisplayName("softwareName")]
        [Description("Name of software generated report")]
        public string softwareName
        {
            get
            {
                return _softwareName;
            }
            set
            {
                // Boolean chg = (_softwareName != value);
                _softwareName = value;
                OnPropertyChanged("softwareName");
                // if (chg) {}
            }
        }

        #endregion -----------  softwareName  -------  [Name of software generated report]

        #region -----------  copyright  -------  [Copyright information - usually include year, copyright sign and all rights reserved]

        private string _copyright = "2013-2017 © All rights reserved"; // = new String();

        /// <summary>
        /// Copyright information - usually include year, copyright sign and all rights reserved
        /// </summary>
        // [XmlIgnore]
        [Category("metaScienceNotation")]
        [DisplayName("copyright")]
        [Description("Copyright information - usually include year, copyright sign and all rights reserved")]
        public string copyright
        {
            get
            {
                return _copyright;
            }
            set
            {
                // Boolean chg = (_copyright != value);
                _copyright = value;
                OnPropertyChanged("copyright");
                // if (chg) {}
            }
        }

        #endregion -----------  copyright  -------  [Copyright information - usually include year, copyright sign and all rights reserved]

        #region -----------  organization  -------  [Name of organization]

        private string _organization = "KOPLAS PRO doo"; // = new String();

        /// <summary>
        /// Name of organization
        /// </summary>
        // [XmlIgnore]
        [Category("metaScienceNotation")]
        [DisplayName("organization")]
        [Description("Name of organization")]
        public string organization
        {
            get
            {
                return _organization;
            }
            set
            {
                // Boolean chg = (_organization != value);
                _organization = value;
                OnPropertyChanged("organization");
                // if (chg) {}
            }
        }

        #endregion -----------  organization  -------  [Name of organization]

        #region -----------  author  -------  [Name of author]

        private string _author = "Goran Grubic"; // = new String();

        /// <summary>
        /// Name of author
        /// </summary>
        // [XmlIgnore]
        [Category("metaScienceNotation")]
        [DisplayName("author")]
        [Description("Name of author")]
        public string author
        {
            get
            {
                return _author;
            }
            set
            {
                // Boolean chg = (_author != value);
                _author = value;
                OnPropertyChanged("author");
                // if (chg) {}
            }
        }

        #endregion -----------  author  -------  [Name of author]
    }
}