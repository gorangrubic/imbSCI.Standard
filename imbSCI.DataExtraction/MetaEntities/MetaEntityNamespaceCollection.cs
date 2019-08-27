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
    public class MetaEntityNamespaceCollection : MetaEntityTargetWithPathBase
    {
        public List<MetaEntityNamespace> Namespaces { get; set; } = new List<MetaEntityNamespace>();

        [XmlIgnore]
        public override List<IMetaEntityExpressionTarget> items
        {
            get
            {

                return Namespaces.ToList<IMetaEntityExpressionTarget>();
            }
        }

        //[XmlIgnore]
        //public override IMetaEntityExpressionTarget Parent { get => this; set { } }

        public MetaEntityNamespaceCollection()
        {
            Parent = this;
        }

        public MetaEntityClass FindClass(string className)
        {
            if (MetaEntityTools.IsMultinodeExpressionPath(className)) return this.SelectTargetByPath(className) as MetaEntityClass;

            foreach (var ns in Namespaces)
            {
                var cs = ns.FindClass(className);
                if (cs != null)
                {
                    return cs;
                }
            }

            return null;
        }

        public override IMetaEntityExpressionTarget SelectTarget(string expressionPathNode)
        {
            if (MetaEntityTools.IsMultinodeExpressionPath(expressionPathNode)) return this.SelectTargetByPath(expressionPathNode);
            var head = Namespaces.FirstOrDefault(x => x.name.Equals(expressionPathNode));

            if (head == null)
            {
                MetaEntityNamespace item = new MetaEntityNamespace();
                item.name = expressionPathNode;
                item.Parent = this;

                Namespaces.Add(item);
                head = item;
            } else
            {
                head.Parent = this;
            }

            return head;
        }

        
    }
}