// --------------------------------------------------------------------------------------------------------------------
// <copyright file="metaPage.cs" company="imbVeles" >
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
namespace imbSCI.Reporting.meta.page
{
    using imbSCI.Core.collection;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.interfaces;
    using imbSCI.Core.reporting;
    using imbSCI.Core.reporting.colors;
    using imbSCI.Core.reporting.format;
    using imbSCI.Data;
    using imbSCI.Data.enums;
    using imbSCI.Data.enums.appends;
    using imbSCI.Data.enums.fields;
    using imbSCI.Reporting.interfaces;
    using imbSCI.Reporting.links;
    using imbSCI.Reporting.meta.blocks;
    using imbSCI.Reporting.meta.core;
    using imbSCI.Reporting.meta.document;
    using imbSCI.Reporting.meta.documentSet;
    using imbSCI.Reporting.meta.presets.links;
    using imbSCI.Reporting.script;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;

    /// <summary>
    /// META page structure -- containes multiple IMetaContent objects, suports templates and rendering
    /// </summary>
    ///  \ingroup_disabled docPage
    ///   \ingroup_disabled docCore
    public abstract class metaPage : MetaContentNestedBase, IAppendDataFields, IMetaComposeAndConstruct, IPageFormatSettings
    {
        //public override IMetaContentNested SearchForChild(string needle)
        //{
        //    needle = CleanNeedle(needle);
        //    if (this.name == needle)
        //    {
        //        return this;
        //    }
        //    return blocks.FirstOrDefault(x => x.name.Contains(needle, StringComparison.CurrentCultureIgnoreCase));
        //}

        public override reportElementLevel elementLevel
        {
            get
            {
                return reportElementLevel.page;
            }
        }

        public override string title
        {
            get
            {
                return pageTitle;
            }

            set
            {
                pageTitle = value;
            }
        }

        private reportLinkCollection _menu_pagemenu;

        /// <summary> </summary>
        public reportLinkCollection menu_pagemenu
        {
            get
            {
                if (_menu_pagemenu == null) _menu_pagemenu = new reportLinkCollection(title, description);
                return _menu_pagemenu;
            }
            protected set
            {
                _menu_pagemenu = value;
                OnPropertyChanged("menu_pagemenu");
            }
        }

        public reportLinkCollection menu_documentmenu
        {
            get
            {
                if (document != null)
                {
                    metaDocument doc = document as metaDocument;
                    return doc.menu_documentmenu;
                }
                return null;
            }
        }

        public reportLinkCollection menu_documentSetMenu
        {
            get
            {
                metaDocumentSet documentSet = this.getParentOfLevel(reportElementLevel.documentSet, false, true) as metaDocumentSet;
                if (documentSet != null)
                {
                    metaDocumentSet set = documentSet as metaDocumentSet;
                    if (set.isThisRoot)
                    {
                        return null;
                    }
                    return set.menu_documentSetMenu;
                }
                return null;
            }
        }

        public reportLinkCollection menu_rootmenu
        {
            get
            {
                metaDocumentRootSet documentRootSet = this.getParentOfLevel(reportElementLevel.documentSet, true, true) as metaDocumentRootSet;
                if (documentRootSet != null)
                {
                    metaDocumentSet set = documentRootSet as metaDocumentSet;

                    return set.menu_documentSetMenu;
                }
                return null;
            }
        }

        private reportLinkToolbar _toolbar_pagetools = new reportLinkToolbar();

        /// <summary> </summary>
        public reportLinkToolbar toolbar_pagetools
        {
            get
            {
                return _toolbar_pagetools;
            }
            protected set
            {
                _toolbar_pagetools = value;
                OnPropertyChanged("toolbar_pagetools");
            }
        }

        public override PropertyCollection AppendDataFields(PropertyCollection data = null)
        {
            if (data == null) data = new PropertyCollection();

            data[templateFieldBasic.page_name] = name;
            data[templateFieldBasic.page_title] = pageTitle;
            data[templateFieldBasic.page_desc] = pageDescription;
            data[templateFieldBasic.page_id] = id;
            data[templateFieldBasic.page_type] = GetType().Name;
            data[templateFieldBasic.page_path] = path;

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

            output = output.add(name + "[" + Count() + "]");

            if (blocks.Any())
            {
                foreach (IMetaContentNested it in blocks)
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

            //data.Add(path, pageData);

            delivery.deliveryInstance del = context as delivery.deliveryInstance;
            del.collectOperationStart(context, this, data);

            foreach (MetaContainerNestedBase pg in blocks)
            {
                pg.collect(data);
            }

            return data;
        }

        #region --- settings ------- Formating and layout settings of this page

        private pageFormat _settings = new pageFormat();

        /// <summary>
        /// Formating and layout settings of this page
        /// </summary>
        public pageFormat settings
        {
            get
            {
                return _settings;
            }
            set
            {
                _settings = value;
                OnPropertyChanged("settings");
            }
        }

        #endregion --- settings ------- Formating and layout settings of this page

        #region -----------  pageTitle  -------  [Header title content for this page]

        private string _pageTitle = ""; // = new String();

        /// <summary>
        /// Header title content for this page
        /// </summary>
        // [XmlIgnore]
        [Category("metaPage")]
        [DisplayName("pageTitle")]
        [Description("Header title content for this page")]
        public string pageTitle
        {
            get
            {
                return _pageTitle;
            }
            set
            {
                // Boolean chg = (_pageTitle != value);
                _pageTitle = value;
                OnPropertyChanged("pageTitle");
                // if (chg) {}
            }
        }

        #endregion -----------  pageTitle  -------  [Header title content for this page]

        #region -----------  pageDescription  -------  [page description]

        private string _pageDescription = ""; // = new String();

        /// <summary>
        /// page description
        /// </summary>
        // [XmlIgnore]
        [Category("metaPage")]
        [DisplayName("pageDescription")]
        [Description("page description")]
        public string pageDescription
        {
            get
            {
                return _pageDescription;
            }
            set
            {
                // Boolean chg = (_pageDescription != value);
                _pageDescription = value;
                OnPropertyChanged("pageDescription");
                // if (chg) {}
            }
        }

        #endregion -----------  pageDescription  -------  [page description]

        #region -----------  basicBlocksFlags  -------  [Description of $property$]

        private metaPageCommonBlockFlags _basicBlocksFlags; // = new metaPageCommonBlockFlags();

        /// <summary>
        /// Description of $property$
        /// </summary>
        // [XmlIgnore]
        [Category("metaPage")]
        [DisplayName("basicBlocksFlags")]
        [Description("Description of $property$")]
        public metaPageCommonBlockFlags basicBlocksFlags
        {
            get
            {
                return _basicBlocksFlags;
            }
            set
            {
                // Boolean chg = (_basicBlocksFlags != value);
                _basicBlocksFlags = value;
                OnPropertyChanged("basicBlocksFlags");
                // if (chg) {}
            }
        }

        #endregion -----------  basicBlocksFlags  -------  [Description of $property$]

        /// <summary>
        /// Constructs the specified resources.
        /// </summary>
        /// <param name="resources"></param>
        public virtual void construct(object[] resources)
        {
            //List<Object> reslist = resources.getFlatList<Object>();

            if (basicBlocksFlags.HasFlag(metaPageCommonBlockFlags.pageHeader))
            {
                header.name = "header";
                header.priority = 50;
                header.title = "{{{page_title}}}";
                header.description = "{{{page_desc}}}";
                //header.content.Add("{{{page_desc")
                //header.title = pageTitle;
                //header.description = pageDescription;

                blocks.Add(header, this);
            }

            if (basicBlocksFlags.HasFlag(metaPageCommonBlockFlags.pageNavigation))
            {
                navigation.name = "navigation";
                navigation.priority = 60;
                blocks.Add(navigation, this);
            }

            if (basicBlocksFlags.HasFlag(metaPageCommonBlockFlags.documentIndexNavigation))
            {
                linksIndexNavigation indexNav = new linksIndexNavigation();
                indexNav.name = "indexNavigation";
                blocks.Add(indexNav, this);
            }

            if (basicBlocksFlags.HasFlag(metaPageCommonBlockFlags.pageKeywords))
            {
                keywords.name = "keywords";
                keywords.priority = 200;
                blocks.Add(keywords, this);
            }

            if (basicBlocksFlags.HasFlag(metaPageCommonBlockFlags.pageFooter))
            {
                footer.name = "footer";
                footer.priority = 150;
                blocks.Add(footer, this);
            }

            this.subConstruct(resources);

            blocks.setParentToItems(this);
        }

        public virtual docScript compose(docScript script)
        {
            script = this.checkScript(script);

            script.x_scopeIn(this);

            //script.add(appendType.i_page, docScriptArguments.dsa_name, docScriptArguments.dsa_title,docScriptArguments.dsa_description)
            //    .set(name, pageTitle, pageDescription);

            // script.add(appendType.s_settings).arg(docScriptArguments.dsa_stylerSettings, settings);

            script.add(appendType.s_palette).arg(acePaletteRole.colorDefault);

            script = this.subCompose(script);

            script.x_scopeOut(this);

            return script;
        }

        protected metaPage()
        {
        }

        /// <summary>
        /// Gets the <see cref="System.Object"/> with the specified name.
        /// </summary>
        /// <value>
        /// The <see cref="System.Object"/>.
        /// </value>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public IMetaContentNested this[string name]
        {
            get
            {
                return blocks[name]; ///.First(x => x.name == name);
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
                return blocks[key];
            }
        }

        /// <summary>
        /// Returns position of child inside primary collection. If fails returns -1
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        public int indexOf(IMetaContentNested child)
        {
            return blocks.IndexOf(child);
        }

        /// <summary>
        /// Returns number of children in primary collection
        /// </summary>
        /// <returns></returns>
        public override int Count()
        {
            return blocks.Count;
        }

        /// <summary>
        /// Sorts all sub collections
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void sortChildren() => blocks.Sort();

        public override IEnumerator GetEnumerator() => blocks.GetEnumerator();

        // public override void construct(metaDocumentTheme theme, PropertyCollection data, params object[] resources);

        #region -----------  blocks  -------  [blocks inside this page]

        //        private metaContentCollection _blocks = new metaContentCollection();
        /// <summary>
        /// blocks inside this page
        /// </summary>
        // [XmlIgnore]
        [Category("metaPage")]
        [DisplayName("blocks")]
        [Description("blocks inside this page")]
        public metaCollection<IMetaContentNested> blocks => items;

        #endregion -----------  blocks  -------  [blocks inside this page]

        #region -----------  attachments  -------  [Attachments to be included in DocumentSet _attachments]

        private linksAttachments _attachments = new linksAttachments();

        /// <summary>
        /// Attachments to be included in DocumentSet _attachments
        /// </summary>
        // [XmlIgnore]
        [Category("metaPage")]
        [DisplayName("attachments")]
        [Description("Attachments to be included in DocumentSet _attachments")]
        public linksAttachments attachments
        {
            get
            {
                return _attachments;
            }
            set
            {
                // Boolean chg = (_attachments != value);
                _attachments = value;
                OnPropertyChanged("attachments");
                // if (chg) {}
            }
        }

        #endregion -----------  attachments  -------  [Attachments to be included in DocumentSet _attachments]

        #region -----------  navigation  -------  [Top navigation links]

        private linksNavigation _navigation = new linksNavigation();

        /// <summary>
        /// Top navigation links
        /// </summary>
        // [XmlIgnore]
        [Category("metaPage")]
        [DisplayName("navigation")]
        [Description("Top navigation links")]
        public linksNavigation navigation
        {
            get
            {
                return _navigation;
            }
            set
            {
                // Boolean chg = (_navigation != value);
                _navigation = value;
                OnPropertyChanged("navigation");
                // if (chg) {}
            }
        }

        #endregion -----------  navigation  -------  [Top navigation links]

        #region -----------  keywords  -------  [Keywords for the document]

        /// <summary>
        /// Keywords for the document
        /// </summary>
        // [XmlIgnore]
        [Category("metaBundle")]
        [DisplayName("keywords")]
        [Description("Keywords for the document")]
        public metaKeywords keywords { get; set; } = new metaKeywords();

        #endregion -----------  keywords  -------  [Keywords for the document]

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

        #endregion -----------  header  -------  [Header meta container]
    }
}