using imbSCI.Core.interfaces;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace imbSCI.BusinessData.Models.Organization
{
    public class BusinessActivityDefinition:IFromString
    {
        public static Regex SELECT_DEFINITION = new Regex(@"([A-Z]?)[\s]*([\d\.]+)[\s]*\-[\s]*([\w\,\.\-\;\s\d]+)\Z");

        public BusinessActivityDefinition()
        {
        }

        public void FromString(String input)
        {
            Match mch = SELECT_DEFINITION.Match(input);
            if (mch.Groups.Count > 2)
            {
                Group = mch.Groups[1].Value;
                Code = mch.Groups[2].Value;
                Description = mch.Groups[3].Value;
            }
        }

        public override string ToString()
        {
            return Group + " " + Code + " - " + Description;
        }

        public String Group { get; set; } = "";

        /// <summary>
        /// <see cref="Group"/> + <see cref="Code"/>
        /// </summary>
        /// <value>
        /// The code full.
        /// </value>
        [XmlIgnore]
        public String CodeFull { get { return Group + Code; } }

        public String Code { get; set; } = "";

        [XmlIgnore]
        public String FullTitle { get; set; } 

        public String Description { get; set; } = "";
    }
}