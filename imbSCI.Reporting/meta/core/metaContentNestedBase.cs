// --------------------------------------------------------------------------------------------------------------------
// <copyright file="metaContentNestedBase.cs" company="imbVeles" >
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
namespace imbSCI.Reporting.meta.core
{
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.enumworks;
    using imbSCI.Core.interfaces;
    using imbSCI.Core.math;
    using imbSCI.Core.reporting;
    using imbSCI.Core.reporting.render;
    using imbSCI.Core.reporting.style.enums;
    using imbSCI.Data;
    using imbSCI.Data.data;
    using imbSCI.Data.enums;
    using imbSCI.Data.enums.fields;
    using imbSCI.Data.interfaces;
    using imbSCI.Reporting.meta.document;
    using imbSCI.Reporting.meta.documentSet;
    using imbSCI.Reporting.meta.page;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Linq;
    using System.Xml.Serialization;

    /// <summary>
    /// Meta container base class
    /// </summary>
    public abstract class MetaContentNestedBase : imbBindable, IMetaContentNested, IObjectWithPathAndChildSelector, IObjectWithPath, IObjectWithReportLevel
    {
        private ILogBuilder _logger;

        /// <summary>
        /// Gets the logger of the root element - or it's own if it has no parent
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        public ILogBuilder logger
        {
            get
            {
                if (parent != null)
                {
                    MetaContentNestedBase p = parent as MetaContentNestedBase;
                    if (p != null) return p.logger;
                }

                return _logger;
            }
            set { _logger = value; }
        }

        public abstract reportElementLevel elementLevel { get; }

        public abstract string title { get; set; }

        public virtual int Count()
        {
            return items.Count();
        }

        //public virtual void sortChildren()
        //{
        //    items.Sort();
        //}

        // public abstract Int32 Count { get; }

        public virtual void sortChildren() => items.Sort();

        private string _description; // = new String();

        /// <summary>
        /// Description content under the main name
        /// </summary>
        // [XmlIgnore]
        [Category("metaHeader")]
        [DisplayName("description")]
        [Description("Description content under the main name")]
        public string description
        {
            get
            {
                return _description;
            }
            set
            {
                // Boolean chg = (_description != value);
                _description = value;
                OnPropertyChanged("description");
                // if (chg) {}
            }
        }

        private IRenderExecutionContext _context;

        /// <summary>
        /// Execution context reference
        /// </summary>
        [XmlIgnore]
        public IRenderExecutionContext context
        {
            get
            {
                if (parent != null)
                {
                    return parent.context;
                }
                else
                {
                    return _context;
                }
            }
            set
            {
                if (parent != null)
                {
                    parent.context = value;
                }
                else
                {
                    _context = value;
                }
            }
        }

        /// <summary>
        /// Logs the structure.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <returns></returns>
        public virtual string logStructure(string prefix = "")
        {
            if (imbSciStringExtensions.isNullOrEmpty(prefix)) prefix = ":";

            string output = prefix;

            output = output.add(name + "[" + Count() + "]", Environment.NewLine);

            if (items.Any())
            {
                foreach (IMetaContentNested it in items)
                {
                    output = output.add(it.logStructure(":"), ":");
                }
            }
            return output;
        }

#pragma warning disable CS1574 // XML comment has cref attribute 'getChildByPath(IObjectWithChildSelector, string)' that could not be resolved
#pragma warning disable CS1574 // XML comment has cref attribute 'metaModelTargetEnum' that could not be resolved
#pragma warning disable CS1574 // XML comment has cref attribute 'metaModelTargetEnum' that could not be resolved
        /// <summary>
        /// Resolves <see cref="imbSCI.Reporting.reporting.style.enums.metaModelTargetEnum" /> against <c>metaContent</c> DOM
        /// </summary>
        /// <param name="target">The kind of relative navigation applied</param>
        /// <param name="needle">The needle: text/string/path used by certain <see cref="imbSCI.Reporting.reporting.style.enums.metaModelTargetEnum" /></param>
        /// <param name="output">Default <see cref="IMetaContentNested" /> if resolve fails</param>
        /// <returns>
        /// Result of resolution
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        /// <seealso cref="imbSCI.Data.extensions.data.imbPathExtensions.getChildByPath(IObjectWithChildSelector, string)" />
        public virtual List<IMetaContentNested> resolve(metaModelTargetEnum target, string needle = "", IMetaContentNested output = null)
#pragma warning restore CS1574 // XML comment has cref attribute 'metaModelTargetEnum' that could not be resolved
#pragma warning restore CS1574 // XML comment has cref attribute 'metaModelTargetEnum' that could not be resolved
#pragma warning restore CS1574 // XML comment has cref attribute 'getChildByPath(IObjectWithChildSelector, string)' that could not be resolved
        {
            bool addDefault = false;

            List<IMetaContentNested> list = new List<IMetaContentNested>();

            needle = needle.ToLower();
            int ind = 0;
            if (parent != null)
            {
                ind = parent.indexOf(this);
            }

            switch (target)
            {
                case metaModelTargetEnum.none:
                    output = null;
                    break;

                case metaModelTargetEnum.defaultTarget:
                    list.Add(output);
                    break;

                case metaModelTargetEnum.document:
                    output = document;
                    break;

                case metaModelTargetEnum.page:
                    output = page;
                    break;

                case metaModelTargetEnum.scope:
                    output = this;
                    break;

                case metaModelTargetEnum.scopeParent:
                    output = parent;
                    break;

                case metaModelTargetEnum.scopeEachChild:

                    if (Count() > 0)
                    {
                        foreach (IMetaContentNested ch in this)
                        {
                            if (imbSciStringExtensions.isNullOrEmpty(needle))
                            {
                                list.Add(ch);
                            }
                            else
                            {
                                if (ch.name.ToLower().Contains(needle))
                                {
                                    list.Add(ch);
                                }
                            }
                        }
                    }

                    break;

                case metaModelTargetEnum.scopeChild:
                    if (Count() > 0)
                    {
                        foreach (IMetaContentNested ch in this)
                        {
                            if (imbSciStringExtensions.isNullOrEmpty(needle))
                            {
                                list.Add(ch);
                                break;
                            }
                            else
                            {
                                if (ch.name.ToLower().Contains(needle))
                                {
                                    list.Add(ch);
                                    break;
                                }
                            }
                        }
                    }
                    break;

                case metaModelTargetEnum.scopeRelativePath:
                    list.Add(this.getChildByPath(needle) as IMetaContentNested);

                    break;

                case metaModelTargetEnum.lastAppend:
                    if (parent != null)
                    {
                        list.AddMultiple(this.getChildByPath(string.Format("..\\[{0}]", ind - 1)));
                    }
                    break;

                case metaModelTargetEnum.nextAppend:
                    if (parent != null)
                    {
                        list.AddMultiple(this.getChildByPath(string.Format("..\\[{0}]", ind + 1)));
                    }
                    break;

                //case metaModelTargetEnum.pencil:

                //case metaModelTargetEnum.setStandard:
                //case metaModelTargetEnum.unsetStandard:
                //case metaModelTargetEnum.setNamedStandard:
                //case metaModelTargetEnum.unsetNamedStandard:
                //case metaModelTargetEnum.asAppend:
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (output == null)
            {
                addDefault = false;
            }

            // if (output == null) output = this;

            if (addDefault)
            {
                if (list.Count == 0)
                {
                    list.Add(output);
                }
            }

            return list;
        }

        private metaCollection<IMetaContentNested> _items;

        /// <summary>
        /// Default primary children collection
        /// </summary>
        public metaCollection<IMetaContentNested> items
        {
            get
            {
                if (_items == null)
                {
                    _items = new metaCollection<IMetaContentNested>();
                    _items.discoverCommonParent(this);
                }

                return _items;
            }
        }

        public virtual IEnumerator GetEnumerator()
        {
            return items.GetEnumerator();
        }

        /// <summary>
        /// Searches for children in all available collections
        /// </summary>
        /// <param name="needle">The needle.</param>
        /// <returns></returns>
        //   public abstract IMetaContentNested SearchForChild(String needle);

        //public virtual IMetaContentNested SearchForChildByPath(String path)
        //{
        //    if (regDone)
        //    {
        //        ((metaDocumentRootSet)root).regPathGet(path);
        //    }

        //    //List<String> prts = path.getPathParts();

        //    //IMetaContentNested head = this;

        //    //for (int i = 0; i < prts.Count(); i++)
        //    //{
        //    //    String needle = CleanNeedle(prts[i]);
        //    //    head = head.SearchForChild(needle);
        //    //    if (head == null)
        //    //    {
        //    //        //
        //    //        break;
        //    //    } else
        //    //    {
        //    //        // continues
        //    //    }

        //    //}

        //    //return head;

        //}

        protected string CleanNeedle(string key)
        {
            key = key.Trim('\\');
            key = key.Trim('/');
            return key;
        }

        public virtual object this[int key]
        {
            get
            {
                return items[key];
            }
        }

        public virtual object this[string key]
        {
            get
            {
                return items[key];
                //return SearchForChildByPath(key);
            }
        }

        public virtual int indexOf(IObjectWithChildSelector child)
        {
            IMetaContentNested __child = child as IMetaContentNested;

            if (__child == null) return -1;
            return items.IndexOf(__child);
        }

        public virtual IEnumerable<IMetaContentNested> children
        {
            get
            {
                return items;
            }
        }

        internal static int PRIORITY_UNKNOWN = metaServicePagePosition.unknown.ToInt32();

        /// <summary>
        /// Appends its data points into new or existing property collection
        /// </summary>
        /// <param name="data">Property collection to add data into</param>
        /// <returns>Updated or newly created property collection</returns>
        public virtual PropertyCollection AppendDataFields(PropertyCollection data = null)
        {
            if (data == null) data = new PropertyCollection();

            //systemStatusData sdata = imbSystemInfo.getSystemStatusData();

            metaDocument asDocument = document as metaDocument;
            metaPage asPage = page as metaPage;

            IMetaContentNested asMetacontent = this as IMetaContentNested;
            if (asMetacontent != null)
            {
                data[templateFieldBasic.document_relpath] = asMetacontent.getPathToParent(document);
                data[templateFieldBasic.page_relpath] = asMetacontent.getPathToParent(page);
                // data[templateFieldBasic.root_relpath] = asMetacontent.getPathToParent(root);
            }
            //data[templateFieldBasic.sys_time] = DateTime.Now.ToShortTimeString();
            //ata[templateFieldBasic.sys_threads] = sdata.threads;
            //data[templateFieldBasic.sys_mem] = this.getPathToParent(asDocument);
            //data[templateFieldBasic.sys_log] = asDocument.header.name;
            //data[templateFieldBasic.sys_runtime] = sdata.timeRunning;

            if (asPage != null)
            {
                data[templateFieldBasic.page_name] = asPage.name;
                data[templateFieldBasic.page_title] = asPage.pageTitle;
                data[templateFieldBasic.page_desc] = asPage.pageDescription;
                data[templateFieldBasic.page_id] = asPage.id;
                data[templateFieldBasic.page_number] = asPage.id; // parent.Count();
                data[templateFieldBasic.page_type] = asPage.GetType().Name;
                if (asPage.parent != null)
                {
                    data[templateFieldBasic.page_count] = asPage.parent.Count();
                }
                else
                {
                    data[templateFieldBasic.page_count] = 0;
                }
            }

            if (asDocument != null)
            {
                if (asDocument.parent != null)
                {
                    data[templateFieldBasic.document_count] = asDocument.parent.Count();
                }
                else
                {
                    data[templateFieldBasic.document_count] = 0;
                }

                data[templateFieldBasic.document_desc] = asDocument.documentDescription;
                data[templateFieldBasic.document_name] = asDocument.name;
                data[templateFieldBasic.document_number] = asDocument.id; // parent.Count();
                data[templateFieldBasic.document_path] = asDocument.path;

                data[templateFieldBasic.document_title] = asDocument.documentTitle;
                data[templateFieldBasic.document_type] = asDocument.GetType().Name;
                data[templateFieldBasic.document_id] = asDocument.id;
            }

            data[templateFieldMetaBlock.block_name] = name;
            data[templateFieldMetaBlock.block_id] = id;
            data[templateFieldMetaBlock.block_priority] = priority;
            data[templateFieldMetaBlock.block_type] = GetType().Name;

            return data;
        }

        /// <summary>
        /// Is visibility changed?
        /// </summary>
        /// <param name="newValue">Value of visibility to be set</param>
        /// <returns></returns>
        public bool setVisibility(bool newValue)
        {
            if (visible == newValue) return false;
            visible = newValue;
            return true;
        }

        /// <summary>
        /// The first document in parent structure - closest to this
        /// </summary>
        public IMetaContentNested document
        {
            get
            {
                if (this is metaDocument) return this as metaDocument;
                IObjectWithParent doc = this.getParentOfLevel(reportElementLevel.document);

                if (doc is metaDocument)
                {
                    return parent.document as IMetaContentNested;
                }
                return null;
            }
        }

        /// <summary>
        /// The first page in parent structure - closest to this
        /// </summary>
        public IMetaContentNested page
        {
            get
            {
                if (this is metaPage) return this as metaPage;

                if (parent != null)
                {
                    return parent.page as IMetaContentNested;
                }

                return null;
            }
        }

        /// <summary>
        /// Top parent object
        /// </summary>
        public IMetaContentNested root
        {
            get
            {
                if (parent != null)
                {
                    return parent.root;
                }
                return this as metaDocumentRootSet;
            }
        }

        /// <summary>
        /// Name of this instance of nested content object
        /// </summary>

        #region --- name ------- Naziv komponente

        private string _name = "";

        /// <summary>
        /// Naziv komponente
        /// </summary>
        public virtual string name
        {
            get
            {
                if (_name == "")
                {
                    _name = GetType().Name;
                }
                if (_name.Contains("Specification"))
                {
                }
                return _name;
            }
            set
            {
                if (imbSciStringExtensions.isNullOrEmpty(value)) return;
                _name = value;
                if (_name.Length > 40)
                {
                    string _pref = _name.Substring(0, 4);
                    _name = _pref + md5.GetMd5Hash(_name);
                }

                OnPropertyChanged("name");
            }
        }

        #endregion --- name ------- Naziv komponente

        #region -----------  id  -------  [ID]

        private long _id; // = new Int32();

        /// <summary>
        /// ID
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        // [XmlIgnore]
        [Category("metaContentNestedBase")]
        [DisplayName("id")]
        [Description("ID")]
        public long id
        {
            get
            {
                return _id;
            }
            set
            {
                // Boolean chg = (_id != value);
                _id = value;
                OnPropertyChanged("id");
                // if (chg) {}
            }
        }

        #endregion -----------  id  -------  [ID]

        #region --- priority ------- Priority defines order of blocks

        private int _priority = 100;

        /// <summary>
        /// Priority defines order of blocks
        /// </summary>
        public int priority
        {
            get
            {
                return _priority;
            }
            set
            {
                if (_priority == PRIORITY_UNKNOWN) return;
                _priority = value;
                OnPropertyChanged("priority");
            }
        }

        #endregion --- priority ------- Priority defines order of blocks

        #region --- visible ------- Controls render visibility of the content block

        private bool _visible = true;

        /// <summary>
        /// Controls render visibility of the content block
        /// </summary>
        public bool visible
        {
            get
            {
                return _visible;
            }
            set
            {
                _visible = value;
                OnPropertyChanged("visible");
            }
        }

        #endregion --- visible ------- Controls render visibility of the content block

        // #region -----------  content  -------  [Text content of the meta item. For headers it is written below description block. For footers it comes before bottomline]
        private List<string> _content = new List<string>();

        /// <summary>
        /// Text content of the meta item. For headers it is written below description block. For footers it comes before bottomline
        /// </summary>
        // [XmlIgnore]
        [Category("MetaContentNestedBase")]
        [DisplayName("content")]
        [Description("Text content of the meta item. For headers it is written below description block. For footers it comes before bottomline")]
        public List<string> content
        {
            get
            {
                return _content;
            }
            set
            {
                // Boolean chg = (_content != value);
                _content = value;
                OnPropertyChanged("content");
                // if (chg) {}
            }
        }

        #region --- parent ------- refernca prema parent objektu

        private IMetaContentNested _parent;

        /// <summary>
        /// refernca prema parent objektu
        /// </summary>
        public IMetaContentNested parent
        {
            get
            {
                return _parent;
            }
            set
            {
                bool regFirst = false;
                if ((_parent == null) && (value != null))
                {
                    regFirst = true;
                }
                else
                {
                    if (root != null)
                    {
                    }
                }
                _parent = value;

                metaDocumentRootSet __root = root as metaDocumentRootSet;
                if (__root != null)
                {
                    __root.regPath(this, regFirst);
                    OnPropertyChanged("parent");
                }
            }
        }

        #endregion --- parent ------- refernca prema parent objektu

        public bool regDone
        {
            get
            {
                return (root is metaDocumentRootSet);
            }
        }

        //    #region --- path ------- Path from root object to this object - including this
        private string _path;

        /// <summary>
        /// Path from root object to this object - including this
        /// </summary>
        public virtual string path
        {
            get
            {
                if (parent != null)
                {
                    _path = parent.path.add(name, "\\");
                }
                else
                {
                    _path = name.addPrefixForRoot();
                }
                // (name, "\\");
                return _path;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is this root page.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is this root page; otherwise, <c>false</c>.
        /// </value>
        public bool isThisRootPage
        {
            get
            {
                if (this == page)
                {
                    if (parent is metaDocument)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is this root.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is this root; otherwise, <c>false</c>.
        /// </value>
        public bool isThisRoot
        {
            get
            {
                return (root == this);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is this document.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is this document; otherwise, <c>false</c>.
        /// </value>
        public bool isThisDocument
        {
            get
            {
                return (document == this);
            }
        }

        object IObjectWithParent.parent => parent;

        object IObjectWithRoot.root => root;
    }
}