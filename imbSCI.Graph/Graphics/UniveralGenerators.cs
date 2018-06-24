using imbSCI.Core.extensions.table;
using imbSCI.Core.math.range.histogram;
using imbSCI.Graph.Graphics.SvgChart;
using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;

namespace imbSCI.Graph.Graphics
{
    /// <summary>
    /// Extension methods for easy creation of SVG charts
    /// </summary>
    public static class UniversalGenerators
    {
        /// <summary>
        /// Makes data table for chart
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="titleSelector">The title selector.</param>
        /// <param name="valueSelector">The value selector.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static DataTable GetChartDataTable<T>(this IEnumerable<T> source, Func<T, String> titleSelector, Func<T, Double> valueSelector, String name = "ChartData")
        {
            DataTable output = new DataTable();
            output.SetTitle(name);

            var cn_name = output.Columns.Add(histogramModel.DEFAULT_COLUMN_NAME);
            var cn_value = output.Columns.Add(histogramModel.DEFAULT_COLUMN_VALUE);

            foreach (var bin in source)
            {
                var dr = output.NewRow();

                dr[cn_name] = titleSelector(bin);
                dr[cn_value] = valueSelector(bin);

                output.Rows.Add(dr);
            }

            return output;
        }

        /// <summary>
        /// Gets the SVG chart: <see cref="Histogram3DChart"/>, <see cref="HistogramChart"/> or <see cref="LinearChart"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="titleSelector">The title selector.</param>
        /// <param name="valueSelector">The value selector.</param>
        /// <param name="is3D">if set to <c>true</c> [is3 d].</param>
        /// <param name="isBarChart">if set to <c>true</c> [is bar chart].</param>
        /// <param name="depth">The depth.</param>
        /// <returns>SVG string code</returns>
        public static String GetSVGChart<T>(this IEnumerable<T> source, Func<T, String> titleSelector, Func<T, Double> valueSelector, Boolean is3D = false, Boolean isBarChart = false, short depth = 10)
        {
            DataTable data = source.GetChartDataTable<T>(titleSelector, valueSelector);

            if (isBarChart)
            {
                if (!is3D)
                {
                    return new HistogramChart(500, 360, data.Rows.Count).GenerateChart(data, histogramModel.DEFAULT_COLUMN_NAME, histogramModel.DEFAULT_COLUMN_VALUE).OuterXml;
                }
                else
                {
                    return new Histogram3DChart(500, 360, data.Rows.Count, depth).GenerateChart(data, histogramModel.DEFAULT_COLUMN_NAME, histogramModel.DEFAULT_COLUMN_VALUE).OuterXml;
                }
            }
            else
            {
                return new LinearChart(500, 360, data.Rows.Count).GenerateChart(data, histogramModel.DEFAULT_COLUMN_NAME, histogramModel.DEFAULT_COLUMN_VALUE).OuterXml;
            }
        }

        /// <summary>
        /// Gets the SVG pie or doughnut chart.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="titleSelector">The title selector.</param>
        /// <param name="valueSelector">The value selector.</param>
        /// <param name="is3D">if set to <c>true</c> [is3 d].</param>
        /// <param name="isDoughnut">if set to <c>true</c> [is doughnut].</param>
        /// <returns>SVG string code</returns>
        public static String GetSVGPieChart<T>(this IEnumerable<T> source, Func<T, String> titleSelector, Func<T, Double> valueSelector, Boolean is3D = false, Boolean isDoughnut = false)
        {
            DataTable data = source.GetChartDataTable<T>(titleSelector, valueSelector);

            if (isDoughnut)
            {
                if (!is3D)
                {
                    return new DoughnutChart(500, 360, data.Rows.Count).GenerateChart(data, histogramModel.DEFAULT_COLUMN_NAME, histogramModel.DEFAULT_COLUMN_VALUE).OuterXml;
                }
                else
                {
                    return new Doughnut3DChart(500, 360, data.Rows.Count).GenerateChart(data, histogramModel.DEFAULT_COLUMN_NAME, histogramModel.DEFAULT_COLUMN_VALUE).OuterXml;
                }
            }
            else
            {
                if (!is3D)
                {
                    return new PieChart(500, 360, data.Rows.Count).GenerateChart(data, histogramModel.DEFAULT_COLUMN_NAME, histogramModel.DEFAULT_COLUMN_VALUE).OuterXml;
                }
                else
                {
                    return new Pie3DChart(500, 360, data.Rows.Count).GenerateChart(data, histogramModel.DEFAULT_COLUMN_NAME, histogramModel.DEFAULT_COLUMN_VALUE).OuterXml;
                }
            }
        }

        /// <summary>
        /// Gets the SVG histogram.
        /// </summary>
        /// <param name="model">The histogram model</param>
        /// <param name="is3D">if set to <c>true</c> it will be 3D</param>
        /// <param name="depth">The depth.</param>
        /// <returns></returns>
        public static String GetSVGHistogram(this histogramModel model, Boolean is3D = false, short depth = 10)
        {
            if (!is3D)
            {
                HistogramChart output = new HistogramChart(500, 360, model.targetBins);
                XmlDocument xml = output.GenerateChart(model.GetDataTableForFrequencies(), histogramModel.DEFAULT_COLUMN_NAME, histogramModel.DEFAULT_COLUMN_VALUE);

                return xml.OuterXml;
            }
            else
            {
                Histogram3DChart output = new Histogram3DChart(500, 360, model.targetBins, depth);
                XmlDocument xml = output.GenerateChart(model.GetDataTableForFrequencies(), histogramModel.DEFAULT_COLUMN_NAME, histogramModel.DEFAULT_COLUMN_VALUE);

                return xml.OuterXml;
            }
        }
    }
}