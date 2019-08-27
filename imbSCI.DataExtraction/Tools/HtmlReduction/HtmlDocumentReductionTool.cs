using HtmlAgilityPack;
using imbSCI.Core.extensions.table;
using imbSCI.Core.extensions.text;
using imbSCI.Core.math;
using imbSCI.Core.reporting;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace imbSCI.DataExtraction.Tools.HtmlReduction
{
/// <summary>
    /// Performs HTML document reduction, dataset page count and contectivity reduction
    /// </summary>
    public class HtmlDocumentReductionTool 
    {
        private HtmlReductionSettings _settings = null;

        public HtmlDocumentReductionTool()
        {
        }

        public HtmlReductionSettings settings
        {
            get {
                if(_settings == null)
                {
                    _settings = HtmlReductionSettings.GetDefaultReductionSettings();
                }
                return _settings; }
            set { _settings = value; }
        }


        protected bool IsNodeEmpty(HtmlNode node)
        {
            if (node.HasChildNodes) return false;
            if (!node.InnerText.isNullOrEmpty()) return false;
            return true;
        }

        protected Regex REGEX_SELECTCOMMENTS { get; set; } = new Regex(@"(\<\!\-\-.*\-\-\>)");
        protected Regex REGEX_EMPTYSPACE { get; set; } = new Regex(@">([\n\s\t]+)<");

        protected Boolean ContainsNonTextNodes(HtmlNode node)
        {
            if (!node.HasChildNodes) return false;
            return node.ChildNodes.Any(x => !x.Name.StartsWith("#"));
        }


        protected String RenderCommentNode(String text)
        {
            var textLines = text.SplitSmart(Environment.NewLine, "", true, true);
            StringBuilder sb = new StringBuilder();

            foreach (var t in textLines)
            {
                sb.AppendLine("<!-- " + t + " -->");
            }

            return sb.ToString();
        }

        protected String RenderNodeOpening(HtmlNode node, String prefix, Boolean hasChildren)
        {
            String line = "";


            line = prefix + "<" + node.Name.ToLower(); 

            foreach (var att in node.Attributes)
            {
                line = line.add(att.Name + $"=\"{att.Value.toStringSafe()}\"", " ");
            }

            if (hasChildren)
            {
                line = line + ">";
            } else
            {
                line = line.add("/>", " ");
            }

            return line;
        }

        protected Boolean RenderTextNode(HtmlNode node, StringBuilder sb)
        {
            String str = node.InnerText;
            str = str.Replace("\t", " ");
            str = str.removeStartsWith(Environment.NewLine);
            //str = str.removeEndsWith(Environment.NewLine);
            str = str.Trim();
            if (!str.isNullOrEmpty())
            {
                sb.Append(str);
                return true;
            } else
            {
                return false;
            }
        }

        protected void RenderNode(HtmlNode node, StringBuilder sb, Int32 level=0, String levelInsert="\t")
        {
            String nodeName = node.Name.ToLower();

            String prefix = "";
            if (level > 0)
            {
                prefix = levelInsert.Repeat(level);
            }

            Boolean insertOpenAndCloseTag = true;

            Int32 multiLineFormat = 0;


            if (settings.tagsToWrapout.Contains(nodeName))
            {
                if (!ContainsNonTextNodes(node))
                {
                    insertOpenAndCloseTag = false;
                }
                else
                {

                }
            }

            if (nodeName.StartsWith("#"))
            {
                insertOpenAndCloseTag = false;
            }

            switch (node.NodeType)
            {
                case HtmlNodeType.Comment:
                    break;
                case HtmlNodeType.Document:
                case HtmlNodeType.Element:

                    if (settings.tagsToForceMultilineFormat.Contains(nodeName))
                    {
                        multiLineFormat = 1;
                    }

                    Int32 newLevel = 0;

                    if (insertOpenAndCloseTag) newLevel++;
                    StringBuilder subSb = new StringBuilder();

                    if (node.HasChildNodes)
                    {
                        foreach (var ch in node.ChildNodes)
                        {
                            RenderNode(ch, subSb, newLevel, levelInsert);
                        }
                    }

                    String inner = subSb.ToString();
                    Boolean hasInnerContent = false;
                    if (inner.Length > 0)
                    {
                        hasInnerContent = true;
                        if (multiLineFormat == 0)
                        {
                            if (ContainsNonTextNodes(node))
                            {
                                multiLineFormat = 1;
                            } else
                            {
                                if (inner.Contains(Environment.NewLine))
                                {
                                    multiLineFormat = 1;
                                }
                                else
                                {
                                    multiLineFormat = -1;
                                }
                            }
                        }
                    } else
                    {
                        if (multiLineFormat == 0)
                        {
                            multiLineFormat = -1;
                        }
                    }

                    String open_line = "";
                    String close_line = "";

                    if (insertOpenAndCloseTag)
                    {
                        open_line = RenderNodeOpening(node, "", hasInnerContent);
                        if (hasInnerContent) close_line = "</" + node.Name.ToLower() + ">";
                    }

                   
                    if (insertOpenAndCloseTag)
                    {
                        if (multiLineFormat == 1)
                        {
                            if (hasInnerContent)
                            {
                                inner = open_line + Environment.NewLine + inner + Environment.NewLine + close_line;
                            } else
                            {
                                inner = open_line;
                            }
                        } else
                        {
                            if (hasInnerContent)
                            {
                                inner = open_line + inner + close_line;
                            } else
                            {
                                inner = open_line;
                            }
                        }
                    } else
                    {
                        
                    }

                    if (multiLineFormat == 1)
                    {
                        var lns = inner.SplitSmart(Environment.NewLine);
                        foreach (var ln in lns)
                        {
                            sb.AppendLine(prefix + ln);
                        }
                    } else
                    {
                        sb.Append(inner);
                    }
                    

                    /*
                    if (hasInnerContent)
                    {
                        if (inner.Contains(Environment.NewLine))
                        {
                            //if (insertOpenAndCloseTag) sb.AppendLine(open_line);

                            

                            //if (insertOpenAndCloseTag) sb.AppendLine(close_line);

                        } else
                        {
                            //if (insertOpenAndCloseTag) sb.Append(open_line);
                            sb.Append(inner);
                            
                            //if (insertOpenAndCloseTag) sb.Append(close_line);
                        }
                        
                    } else
                    {
                       // if (insertOpenAndCloseTag) sb.AppendLine(open_line);
                    }*/

                    //if (insertOpenAndCloseTag)
                    //{
                    //    if (hasInnerContent)
                    //    {
                    //        if (!inner.Contains(Environment.NewLine))
                    //        {

                    //        }
                    //        else
                    //        {
                    //            sb.AppendLine(prefix + "</" + node.Name.ToLower() + ">");
                    //        }

                    //    }
                    //}

                    break;
                case HtmlNodeType.Text:
                    RenderTextNode(node, sb);
                    break;
                default:
                    break;
            }

           // return wasRendered;

        }

        /// <summary>
        /// Reduces the document.
        /// </summary>
        /// <param name="htmlInput">The HTML input.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="logger">The logger.</param>
        /// <returns></returns>
        public String ReduceDocument(String htmlInput)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.OptionFixNestedTags = true;
            htmlDocument.OptionAutoCloseOnEnd = true;
            
            
            htmlDocument.LoadHtml(htmlInput);

            List<HtmlNode> htmlNodes = htmlDocument.DocumentNode.ChildNodes.ToList();


            var tagNameReplacements = settings.tagNameReplacement.GetDictionary();
            var attWithValueToRemove = settings.attributeWithValueToRemove.GetDictionary();

            // first phase
            while (htmlNodes.Any())
            {
                List<HtmlNode> nextIteration = new List<HtmlNode>();

                foreach (HtmlNode node in htmlNodes)
                {

                    String nodeName = node.Name.ToLower();

                    if (tagNameReplacements.ContainsKey(nodeName))
                    {
                        node.Name = tagNameReplacements[nodeName].value;
                        nodeName = tagNameReplacements[nodeName].value;
                    }


                    if (settings.tagsToRemove.Contains(nodeName)) //.Any(x => x.Equals(node.Name, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        node.Remove();
                    } 
                    else
                    {
                        Boolean addToNextIteration = true;

                        if (settings.emptyTagsToRemove.Contains(nodeName)) //.Any(x=>x.Equals(node.Name, StringComparison.InvariantCultureIgnoreCase))) {
                        { 
                            if (IsNodeEmpty(node))
                            {
                                node.Remove();
                                addToNextIteration = false;
                            }
                        }

                        if (settings.tagsToRemoveAllAttributes.Contains(nodeName)) //.Any(x=>x.Equals(node.Name, StringComparison.InvariantCultureIgnoreCase))) {
                        {
                            node.Attributes.RemoveAll();
                        }


                        if (addToNextIteration)
                        {
                            nextIteration.Add(node);

                            foreach (var attribute in node.Attributes.ToList())
                            {
                                String attributeName = attribute.Name.ToLower();

                                if (settings.attributesToRemove.Contains(attributeName)) //.Any(x => x.Equals(attribute.Name, StringComparison.InvariantCultureIgnoreCase)))
                                {
                                    attribute.Remove();
                                    
                                }

                                if (attribute.Value.isNullOrEmpty())
                                {
                                    //attribute.Value = " ";
                                    //attribute.Remove();
                                    
                                } else
                                {
                                    if (attWithValueToRemove.ContainsKey(attributeName))
                                    {
                                        if (attribute.Value.toStringSafe() == attWithValueToRemove[attributeName].value)
                                        {
                                            attribute.Remove();
                                        }
                                    }
                                }
                            }
                        }

                        
                    }
                }

                htmlNodes = new List<HtmlNode>();
                foreach (HtmlNode node in nextIteration)
                {
                    htmlNodes.AddRange(node.ChildNodes.ToList());
                }
            }
            
            String outputHtml = htmlDocument.DocumentNode.OuterHtml;

            if (settings.ReduceEmptySpace)
            {
                outputHtml = HtmlEntity.DeEntitize(outputHtml);



                outputHtml = REGEX_SELECTCOMMENTS.Replace(outputHtml, "");

                

                outputHtml = REGEX_EMPTYSPACE.Replace(outputHtml, ">" + Environment.NewLine + "<");

                outputHtml = outputHtml.Replace("><", ">" + Environment.NewLine + "<");

                String doubleNewLine = Environment.NewLine + Environment.NewLine;

                Int32 i = 0;
                while (outputHtml.IndexOf(doubleNewLine) > 0)
                {
                    outputHtml = outputHtml.Replace(doubleNewLine, Environment.NewLine);
                    i++;

                    if (i > 100)
                    {
                        break;
                    }
                }

                
            }

            if (settings.RebuildHtml)
            {
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(outputHtml);

                StringBuilder sb = new StringBuilder();

                RenderNode(document.DocumentNode, sb, 0);

                outputHtml = sb.ToString();

            }

            if (settings.InsertReductionSignature)
            {
                String headerComment = RenderCommentNode("imbSCI.DataExtraction - Reduced HTML document");
                outputHtml = headerComment + Environment.NewLine + outputHtml;
            }

            return outputHtml;

        }



    }
}