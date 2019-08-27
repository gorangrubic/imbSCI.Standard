using System;
using System.Collections.Generic;

namespace imbSCI.Core.math.timeseries
{
    public static class historyTools
    {
        public const String Q1 = "Q1";
        public const String Q2 = "Q2";
        public const String Q3 = "Q3";
        public const String Q4 = "Q4";

        public const String BINNAME_BEFORE = "Before";
        public const String BINNAME_AFTER = "After";

        public static DateTime MinDate { get; set; } = new DateTime(2000, 1, 1);
        public static DateTime MaxDate { get; set; } = new DateTime(2100, 1, 1);

        public static String GetQuartalName(this DateTime date)
        {
            if (date.Month < 4) return Q1;
            if (date.Month < 7) return Q2;
            if (date.Month < 10) return Q3;
            return Q4;
        }

        public static Int32 GetStartingMonthsForQuartalName(this String q)
        {
            if (q == Q1) return 0;
            if (q == Q2) return 3;
            if (q == Q3) return 6;
            if (q == Q4) return 9;

            return 0;
        }

        public static DateTime PeriodNameToStartDate(String periodName)
        {
            if (periodName.Length > 5)
            {
                String Q = periodName.Substring(4, 2);

                String Y = periodName.Substring(0, 4);

                Int32 y = Convert.ToInt32(Y);
                Int32 m = GetStartingMonthsForQuartalName(Q);

                DateTime date = new DateTime(y, m, 0);
                return date;
            }

            return default(DateTime);
        }



        public static historyBinSize GetPeriodSize(this TimeSpan span)
        {
            historyBinSize output = historyBinSize.undefined;


            if (span.TotalDays == 7)
            {
                return historyBinSize.week;
            }

            if (span.TotalDays == 14)
            {
                return historyBinSize.twoWeeks;
            }

            if (span.TotalDays == 1)
            {
                return historyBinSize.day;
            }

            if (span.TotalDays > 88 && span.TotalDays < 93)
            {
                return historyBinSize.quartal;
            }

            if (span.TotalDays > 28 && span.TotalDays < 32)
            {
                return historyBinSize.month;
            }

            if (span.TotalDays == 365)
            {
                return historyBinSize.year;
            }

            if (span.TotalDays == (365 * 3))
            {
                return historyBinSize.ThreeYears;
            }

            if (span.TotalDays == (365 * 5))
            {
                return historyBinSize.FiveYears;
            }

            return historyBinSize.custom;

        }

        public static historyPeriod BlendPeriods(this IEnumerable<historyPeriod> periods)
        {
            historyPeriod output = new historyPeriod();
            DateTime minDate = DateTime.MaxValue;
            DateTime maxDate = DateTime.MinValue;

            foreach (historyPeriod p in periods)
            {
                if (p != null)
                {
                    if (minDate > p.start)
                    {
                        minDate = p.start;
                    }
                    if (maxDate < p.end)
                    {
                        maxDate = p.end;
                    }
                }
            }

            output.start = minDate;
            output.end = maxDate;
            output.periodName = output.GetPeriodName();
            return output;
        }

        public static historyPeriod ToPeriod(this historyBinSize binSize, DateTime fromDate, Int32 numberOfBins = 1)
        {
            DateTime end = ToEndDateTime(binSize, fromDate, numberOfBins);

            return new historyPeriod(fromDate, end);
        }

        public static DateTime ToEndDateTime(this historyBinSize binSize, DateTime fromDate, Int32 numberOfBins = 1)
        {
            DateTime end = fromDate;
            for (int i = 0; i < numberOfBins; i++)
            {
                switch (binSize)
                {
                    case historyBinSize.day:
                        end = fromDate.AddDays(1);
                        break;

                    case historyBinSize.week:
                        end = fromDate.AddDays(7);
                        break;
                    case historyBinSize.twoWeeks:
                        end = fromDate.AddDays(14);
                        break;
                    case historyBinSize.ThreeYears:
                        end = fromDate.AddYears(3);
                        break;

                    case historyBinSize.FiveYears:
                        end = fromDate.AddYears(3);
                        break;
                    case historyBinSize.month:
                        end = fromDate.AddMonths(1);
                        break;
                    case historyBinSize.quartal:
                        end = fromDate.AddMonths(3);

                        break;
                    case historyBinSize.year:
                        end = fromDate.AddYears(1);
                        break;
                    default:
                        break;
                }

            }

            return end;
        }

        public static TimeSpan ToTimeSpan(this historyBinSize binSize, DateTime fromDate, Int32 numberOfBins = 1)
        {
            DateTime end = ToEndDateTime(binSize, fromDate, numberOfBins);

            return end.Subtract(fromDate);
        }


    }
}