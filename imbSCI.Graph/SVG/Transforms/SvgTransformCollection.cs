using System.Collections.Generic;
using System.ComponentModel;

namespace Svg.Transforms
{
    [TypeConverter(typeof(SvgTransformConverter))]
    public class SvgTransformCollection : List<SvgTransform>
    {
    }
}