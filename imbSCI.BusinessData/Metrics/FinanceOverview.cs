using imbSCI.BusinessData.Metrics.Core;
using imbSCI.Core.attributes;
using imbSCI.Core.reporting.zone;
using imbSCI.Data.enums.fields;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace imbSCI.BusinessData.Metrics
{
    /// <summary>Short finance overview for particular year</summary>
    /// <seealso cref="imbSCI.BusinessData.Metrics.Core.AnnualMetricsBase" />
    public class FinanceOverview : AnnualMetricsBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FinanceOverview"/> class - used for XML serialization
        /// </summary>
        public FinanceOverview()
        {
        }

        [DisplayName("Employees")]
        [Description("Number of employees in the company")]
        [imb(imbAttributeName.measure_letter, "Person")]
        [imb(imbAttributeName.reporting_valueformat, "0")]
        [imb(imbAttributeName.reporting_columnWidth, 10)]
        public Int32 EmployeeCount { get; set; } = 0;

        protected Decimal _Cash = default(System.Decimal);

        [Category("Assets")]
        [DisplayName("Cash")]
        [Description("Cash available at company accounts")]
        [imb(imbAttributeName.measure_letter, "C")]
        [imb(imbAttributeName.reporting_valueformat, "0,0.00")]
        [imb(imbAttributeName.reporting_columnWidth, 20)]
        [imb(templateFieldDataTable.col_alignment, textCursorZoneCorner.Right)]
        /// <summary>
        /// Content: numeric ValueType: Decimal
        /// </summary>
        public Decimal Cash
        {
            get
            {
                return _Cash;
            }
            set
            {
                _Cash = value;
            }
        }

        protected Decimal _Profit = default(System.Decimal);

        [Category("Assets")]
        [DisplayName("Profit")]
        [Description("Earnings before Interest and taxes (EBIT)")]
        [imb(imbAttributeName.measure_letter, "EBIT")]
        [imb(imbAttributeName.reporting_valueformat, "0,0.00")]
        [imb(imbAttributeName.reporting_columnWidth, 20)]
        [imb(templateFieldDataTable.col_alignment, textCursorZoneCorner.Right)]
        /// <summary>
        /// Content: numeric ValueType: Decimal
        /// </summary>
        public Decimal Profit
        {
            get
            {
                return _Profit;
            }
            set
            {
                _Profit = value;
            }
        }

        protected Decimal _NetWorkingCapital = default(System.Decimal);

        [Category("Balance")]
        [DisplayName("Net working capital")]
        [Description("Net working capital is the aggregate amount of all current assets and current liabilities. ")]
        [imb(imbAttributeName.reporting_escapeoff)]
        [imb(imbAttributeName.measure_letter, "NWC")]
        [imb(imbAttributeName.reporting_valueformat, "0,0.00")]
        [imb(imbAttributeName.reporting_columnWidth, 20)]
        [imb(imbAttributeName.basicColor, "Color [LightGray]")]
        [imb(templateFieldDataTable.col_alignment, textCursorZoneCorner.Right)]
        /// <summary>
        /// Content: numeric ValueType: Decimal
        /// </summary>
        public Decimal NetWorkingCapital
        {
            get
            {
                return _NetWorkingCapital;
            }
            set
            {
                _NetWorkingCapital = value;
            }
        }

        protected Decimal _InputLiabilities = default(System.Decimal);

        [Category("Balance")]
        [DisplayName("Trade receivables")]
        [Description("Trade receivables are amounts billed by a business to its customers when it delivers goods or services to them in the ordinary course of business.")]
        [imb(imbAttributeName.measure_letter, "TR")]
        [imb(imbAttributeName.reporting_valueformat, "0,0.00")]
        [imb(imbAttributeName.reporting_columnWidth, 20)]
        [imb(imbAttributeName.basicColor, "Color [LightGray]")]
        [imb(templateFieldDataTable.col_alignment, textCursorZoneCorner.Right)]
        /// <summary>
        /// Content: numeric ValueType: Decimal
        /// </summary>
        public Decimal InputLiabilities
        {
            get
            {
                return _InputLiabilities;
            }
            set
            {
                _InputLiabilities = value;
            }
        }

        protected Decimal _OutputLiabilities = default(System.Decimal);

        [Category("Balance")]
        [DisplayName("Trade payable")]
        [Description("A trade payable is an amount billed to a company by its suppliers for goods delivered to or services consumed by the company in the ordinary course of business. ")]
        [imb(imbAttributeName.measure_letter, "TP")]
        [imb(imbAttributeName.reporting_valueformat, "0,0.00")]
        [imb(imbAttributeName.reporting_columnWidth, 20)]
        [imb(templateFieldDataTable.col_alignment, textCursorZoneCorner.Right)]
        /// <summary>
        /// Content: numeric ValueType: Decimal
        /// </summary>
        public Decimal OutputLiabilities
        {
            get
            {
                return _OutputLiabilities;
            }
            set
            {
                _OutputLiabilities = value;
            }
        }
    }
}