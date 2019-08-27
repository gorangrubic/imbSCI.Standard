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
    public class WordPressChartTool
    {

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
                                "[wp_charts title=\"{{{title}}}\" type=\"doughnut\" align=\"{{{align}}}\" margin=\"{{{margin}}}\" data=\"{{{data}}}\" colors=\"{{{colors}}}\"]");
                            _chartTemplate.Add(chartTypeEnum.line,
                                "[wp_charts title=\"{{{title}}}\" type=\"linechart\" align=\"{{{align}}}\" margin=\"{{{margin}}}\" datasets=\"{{{data}}}\" labels=\"{{{labels}}}\" colors=\"{{{colors}}}\"]");
                            _chartTemplate.Add(chartTypeEnum.bar,
                                "[wp_charts title=\"{{{title}}}\" type=\"barchart\" align=\"{{{align}}}\" margin=\"{{{margin}}}\" datasets=\"{{{data}}}\" labels=\"{{{labels}}}\" colors=\"{{{colors}}}\"]");
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
            Dictionary<String, String> output = new Dictionary<string, string>();
            output.Add("margin", "5px 20px");
            output.Add("align", "alignleft");
            output.Add("colors", "");
            output.Add("data", "");
            output.Add("labels", "");
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


        public String buildOneDimensionalChart<T>(chartTypeEnum chartType, String title, List<T> source, Func<T, String> label, Func<T, String> value, Func<T, String> color = null)
        {
            var fields = GetDefaultValues();

            fields.Add("title", title);

            //List<String> color_set = new List<string>() { "69D2E7", "#E0E4CC", "#F38630", "#96CE7F", "#CEBC17", "#CE4264" };
            circularSelector<String> color_selector = new circularSelector<String>("69D2E7", "#E0E4CC", "#F38630", "#96CE7F", "#CEBC17", "#CE4264");


            foreach (T item in source)
            {
                String label_str = label(item);
                String value_str = value(item);
                String color_str = "";
                if (color == null)
                {
                    color_str = color_selector.next();
                }
                else
                {
                    color_str = color(item);
                }
                fields["colors"] = fields["colors"].add(color_str, ",");
                fields["data"] = fields["data"].add(value_str, ",");
                fields["labels"] = fields["labels"].add(label_str, ",");
            }
            stringTemplate template = new stringTemplate(chartTemplate[chartType]);
            return template.applyToContent(fields);
        }

        public String buildTwoDimensionalChart<T>(chartTypeEnum chartType, String title, List<T> source, Func<T, String> label, Func<T, String> color = null, params Func<T, String>[] value)
        {
            var fields = GetDefaultValues();

            fields.Add("title", title);

            //List<String> color_set = new List<string>() { "69D2E7", "#E0E4CC", "#F38630", "#96CE7F", "#CEBC17", "#CE4264" };

            List<String> value_str = new List<string>();
            foreach (var f in value)
            {
                value_str.Add("");//.Add(f(item));
            }


            foreach (T item in source)
            {
                String label_str = label(item);

                for (int i = 0; i < value.Length; i++)
                {
                    value_str[i] = value_str[i].add(value[i](item), ",");
                }

                //String value_str = value(item);
                String color_str = "";
                if (color == null)
                {
                    color_str = color_selector.next();
                }
                else
                {
                    color_str = color(item);
                }
                fields["colors"] = fields["colors"].add(color_str, ",");

                fields["labels"] = fields["labels"].add(label_str, ",");
            }

            foreach (var f in value_str)
            {
                fields["data"] = fields["data"].add(f, " next ");
            }



            stringTemplate template = new stringTemplate(chartTemplate[chartType]);
            return template.applyToContent(fields);
        }
    }
}