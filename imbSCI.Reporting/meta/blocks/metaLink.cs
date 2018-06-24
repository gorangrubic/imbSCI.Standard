// --------------------------------------------------------------------------------------------------------------------
// <copyright file="metaLink.cs" company="imbVeles" >
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
    using imbSCI.Core.interfaces;
    using imbSCI.Data.enums;
    using imbSCI.Data.enums.appends;
    using imbSCI.Reporting.script;
    using System.ComponentModel;

    /// <summary>
    /// Meta representation of link
    /// </summary>
    /// docBlocks_elementary
    public class metaLink : MetaContainerNestedBase, IMetaContentNested
    {
        public override void construct(object[] resources)
        {
            // colors = imbSCI.Cores.colors.acePaletteRole.colorA;
            width = blockWidth.full;
        }

        public override docScript compose(docScript script)
        {
            if (script == null) script = new docScript(name);
            //script.x_scopeIn(this);

            //            script.add(appendType.s_palette).arg(colors);

            script.c_link(name, url, title, description, type);

            //     script.add(appendType.c_section, docScriptArguments.dsa_name, docScriptArguments.dsa_content, docScriptArguments.dsa_description, docScriptArguments.dsa_class_attribute, docScriptArguments.dsa_id_attribute).set(name, content, description, "header", "#" + name);

            //          script.add(appendType.s_palette).arg(acePaletteRole.colorDefault);

            //script.x_scopeOut(this);
            return script;
        }

        /// <summary>
        ///
        /// </summary>
        public string description { get; set; } = "";

        #region -----------  tool  -------  [Associated tool to open with]

        private externalTool _tool = externalTool.autodetect; // = new externalTool();

        /// <summary>
        /// Associated tool to open with
        /// </summary>
        // [XmlIgnore]
        [Category("metaLink")]
        [DisplayName("tool")]
        [Description("Associated tool to open with")]
        public externalTool tool
        {
            get
            {
                return _tool;
            }
            set
            {
                // Boolean chg = (_tool != value);
                _tool = value;
                OnPropertyChanged("tool");
                // if (chg) {}
            }
        }

        #endregion -----------  tool  -------  [Associated tool to open with]

        #region --- display ------- Display option

        private displayOption _display = displayOption.primary;

        /// <summary>
        /// Display option
        /// </summary>
        public displayOption display
        {
            get
            {
                return _display;
            }
            set
            {
                _display = value;
                OnPropertyChanged("display");
            }
        }

        #endregion --- display ------- Display option

        #region -----------  url  -------  [Url to open]

        private string _url = ""; // = new String();

        /// <summary>
        /// Url to open - may be null if link is not external
        /// </summary>
        // [XmlIgnore]
        [Category("metaLink")]
        [DisplayName("url")]
        [Description("Url to open")]
        public string url
        {
            get
            {
                return _url;
            }
            set
            {
                // Boolean chg = (_url != value);
                _url = value;
                OnPropertyChanged("url");
                // if (chg) {}
            }
        }

        #endregion -----------  url  -------  [Url to open]

        #region -----------  title  -------  [prefered display title]

        private string _title = ""; // = new String();

        /// <summary>
        /// prefered display title
        /// </summary>
        // [XmlIgnore]
        [Category("MetaContentNestedWithNameBase")]
        [DisplayName("title")]
        [Description("prefered display title")]
        public string title
        {
            get
            {
                return _title;
            }
            set
            {
                // Boolean chg = (_title != value);
                _title = value;
                OnPropertyChanged("title");
                // if (chg) {}
            }
        }

        #endregion -----------  title  -------  [prefered display title]

        #region --- target ------- Link target

        private IMetaContentNested _target;

        /// <summary>
        /// Target on what this link points - may be null if link is not internal
        /// </summary>
        public IMetaContentNested target
        {
            get
            {
                return _target;
            }
            set
            {
                _target = value;
                OnPropertyChanged("target");
            }
        }

        #endregion --- target ------- Link target

        private appendLinkType _type = appendLinkType.link;

        /// <summary>
        /// Link type
        /// </summary>
        public appendLinkType type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
                OnPropertyChanged("type");
            }
        }
    }
}