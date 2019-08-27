using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using imbSCI.Core.extensions.text;
using imbSCI.Data;

namespace imbSCI.DataExtraction.Analyzers.Data
{
    /// <summary>
    /// Finds common prefix and sufix in URLs
    /// </summary>
    [Serializable]
    public class UrlAnalysis
    {
        public const String PlaceholderUnescaped = "{0}";
        public const String PlaceholderEscaped = "----0----";

        public const String PlaceholderRegex = @"([=\?\&_/\w\s\-\.]+)";

        public static Regex GetRegex(String UrlFormat)
        {
            
            var parts = UrlFormat.SplitSmart(PlaceholderUnescaped, "", true, true);

            List<String> escaped_parts = new List<string>();
            foreach (String p in parts)
            {
                escaped_parts.Add(Regex.Escape(p));
            }
            parts = escaped_parts;
            String regexExpression = "";
            if (parts.Count == 1)
            {
                regexExpression = parts[0] + PlaceholderRegex;
            } else if (parts.Count == 2)
            {
                regexExpression = parts[0] + PlaceholderRegex + parts[1];
            }

            return new Regex(regexExpression);
        }

        public static String EscapeForAceScript(String UrlFormat)
        {
            String output = UrlFormat.Replace(PlaceholderUnescaped, PlaceholderEscaped);
            return output;
        }

        public static String EscapeFromAceScript(String UrlFormat)
        {
            String output = UrlFormat.Replace(PlaceholderEscaped, PlaceholderUnescaped);
            return output;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlAnalysis"/> class.
        /// </summary>
        public UrlAnalysis()
        {

        }

        /// <summary>
        /// Returns Format String: CommonPrefix{0}CommonSufix
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public String ToString()
        {
            if (!IsSet())
            {
                return "";
            }
            if (IsInvariable)
            {
                return CommonPrefix;
            }
            return CommonPrefix + PlaceholderUnescaped + CommonSufix;
        }

        /// <summary>
        /// Determines whether this instance is configured
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is set; otherwise, <c>false</c>.
        /// </returns>
        public Boolean IsSet()
        {
            if (!CommonSufix.isNullOrEmpty()) return true;
            if (!CommonPrefix.isNullOrEmpty()) return true;
            return false;
        }

        /// <summary>
        /// Extracts the variable part of URL
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public String GetVariable(String url)
        {
            if (IsInvariable)
            {
                return "";
            }
            if (IsMatch(url))
            {
                String output = url.removeEndsWith(CommonSufix);
                output = output.removeStartsWith(CommonPrefix);
                return output;
            } else
            {
                return "";
            }
        }

        /// <summary>
        /// Determines whether the specified URL is match 
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>
        ///   <c>true</c> if the specified URL is match; otherwise, <c>false</c>.
        /// </returns>
        public Boolean IsMatch(String url)
        {
            Boolean output = false;
            
            if (!CommonSufix.isNullOrEmpty())
            {
                output = url.EndsWith(CommonSufix);
                if (output)
                {
                    output = url.StartsWith(CommonPrefix);
                }
            } else
            {
                output = url.StartsWith(CommonPrefix);
            }
            return output;
        }


        /// <summary>
        /// Analyzes the specified URL list.
        /// </summary>
        /// <param name="urlList">The URL list.</param>
        public void Analyze(IEnumerable<String> urlList)
        {
            CommonPrefix = urlList.commonStartSubstring(0);
            List<String> inverted_list = new List<string>();
            foreach (String url in urlList)
            {
                inverted_list.Add(url.Inverse());
            }
            CommonSufix = inverted_list.commonStartSubstring(0);
            CommonSufix = CommonSufix.Inverse();
        }

        /// <summary>
        /// Gets a value indicating whether this instance has no variable part of the url
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is invariable; otherwise, <c>false</c>.
        /// </value>
        [XmlIgnore]
        public Boolean IsInvariable
        {
            get
            {
                if (IsSet())
                {
                    return CommonPrefix == CommonSufix;
                }  else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets or sets the common prefix.
        /// </summary>
        /// <value>
        /// The common prefix.
        /// </value>
        public String CommonPrefix { get; set; } = "";

        /// <summary>
        /// Gets or sets the common sufix.
        /// </summary>
        /// <value>
        /// The common sufix.
        /// </value>
        public String CommonSufix { get; set; } = "";
    }
}
