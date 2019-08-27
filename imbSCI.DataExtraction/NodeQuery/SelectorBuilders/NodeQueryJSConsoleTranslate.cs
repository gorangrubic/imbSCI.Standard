using HtmlAgilityPack;
using imbSCI.Core.extensions.io;
using imbSCI.Core.extensions.text;
using imbSCI.Core.style.css;
using imbSCI.Data;
using imbSCI.Data.extensions;
using imbSCI.Data.extensions.data;
using imbSCI.DataExtraction.NodeQuery.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.DataExtraction.NodeQuery.SelectorBuilders
{
    public static class NodeQueryJSConsoleTranslate
    {
        public static String GetNodeSelectionJS(this HtmlNode node)
        {
            String output = "";
            if (node == null) return "";

            if (node.Name.Equals(htmlTagEnum.a.ToString(),StringComparison.InvariantCultureIgnoreCase))
            {
                String href_attribute = node.GetAttributeValue("href", "");
                String text = ""; // node.InnerText;
                String xpath = node.XPath;
                if (!href_attribute.isNullOrEmpty() || !text.isNullOrEmpty())
                {
                    xpath = $"//" + node.Name;
                    String inner = "";
                    if (!href_attribute.isNullOrEmpty())
                    {
                        inner = inner.add($"@href='{href_attribute}'", " ");
                    }
                    //if (!text.isNullOrEmpty())
                    //{
                    //    inner = inner.add($"text()=\\\"{text}\\\"", " and ");
                    //}
                    if (!inner.isNullOrEmpty())
                    {
                        xpath = xpath + "[" + inner + "]";
                    }
                }

                output = $"$x(\"{xpath}\")[0]";
            } else if (!node.Id.isNullOrEmpty())
            {
                output = $"document.getElementById(\"{node.Id}\")";
            } else if (node.GetAttributeValue("class", "")!= "")
            {
                output = $"document.getElementsByClassName(\"{node.GetAttributeValue("class", "")}\")[0]";
            } 
            return output;
        }


        public static String GetConsoleSyntax(this NodeQueryType queryType, WebClientCommandType commandType, String query, String value = "")
        {
            String output = "";
            switch (queryType)
            {
                case NodeQueryType.cssSelector:
                    output = "$$(\"" + query + "\")";
                    break;

                case NodeQueryType.consoleJS:
                    output = "document.querySelector('" + query + "');"; // body > div > div.dx-datagrid-headers.dx-datagrid-content.dx-datagrid-nowrap')
                    break;

                case NodeQueryType.jQuery:
                    output = "$('" + query + "')";
                    break;

                case NodeQueryType.xpath:
                    output = "$x(\"" + query + "\")";
                    break;

                default:
                    break;
            }

            switch (commandType)
            {
                case WebClientCommandType.Click:
                    output += "[0].click();";
                    break;

                case WebClientCommandType.SetValue:
                    output += "[0].value = '" + value + "';";
                    break;

                case WebClientCommandType.Select:
                    output += ";";
                    break;

                default:
                    break;
            }

            return output;
        }
    }
}