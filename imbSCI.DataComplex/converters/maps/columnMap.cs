using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;

using imbSCI.Data.data;
using imbSCI.Data;
using imbSCI.Data.extensions.data;

namespace imbSCI.DataComplex.converters.maps
{
    public class columnMapSet<T>:List<columnMap<T>>
    {

        protected columnMap<T> defaultMap { get; set; } = new columnMap<T>();

        public columnMapSet()
        {
            if (typeof(T).IsEnum)
            {
                // creates default map
                var n = Enum.GetNames(typeof(T)).ToList();
                var v = Enum.GetValues(typeof(T));

                defaultMap = new columnMap<T>();
                foreach (T e in v)
                {
                    defaultMap.Add(e, e.ToString());
                }
                this.Add(defaultMap);
            }
        }

        public columnMap<T> CloneDefaultMap()
        {
            columnMap<T> output = new columnMap<T>();
            foreach(var pair in defaultMap)
            {
                output.Add(pair.Key, pair.Value);
            }
            Add(output);
            return output;
        }

        public columnMap<T> RecognizeSet(DataTable dataTable)
        {
            List<String> columns = new List<string>();
            foreach (DataColumn dc in dataTable.Columns)
            {
                columns.Add(dc.ColumnName);
            }

            return RecognizeSet(columns);
        }

        /// <summary>
        /// Returns column map having the highest crosssection with given column labels
        /// </summary>
        /// <param name="columnLabels">The column labels.</param>
        /// <returns></returns>
        public columnMap<T> RecognizeSet(List<String> columnLabels)
        {
            
            Int32 maxScore = Int32.MinValue;

            columnMap<T> output = null;

            foreach (columnMap<T> map in this)
            {
                var mapLabels = map.Values.ToList();

                Int32 c = mapLabels.GetCrossSection(columnLabels).Count;
                if (maxScore < c)
                {
                    output = map;
                    maxScore = c;
                }

            }

            return output;

        }
    }

    public class columnMap<T>:Dictionary<T, String>
    {


    }
}
