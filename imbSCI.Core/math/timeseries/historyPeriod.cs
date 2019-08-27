using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace imbSCI.Core.math.timeseries
{
    /// <summary>
    /// Timespan representation
    /// </summary>
    public class historyPeriod
    {
        private String _periodName;

        /// <summary>
        /// Gets or sets the name of the period.
        /// </summary>
        /// <value>
        /// The name of the period.
        /// </value>
        public String periodName
        {
            get
            {
                if (_periodName == null)
                {
                    _periodName = GetPeriodName();
                }
                return _periodName;
            }
            set { _periodName = value; }
        }

        /// <summary>
        /// Determines whether [is in period] [the specified event date time].
        /// </summary>
        /// <param name="eventDateTime">The event date time.</param>
        /// <returns>
        ///   <c>true</c> if [is in period] [the specified event date time]; otherwise, <c>false</c>.
        /// </returns>
        public Boolean IsInPeriod(DateTime eventDateTime)
        {
            return (eventDateTime >= start) && (eventDateTime < end);
        }

        /// <summary>
        /// Determines whether [is ebraced by period] [the specified period].
        /// </summary>
        /// <param name="period">The period.</param>
        /// <returns>
        ///   <c>true</c> if [is ebraced by period] [the specified period]; otherwise, <c>false</c>.
        /// </returns>
        public Boolean IsEbracedByPeriod(historyPeriod period)
        {
            if ((start >= period.start) && (end <= period.end))
            {
                return true;
            }
            return false;
        }



        /// <summary>
        /// Gets a verbose name for this period
        /// </summary>
        /// <returns></returns>
        public String GetPeriodName()
        {
            String output = "";

            historyBinSize binSize = span.GetPeriodSize();

            switch (binSize)
            {
                case historyBinSize.quartal:
                    output = start.Year.ToString() + start.GetQuartalName();
                    break;
                case historyBinSize.year:
                    output = start.Year.ToString();
                    break;
                case historyBinSize.month:
                    output = start.ToString("yyyy-MM");
                    break;
            }

            if (output == "")
            {
                output = start.ToShortDateString() + "-" + end.ToShortDateString();
            }
            return output;
        }

        public historyPeriod()
        {

        }

        public historyPeriod(DateTime _start, DateTime _end)
        {
            start = _start;
            end = _end;
        }

        public historyPeriod(DateTime _start, TimeSpan _duration)
        {
            start = _start;
            end = _start.Add(_duration);
        }

        /// <summary>
        /// Gets the years.
        /// </summary>
        /// <param name="Inclusive">if set to <c>true</c> [inclusive].</param>
        /// <returns></returns>
        public List<Int32> GetYears(Boolean Inclusive=true)
        {
            List<Int32> output = new List<int>();
            for (int i = start.Year; i <= end.Year; i++)
            {
                output.Add(i);
            }
            return output;
        }

        /// <summary>
        /// Adjusts start or end so it enbraces specified <c>date</c>
        /// </summary>
        /// <param name="date">The date.</param>
        public void Deploy(DateTime date)
        {
            if (start > date) start = date;
            if (end < date) end = date;
        }

        public void Reset()
        {
            start = DateTime.MaxValue;
            end = DateTime.MinValue;
        }

        public DateTime start { get; set; } = DateTime.MaxValue;
        public DateTime end { get; set; } = DateTime.MinValue;

        [XmlIgnore]
        public TimeSpan span
        {
            get
            {
                return end.Subtract(start);
            }
        }
    }
}