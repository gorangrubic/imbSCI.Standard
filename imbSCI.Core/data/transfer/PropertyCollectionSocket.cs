using imbSCI.Core.extensions.typeworks;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace imbSCI.Core.data.transfer
{
    /// <summary>
    /// Provides access to instances of properties targeted by <see cref="PropertyInfo.PropertyType"/>, specified by generic parameter {TCommonBase}
    /// </summary>
    /// <typeparam name="TCommonBase">Common type base to select properties .</typeparam>
    public class PropertyCollectionSocket<TCommonBase> where TCommonBase : class
    {
        public PropertyCollectionSocket()
        {

        }

        /// <summary>
        /// Calls <see cref="SetTargetProperties"/> automatically
        /// </summary>
        /// <param name="targetHost">The target host.</param>
        public PropertyCollectionSocket(Object targetHost)
        {
            SetTargetProperties(targetHost);
        }

        /// <summary>
        /// Collection with all childstates from 
        /// </summary>
        /// <value>
        /// The child states.
        /// </value>
        protected List<PropertyInfo> TargetProperties { get; set; } = new List<PropertyInfo>();

        public Boolean Remove(String propertyName)
        {
            PropertyInfo pi = TargetProperties.FirstOrDefault(x => x.Name.Equals(propertyName));
            if (pi != null)
            {
                return TargetProperties.Remove(pi);
            }
            else { 
                return false;
            }
        }

        /// <summary>
        /// Collects current values from <see cref="TargetProperties"/>. If target host not specifeid, used the one stored in <see cref="TargetHost"/>
        /// </summary>
        /// <param name="targetHost">The target host.</param>
        /// <returns></returns>
        public List<TCommonBase> Collect(Object targetHost=null)
        {
            if (targetHost == null) targetHost = TargetHost;

            if (TargetHost == null)
            {
                throw new ArgumentNullException(nameof(targetHost), $"Target host is null in both method parameter and {nameof(TargetHost)}");
            }

            List<TCommonBase> output = new List<TCommonBase>();

            foreach (var pi in TargetProperties)
            {
                Object ch = pi.GetValue(targetHost, null);

                TCommonBase wdsm = ch as TCommonBase;
                if (wdsm != null)
                {
                    output.Add(wdsm);
                } else
                {

                }
            }

            return output;

        }

        /// <summary>
        /// Scans the target host and sets target property list
        /// </summary>
        /// <param name="targetHost">The target host.</param>
        public void SetTargetPropertiesFromType(Type targetHost)
        {
            TargetProperties.Clear();
            var props = targetHost.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public).ToList();
            foreach (var pi in props)
            {
                var wdsm_interface = pi.PropertyType.GetInterfaces().FirstOrDefault(x => x == typeof(TCommonBase));
                
                if (wdsm_interface != null)
                {
                    TargetProperties.Add(pi);
                } else
                {
                    Type head = pi.PropertyType;
                    Type target = typeof(TCommonBase);
                    Boolean accept = false;
                    while (head != null)
                    {
                        if (head == target)
                        {
                            accept = true;
                            break;
                        } else
                        {
                            head = head.BaseType;
                        }
                        if (head == typeof(Object)) break;
                    }
                    if (accept) TargetProperties.Add(pi);
                }
            }
        }

        protected Object TargetHost { get; set; } = null;

        /// <summary>
        /// Scans the target host and sets target property list
        /// </summary>
        /// <param name="targetHost">The target host.</param>
        public void SetTargetProperties(Object targetHost)
        {
            TargetProperties.Clear();
            if (targetHost != typeof(Type))
            {
             
                SetTargetPropertiesFromType(targetHost.GetType());
                
            } else
            {
                SetTargetPropertiesFromType(targetHost as Type);
            }

            TargetHost = targetHost;
        }
    }
}