using imbSCI.Core.extensions.text;
using imbSCI.Core.reporting.render;
using System;
using System.Collections.Generic;
using System.IO;

namespace imbSCI.Reporting.wordpress
{
    public static class reportDocumentTools
    {



        //public static void TakeContent(this IReportDocument document)
        //{

        //    base.TakeContent();

        //    foreach (reportDocument doc in posts)
        //    {
        //        doc.TakeContent();
        //    }
        //}
        public static reportDocument AddChild(this IReportDocument parent, reportDocumentType postType, String title, ITextRender logger = null, String _bid = "")
        {
            reportDocument output = new reportDocument(postType)
            {
                Title = title,
                BID = _bid.or(parent.BID),
            };

            if (parent is reportDocument parentReportDocument)
            {
                output.parent = parentReportDocument;
                if (logger != null)
                {
                    logger.AppendLine("New [" + postType.ToString() + "] [" + title + "] created under [" + parentReportDocument.Title + "]");
                }
            }


            parent.Children.Add(output);



            return output;
        }

        public static String GetCategoryPath(this reportDocument document, Boolean skipSelf = true, Boolean allowRootDocument = true)
        {
            List<reportDocument> categories = document.GetCategories(skipSelf, allowRootDocument);

            String output = "";

            foreach (var cat in categories)
            {
                if (output == "")
                {
                    output = cat.Title;
                }
                else
                {
                    output = cat.Title + Path.DirectorySeparatorChar + output;
                }
            }

            return output;
        }

        public static void SetParents(this reportRootDocument rootDocument)
        {

            List<reportDocument> documents = new List<reportDocument>() { rootDocument };
            List<reportDocument> nextIteration = new List<reportDocument>();

            while (documents.Count > 0)
            {
                nextIteration = new List<reportDocument>();

                foreach (reportDocument item in documents)
                {
                    foreach (reportDocument subitem in item.Children)
                    {
                        subitem.parent = item;
                    }

                    nextIteration.AddRange(item.Children);
                }

                documents = nextIteration;
            }



        }



        /// <summary>
        /// Gets the categories> from leaf to root
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="skipSelf">if set to <c>true</c> [skip self].</param>
        /// <param name="allowRootDocument">if set to <c>true</c> [allow root document].</param>
        /// <returns></returns>
        public static List<reportDocument> GetCategories(this reportDocument document, Boolean skipSelf = true, Boolean allowRootDocument = true)
        {
            reportDocument head = document.GetCategory(skipSelf, allowRootDocument);

            List<reportDocument> output = new List<reportDocument>();

            while (head != null)
            {
                output.Add(head);
                var new_head = head.GetCategory(true, allowRootDocument);
                if (new_head == head) break;
                head = new_head;
            }
            return output;
        }

        /// <summary>
        /// Gets the category.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="skipSelf">in case <c>document</c> is a category, if set to <c>true</c> it will not include it into consideration</param>
        /// <param name="allowRootDocument">if set to <c>true</c> [allow root document].</param>
        /// <returns></returns>
        public static reportDocument GetCategory(this reportDocument document, Boolean skipSelf = true, Boolean allowRootDocument = true)
        {
            reportDocument head = document;
            if (document.PostType == reportDocumentType.category)
            {
                if (skipSelf)
                {
                    head = head.parent;
                }
            }

            while (true)
            {
                if (head == null)
                {
                    break;
                }
                if (head.PostType == reportDocumentType.category)
                {
                    return head;
                }
                if (head.parent == null)
                {
                    if (allowRootDocument)
                    {
                        return head;
                    }
                    else
                    {
                        return null;
                    }
                }
                head = head.parent;
            }

            return null;
        }
    }
}