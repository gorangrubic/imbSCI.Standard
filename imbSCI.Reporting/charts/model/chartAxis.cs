using imbSCI.Data;
using imbSCI.Reporting.charts.core;
using Newtonsoft.Json;
using System;
using System.Xml.Serialization;

namespace imbSCI.Reporting.charts.model
{
    public class chartAxis
    {
        [XmlAttribute(AttributeName = "type")]
        public chartTypeEnum axisType { get; set; } = chartTypeEnum.timeseries;

        [JsonProperty(PropertyName = "tick")]
        public chartAxisTick tick { get; set; }

        public String label { get; set; } = "";

        public String axisLetter { get; set; } = "x";

        public chartAxis()
        {

        }

        public String ToJS()
        {

            String output = axisLetter + ": {" + Environment.NewLine;


            if (!label.IsNullOrEmpty())
            {
                output = output.add(" label: '" + label + "'");
            }

            if (axisType != chartTypeEnum.none)
            {
                output = output.add("type : '" + axisType.ToString() + "'", ", " + Environment.NewLine);
            }



            if (tick != null)
            {
                output = output.add(tick.ToJS(), ", " + Environment.NewLine);
            }

            output = output.add("}", ", " + Environment.NewLine);

            return output;

        }
    }
}
