// --------------------------------------------------------------------------------------------------------------------
// <copyright file="metaDataTable.cs" company="imbVeles" >
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
    using imbSCI.Core.extensions.text;
    using imbSCI.Reporting.meta.data;
    using imbSCI.Reporting.script;
    using System.Data;

    /// <summary>
    /// Content block with DataTable
    /// </summary>
    /// \ingroup_disabled docBlocks_common
    public class metaDataTable : MetaContainerNestedBase
    {
        /// <summary>
        ///
        /// </summary>
        public dataProviderForStyler stylerSettings { get; set; }

        /// <summary>
        /// Expects: theme,
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
            // List<Object> reslist = resources.getFlatList<Object>();

            // colors = imbSCI.Cores.colors.acePaletteRole.colorA;
            // width = blockWidth.full;

            //DataTable dt = reslist.getFirstOfType<DataTable>(false, null);
            //table = dt;
            //description = table.ExtendedProperties.getProperString(description, templateFieldDataTable.data_tabledesc);
            //name = table.ExtendedProperties.getProperString(name, templateFieldDataTable.data_tablename);

            /// ------------------------------- Constructing styler settings ------------

            //cursorVariatorHeadFootFlags headFoot = cursorVariatorHeadFootFlags.doHeadZone | cursorVariatorHeadFootFlags.doFootZone | cursorVariatorHeadFootFlags.doFootExtendedZone
            //| cursorVariatorHeadFootFlags.doHeadExtenedZone | cursorVariatorHeadFootFlags.addTableNameHeader | cursorVariatorHeadFootFlags.addTableDescFooter
            //| cursorVariatorHeadFootFlags.addRowNumberOnMajor | cursorVariatorHeadFootFlags.addRowNumberOnMinor;

            //cursorVariatorOddEvenFlags oddEven = cursorVariatorOddEvenFlags.doOddEven | cursorVariatorOddEvenFlags.doMinorOn5 | cursorVariatorOddEvenFlags.doMajorOn2Minor;

            //appendTableOptionFlags tableOps = appendTableOptionFlags.footMearged | appendTableOptionFlags.topHeadMerged | appendTableOptionFlags.topHeadFullWidth
            //| appendTableOptionFlags.footFullWidth | appendTableOptionFlags.footAlignmentCenter | appendTableOptionFlags.topHeadAlignmentCenter;

            //styleFourSide container = new styleFourSide();

            //stylerSettings = new dataProviderForStyler(headFoot, tableOps, oddEven, container, acePaletteRole.colorDefault, colors);
        }

        public override docScript compose(docScript script)
        {
            if (script == null) script = new docScript(name);
            script.x_scopeIn(this);

            script.c_table(table, title, description);

            //  script.add(appendType.c_table).arg(d.dsa_dataTable, table).arg(d.dsa_title, table.TableName).arg(d.dsa_description, description);

            script.x_scopeOut(this);

            return script;
        }

        /// <summary>
        ///
        /// </summary>
        public string title { get; set; } = "";

        public metaDataTable(DataTable __table)
        {
            table = __table;

            name = table.TableName.imbTitleCamelOperation(false, true);

            description = "Content from database table " + table.TableName + " ";
        }

        public metaDataTable()
        {
        }

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

        #region --- table ------- data table to show

        private DataTable _table;
        private string _description;

        /// <summary>
        /// data table to show
        /// </summary>
        public DataTable table
        {
            get
            {
                return _table;
            }
            set
            {
                _table = value;
                OnPropertyChanged("table");
            }
        }

        #endregion --- table ------- data table to show
    }
}