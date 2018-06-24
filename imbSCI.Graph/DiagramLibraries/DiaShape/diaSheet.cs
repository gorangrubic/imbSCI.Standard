using imbSCI.Core.files;
using imbSCI.Core.files.folders;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace imbSCI.Graph.DiagramLibraries.DiaShape
{
    /// <summary>
    /// Localization sheet for Dia
    /// </summary>
    [XmlRoot(ElementName = "sheet", Namespace = "http://www.lysator.liu.se/~alla/dia/dia-sheet-ns")]
    public class diaSheet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="diaSheet"/> class.
        /// </summary>
        public diaSheet()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="diaSheet"/> class.
        /// </summary>
        /// <param name="__name">The name.</param>
        /// <param name="__description">The description.</param>
        public diaSheet(String __name, String __description)
        {
            name = __name;
            description = __description;
        }

        private String _name = "";

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [XmlIgnore]
        public String name
        {
            get
            {
                if (!names.Any())
                {
                    names[0] = new xmlTextLocaleEntry("NewSheet");
                }

                return names[0].text;
            }
            set
            {
                if (!names.Any())
                {
                    names = new xmlTextLocaleEntry[] { new xmlTextLocaleEntry(value) };
                }
                else
                {
                    names[0].text = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [XmlIgnore]
        public String description
        {
            get
            {
                if (!descriptions.Any())
                {
                    descriptions[0] = new xmlTextLocaleEntry("");
                }

                return descriptions[0].text;
            }
            set
            {
                if (!descriptions.Any())
                {
                    descriptions = new xmlTextLocaleEntry[] { new xmlTextLocaleEntry(value) };
                }
                else
                {
                    descriptions[0].text = value;
                }
            }
        }

        /// <summary>
        /// Loads the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static diaSheet Load(String path)
        {
            diaSheet output = objectSerialization.loadObjectFromXML<diaSheet>(path);

            return output;
        }

        /// <summary>
        /// Saves the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        public void Save(String path)
        {
            objectSerialization.saveObjectToXML(this, path);
        }

        /// <summary>
        /// Saves the specified folder.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="description">The description.</param>
        /// <returns></returns>
        public String Save(folderNode folder, String filename, String description = "")
        {
            filename = filename.ensureEndsWith(".sheet");
            String path = folder.pathFor(filename, Data.enums.getWritableFileMode.newOrExisting, description, true);

            objectSerialization.saveObjectToXML(this, path);
            return path;
        }

        //[XmlArrayItem(ElementName = "name")]
        //public List<xmlTextLocaleEntry> names { get; set; } = new List<xmlTextLocaleEntry>();

        [XmlElement(ElementName = "name")]
        public xmlTextLocaleEntry[] names { get; set; } = new xmlTextLocaleEntry[] { };

        [XmlElement(ElementName = "description")]
        public xmlTextLocaleEntry[] descriptions { get; set; } = new xmlTextLocaleEntry[] { };

        //[XmlArrayItem(ElementName = "description")]
        //public List<xmlTextLocaleEntry> descriptions { get; set; } = new List<xmlTextLocaleEntry>();

        [XmlArray(ElementName = "contents")]
        [XmlArrayItem(ElementName = "object")]
        public List<diaSheetObject> contents { get; set; } = new List<diaSheetObject>();
    }
}