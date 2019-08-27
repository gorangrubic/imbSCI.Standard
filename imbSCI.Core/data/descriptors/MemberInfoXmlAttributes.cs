using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace imbSCI.Core.data.descriptors
{

    public class PropertyInfoXmlAttributes:MemberInfoXmlAttributes
    {
        public override void Deploy(MemberInfo memberInfo)
        {
            Property = memberInfo as PropertyInfo;
            base.Deploy(memberInfo);
        }
        public PropertyInfo Property { get; protected set; }
    }


    public class MemberInfoXmlAttributes: Dictionary<String, Attribute>
    {
        public Boolean CanWrite { get; set; } = false;

        public Boolean IsAttribute {
            get
            {
                return ContainsKey(nameof(XmlAttributeAttribute));
            }
        }

        public String ElementName { get; set; } = "";

        public Boolean Ignore
        {
            get
            {
                return ContainsKey(nameof(XmlIgnoreAttribute));
            }
        }

        public virtual void Deploy(MemberInfo memberInfo)
        {

            var attributes = memberInfo.GetCustomAttributes(true);
            foreach (Attribute attribute in attributes)
            {
                if (!ContainsKey(attribute.GetType().Name)) Add(attribute.GetType().Name, attribute);
                if (attribute is XmlElementAttribute elementAttribute)
                {
                    ElementName = elementAttribute.ElementName;
                }  
                if (attribute is XmlRootAttribute rootAttribute)
                {
                    ElementName = rootAttribute.ElementName;
                }
            }

            if (memberInfo is PropertyInfo pi) {
                CanWrite = pi.CanWrite;
                if (ElementName.isNullOrEmpty()) ElementName = pi.Name;
            } else if (memberInfo is Type ti)
            {
                if (ElementName.isNullOrEmpty()) ElementName = ti.Name;
            }

            
        }

        public MemberInfoXmlAttributes()
        {

        }
    }
}