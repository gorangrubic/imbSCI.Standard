using System;

namespace imbSCI.Core.math.timeseries
{
    /// <summary>
    /// Timeseries entry
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class historyEntry<T> where T : class
    {
        public DateTime dateTime { get; set; }

        public T data { get; set; }
        public String uid { get; set; }

        public historyEntry(T _data, DateTime _dateTime, String _uid)
        {
            data = _data;
            dateTime = _dateTime;
            uid = _uid;
        }
    }

}
