using imbSCI.Core.extensions.table;
using imbSCI.Core.math.classificationMetrics;
using imbSCI.Core.math.range.finder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace imbSCI.Core.math.measurement
{
    /*
    public class RangedMetrics<T> : IMetricsBase where T : MetricsBase
    {
        public String name { get; set; } = "";

        /// <summary>
        /// The rangers
        /// </summary>
        public Dictionary<String, rangeFinder> rangers = new Dictionary<string, rangeFinder>();

        List<String> fields = new List<string>();


        // public List<String> metricToExclude { get; set; } = new List<string>() { "Count" };


        public Dictionary<String, Double> vectorValues = new Dictionary<string, double>();

        /// <summary>
        /// Gets the dictionary.
        /// </summary>
        /// <param name="includeIntegers">if set to <c>true</c> [include integers].</param>
        /// <param name="includeDoubles">if set to <c>true</c> [include doubles].</param>
        /// <returns></returns>
        public Dictionary<string, double> GetDictionary(bool includeIntegers = true, bool includeDoubles = true)
        {
            Dictionary<String, Double> output = new Dictionary<string, double>();
            if (vectorValues.Any()) return vectorValues;

            foreach (String f in fields)
            {

                var subDict = rangers[f].GetDictionary();

                foreach (var p in subDict)
                {
                    String pk = f + "_" + p.Key;
                    output.Add(pk, p.Value);
                }


            }
            vectorValues = output;
            return output;
        }

        /// <summary>
        /// Returns list of <see cref="Double"/> properties
        /// </summary>
        /// <param name="includeIntegers">if set to <c>true</c> [include integers].</param>
        /// <param name="includeDoubles">if set to <c>true</c> [include doubles].</param>
        /// <returns></returns>
        public List<string> GetFields(bool includeIntegers = true, bool includeDoubles = true)
        {
            List<string> output = new List<string>();

            if (vectorValues.Any()) return vectorValues.Keys.ToList();
            foreach (String f in fields)
            {

                var subDict = rangers[f].GetDictionary().Keys.ToList();

                foreach (var p in subDict)
                {
                    String pk = f + "_" + p;
                    output.Add(pk);
                }


            }

            return output;

        }

        public double GetVector(string propertyName)
        {
            var dict = GetDictionary(true, true);
            return dict[propertyName];
        }

        public List<double> GetVectors(bool includeIntegers = true, bool includeDoubles = true)
        {
            var dict = GetDictionary(true, true);
            return dict.Values.ToList();
        }

        public void Learn(IEnumerable<T> input)
        {
            if (!rangers.Any())
            {
                T first = input.First();

                fields = first.GetFields(true, true);

                foreach (String f in fields)
                {
                    rangers.Add(f, new rangeFinder(f));
                }
            }

            foreach (T t in input)
            {
                Dictionary<String, Double> dict = t.GetDictionary(true, true);

                foreach (String s in fields)
                {
                    if (dict.ContainsKey(s))
                    {
                        rangers[s].Learn(dict[s]);
                    }
                }
            }
        }

        public reportExpandedData SendTo(reportExpandedData target, string formatDouble = "F5")
        {
            var dict = GetDictionary(true, true);
            foreach (var p in dict)
            {
                String pk = p.Key;
                target.Add(pk, p.Value.ToString("F5"), "");
            }

            return target;

        }

        public DataRow SetDataRow(DataRow dr, DataTable table, bool addRowBeforeEnd = true)
        {
            if (dr == null) dr = table.NewRow(); // new DataTable(name);

            var dict = GetDictionary(true, true);

            if (!table.Columns.Contains("Name"))
            {
                dr["Name"] = name;
            }

            foreach (var p in dict)
            {
                dr[p.Key] = p.Value;
            }


            if (addRowBeforeEnd)
            {
                table.Rows.Add(dr);
            }

            return dr;
        }

        public DataTable SetDataTable(string name, DataTable table = null)
        {
            if (table == null) table = new DataTable(name);

            if (!table.Columns.Contains("Name"))
            {
                table.Columns.Add("Name");
            }
            var dict = GetDictionary(true, true);

            foreach (var p in dict)
            {
                var dc = table.Columns.Add(p.Key);
                dc.DataType = typeof(Double);

            }




            return table;
        }
    }*/
}