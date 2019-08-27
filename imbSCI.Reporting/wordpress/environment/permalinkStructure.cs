using imbSCI.Core.extensions.io;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.IO;

namespace imbSCI.Reporting.wordpress.environment
{

    /// <summary>
    /// Permalink generator - used to predict link of a document in the final deployement
    /// </summary>
    public class permalinkStructure
    {
        private String _customStructure = @"/index.php/%year%/%monthnum%/%day%/%postname%/";

        public enum permalinkElements
        {
            year,
            monthnum,
            day,
            hour,
            minute,
            second,
            post_id,
            postname,
            category,
            author
        }

        public String customStructure
        {
            get { return _customStructure; }
            set
            {
                _customStructure = value;
                DeployStructure(_customStructure);
            }
        }

        public String GetSlug(String input)
        {
            String o = input.ToLower().Replace(" ", "-").Replace(Path.DirectorySeparatorChar.ToString(), "");
            o = o.Replace("č", "c");
            o = o.Replace("ć", "c");
            o = o.Replace("š", "š");
            o = o.Replace("ž", "z");
            o = o.Replace("đ", "d");
            o = o.getFilename();

            return o;

        }

        public String UNCATEGORIZED_NAME { get; set; } = "uncategorized";
        /// <summary>
        /// Gets the permalink.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="date">The date.</param>
        /// <param name="extraData">The extra data.</param>
        /// <returns></returns>
        public String GetPermalink(reportDocument document, DateTime date = default(DateTime), Dictionary<permalinkElements, String> extraData = null)
        {
            String output = customStructure;
            foreach (String str_form in elementsUsed)
            {
                if (output.Contains(str_form))
                {
                    permalinkElements element = elementsByString[str_form];
                    String replacement = "";

                    switch (element)
                    {
                        case permalinkElements.category:
                            String p = GetSlug(document.GetCategoryPath(true, true));
                            if (p.isNullOrEmpty()) p = UNCATEGORIZED_NAME;
                            replacement = p;
                            break;
                        case permalinkElements.day:
                            replacement = date.ToString("dd");
                            break;
                        case permalinkElements.hour:
                            replacement = date.ToString("hh");
                            break;
                        case permalinkElements.minute:
                            replacement = date.ToString("mm");
                            break;
                        case permalinkElements.monthnum:
                            replacement = date.ToString("MM");
                            break;
                        case permalinkElements.post_id:
                            break;
                        case permalinkElements.postname:
                            replacement = GetSlug(document.Title);
                            break;
                        case permalinkElements.second:
                            replacement = date.ToString("s");
                            break;
                        case permalinkElements.year:
                            replacement = date.ToString("yyyy");
                            break;
                        default:
                            if (extraData != null)
                            {
                                if (extraData.ContainsKey(element))
                                {
                                    replacement = extraData[element];
                                }
                            }
                            break;
                    }


                    output = output.Replace(str_form, replacement);
                }

            }

            return output;

        }

        List<String> elementsUsed { get; set; } = new List<string>();

        private void DeployStructure(String structure)
        {
            elementsUsed = new List<string>();

            foreach (String element in elementsAll)
            {
                if (structure.Contains(element))
                {
                    elementsUsed.Add(element);
                }

            }

        }

        List<String> elementsAll { get; set; } = new List<string>();

        Dictionary<String, permalinkElements> elementsByString { get; set; } = new Dictionary<string, permalinkElements>();

        private void Deploy()
        {
            var el = Enum.GetValues(typeof(permalinkElements));

            foreach (permalinkElements element in el)
            {
                String str_form = "%" + element.ToString() + "%";
                elementsAll.Add(str_form);
                elementsByString.Add(str_form, element);
            }
        }

        public permalinkStructure()
        {
            Deploy();
            DeployStructure(_customStructure);
        }

    }
}
