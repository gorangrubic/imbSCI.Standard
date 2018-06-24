// --------------------------------------------------------------------------------------------------------------------
// <copyright file="metaCodeBlock.cs" company="imbVeles" >
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
    using imbSCI.Reporting.script;
    using System.Collections.Generic;
    using System.ComponentModel;

    /// <summary>
    /// wrapper koji prikazuje content
    /// </summary>
    /// \ingroup_disabled docBlocks_common
    public class metaCodeBlock : MetaContainerNestedBase
    {
        public override docScript compose(docScript script)
        {
            if (script == null) script = new docScript(name);
            script.x_scopeIn(this);

            //script.open("code", title, desc);

            //script.add(appendType.s_palette).arg(colors);

            script.code(title, description, content, codetypename);

            //  script.add(appendType.c_section, docScriptArguments.dsa_name, docScriptArguments.dsa_content, docScriptArguments.dsa_description, docScriptArguments.dsa_class_attribute, docScriptArguments.dsa_id_attribute).set(name, content, description, "codeblock", "#"+name);

            // script.add(appendType.s_palette).arg(acePaletteRole.colorDefault);

            script.x_scopeOut(this);
            return script;
        }

        public override void construct(object[] resources)
        {
            colors = acePaletteRole.colorC;
            width = blockWidth.full;
        }

        /// <summary>
        ///
        /// </summary>
        public string codetypename { get; set; } = "html";

        private string _title = "";

        /// <summary> </summary>
        public string title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                OnPropertyChanged("title");
            }
        }

        #region -----------  description  -------  [Content for footer]

        private string _description = ""; // = new String();

        /// <summary>
        /// Content for footer
        /// </summary>
        // [XmlIgnore]
        [Category("metaCodeBlock")]
        [DisplayName("description")]
        [Description("Content for footer")]
        public string description
        {
            get
            {
                return _description;
            }
            set
            {
                // Boolean chg = (_description != value);
                _description = value;
                OnPropertyChanged("description");
                // if (chg) {}
            }
        }

        #endregion -----------  description  -------  [Content for footer]

        /// <summary>
        /// Code block -- containes custom code that may have template params
        /// </summary>
        /// <param name="__name">What whould be writen at head of the block? leave empty to hide head</param>
        /// <param name="__description">What would be written as footer of the block? leave empty to hide footer </param>
        /// <param name="__url">What link to put at bottom of footer? no link if empty</param>
        /// <param name="__code"></param>
        public metaCodeBlock(string __name, string __description, IEnumerable<string> __code)
        {
            content.AddRange(__code);
            name = __name;
            title = __name;
            description = __description;
        }
    }
}