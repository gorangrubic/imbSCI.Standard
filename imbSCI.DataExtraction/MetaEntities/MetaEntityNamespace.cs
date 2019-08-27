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

    public class MetaEntityNamespace: MetaEntityTargetWithPathBase
    {
        
        public List<MetaEntityClass> Declarations { get; set; } = new List<MetaEntityClass>();
        [XmlIgnore]
        public override List<IMetaEntityExpressionTarget> items
        {
            get
            {

                return Declarations.ToList<IMetaEntityExpressionTarget>();
            }
        }

        public MetaEntity CreateEntity(String EntityClassName, String entityName = "")
        {
            var metaClass = Declarations.FirstOrDefault(x => x.name == EntityClassName);
            return metaClass.CreateEntity(this, entityName);
        }

        public MetaEntityClass FindClass(string className)
        {
            if (MetaEntityTools.IsMultinodeExpressionPath(className))
            {
                return Parent.SelectTargetByPath(className) as MetaEntityClass;
            }

            return Declarations.FirstOrDefault(x => x.name.Equals(className));

        }

        public override IMetaEntityExpressionTarget SelectTarget(string expressionPathNode)
        {
            if (MetaEntityTools.IsMultinodeExpressionPath(expressionPathNode)) return Parent.SelectTargetByPath(expressionPathNode);

            var head = Declarations.FirstOrDefault(x => x.name.Equals(expressionPathNode));

            if (head == null)
            {
                MetaEntityClass item = new MetaEntityClass();
                item.name = expressionPathNode;
                item.Parent = this;

                Declarations.Add(item);
                head = item;
            }


            return head;
        }

      
        public MetaEntityNamespace()
        {

        }
    }
}