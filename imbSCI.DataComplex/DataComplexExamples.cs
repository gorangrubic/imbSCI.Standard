using imbSCI.Core.attributes;
using imbSCI.Core.data.diagnostics;
using imbSCI.Core.enums;
using imbSCI.Core.extensions.text;
using imbSCI.Core.style.color;
using imbSCI.DataComplex.data;
using imbSCI.DataComplex.tables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;

namespace imbSCI.DataComplex
{
    /// <summary>Example, demonstrating data annotation with reporting, using generic, DataTable based, collection</summary>
    /// <seealso cref="imbSCI.DataComplex.data.TestMicroEnvironmentBase" />
    public class DataComplexExamples : TestMicroEnvironmentBase
    {
        /// <summary>
        /// This is a dummy data class, used for data annotation & reporting demonstration
        /// </summary>
        [imb(imbAttributeName.reporting_categoryOrder, "Group 1,Group 2")]
        [imb(imbAttributeName.basicColor, "#999999")]
        public class DataEntryTest
        {
            public static Random rnd { get; set; } = new Random();

            /// <summary>
            /// Constructor, setting some random values
            /// </summary>
            public DataEntryTest()
            {
                myProperty = imbStringGenerators.getRandomString(15);
                myPropertyRatio = rnd.NextDouble();

                isInState = rnd.NextDouble() > 0.5;
                BigNumber = Convert.ToInt32(rnd.NextDouble() * 200000);
            }

            /// <summary>  </summary>
            [Category("Group 2")]
            [DisplayName("Random string")] //
            [Description("Text property with random string generation attached")] // [imb(imbAttributeName.reporting_escapeoff)]
            [imb(imbAttributeName.measure_letter, "l")]
            [imb(imbAttributeName.reporting_columnWidth, "50")] // width can be specified as integer or string, no difference
            public String myProperty { get; set; } = default(String);

            /// <summary> Example of boolean property </summary>
            [Category("Group 1")]
            [DisplayName("Is In State")]
            [imb(imbAttributeName.viewPriority, 200)]
            [imb(imbAttributeName.measure_letter, "T")]
            [imb(imbAttributeName.basicColor, "#0066FF")] // you can use string hex (RGB or ARGB, both in 256 or 8 bit form)
            [imb(imbAttributeName.reporting_columnWidth, 10)] // width can be specified as integer or string, no difference
            [Description("Example of boolean property")]
            public Boolean isInState { get; set; } = true;

            /// <summary> Example of integer property </summary>
            [Category("Group 1")]
            [DisplayName("Big number")]
            [imb(imbAttributeName.measure_letter, "C")]
            [imb(imbAttributeName.measure_setUnit, "n")]
            [imb(imbAttributeName.menuPriority, 100)] // <-- menuPriorty is meant for menu declarations, but works here too
            [imb(imbAttributeName.basicColor, ColorWorks.ColorRed)] // This will not be applied, as "isInState" property already defined color for the group
            [imb(imbAttributeName.reporting_valueformat, "#,###")] // format is specified according to String.Format format
            [imb(imbAttributeName.measure_important, dataPointImportance.important)] // this will set "bold" for values in the report
            [Description("Some important big number, having set importance flag and number format")]
            public Int32 BigNumber { get; set; } = 150000;

            /// <summary> Example of integer property </summary>
            [Category("Group 3")]
            [DisplayName("Some number")]
            [imb(imbAttributeName.basicColor, ColorWorks.ColorRed)] // <- therefore, you can use constants from imbSCI.Core.style.color.ColorWorks
            [imb(imbAttributeName.reporting_valueformat, "#,###")] // format is specified according to String.Format format
            [Description("Group 3 member")]
            public Int32 SomeNumber { get; set; } = 15000;

            /// <summary> Ratio </summary>
            [Category("Group 3")]
            [DisplayName("myPropertyRatio")]
            [imb(imbAttributeName.measure_letter, "R")]
            [imb(imbAttributeName.measure_setUnit, "%")]
            [imb(imbAttributeName.viewPriority, 200)]
            [imb(imbAttributeName.reporting_valueformat, "P2")] // you can use Standard Numeric Formats
            [imb(imbAttributeName.basicColor, "#FFFFFF")] // <- this color will prevail the one defined at SomeNumber, because myPropertyRatio has higher priority in the group
            [Description("Ratio")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")][imb(imbAttributeName.reporting_escapeoff)]
            public Double myPropertyRatio { get; set; } = default(Double);
        }

        /// <summary>
        /// Creates generic DataTable collection, adds 5 rows and generates Excel and CSV file
        /// </summary>
        public void ExampleOne_DataTableDataAnnotation()
        {
            // creating typed DataTable collection, holding DataEntryTest class
            DataTableTypeExtended<DataEntryTest> dataTableTypeExtended = new DataTableTypeExtended<DataEntryTest>(nameof(DataEntryTest), nameof(ExampleOne_DataTableDataAnnotation));

            // adding five rows
            dataTableTypeExtended.AddRow(new DataEntryTest());
            dataTableTypeExtended.AddRow(new DataEntryTest());
            dataTableTypeExtended.AddRow(new DataEntryTest());
            dataTableTypeExtended.AddRow(new DataEntryTest());
            dataTableTypeExtended.AddRow(new DataEntryTest());

            // Generating and exporting report into Excel file
            DataTableForStatistics report = dataTableTypeExtended.GetReportAndSave(folderResults);
        }
    }
}