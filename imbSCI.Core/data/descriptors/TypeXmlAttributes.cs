using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace imbSCI.Core.data.descriptors
{
/// <summary>
    /// Helper class for custom XML serializations
    /// </summary>
    /// <seealso cref="System.Collections.Generic.Dictionary{System.Reflection.PropertyInfo, imbSCI.Core.data.MemberInfoXmlAttributes}" />
    public class TypeXmlAttributes : List<PropertyInfoXmlAttributes>
    {
        public MemberInfoXmlAttributes Root { get; set; } = new MemberInfoXmlAttributes();

        public PropertyInfoXmlAttributes Get(String name)
        {
            if (Properties.ContainsKey(name)) return Properties[name];
            return null;
        }

        protected Dictionary<String, PropertyInfoXmlAttributes> Properties { get; set; } = new Dictionary<string, PropertyInfoXmlAttributes>();

        public TypeXmlAttributes(Type type)
        {
            Root.Deploy(type);

            var props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var p in props)
            {
                PropertyInfoXmlAttributes item = new PropertyInfoXmlAttributes();
                item.Deploy(p);
                Add(item);
                Properties.Add(p.Name, item);
            }
        }
    }
}