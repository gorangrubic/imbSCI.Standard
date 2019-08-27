using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using System.Xml.Serialization;

namespace imbSCI.Core.data.descriptors
{
  public static class TypeDescriptorTools
    {
        public static TypeXmlAttributes GetXmlInfo(Type type)
        {
            if (typeXmlAttributes.ContainsKey(type)) return typeXmlAttributes[type];

            TypeXmlAttributes output = new TypeXmlAttributes(type);
            if (!typeXmlAttributes.ContainsKey(type))
            {
                lock (_typeXmlAttributesGet_lock)
                {
                    if (!typeXmlAttributes.ContainsKey(type))
                    {
                        typeXmlAttributes.Add(type, output);
                    }
                }
            }
            return output;

        }

        private static Object _typeXmlAttributesGet_lock = new Object();

        private static Object _typeXmlAttributes_lock = new Object();
        private static Dictionary<Type,TypeXmlAttributes> _typeXmlAttributes;
        /// <summary>
        /// static and autoinitiated object
        /// </summary>
        private static Dictionary<Type,TypeXmlAttributes> typeXmlAttributes
        {
            get
            {
                if (_typeXmlAttributes == null)
                {
                    lock (_typeXmlAttributes_lock)
                    {

                        if (_typeXmlAttributes == null)
                        {
                            _typeXmlAttributes = new Dictionary<Type,TypeXmlAttributes>();
                            // add here if any additional initialization code is required
                        }
                    }
                }
                return _typeXmlAttributes;
            }
        }




         private static Object _EnumDescriptor_Get_lock = new Object();
        private static Object _enumDescriptors_lock = new Object();
        private static Dictionary<Type, IEnumTypeDescriptor> _enumDescriptors;
        /// <summary>
        /// static and autoinitiated object
        /// </summary>
        private static Dictionary<Type, IEnumTypeDescriptor> enumDescriptors
        {
            get
            {
                if (_enumDescriptors == null)
                {
                    lock (_enumDescriptors_lock)
                    {

                        if (_enumDescriptors == null)
                        {
                            _enumDescriptors = new Dictionary<Type, IEnumTypeDescriptor>();
                            // add here if any additional initialization code is required
                        }
                    }
                }
                return _enumDescriptors;
            }
        }

        public static EnumTypeDescriptor<T> GetEnumTypeDescriptor<T>()
        {
            if (enumDescriptors.ContainsKey(typeof(T))) return enumDescriptors[typeof(T)] as EnumTypeDescriptor<T>;
            EnumTypeDescriptor<T> output = new EnumTypeDescriptor<T>();

            if (!enumDescriptors.ContainsKey(typeof(T)))
            {
                lock (_EnumDescriptor_Get_lock)
                {
                    if (!enumDescriptors.ContainsKey(typeof(T)))
                    {
                        enumDescriptors.Add(typeof(T), output);
                    }
                }
            }
            return output;

        }

    }
}