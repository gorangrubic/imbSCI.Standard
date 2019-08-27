using imbSCI.Data.collection.nested;
using imbSCI.Data.interfaces;
using System;
using System.Collections.Generic;

namespace imbSCI.Graph.FreeGraph.adapters
{

    /// <summary>
    /// Stores instances of a <c>Type</c> (inplementing IObjectWithNameWeightAndType)
    /// </summary>
    /// <seealso cref="imbSCI.Data.collection.nested.aceDictionary2D{System.Type, System.String, imbSCI.Data.interfaces.IObjectWithNameWeightAndType}" />
    public class freeGraphTypedNodesRegister : aceDictionary2D<Type, String, IObjectWithNameWeightAndType>
    {

    }

}