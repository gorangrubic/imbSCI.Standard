using imbSCI.Core.extensions.typeworks;
using imbSCI.Reporting.Html.Units.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace imbSCI.Reporting.Html.Units
{
    public static class HtmlUnitTools
    {

        public static String REGEX_TEMPLATE_START = "(\\<\\s*{0}\\s*\\>)";

        public static String REGEX_TEMPLATE_END = @"(\</\s*{0}\s*\>)";


        /// <summary>
        /// Designate string index position where a unit should be inserted
        /// </summary>
        /// <param name="UnitLocation">The unit location.</param>
        /// <param name="html">The HTML.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">UnitLocation - Location enum must have start or end bits set</exception>
        public static Int32 GetInsertPosition(HtmlUnitLocation UnitLocation, String html)
        {
            String needle = "";

            if (UnitLocation.HasFlag(HtmlUnitLocation.body))
            {
                needle = "body";
            }
            else if (UnitLocation.HasFlag(HtmlUnitLocation.head))
            {
                needle = "head";
            }
            String template = REGEX_TEMPLATE_START;

            if (UnitLocation.HasFlag(HtmlUnitLocation.start))
            {
                template = REGEX_TEMPLATE_START;
            }
            else if (UnitLocation.HasFlag(HtmlUnitLocation.end))
            {
                template = REGEX_TEMPLATE_END;
            }

            Regex search = new Regex(String.Format(template, needle), RegexOptions.IgnoreCase);
            var m = search.Match(html);

            if (m.Success)
            {

                if (UnitLocation.HasFlag(HtmlUnitLocation.start))
                {
                    return m.Index + m.Length;
                }
                else if (UnitLocation.HasFlag(HtmlUnitLocation.end))
                {
                    return m.Index;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(UnitLocation), "Location enum must have start or end bits set");
                    return -1;
                }
            }
            else
            {
                
                return -1;
            }
        }

        public static String Apply(this IHtmlUnit unit, String htmlDocument)
        {

            Int32 position = GetInsertPosition(unit.UnitLocation, htmlDocument);

            if (position > -1)
            {
                String unitRender = unit.Render();

                String before = htmlDocument.Substring(0, position);

                String after = htmlDocument.Substring(position);

                return before + unitRender + after;
            }
            else
            {
                throw new ArgumentOutOfRangeException("Position [" + unit.UnitLocation.ToString() + "] not found for [" + unit.GetType().GetCleanTypeName() + " of " + unit.Tag + "]");
                return htmlDocument;
            }


        }


        public static List<String> GetRenders(this IEnumerable<IHtmlUnit> items)
        {
            List<String> output = new List<string>();

            foreach (IHtmlUnit unit in items)
            {
                output.Add(unit.Render());
            }
            return output;
        }


        public static String Apply(this IEnumerable<IHtmlUnit> items, String htmlDocument)
        {
            foreach (IHtmlUnit unit in items)
            {
                htmlDocument = Apply(unit, htmlDocument);
            }
            return htmlDocument;
        }

    }
}