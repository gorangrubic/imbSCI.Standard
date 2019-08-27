using System;
using System.Collections.Generic;

namespace imbSCI.Core.math.measurement
{
    /// <summary>
    /// Additional operations over objects inheriting <see cref="MetricsBase"/>
    /// </summary>
    public static class MetricsExtensions
    {


        private static Object _CachedMetricsDefinitions_lock = new Object();
        private static Dictionary<String, MetricsDictionaries> _CachedMetricsDefinitions;
        /// <summary>
        /// Metric types definitions cached for faster execution of <see cref="MetricsBase.Init"/>
        /// </summary>
        internal static Dictionary<String, MetricsDictionaries> CachedMetricsDefinitions
        {
            get
            {
                if (_CachedMetricsDefinitions == null)
                {
                    lock (_CachedMetricsDefinitions_lock)
                    {

                        if (_CachedMetricsDefinitions == null)
                        {
                            _CachedMetricsDefinitions = new Dictionary<String, MetricsDictionaries>();
                            // add here if any additional initialization code is required
                        }
                    }
                }
                return _CachedMetricsDefinitions;
            }
        }


        private static Object StoreMetricsLock = new Object();



        /// <summary>
        /// Stores the metrics definition.
        /// </summary>
        /// <param name="source">The source.</param>
        internal static void StoreMetricsDefinition(MetricsDictionaries source)
        {
            Type t = source.GetType();
            if (!MetricsExtensions.CachedMetricsDefinitions.ContainsKey(t.FullName))
            {

                lock (StoreMetricsLock)
                {

                    if (!MetricsExtensions.CachedMetricsDefinitions.ContainsKey(t.FullName))
                    {

                        MetricsDictionaries cache = new MetricsDictionaries();
                        source.SetTo(cache);
                        MetricsExtensions.CachedMetricsDefinitions.Add(t.FullName, cache);
                    }

                }

            }
        }






        /// <summary>
        /// Sums the specified metrics.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="metrics">The metrics.</param>
        /// <returns></returns>
        public static T Sum<T>(this IEnumerable<T> metrics) where T : MetricsBase, new()
        {
            T output = new T();

            foreach (T m in metrics)
            {
                output.Plus(m);
            }

            return output;
        }
    }
}