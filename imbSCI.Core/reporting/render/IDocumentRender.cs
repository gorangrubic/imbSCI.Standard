// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDocumentRender.cs" company="imbVeles" >
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
// Project: imbSCI.Core
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Core.reporting.render
{
    using imbSCI.Core.enums;
    using imbSCI.Core.reporting.colors;
    using imbSCI.Core.reporting.format;
    using imbSCI.Core.reporting.style;
    using imbSCI.Core.reporting.style.core;
    using imbSCI.Core.reporting.style.shot;
    using imbSCI.Core.reporting.zone;
    using imbSCI.Data.enums.fields;
    using System;
    using System.Data;
    using System.IO;

    /// <summary>
    /// Interface for Document render classes
    /// </summary>
    /// <seealso cref="ITabLevelControler" />
    /// <seealso cref="ITextRender" />
    /// \ingroup_disabled report_int
    public interface IDocumentRender : ITabLevelControler, ITextRender, IRender
    {
        // Object AddPage(string __name);
        void saveDocument(FileInfo fi);

        acePaletteProvider paletteProvider { get; }

        #region ------------------ NEW APPENDS

        /// <summary>
        /// Appends the function.
        /// </summary>
        /// <param name="functionCode">The function code.</param>
        /// <param name="breakLine">if set to <c>true</c> [break line].</param>
        Object AppendFunction(String functionCode, Boolean breakLine = false);

        /// <summary>
        /// Sets document description information
        /// </summary>
        /// <param name="data">Property collection that applies info</param>
        /// <param name="alsoAppendAsPairs">If TRUE it will also create output using <c>AppendPairs</c> method</param>
        /// <param name="fieldsToUse">What fields to use?</param>
        Object AppendInfo(PropertyCollection data, Boolean alsoAppendAsPairs, params templateFieldBasic[] fieldsToUse);

        /// <summary>
        /// Gets the page - if name is empty returns the current page
        /// </summary>
        /// <param name="name">The name to use as selector</param>
        /// <returns>Page object</returns>
        /// \ingroup_disabled renderapi_service
        Object getPage(String name = "");

        /// <summary>
        /// Set provided page as current scope
        /// </summary>
        /// <param name="toPage">To page.</param>
        /// <returns></returns>
        void scopePage(Object toPage);

        /// <summary>
        /// Set specified document as current document
        /// </summary>
        /// <param name="toDocument">To document.</param>
        void scopeDocument(Object toDocument);

        #endregion ------------------ NEW APPENDS

        /// <summary>
        /// Updates internal meta data storage (custom properties/references/fields) according <c>mode</c>.
        /// </summary>
        /// <param name="data">New data</param>
        /// <param name="mode">Policy on combining data</param>
        /// <param name="alsoAppendAsPairs">If TRUE it will also create output using <c>AppendPairs</c> method</param>
        /// <returns>OuterXML/String or proper DOM object of container - if created</returns>
        Object AppendData(PropertyCollection data, existingDataMode mode, Boolean alsoAppendAsPairs);

        /// <summary>
        /// Applies the style (or part of style to an area
        /// </summary>
        /// <param name="shot">The shot.</param>
        /// <param name="areaToApply">The area to apply.</param>
        void ApplyStyle(IStyleInstruction shot, selectRangeArea areaToApply, params Object[] resources);

        /// <summary>
        /// Executes styling instruction that has more shots against more areas
        /// </summary>
        /// <param name="ins">The ins.</param>
        void ApplyStyle(areaStyleInstruction ins);

        #region ------------- NEW INFRASTRUCTURE

        IRenderExecutionContext context
        {
            get;
        }

        /// <summary>
        /// deploys style and palette provider
        /// </summary>
        /// <param name="__style">The style.</param>
        /// \ingroup_disabled renderapi_service
        void setContext(IRenderExecutionContext __context);

        //IMetaContentNested scope { get; }

        /// <summary>
        /// active document style
        /// </summary>
        /// <value>
        /// The style.
        /// </value>
        styleTheme theme { get; }

        /// <summary>
        /// Gets the embedded data.
        /// </summary>
        /// <value>
        /// The embedded data.
        /// </value>
        PropertyCollection data { get; }

        reportOutputSupport formats { get; }

        void ApplyColor(String hexColorCode, selectRangeArea area = null, Boolean toForeground = false);

        void ApplyColumn(Int32 column, Int32 width, textCursorZoneCorner align, Boolean doAutofit = false);

        void ApplyRow(Int32 row, Int32 height, textCursorZoneCorner align, Boolean doAutofit = false);

        /// <summary>
        /// Get output object or string. If <c>filepath</c> supplied it will save the output. If <c>data</c> object supplied it will append existing data and apply together on any existing paceholders
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="filepath"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        //   T ToOutput<T>(reportOutputFormat outputs, String filepath = null, PropertyCollection data = null);

        #endregion ------------- NEW INFRASTRUCTURE
    }
}