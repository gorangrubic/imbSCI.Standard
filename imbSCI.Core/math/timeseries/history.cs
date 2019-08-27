using imbSCI.Core.math;
using System;
using System.Collections.Generic;
using System.Linq;

namespace imbSCI.Core.math.timeseries
{
    /// <summary>
    /// Raw history data
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class history<T> where T : class
    {


        Int32 LastCount = Int32.MinValue;

        public Int32 Count { get { return events.Count; } }

        protected void CheckRefresh()
        {
            Boolean doRefresh = false;
            if (LastCount != Count)
            {
                doRefresh = true;
            }
            if (uids.Count != Count)
            {
                doRefresh = true;
            }

            if (doRefresh)
            {
                var sorted_events = events.OrderBy(x => x.dateTime).ToList();

                events.Clear();

                foreach (var entry in sorted_events)
                {
                    if (entry.data != null)
                    {
                        if (first_entry == null) first_entry = entry;
                        events.Add(entry);
                        last_entry = entry;
                    }
                }

                if (uids.Count != Count)
                {
                    uids.Clear();
                    events.ForEach(x => uids.Add(x.uid));
                }

                LastCount = Count;

            }

        }

        historyEntry<T> first_entry = null;
        historyEntry<T> last_entry = null;

        public DateTime StartTime
        {
            get
            {
                if (first_entry == null) CheckRefresh();
                if (first_entry == null) return DateTime.MinValue;
                return first_entry.dateTime;
            }
        }

        public DateTime EndTime
        {
            get
            {
                if (last_entry == null) CheckRefresh();
                if (last_entry == null) return DateTime.MaxValue;
                return last_entry.dateTime;
            }
        }

        protected Func<T, DateTime> dateFunction = null;
        protected Func<T, String> uidFunction = null;

        public history(Func<T, DateTime> _dateFunction, Func<T, String> _uidFunction)
        {
            dateFunction = _dateFunction;
            uidFunction = _uidFunction;
        }

        protected List<historyEntry<T>> events { get; set; } = new List<historyEntry<T>>();

        protected List<String> uids { get; set; } = new List<string>();


        /// <summary>
        /// Creates the history bins.
        /// </summary>
        /// <param name="timeFrameStart">The time frame start.</param>
        /// <param name="timeFrameResolution">The time frame resolution.</param>
        /// <returns></returns>
        public historyBinCollection<T> CreateHistoryBins(historyPeriodCollection periods, Boolean includeBeforeAndAfter)
        {
            historyBinCollection<T> output = new historyBinCollection<T>(periods, includeBeforeAndAfter);
            foreach (var entry in events)
            {
                output.Add(entry.data, entry.dateTime);
            }
            return output;

        }

        /// <summary>Gets the entries.</summary>
        /// <returns>All events in the history</returns>
        public List<historyEntry<T>> GetEntries()
        {
            List<historyEntry<T>> output = new List<historyEntry<T>>();

            foreach (var e in events)
            {
                output.Add(e);
            }

            return output;

        }

        public List<historyEntry<T>> GetEntries(DateTime start, TimeSpan span)
        {
            List<historyEntry<T>> output = new List<historyEntry<T>>();
            DateTime end = start.Add(span);
            return GetEntries(start, end);

        }

        public List<historyEntry<T>> GetEntries(DateTime start, DateTime end)
        {
            List<historyEntry<T>> output = new List<historyEntry<T>>();

            foreach (var entry in events)
            {
                if ((entry.dateTime >= start) || (entry.dateTime <= end))
                {
                    output.Add(entry);
                }
            }

            return output;
        }

        public T GetEntryData(String uid)
        {
            historyEntry<T> entry = GetEntry(uid);
            if (entry != null) return entry.data;
            return null;
        }

        public historyEntry<T> GetEntry(String uid)
        {
            if (!uids.Contains(uid)) return null;
            return events.First(x => x.uid == uid);
        }

        public Boolean Remove(String uid)
        {
            historyEntry<T> entry = GetEntry(uid);
            if (entry != null)
            {
                events.Remove(entry);
                uids.Remove(uid);
                if (entry == last_entry) last_entry = null;
                if (entry == first_entry) first_entry = null;
                return true;
            }
            return false;
        }

        public Boolean Remove(T eventData)
        {
            String uid = uidFunction(eventData);
            return Remove(eventData);
        }

        public void AddRange(IEnumerable<T> eventData)
        {
            if (eventData != null)
            {
                foreach (T data in eventData)
                {
                    Add(data);
                }
            }
        }

        public void Add(T eventData)
        {

            String uid = uidFunction(eventData);
            DateTime dateTime = dateFunction(eventData);
            if (uids.Contains(uid)) return;
            historyEntry<T> entry = new historyEntry<T>(eventData, dateTime, uid);
            events.Add(entry);
            uids.Add(uid);
            if (first_entry == null)
            {
                first_entry = entry;
            }
            else if (last_entry == null)
            {
                last_entry = entry;
            }
            else
            {
                if (first_entry.dateTime > dateTime) first_entry = entry;
                if (last_entry.dateTime < dateTime) last_entry = entry;
            }

        }

    }
}