using System;

namespace imbSCI.Graph.Graphics.SvgAPI.Core
{
#pragma warning disable CS1574 // XML comment has cref attribute 'svgPath' that could not be resolved
    /// <summary>
    /// Used for <see cref="svgPath"/>
    /// </summary>
    public interface ISVGInlineArgument
#pragma warning restore CS1574 // XML comment has cref attribute 'svgPath' that could not be resolved
    {
        String ToString();

        void FromString(String input);
    }
}