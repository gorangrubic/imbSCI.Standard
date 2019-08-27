using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.table.style;
using imbSCI.Data.enums.fields;
using imbSCI.DataComplex.extensions.data.modify;
using OfficeOpenXml.Style;
using System;
using System.Data;
using imbSCI.Core.reporting;
using imbSCI.Core.reporting.style.core;
using imbSCI.Core.reporting.style.enums;
using imbSCI.Core.reporting.style.shot;
using imbSCI.Core.reporting.zone;
using imbSCI.Data.enums;
using imbSCI.Data.enums.reporting;
using imbSCI.DataComplex.converters;
using imbSCI.DataComplex.extensions.data.formats;
using imbSCI.DataComplex.extensions.data.operations;
using imbSCI.DataComplex.extensions.data.schema;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Style.XmlAccess;
using imbSCI.Core.extensions.enumworks;

namespace imbSCI.DataComplex.tables.extensions
{
    public static class DataTableExportUtilities
    {
        

        /// <summary>
        /// Checks if data type is allowed for the DataTable
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static bool checkIfDataTypeIsAllowed(this Type type)
        {
            if (type == typeof(bool)) return true;
            if (type == typeof(byte)) return true;
            if (type == typeof(byte[])) return true;
            if (type == typeof(char)) return true;
            if (type == typeof(DateTime)) return true;
            if (type == typeof(decimal)) return true;
            if (type == typeof(double)) return true;
            if (type == typeof(Guid)) return true;
            if (type == typeof(short)) return true;
            if (type == typeof(int)) return true;
            if (type == typeof(long)) return true;
            if (type == typeof(sbyte)) return true;
            if (type == typeof(float)) return true;
            if (type == typeof(string)) return true;
            if (type == typeof(TimeSpan)) return true;
            if (type == typeof(ushort)) return true;
            if (type == typeof(uint)) return true;
            if (type == typeof(ulong)) return true;

            return false;
        }

    }
}