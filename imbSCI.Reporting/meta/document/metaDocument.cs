// --------------------------------------------------------------------------------------------------------------------
// <copyright file="metaDocument.cs" company="imbVeles" >
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
// Project: imbSCI.Reporting
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Reporting.meta.document
{
    using imbSCI.Core.collection;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.interfaces;
    using imbSCI.Core.reporting.format;
    using imbSCI.Core.reporting.style.core;
    using imbSCI.Data;
    using imbSCI.Data.enums;
    using imbSCI.Data.enums.fields;
    using imbSCI.Data.interfaces;
    using imbSCI.Reporting.interfaces;
    using imbSCI.Reporting.links;
    using imbSCI.Reporting.meta.blocks;
    using imbSCI.Reporting.meta.collection;
    using imbSCI.Reporting.meta.core;
    using imbSCI.Reporting.meta.page;
    using imbSCI.Reporting.script;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;

    /// <summary>
    /// One page inside metaDocument. It may represent one file or multiple files nested in folder
    /// </summary>
    /// <remarks>
    /// Document may have its service pages> indexPage, readmePage,
    /// </remarks>
    /// \ingroup_disabled docDocument
    /// \ingroup_disabled docCore
    public abstract class metaDocument : MetaContentNestedBase, IObjectWithDescription, IMetaContentNested, IMetaHasHeader, IMetaComposeAndConstruct, IObjectWithPathAndChildSelector //IMetaConstruct, IMetaCompose
    {
        //public override IMetaContentNested SearchForChild(string needle)
        //{
        //    needle = CleanNeedle(needle);
        //    if (this.name == needle)
        //    {
        //        return this;
        //    }
        //    return pages.FirstOrDefault(x => x.name.Contains(needle, StringComparison.CurrentCultureIgnoreCase));
        //}

        public override reportElementLevel elementLevel
        {
            get
            {
                return reportElementLevel.document;
            }
        }

        // public abstract metaPage indexPage { get; }

        public virtual metaPage indexPage
        {
            get
            {
                return index;
            }
        }

        private metaCustomizedSimplePage _index;

        /// <summary> </summary>
        public metaCustomizedSimplePage index
        {
            get
            {
                if (_index == null)
                {
                    _index = new metaCustomizedSimplePage(documentTitle, documentDescription);
                    _index.name = "index";
                }
                return _index;
            }
            protected set
            {
                _index = value;
                OnPropertyChanged("index");
            }
        }

        private reportLinkCollection _menu_documentmenu;

        /// <summary> </summary>
        public reportLinkCollection menu_documentmenu
        {
            get
            {
                if (_menu_documentmenu == null) _menu_documentmenu = new reportLinkCollection(title, description);
                return _menu_documentmenu;
            }
            protected set
            {
                _menu_documentmenu = value;
                OnPropertyChanged("menu_documentmenu");
            }
        }

        /// <summary>
        ///
        /// </summary>
        public string description
        {
            get { return documentDescription; }
            set { _documentDescription = value; }
        }

        public override string title
        {
            get
            {
                return documentTitle;
            }

            set
            {
                documentTitle = title;
            }
        }

        public override PropertyCollection AppendDataFields(PropertyCollection data = null)
        {
            if (data == null) data = new PropertyCollection();

            data[templateFieldBasic.document_name] = name;
            data[templateFieldBasic.document_title] = documentTitle;
            data[templateFieldBasic.document_desc] = documentDescription;
            data[templateFieldBasic.document_id] = id;
            data[templateFieldBasic.document_type] = GetType().Name;
            data[templateFieldBasic.document_path] = path;
            if (parent != null)
            {
                parent.AppendDataFields(data);
            }

            return data;
        }

        public override string logStructure(string prefix = "")
        {
            if (imbSciStringExtensions.isNullOrEmpty(prefix)) prefix = ":";

            string output = prefix;

            output = output.add(name + "[" + Count() + "]", Environment.NewLine);

            if (pages.Any())
            {
                foreach (IMetaContentNested it in pages)
                {
                    output = output.add(it.logStructure(":"), ":");
                }
            }
            return output;
        }

        /// <summary>
        /// Collects data trough this meta node and its children
        /// </summary>
        /// <param name="data">PropertyCollectionDictionary to fill in</param>
        /// <returns>
        /// New or updated Dictionary
        /// </returns>
        public virtual PropertyCollectionDictionary collect(PropertyCollectionDictionary data = null)
        {
            if (data == null) data = new PropertyCollectionDictionary();

            AppendDataFields(data[path]);

            delivery.deliveryInstance del = context as delivery.deliveryInstance;
            del.collectOperationStart(context, this, data);

            foreach (metaPage pg in pages)
            {
                pg.collect(data);
            }

            return data;
        }

        #region --- form ------- filesystem form of document

        private reportOutputForm _form = reportOutputForm.folder;

        /// <summary>
        /// filesystem form of document
        /// </summary>
        public reportOutputForm form
        {
            get
            {
                return _form;
            }
            set
            {
                _form = value;
                OnPropertyChanged("form");
            }
        }

        #endregion --- form ------- filesystem form of document

        /// <summary>
        /// Constructs the specified resources.
        /// </summary>
        /// <param name="resources"></param>
        public abstract void construct(object[] resources); //compose(IMetaComposer composer, metaDocumentTheme theme, PropertyCollection data, params object[] resources)

        /// <summary>
        /// Basic <c>construct</c> procedure - takes <c>theme</c>
        /// </summary>
        /// <param name="resources">The resources.</param>
        public virtual void baseConstruct(object[] resources)
        {
            List<object> reslist = resources.getFlatList<object>();

            theme = reslist.getFirstOfType<styleTheme>(false, null);

            foreach (metaPage pg in pages)
            {
                if (pg != null)
                {
                    try
                    {
                        pg.construct(resources);
                    }
                    catch (Exception ex)
                    {
                        logger.log("Base Contruct exception: " + ex.Message);
                    }
                }
            }

            pages.Sort();

            // pages.setParentToItems(this);
            //service// pages.setParentToItems(this);
        }

        /// <summary>
        /// Composes the specified script.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <returns></returns>
        public abstract docScript compose(docScript script);

        public const int menugroup_pages_priority = 20;

        /// <summary>
        /// Bases the compose.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <returns></returns>
        public virtual docScript baseCompose(docScript script)
        {
            if (script == null) script = new docScript(name);

            pages.Sort();

            menu_documentmenu.AddGroup("Pages", "Pages contained in this documen", menugroup_pages_priority);

            foreach (metaPage pg in pages)
            {
                pg.compose(script);
                menu_documentmenu.AddLinkToElement(pg);
            }

            return script;
        }

        /// <summary>
        /// Gets the <see cref="IMetaContentNested"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="IMetaContentNested"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public IMetaContentNested this[string key]
        {
            get
            {
                return pages[key];
            }
        }

        /// <summary>
        /// Gets the <see cref="IMetaContentNested"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="IMetaContentNested"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public IMetaContentNested this[int key]
        {
            get
            {
                return pages[key];
            }
        }

        #region --- documentBottomLine ------- Last line in footer

        private string _documentBottomLine = "";

        /// <summary>
        /// Last line in footer
        /// </summary>
        public string documentBottomLine
        {
            get
            {
                return _documentBottomLine;
            }
            protected set
            {
                _documentBottomLine = value;
                OnPropertyChanged("documentBottomLine");
            }
        }

        #endregion --- documentBottomLine ------- Last line in footer

        #region --- documentDescription ------- Description of the document

        private string _documentDescription = "";

        /// <summary>
        /// Description of the document
        /// </summary>
        public string documentDescription
        {
            get
            {
                return _documentDescription;
            }
            protected set
            {
                _documentDescription = value;
                OnPropertyChanged("documentDescription");
            }
        }

        #endregion --- documentDescription ------- Description of the document

        #region --- documentTitle ------- Display title for document

        private string _documentTitle = "";

        /// <summary>
        /// Display title for document
        /// </summary>
        public string documentTitle
        {
            get
            {
                return _documentTitle;
            }
            protected set
            {
                _documentTitle = value;
                OnPropertyChanged("documentTitle");
            }
        }

        #endregion --- documentTitle ------- Display title for document

        #region --- theme ------- reference to styleTheme

        private styleTheme _theme;

        /// <summary>
        /// reference to styleTheme - meant to be used by others via <c>document</c> reference
        /// </summary>
        public styleTheme theme
        {
            get
            {
                return _theme;
            }
            protected set
            {
                _theme = value;
                OnPropertyChanged("theme");
            }
        }

        #endregion --- theme ------- reference to styleTheme

        #region -----------  scripts  -------  [Scripts to be included in DocumentSet _scripts folder]

        private linksScripts _scripts = new linksScripts();

        /// <summary>
        /// Scripts to be included in DocumentSet _scripts folder
        /// </summary>
        // [XmlIgnore]
        [Category("metaPage")]
        [DisplayName("scripts")]
        [Description("Scripts to be included in DocumentSet _scripts folder")]
        public linksScripts scripts
        {
            get
            {
                return _scripts;
            }
            set
            {
                // Boolean chg = (_scripts != value);
                _scripts = value;
                OnPropertyChanged("scripts");
                // if (chg) {}
            }
        }

        #endregion -----------  scripts  -------  [Scripts to be included in DocumentSet _scripts folder]

        #region -----------  styles  -------  [Styles to be included in DocumentSet _styles folder]

        private linksStylesheets _styles = new linksStylesheets();

        /// <summary>
        /// Styles to be included in DocumentSet _styles folder
        /// </summary>
        // [XmlIgnore]
        [Category("metaPage")]
        [DisplayName("styles")]
        [Description("Styles to be included in DocumentSet _styles folder")]
        public linksStylesheets styles
        {
            get
            {
                return _styles;
            }
            set
            {
                // Boolean chg = (_styles != value);
                _styles = value;
                OnPropertyChanged("styles");
                // if (chg) {}
            }
        }

        #endregion -----------  styles  -------  [Styles to be included in DocumentSet _styles folder]

        #region -----------  pages  -------  [Collection of metaPages]

        //  private metaPageCollection _pages = new metaPageCollection();

        /// <summary>
        /// Collection of metaPages
        /// </summary>
        // [XmlIgnore]
        [Category("metaDocument")]
        [DisplayName("pages")]
        [Description("Collection of metaPages")]
        public metaCollection<IMetaContentNested> pages
        {
            get
            {
                return items;
            }
        }

        public override int Count()
        {
            return pages.Count;
        }

        public override IEnumerator GetEnumerator()
        {
            return pages.GetEnumerator();
        }

        public int indexOf(IMetaContentNested child)
        {
            return pages.IndexOf(child as metaPage);
        }

        public override void sortChildren()
        {
            pages.Sort();
        }

        // public abstract void construct(metaDocumentTheme theme, PropertyCollection data, params object[] resources);

        #endregion -----------  pages  -------  [Collection of metaPages]

        #region -----------  footer  -------  [Footer meta container]

        /// <summary>
        /// Footer meta container
        /// </summary>
        // [XmlIgnore]
        [Category("metaBundle")]
        [DisplayName("footer")]
        [Description("Footer meta container")]
        public metaFooter footer { get; set; } = new metaFooter();

        #endregion -----------  footer  -------  [Footer meta container]

        #region -----------  header  -------  [Header meta container]

        /// <summary>
        /// Header meta container
        /// </summary>
        // [XmlIgnore]
        [Category("metaBundle")]
        [DisplayName("header")]
        [Description("Header meta container")]
        public metaHeader header { get; set; } = new metaHeader();

        object IObjectWithRoot.root
        {
            get
            {
                return root;
            }
        }

        #endregion -----------  header  -------  [Header meta container]
    }
}