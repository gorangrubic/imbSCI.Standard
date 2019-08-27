using imbSCI.Data;
using System;
using System.Collections.Generic;

namespace imbSCI.Reporting.charts.model
{
    public class chartModel
    {

        public string ToJS(Boolean includeFunctionPrefix)
        {
            String output = "";

            if (!bindto.isNullOrEmpty())
            {
                output = " bindto: '#" + bindto + "'";
            }

            //
            if (data != null)
            {
                output = output.add(data.ToJS(), ", " + Environment.NewLine);
            }

            if (chartFormat != null)
            {
                output = output.add(chartFormat.ToJS(), ", " + Environment.NewLine);
            }

            if (axis != null)
            {
                output = output.add(axis.ToJS(), ", " + Environment.NewLine);
            }

            if (includeFunctionPrefix) output = "var chart = c3.generate({" + Environment.NewLine + output + Environment.NewLine + "});";

            return output;
        }

        public chartModel()
        {

        }

        public String bindto { get; set; } = "{0}";

        public List<chartAxis> axis { get; set; } = new List<chartAxis>();

        public chartData data { get; set; } = new chartData();

        public chartTypeFormatting chartFormat { get; set; } = null;
    }
}