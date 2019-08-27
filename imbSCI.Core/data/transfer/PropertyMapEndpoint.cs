using imbSCI.Core.extensions.typeworks;
using imbSCI.Core.extensions.data;
using imbSCI.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;

namespace imbSCI.Core.data.transfer
{
    public class PropertyMapEndpoint
    {
        public ICustomTypeConverter AssignedConverter { get; set; }
        public PropertyMapLinkFlags flags { get; set; }

        public PropertyInfo property { get; set; } 
        public Boolean IsIList { get; set; }
        public Boolean DefaultValueSet { get; set; } = false;
        public Object DefaultValue { get; set; }

        public PropertyMapEndpoint(PropertyInfo _property,PropertyMapLinkFlags _flags, Object DefaultInstance=null)
        {
            property = _property;
            IsIList = _property.PropertyType.IsIList();
            if (DefaultInstance != null)
            {
                if (!IsIList)
                {
                    DefaultValue = property.GetValue(DefaultInstance, null);
                    DefaultValueSet = true;
                }
            }
            
        }
        
        public Object GetValue(Object source,IPropertyMapSocket socket)
        {
            if (source == null) return null;

            Object vl = property.GetValue(source, null); 

            if (IsIList)
            {
                if (vl is IList vlist) {

                    if (flags.HasFlag(PropertyMapLinkFlags.IListTransferAll))
                    {
                        return vlist.ConvertToList<Object>();

                    } else if (flags.HasFlag(PropertyMapLinkFlags.IListTransferFirst))
                    {
                        return vlist.getFirstSafe(); //.FirstOrDefault();
                    } else if (flags.HasFlag(PropertyMapLinkFlags.IListTransferLast))
                    {
                        return vlist.getLastSafe();
                    }

                } else
                {
                    return vl;
                }

            } else
            {
                return property.GetValue(source, null);
            }
            return vl;
        }

        public void SetValue(Object target, Object vl, Type vl_type, IPropertyMapSocket socket)
        {
            if (target == null) return;
            
            Object t_vl = property.GetValue(target, null);
            IList t_list = t_vl as IList;

            if (t_list != null)
            {
                if (vl is IList vlist) {

                    if (flags.HasFlag(PropertyMapLinkFlags.IListTransferAll))
                    {
                        foreach (var v in vlist)
                        {
                          if (!t_list.Contains(v)||!flags.HasFlag(PropertyMapLinkFlags.IListUnique))  t_list.Add(v);
                        }

                    } else if (flags.HasFlag(PropertyMapLinkFlags.IListTransferFirst))
                    {
                        var v = vlist.getFirstSafe();
                       if (!t_list.Contains(v)||!flags.HasFlag(PropertyMapLinkFlags.IListUnique)) t_list.Add(v);
                    } else if (flags.HasFlag(PropertyMapLinkFlags.IListTransferLast))
                    {
                        var v = vlist.getLastSafe();
                      if (!t_list.Contains(v)||!flags.HasFlag(PropertyMapLinkFlags.IListUnique))  t_list.Add(v);
                    }

                }

            } else
            {
                

                if (flags.HasFlag(PropertyMapLinkFlags.IgnoreNullOrEmptyValues) && vl.isNullOrEmpty()) return;
                if (flags.HasFlag(PropertyMapLinkFlags.IgnoreDefaultValues) && DefaultValueSet && vl.Equals(DefaultValue)) return;
                if (flags.HasFlag(PropertyMapLinkFlags.SkipNonDefaultValueOverwrite) && !t_vl.Equals(DefaultValue)) return;

                if (property.PropertyType != vl_type)
                {
                    vl = socket.Converter.ConvertValueFor(vl, property);
                }

                property.SetValue(target, vl, null); 
            }

        }
    }
}