using imbSCI.Core.extensions.typeworks;
using imbSCI.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace imbSCI.Core.data.transfer
{
  

    public class PropertyMapLink
    {
        public static PropertyMapLink Create(PropertyInfo source, PropertyInfo target, IPropertyMapSocket socket,  PropertyMapLinkFlags flags = PropertyMapLinkFlags.none)
        {
            if (!target.CanWrite) return null;
            if (!source.CanRead) return null;
            if (!target.GetIndexParameters().isNullOrEmpty()) return null;
            if (!source.GetIndexParameters().isNullOrEmpty()) return null;

            if (flags == PropertyMapLinkFlags.none) flags = socket.GeneralLinkOptions;
            PropertyMapLink output = new PropertyMapLink();

            output.Source = new PropertyMapEndpoint(source, flags, socket.FromDefaultInstance);
            output.Target = new PropertyMapEndpoint(target, flags, socket.ToDefaultInstance);

            return output;
        }

        public void Execute(Object sourceInstance, Object targetInstance, IPropertyMapSocket socket)
        {
            Object vs = Source.GetValue(sourceInstance, socket);
            Target.SetValue(targetInstance, vs, Source.property.PropertyType, socket);
        }

        public PropertyMapEndpoint Source { get; set; } 
        public PropertyMapEndpoint Target { get; set; } 
    }
}
