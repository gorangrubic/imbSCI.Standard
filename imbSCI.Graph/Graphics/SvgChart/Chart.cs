// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Chart.cs" company="imbVeles" >
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

using imbSCI.Graph.Graphics.SvgDocument;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace imbSCI.Graph.Graphics.SvgChart
{
    public abstract class Chart
    {
        #region Attributes

        protected readonly int width;
        protected readonly int height;
        protected readonly int legendItems;
        protected Color[] colors;
        private System.Drawing.Rectangle legendArea;
        private System.Drawing.Rectangle chartArea;
        public Document doc;

        #endregion Attributes

        #region Constants

        private const short leftMargin = 10;
        private const short rightMargin = 10;
        private const short topMargin = 10;
        private const short bottomMargin = 25;
        private const short rectLegendWidth = 10;
        private const short rectLegendHeight = 10;
        private const short rectLegendSpace = 5;
        private const short maxLengthLegendText = 15;
        protected const short weightChar = 8;

        #endregion Constants

        #region Properties

        #region Legend Area

        public System.Drawing.Rectangle LegendArea
        {
            get
            {
                return this.legendArea;
            }
        }

        #endregion Legend Area

        #region Chart Area

        public System.Drawing.Rectangle ChartArea
        {
            get
            {
                return this.chartArea;
            }
        }

        #endregion Chart Area

        public Color[] Colors
        {
            get
            {
                return this.colors;
            }
        }

        public Document Doc
        {
            get
            {
                return this.doc;
            }
        }

        #endregion Properties

        #region Constructor

        public Chart(int _width, int _height, int _legendItems)
        {
            this.width = _width;
            this.height = _height;
            this.legendItems = _legendItems;
            this.doc = new Document(this.width, this.height);
            this.CalcLegendArea();
            Random rnd;
            this.colors = new System.Drawing.Drawing2D.ColorBlend(this.legendItems).Colors;
            if (this.GetType() != typeof(LinearChart))
            {
                for (int i = 0; i < this.colors.Length; i++)
                {
                    rnd = new Random(i);
                    this.colors[i] = System.Drawing.Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
                }
            }
            this.CalcChartArea();
        }

        #endregion Constructor

        #region Methods

        #region Private Methods

        /// <summary>
        /// Calculate the Legend Area
        /// </summary>
        private void CalcLegendArea()
        {
            short rows = 0;
            int yStartPos = this.height - 20;
            if (this.GetType() != typeof(LinearChart))
            {
                rows = 1;
                if (((rectLegendHeight + rectLegendSpace) * this.legendItems) > (this.height - 40))
                    rows++;
            }

            this.legendArea = new System.Drawing.Rectangle(leftMargin, topMargin, ((maxLengthLegendText * weightChar / 2) + rectLegendWidth + rectLegendSpace) * rows, this.height - topMargin - bottomMargin);
        }

        /// <summary>
        /// Calculate the Chart Area
        /// </summary>
        private void CalcChartArea()
        {
            int xStartPos = (this.legendArea.X + this.legendArea.Width) + 10;
            int yStartPos = this.legendArea.Y;
            this.chartArea = new System.Drawing.Rectangle(xStartPos, yStartPos, (this.width - rightMargin - xStartPos), (this.height - topMargin - bottomMargin));
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Get a XmlDocument with the svg's transformed document.
        /// </summary>
        /// <param name="_data">DataTable</param>
        /// <param name="_colNames">string</param>
        /// <param name="_colValues">string</param>
        /// <returns>XmlDocument</returns>
        public virtual XmlDocument GenerateChart(DataTable _data, string _colNames, string _colValues)
        {
            return (XmlDocument)this.doc.ToXml();
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Creates the legend item for a value.
        /// </summary>
        /// <param name="text">string</param>
        /// <param name="indexColumn">int</param>
        /// <param name="indexRow">int</param>
        /// <param name="indexColor">int</param>
        protected virtual void GenerateLegendItem(string text, ref int indexColumn, ref int indexRow, int indexColor)
        {
            int x = 0;
            int y = 0;

            Text txt;
            if (this.LegendArea.Y + ((rectLegendSpace + rectLegendHeight) * indexRow) > (this.legendArea.Y + this.legendArea.Height))
            {
                indexColumn++;
                indexRow = 0;
            }

            x = this.legendArea.X + (indexColumn * ((maxLengthLegendText * (weightChar / 2)) + rectLegendSpace * 4));
            y = this.LegendArea.Y + ((rectLegendSpace + rectLegendHeight) * indexRow);

            SvgDocument.Rectangle rect = new SvgDocument.Rectangle(rectLegendWidth, rectLegendHeight);
            rect.X = x;
            rect.Y = y;
            rect.Color = this.colors[indexColor];
            this.doc.SvgObjects.Add(rect);

            text = text.ToLower();
            if (text.Length > 15)
                text = text.Substring(0, maxLengthLegendText);
            txt = new Text(text);
            txt.X = x + rectLegendWidth + rectLegendSpace;
            txt.Y = y + rectLegendHeight;
            txt.Font_Size = weightChar;
            this.doc.SvgObjects.Add(txt);
        }

        /// <summary>
        /// Parse de XmlDocument with the svg objects to a well formated SVG Document.
        /// </summary>
        /// <returns>XmlDocument</returns>
        protected XmlDocument Transform()
        {
            XmlDocument xmlResult = new XmlDocument();
            XslCompiledTransform xslt = new XslCompiledTransform();

            DirectoryInfo di = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            FileInfo[] found = di.GetFiles("Charts.xslt", SearchOption.AllDirectories);

            FileInfo path = found.FirstOrDefault();

            if (path != null)
            {
                xslt.Load(path.FullName);
                XPathNavigator nav = this.Doc.ToXml().CreateNavigator();
                StringBuilder sw = new StringBuilder();
                XmlWriter xr = XmlWriter.Create(sw, null);
                xslt.Transform(nav, xr);
                xmlResult.LoadXml(sw.ToString());
            }
            else
            {
                throw new FileNotFoundException("SVG Charting: Charts.xslt file not found in directory:" + di.FullName);
            }
            return xmlResult;
        }

        #endregion Protected Methods

        #endregion Methods
    }
}