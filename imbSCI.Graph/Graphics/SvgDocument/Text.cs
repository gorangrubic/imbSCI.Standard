// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Text.cs" company="imbVeles" >
//
// Copyright (C) 2018 imbVeles
//
// This program is free software: you can redistribute it and/or modify
// it under the +terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/.
// </copyright>
// <summary>
// Project: imbSCI.Graph
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------

//AUTHOR	: GERARD CASTELLÓ
//DATE		: 17/JUN/2010

using System.Xml;

namespace imbSCI.Graph.Graphics.SvgDocument
{
    public class Text : SVGObject, ISVGTranform
    {
        #region Attributes and Constants

        private string value;
        private double font_size;
        private ushort rotate;
        private const double FONT_SIZE = 8.5;

        #endregion Attributes and Constants

        #region Properties

        public double Font_Size
        {
            get
            {
                return this.font_size;
            }
            set
            {
                this.font_size = value;
            }
        }

        public ushort Rotate
        {
            get
            {
                if (this.rotate > 360)
                    return 360;
                else
                    return this.rotate;
            }
            set
            {
                this.rotate = value;
            }
        }

        #endregion Properties

        #region Constructor

        public Text(string _value)
        {
            this.value = _value;
            this.font_size = FONT_SIZE;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get a XmlNode with all details of this object
        /// </summary>
        /// <returns>XmlNode</returns>
        public XmlNode ToXml()
        {
            XmlDocument xmlText = new XmlDocument();
            xmlText.LoadXml("<text></text>");
            xmlText.FirstChild.InnerText = this.value;
            xmlText = base.GetNode(xmlText);
            XmlAttribute xmlFont_Size = xmlText.CreateAttribute("font-size");
            XmlAttribute xmlRotate = xmlText.CreateAttribute("rotate");
            xmlFont_Size.Value = this.font_size.ToString();
            xmlRotate.Value = this.rotate.ToString();
            xmlText.FirstChild.Attributes.Append(xmlFont_Size);
            xmlText.FirstChild.Attributes.Append(xmlRotate);
            return xmlText.FirstChild;
        }

        #endregion Methods
    }
}