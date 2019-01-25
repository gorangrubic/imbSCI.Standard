// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Document.cs" company="imbVeles" >
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

using System.Collections;
using System.Xml;

namespace imbSCI.Graph.Graphics.SvgDocument
{
    public class Document : ISVGTranform
    {
        #region Attributes

        private readonly XmlDocument svgDoc;
        private int width;
        private int height;
        private ArrayList svgObjects;

        #endregion Attributes

        #region Properties

        public ArrayList SvgObjects
        {
            get
            {
                return this.svgObjects;
            }
            set
            {
                this.svgObjects = value;
            }
        }

        #endregion Properties

        #region Constructor

        public Document(int _width, int _height)
        {
            this.width = _width;
            this.height = _height;
            this.svgObjects = new ArrayList();
            this.svgDoc = new XmlDocument();
            this.svgDoc.AppendChild(this.svgDoc.CreateXmlDeclaration("1.0", "utf-8", "no"));
            this.svgDoc.LoadXml("<svg></svg>");
            XmlNode xmlObjects = this.svgDoc.CreateElement("svgObjects");
            XmlNode xmlDetails = this.svgDoc.CreateElement("svgDetails");
            XmlNode xmlHeight = this.svgDoc.CreateElement("height");
            XmlNode xmlWidth = this.svgDoc.CreateElement("width");
            xmlWidth.InnerText = this.width.ToString();
            xmlHeight.InnerText = this.height.ToString();
            xmlDetails.AppendChild(xmlWidth);
            xmlDetails.AppendChild(xmlHeight);
            this.svgDoc.FirstChild.AppendChild(xmlObjects);
            this.svgDoc.FirstChild.AppendChild(xmlDetails);
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get a XmlDocument with all details of the svg document.
        /// </summary>
        /// <returns>XmlDocument</returns>
        public XmlNode ToXml()
        {
            XmlNode objects = this.svgDoc.SelectSingleNode("//svgObjects");
            foreach (ISVGTranform obj in this.svgObjects)
                objects.AppendChild(this.svgDoc.ImportNode(obj.ToXml(), true));

            return this.svgDoc;
        }

        #endregion Methods
    }
}