using imbSCI.DataComplex.converters.core;
using System;

namespace imbSCI.DataComplex.converters
{
    public class DataTableConverterASCIISettings : ConverterSettingsBase
    {

        public static DataTableConverterASCIISettings GetTabSeparatedValues()
        {
            DataTableConverterASCIISettings output = new DataTableConverterASCIISettings();
            output.columnSeparator = '\t'.ToString();

            output.rowSeparator = Environment.NewLine;

            return output;
        }

        public static DataTableConverterASCIISettings GetCommaSeparatedValues()
        {
            DataTableConverterASCIISettings output = new DataTableConverterASCIISettings();
            output.columnSeparator = ",";


            output.rowSeparator = Environment.NewLine;

            return output;
        }


        public String columnSeparator { get; set; }

        public String rowSeparator { get; set; } = Environment.NewLine;

        /// <summary>
        /// What value to use as replacement for <see cref="columnSeparator"/> and <see cref="rowSeparator"/> if found in cell value
        /// </summary>
        /// <value>
        /// The separator replacement.
        /// </value>
        public String separatorReplacement { get; set; } = " ";

        public String value_format { get; set; } = "";


        public String encoding { get; set; } = "";

        /// <summary>
        /// Index of row with column name. To disable, set: -1
        /// </summary>
        /// <value>
        /// The index of the column name row.
        /// </value>
        public Int32 columnNameRowIndex { get; set; } = 0;


        public DataTableConverterASCIISettings()
        {


            columnSeparator = '\t'.ToString();

            rowSeparator = Environment.NewLine;
        }
    }
}