using imbSCI.Core.attributes;
using imbSCI.Core.extensions.table;
using imbSCI.Core.math.classificationMetrics;
using imbSCI.Core.math.range.finder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace imbSCI.Core.math.measurement
{
/// <summary>
    /// Used internally. 
    /// </summary>
    public class MetricsDictionaries
    {
        public MetricsDictionaries()
        {

        }

        public void SetTo(MetricsDictionaries target)
        {
            target.IntegersDict = IntegersDict;
            target.DoublesDict = DoublesDict;
            target.DecimalsDict = DecimalsDict;
            target.IgnoreComputationsDict = IgnoreComputationsDict;

            target.Integers = Integers;
            target.Doubles = Doubles;
            target.Decimals = Decimals;
            target.IgnoreComputations = IgnoreComputations;
        }

        protected Dictionary<String, PropertyInfo> IntegersDict { get; set; } = new Dictionary<String, PropertyInfo>();
        protected Dictionary<String, PropertyInfo> DoublesDict { get; set; } = new Dictionary<String, PropertyInfo>();
        protected Dictionary<String, PropertyInfo> DecimalsDict { get; set; } = new Dictionary<String, PropertyInfo>();

        protected Dictionary<String, PropertyInfo> IgnoreComputationsDict { get; set; } = new Dictionary<String, PropertyInfo>();

        protected List<PropertyInfo> Integers { get; set; } = new List<PropertyInfo>();
        protected List<PropertyInfo> Doubles { get; set; } = new List<PropertyInfo>();
        protected List<PropertyInfo> Decimals { get; set; } = new List<PropertyInfo>();


        /// <summary>
        /// List of properties that should be ignored in computation operations
        /// </summary>
        /// <value>
        /// The ignore computations.
        /// </value>
        protected List<PropertyInfo> IgnoreComputations { get; set; } = new List<PropertyInfo>();
    }
}