// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbStringBuilderBase.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting.render.core
{
    using imbSCI.Core.collection;
    using imbSCI.Core.collection.checkLists;
    using imbSCI.Core.enums;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.extensions.table;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.reporting.extensions;
    using imbSCI.Core.reporting.format;
    using imbSCI.Core.reporting.lowLevelApi;
    using imbSCI.Core.reporting.render.config;
    using imbSCI.Core.reporting.render.contentControl;
    using imbSCI.Core.reporting.render.converters;
    using imbSCI.Core.reporting.template;
    using imbSCI.Core.reporting.zone;
    using imbSCI.Data;
    using imbSCI.Data.enums;
    using imbSCI.Data.enums.appends;
    using imbSCI.Data.enums.fields;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    /**
     * \addtogrpup renderapi
     * RenderAPI: IRender, ITextRender and IDocumentRender methods
     *
     * \addtogroup_disabled renderapi_style
     * RenderAPI: Styling methods
     * \ingroup_disabled renderapi
     *
     * \addtogroup_disabled renderapi_append
     * RenderAPI: Content append methods
     * \ingroup_disabled renderapi
     *
     * \addtogroup_disabled renderapi_service
     * RenderAPI: Logistics, resources and settings
     * \ingroup_disabled renderapi
     */

    /// <summary>
    /// Basic level render mechanism. Inherited by [Markdown, Style, Text] builders ... builders. Planed: [HTML, XML] [XLS, CSV, ODS] [RTF, Word, PDF] builders
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TG"></typeparam>
    [Serializable]
    public abstract class imbStringBuilderBase : imbReportingBindable, ITabLevelControler, IStringBuilderLengths, IConsoleControl
    {
        /// <summary>
        /// Gets a value indicating whether [variable allow automatic output to console].
        /// </summary>
        /// <value>
        /// <c>true</c> if [variable allow automatic output to console]; otherwise, <c>false</c>.
        /// </value>
        public virtual Boolean VAR_AllowAutoOutputToConsole { get { return false; } }

        private Boolean _isEnabled = true;

        /// <summary>If <c>true</c> it will allow append of new content, if <c>false</c> it will ignore any append call</summary>
        public Boolean isEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                _isEnabled = value;
                OnPropertyChanged("isEnabled");
            }
        }

        private Boolean _VAR_AllowInstanceToOutputToConsole = true;

        /// <summary>
        ///
        /// </summary>
        public virtual Boolean VAR_AllowInstanceToOutputToConsole
        {
            get { return _VAR_AllowInstanceToOutputToConsole; }
            set { _VAR_AllowInstanceToOutputToConsole = value; }
        }

        private Int32 _writeToConsoleAltColor = -1;

        /// <summary> </summary>
        protected Int32 writeToConsoleAltColor
        {
            get
            {
                return _writeToConsoleAltColor;
            }
            set
            {
                _writeToConsoleAltColor = value;
                OnPropertyChanged("writeToConsoleAltColor");
            }
        }

        /// <summary>
        /// Sets the alternative color mode for console output. Use set exact to set exactly the value for alternative color, otherwise it works in Toggle mode.
        /// </summary>
        /// <param name="altChange">The alt change.</param>
        /// <param name="setExact">if set to <c>true</c> [set exact].</param>
        public void consoleAltColorToggle(Boolean setExact = false, Int32 altChange = -1)
        {
            if (setExact)
            {
                writeToConsoleAltColor = altChange;
            }
            else
            {
                writeToConsoleAltColor = altChange * writeToConsoleAltColor;
            }
        }

        /// <summary>
        /// Should be called on every base append if allowed
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="breakLine">if set to <c>true</c> [break line].</param>
        protected void writeToConsole(String content, Boolean breakLine)
        {
            if (VAR_AllowInstanceToOutputToConsole)
            {
                screenOutputControl.logToConsoleControl.writeToConsole(content, this, breakLine, writeToConsoleAltColor);
            }
        }

        private Object sbAppendLock = new Object();

        protected void __lockedAppend(String __in, Boolean line = false)
        {
            lock (sbAppendLock)
            {
                if (line)
                {
                    sb.AppendLine(__in);
                }
                else
                {
                    sb.Append(__in);
                }
            }
        }

        /// <summary>
        /// Direct content injection, bypassing all internal transformations by class implementing <see cref="ITextRender" />
        /// </summary>
        /// <param name="content">The content.</param>
        public virtual void AppendDirect(string content)
        {
            if (VAR_AllowAutoOutputToConsole) writeToConsole(content, false);
            if (isEnabled)
            {
                __lockedAppend(content);
            }
        }

        /// <summary>
        /// Saves <c>content</c> to specified path. Path is local to context scope
        /// </summary>
        /// <param name="outputpath">The filepath, including filename and extension</param>
        /// <param name="content">Any string content</param>
        /// <exception cref="NotImplementedException"></exception>
        public void AppendToFile(string outputpath, string content)
        {
            FileInfo fi = outputpath.getWritableFile(getWritableFileMode.overwrite);
            if (fi != null)
            {
                content.saveStringToFile(fi.FullName, getWritableFileMode.overwrite, Encoding.UTF8);
            }
        }

        /// <summary>
        /// Loads content from <c>sourcepath</c> into renderer [if <c>datakey</c> is <see cref="imbSCI.Data.enums.fields.templateFieldSubcontent.none" /> or into data field if specified.
        /// </summary>
        /// <param name="sourcepath">The sourcepath.</param>
        /// <param name="datakey">The datakey.</param>
        /// <param name="isLocalSource">if set to <c>true</c> <c>sourcepath</c> is interpreted as local to context. This parameter has no effect when used on builder directly</param>
        /// <exception cref="NotImplementedException"></exception>
        public virtual void AppendFromFile(string sourcepath, templateFieldSubcontent datakey = templateFieldSubcontent.none, bool isLocalSource = false)
        {
            if (File.Exists(sourcepath))
            {
                String __filecontent = openBase.openFileToString(sourcepath, true, false);

                if (datakey == templateFieldSubcontent.none)
                {
                    AppendDirect(__filecontent);
                }
                else
                {
                    data.add(datakey, __filecontent);
                }
            }
        }

        protected converterBase _converter;

        /// <summary> </summary>
        public abstract converterBase converter { get; }

        public T getConverter<T>() where T : converterBase
        {
            return converter as T;
        }

        /// <summary>
        /// File from <c>sourcepath</c> is copied to <c>outputpath</c> or used as data template if <c>isDataTeplate</c> is true
        /// </summary>
        /// <param name="sourcepath">The sourcepath - within application directory</param>
        /// <param name="outputpath">The outputpath - local to context</param>
        /// <param name="isDataTemplate">if set to <c>true</c> the <c>soucepath</c> content will be processed as data template before saving output to <c>outputpath</c></param>
        /// <exception cref="NotImplementedException"></exception>
        public virtual void AppendFile(string sourcepath, string outputpath, bool isDataTemplate = false)
        {
            if (File.Exists(sourcepath))
            {
                if (isDataTemplate)
                {
                    String __filecontent = openBase.openFileToString(sourcepath, true, false);

                    String __output = __filecontent.applyToContent(false, data);
                    FileInfo fi = outputpath.getWritableFile(getWritableFileMode.overwrite);
                    __output.saveStringToFile(fi.FullName, getWritableFileMode.overwrite, Encoding.UTF8);
                }
            }
            //   throw new NotImplementedException();
        }

        /// <summary>
        /// Appends the image tag/call.
        /// </summary>
        /// <param name="imageSrc">Source url/path of the image</param>
        /// <param name="imageAltText">The image alt text.</param>
        /// <param name="imageRef">The image reference ID used internally</param>
        /// <exception cref="NotImplementedException"></exception>
        public abstract void AppendImage(string imageSrc, string imageAltText, string imageRef);

        /// <summary>
        /// Inserts <c>mathFormula</c> block
        /// </summary>
        /// <param name="mathFormula">The math formula: LaTeX, KaTex, asciimath...</param>
        /// <param name="mathFormat">The math format used to describe the formula</param>
        /// <exception cref="NotImplementedException"></exception>
        public abstract void AppendMath(string mathFormula, string mathFormat = "asciimath");

        //{
        //    AppendLine(mathFormula);
        //}

        /// <summary>
        /// Appends the content with label decoration
        /// </summary>
        /// <param name="content">The content to show inside label</param>
        /// <param name="isBreakLine">if set to <c>true</c> if will break line after this append</param>
        /// <param name="comp_style">Special style tag, class, definition.</param>
        /// <exception cref="NotImplementedException"></exception>
        public abstract void AppendLabel(string content, bool isBreakLine = true, object comp_style = null);

        //{
        //    Append(content, appendType.bold, isBreakLine);
        //}

        /// <summary>
        /// Creates panel with <c>content</c> and (optionally) with <c>comp_heading</c> and <c>comp_description</c> as footer.
        /// </summary>
        /// <param name="content">String content to place inside the panel</param>
        /// <param name="comp_heading">The heading for the panel. If blank panel will have no heading</param>
        /// <param name="comp_description">Description to be placed at bottom of the panel</param>
        /// <param name="comp_style">Special style tag, class, definition.</param>
        /// <exception cref="NotImplementedException"></exception>
        public abstract void AppendPanel(string content, string comp_heading = "", string comp_description = "", object comp_style = null);

        //{
        //    throw new NotImplementedException();
        //}

        /// <summary>
        /// Appends the check list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="isVertical">if set to <c>true</c> [is vertical].</param>
        /// <param name="filter">The filter.</param>
        public virtual void AppendCheckList(checkList list, Boolean isVertical, checkListItemValue filter = checkListItemValue.none)
        {
            PropertyCollectionExtended pce = list.getCheckedItems();

            AppendLine();

            foreach (var pe in pce.entries)
            {
                String ln = "";
                Boolean ok = true;

                checkListItemValue val = list.getCheckValue(pe.Key);

                if (filter != checkListItemValue.none)
                {
                    if (val != filter) ok = false;  //pe[PropertyEntryColumn.entry_value]
                }

                if (ok)
                {
                    ln = " - [";
                    if (val == checkListItemValue.checkTrue)
                    {
                        ln += "x";
                    }
                    else if (val == checkListItemValue.checkFalse)
                    {
                        ln += " ";
                    }
                    else
                    {
                        ln += " ";
                    }
                    ln += "] " + pe.Value[PropertyEntryColumn.entry_name].toStringSafe();

                    if (!imbSciStringExtensions.isNullOrEmptyString(pe.Value[PropertyEntryColumn.entry_description]))
                    {
                        ln += "_" + pe.Value[PropertyEntryColumn.entry_description] + "_";
                    }
                }
                if (isVertical)
                {
                    _AppendLine(ln);
                }
                else
                {
                    _Append(ln + " | ");
                }
            }

            AppendLine();
        }

        /// <summary>
        /// Gets the content blocks - returns content in subsections, where main content is in <see cref="imbSCI.Data.enums.fields.templateFieldSubcontent.main"/>
        /// </summary>
        /// <param name="includeMain">if set to <c>true</c> [include main].</param>
        /// <returns></returns>
        public virtual PropertyCollection getContentBlocks(Boolean includeMain, reportOutputFormatName format = reportOutputFormatName.none)
        {
            return sbControler.getDataset(includeMain);
            //   => sbControler.getDataset(includeMain);
        }

        private stringBuilderControler _sbControler = new stringBuilderControler();

        /// <summary>
        /// Controler for sub contents and stringBuilders
        /// </summary>
        public stringBuilderControler sbControler
        {
            get { return _sbControler; }
            protected set { _sbControler = value; }
        }

        protected StringBuilder sb
        {
            get
            {
                return sbControler.sb;
            }
        }// = new StringBuilder();


        public virtual void SubcontentStart(String key, Boolean cleanPriorContent = false)
        {
            sbControler.switchToActive(key);
            //sbControler.active = key;
            if (cleanPriorContent)
            {
                sb.Clear();
            }
        }


        /// <summary>
        /// Starts Subcontent session or continues existing. Optionally erazes all existing content under subsession specified by <c>key</c>
        /// </summary>
        /// <param name="key">Subsession selector key</param>
        /// <param name="cleanPriorContent">Optionally erazes all existing content under subsession specified by <c>key</c></param>
        public virtual void SubcontentStart(templateFieldSubcontent key, Boolean cleanPriorContent = false)
        {
            sbControler.switchToActive(key);
            //sbControler.active = key;
            if (cleanPriorContent)
            {
                sb.Clear();
            }
        }

        /// <summary>
        /// Ends Subcontent session, switches to the <see cref="imbSCI.Data.enums.fields.templateFieldSubcontent.main"/> content, and returns content from the session just closed.
        /// </summary>
        public virtual String SubcontentClose()
        {
            //if (sbControler.active == templateFieldSubcontent.main) return "";

            String output = sb.ToString();
            sbControler.switchToBack();

            //sbControler.switchToActive(templateFieldSubcontent.main);
            //sbControler.active = templateFieldSubcontent.main;

            return output;
        }

        #region COMMON APPENDS

        /// <summary>
        /// Appends the function.
        /// </summary>
        /// <param name="functionCode">The function code.</param>
        /// <param name="breakLine">if set to <c>true</c> [break line].</param>
        /// <returns></returns>
        public virtual object AppendFunction(string functionCode, bool breakLine = false)
        {
            // var exp = NCalc.Expression.Compile(functionCode, true);
            Append(functionCode.ToString(), appendType.marked, breakLine);
            return functionCode;
        }

        /// <summary>
        /// Updates internal meta data storage (custom properties/references/fields) according <c>mode</c>.
        /// </summary>
        /// <param name="data">New data</param>
        /// <param name="mode">Policy on combining data</param>
        /// <param name="alsoAppendAsPairs">If TRUE it will also create output using <c>AppendPairs</c> method</param>
        /// <returns>
        /// OuterXML/String or proper DOM object of container - if created
        /// </returns>
        /// \ingroup_disabled renderapi_append
        public virtual object AppendData(PropertyCollection data, existingDataMode mode, bool alsoAppendAsPairs)
        {
            this.data.AppendData(data, mode);
            if (alsoAppendAsPairs) return AppendPairs(data, false);
            return "";
        }

        /// <summary>
        /// Appends the frame.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="title">The title.</param>
        /// <param name="footnote">The footnote.</param>
        /// <param name="paragraphs">The paragraphs.</param>
        /// <returns></returns>
        /// \ingroup_disabled renderapi_append
        public virtual object AppendFrame(string content, int width, int height, string title = "", string footnote = "", IEnumerable<string> paragraphs = null)
        {
            AppendLine("-".Repeat(zone.innerLeftPosition) + title.ToUpper().toWidthExact(zone.outerRightPosition, "-"));
            AppendHorizontalLine();
            AppendLine(content);
            AppendList(paragraphs, false);
            AppendHorizontalLine();
            AppendLine("-".Repeat(zone.innerLeftPosition) + title.ToUpper().toWidthExact(zone.outerRightPosition, "-"));
            AppendLine(footnote);
            return "";
        }

        /// <summary>
        /// Appends the section.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="title">The title.</param>
        /// <param name="footnote">The footnote.</param>
        /// <param name="paragraphs">The paragraphs.</param>
        /// <returns></returns>
        /// \ingroup_disabled renderapi_append
        public virtual object AppendSection(string content, string title, string footnote = null, IEnumerable<string> paragraphs = null)
        {
            AppendLine(title.ToUpper());
            AppendHorizontalLine();
            AppendLine(content);
            AppendList(paragraphs, false);
            AppendHorizontalLine();
            AppendLine(footnote);
            return "";
        }

        /// <summary>
        /// HTML/XML builder adds H tag with proper level sufix, on Table it applies style and for H1 and H2
        /// </summary>
        /// <param name="content">Text</param>
        /// <param name="level">from 1 to 6</param>
        /// <returns>OuterXML/String or proper DOM object of container</returns>
        /// \ingroup_disabled renderapi_append
        public virtual object AppendHeading(string content, int level = 1)
        {
            if (content.isNullOrEmpty()) return "";
            appendType hd = (appendType)appendType.heading + level;
            Append(content, hd, true);

            //_AppendLine(content.markdownText(hd));
            return "";
        }

        /// <summary>
        /// Appends the paragraph.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="fullWidth">if set to <c>true</c> [full width].</param>
        /// <returns></returns>
        /// \ingroup_disabled renderapi_append
        public virtual object AppendParagraph(string content, bool fullWidth = false)
        {
            Append(content, appendType.paragraph, true);
            //_AppendLine("p {");
            //_AppendLine(content);
            //_AppendLine("}");

            return "";
        }

        /// <summary>
        /// Appends the quote.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        /// \ingroup_disabled renderapi_append
        public virtual object AppendQuote(string content)
        {
            Append(content, appendType.quotation, true);
            //_AppendLine("q {");
            //_AppendLine(content);
            //_AppendLine("}");

            return "";
        }

        /// <summary>
        /// HTML/XML adds <c>q</c> tag, Table aplies <c>smallText</c> style
        /// </summary>
        /// <param name="content">Text content of the quote</param>
        /// <returns>
        /// OuterXML/String or proper DOM object of container
        /// </returns>
        /// \ingroup_disabled renderapi_append
        public virtual object AppendCite(string content)
        {
            Append(content, appendType.squareQuote, true);
            //_AppendLine("cite {");
            //_AppendLine(content);
            //_AppendLine("}");

            return "";
        }

        /// <summary>
        /// HTML/XML adds <c>q</c> tag, Table aplies <c>smallText</c> style
        /// </summary>
        /// <param name="content">Text content of the quote</param>
        /// <returns>
        /// OuterXML/String or proper DOM object of container
        /// </returns>
        /// \ingroup_disabled renderapi_append
        public virtual object AppendCode(string content)
        {
            Append(content, appendType.source, true);
            //_AppendLine("code {");
            //_AppendLine(content);
            //_AppendLine("}");

            return "";
        }

        /// <summary>
        /// HTML/XML adds <c>q</c> tag, Table aplies <c>smallText</c> style
        /// </summary>
        /// <param name="content">Text content of the quote</param>
        /// <returns>
        /// OuterXML/String or proper DOM object of container
        /// </returns>
        /// \ingroup_disabled renderapi_append
        public virtual object AppendCode(String content, String codetypename)
        {
            //Append(content, appendType.source, true);
            _AppendLine("code:" + codetypename + " {");
            _AppendLine(content);
            _AppendLine("}");

            return "";
        }

        /// <summary>
        /// Appends the comment.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        /// \ingroup_disabled renderapi_append
        public virtual object AppendComment(string content)
        {
            AppendLine();
            Append(" > " + content, appendType.comment, true);
            AppendLine();
            return "";
        }

        #endregion COMMON APPENDS

        private PropertyCollection _data = new PropertyCollection();

        /// <summary>
        /// Data embedded with builder
        /// </summary>
        /// <value>
        /// The embedded data.
        /// </value>
        public PropertyCollection data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
        }

        #region --- settings ------- Builder settings

        protected builderSettings _settings = new builderSettings();

        /// <summary>
        /// Builder settings
        /// </summary>
        public virtual builderSettings settings
        {
            get
            {
                return _settings;
            }
            protected set
            {
                _settings = value;
                OnPropertyChanged("settings");
            }
        }

        #endregion --- settings ------- Builder settings

        /// <summary>
        /// konstruktor koji postavlja tabLevel, podrazumevani tab level je 2
        /// </summary>
        /// <param name="__tabLevel"></param>
        public imbStringBuilderBase(Int32 __tabLevel)
        {
            tabLevel = __tabLevel;
            //tabInsert = tab.Repeat(tabLevel);
            prepareBuilder();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="imbStringBuilderBase"/> class.
        /// </summary>
        public imbStringBuilderBase()
        {
            // tabLevel = __tabLevel;
            //tabInsert = tab.Repeat(tabLevel);
            prepareBuilder();
        }

        /// <summary>
        /// Autocall->prepareBuilder() - Initializes a new instance of the <see cref="imbStringBuilderBase"/> class.
        /// </summary>
        /// <param name="__builderAPI">The builder API.</param>
        protected imbStringBuilderBase(reportAPI __builderAPI = reportAPI.textBuilder)
        {
            prepareBuilder();
        }

        #region -----------  formats  -------  [Description of $property$]

        protected reportOutputSupport _formats; // = new reportOutputSupport();

        /// <summary>
        /// Gets the output support definition for this report kind
        /// </summary>
        /// <value>
        /// The object containing output support info
        /// </value>
        /// \ingroup_disabled renderapi_service
        public reportOutputSupport formats
        {
            get
            {
                if (settings != null) return settings.formats;
                return _formats;
            }
            protected set
            {
                if (settings != null) settings.formats = value;
                _formats = value;
            }
        }

        #endregion -----------  formats  -------  [Description of $property$]

        /// <summary>
        /// List of content segments/entries/Appends
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        /// \ingroup_disabled renderapi_service
        public virtual IList content
        {
            get
            {
                return contentElements;
            }
        }

        private List<String> _content = new List<string>();

        /// <summary>
        /// Collection for basic text builders
        /// </summary>
        /// \ingroup_disabled renderapi_service
        public List<String> contentElements
        {
            get
            {
                return _content;
            }
        }

        /// <summary>
        /// Returns last segment of content (since last call)
        /// </summary>
        /// <returns>Last segment or line that was Appended</returns>
        /// \ingroup_disabled renderapi_service
        public String getLastLine(Boolean removeIt = false)
        {
            Int32 l = 0;

            if (lastLength == -1)
            {
                lastLength = 0;
            }
            l = Convert.ToInt32(Length - lastLength);

            if (l > 0)
            {
                lastLength = Length;
                String lastLine = sb.ToString().substring(-l);

                if (removeIt)
                {
                    sb.Remove(sb.Length - lastLine.Length, lastLine.Length);
                }
                else
                {
                    contentElements.Add(lastLine);
                }
                return imbSciStringExtensions.removeEndsWith(imbSciStringExtensions.removeStartsWith(lastLine, Environment.NewLine), Environment.NewLine);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Gets specified content segment, or complete content
        /// </summary>
        /// <param name="fromLength">From length - by default from start</param>
        /// <param name="toLength">To length - by default to the end</param>
        /// <returns>The slice of the content</returns>
        public String GetContent(long fromLength = long.MinValue, long toLength = long.MinValue)
        {
            if (fromLength == long.MinValue) fromLength = 0;

            if (toLength == long.MinValue) toLength = Length;
            if (fromLength > toLength)
            {
                fromLength = 0;
            }
            return sb.ToString().Substring(Convert.ToInt32(fromLength), Convert.ToInt32((toLength - fromLength)));
        }

        #region --- lastLength ------- Length since last getLastLine call

        private long _lastLength = -1;

        /// <summary>
        /// Length since last getLastLine call
        /// </summary>
        /// \ingroup_disabled renderapi_service
        public long lastLength
        {
            get
            {
                return _lastLength;
            }
            private set
            {
                _lastLength = value;
                OnPropertyChanged("lastLength");
            }
        }

        #endregion --- lastLength ------- Length since last getLastLine call

        #region --- Length ------- Length of internal content

        private long _Length = 0;

        /// <summary>
        /// Length of internal content
        /// </summary>
        /// \ingroup_disabled renderapi_service
        public long Length
        {
            get
            {
                if (sb != null) return sb.Length; // _Length;
                return _Length;
            }
            private set
            {
                _Length = value;
                OnPropertyChanged("Length");
            }
        }

        #endregion --- Length ------- Length of internal content

        #region --- c ------- cursor

        protected cursor _c;

        /// <summary>
        /// Reference to cursor object
        /// </summary>
        /// \ingroup_disabled renderapi_service
        public virtual cursor c
        {
            get
            {
                return _c;
            }
            //set
            //{
            //    _c = value;
            //    OnPropertyChanged("c");
            //}
        }

        #endregion --- c ------- cursor

        #region --- cursorZone ------- zone of text format

        protected cursorZone _zone;

        /// <summary>
        /// zone of text format - reference
        /// </summary>
        /// \ingroup_disabled renderapi_service
        public virtual cursorZone zone
        {
            get
            {

                return _zone;
            }
        }

        #endregion --- cursorZone ------- zone of text format

        protected Char tab = '\t';
        protected Int32 _tabLevel = 0;
        protected String _linePrefix = "";
        protected tagStack _openedTags = new tagStack();

        /// <summary>
        /// Kolekcija svih otvorenih tagova
        /// </summary>
        /// \ingroup_disabled renderapi_service
        public tagStack openedTags
        {
            get { return _openedTags; }
            set
            {
                _openedTags = value;
                OnPropertyChanged("openedTags");
            }
        }

        /// <summary>
        /// Prefix koji se dodaje ispred teksta -- tabovi
        /// </summary>
        /// \ingroup_disabled renderapi_service
        public String tabInsert
        {
            get
            {
                if (tabLevel > 0)
                {
                    return new String(tab, tabLevel);
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// nivo na kome je tab sada
        /// </summary>
        /// \ingroup_disabled renderapi_service
        [XmlIgnore]
        public virtual Int32 tabLevel
        {
            get
            {
                if (_tabLevel < 0) _tabLevel = 0;
                return _tabLevel;
            }
            set
            {
                _tabLevel = value;
                //                tabInsert = tab.Repeat(tabLevel);
                OnPropertyChanged("tabLevel");
            }
        }

        /// <summary>
        /// Podesavanje NewLine stringa koji se dodaje kada dodje do BreakLine instrukcije
        /// </summary>
        /// \ingroup_disabled renderapi_service
        protected virtual String newLineString
        {
            get { return Environment.NewLine; }
        }

        /// <summary>
        /// Prefix koji se dodaje ispred svake linije
        /// </summary>
        /// \ingroup_disabled renderapi_service
        [XmlIgnore]
        public String linePrefix
        {
            get { return _linePrefix; }
            set
            {
                _linePrefix = value;
                // OnPropertyChanged("linePrefix");
            }
        }

        public String ToString(Boolean doFlush = false)
        {
            return ContentToString(doFlush);
        }

        /// <summary>
        /// Vraca sadrzaj u String obliku
        /// </summary>
        /// <param name="doFlush">if TRUE it will clear exported content</param>
        /// <returns>Text representation of content - if its applicable</returns>
        /// \ingroup_disabled renderapi_service
        public virtual String ContentToString(Boolean doFlush = false, reportOutputFormatName format = reportOutputFormatName.none)
        {
            String output = "";
            if (sb.Length > 0)
            {
                try
                {
                    output = sb.ToString();
                }
                catch (Exception ex)
                {
                    output = "ERROR in ContentToString() call";
                    output = output.addLine(ex.LogException("imbStringBuilderBase failed to deliver the content", "imbStringBuilderBase failed"));
                }
            }

            if (doFlush) Clear();
            return output;
        }

        public virtual String newLineInsert
        {
            get
            {
                return Environment.NewLine;
            }
        }

        /// <summary>
        /// CORE METHOD: Metod koji pozivaju svi drugi AppendLine / Pair / link metodi -- sluzi za override / promenu ponasanja kod izvedenih klasa
        /// </summary>
        /// <param name="input"></param>
        /// \ingroup_disabled renderapi_append
        protected virtual void _AppendLine(String input)
        {
            // String newContent = linePrefix + tabInsert + input;
            // sb.AppendLine


            String nl = newLineInsert;

            String lpt = linePrefix + tabInsert;

            if (!input.StartsWith(lpt)) input = lpt + input;
            //input = input.ensureStartsWith(linePrefix + tabInsert);


            if (!input.StartsWith(nl)) input = nl + input;
            if (!input.EndsWith(nl)) input = input + nl;

            //input = input.removeStartsWith(newLineInsert).removeEndsWith(newLineInsert);  //imbSciStringExtensions.removeStartsWith(imbSciStringExtensions.removeEndsWith(input, newLineInsert), newLineInsert);
            //input = imbSciStringExtensions.ensureEndsWith(input, newLineInsert);

            // contentElements.Add(newContent);

            if (isEnabled) __lockedAppend(input); // sb.Append(input);
            if (VAR_AllowAutoOutputToConsole) writeToConsole(input, true);
            //getLastLine(false);
        }

        /// <summary>
        /// CORE METHOD: Metod koji pozivaju svi drugi Append -- sluzi za override / promenu ponasanja kod izvedenih klasa
        /// </summary>
        /// <param name="input"></param>
        /// \ingroup_disabled renderapi_append
        protected virtual void _Append(String input, Boolean breakLine = false)
        {
            if (breakLine)
            {
                _AppendLine(input);
            }
            else
            {
                if (VAR_AllowAutoOutputToConsole) writeToConsole(input, breakLine);
                if (isEnabled) { __lockedAppend(input); } // sb.Append(input);
            }

            //getLastLine(false);
        }

        /// <summary>
        /// Clears all content from this builder
        /// </summary>
        /// \ingroup_disabled renderapi_append
        public void Clear()
        {
            closeAll();
            lastLength = 0;
            contentElements.Clear();
            sbControler.Clear();
            sb.Clear();
            openedTags.Clear();
            rootTabLevel();
        }

        /// <summary>
        /// Prelazi u sledeci tab level
        /// </summary>
        /// \ingroup_disabled renderapi_append
        public void nextTabLevel()
        {
            tabLevel++;
        }

        /// <summary>
        /// Prebacuje u prethodni tabLevel
        /// </summary>
        /// \ingroup_disabled renderapi_append
        public void prevTabLevel()
        {
            if (tabLevel > 0)
            {
                tabLevel--;
            }
        }

        /// <summary>
        /// Sets tab level to root
        /// </summary>
        /// \ingroup_disabled renderapi_append
        public void rootTabLevel()
        {
            tabLevel = 0;
        }

        /// <summary>
        /// Deletes the last appends and returns it
        /// </summary>
        /// <returns>Append that was deleted</returns>
        /// \ingroup_disabled renderapi_append
        public virtual Object deleteLast()
        {
            return getLastLine(true);
        }

        /// <summary>
        /// Adds new <c>stringTemplate</c> placeholder string into template
        /// </summary>
        /// <param name="fieldName"></param>
        /// \ingroup_disabled renderapi_append
        public virtual void AppendPlaceholder(Object fieldName, Boolean breakLine = false)
        {
            _Append(stringTemplateTools.renderToTemplate(fieldName));
            //  getLastLine();
        }

        /// <summary>
        /// Appends new line with line prefix, tab insert and break (enter)
        /// </summary>
        /// \ingroup_disabled renderapi_append
        public virtual void AppendLine(String content = "")
        {
            _AppendLine(content);
            //  getLastLine();
        }

        /// <summary>
        /// General Append call - appends inline or in new line
        /// </summary>
        /// <param name="content">String to add</param>
        /// <param name="type">Disabled</param>
        /// <param name="breakLine">On TRUE it will break into new line</param>
        /// \ingroup_disabled renderapi_append
        public virtual void Append(String content, appendType type, Boolean breakLine = false)
        {
            // String input = content.plainText(type);
            if (breakLine)
            {
                _Append(content);
            }
            else
            {
                _AppendLine(content);
                //   AppendLine(content.markdownText(type));
            }
            //  getLastLine();
            //sb.AppendLine(linePrefix + tabInsert + content.ToUpper());
        }

        /// <summary>
        /// Appends a KeyValue pair.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="breakLine">if set to <c>true</c> [break line].</param>
        /// <param name="between">String to put in middle cell</param>
        /// \ingroup_disabled renderapi_append
        public virtual void AppendPair(Enum key, Object value, Boolean breakLine = true, String between = ":")
        {
            _Append(imbSciStringExtensions.add(key.ToString(), value.toStringSafe(), between), breakLine);
            // if (breakLine) sb.Append(Environment.NewLine);
            // getLastLine();
        }

        /// <summary>
        /// Appends a KeyValue pair
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="breakLine">if set to <c>true</c> [break line].</param>
        /// <param name="between">The between.</param>
        /// \ingroup_disabled renderapi_append
        public virtual void AppendPair(String key, Object value, Boolean breakLine = true, String between = " = ")
        {
            _Append(imbSciStringExtensions.add(key, value.toStringSafe(), between), breakLine);

            // getLastLine();
        }

        /// <summary>
        /// Appends collection of pairs.
        /// </summary>
        /// <param name="data">Data to use as pair source</param>
        /// <param name="isHorizontal">Should output be horizontal</param>
        /// <param name="between">Content to place between. If empty it will skip middle column</param>
        /// <returns>OuterXML/String or proper DOM object of container</returns>
        /// \ingroup_disabled renderapi_append
        public virtual object AppendPairs(PropertyCollection data, Boolean isHorizontal = false, String between = "")
        {
            if (between.isNullOrEmpty()) between = " \t \t \t";
            foreach (DictionaryEntry entry in data)
            {
                // _Append(entry.Key.toStringSafe().add(entry.Value.toStringSafe(), between), !isHorizontal);
                AppendPair(entry.Key.ToString(), entry.Value, isHorizontal, between);
                if (!isHorizontal) AppendLine();
            }
            //return getLastLine();
            return "";
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Renders link, image or reference text representation
        /// </summary>
        /// <param name="url">url or reference id</param>
        /// <param name="name">Name of link</param>
        /// <param name="caption">Title - popup content</param>
        /// <param name="linkType">Select if output is link, image or reference</param>
        /// \ingroup_disabled renderapi_append
        public virtual void AppendLink(String url, String name, String caption = "", appendLinkType linkType = 0)
        {
            _AppendLine(url.textLink(name, caption, linkType));
            // getLastLine();
        }

        #region --- openTagFormat ------- format template for tag opening

        private String _openTagFormat = "{0}" +
                                        "";

        /// <summary>
        /// format template for tag opening
        /// </summary>
        /// \ingroup_disabled renderapi_service
        protected String openTagFormat
        {
            get
            {
                return _openTagFormat;
            }
            set
            {
                _openTagFormat = value;
                OnPropertyChanged("openTagFormat");
            }
        }

        #endregion --- openTagFormat ------- format template for tag opening

        #region --- closeTagFormat ------- format template for tag closing

        private String _closeTagFormat = "{0}" +
                                         "";

        /// <summary>
        /// format template for tag/group/section closing
        /// </summary>
        /// \ingroup_disabled renderapi_service
        public String closeTagFormat
        {
            get
            {
                return _closeTagFormat;
            }
            set
            {
                _closeTagFormat = value;
                OnPropertyChanged("closeTagFormat");
            }
        }

        #endregion --- closeTagFormat ------- format template for tag closing

#pragma warning disable CS1570 // XML comment has badly formed XML -- 'End tag 'remarks' does not match the start tag 'para'.'
#pragma warning disable CS1570 // XML comment has badly formed XML -- 'Expected an end tag for element 'para'.'
#pragma warning disable CS1570 // XML comment has badly formed XML -- 'Expected an end tag for element 'remarks'.'
        /// <summary>
        /// Merges content scoped by area
        /// </summary>
        /// <param name="areaToMerge">The area to merge.</param>
        /// <remarks>
        /// <para>Implementations are different.<para>
        /// <para>markdown builder will use only vertical range to left-pan scoped lines for one tab and will insert empty line on top of it</para>
        /// <para>text builder will use only vertical range to remove all linebreaks from content of scoped lines</para>
        /// <para>html builder will a) put content of scoped sibling nodes into new sub node (same tag as the current node). b) in case it is inside a table structure it will merge cells</para>
        /// <para>Table document builder will merge scoped cells</para>
        /// </remarks>
        /// <returns>Merged content</returns>
        ///  \ingroup_disabled renderapi_append
        public virtual Object merge(selectRangeArea areaToMerge)
#pragma warning restore CS1570 // XML comment has badly formed XML -- 'Expected an end tag for element 'remarks'.'
#pragma warning restore CS1570 // XML comment has badly formed XML -- 'Expected an end tag for element 'para'.'
#pragma warning restore CS1570 // XML comment has badly formed XML -- 'End tag 'remarks' does not match the start tag 'para'.'
        {
            var lines = contentElements.GetRange(areaToMerge.y, areaToMerge.height);

            Int32 position = contentElements.GetRange(areaToMerge.y, areaToMerge.height).getTotalLength();

            Int32 length = lines.getTotalLength();

            String contentToMerge = lines.toCsvInLine(Environment.NewLine);

            contentElements.RemoveRange(areaToMerge.y, areaToMerge.height);

            String contentMerged = contentToMerge.Replace(Environment.NewLine, "");
            sb.Remove(position, length);
            sb.Insert(position, contentMerged);
            return contentMerged;
        }

        /// <summary>
        /// Opens the specified tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        ///  \ingroup_disabled renderapi_append
        public virtual tagBlock open(Enum tag, String title = "", String description = "")
        {
            return open(tag.ToString(), title, description);
        }

        /// <summary>
        /// Closes the specified tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        ///  \ingroup_disabled renderapi_append
        public virtual tagBlock close(Enum tag)
        {
            return close(tag.ToString());
        }

        /// <summary>
        /// Opens the specified tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        ///  \ingroup_disabled renderapi_append
        public virtual tagBlock open(String tag, String title = "", String description = "")
        {
            tagBlock tb = openedTags.Add(tag, title, description);
            String __op = String.Format(openTagFormat, tag);
            _AppendLine(__op);
            // openedTags.Push(tb);
            return tb;
        }

        /// <summary>
        /// Dodaje HTML zatvaranje taga -- zatvara poslednji koji je otvoren
        /// </summary>
        /// <remarks>
        /// Ako je prosledjeni tag none onda zatvara poslednji tag koji je otvoren.
        /// </remarks>
        /// <param name="tag"></param>
        ///  \ingroup_disabled renderapi_append
        public virtual tagBlock close(String tag = "none")
        {
            tagBlock tb = null;
            //tag = tag.checkForDefaultTag(reportOutputRoles.container);

            if (tag == "none")
            {
                if (openedTags.Any())
                {
                    tb = openedTags.Pop();
                    tag = tb.tag;// openedTags.Pop();
                }
                else
                {
                    tag = "error";
                }
            }
            else
            {
                tb = openedTags.Pop();
                // tag = tb.tag;
            }
            String __cl = String.Format(closeTagFormat, tag);
            if (tag != "none")
            {
                prevTabLevel();

                _AppendLine(__cl);
            }
            return tb;
        }

        /// <summary>
        /// Closes all tags/groups/sections that were currently open
        /// </summary>
        ///  \ingroup_disabled renderapi_append
        public virtual void closeAll()
        {
            String tag = "none"; // htmlTagName.none;

            Int32 c = openedTags.Count;

            for (int i = 0; i < c; i++)
            {
                var tg = openedTags.Pop();
                _AppendLine("#".a(tg.tag));
            }
        }

        protected List<String> _failedArguments = new List<string>();
        protected Boolean _isArgumentFailed = false;

        /// <summary>
        /// Da li je do sada neka od provera Null vrednosti vratila TRUE vrednost
        /// </summary>
        [XmlIgnore]
        [Category("Switches")]
        [DisplayName("isArgumentFailed")]
        [Description("Da li je do sada neka od provera Null vrednosti vratila TRUE vrednost")]
        public Boolean isArgumentFailed
        {
            get { return _isArgumentFailed; }
            set
            {
                _isArgumentFailed = value;
                OnPropertyChanged("isArgumentFailed");
            }
        }

        //  public string tabInsert { get; set; }

        /// <summary>
        /// Renders list and sublists
        /// </summary>
        /// <param name="content"></param>
        /// <param name="isOrderedList"></param>
        ///  \ingroup_disabled renderapi_append
        public virtual void AppendList(IEnumerable<Object> content, Boolean isOrderedList = false)
        {
            String cont = content.textList(isOrderedList, sb);
            AppendParagraph(cont);
            //sb.AppendLine(linePrefix + tabInsert + content.ToUpper());
        }

        /// <summary>
        /// Adds horizontal line
        /// </summary>
        ///  \ingroup_disabled renderapi_append
        public virtual void AppendHorizontalLine()
        {
            if (zone != null)
            {
                _AppendLine(" ".Repeat(zone.innerLeftPosition) + "-".Repeat(zone.innerRightPosition));
            }
            else
            {
                _AppendLine(" --- ");
            }
            //sb.AppendLine(linePrefix + tabInsert + "---");
        }

#pragma warning disable CS1574 // XML comment has cref attribute 'validateTable()' that could not be resolved
        /// <summary>
        /// Renders DataTable
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="doThrowException">if set to <c>true</c> it will throw an exception on <see cref="validateTable()"/> return false.</param>
        public virtual void AppendTable(DataTable table, bool doThrowException = true)
#pragma warning restore CS1574 // XML comment has cref attribute 'validateTable()' that could not be resolved
        {
            if (table.validateTable())
            {
                _AppendLine(table.textTable());
            }
            else
            {
                if (doThrowException) throw new ArgumentException(nameof(table), "AppendTable(" + table?.TableName.toStringSafe() + ") failed: data table is failed on [table.validateTable()] test");
                AppendLine("AppendTable failed");
                return;
            }
        }

        protected DirectoryInfo _directoryCurrent;

        public virtual DirectoryInfo directoryScope
        {
            get
            {
                if (_directoryCurrent == null) _directoryCurrent = new DirectoryInfo(Directory.GetCurrentDirectory());
                return _directoryCurrent;
            }

            set
            {
                _directoryCurrent = value;
            }
        }

        /// <summary>
        /// Loads the page from filepath. If it is document type then imports page with targeted name
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public virtual Object loadPage(String filepath, String name = "", reportOutputFormatName format = reportOutputFormatName.none)
        {
            FileInfo fi = filepath.getWritableFile(getWritableFileMode.newOrExisting);
            var newLines = fi.openFileToList(true);
            //Clear();

            newLines.ForEach(x => Append(x, appendType.regular, true));
            return fi;
        }

        public virtual FileInfo loadDocument(string filepath, string name = "", reportOutputFormatName format = reportOutputFormatName.none)
        {
            FileInfo fi = filepath.getWritableFile(getWritableFileMode.newOrExisting);
            var newLines = fi.openFileToList(true);
            Clear();
            newLines.ForEach(x => Append(x, appendType.regular, true));
            return fi;
        }

        public virtual FileInfo saveDocument(string name, getWritableFileMode mode, reportOutputFormatName format = reportOutputFormatName.none)
        {
            String filename = formats.getFilename(name, format);

            FileInfo fi = filename.getWritableFile(getWritableFileMode.newOrExisting);
            getLastLine();

            saveBase.saveToFile(fi.FullName, contentElements);

            // throw new NotImplementedException();

            return fi;
        }

        public virtual void saveDocument(FileInfo fi)
        {
            getLastLine();
            saveBase.saveToFile(fi.FullName, contentElements);
        }

        public virtual object getDocument()
        {
            return contentElements;
        }

        public virtual void prepareBuilder()
        {
            formats = reportOutputSupport.getFormatSupportFor(reportAPI.textBuilder, "output");
        }
    }
}