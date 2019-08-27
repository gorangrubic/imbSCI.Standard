using imbSCI.Core.collection;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Data;
using imbSCI.Data.interfaces;
using imbSCI.DataExtraction.MetaTables.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imbSCI.DataExtraction.MetaEntities
{
    public interface IMetaEntityExpressionTargetWithPath:IMetaEntityExpressionTarget
    {
        String GetNamepath(IMetaEntityExpressionTarget fromParent=null);
    }

    public interface IMetaEntityExpressionTarget:IObjectWithName
    {
        void Deploy();

        List<IMetaEntityExpressionTarget> items { get; }

        IMetaEntityExpressionTarget SelectTarget(String expressionPathNode);

        IMetaEntityExpressionTarget Parent { get; set; }
    }
}