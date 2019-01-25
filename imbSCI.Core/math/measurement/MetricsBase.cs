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
    /// <summary>
    /// Base class for metrics containers
    /// </summary>
    public abstract class MetricsBase
    {
        /// <summary>
        /// Sends metric data into
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="formatDouble">The format double.</param>
        /// <returns></returns>
        public reportExpandedData SendTo(reportExpandedData target, String formatDouble = "F5")
        {
            if (target == null) target = new reportExpandedData();
            foreach (var pi in Integers)
            {
                target.Add(pi.Name, ((Int32)pi.GetValue(this, null)).ToString());
            }

            foreach (var pi in Doubles)
            {
                target.Add(pi.Name, ((Double)pi.GetValue(this, null)).ToString(formatDouble));
            }
            return target;
        }

        protected MetricsBase()
        {
            Init();
        }

        /// <summary>
        /// Pluses the specified b.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="b">The b.</param>
        public void Plus<T>(T b) where T : MetricsBase
        {
            Type tb = typeof(T);

            foreach (var pi in Integers)
            {
                var pib = tb.GetProperty(pi.Name);
                Int32 rb = (Int32)pib.GetValue(b, null);
                Int32 ra = (Int32)pi.GetValue(this, null);
                pi.SetValue(this, rb + ra, null);
            }

            foreach (var pi in Doubles)
            {
                var pib = tb.GetProperty(pi.Name);
                Double rb = (Double)pib.GetValue(b, null);
                Double ra = (Double)pi.GetValue(this, null);
                pi.SetValue(this, rb + ra, null);
            }
        }

        /// <summary>
        /// Minuses the specified b.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="b">The b.</param>
        public void Minus<T>(T b) where T : MetricsBase
        {
            Type tb = typeof(T);

            foreach (var pi in Integers)
            {
                var pib = tb.GetProperty(pi.Name);
                Int32 rb = (Int32)pib.GetValue(b, null);
                Int32 ra = (Int32)pi.GetValue(this, null);
                pi.SetValue(this, rb - ra, null);
            }

            foreach (var pi in Doubles)
            {
                var pib = tb.GetProperty(pi.Name);
                Double rb = (Double)pib.GetValue(b, null);
                Double ra = (Double)pi.GetValue(this, null);
                pi.SetValue(this, rb - ra, null);
            }
        }

        /// <summary>
        /// Divides the specified b.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="b">The b.</param>
        public void Divide<T>(T b) where T : MetricsBase
        {
            Type tb = typeof(T);

            foreach (var pi in Integers)
            {
                var pib = tb.GetProperty(pi.Name);
                Int32 rb = (Int32)pib.GetValue(b, null);
                Int32 ra = (Int32)pi.GetValue(this, null);
                Int32 r = 0;
                if (ra != 0 && rb != 0) r = ra / rb;
                pi.SetValue(this, r, null);
            }

            foreach (var pi in Doubles)
            {
                var pib = tb.GetProperty(pi.Name);
                Double rb = (Double)pib.GetValue(b, null);
                Double ra = (Double)pi.GetValue(this, null);
                Double r = 0;
                if (ra != 0 && rb != 0) r = ra / rb;
                pi.SetValue(this, r, null);
                //pi.SetValue(this, rb / ra, null);
            }
        }

        /// <summary>
        /// Powers the specified b.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="b">The b.</param>
        public void Power<T>(T b) where T : MetricsBase
        {
            Type tb = typeof(T);

            foreach (var pi in Integers)
            {
                var pib = tb.GetProperty(pi.Name);
                Int32 rb = (Int32)pib.GetValue(b, null);
                Int32 ra = (Int32)pi.GetValue(this, null);
                pi.SetValue(this, rb * ra, null);
            }

            foreach (var pi in Doubles)
            {
                var pib = tb.GetProperty(pi.Name);
                Double rb = (Double)pib.GetValue(b, null);
                Double ra = (Double)pi.GetValue(this, null);
                pi.SetValue(this, rb * ra, null);
            }
        }

        /// <summary>
        /// Divides the specified b.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="b">The b.</param>
        public void Divide(Double b)
        {
            foreach (var pi in Integers)
            {
                Int32 ra = (Int32)pi.GetValue(this, null);
                Int32 r = 0;
                if (ra != 0 && b != 0) r = Convert.ToInt32(Convert.ToDouble(ra) / b);
                pi.SetValue(this, r, null);
            }

            foreach (var pi in Doubles)
            {
                Double ra = (Double)pi.GetValue(this, null);
                Double r = 0;
                if (ra != 0 && b != 0) r = Convert.ToDouble(ra) / b;
                pi.SetValue(this, r, null);
            }
        }

        /// <summary>
        /// Powers the specified b.
        /// </summary>
        /// <param name="b">The b.</param>
        public void Power(Double b)
        {
            foreach (var pi in Integers)
            {
                Int32 ra = (Int32)pi.GetValue(this, null);
                pi.SetValue(this, ra * b, null);
            }

            foreach (var pi in Doubles)
            {
                Double ra = (Double)pi.GetValue(this, null);
                pi.SetValue(this, ra * b, null);
            }
        }

        /// <summary>
        /// Initializes this instance, registers all Integers and Doubles
        /// </summary>
        protected void Init()
        {
            var t = GetType();
            PropertyInfo[] props = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            props = props.ToList().OrderBy(x => x.Name).ToArray();

            Integers.Clear();
            Doubles.Clear();
            foreach (var p in props)
            {
                if (p.GetIndexParameters().Length == 0)
                {
                    if (p.PropertyType == typeof(Int32))
                    {
                        Integers.Add(p);
                        IntegersDict.Add(p.Name, p);
                    }
                    else if (p.PropertyType == typeof(Double))
                    {
                        Doubles.Add(p);
                        DoublesDict.Add(p.Name, p);
                    }
                }
            }
        }

        /*
        public DataTable SetDataTable(String name, DataTable table = null)
        {
            if (table == null) table = new DataTable(name);

            foreach (PropertyInfo db in Doubles)
            {
                if (!table.Columns.Contains(db.Name))
                {
                    var dc = table.Columns.Add(db.Name);
                    dc.SetFormat("F5");
                }
            }

            foreach (PropertyInfo db in Integers)
            {
                if (!table.Columns.Contains(db.Name))
                {
                    var dc = table.Columns.Add(db.Name);
                }
            }

            return table;
        }
        /*
        /// <summary>
        /// Sets the data row.
        /// </summary>
        /// <param name="dr">The dr.</param>
        /// <param name="table">The table.</param>
        /// <param name="addRowBeforeEnd">if set to <c>true</c> [add row before end].</param>
        /// <returns></returns>
        public DataRow SetDataRow(DataRow dr, DataTable table, Boolean addRowBeforeEnd = true)
        {
            if (dr == null) dr = table.NewRow(); // new DataTable(name);

            foreach (PropertyInfo db in Doubles)
            {
                if (table.Columns.Contains(db.Name))
                {
                    dr[db.Name] = db.GetValue(this, null);
                }
            }

            foreach (PropertyInfo db in Integers)
            {
                if (table.Columns.Contains(db.Name))
                {
                    dr[db.Name] = db.GetValue(this, null);
                }
            }
            if (addRowBeforeEnd)
            {
                table.Rows.Add(dr);
            }

            return dr;
        }*/

        /// <summary>
        /// Gets value of specified property or 0 if no property found under that name
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public virtual Double GetVector(String propertyName)
        {
            if (DoublesDict.ContainsKey(propertyName))
            {
                return (Double)DoublesDict[propertyName].GetValue(this, null); ;
            }
            if (IntegersDict.ContainsKey(propertyName))
            {
                return Convert.ToDouble(IntegersDict[propertyName].GetValue(this, null));
            }
            return 0;
        }

        /// <summary>
        /// Gets the dictionary.
        /// </summary>
        /// <param name="includeIntegers">if set to <c>true</c> [include integers].</param>
        /// <param name="includeDoubles">if set to <c>true</c> [include doubles].</param>
        /// <returns></returns>
        public virtual Dictionary<String, Double> GetDictionary(Boolean includeIntegers = true, Boolean includeDoubles = true)
        {
            Dictionary<String, Double> output = new Dictionary<string, double>();

            if (includeIntegers)
            {
                foreach (var pi in Integers)
                {
                    output.Add(pi.Name, GetVector(pi.Name)); // (Double)pi.GetValue(this, null));
                }
            }

            if (includeDoubles)
            {
                foreach (var pi in Doubles)
                {
                    output.Add(pi.Name, GetVector(pi.Name));
                }
            }

            return output;
        }

        /// <summary>
        /// Gets list of integer and double properties
        /// </summary>
        /// <param name="includeIntegers">if set to <c>true</c> [include integers].</param>
        /// <param name="includeDoubles">if set to <c>true</c> [include doubles].</param>
        /// <returns></returns>
        public virtual List<Double> GetVectors(Boolean includeIntegers = true, Boolean includeDoubles = true)
        {
            List<Double> output = new List<Double>();

            if (includeIntegers)
            {
                foreach (var pi in Integers)
                {
                    output.Add(GetVector(pi.Name));
                }
            }

            if (includeDoubles)
            {
                foreach (var pi in Doubles)
                {
                    output.Add(GetVector(pi.Name));
                }
            }
            return output;
        }

        /// <summary>
        /// Gets list of integer and double properties
        /// </summary>
        /// <param name="includeIntegers">if set to <c>true</c> [include integers].</param>
        /// <param name="includeDoubles">if set to <c>true</c> [include doubles].</param>
        /// <returns></returns>
        public virtual List<String> GetFields(Boolean includeIntegers = true, Boolean includeDoubles = true)
        {
            List<String> output = new List<string>();

            if (includeIntegers)
            {
                foreach (var pi in Integers)
                {
                    output.Add(pi.Name);
                }
            }

            if (includeDoubles)
            {
                foreach (var pi in Doubles)
                {
                    output.Add(pi.Name);
                }
            }
            return output;
        }

        protected Dictionary<String, PropertyInfo> IntegersDict { get; set; } = new Dictionary<String, PropertyInfo>();
        protected Dictionary<String, PropertyInfo> DoublesDict { get; set; } = new Dictionary<String, PropertyInfo>();

        protected List<PropertyInfo> Integers { get; set; } = new List<PropertyInfo>();
        protected List<PropertyInfo> Doubles { get; set; } = new List<PropertyInfo>();
    }
}