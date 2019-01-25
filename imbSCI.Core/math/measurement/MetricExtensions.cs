using System.Collections.Generic;

namespace imbSCI.Core.math.measurement
{
    public static class MetricExtensions
    {

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