using System.Collections.Generic;

namespace imbSCI.Data.collection.graph
{
    /// <summary>
    /// Collection of <see cref="graphNodeSet"/>s, used internally by <see cref="graphTools.GetFirstNodeWithLeafs{T}(IEnumerable{T}, List{string}, int, int, int)"/>
    /// </summary>
    /// <seealso cref="System.Collections.Generic.List{imbSCI.Data.collection.graph.graphNodeSet}" />
    public class graphNodeSetCollection : List<graphNodeSet>
    {
    }
}