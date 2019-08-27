using imbSCI.Core.extensions.table;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Core.math.range.finder;
using imbSCI.Core.reporting.render;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace imbSCI.Core.math.range.frequency
{

    /// <summary>
    /// Counts instance frequencies 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class frequencyCounter<T>
    {

        /// <summary>
        /// Gets the data table with results
        /// </summary>
        /// <param name="itemLabelFunction">Getter for item label</param>
        /// <param name="Ranked">Should results be shown in ranked order</param>
        /// <param name="AdditionalItemColumns">Optional additional item information columns</param>
        /// <returns></returns>
        public DataTable GetDataTable(Func<T, String> itemLabelFunction, Boolean Ranked=true, params Func<T, String>[] AdditionalItemColumns)
        {
            DataTable table = new DataTable("FrequencyCounter");
            

            DataColumn column_rank = null;
            if (Ranked)
            {
                column_rank = table.Columns.Add("Rank").SetHeading("#").SetWidth(5);
            }


            var column_itemLabel = table.Columns.Add("Item").SetWidth(30).SetDesc("Item being counted");

            var column_frequency = table.Columns.Add("Freq", typeof(Int32)).SetWidth(10).SetLetter("f^i").SetDesc("Frequency - number of item occurences");

            var column_percent = table.Columns.Add("Rate", typeof(Double)).SetWidth(10).SetFormat("F2").SetLetter("f^i/sum(f)").SetDesc("Item frequency / total frequency");

            var column_normalized = table.Columns.Add("NFreq").SetWidth(10).SetFormat("F2").SetDesc("Normalized item frequency, frequency / max. frequency").SetLetter("f^i/max(f)");


            // making additional columns
            List<Func<T, string>> addColumns = new List<Func<T, string>>();

            Dictionary<DataColumn, Func<T, string>> AddColumnsFunctionDict = new Dictionary<DataColumn, Func<T, string>>();

            if (AdditionalItemColumns != null)
            {
                addColumns.AddRange(AdditionalItemColumns);
            }

            Int32 c = 1;
            foreach (var addItemColumn in AdditionalItemColumns)
            {

                DataColumn dc = table.Columns.Add("ItemInfo" + c.ToString()).SetHeading("Item info " + c.ToString()).SetWidth(80);
                AddColumnsFunctionDict.Add(dc, addItemColumn);
                c++;
            }
            // ---

            var records = GetRecords();
            
            if (Ranked)
            {
                records = records.OrderByDescending(x => x.Value).ToList();
            }

            Int32 maxFreq = GetTopFrequency();

            for (int i = 0; i < records.Count; i++)
            {
                DataRow dr = table.NewRow();
                var record = records[i];

                if (Ranked)
                {
                    dr[column_rank] = (i + 1).ToString();
                }

                dr[column_itemLabel] = itemLabelFunction(record.Key); // source.itemLabel;

                dr[column_frequency] = record.Value;

                dr[column_normalized] = record.Value.GetRatio(maxFreq); // source.normalized;


                dr[column_percent] = record.Value.GetRatio(TotalFrequency);


                foreach (var pair in AddColumnsFunctionDict)
                {
                    dr[pair.Key] = pair.Value(record.Key);
                }
                
                table.Rows.Add(dr);
            }


            table.SetAdditionalInfoEntry("Max(f)", maxFreq, "Highest frequency");
            table.SetAdditionalInfoEntry("Sum(f)", TotalFrequency, "Total frequency");
            table.SetAdditionalInfoEntry("|I|", records.Count, "Distinct instances");
            table.SetDescription("Results for " + GetType().GetCleanTypeName() + " instance");

            return table;
        }

        /// <summary>
        /// Reports the specified output.
        /// </summary>
        /// <param name="output">The output.</param>
        public void Report(ITextRender output)
        {
            var bins = GetFrequencyBins();
            foreach (Int32 sc in bins.Keys.ToList().OrderByDescending(x=>x))
            {
                output.AppendLine("F:[" + sc.ToString() + "] C:" + bins[sc].Count);
                StringBuilder sb = new StringBuilder();
                foreach (T i in bins[sc])
                {
                    sb.Append(i.ToString() + " ");
                }
                output.AppendLine(sb.ToString());
            }

        }

        /// <summary>
        /// Gets list with all distinct {T} instances counted by <see cref="Count(T)"/>
        /// </summary>
        /// <returns></returns>
        public List<T> GetDistinctItems()
        {
            return frequency.Keys.ToList();
        }

        /// <summary>
        /// Number of different {T} ever counted by <see cref="Count(T)"/>
        /// </summary>
        /// <returns></returns>
        public Int32 DistinctCount()
        {
            return frequency.Count;
        }

        protected Dictionary<T, Int32> frequency { get; set; } = new Dictionary<T, int>();

        /// <summary>
        /// Gets the records.
        /// </summary>
        /// <returns></returns>
        public List<KeyValuePair<T, Int32>> GetRecords()
        {
            return frequency.ToList();
        }

        /// <summary>
        /// Gets the frequency for item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public Int32 GetFrequencyForItem(T item)
        {
            if (!frequency.ContainsKey(item)) return 0;
            return frequency[item];
        }

        /// <summary>
        /// Gets the range.
        /// </summary>
        /// <returns></returns>
        public rangeFinder GetRange()
        {
            rangeFinder output = new rangeFinder();
            foreach (var pair in frequency)
            {
                output.Learn(pair.Value);
            }
            return output;
        }

        /// <summary>
        /// Gets the frequency bins: items are grouped by the same frequency. 
        /// </summary>
        /// <returns></returns>
        public Dictionary<Int32, List<T>> GetFrequencyBins()
        {
            Dictionary<Int32, List<T>> output = new Dictionary<Int32, List<T>>();

            var sorted = frequency.OrderByDescending(x => x.Value);
            Int32 topFrequency = 0;

            foreach (KeyValuePair<T, int> s in sorted)
            {
                if (!output.ContainsKey(s.Value))
                {
                    output.Add(s.Value, new List<T>());
                }

                output[s.Value].Add(s.Key);

            }

            return output;
        }

        public Int32 GetTopFrequency()
        {
            if (!frequency.Any()) return 0;

            return frequency.Max((x => x.Value));
        }

        public List<T> GetItemsWithTopFrequency(Int32 tolerance = 0)
        {
            List<T> output = new List<T>();

            var sorted = frequency.OrderByDescending(x => x.Value);
            Int32 topFrequency = 0;

            foreach (KeyValuePair<T, int> s in sorted)
            {
                if (topFrequency == 0)
                {
                    topFrequency = s.Value;
                }
                if (s.Value >= (topFrequency - tolerance))
                {
                    output.Add(s.Key);
                }


            }

            return output;
            
        }


        public Int32 TotalFrequency { get; protected set; } = 0;

        /// <summary>
        /// Returns if any <see cref="Count(T)"/> call was made, i.e. if <see cref="TotalFrequency"/> is above 0
        /// </summary>
        /// <returns></returns>
        public Boolean Any()
        {
            return TotalFrequency > 0;
        }

        public virtual void Count(T input)
        {
            if (input == null) return;
            if (!frequency.ContainsKey(input)) frequency.Add(input, 0);
            frequency[input]++;
            TotalFrequency++;
        }

        /// <summary>
        /// Counts in the collection specified
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public void Count(IEnumerable<T> inputs)
        {
            foreach (T inp in inputs)
            {
                Count(inp);
            }
        }

        /// <summary>
        /// Gets the most frequent item.
        /// </summary>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public T GetMostFrequentItem(T defaultValue=default(T))
        {
            if (frequency.Count == 0)
            {
                return defaultValue;
            }
            var sorted = frequency.OrderByDescending(x => x.Value);

            if (sorted != null)
            {
                if (sorted.Any())
                {
                    return sorted.FirstOrDefault().Key;
                }

            }

            return default(T);
        }

    }
}
