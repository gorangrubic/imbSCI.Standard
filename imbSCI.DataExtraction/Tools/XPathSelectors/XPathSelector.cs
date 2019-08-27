using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imbSCI.DataExtraction.Tools.XPathSelectors
{
    /// <summary>
    /// Utility class for finding optimal XPath query
    /// </summary>
    /// <seealso cref="imbSCI.DataExtraction.Tools.XPathSelectors.XPathSelectorItem" />
    public class XPathSelector:XPathSelectorItem
    {
       
        public XPathSelector()
        {
            type = XPathSelectorItemType.target;
        }

        /// <summary>
        /// Renders XPath selector
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            
            String prefix = "";
            var parents = items.Where(x => x.type == XPathSelectorItemType.parent).ToList();
            parents = parents.OrderByDescending(x => x.priority).ToList();
            foreach (var p in parents)
            {
                prefix = prefix.add(p.RenderSelf(), "/");
            }

            prefix = prefix.add(this.RenderSelf(), "/");

            return prefix;

        }
    }
}
