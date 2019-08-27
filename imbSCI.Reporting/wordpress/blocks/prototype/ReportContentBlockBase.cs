using imbSCI.Data;
using System;
using System.Text.RegularExpressions;

namespace imbSCI.Reporting.wordpress.blocks.prototype
{
    public abstract class ReportContentBlockBase
    {



        /// <summary>
        /// Initializes template strings and other settings
        /// </summary>
        public abstract void Deploy();

        public String outerTemplate { get; set; } = "{0}";

        public String innerTemplate { get; set; } = "{0}";

        /// <summary>
        /// Determines whether given template string is ready for consumption
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="numberOfPlaceholders">Required number of <see cref="string.Format(string, object)"/> placeholders. Leave it zero to skip the placeholder check</param>
        /// <returns>
        ///   <c>true</c> if [is template set] [the specified template]; otherwise, <c>false</c>.
        /// </returns>
        protected Boolean IsTemplateSet(String template, Int32 numberOfPlaceholders = 0)
        {
            if (template.isNullOrEmpty()) return false;
            if (template == "{0}") return false;

            if (numberOfPlaceholders > 0)
            {
                var mch = selectPlaceHolders.Matches(template);
                if (mch.Count >= numberOfPlaceholders)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;

        }

        public static Regex selectPlaceHolders = new Regex(@"\{([\d]*)\}");

        public String WrapInTemplate(String template, String content)
        {
            if (!IsTemplateSet(template)) return content;

            return String.Format(template, content);
        }


        //  public static Regex selectEscapedPlaceHolders = new Regex(@"\[([\d]*)\]");

    }
}