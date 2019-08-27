using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.io;
using imbSCI.Core.extensions.table;
using imbSCI.Core.math.classificationMetrics;
using imbSCI.Core.reporting.template;
using imbSCI.Data;
using imbSCI.Data.collection.special;
using imbSCI.Reporting.charts.core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace imbSCI.Reporting.charts
{
public class WordPressKoplasChartTool
    {

        public Int32 currentIdIndex { get; set; } = 0;


        private static Object _chartTemplate_lock = new Object();
        private static Dictionary<chartTypeEnum, String> _chartTemplate;

        circularSelector<String> color_selector = new circularSelector<String>("69D2E7", "#E0E4CC", "#F38630", "#96CE7F", "#CEBC17", "#CE4264");


        /// <summary>
        /// static and autoinitiated object
        /// </summary>
        public static Dictionary<chartTypeEnum, String> chartTemplate
        {
            get
            {
                if (_chartTemplate == null)
                {
                    lock (_chartTemplate_lock)
                    {

                        if (_chartTemplate == null)
                        {
                            _chartTemplate = new Dictionary<chartTypeEnum, String>();
                            _chartTemplate.Add(chartTypeEnum.donut,
                                "<!--wp:shortcode-->[dounat id=\"{{{title}}}\" title=\"{{{title}}}\"]{{{data}}}[/chart]<!--/wp:shortcode-->");

                            _chartTemplate.Add(chartTypeEnum.line, "<!--wp:shortcode-->[chart id=\"{{{title}}}\" title=\"{{{title}}}\"]{{{data}}}[/chart]<!--/wp:shortcode-->");

                            _chartTemplate.Add(chartTypeEnum.bar,
                                "<!--wp:shortcode-->[bars id=\"{{{title}}}\" title=\"{{{title}}}\"]{{{data}}}[/bars]<!--/wp:shortcode-->");
                            // add here if any additional initialization code is required
                        }
                    }
                }
                return _chartTemplate;
            }
        }

        public Dictionary<String, String> attributes = new Dictionary<string, string>();

        public Dictionary<String, String> GetDefaultValues()
        {
            currentIdIndex++;
            Dictionary<String, String> output = new Dictionary<string, string>();
            output.Add("margin", "5px 20px");
            output.Add("align", "alignleft");
            output.Add("colors", "");
            output.Add("data", "");
            output.Add("labels", "");
            output.Add("id", currentIdIndex.ToString("D5"));
            foreach (var pair in attributes)
            {
                if (!output.ContainsKey(pair.Key)) output.Add(pair.Key, pair.Value);
                else
                {
                    output[pair.Key] = pair.Value;
                }
            }


            return output;
        }


        public String buildOneDimensionalChart<T>(chartTypeEnum chartType, String title, Dictionary<String, Double> source)
        {
            var fields = GetDefaultValues();

            fields.Add("title", title);

            //List<String> color_set = new List<string>() { "69D2E7", "#E0E4CC", "#F38630", "#96CE7F", "#CEBC17", "#CE4264" };
            circularSelector<String> color_selector = new circularSelector<String>("69D2E7", "#E0E4CC", "#F38630", "#96CE7F", "#CEBC17", "#CE4264");


            foreach (var item in source)
            {

                String data = "columns: -:: ";

                String column = "-:: !" + item.Key + "!".add(item.Value.ToString(), ", ") + " ::-";

                fields["data"] = fields["data"].add(column, ", ");

                fields["data"] += " ::-";
            }
            stringTemplate template = new stringTemplate(chartTemplate[chartType]);
            return template.applyToContent(fields);
        }

        public String buildTwoDimensionalChart(chartTypeEnum chartType, DataTable dataTable, params String[] columns)
        {
            JSONChartDataTool tool = new JSONChartDataTool();

            String data = tool.DataToJ3CodeData(dataTable, columns);
            var fields = GetDefaultValues();

            fields.Add("title", dataTable.GetTitle().getFilename());
            fields["data"] = data;
            stringTemplate template = new stringTemplate(chartTemplate[chartType]);
            return template.applyToContent(fields);

        }
        /*
        public String buildTwoDimensionalChart<T>(chartTypeEnum chartType, String title, List<T> source, Func<T, String> label, Func<T, String> color = null, params Func<T, String>[] value)
        {
            var fields = GetDefaultValues();

            fields.Add("title", title);

            //List<String> color_set = new List<string>() { "69D2E7", "#E0E4CC", "#F38630", "#96CE7F", "#CEBC17", "#CE4264" };

            List<String> label_str = new List<string>();


            List<String> value_str = new List<string>();
            foreach (var f in value)
            {
                value_str.Add("");//.Add(f(item));
            }


            foreach (T item in source)
            {
                // = label(item);

                for (int i = 0; i < value.Length; i++)
                {
                    value_str[i] = value_str[i].add(value[i](item), ",");
                    label_str.Add(label(item));
                }

                ////String value_str = value(item);
                //String color_str = "";
                //if (color == null)
                //{
                //    color_str = color_selector.next();
                //}
                //else
                //{
                //    color_str = color(item);
                //}
                //fields["colors"] = fields["colors"].add(color_str, ",");


                //label_str.Add(label_str);
                //fields["labels"] = fields["labels"].add(label_str, ",");
            }

            String data = "columns: -:: ";

            for (int i2 = 0; i2 < label_str.Count; i2++)
            {
                String column = "-:: !" + label_str[i2] + "!".add(value_str[i2], ", ") + " ::-";

                fields["data"] = fields["data"].add(column, ", ");
            }

            stringTemplate template = new stringTemplate(chartTemplate[chartType]);
            String output = template.applyToContent(fields);
            return output;
        }*/
    }
}