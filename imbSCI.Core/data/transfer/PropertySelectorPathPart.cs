using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.text;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Data;
using imbSCI.Data.collection.graph;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace imbSCI.Core.data.transfer
{
    

    
    
    /// <summary>
    /// Property selector unit (extracted from path format or used stand alone) in format: <c>prefix</c><c>name</c>[<c>index</c>]. Like: <c>$nekiNode[1]</c>  <c>#drugi-1Selector</c>  <c>^tre_ci[10]</c>  <c>peti[10] sesti</c>
    /// </summary>
    /// <remarks>
    /// Input string <c>$nekiNode[1]</c> is interpreted as: <see cref="prefix"/>="$", <see cref="name"/>="nekiNode">, <see cref="index"/>=1
    /// </remarks>
    public class PropertySelectorPathPart
    {
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(prefix);
            sb.Append(name);
            if (index > -1)
            {
                sb.Append("[" + index + "]");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Optional prefix has single character width, and can have value as one of non-letter characters: $ # % ^ &amp; ! + *.
        /// </summary>
        /// <value>
        /// The prefix.
        /// </value>
        public String prefix { get; set; } = "";

        /// <summary>
        /// Name is main idenfier string. Value can contain all letters, numbers and some symbols (like underline and minus)
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public String name { get; set; } = "";

        /// <summary>
        /// Optional zero-based positive index, specified in square brackets
        /// </summary>
        /// <value>
        /// The index.
        /// </value>
        public Int32 index { get; set; } = -1;

        public T SelectIndex<T>(IList<T> target)
        {
            if (index > 0)
            {
                return target[index];
            } else
            {
                return target.FirstOrDefault();
            }
        }

        public static Regex SELECT_FEATUREIDS = new Regex(@"([\$\#\&\%\!\/\*\+\^]{1})?([\w_-]+)(\[([\d]+)\])?");

        public static Boolean ContainsSelectorPathParts(String input, String prefix)
        {
            return GetSelectorPathParts(input, prefix).Any();
        }

        public static List<PropertySelectorPathPart> GetSelectorPathParts(String input)
        {
            List<PropertySelectorPathPart> output = new List<PropertySelectorPathPart>();

            var mch = SELECT_FEATUREIDS.Matches(input);

            foreach (Match m in mch)
            {
                PropertySelectorPathPart pp = new PropertySelectorPathPart();
                pp.Deploy(m);
                output.Add(pp);

            }

            return output;
        }

        public static List<PropertySelectorPathPart> GetSelectorPathParts(String input, String prefix)
        {
            List<PropertySelectorPathPart> output = GetSelectorPathParts(input);

            return output.Where(x => x.prefix == prefix).ToList();
        }

        public PropertySelectorPathPart() { }

        public PropertySelectorPathPart(String pathPart)
        {
            Deploy(pathPart);
        }

        protected void Deploy(Match m)
        {
            if (m.Success)
            {
                prefix = m.Groups[1].Value.or("");
                name = m.Groups[2].Value.or("");

                String indexString = m.Groups[4].Value.or("-1");
                index = Int32.Parse(indexString);
            }
        }

        protected void Deploy(String pathPart) {

            Match m = SELECT_FEATUREIDS.Match(pathPart);
            Deploy(m);
        }
    }
}