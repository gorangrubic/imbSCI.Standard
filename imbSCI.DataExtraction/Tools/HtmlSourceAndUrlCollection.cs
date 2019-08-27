using HtmlAgilityPack;
using imbSCI.Core.files.folders;
using imbSCI.Data;
using imbSCI.Data.interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.DataExtraction.Tools
{
    public class HtmlSourceAndUrlCollection:IObjectWithName
    {
        public HtmlSourceAndUrlCollection()
        {

        }

     

        public String name { get; set; }

        public void SetSourceInfo(folderNode _sourceFolder, SearchOption _searchOption)
        {
            searchOption = _searchOption;
            sourceFolder = _sourceFolder;
            name = _sourceFolder.name;
        }

        /// <summary>
        /// Number of items, including all subcollections
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public Int32 Count
        {
            get
            {
                Int32 output = items.Count;
                foreach (var i in SubCollections)
                {
                    output += i.Count;
                }
                return output;
            }
        }

        /// <summary>
        /// Gets all items - including the subcollection treee
        /// </summary>
        /// <returns></returns>
        public List<HtmlSourceAndUrl> GetAllItems()
        {
            List<HtmlSourceAndUrl> output = new List<HtmlSourceAndUrl>();
            output.AddRange(items);

            foreach (var sub in SubCollections)
            {
                output.AddRange(sub.GetAllItems());
            }
            return output;
        }


        public List<HtmlSourceAndUrlCollection> SubCollections { get; set; } = new List<HtmlSourceAndUrlCollection>();

        public SearchOption searchOption { get; protected set; } = SearchOption.TopDirectoryOnly;

        [XmlIgnore]
        public folderNode sourceFolder { get; protected set; } 

        public List<HtmlSourceAndUrl> items { get; set; } = new List<HtmlSourceAndUrl>();
    }
}