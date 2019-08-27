using System;
using System.Collections.Generic;

namespace imbSCI.Core.math.timeseries
{
    /// <summary>
    /// Collection of data taken from <see cref="historyEntry{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.Collections.Generic.List{T}" />
    public class historyBin<T> : List<T> where T : class
    {
        public historyBin()
        {

        }

        public historyBin(historyPeriod _period)
        {
            period = _period;
        }

        public historyPeriod period { get; protected set; } = new historyPeriod();

        public void Set(DateTime _start, DateTime _end)
        {
            period = new historyPeriod(_start, _end);

        }

        public void Set(IEnumerable<T> data, DateTime _start, DateTime _end)
        {
            period = new historyPeriod(_start, _end);
            AddRange(data);
        }

        public void Set(IEnumerable<T> data, Func<T, DateTime> dateFunction)
        {
            AddRange(data);
            SetStartEnd(data, dateFunction);
        }

        protected void SetStartEnd(IEnumerable<T> data, Func<T, DateTime> dateFunction)
        {
            period.Reset();


            foreach (T entry in data)
            {

                DateTime date = dateFunction(entry);
                period.Deploy(date);

            }
        }


    }

}
