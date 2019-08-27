using System;

namespace imbSCI.Reporting.charts.model
{
    public class dataAxisXYPair
    {

        public dataAxisXYPair() { }

        public String YColumnName { get; set; } = "";

        public String XColumnName { get; set; } = "";

        public String ToJS()
        {
            String output = YColumnName + ": " + "'" + YColumnName + "'";

            return output;
        }
    }
}