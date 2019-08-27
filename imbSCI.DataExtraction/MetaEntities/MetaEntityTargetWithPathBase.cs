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
using System.Xml.Serialization;

namespace imbSCI.DataExtraction.MetaEntities
{
    public abstract class MetaEntityTargetWithPathBase: IMetaEntityExpressionTargetWithPath
    {

        public void Deploy()
        {
            foreach (var nspace in items)
            {
                nspace.Parent = this;

                nspace.Deploy();
            }

        }

        protected MetaEntityTargetWithPathBase()
        {

        }

        [XmlIgnore]
        public abstract List<IMetaEntityExpressionTarget> items { get; }

        public virtual string name { get; set; } = "";

        [XmlIgnore]
        public virtual IMetaEntityExpressionTarget Parent { get; set; }

        public string GetNamepath(IMetaEntityExpressionTarget fromParent = null)
        {
            return this.GetSelectExpression(fromParent);
        }

        public abstract IMetaEntityExpressionTarget SelectTarget(string expressionPathNOde);
    }
}