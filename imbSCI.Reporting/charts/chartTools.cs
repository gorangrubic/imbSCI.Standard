// --------------------------------------------------------------------------------------------------------------------
// <copyright file="chartTools.cs" company="imbVeles" >
//
// Copyright (C) 2018 imbVeles
//
// This program is free software: you can redistribute it and/or modify
// it under the +terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/.
// </copyright>
// <summary>
// Project: imbSCI.Reporting
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Reporting.charts
{
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.reporting.extensions;
    using imbSCI.Data;
    using imbSCI.Reporting.charts.core;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;

    public static class chartTools
    {
        /// <summary>
        /// Builds the chart string for <c>C3js</c> rendering
        /// </summary>
        /// <param name="chartType">Type of the chart.</param>
        /// <param name="features">The features.</param>
        /// <param name="data">The data.</param>
        /// <param name="typesForSeries">The types for series.</param>
        /// <returns></returns>
        public static string buildChart(chartTypeEnum chartType, chartFeatures features, DataTable data, chartSizeEnum size, chartTypeEnum typesForSeries = chartTypeEnum.none)
        {
            if (features != chartFeatures.none)
            {
                features |= chartFeatures.bindto;
            }

            PropertyCollection pc = new PropertyCollection();

            string obj_id = data.TableName.getCleanPropertyName();

            pc.Add(chartDataColumnEnum.chart_bindto, obj_id);

            pc.Add(chartDataColumnEnum.chart_type, chartType.ToString());

            switch (size)
            {
                case chartSizeEnum.big500x1000:
                    pc.Add(chartDataColumnEnum.chart_height, 500);
                    pc.Add(chartDataColumnEnum.chart_width, 1000);
                    break;

                case chartSizeEnum.half300x500:
                    pc.Add(chartDataColumnEnum.chart_height, 300);
                    pc.Add(chartDataColumnEnum.chart_width, 500);
                    break;

                case chartSizeEnum.mid300x1000:
                    pc.Add(chartDataColumnEnum.chart_height, 300);
                    pc.Add(chartDataColumnEnum.chart_width, 1000);
                    break;

                case chartSizeEnum.mini200x380:
                    pc.Add(chartDataColumnEnum.chart_height, 200);
                    pc.Add(chartDataColumnEnum.chart_width, 380);
                    break;

                default:
                    pc.Add(chartDataColumnEnum.chart_height, 300);
                    pc.Add(chartDataColumnEnum.chart_width, 1000);
                    break;
            }

            string output_t = masterTemplate;

            if (!features.HasFlag(chartFeatures.withoutHtml))
            {
                output_t = wrapTemplate.Replace("{{{" + nameof(chartDataColumnEnum.chart_wrap) + "}}}", output_t);
            }

            if (features.HasFlag(chartFeatures.skipFirstRow))
            {
                data.Rows.RemoveAt(0);
            }

            string dataInsert = data.buildDataInsertHorizontaly();

            string innerInsert = "";

            List<chartTypeEnum> ts = typesForSeries.getEnumListFromFlags<chartTypeEnum>();
            int d = 2;
            foreach (chartTypeEnum t in ts)
            {
                if (t != chartTypeEnum.none)
                {
                    innerInsert += "data" + d.ToString() + ": '" + t.ToString() + "', " + Environment.NewLine;
                }
                d++;
            }
            string typesLine = @"types: { {{{chart_types}}} } ".Replace("{{{" + chartDataColumnEnum.chart_types + "}}}", innerInsert);
            if (typesForSeries != chartTypeEnum.none)
            {
                innerInsert = typesLine;
            }

            List<chartFeatures> ls = features.getEnumListFromFlags<chartFeatures>();
            foreach (chartFeatures ch in ls)
            {
                if (featureTemplates.ContainsKey(ch))
                {
                    innerInsert = innerInsert.add(featureTemplates[ch], "," + Environment.NewLine);
                }
            }

            pc.Add(chartDataColumnEnum.chart_inner, innerInsert);
            pc.Add(chartDataColumnEnum.chart_data, dataInsert);

            string filename = obj_id.add("csv", ".");
            pc.add(chartDataColumnEnum.chart_url, filename);

            string output = output_t.applyToContent(false, pc);

            return output;
        }

        /// <summary>
        /// Builds the data insert horizontaly.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns></returns>
        public static string buildDataInsertHorizontaly(this DataTable dt)
        {
            StringBuilder sb = new StringBuilder();

            List<string> columnNames = dt.Columns.Cast<DataColumn>().
                Select(column => column.ColumnName).ToList();

            List<string> lines = new List<string>();

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                lines.Add("");
                lines[i] = "['" + columnNames[i] + "'";
                foreach (DataRow row in dt.Rows)
                {
                    lines[i] = lines[i].add(row[dt.Columns[i]].toStringSafe(), ",");
                }
                lines[i] = lines[i] + "]";
                if (i < dt.Columns.Count - 1)
                {
                    lines[i] = lines[i] + ",";
                }
            }

            string ins = "";
            for (int i = 0; i < lines.Count; i++)
            {
                sb.AppendLine(lines[i] + ins);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Builds the CSV of datatable in vertical arangement
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns>CSV value</returns>
        public static string buildDataInsertVertically(this DataTable dt)
        {
            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in dt.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                sb.AppendLine(string.Join(",", fields));
            }
            return sb.ToString();
        }

        private static string _wrapTemplate;

        /// <summary> </summary>
        internal static string wrapTemplate
        {
            get
            {
                if (_wrapTemplate.isNullOrEmpty())
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("<div id='{{{chart_bindto}}}'></div>");
                    sb.AppendLine("<script type=\"text/javascript\">");
                    sb.AppendLine("{{{chart_wrap}}}");
                    //sb.AppendLine("    bindto: '#{{{chart_bindto}}}',");
                    //sb.AppendLine("    data: {");
                    //sb.AppendLine("        {{{chart_data}}}");
                    //sb.AppendLine("    },");
                    //sb.AppendLine(@"    types: { '{{{chart_type}}}',
                    //    } ");
                    //sb.AppendLine("    {{{chart_template}}}");

                    //sb.AppendLine("    });");
                    sb.AppendLine("setTimeout( function () { chart.resize({ height: {{{chart_height}}}, width: {{{" + nameof(chartDataColumnEnum.chart_width) + "}}} }) } , 500); ");
                    sb.AppendLine("</script>");

                    _wrapTemplate = sb.ToString();
                }
                return _wrapTemplate;
            }
            set
            {
                _wrapTemplate = value;
            }
        }

        private static string _masterTemplate;

        /// <summary> </summary>
        internal static string masterTemplate
        {
            get
            {
                if (_masterTemplate.isNullOrEmpty())
                {
                    StringBuilder sb = new StringBuilder();

                    // sb.AppendLine("<div id='{{{chart_bindto}}}'></div>");
                    // sb.AppendLine("<script type=\"text/javascript\">");
                    sb.AppendLine("var chart = c3.generate({");
                    sb.AppendLine(@"   bindto: '#{{{chart_bindto}}}',");
                    sb.AppendLine(@"   type: '{{{chart_type}}}',");
                    sb.AppendLine("    data: {");
                    sb.AppendLine("    columns: [");
                    sb.AppendLine("        {{{chart_data}}}");
                    sb.AppendLine("    ] },");

                    sb.AppendLine("    {{{chart_inner}}}");
                    sb.AppendLine("    });");
                    // sb.AppendLine("</script>");

                    _masterTemplate = sb.ToString();
                }
                return _masterTemplate;
            }
            set
            {
                _masterTemplate = value;
            }
        }

        private static Dictionary<chartFeatures, string> _featureTemplates;

        /// <summary>
        /// static and autoinitiated object
        /// </summary>
        public static Dictionary<chartFeatures, string> featureTemplates
        {
            get
            {
                if (_featureTemplates == null)
                {
                    _featureTemplates = new Dictionary<chartFeatures, string>();
                    _featureTemplates.Add(chartFeatures.showData, "");

                    _featureTemplates.Add(chartFeatures.urlJSON, @"url: '{{{chart_url}}}', mimeType: 'json'");

                    _featureTemplates.Add(chartFeatures.urlCSV, @"url: '{{{chart_url}}}'");

                    _featureTemplates.Add(chartFeatures.axisY2, @"axes: { data2: 'y2'}");

                    _featureTemplates.Add(chartFeatures.showY1AxisLabel, @" y: {
        label: {
          text: 'Y axis',
          position: 'outer-middle'
        }
      }");

                    _featureTemplates.Add(chartFeatures.showY2AxisLabel, @" y2: {
        label: {
          text: 'Y2 axis',
          position: 'outer-middle'
        }
      }");
                    _featureTemplates.Add(chartFeatures.bindto, "bindto: '#{{{chart_bindto}}}',");
                }
                return _featureTemplates;
            }
        }

        private static Dictionary<chartTypeEnum, string> _templates;

        #region imbObject Property <Dictionary<chartTypeEnum, String>> templates

        /// <summary>
        /// imbControl property templates tipa Dictionary{chartTypeEnum, String}
        /// </summary>
        public static Dictionary<chartTypeEnum, string> templates
        {
            get
            {
                if (_templates == null) _templates = new Dictionary<chartTypeEnum, string>();

                _templates.Add(chartTypeEnum.bar, @"bar: {
        width: {
                    ratio: 0.5
        }
            }");

                _templates.Add(chartTypeEnum.pie, @"        onclick: function (d, i) { console.log('onclick', d, i); },
        onmouseover: function(d, i) { console.log('onmouseover', d, i); }, onmouseout: function(d, i) { console.log('onmouseout', d, i); }");

                _templates.Add(chartTypeEnum.gauge, @"        onclick: function (d, i) { console.log('onclick', d, i); },
        onmouseover: function(d, i) { console.log('onmouseover', d, i); }, onmouseout: function(d, i) { console.log('onmouseout', d, i); }");

                return _templates;
            }
            set
            {
                _templates = value;
            }
        }

        #endregion imbObject Property <Dictionary<chartTypeEnum, String>> templates
    }
}