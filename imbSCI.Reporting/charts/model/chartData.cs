using imbSCI.Data;
using imbSCI.Reporting.charts.core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace imbSCI.Reporting.charts.model
{
    public class chartData
    {

        public List<dataAxisXYPair> dataAxisPairs { get; set; } = new List<dataAxisXYPair>();

        public List<chartDataColumn> columns { get; set; } = new List<chartDataColumn>();

        public chartTypeEnum type { get; set; } = chartTypeEnum.none;

        public String ToJS(String propertyNamePrefix = "data: {")
        {

            String output = propertyNamePrefix + Environment.NewLine;

            if (dataAxisPairs.Count() > 1)
            {
                output = output + " xs : {" + Environment.NewLine;
                String inner = "";
                foreach (var p in dataAxisPairs)
                {
                    inner = inner.add(p.ToJS(), ", " + Environment.NewLine);
                }
                output = output + inner + "}," + Environment.NewLine;

            }
            else if (dataAxisPairs.Count() == 1)
            {
                output = output + dataAxisPairs[0].ToJS();
            }
            else
            {

            }

            output = output.add(columns.ToJS(Environment.NewLine + "columns:"), ", " + Environment.NewLine);

            if (type != chartTypeEnum.none)
            {
                output = output.add("type : '" + type.ToString() + "'", ", " + Environment.NewLine);
            }

            output = output.add("}", Environment.NewLine);
            return output;
        }

    }
}