using System.Collections.Generic;

namespace imbSCI.Data.collection.graph
{
#pragma warning disable CS1584 // XML comment has syntactically incorrect cref attribute 'System.Collections.Generic.List{imbSCI.Data.collection.graph.graphNodeSet}'
#pragma warning disable CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
    /// <summary>
    /// Collection of <see cref="graphNodeSet"/>s, used internally by <see cref="graphTools.GetFirstNodeWithLeafs{T}(IEnumerable{T}, List{string}, int, int, int)"/>
    /// </summary>
    /// <seealso cref="System.Collections.Generic.List{imbSCI.Data.collection.graph.graphNodeSet}" />
    public class graphNodeSetCollection : List<graphNodeSet>
#pragma warning restore CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning restore CS1584 // XML comment has syntactically incorrect cref attribute 'System.Collections.Generic.List{imbSCI.Data.collection.graph.graphNodeSet}'
    {
    }
}