using imbSCI.Core.extensions.text;
using System;

namespace imbSCI.Graph.Graphics.SvgAPI.Core
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="imbSCI.Graph.Graphics.SvgAPI.Core.ISVGInlineArgument" />
    public class SVGLetterArgument : ISVGInlineArgument
    {
        public SVGLetterArgument()
        {
        }

        public SVGLetterArgument(String _value)
        {
            valueLetter = _value;
        }

        /// <summary>
        /// Gets or sets the value letter.
        /// </summary>
        /// <value>
        /// The value letter.
        /// </value>
        public String valueLetter { get; set; } = "";

        /// <summary>
        /// Gets or sets a value indicating whether this instance is upper case.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is upper case; otherwise, <c>false</c>.
        /// </value>
        public Boolean isUpperCase
        {
            get
            {
                return valueLetter.isUpperCase();
            }
            set
            {
                if (value)
                {
                    valueLetter = valueLetter.ToUpper();
                }
                else
                {
                    valueLetter = valueLetter.ToLower();
                }
            }
        }

        public override string ToString()
        {
            return valueLetter;
        }

        public void FromString(String input)
        {
            valueLetter = input.Trim();
        }
    }
}