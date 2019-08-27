using imbSCI.Core.math.range.finder;
using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.Core.math.range.variants
{

    /// <summary>
    /// Utility used to generate variants of settings/parameter objects. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class instanceGradient<T> where T:class, new()
    {

        public rangeFinderCollectionForMetrics<T> rangeFinderCollection { get; set; } = new rangeFinderCollectionForMetrics<T>();

        /// <summary>
        /// Initializes <see cref="instanceGradient{T}"/> with marginal values, taken from . 
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        public instanceGradient(T from, T to)
        {
            rangeFinderCollection.Learn(from);
            rangeFinderCollection.Learn(to);

        }


        public List<T> GetGradientSteps(Int32 steps)
        {
            List<T> output = new List<T>();

            
            for (int i = 0; i <= steps; i++)
            {
                Double posInRange = i.GetRatio(steps);

                T step = new T();

                rangeFinderCollection.SetValuesForRangePosition(posInRange, step);

                output.Add(step);

            }

            return output;
        }

    }
}
