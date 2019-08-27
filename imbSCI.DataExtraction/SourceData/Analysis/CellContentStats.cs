using imbSCI.Core.data;
using imbSCI.Core.extensions.text;
using imbSCI.Core.files.folders;
using imbSCI.Core.math;
using imbSCI.Core.math.range.frequency;
using imbSCI.Core.reporting.render;
using imbSCI.Core.reporting.zone;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imbSCI.DataExtraction.SourceData.Analysis
{
    public class CellContentStats : IEquatable<CellContentStats>
    {
        //protected Dictionary<CellContentType, Double> rate { get; set; } = new Dictionary<CellContentType, Double>();

        //protected Dictionary<CellContentType, Int32> frequency { get; set; } = new Dictionary<CellContentType, int>();

        public void Report(folderNode folder, ITextRender output)
        {
            List<System.Reflection.PropertyInfo> plist = this.ReportBase(output, false);

            output.AppendLine("Cell content type counter:");
            output.AppendLine("-- root:");
            cellContentTypeCounter.Report(output);
            output.AppendLine("-- real:");
            cellContentRealTypeCounter.Report(output);
        }

        protected frequencyCounter<CellContentType> cellContentTypeCounter { get; set; } = new frequencyCounter<CellContentType>();

        protected frequencyCounter<CellContentType> cellContentRealTypeCounter { get; set; } = new frequencyCounter<CellContentType>();

        public List<CellContentType> results { get; protected set; } = new List<CellContentType>();
        public List<CellContentType> results_real { get; protected set; } = new List<CellContentType>();

        public Int32 total_frequency => cellContentTypeCounter.TotalFrequency;

        public Int32 margin_start { get; protected set; } = 0;

        public Int32 margin_end { get; protected set; } = 0;

        public Int32 min_width { get; protected set; } = Int32.MaxValue;
        public Int32 max_width { get; protected set; } = Int32.MinValue;

        public CellContentType dominantType { get; protected set; } = CellContentType.unknown;

        public CellContentStats()
        {
            Init();
        }

        public Boolean IsUniformFormat()
        {
            return cellContentTypeCounter.DistinctCount() == 1; //(frequency.Count < 2);
        }

        /// <summary>
        /// Processes the specified information point
        /// </summary>
        /// <param name="info">The information on cell content</param>
        public virtual void Assign(CellContentInfo info)
        {
            var types = info.type.getEnumListFromFlags<CellContentType>();
            CellContentType t = info.type;


            foreach (CellContentType mt in measuredTypes)
            {
                if (t.HasFlag(mt))
                {
                    cellContentTypeCounter.Count(mt);
                    break;
                }
            }
           
            cellContentRealTypeCounter.Count(t);

            if (cellContentTypeCounter.DistinctCount() > 0)
            {
                dominantType = cellContentTypeCounter.GetItemsWithTopFrequency().FirstOrDefault();
            }
            results.Add(t);
            results_real.Add(info.type);

            //frequency[t]++;

            min_width = Math.Min(min_width, info.length);
            max_width = Math.Max(max_width, info.length);
        }

        internal virtual void Compute()
        {
            //Int32 totalFrequency = 0;
           // List<CellContentType> types = cellContentTypeCounter.GetDistinctItems();  //new List<CellContentType>();
          //  totalFrequency = cellContentTypeCounter.TotalFrequency;

            //foreach (var pair in frequency)
            //{
            //    totalFrequency += pair.Value;
            //    types.Add(pair.Key);
            //}

           // Double maxRate = Double.MinValue;
           
            /*
            foreach (var pair in frequency)
            {
                Double r = frequency[pair.Key].GetRatio(totalFrequency);
                if (maxRate < r)
                {
                    maxRate = r;
                    maxType = pair.Key;
                }
                rate[pair.Key] = r;
            }*/
            // = maxType;

            Int32 m_start = 0;
            for (int i = 0; i < results.Count; i++)
            {
                if (results[i] != dominantType)
                {
                    m_start++;
                }
                else
                {
                    break;
                }
            }

            margin_start = m_start;

            Int32 m_end = 0;
            for (int i = results.Count - 1; i >= 0; i--)
            {
                if (results[i] != dominantType)
                {
                    m_end++;
                }
                else
                {
                    break;
                }
            }
            margin_end = m_end;
           // total_frequency = total_frequency;
        }

        private static Object _measuredTypes_lock = new Object();
        private static List<CellContentType> _measuredTypes;

        /// <summary>
        /// static and autoinitiated object
        /// </summary>
        public static List<CellContentType> measuredTypes
        {
            get
            {
                if (_measuredTypes == null)
                {
                    lock (_measuredTypes_lock)
                    {
                        if (_measuredTypes == null)
                        {
                            _measuredTypes = new List<CellContentType>();
                            _measuredTypes.AddRange(new CellContentType[] {  CellContentType.textual, CellContentType.bank_account_number, CellContentType.date_format, CellContentType.formatted, CellContentType.mixed, CellContentType.withPercentage, CellContentType.numeric });
                            // add here if any additional initialization code is required
                        }
                    }
                }
                return _measuredTypes;
            }
        }

        protected void Init()
        {
            //foreach (CellContentType t in measuredTypes)
            //{
            //    frequency.Add(t, 0);
            //    rate.Add(t, 0);
            //}
        }

        /// <summary>
        /// Indicates whether the content type frequencies are the same
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.
        /// </returns>
        public bool Equals(CellContentStats other)
        {
            var items = other.cellContentTypeCounter.GetDistinctItems();
            foreach (var i in items)
            {
                if (cellContentTypeCounter.GetFrequencyForItem(i) == other.cellContentTypeCounter.GetFrequencyForItem(i))
                {

                } else
                {
                    return false;
                }
            }
            //cellContentTypeCounter
            //foreach (var pair in frequency)
            //{
            //    if (other.frequency[pair.Key] != pair.Value)
            //    {
            //        return false;
            //    }
            //}

            return true;
        }
    }
}