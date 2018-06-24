// --------------------------------------------------------------------------------------------------------------------
// <copyright file="metaDocumentSet.cs" company="imbVeles" >
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
namespace imbSCI.Reporting.meta.documentSet
{
    using imbSCI.Core.collection;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.interfaces;
    using imbSCI.Data;
    using imbSCI.Data.enums;
    using imbSCI.Data.enums.fields;
    using imbSCI.Data.interfaces;
    using imbSCI.Reporting.interfaces;
    using imbSCI.Reporting.links;
    using imbSCI.Reporting.meta.blocks;
    using imbSCI.Reporting.meta.core;
    using imbSCI.Reporting.meta.document;
    using imbSCI.Reporting.meta.page;
    using imbSCI.Reporting.script;
    using System;
    using System.ComponentModel;
    using System.Data;

    /// <summary>
    /// Root reporting object - supports multiply documents and hierarchy
    /// UPDATE: Now not :) <see cref="metaDocumentRootSet"/>
    /// </summary>
    /// <remarks>
    /// <para>Meta model is universal data and structure model for imbVeles reporting</para>
    /// <para>It supports multi format and structure exporting via metaExporters namespace</para>
    /// <list type="table">
    /// <listheader>Meta reporting model structure</listheader>
    /// <item>
    ///     <term>metaDocumentSet (one test run)</term>
    ///     <description>Master object - represents one complete multi document report.</description>
    /// </item>
    /// <item>
    ///     <term>metaDocument (one item/iteration of test run or aluxuary documents)</term>
    ///     <description>Document contains pages and common settings between them</description>
    /// </item>
    /// <item>
    ///     <term>metaPage (list of content presenters and data)</term>
    ///     <description>Page may contain different IMetaContent objects.</description>
    /// </item>
    /// <item>
    ///     <term>metaContent (instance of IMetaContent)</term>
    ///     <description>Content may be: set of links, menu, data table representation, images etc</description>
    /// </item>
    /// </list>
    /// </remarks>
    /// \ingroup_disabled docCore
    public abstract class metaDocumentSet : MetaContentNestedBase, IMetaComposeAndConstruct, IObjectWithPathAndChildSelector
    {
        public metaDocumentSet()
        {
        }

        public void Add(object input)
        {
            if (input is metaDocumentSet)
            {
                metaDocumentSet input_metaDocumentSet = (metaDocumentSet)input;
                documentSets.Add(input_metaDocumentSet, this);
                return;
            }

            if (input is metaDocument)
            {
                metaDocument input_metaDocument = (metaDocument)input;
                documents.Add(input_metaDocument, this);
                return;
            }

            if (input is metaPage)
            {
                metaPage input_metaPage = (metaPage)input;
                pages.Add(input_metaPage, this);
                return;
            }
        }

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
                    _index = new metaCustomizedSimplePage(documentSetTitle, documentSetDescription);
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

        public override reportElementLevel elementLevel
        {
            get
            {
                return reportElementLevel.documentSet;
            }
        }

        private reportLinkCollection _menu_documentSetMenu;

        /// <summary> </summary>
        public reportLinkCollection menu_documentSetMenu
        {
            get
            {
                if (_menu_documentSetMenu == null) _menu_documentSetMenu = new reportLinkCollection(title, description);
                return _menu_documentSetMenu;
            }
            protected set
            {
                _menu_documentSetMenu = value;
                OnPropertyChanged("menu_documentSetMenu");
            }
        }

        public override string title
        {
            get
            {
                return documentSetTitle;
            }

            set
            {
                documentSetTitle = title;
            }
        }

        public override PropertyCollection AppendDataFields(PropertyCollection data = null)
        {
            if (data == null) data = new PropertyCollection();

            data[templateFieldBasic.documentset_name] = name;
            data[templateFieldBasic.documentset_title] = documentSetTitle;
            data[templateFieldBasic.documentset_desc] = documentSetDescription;
            data[templateFieldBasic.documentset_id] = id;
            data[templateFieldBasic.documentset_type] = GetType().Name;
            data[templateFieldBasic.documentset_path] = path;

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

            if (documents.Any())
            {
                foreach (IMetaContentNested it in documents)
                {
                    output = output.add(it.logStructure(":"), ":");
                }
            }
            return output;
        }

        #region -----------  notation  -------  [Extra notation data]

        private metaNotation _notation; // = new metaNotation();

        /// <summary>
        /// Extra notation data
        /// </summary>
        // [XmlIgnore]
        [Category("metaBundle")]
        [DisplayName("notation")]
        [Description("Extra notation data")]
        public metaNotation notation
        {
            get
            {
                if (_notation == null)
                {
                    _notation = new metaNotation();
                    _notation.parent = this;
                }
                return _notation;
            }
            set
            {
                // Boolean chg = (_notation != value);
                _notation = value;
                //  OnPropertyChanged("notation");
                // if (chg) {}
            }
        }

        #endregion -----------  notation  -------  [Extra notation data]

        /// <summary>
        /// Composes the specified script.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <returns></returns>
        public abstract docScript compose(docScript script);

        public const int menugroup_pages_priority = 20;
        public const int menugroup_documents_priority = 30;
        public const int menugroup_documentsets_priority = 40;

        /// <summary>
        /// Bases the compose.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <returns></returns>
        public virtual docScript baseCompose(docScript script)
        {
            if (script == null) script = new docScript(name);

            int c = 0;

            pages.Sort();

            menu_documentSetMenu.AddGroup("Pages", "Pages contained in this document set ", menugroup_pages_priority);

            foreach (metaPage pg in pages)
            {
                pg.compose(script);
                menu_documentSetMenu.AddLinkToElement(pg).priority = pg.priority;
            }

            documents.Sort();

            menu_documentSetMenu.AddGroup("Documents", "Documents contained within the document set", menugroup_documents_priority);

            foreach (metaDocument pg in documents)
            {
                pg.compose(script);
                menu_documentSetMenu.AddLinkToElement(pg).priority = pg.priority;
            }

            documentSets.Sort();

            menu_documentSetMenu.AddGroup("Sub-reports", "Document sets contained within this document set", menugroup_documentsets_priority);

            foreach (metaDocumentSet pg in documentSets)
            {
                pg.compose(script);
                menu_documentSetMenu.AddLinkToElement(pg).priority = pg.priority;
            }

            return script;
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

            foreach (metaDocumentSet pg in documentSets)
            {
                pg.collect(data);
            }

            foreach (metaDocument pg in documents)
            {
                pg.collect(data);
            }

            foreach (metaPage pg in pages)
            {
                pg.collect(data);
            }

            return data;
        }

        /// <summary>
        /// Constructs the specified resources.
        /// </summary>
        /// <param name="resources"></param>
        public abstract void construct(object[] resources);

        /// <summary>
        /// Bases the construct.
        /// </summary>
        /// <param name="resources"></param>
        public virtual void baseConstruct(object[] resources)
        {
            documentSets.setParentToItems(this);
            documents.setParentToItems(this);
            pages.setParentToItems(this);

            foreach (metaDocumentSet pg in documentSets)
            {
                try
                {
                    pg.construct(resources);
                }
                catch (Exception ex)
                {
                    if (logger != null) logger.log("Document set - construct exception: " + ex.Message);
                }
            }

            foreach (metaDocument pg in documents)
            {
                try
                {
                    pg.construct(resources);
                }
                catch (Exception ex)
                {
                    if (logger != null) logger.log("Document - construct exception: " + ex.Message);
                }
            }

            foreach (metaPage pg in pages)
            {
                try
                {
                    pg.construct(resources);
                }
                catch (Exception ex)
                {
                    if (logger != null) logger.log("Page - construct exception: " + ex.Message);
                }
            }

            documentSets.setParentToItems(this);
            documents.setParentToItems(this);
            pages.setParentToItems(this);
        }

        #region --- documentSetTitle ------- Title describing complete documentSet

        private string _documentSetTitle = "";

        /// <summary>
        /// Title describing complete documentSet
        /// </summary>
        public string documentSetTitle
        {
            get
            {
                return _documentSetTitle;
            }
            set
            {
                _documentSetTitle = value;
                OnPropertyChanged("documentSetTitle");
            }
        }

        #endregion --- documentSetTitle ------- Title describing complete documentSet

        #region --- documentSetDescription ------- Short description of complete documentSet

        private string _documentSetDescription = "";

        /// <summary>
        /// Short description of complete documentSet
        /// </summary>
        public string documentSetDescription
        {
            get
            {
                return _documentSetDescription;
            }
            set
            {
                _documentSetDescription = value;
                OnPropertyChanged("documentSetDescription");
            }
        }

        #endregion --- documentSetDescription ------- Short description of complete documentSet

        // private metaDocumentCollection _documents = new metaDocumentCollection();

        private metaCollection<metaDocumentSet> _documentSets;

        /// <summary>
        /// Sub document sets
        /// </summary>
        public metaCollection<metaDocumentSet> documentSets
        {
            get
            {
                if (_documentSets == null)
                {
                    _documentSets = new metaCollection<metaDocumentSet>();
                    _documentSets.discoverCommonParent(this);
                }
                return _documentSets;
            }
            set { _documentSets = value; }
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
                return documents[key];
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
                return documents[key];
            }
        }

        /// <summary>
        /// Collection of metaDocuments inside the set
        /// </summary>
        [Category("metaDocumentSet")]
        [DisplayName("documents")]
        [Description("Collection of metaDocuments inside the set")]
        public metaCollection<IMetaContentNested> documents
        {
            get
            {
                return items;
            }
        }

        #region -----------  servicePages  -------  [Registry of service pages used by this document]

        private metaCollection<metaPage> _servicePages;

        /// <summary>
        /// Registry of service pages used by this document
        /// </summary>
        [Category("metaDocument")]
        [DisplayName("servicePages")]
        [Description("Registry of service pages used by this document")]
        public metaCollection<metaPage> pages
        {
            get
            {
                if (_servicePages == null)
                {
                    _servicePages = new metaCollection<metaPage>();
                    _servicePages.discoverCommonParent(this);
                }
                return _servicePages;
            }
        }

        #endregion -----------  servicePages  -------  [Registry of service pages used by this document]

        private IMetaContentNested root
        {
            get
            {
                if (parent != null) return parent.root;
                return this;
            }
        }

        // public abstract void construct(metaDocumentTheme theme, PropertyCollection data, params object[] resources);

        ///// <summary>
        ///// Returns number of children in primary collection
        ///// </summary>
        ///// <returns></returns>
        //public override int Count()
        //{
        //    return documents.Count;
        //}

        ///// <summary>
        ///// Gets the enumerator.
        ///// </summary>
        ///// <returns></returns>
        //public override IEnumerator GetEnumerator()
        //{
        //    return documents.GetEnumerator();
        //}

        /// <summary>
        /// Returns position of child inside primary collection. If fails returns -1
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        public int indexOf(IMetaContentNested child)
        {
            return documents.IndexOf(child as metaDocument);
        }

        /// <summary>
        /// Sorts all sub collections
        /// </summary>
        public override void sortChildren()
        {
            documents.Sort();
        }
    }
}