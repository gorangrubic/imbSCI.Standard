using System;

namespace imbSCI.Graph.Graphics.SvgAPI.Core
{
    /// <summary>
    /// Used for <see cref="svgPath"/>
    /// </summary>
    public interface ISVGInlineArgument
    {
        String ToString();

        void FromString(String input);
    }
}