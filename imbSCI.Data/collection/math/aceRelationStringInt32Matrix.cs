using imbSCI.Data.collection.nested;
using imbSCI.Data.primitives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace imbSCI.Data.collection.math
{
public class aceRelationStringInt32Matrix : aceRelationMatrix<String, String, Int32>
    {
        public aceRelationStringInt32Matrix(IEnumerable<string> __axisX, IEnumerable<string> __axisY, aceRelationValue<string, string, int> __init) : base(__axisX, __axisY, __init)
        {
        }
    }
}