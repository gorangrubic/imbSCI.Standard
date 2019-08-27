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
    public class MetaEntityClassProperty : MetaTableProperty, IMetaEntityExpressionTargetWithPath
    {
        private String _propertyTypeName = "String";


        protected MetaEntityClass propertTypeClassDefinition { get; set; }

        public MetaEntityClassProperty() { }

        public MetaEntityClassPropertyType type { get; set; } = MetaEntityClassPropertyType.value;

        [XmlIgnore]
        public IMetaEntityExpressionTarget Parent { get; set; }

        public String name
        {
            get { return PropertyName; }
            set { PropertyName = value; }
        }

        public String PropertyTypeName
        {
            get {
                if (type.HasFlag(MetaEntityClassPropertyType.entity)) return _propertyTypeName;

                return ValueTypeName; }
            set {
                if (ValueTypeName.isNullOrEmpty()) ValueTypeName = value;
                _propertyTypeName = value;
            }
        }
        [XmlIgnore]
        public List<IMetaEntityExpressionTarget> items { 
            get {
                return new List<IMetaEntityExpressionTarget>();
            }
        }
        public void Deploy()
        {

        }

        public string GetNamepath(IMetaEntityExpressionTarget fromParent = null)
        {
            return this.GetSelectExpression(fromParent);
        }

        public IMetaEntityExpressionTarget SelectTarget(string expressionPathNode)
        {
            if (MetaEntityTools.IsMultinodeExpressionPath(expressionPathNode)) return this.SelectTargetByPath(expressionPathNode);

            if (propertTypeClassDefinition != null)
            {
                return propertTypeClassDefinition.SelectTarget(expressionPathNode);
            }
            return this;
        }

       
    }
}