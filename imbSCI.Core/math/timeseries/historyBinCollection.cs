using System;
using System.Collections.Generic;

namespace imbSCI.Core.math.timeseries
{
    public class historyBinCollection<T> : Dictionary<String, historyBin<T>> where T : class
    {

        public historyBin<T> beforeBin { get; set; }
        public historyBin<T> afterBin { get; set; }

        

        public void Add(T data, DateTime date)
        {
            historyPeriod period = periodCollection.GetPeriod(date);
            if (period != null)
            {
                if (period.periodName == historyTools.BINNAME_BEFORE)
                {
                    beforeBin.Add(data);
                }
                else if (period.periodName == historyTools.BINNAME_AFTER)
                {
                    afterBin.Add(data);
                }
                else
                {
                    this[period.periodName].Add(data);
                }
            }

        }

        public historyPeriodCollection periodCollection { get; set; }

        public historyBinCollection(historyPeriodCollection periods, Boolean includeBeforeAndAfter)
        {
            periodCollection = periods;

            beforeBin = new historyBin<T>(periodCollection.beforePeriod);
            afterBin = new historyBin<T>(periodCollection.afterPeriod);

            foreach (var period in periods.GetAllPeriods(includeBeforeAndAfter))
            {
                if (period.periodName == historyTools.BINNAME_BEFORE)
                {
                    Add(period.periodName, beforeBin);

                }
                else if (period.periodName == historyTools.BINNAME_AFTER)
                {
                    Add(period.periodName, afterBin);

                }
                else
                {
                    Add(period.periodName, new historyBin<T>(period));
                }

            }


        }

    }
}