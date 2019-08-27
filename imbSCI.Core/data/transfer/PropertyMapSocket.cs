using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace imbSCI.Core.data.transfer
{
    [Flags]
    public enum PropertyMapGenerationOptions
    {
        none = 0,
        matchName=1,
        ignoreCase=2,
        doAutoMapping = 4
        

    }

    public interface IPropertyMapSocket
    {
        PropertyMapLinkFlags GeneralLinkOptions { get; }
        PropertyMapGenerationOptions Options { get; }
        Type ToType { get; }
        Type FromType { get; }
        Object FromDefaultInstance { get; }
        Object ToDefaultInstance { get; }
        CustomTypeConverterDictionary Converter { get; }
         List<PropertyMapLink> Links { get; }
        PropertyMapLink AddLink(String sourceProperty, String targetProperty, PropertyMapLinkFlags flags = PropertyMapLinkFlags.none);
    }

    public class PropertyMapSocket : IPropertyMapSocket
    {
        public Type FromType { get; protected set; }
        public Type ToType { get; protected set; }

        public Object FromDefaultInstance { get; protected set; } 
        public Object ToDefaultInstance { get; protected set; } 

        public PropertyMapSocket(Type fromType, Type toType) {
            FromType = fromType;
            toType = toType;

            DeployTypes();
        }

        public PropertyMapLinkFlags GeneralLinkOptions { get; set; } = PropertyMapLinkFlags.IListUnique | PropertyMapLinkFlags.IListTransferAll;

        public PropertyMapGenerationOptions Options { get; set; } = PropertyMapGenerationOptions.matchName | PropertyMapGenerationOptions.ignoreCase | PropertyMapGenerationOptions.doAutoMapping;

        public CustomTypeConverterDictionary Converter { get; set; } = new CustomTypeConverterDictionary();

        public List<PropertyMapLink> Links { get; protected set; } = new List<PropertyMapLink>();

        Dictionary<string, PropertyInfo> fromProps = new Dictionary<string, PropertyInfo>();
        Dictionary<string, PropertyInfo> toProps = new Dictionary<string, PropertyInfo>();

        /// <summary>
        /// Determines whether the specified f pi contains link.
        /// </summary>
        /// <param name="f_pi">The f pi.</param>
        /// <param name="t_pi">The t pi.</param>
        /// <returns>
        ///   <c>true</c> if the specified f pi contains link; otherwise, <c>false</c>.
        /// </returns>
        public Boolean ContainsLink(PropertyInfo f_pi, PropertyInfo t_pi)
        {
            return Links.Any(x => (x.Source.property.Name.Equals(f_pi.Name) && x.Target.property.Name.Equals(t_pi.Name)));
        }

        public Boolean IsDeployed
        {
            get
            {
                return Links.Any();
            }
        }

        /// <summary>
        /// Adds the link.
        /// </summary>
        /// <param name="sourceProperty">The source property.</param>
        /// <param name="targetProperty">The target property.</param>
        /// <param name="flags">The flags.</param>
        /// <returns>Null if source or target property was not found</returns>
        public PropertyMapLink AddLink(String sourceProperty, String targetProperty, PropertyMapLinkFlags flags = PropertyMapLinkFlags.none)
        {
            PropertyInfo f_pi = null;
            PropertyInfo t_pi = null;


            if (Options.HasFlag(PropertyMapGenerationOptions.ignoreCase))
            {
                f_pi = fromProps.Search(sourceProperty, StringComparison.InvariantCultureIgnoreCase, true).FirstOrDefault();
                t_pi = toProps.Search(targetProperty, StringComparison.InvariantCultureIgnoreCase, true).FirstOrDefault();
            }
            else
            {
                if (!fromProps.ContainsKey(sourceProperty)) return null;
                if (!toProps.ContainsKey(targetProperty)) return null;

                f_pi = fromProps[sourceProperty]; //, StringComparison.InvariantCultureIgnoreCase, true).FirstOrDefault();
                t_pi = toProps[targetProperty]; //, StringComparison.InvariantCultureIgnoreCase, true).FirstOrDefault();
            }

            if (f_pi == null || t_pi == null) return null;

            if (ContainsLink(f_pi, t_pi)) return null;


            PropertyMapLink link = PropertyMapLink.Create(f_pi, t_pi, this, flags);
            if (link != null)
            {
                Links.Add(link);
            }
            return link;
        }

        protected virtual void DeployTypes()
        {

            fromProps = FromType.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(x => x.Name);
            toProps = ToType.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(x => x.Name);
        }



        /// <summary>
        /// Deploys this instance.
        /// </summary>
        public void Deploy(PropertyMapGenerationOptions __options = PropertyMapGenerationOptions.none)
        {

            if (__options != PropertyMapGenerationOptions.none)
            {
                Options = __options;
            }

            if (FromType.hasParameterlessConstructor()) FromDefaultInstance = FromType.getInstance(null);
            if (ToType.hasParameterlessConstructor()) ToDefaultInstance = ToType.getInstance(null);

            if (Options.HasFlag(PropertyMapGenerationOptions.doAutoMapping))
            {


                // fromProps.Select(x => toProps.ContainsKey(x.Key));

                if (Options.HasFlag(PropertyMapGenerationOptions.matchName))
                {

                    List<String> matchFromList = fromProps.Keys.Where(x => toProps.ContainsKey(x)).ToList();
                    foreach (var k in matchFromList)
                    {
                        PropertyMapLink link = PropertyMapLink.Create(fromProps[k], toProps[k], this);
                        if (link != null)
                        {
                            Links.Add(link);
                        }
                    }
                }
            }

        }


        /// <summary>
        /// Executes defined property map links
        /// </summary>
        /// <param name="Target">The target.</param>
        /// <param name="Source">The source.</param>
        public virtual void SetPropertiesUntyped(Object Target, Object Source)
        {
            if (Target.GetType() != ToType) throw new Exception("Target object is not of target type");
            if (Source.GetType() != FromType) throw new Exception("Target object is not of target type");

            foreach (PropertyMapLink link in Links)
            {
                link.Execute(Source, Target, this);
            }
        }

        
    }

    public class PropertyMapSocket<TFrom, TTo> : PropertyMapSocket
        where TFrom:class
        where TTo:class
    {
        public PropertyMapSocket():base(typeof(TFrom), typeof(TTo)) {
            DeployTypes();
        }

        protected override void DeployTypes()
        {
            FromType = typeof(TFrom);
            ToType = typeof(TTo);

            FromDefaultInstance = default(TFrom);
            ToDefaultInstance = default(TTo);

            base.DeployTypes();
            
        }

        /// <summary>
        /// Executes defined property map links
        /// </summary>
        /// <param name="Target">The target.</param>
        /// <param name="Source">The source.</param>
        public void SetProperties(TTo Target, TFrom Source)
        {
            foreach (PropertyMapLink link in Links)
            {
                link.Execute(Source, Target, this);
            }
        }


    }
}