using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace imbSCI.Graph.Graphics.SvgAPI.BasicShapes
{
#pragma warning disable CS1584 // XML comment has syntactically incorrect cref attribute 'System.Collections.Generic.List{imbSCI.Graph.Graphics.SvgAPI.BasicShapes.svgPathInstructionBase}'
#pragma warning disable CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
    /// <summary>
    /// Path instructions set
    /// </summary>
    /// <seealso cref="System.Collections.Generic.List{imbSCI.Graph.Graphics.SvgAPI.BasicShapes.svgPathInstructionBase}" />
    public class svgPathInstructionSet : List<svgPathInstructionBase>
#pragma warning restore CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning restore CS1584 // XML comment has syntactically incorrect cref attribute 'System.Collections.Generic.List{imbSCI.Graph.Graphics.SvgAPI.BasicShapes.svgPathInstructionBase}'
    {
        public static Regex REGEX_ARGUMENTSELECT = new Regex(@"(\w{1}[\d\s,\.]*)");

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
            foreach (svgPathInstructionBase arg in this)
            {
                sb.Append(arg.ToString());
                if (last == arg) sb.Append(" ");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Populate instructions from string input
        /// </summary>
        /// <param name="input">The input.</param>
        public void FromString(String input)
        {
            var parts = REGEX_ARGUMENTSELECT.Matches(input);
            List<String> values = new List<string>();
            foreach (Match p in parts)
            {
                values.Add(p.Value);
            }

            for (int i = 0; i < Count; i++)
            {
                svgPathInstructionBase arg = this[i];
                arg.FromString(values[i]);
            }
        }
    }
}