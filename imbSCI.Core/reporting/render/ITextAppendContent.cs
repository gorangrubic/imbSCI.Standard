// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITextAppendContent.cs" company="imbVeles" >
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
    using imbSCI.Data.enums.appends;
    using imbSCI.Data.enums.fields;
    using System;
    using System.Collections.Generic;
    using System.Data;

    public interface ITextAppendContent
    {
        #region Append extension v3

        /// <summary>
        /// Direct content injection, bypassing all internal transformations by class implementing <see cref="ITextRender"/>
        /// </summary>
        /// <param name="content">The content.</param>
        void AppendDirect(String content);

        /// <summary>
        /// Saves <c>content</c> to specified path. Path is local to context scope
        /// </summary>
        /// <param name="outputpath">The filepath, including filename and extension</param>
        /// <param name="content">Any string content</param>
        void AppendToFile(String outputpath, String content);

        /// <summary>
        /// Loads content from <c>sourcepath</c> into renderer [if <c>datakey</c> is <see cref="imbSCI.Data.enums.fields.templateFieldSubcontent.none"/> or into data field if specified.
        /// </summary>
        /// <param name="sourcepath">The sourcepath.</param>
        /// <param name="datakey">The datakey.</param>
        /// <param name="isLocalSource">if set to <c>true</c> <c>sourcepath</c> is interpreted as local to context</param>
        void AppendFromFile(String sourcepath, templateFieldSubcontent datakey = templateFieldSubcontent.none, Boolean isLocalSource = false);

        /// <summary>
        /// File from <c>sourcepath</c> is copied to <c>outputpath</c> or used as data template if <c>isDataTeplate</c> is true
        /// </summary>
        /// <param name="sourcepath">The sourcepath - within application directory</param>
        /// <param name="outputpath">The outputpath - local to context</param>
        /// <param name="isDataTemplate">if set to <c>true</c> the <c>soucepath</c> content will be processed as data template before saving output to <c>outputpath</c></param>
        void AppendFile(String sourcepath, String outputpath, Boolean isDataTemplate = false);

        /// <summary>
        /// Appends the image tag/call.
        /// </summary>
        /// <param name="imageSrc">Source url/path of the image</param>
        /// <param name="imageAltText">The image alt text.</param>
        /// <param name="imageRef">The image reference ID used internally</param>
        void AppendImage(String imageSrc, String imageAltText, String imageRef);

        /// <summary>
        /// Inserts <c>mathFormula</c> block
        /// </summary>
        /// <param name="mathFormula">The math formula: LaTeX, KaTex, asciimath...</param>
        /// <param name="mathFormat">The math format used to describe the formula</param>
        void AppendMath(String mathFormula, String mathFormat = "asciimath");

        /// <summary>
        /// Appends the content with label decoration
        /// </summary>
        /// <param name="content">The content to show inside label</param>
        /// <param name="isBreakLine">if set to <c>true</c> if will break line after this append</param>
        /// <param name="comp_style">Special style tag, class, definition.</param>
        void AppendLabel(String content, Boolean isBreakLine = true, Object comp_style = null);

        /// <summary>
        /// Creates panel with <c>content</c> and (optionally) with <c>comp_heading</c> and <c>comp_description</c> as footer.
        /// </summary>
        /// <param name="content">String content to place inside the panel</param>
        /// <param name="comp_heading">The heading for the panel. If blank panel will have no heading</param>
        /// <param name="comp_description">Description to be placed at bottom of the panel</param>
        /// <param name="comp_style">Special style tag, class, definition.</param>
        void AppendPanel(String content, String comp_heading = "", String comp_description = "", Object comp_style = null);

        #endregion Append extension v3

        /// <summary>
        /// Appends the function.
        /// </summary>
        /// <param name="functionCode">The function code.</param>
        /// <param name="breakLine">if set to <c>true</c> [break line].</param>
        /// <returns></returns>
        object AppendFunction(string functionCode, bool breakLine = false);

        /// <summary>
        /// Appends string with template placeholder tag {{{ }}} / creates field to call custom property --> for document builder: introduces custom parameter and field
        /// </summary>
        /// <param name="fieldName">String, enum what ever</param>
        /// <param name="breakLine">on TRUE it is new line call, on FALSE its inline call</param>
        /// <param name="type">Type of append render</param>
        void AppendPlaceholder(Object fieldName, Boolean breakLine = false);

        /// <summary>
        /// Horizontal line divider.
        /// </summary>
        /// <remarks>It respect active full width and/or background color</remarks>
        void AppendHorizontalLine();

        /// <summary>
        /// On HTML/XML builder adds invisible comment tag, on Table builder it adds comment to the current cell, on Document builder it adds pop-up comment on aplicable way
        /// </summary>
        /// <param name="content">Text content for the paragraph</param>
        Object AppendComment(String content);

        /// <summary>
        /// HTML/XML builder adds H tag with proper level sufix, on Table it applies style and for H1 and H2
        /// </summary>
        /// <param name="content">Text</param>
        /// <param name="level">from 1 to 6</param>
        /// <returns>OuterXML/String or proper DOM object of container</returns>
        Object AppendHeading(String content, Int32 level = 1);

        /// <summary>
        /// HTML/XML adds <c>q</c> tag, Table aplies <c>smallText</c> style
        /// </summary>
        /// <param name="content">Text content of the quote</param>
        /// <returns>OuterXML/String or proper DOM object of container</returns>
        Object AppendQuote(String content);

        /// <summary>
        /// HTML/XML adds <c>q</c> tag, Table aplies <c>smallText</c> style
        /// </summary>
        /// <param name="content">Text content of the quote</param>
        /// <returns>OuterXML/String or proper DOM object of container</returns>
        Object AppendCite(String content);

        /// <summary>
        /// HTML/XML adds <c>q</c> tag, Table aplies <c>smallText</c> style
        /// </summary>
        /// <param name="content">Text content of the quote</param>
        /// <returns>OuterXML/String or proper DOM object of container</returns>
        Object AppendCode(String content);

        /// <summary>
        /// HTML/XML adds <c>q</c> tag, Table aplies <c>smallText</c> style
        /// </summary>
        /// <param name="content">Text content of the quote</param>
        /// <param name="codetypename">The codetypename: i.e. html</param>
        /// <returns>
        /// OuterXML/String or proper DOM object of container
        /// </returns>
        Object AppendCode(String content, String codetypename);

        /// <summary>
        /// HTML/XML adds <c></c>
        /// </summary>
        /// <param name="content">Initial content</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="title">Title of the section</param>
        /// <param name="footnote">Description under the section</param>
        /// <param name="paragraphs">Additional paragraphs to place inside</param>
        /// <returns>OuterXML/String or proper DOM object of container</returns>
        Object AppendFrame(String content, Int32 width, Int32 height, String title = "", String footnote = "", IEnumerable<String> paragraphs = null);

        /// <summary>
        /// Appends collection of pairs.
        /// </summary>
        /// <param name="data">Data to use as pair source</param>
        /// <param name="isHorizontal">Should output be horizontal</param>
        /// <param name="between">Content to place between. If empty it will skip middle column</param>
        /// <returns>OuterXML/String or proper DOM object of container</returns>
        Object AppendPairs(PropertyCollection data, Boolean isHorizontal = false, String between = "");

        /// <summary>
        /// Appends content wrapped into paragraph tag. Table builders will merge whole line if "fullWidth" is TRUE.
        /// </summary>
        /// <param name="content">Text content for the paragraph</param>
        /// <param name="fullWidth">if TRUE it will maximize width</param>
        /// <returns>OuterXML/String or proper DOM object of container</returns>
        Object AppendParagraph(String content, Boolean fullWidth = false);

        /// <summary>
        /// Creates new section with title and content. Optionally it may contain: additional paragraphs for content and footnote on bottom
        /// </summary>
        /// <param name="content">Main content of the section</param>
        /// <param name="title">Title of the section</param>
        /// <param name="footnote">Description under the section</param>
        /// <param name="paragraphs">Additional paragraphs to place inside</param>
        /// <returns>OuterXML/String or proper DOM object of container</returns>
        Object AppendSection(String content, String title, String footnote = null, IEnumerable<String> paragraphs = null);

        /// <summary>
        /// Renders key-> value pair
        /// </summary>
        /// <param name="key">Property name or collection key</param>
        /// <param name="value">ToString value</param>
        /// <param name="breakLine">should break line </param>
        void AppendPair(String key, Object value, Boolean breakLine = true, String between = " = ");

        /// <summary>
        /// Appends inline or new line content.
        /// </summary>
        /// <param name="content">String content to be wrapped into container</param>
        /// <param name="type">Container type - for text it is always none</param>
        /// <param name="breakLine">Inline (FALSE) or new line (TRUE)</param>
        /// <remarks>It is supported by: Source, Document and Table builders</remarks>
        void Append(String content, appendType type = appendType.none, Boolean breakLine = false);

        /// <summary>
        /// Appends the line.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="type">The type.</param>
        void AppendLine(String content);

        /// <summary>
        /// Renders IEnumerable that may contain other IEnumerables
        /// </summary>
        /// <param name="content">Collection with objects and/or subcollections</param>
        /// <param name="isOrderedList">On TRUE it will be ordered list with number, FALSE will create button list</param>
        /// <remarks>In Document builders isOrderedList has isHorizontal role</remarks>
        void AppendList(IEnumerable<Object> content, Boolean isOrderedList = false);

        /// <summary>
        /// Renders link, image or reference
        /// </summary>
        /// <param name="url">url or reference id</param>
        /// <param name="name">Name of link</param>
        /// <param name="caption">Title - popup content</param>
        /// <param name="linkType">Select if output is link, image or reference</param>
        void AppendLink(String url, String name, String caption = "", appendLinkType linkType = appendLinkType.link);

        /// <summary>
        /// Renders DataTable
        /// </summary>
        /// <param name="table"></param>
        void AppendTable(DataTable table, Boolean doThrowException = true);
    }
}