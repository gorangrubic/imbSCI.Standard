using imbSCI.BusinessData.Metrics.Core;
using imbSCI.Core.attributes;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace imbSCI.BusinessData.Metrics
{
    /// <summary>
    /// Contains annual records on international trade
    /// </summary>
    public class InternationalTradeRecords : AnnualMetricsCollection<InternationalTradeByCountry>, IAnnualMetricsBase
    {
        public InternationalTradeRecords()
        {
        }

        /// <summary>
        /// Year of the metrics
        /// </summary>
        /// <value>
        /// The year.
        /// </value>
        [imb(imbAttributeName.measure_excludeFromMetrics)]
        public Int32 year { get; set; } = 2019;

        [XmlIgnore]
        public DateTime PeriodStart
        {
            get
            {
                return new DateTime(year, 1, 1);
            }
        }

        /// <summary>Inclusive end of the period.</summary>
        /// <value>The period end.</value>
        [XmlIgnore]
        public DateTime PeriodEnd
        {
            get
            {
                return new DateTime(year, 12, 31);
            }
        }


        /// <summary>Gets the records according to criteria</summary>
        /// <param name="direction">Trade direction</param>
        /// <param name="countryNameNeedle">Country name or names, given as string list with items separated by , | or ;</param>
        /// <returns>Matched records</returns>
        public List<InternationalTradeByCountry> GetRecords(InternationalTradeDirection direction, String countryNameNeedle = "")
        {
            var selected = items.Where(x => x.Direction.HasFlag(direction));

            List<InternationalTradeByCountry> output = new List<InternationalTradeByCountry>();

            if (selected == null) return output;

            if (!countryNameNeedle.isNullOrEmpty())
            {
                List<String> countries = countryNameNeedle.Split(",|;".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();

                foreach (var record in selected)
                {
                    if (countries.Any(x => record.Country.Equals(x, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        output.Add(record);
                    }
                }
            }
            else
            {
                output.AddRange(selected);
            }

            return output;
        }

        public string GetUID()
        {
            return year.ToString();
        }
    }
}