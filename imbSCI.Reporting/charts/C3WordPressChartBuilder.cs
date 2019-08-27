using imbSCI.Core.extensions.io;
using imbSCI.Core.extensions.table;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Data;
using imbSCI.DataComplex.extensions.data.operations;
using imbSCI.Reporting.charts.core;
using imbSCI.Reporting.charts.model;
using System;
using System.Collections.Generic;
using System.Data;

namespace imbSCI.Reporting.charts
{

    /// <summary>
    /// Builder for C3 charts for Gutenberg shortcode block in WordPress page-post content
    /// </summary>
    public class C3WordPressChartBuilder
    {

        /// <summary>
        /// Gets or sets the shortcode tool.
        /// </summary>
        /// <value>
        /// The shortcode tool.
        /// </value>
        public ShortcodeJSInsertTool shortcodeTool { get; protected set; } = new ShortcodeJSInsertTool();

        /// <summary>
        /// Builds the chart.
        /// </summary>
        /// <param name="chartType">Type of the chart.</param>
        /// <param name="features">The features.</param>
        /// <param name="data">The data.</param>
        /// <param name="size">The size.</param>
        /// <param name="typesForSeries">The types for series.</param>
        /// <returns></returns>
        public String BuildChart(chartTypeEnum chartType, chartFeatures features, DataTable data, chartSizeEnum size, chartTypeEnum typesForSeries = chartTypeEnum.none)
        {
            List<chartFeatures> featureList = features.getEnumListFromFlags<chartFeatures>();
            if (!featureList.Contains(chartFeatures.withoutHtml)) features |= chartFeatures.withoutHtml;

            String js = chartTools.buildChart(chartType, features, data, size, typesForSeries, "{0}");

            return shortcodeTool.Create(js, "", null);
        }

        public chartSizeEnum ChartSize = chartSizeEnum.half300x500;


        



        public String buildOneDimensionalChart(chartTypeEnum chartType, String title, Dictionary<String, Double> source, String labelColumnName = "Label", String valueColumnName = "Value", String valueColumnFormat = "")
        {
            DataTable finalDataTable = new DataTable(); // dataTable.GetSubColumnTable(-1, columns);
            finalDataTable.SetTitle(title);

            DataColumn labelColumn = finalDataTable.Columns.Add(labelColumnName.getCleanPropertyName(), typeof(String)).SetHeading(labelColumnName);
            DataColumn valueColumn = finalDataTable.Columns.Add(valueColumnName.getCleanPropertyName(), typeof(Double)).SetFormat(valueColumnFormat);

            foreach (var pair in source)
            {
                var dr = finalDataTable.NewRow();
                dr[labelColumn] = pair.Key;
                dr[valueColumn] = pair.Value;
            }

            chartFeatures features = chartFeatures.transposeTable | chartFeatures.withoutHtml;

            String chartCode = BuildChart(chartType, features, finalDataTable, ChartSize);

            return chartCode;
        }

        public String buildTwoDimensionalChart(chartTypeEnum chartType, DataTable dataTable, String[] columns)
        {
            DataTable finalDataTable = dataTable.GetSubColumnTable(-1, columns);

            chartFeatures features = chartFeatures.transposeTable | chartFeatures.withoutHtml;

            String chartCode = BuildChart(chartType, features, finalDataTable, ChartSize);

            return chartCode;
        }

        public C3WordPressChartBuilder()
        {

        }
    }
}