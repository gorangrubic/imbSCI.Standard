// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbDocumentBuilderBase.cs" company="imbVeles" >
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
    using imbSCI.Core.reporting.colors;
    using imbSCI.Core.reporting.format;
    using imbSCI.Core.reporting.style;
    using imbSCI.Core.reporting.style.core;
    using imbSCI.Core.reporting.style.enums;
    using imbSCI.Core.reporting.style.shot;
    using imbSCI.Core.reporting.zone;
    using imbSCI.Data.enums;
    using imbSCI.Data.enums.fields;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.IO;

    /// <summary>
    /// Base class for document builders:  XLS, RTF, PDF, HTML ...
    /// </summary>
    /// <seealso cref="imbStringBuilderBase" />
    /// <seealso cref="ITextRender" />
    /// <seealso cref="IDocumentRender" />
    /// <typeparam name="TDoc">Output DOM/Object</typeparam>
    /// <typeparam name="TPage">Type created as new Page</typeparam>
    public abstract class imbDocumentBuilderBase<TDoc, TPage> : imbStringBuilderBase, ITextRender, IDocumentRender where TDoc : class, new()
    {
        #region -----------  paletteProvider  -------  [ACE PaletteProvider]

        private acePaletteProvider _paletteProvider; // = new acePaletteProvider();

        /// <summary>
        /// ACE PaletteProvider
        /// </summary>
        /// <value>
        /// The palette provider from <c>styleTheme</c>
        /// </value>
        // [XmlIgnore]
        [Category("imbDocumentBuilderBase")]
        [DisplayName("paletteProvider")]
        [Description("ACE PaletteProvider")]
        public acePaletteProvider paletteProvider
        {
            get
            {
                return theme.palletes;
            }
        }

        #endregion -----------  paletteProvider  -------  [ACE PaletteProvider]

        #region --- context ------- Bindable property

        private IRenderExecutionContext _context;

        /// <summary>
        /// Bindable property
        /// </summary>
        public IRenderExecutionContext context
        {
            get
            {
                return _context;
            }
            set
            {
                _context = value;
                OnPropertyChanged("context");
            }
        }

        #endregion --- context ------- Bindable property

        public abstract void saveDocument(FileInfo fi);

        /// <summary>
        /// deploys style and palette provider
        /// </summary>
        /// <param name="__style">The style.</param>
        /// \ingroup_disabled renderapi_service
        public void setContext(IRenderExecutionContext __context)
        {
            context = __context;
            if (context.theme != null)
            {
                _zone = context.theme.zoneMain;
                _c = context.theme.cMain;
            }
            else
            {
                context.log(String.Format("- context.style is null - {0} will use its own cursor and zone definitions", this.GetType().Name));
            }
        }

        public abstract void deployPage(pageFormat pageSettings);

        protected void deployPageCommon(pageFormat pageSettings)
        {
            zone.reset(0, 0, 1, 1);
            zone.setZoneStructure(pageSettings.zoneLayoutPreset, true);
            zone.setPresetSpatialSettings(pageSettings.zoneSpatialPreset);
            theme.palletes.active = pageSettings.mainColor;

            c.moveToCorner(settings.cursorBehaviour.pageScopeInMove);
        }

        /// <summary>
        /// Gets the document instance - current document
        /// </summary>
        /// <remarks>
        /// Usual application do not require document instance outside of builder class.
        /// But if you need some special thing to do.. here it is
        /// </remarks>
        /// <returns></returns>
        /// \ingroup_disabled renderapi_service
        public TDoc getDocument()
        {
            return document;
        }

        /// <summary>
        /// Loads the document from filepath into internal object of TDoc type as current document
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        /// <returns>FileInfo pointing to the loaded document</returns>
        /// \ingroup_disabled renderapi_service
        public abstract FileInfo loadDocument(String filepath, String name = "", reportOutputFormatName format = reportOutputFormatName.none);

        /// <summary>
        /// Creates new document both in filesytem and internal memory. Location for new file is current directory.
        /// </summary>
        /// <param name="name">Name of new document. It will transform it to filename version and add proper file extension. No problem if you put extension alone.</param>
        /// <param name="mode">How any existing file should be handled</param>
        /// <returns>Newly created document</returns>
        /// \ingroup_disabled renderapi_service
        public abstract Object addDocument(String name, Boolean scopeToNew = true, getWritableFileMode mode = getWritableFileMode.autoRenameExistingOnOtherDate, reportOutputFormatName format = reportOutputFormatName.none);

        /// <summary>
        /// Saves the current document, returns <c>FileInfo</c> pointing to it
        /// </summary>
        /// <param name="name">The name without extension</param>
        /// <param name="mode">Existing file mode</param>
        /// <returns>File info pointing to</returns>
        /// \ingroup_disabled renderapi_service
        public abstract FileInfo saveDocument(String name, getWritableFileMode mode, reportOutputFormatName format = reportOutputFormatName.none);

        /// <summary>
        /// Adds new page, drives cursor to upper-left corner
        /// </summary>
        /// <param name="name">The name of newly created page.</param>
        /// <param name="mode">In case page with the same name already exists</param>
        /// <returns>Page object - usually not directly used</returns>
        /// \ingroup_disabled renderapi_service
        public abstract TPage addPage(String name, Boolean scopeToNew = true, getWritableFileMode mode = getWritableFileMode.autoRenameThis, reportOutputFormatName format = reportOutputFormatName.none);

        /// <summary>
        /// Gets the page - if name is empty returns the current page
        /// </summary>
        /// <param name="name">The name to use as selector</param>
        /// <returns>Page object</returns>
        /// \ingroup_disabled renderapi_service
        public abstract TPage getPage(String name = "");

        /// <summary>
        /// Scopes the document.
        /// </summary>
        /// <param name="toDocument">To document.</param>
        public void scopeDocument(Object toDocument)
        {
            document = toDocument as TDoc;
        }

        /// <summary>
        /// Scopes the page.
        /// </summary>
        /// <param name="toPage">To page.</param>
        public void scopePage(Object toPage)
        {
            page = (TPage)toPage;
        }

        /// <summary>
        /// Saves the document containing this page.
        /// </summary>
        /// <returns></returns>
        /// \ingroup_disabled renderapi_service
        public abstract FileInfo savePage(String name, reportOutputFormatName format = reportOutputFormatName.none);

        #region -----------  page  -------  [current page]

        private TPage _page;

        /// <summary>
        /// current page
        /// </summary>
        /// \ingroup_disabled renderapi_service
        // [XmlIgnore]
        [Category("imbDocumentBuilderBase")]
        [DisplayName("page")]
        [Description("current page")]
        public TPage page
        {
            get
            {
                return _page;
            }
            set
            {
                // Boolean chg = (_page != value);
                _page = value;
                OnPropertyChanged("page");
                // if (chg) {}
            }
        }

        #endregion -----------  page  -------  [current page]

        #region -----------  document  -------  [document being built]

        private TDoc _document; // = new TDoc();

                                /// <summary>
                                /// document being built
                                /// </summary>
                                /// \ingroup_disabled renderapi_service
        // [XmlIgnore]
        [Category("imbDocumentBuilderBase")]
        [DisplayName("document")]
        [Description("document being built")]
        public TDoc document
        {
            get
            {
                return _document;
            }
            set
            {
                // Boolean chg = (_document != value);
                _document = value;
                OnPropertyChanged("document");
                // if (chg) {}
            }
        }

        #endregion -----------  document  -------  [document being built]

        // = new styleTheme("#f69c55", "#4a525a", "#0095ff", 24, 10, new fourSideSetting(4, 8), new fourSideSetting(2, 4), aceFont.Helvetica, aceFont.Impact);
        /// <summary>
        /// Gets or sets the style.
        /// </summary>
        /// <value>
        /// The style.
        /// </value>
        /// \ingroup_disabled renderapi_service
        public styleTheme theme
        {
            get
            {
                return context.theme;
            }
        }

        ///// <summary>
        ///// Current scope within the context
        ///// </summary>
        ///// <value>
        ///// The scope
        ///// </value>
        ///// \ingroup_disabled renderapi_service
        //public IMetaContentNested scope
        //{
        //    get
        //    {
        //        return context.scope;
        //    }
        //}

        /// <summary>
        /// Aapply call default target - used when <c>metaModelTargetEnum.defaultTarget</c> argument is passed
        /// </summary>
        protected metaModelTargetEnum applyCallDefaultTarget = metaModelTargetEnum.asAppend;

        protected imbDocumentBuilderBase(reportAPI __builderAPI = reportAPI.textBuilder) : base(__builderAPI)
        {
        }

        public imbDocumentBuilderBase(int __tabLevel) : base(__tabLevel)
        {
        }

        public imbDocumentBuilderBase() : base()
        {
        }

        /// <summary>
        /// Sets document description information
        /// </summary>
        /// <param name="data">Property collection that applies info</param>
        /// <param name="alsoAppendAsPairs">If TRUE it will also create output using <c>AppendPairs</c> method</param>
        /// <param name="fieldsToUse">What fields to use?</param>
        /// <returns>DOM of created Pairs - if <c>alsoAppendAsPairs</c> was TRUE</returns>
        public abstract object AppendInfo(PropertyCollection data, bool alsoAppendAsPairs, params templateFieldBasic[] fieldsToUse);

        /// <summary>
        /// Loads the page from filepath. If it is document type then imports page with targeted name
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public abstract object loadPage(string filepath, string name = "");

        /// <summary>
        /// Gets the document instance - current document
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Usual application do not require document instance outside of builder class.
        /// But if you need some special thing to do.. here it is
        /// </remarks>
        /// \ingroup_disabled renderapi_service
        object ITextRender.getDocument() => this.getDocument();

        /// <summary>
        /// Adds new page, drives cursor to upper-left corner
        /// </summary>
        /// <param name="name">The name of newly created page.</param>
        /// <param name="scopeToNew"></param>
        /// <param name="mode">In case page with the same name already exists</param>
        /// <returns>
        /// Page object - usually not directly used
        /// </returns>
        /// \ingroup_disabled renderapi_service
        object ITextRender.addPage(string name, bool scopeToNew, getWritableFileMode mode, reportOutputFormatName format = reportOutputFormatName.none)
        {
            return addPage(name, scopeToNew, mode);
        }

        /// <summary>
        /// Gets the page - if name is empty returns the current page
        /// </summary>
        /// <param name="name">The name to use as selector</param>
        /// <returns>
        /// Page object
        /// </returns>
        /// \ingroup_disabled renderapi_service
        object IDocumentRender.getPage(string name) => this.getPage() as Object;

        public abstract void ApplyStyle(IStyleInstruction shot, selectRangeArea areaToApply, params Object[] resources);

        public abstract void ApplyStyle(areaStyleInstruction ins);

        public abstract void ApplyColor(String hexColorCode, selectRangeArea area = null, Boolean toForeground = false);

        public abstract void ApplyColumn(Int32 column, Int32 width, textCursorZoneCorner align, Boolean doAutofit = false);

        public abstract void ApplyRow(Int32 row, Int32 height, textCursorZoneCorner align, Boolean doAutofit = false);

        public void AppendLine()
        {
            throw new NotImplementedException();
        }
    }
}