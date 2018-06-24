// --------------------------------------------------------------------------------------------------------------------
// <copyright file="metaSimpleAppend.cs" company="imbVeles" >
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
    using imbSCI.Reporting.script;
    using System.ComponentModel;
    using System.Linq;

    public class metaSimpleAppend : MetaContainerNestedBase
    {
        //public override IMetaContentNested SearchForChild(string needle)
        //{
        //    needle = CleanNeedle(needle);
        //    if (this.name == needle) return this;
        //    return null;
        //}

        public override void construct(object[] resources)
        {
            colors = acePaletteRole.colorA;
            width = blockWidth.full;
        }

        public override docScript compose(docScript script)
        {
            if (script == null) script = new docScript(name);
            script.x_scopeIn(this);

            script.add(appendType.s_palette).arg(colors);

            if (content.Any())
            {
                //script.add(appendType.s_variation);
                script.add(type, name).arg(acePaletteVariationRole.heading);
                script.add(appendType.c_data).arg(docScriptArguments.dsa_content, content);
            }
            else
            {
                script.add(type, name);
            }

            script.add(appendType.s_palette).arg(acePaletteRole.colorDefault);

            script.x_scopeOut(this);
            return script;
        }

        #region -----------  type  -------  [Append Type to use for appending each line of this content]

        private appendType _type = appendType.paragraph; // = new appendType();

        /// <summary>
        /// Append Type to use for appending each line of this content
        /// </summary>
        // [XmlIgnore]
        [Category("metaSimpleAppend")]
        [DisplayName("type")]
        [Description("Append Type to use for appending each line of this content")]
        public appendType type
        {
            get
            {
                return _type;
            }
            set
            {
                // Boolean chg = (_type != value);
                _type = value;
                OnPropertyChanged("type");
                // if (chg) {}
            }
        }

        #endregion -----------  type  -------  [Append Type to use for appending each line of this content]
    }
}