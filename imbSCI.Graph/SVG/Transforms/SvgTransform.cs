using System.Drawing.Drawing2D;

namespace Svg.Transforms
{
    public abstract class SvgTransform
    {
        public abstract Matrix Matrix { get; }

        public abstract string WriteToString();
    }
}