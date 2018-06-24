// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataTableExtended.cs" company="imbVeles" >
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
// Project: imbSCI.DataComplex
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
using System;

namespace imbSCI.DataComplex.tables
{
    using imbSCI.Core.data;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.extensions.table;
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Data;
    using imbSCI.DataComplex.extensions.data.schema;
    using System.Data;

    /// <summary>
    /// Simple untyped object datatable implementation
    /// </summary>
    /// <seealso cref="System.Data.DataTable" />
    public class DataTableExtended : DataTable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataTableExtended"/> class.
        /// </summary>
        public DataTableExtended()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTableExtended"/> class.
        /// </summary>
        /// <param name="tablename">The tablename.</param>
        public DataTableExtended(String tablename) : base(tablename)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTableExtended"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="primaryKeyName">Name of the primary key - optionally.</param>
        /// <param name="title">The title.</param>
        /// <param name="description">The description.</param>
        public DataTableExtended(Type type, string title, String description = "") : base(title.getCleanPropertyName())
        {
            var settings = new settingsEntriesForObject(type, false);

            if (description.isNullOrEmpty())
            {
                description = settings.Description;
            }

            DataTableExtended table = this;
            table.settings = settings;

            table.SetClassType(type);
            table.SetClassName(type.Name);
            table.SetDescription(description);
            table.SetTitle(title);

            table.SetCategoryPriority(settings.CategoryByPriority);

            //table.PrimaryKey = new DataColumn[] { };

            foreach (settingsPropertyEntry spe in settings.spes.Values)
            {
                if (!spe.IsXmlIgnore)
                {
                    var column = table.Add(spe);
                }
            }
        }

        protected settingsEntriesForObject settings { get; set; }
        //  protected DataColumn primaryKey { get; set; }

        /// <summary>
        /// Adds the object as row in the table
        /// </summary>
        /// <param name="input">The input.</param>
        protected void addRow(Object input)
        {
            if (input == null) return;
            String key = "";

            //if (primaryKey != null)
            //{
            //    key = input.imbGetPropertySafe(primaryKey.ColumnName, primaryKey.DataType.GetDefaultValue()).toStringSafe();
            //    String ktry = key;
            //    Boolean repeat = true;
            //    Int32 c = 0;
            //    while (repeat)
            //    {
            //        c++;

            //        String exp = primaryKey.ColumnName + " = '" + ktry + "'";
            //       Int32 i = DefaultView.Find(ktry);

            //        if (i > 0)
            //        {
            //            Console.WriteLine("Key " + ktry + " found at [" + i + "]");
            //            repeat = true;
            //            ktry = key + c.ToString("D5");
            //        } else
            //        {
            //            Console.WriteLine("Key " + ktry + " accepted after [" + c + "]");
            //            repeat = false;
            //            input.imbSetPropertySafe(primaryKey.ColumnName, ktry);
            //        }

            //    }
            //}

            var row = this.NewRow();
            var addTheRow = true;

            foreach (settingsPropertyEntry spe in settings.spes.Values)
            {
                if (!spe.IsXmlIgnore)
                {
                    //if (spe.isPrimaryKey)
                    //{
                    //    string primKey = row[primaryKey].toStringSafe("");

                    //}

                    row[(string)spe.pi.Name] = input.imbGetPropertySafe(spe.pi, spe.pi.PropertyType.GetDefaultValue());
                }
            }

            if (addTheRow)
            {
                Rows.Add(row);
            }
        }
    }
}