using HtmlAgilityPack;
using imbSCI.Core.files;
using imbSCI.Core.files.folders;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace imbSCI.DataExtraction.Tools
{
    public static class HtmlSourceTools
    {
        public static String GetHtml(this List<HtmlNode> nodes, Boolean WrapInHtmlDocument, String headInsert="")
        {
            StringBuilder sb = new StringBuilder();

            if (WrapInHtmlDocument)
            {
                sb.AppendLine("<html>");
                sb.AppendLine("<head>");
                sb.AppendLine(headInsert);
                sb.AppendLine("</head>");
                sb.AppendLine("<body>");
            }

            foreach (HtmlNode node in nodes)
            {
                if (WrapInHtmlDocument)
                {
                    sb.AppendLine("<div>");
                    sb.AppendLine("<h1>");
                    sb.AppendLine(node.XPath);
                    sb.AppendLine("</h1>");
                    sb.AppendLine("<div>");
                }
                sb.AppendLine(node.OuterHtml);

                if (WrapInHtmlDocument)
                {
                    sb.AppendLine("</div>");
                    sb.AppendLine("</div>");
                }
            }


            if (WrapInHtmlDocument)
            {
                sb.AppendLine("</body>");
                sb.AppendLine("</html>");
            }

            return sb.ToString();
        }

        public static List<HtmlSourceAndUrl> MergeAll(this IEnumerable<HtmlSourceAndUrlCollection> inputs)
        {
            List<HtmlSourceAndUrl> output = new List<HtmlSourceAndUrl>();

            foreach (var c in inputs)
            {
                output.AddRange(c.GetAllItems());
            }

            return output;
        }


        public const String HTML_EXTENSION = "html";
        public const String URL_EXTENSION = "txt";

        public static String Save(HtmlDocument htmlDocument, String url, folderNode folder, String filename = "htmlsource")
        {
            HtmlSourceAndUrl item = new HtmlSourceAndUrl()
            {
                html = htmlDocument.DocumentNode.OuterHtml,
                url = url
            };

            return Save(item, folder, filename);
        }

        public static void Save(this HtmlSourceAndUrlCollection sources, folderNode folder, String filename = "htmlsource", Boolean deleteExisting=true)
        {

            if (deleteExisting) folder.deleteFiles();

            if (filename.isNullOrEmpty())
            {
                Int32 c = 0;
                foreach (var s in sources.items)
                {
                    s.Save(folder, c.ToString());
                    c++;
                }
            } else
            {
                foreach (var s in sources.items)
                {
                    s.Save(folder, filename);
                }
            }
            

            foreach (var sb in sources.SubCollections)
            {
                var f = folder.Add(sb.name, sb.name, "HTML sources subcollection of " + sources.name + ".");
                sb.Save(f, filename);
                
            }
        }

        /// <summary>
        /// Saves the specified item
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="folder">The folder.</param>
        /// <param name="filename">The filename.</param>
        public static String Save(this HtmlSourceAndUrl item, folderNode folder, String filename = "htmlsource")
        {
            String outputPath = folder.pathFor(filename + "." + HTML_EXTENSION, Data.enums.getWritableFileMode.autoRenameThis, "Saved HTML document");

            File.WriteAllText(outputPath, item.html);

            String name = Path.GetFileNameWithoutExtension(outputPath);

            String urlContent = item.url;

            outputPath = folder.pathFor(filename + "." + URL_EXTENSION, Data.enums.getWritableFileMode.autoRenameThis, "Saved origin url for HTML document");

            File.WriteAllText(outputPath, urlContent);
            return outputPath;
        }

        public static Regex Regex_SavedFromUrl = new Regex(@"<!-- saved from url=\([\d]+\)([\w\d\.:/\-\?_=%&]+) -->", RegexOptions.Multiline);

        public static String DetectOriginURL(String htmlSource)
        {
            String url = "";

            var m = Regex_SavedFromUrl.Match(htmlSource);
            if (m.Success)
            {
                url = m.Groups[0].Value;
            }

            return url;
        }

        /// <summary>
        /// Sources indexed by root node of their documents
        /// </summary>
        /// <param name="sources">The sources.</param>
        /// <returns></returns>
        public static Dictionary<HtmlNode, HtmlSourceAndUrl> GetDocumentNodeDictionary(this IEnumerable<HtmlSourceAndUrl> sources)
        {
            Dictionary<HtmlNode, HtmlSourceAndUrl> output = new Dictionary<HtmlNode, HtmlSourceAndUrl>();
            foreach (var s in sources)
            {
                output.Add(s.htmlDocument.DocumentNode, s);
            }
            return output;

        }


        public static HtmlSourceAndUrlCollection LoadAllInSubfolders(this folderNode folder, String filename = "htmlsource", Boolean removeIncompleteEntries = true, Boolean removeEmptyDocuments = true)
        {
            HtmlSourceAndUrlCollection output = LoadAll(folder, filename, removeIncompleteEntries,removeEmptyDocuments);

            DirectoryInfo directory = folder;
            

            var allFiles = directory.GetFiles(filename + "*.html", SearchOption.AllDirectories);
            List<DirectoryInfo> subdirectories = new List<DirectoryInfo>();
            foreach (FileInfo fi in allFiles)
            {
                if (!subdirectories.Any(x=>x.FullName==fi.DirectoryName))
                {
                    if (fi.Directory.FullName != directory.FullName)
                    {
                        subdirectories.Add(fi.Directory);
                    }
                }
            }

            foreach (folderNode subfolder in subdirectories)
            {
                output.SubCollections.Add(LoadAll(subfolder, filename, removeIncompleteEntries,removeEmptyDocuments));
            }

            return output;
        }


        /// <summary>
        /// Loads all items from folder
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public static HtmlSourceAndUrlCollection LoadAll(this folderNode folder, String filename = "htmlsource", Boolean removeIncompleteEntries = true, Boolean setLocalFilepath=false, Boolean removeEmptyDocuments = true)
        {
            var files = folder.findFiles(filename + "*.*", SearchOption.TopDirectoryOnly);

            Dictionary<String, HtmlSourceAndUrl> loadedDictionary = new Dictionary<string, HtmlSourceAndUrl>();
            HtmlSourceAndUrlCollection output = new HtmlSourceAndUrlCollection();
            output.SetSourceInfo(folder, SearchOption.TopDirectoryOnly);

            foreach (String filepath in files)
            {
                var fn = Path.GetFileNameWithoutExtension(filepath);

                if (!loadedDictionary.ContainsKey(fn))
                {
                    loadedDictionary.Add(fn, new HtmlSourceAndUrl());
                }

                Load(filepath, loadedDictionary[fn]);
            }

            // detecting URL for not loaded one
            foreach (var pair in loadedDictionary)
            {
                if (pair.Value.url.isNullOrEmpty())
                {
                    pair.Value.url = DetectOriginURL(pair.Value.html);
                }
            }


            if (removeIncompleteEntries)
            {
                foreach (var pair in loadedDictionary)
                {
                    if (pair.Value.IsComplete)
                    {
                        output.items.Add(pair.Value);
                    }
                }
            }
            else
            {
                output.items.AddRange(loadedDictionary.Values);
            }

            if (setLocalFilepath)
            {
                foreach (var item in output.items)
                {
                    item.filepath = item.filepath.removeStartsWith(folder.path);
                }
            }

            if (removeEmptyDocuments)
            {
                foreach (var item in output.items.ToList())
                {
                    if (item.html.isNullOrEmpty())
                    {
                        output.items.Remove(item);
                    }
                }
            }

            return output;
        }


        public static HtmlSourceAndUrl LoadSource(String filepath)
        {
            String filename = Path.GetFileNameWithoutExtension(filepath);
            String directory = Path.GetDirectoryName(filepath);

            DirectoryInfo di = new DirectoryInfo(directory);
            FileInfo[] filenames = di.GetFiles(filename + ".*", SearchOption.TopDirectoryOnly);
            var output = new HtmlSourceAndUrl();

            foreach (var file in filenames)
            {
                Load(file.FullName, output);
            }

            return output;
        }

        /// <summary>
        /// Loads HTML or URL information from specified filepath, and stores to <c>output</c>
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        /// <param name="output">The output.</param>
        /// <returns></returns>
        internal static HtmlSourceAndUrl Load(String filepath, HtmlSourceAndUrl output = null)
        {
            if (output == null) output = new HtmlSourceAndUrl();

            String ext = Path.GetExtension(filepath);

            ext = ext.Trim('.').ToLower();

            switch (ext)
            {
                case HTML_EXTENSION:
                    output.html = File.ReadAllText(filepath);
                    output.filepath = filepath;
                    break;

                case URL_EXTENSION:
                    output.url = File.ReadAllText(filepath);
                    break;
                default:
                    break;
            }

            return output;
        }
    }
}