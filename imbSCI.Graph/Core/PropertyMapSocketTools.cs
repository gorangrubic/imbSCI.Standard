using imbSCI.Core.data.transfer;
using imbSCI.Data;
using imbSCI.Graph.DGML;
using imbSCI.Graph.DGML.core;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace imbSCI.Graph.Core
{
    public static class PropertyMapSocketTools
    {
        public static DirectedGraph MakeDirectedGraph(this IPropertyMapSocket propertyMapSocket) {

            DirectedGraph dgml = new DirectedGraph();

            dgml.Populate<Type, PropertyInfo>(new Type[] { propertyMapSocket.FromType, propertyMapSocket.ToType },
                x => x.GetProperties(BindingFlags.Instance | BindingFlags.Public),
                x => x.Name,
                x => x.Name,
                y => GetPropertyUID(y),
                y => y.Name,
                false, false);

            dgml.Link<PropertyMapLink>(propertyMapSocket.Links,
                x => GetPropertyUID(x.Source.property),
                x => GetPropertyUID(x.Target.property), x => x.Target.flags.ToString(), true);


            return dgml;

        }

        public static List<PropertyMapLink> SetLinksFromGraph(this IPropertyMapSocket propertyMapSocket, DirectedGraph dgml)
        {
            List<PropertyMapLink> output = new List<PropertyMapLink>();
            foreach (Link link in dgml.Links)
            {
                String sourcePropertyName = GetPropertyNameFromUID(link.Source);
                String targetPropertyName = GetPropertyNameFromUID(link.Target);

                if (!(sourcePropertyName.isNullOrEmpty() || targetPropertyName.isNullOrEmpty()))
                {
                   // log.log("Link [" + sourcePropertyName + "] to [" + targetPropertyName + "]");

                    var mapLink = propertyMapSocket.AddLink(sourcePropertyName, targetPropertyName);
                    if (mapLink != null)
                    {
                        output.Add(mapLink);
                    }
                }
                // var p1 = link.Source.Replace(map2.FromType.Name, "");
                // var p2 = link.Source.Replace(map2.ToType.Name, "");   
            }
            return output;
        }


        private static Regex SelectPropertyName = new Regex(@"([\w\d]+)\Z");
        private static String GetPropertyNameFromUID(String UID)
        {
            if (UID.Contains("."))
            {
                return SelectPropertyName.Match(UID).Value;
            }
            else
            {
                return "";
            }
        }

        private static String GetPropertyUID(PropertyInfo pi)
        {
            return pi.DeclaringType.Name + "." + pi.Name;
        }

    }
}
