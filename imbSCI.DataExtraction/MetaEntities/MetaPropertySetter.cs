using imbSCI.Core.collection;
using imbSCI.Core.extensions.text;
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

    public class MetaPropertySetter: IMetaEntityExpressionTarget
    {
        [XmlIgnore]
        public List<IMetaEntityExpressionTarget> items
        {
            get
            {
                return new List<IMetaEntityExpressionTarget>();
            }
        }
        public void Deploy()
        {

        }



        public String name { get; set; }

        [XmlIgnore]
        public IMetaEntityExpressionTarget Parent { get; set; }

        public Object Value { get; set; }

        public MetaPropertySetter()
        {

        }

        public IMetaEntityExpressionTarget SelectTarget(string expressionPathNode)
        {
            return this;
        }
    }
}