using imbSCI.BusinessData.Core;
using imbSCI.Core.attributes;
using imbSCI.Core.math.measurement;
using System;
using System.Xml.Serialization;

namespace imbSCI.BusinessData.Metrics.Core
{
    /// <summary>
    /// Base class for annual business metrics
    /// </summary>
    public abstract class AnnualMetricsBase : MetricsBase, IEquatable<IAnnualMetricsBase>, IAnnualMetricsBase
    {
        /// <summary>
        /// Gets UID used to identify matching entries
        /// </summary>
        /// <returns></returns>
        public virtual String GetUID()
        {
            return year.ToString();
        }

        /// <summary>
        /// Indicates whether the current object is equal by <see cref="GetUID()"/>to another object of the same type
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.
        /// </returns>
        public bool Equals(IAnnualMetricsBase other)
        {
            return GetUID() == other.GetUID();
        }

        public override bool Equals(object obj)
        {
            if (obj is IAnnualMetricsBase objI)
            {
                return Equals(objI);
            }
            return false;
        }

        /// <summary>
        /// Indicates whether the current object is equal by <see cref="GetUID()"/>to another object of the same type
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.
        /// </returns>
        public bool Equals(IRecordWithGetUID other)
        {
            return GetUID() == other.GetUID();
        }

        /// <summary>
        /// Year of the metrics
        /// </summary>
        /// <value>
        /// The year.
        /// </value>
        [imb(imbAttributeName.measure_excludeFromMetrics)]
        public Int32 year { get; set; } = 2019;

        [XmlIgnore]
        public DateTime PeriodStart
        {
            get
            {
                return new DateTime(year, 1, 1);
            }
        }

        /// <summary>Inclusive end of the period.</summary>
        /// <value>The period end.</value>
        [XmlIgnore]
        public DateTime PeriodEnd
        {
            get
            {
                return new DateTime(year, 12, 31);
            }
        }

        public AnnualMetricsBase()
        {
        }
    }
}