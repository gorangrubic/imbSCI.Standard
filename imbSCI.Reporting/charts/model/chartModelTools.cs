using imbSCI.Data;
using System;
using System.Collections.Generic;

namespace imbSCI.Reporting.charts.model
{
    public static class chartModelTools
    {
        public static String ToJS(this IEnumerable<chartDataColumn> columns, String propertyNamePrefix = "columns:")
        {

            String output = propertyNamePrefix + " [";
            String inner = "";
            foreach (chartDataColumn c in columns)
            {
                inner = inner.add(c.ToJS(), ", " + Environment.NewLine);
            }
            output += Environment.NewLine + inner;
            return output + "]";
        }
        public static String ToJS(this IEnumerable<chartAxis> columns, String propertyNamePrefix = "axis:")
        {

            String output = propertyNamePrefix + " {";
            String inner = "";

            foreach (chartAxis c in columns)
            {
                inner = inner.add(c.ToJS(), ", " + Environment.NewLine);
            }

            output += Environment.NewLine + inner;
            return output + "}";
        }



    }
}