using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace imbSCI.Graph.Graphics.SvgAPI.Core
{
    /// <summary>
    /// Inline arguments
    /// </summary>
    /// <seealso cref="System.Collections.Generic.List{imbSCI.Graph.Graphics.SvgAPI.Core.ISVGInlineArgument}" />
    public class SVGInlineArguments : List<ISVGInlineArgument>
    {
        //  public static Regex REGEX_ARGUMENTSELECT = new Regex(@"(\w{1}[\d\s,\.]*");

        public static Regex REGEX_PARAMSELECT = new Regex(@"([\-\d\w\,\.]+)\b");

        public SVGInlineArguments()
        {
        }

        public SVGInlineArguments(params ISVGInlineArgument[] args)
        {
            foreach (var a in args)
            {
                Add(a);
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            var last = this.Last();
            foreach (ISVGInlineArgument arg in this)
            {
                sb.Append(arg.ToString());
                if (last == arg) sb.Append(" ");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Froms the string.
        /// </summary>
        /// <param name="input">The input.</param>
        public void FromString(String input)
        {
            var parts = REGEX_PARAMSELECT.Matches(input);
            List<String> values = new List<string>();
            foreach (Match p in parts)
            {
                values.Add(p.Value);
            }

            for (int i = 0; i < Count; i++)
            {
                ISVGInlineArgument arg = this[i];
                arg.FromString(values[i]);
            }
        }
    }
}