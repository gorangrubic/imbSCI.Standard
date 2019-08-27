using imbSCI.Core.extensions.table;
using imbSCI.Core.extensions.text;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace imbSCI.Reporting.charts.model
{
    public class chartDataColumn
    {

        public String dataFormatting { get; set; } = "";
        public String columnLabel { get; set; } = "";
        public String escapeValueFormat { get; set; } = "{0}";
        public List<Object> columnValues { get; set; } = new List<object>();

        public chartDataColumn()
        {

        }

        public chartDataColumn(String label, Object value)
        {
            columnLabel = label;
            columnValues.Add(value);
        }

        public chartDataColumn(DataColumn source)
        {
            columnLabel = source.GetHeading();
            dataFormatting = source.GetFormat();

            if (!source.GetValueType().isNumber())
            {
                escapeValueFormat = "'{0}'";
            }

            foreach (DataRow dr in source.Table.Rows)
            {
                String v = dr[source].toStringSafe("", dataFormatting);
                if (escapeValueFormat != "{0}")
                {

                    v = String.Format(escapeValueFormat, v);


                }
                columnValues.Add(v);
            }
        }

        public String ToJS()
        {
            String output = "['" + columnLabel + "'";
            foreach (Object vl in columnValues)
            {

                output = output.add(vl.toStringSafe("", dataFormatting), ", ");
            }
            output += "]";
            return output;
        }

    }
}