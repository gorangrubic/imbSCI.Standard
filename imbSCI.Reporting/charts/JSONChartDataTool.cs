using imbSCI.Core.extensions.table;
using System;
using System.Data;
using System.Linq;
using System.Text;

namespace imbSCI.Reporting.charts
{
    public class JSONChartDataTool
    {
        public string leftSquareBracket = "-::";

        public string rightSquareBracket = "::-";

        public string singleQuoteBracket = "!";

        public string dataEntrySeparator = ",";

        public String DataToJ3CodeData(DataTable table, params String[] columnToSeries)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(leftSquareBracket + " ");

            for (int i2 = 0; i2 < columnToSeries.Length; i2++)
            {
                String column_name = columnToSeries[i2];
                DataColumn dc = table.Columns[column_name];
                String label = dc.GetHeading();

                sb.Append(leftSquareBracket + " " + singleQuoteBracket + label + singleQuoteBracket + " " + dataEntrySeparator + " ");

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    DataRow dr = table.Rows[i];

                    if (i == table.Rows.Count - 1)
                    {
                        sb.Append(dr[dc] + " ");
                    }
                    else
                    {
                        sb.Append(dr[dc] + dataEntrySeparator + " ");
                    }
                }

                if (column_name == columnToSeries.Last())
                {
                    sb.Append(" " + rightSquareBracket);
                }
                else
                {
                    sb.Append(" " + rightSquareBracket + " " + dataEntrySeparator);
                }


            }
            sb.Append(" " + rightSquareBracket);
            return sb.ToString();
        }
    }
}