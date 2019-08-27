using imbSCI.BusinessData.Metrics.Core;
using System;

namespace imbSCI.BusinessData.Metrics
{
    /// <summary>
    /// Import and/or export turnover, by country
    /// </summary>
    /// <seealso cref="imbSCI.BusinessData.Metrics.Core.AnnualMetricsBase" />
    public class InternationalTradeByCountry : AnnualMetricsBase
    {
        public InternationalTradeByCountry()
        {
        }

        public override string GetUID()
        {
            return base.GetUID() + "_" + Country + "_" + Direction.ToString();
        }

        /// <summary>
        /// Gets or sets the turnover.
        /// </summary>
        /// <value>
        /// The turnover.
        /// </value>
        public Decimal Turnover { get; set; } = 0;



        public Decimal January { get; set; } = 0;
        public Decimal February { get; set; } = 0;
        public Decimal March { get; set; } = 0;
        public Decimal April { get; set; } = 0;
        public Decimal May { get; set; } = 0;
        public Decimal June { get; set; } = 0;
        public Decimal July { get; set; } = 0;
        public Decimal August { get; set; } = 0;
        public Decimal September { get; set; } = 0;
        public Decimal October { get; set; } = 0;
        public Decimal November { get; set; } = 0;
        public Decimal December { get; set; } = 0;

        /// <summary>
        /// Gets or sets the country name
        /// </summary>
        /// <value>
        /// The country.
        /// </value>
        public String Country { get; set; } = "";

        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        /// <value>
        /// The direction.
        /// </value>
        public InternationalTradeDirection Direction { get; set; } = InternationalTradeDirection.unknown;
    }
}