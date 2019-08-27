using imbSCI.Data;
using imbSCI.Reporting.charts.core;
using System;

namespace imbSCI.Reporting.charts.model
{
    public class chartTypeFormatting
    {
        public chartTypeFormatting()
        {

        }

        public chartTypeEnum axisType { get; set; } = chartTypeEnum.bar;

        public Double width { get; set; } = 0.0;
        public String title { get; set; } = "";

        public String ToJS()
        {

            String output = axisType.ToString() + ": {" + Environment.NewLine;

            String inner = "";
            if (width != 0)
            {
                inner = inner.add("width: " + width.ToString(), ", " + Environment.NewLine);
            }

            if (!title.isNullOrEmpty())
            {
                inner = inner.add("title: '" + width.ToString() + "'", ", " + Environment.NewLine);
            }


            return output + Environment.NewLine + "}";
        }
    }
}