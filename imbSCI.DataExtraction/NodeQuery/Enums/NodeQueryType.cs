using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.DataExtraction.NodeQuery.Enums
{
    public enum NodeQueryType
    {
        /// <summary>
        /// Performs XPath selection
        /// </summary>
        xpath,

        /// <summary>
        /// Regex over URL
        /// </summary>
        urlRegex,

        jQuery,

        /// <summary>
        /// Uses CSS selector
        /// </summary>
        cssSelector,

        /// <summary>
        /// JS query selector like: document.querySelector('body > div > div.dx-datagrid-headers.dx-datagrid-content.dx-datagrid-nowrap')
        /// </summary>
        consoleJS,
        /// <summary>
        /// Simple query, selecting items by index ranges, specified like: 0-3,5,7 (this will select nodes at indexes: 0,1,2,3,5,7). Index out-of-range is allowed.
        /// </summary>
        indexSelection,
    }
}