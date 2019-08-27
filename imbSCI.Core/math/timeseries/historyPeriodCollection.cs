using System;
using System.Collections.Generic;
using System.Linq;

namespace imbSCI.Core.math.timeseries
{
    public class historyPeriodCollection : List<historyPeriod>
    {
        private historyPeriod _beforePeriod;
        private historyPeriod _afterPeriod;

        public historyPeriodCollection()
        {

        }

        public List<historyPeriod> GetSecondHalfPeriods()
        {
            List<historyPeriod> output = new List<historyPeriod>();


            historyPeriodCollection list = this;
            for (int i1 = 0; i1 < list.Count; i1++)
            {
                if (i1 > (list.Count / 2))
                {
                    historyPeriod i = list[i1];
                    output.Add(i);
                }


            }


            return output;
        }


        /// <summary>
        /// Gets all periods - including <see cref="beforePeriod"/> and <see cref="afterPeriod"/>
        /// </summary>
        /// <returns></returns>
        public List<historyPeriod> GetAllPeriods(Boolean includeBeforeAndAfter)
        {
            List<historyPeriod> output = new List<historyPeriod>();
            if (includeBeforeAndAfter) output.Add(beforePeriod);
            output.AddRange(this);
            if (includeBeforeAndAfter) output.Add(afterPeriod);
            return output;
        }

        public historyPeriod GetPeriod(DateTime eventDateTime, Boolean includeBeforeAndAfter = true)
        {
            var output = this.FirstOrDefault(x => x.IsInPeriod(eventDateTime));

            if (output == null)
            {
                if (includeBeforeAndAfter)
                {
                    if (beforePeriod.IsInPeriod(eventDateTime)) return beforePeriod;
                    if (afterPeriod.IsInPeriod(eventDateTime)) return afterPeriod;


                }
            }
            return output;

        }

        public historyPeriod beforePeriod
        {
            get
            {
                if (_beforePeriod == null)
                {
                    if (Count > 0)
                    {
                        _beforePeriod = new historyPeriod(historyTools.MinDate, this.Min(x => x.start));
                        _beforePeriod.periodName = historyTools.BINNAME_BEFORE;
                    }
                }
                return _beforePeriod;
            }
            set { _beforePeriod = value; }
        }
        public historyPeriod afterPeriod
        {
            get
            {
                if (_afterPeriod == null)
                {
                    if (Count > 0)
                    {
                        _afterPeriod = new historyPeriod(this.Max(x => x.end), historyTools.MaxDate);
                        _afterPeriod.periodName = historyTools.BINNAME_AFTER;
                    }
                }

                return _afterPeriod;
            }
            set { _afterPeriod = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="historyPeriodCollection"/> class.
        /// </summary>
        /// <param name="_start">The start.</param>
        /// <param name="_end">The end.</param>
        /// <param name="periodSize">Size of the period.</param>
        /// <param name="lastPeriodEqual">if set to <c>true</c> the last period will be extended over <c>_end</c> date, to be equal with other periods in the collection</param>
        public historyPeriodCollection(DateTime _start, DateTime _end, historyBinSize periodSize, Boolean lastPeriodEqual = true)
        {
            DateTime head = _start;
            historyPeriod lastPeriod = null;
            while (head < _end)
            {
                lastPeriod = periodSize.ToPeriod(head);
                Add(lastPeriod);
                head = lastPeriod.end;
            }

            if (!lastPeriodEqual)
            {
                lastPeriod.end = _end;
            }



        }
    }
}