// --------------------------------------------------------------------------------------------------------------------
// <copyright file="metaVariablePairs.cs" company="imbVeles" >
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
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.reporting.colors;
    using imbSCI.Data.enums.appends;
    using imbSCI.Data.enums.fields;
    using imbSCI.Reporting.script;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;

    /// <summary>
    /// Displays table with variable pairs
    /// </summary>
    /// <seealso cref="MetaContainerNestedBase" />
    public class metaVariablePairs : MetaContainerNestedBase
    {
        /// <summary>
        /// Performs construction (or upgrade) of DOM according to cpecific data set supplied.
        /// </summary>
        /// <param name="resources"></param>
        /// <remarks>
        /// <para>This method is meant to be called just after constructor and before <c>compose</c> or other application method. </para>
        /// <para>It is not automatically called by constructor for easier prerequirements handling. </para>
        /// <para>Inside the method it is safe to access <c>parent</c>, <c>page</c>, <c>document</c> or any other automatic property.</para>
        /// <para>This method is meant to be called just once: it should remove any existing dynamically created nodes at beginning of execution - in purpose that any subsequent call produces the same result</para>
        /// </remarks>
        public override void construct(object[] resources)
        {
            List<object> reslist = resources.getFlatList<object>();

            colors = acePaletteRole.colorA;
            width = blockWidth.full;

            PropertyCollection dpair = reslist.getFirstOfType<PropertyCollection>(false, null);
            variables = dpair;

            description = dpair.getAndRemoveProperString(templateFieldBasic.meta_desc, templateFieldDataTable.data_tabledesc, templateFieldMetaBlock.block_desc, docScriptArguments.dsa_description, docScriptArguments.dsa_footer);
            name = dpair.getAndRemoveProperString(templateFieldBasic.meta_softwareName, templateFieldDataTable.data_tablename, templateFieldMetaBlock.block_name, docScriptArguments.dsa_title, docScriptArguments.dsa_name);

            var palRole = dpair.getAndRemoveProperField(appendType.s_palette, docScriptArguments.dsa_paletteRole);
            if (palRole is acePaletteRole)
            {
                colors = (acePaletteRole)palRole;
            }
            else
            {
                colors = acePaletteRole.colorB;
            }

            doHorizontal = (bool)dpair.getAndRemoveProperField(docScriptArguments.dsa_isHorizontal);
            //var wdth = dpair.getAndRemoveProperField(docScriptArguments.dsa_);
        }

        public override docScript compose(docScript script)
        {
            if (script == null) script = new docScript(name);
            script.x_scopeIn(this);

            script.add(appendType.s_palette).arg(colors);

            script.add(appendType.heading_2, name);

            script.add(appendType.c_data).arg(docScriptArguments.dsa_dataPairs, variables).arg(docScriptArguments.dsa_separator, "").hr().arg(docScriptArguments.dsa_title, name).arg(docScriptArguments.dsa_description, description).arg(docScriptArguments.dsa_id_attribute, "pair_" + name);

            script.add(appendType.italic, description);
            //;

            //script.add(appendType.c_section, docScriptArguments.dsa_name, docScriptArguments.dsa_content, docScriptArguments.dsa_description, docScriptArguments.dsa_class_attribute, docScriptArguments.dsa_id_attribute).set(name, content, description, "header", "#" + name);

            script.add(appendType.s_palette).arg(acePaletteRole.colorDefault);

            script.x_scopeOut(this);

            return script;
        }

        #region -----------  variables  -------  [Variables to show]

        private PropertyCollection _variables; // = new PropertyCollection();

        /// <summary>
        /// Variables to show
        /// </summary>
        // [XmlIgnore]
        [Category("metaVariablePairs")]
        [DisplayName("variables")]
        [Description("Variables to show")]
        public PropertyCollection variables
        {
            get
            {
                return _variables;
            }
            set
            {
                // Boolean chg = (_variables != value);
                _variables = value;
                OnPropertyChanged("variables");
                // if (chg) {}
            }
        }

        #endregion -----------  variables  -------  [Variables to show]

        #region -----------  description  -------  [Description of variable pairs]

        private string _description; // = new String();

        /// <summary>
        /// Description of variable pairs
        /// </summary>
        // [XmlIgnore]
        [Category("metaVariablePairs")]
        [DisplayName("description")]
        [Description("Description of variable pairs")]
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

        #endregion -----------  description  -------  [Description of variable pairs]

        #region ----------- Boolean [ doHorizontal ] -------  [Render variables into horizontal table]

        private bool _doHorizontal = false;

        /// <summary>
        /// Render variables into horizontal table
        /// </summary>
        [Category("Switches")]
        [DisplayName("doHorizontal")]
        [Description("Render variables into horizontal table")]
        public bool doHorizontal
        {
            get { return _doHorizontal; }
            set { _doHorizontal = value; OnPropertyChanged("doHorizontal"); }
        }

        #endregion ----------- Boolean [ doHorizontal ] -------  [Render variables into horizontal table]

        public metaVariablePairs()
        {
            //name = __name;
            //description = __description;
            //variables = __variables;
            //doHorizontal = horizontal;
        }
    }
}