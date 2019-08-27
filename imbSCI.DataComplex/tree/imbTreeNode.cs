// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbTreeNode.cs" company="imbVeles" >
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
// Project: imbSCI.DataComplex
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
using imbSCI.Data;
using imbSCI.Data.data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace imbSCI.DataComplex.tree
{
    #region imbVeles using

    using imbSCI.Core.attributes;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.math;
    using imbSCI.Core.reporting.template;
    using imbSCI.Data.interfaces;
    using imbSCI.DataComplex.path;
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Text;

    // using imbCore.resources.typology; //using aceCommonTypes.extensions;

    #endregion imbVeles using

    /// <summary>
    /// imbTree spojnica - može biti grana ili list
    /// </summary>
    [imb(imbAttributeName.xmlEntityOutput, "name,sourcePath")]
    [imb(imbAttributeName.xmlNodeValueProperty, "sourceContent")]
    //[JsonDictionary("nodeTree", "UID", true, null, )
    public class imbTreeNode : imbBindable, IDictionary<string, imbTreeNode>, IObjectWithPath, IObjectWithIDandName, INotifyCollectionChanged,  //IObservable<KeyValuePair<String, imbTreeNode>>
                                        IObjectWithParent, IObjectWithID, IObjectWithNameAndDescription, IObjectWithPathAndChildSelector, IObjectWithUID, IEquatable<imbTreeNode>
    {
        private Dictionary<String, imbTreeNode> _items = new Dictionary<String, imbTreeNode>();

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public Dictionary<String, imbTreeNode> items
        {
            get
            {
                return _items;
            }
            protected set
            {
                _items = value;
                // OnPropertyChanged("items");
            }
        }

        public imbTreeNode()
        {
        }

        event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
        {
            add
            {
                // throw new NotImplementedException();
            }

            remove
            {
                //throw new NotImplementedException();
            }
        }

        #region --- UID ------- Univerzalni ID

        //private String _UID = "";
        /// <summary>
        /// Univerzalni ID
        /// </summary>
        public String UID
        {
            get
            {
                return keyHash;
            }
            set
            {
                //_UID = value;
                //OnPropertyChanged("UID");
            }
        }

        #endregion --- UID ------- Univerzalni ID

        #region VALUE - PATH - SOURCE

        #region -----------  sourceContent  -------  [String sadrzaj koji se odnosi na ovaj node - opcija]

        private String _sourceContent = ""; // = new String();

        /// <summary>
        /// String sadrzaj koji se odnosi na ovaj node - opcija
        /// </summary>
        // [XmlIgnore]
        [Category("imbTreeNode")]
        [DisplayName("sourceContent")]
        [Description("String sadrzaj koji se odnosi na ovaj node - opcija")]
        public String sourceContent
        {
            get { return _sourceContent; }
            set
            {
                // Boolean chg = (_sourceContent != value);
                _sourceContent = value;
                OnPropertyChanged("sourceContent");
                // if (chg) {}
            }
        }

        #endregion -----------  sourceContent  -------  [String sadrzaj koji se odnosi na ovaj node - opcija]

        #region -----------  sourcePath  -------  [Izvorna putanja koja je dodeljena prilikom kreiranja noda]

        protected String _sourcePath = "";

        // = new String();
        /// <summary>
        /// Izvorna putanja koja je dodeljena prilikom kreiranja noda
        /// </summary>
        // [XmlIgnore]
        [Category("imbTreeNode")]
        [DisplayName("sourcePath")]
        [Description("Izvorna putanja koja je dodeljena prilikom kreiranja noda")]
        public String sourcePath
        {
            get
            {
                return _sourcePath;
            }
            set
            {
                // Boolean chg = (_sourcePath != value);
                //_sourcePath = value;
                if (_sourcePath == "") { _sourcePath = value; }
                OnPropertyChanged("sourcePath");
                // if (chg) {}
            }
        }

        #endregion -----------  sourcePath  -------  [Izvorna putanja koja je dodeljena prilikom kreiranja noda]

        #region --- value ------- wrappovan objekat koji se nalazi na listu

        private Object _value;

        /// <summary>
        /// wrappovan objekat koji se nalazi na listu
        /// </summary>
        [XmlIgnore]
        [Category("imbTreeNode")]
        [DisplayName("value")]
        [Description("Vrednost dodeljena nodeu")]
        public virtual Object value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                OnPropertyChanged("value");
            }
        }

        #endregion --- value ------- wrappovan objekat koji se nalazi na listu

        #endregion VALUE - PATH - SOURCE

        /// <summary>
        /// Vraca referencu prema korenskom elementu
        /// </summary>
        public imbTree root
        {
            get
            {
                if (this is imbTree) return this as imbTree;

                if (parent is imbTree)
                {
                    return parent as imbTree;
                }
                if (parent == null)
                {
                    return null;
                }
                else
                {
                    return parent.root;
                }
            }
        }

        /// <summary>
        /// Makes the string report.
        /// </summary>
        /// <param name="isRoot">if set to <c>true</c> [is root].</param>
        /// <returns></returns>
        public List<String> makeStringReport(Boolean isRoot = false)
        {
            ///String output = "";

            List<String> pathList = new List<string>();
            pathList.Add(sourcePath);
            foreach (imbTreeNode nd in this.Values)
            {
                pathList.AddRange(nd.makeStringReport());
            }

            if (isRoot)
            {
                pathList.Sort();
            }
            return pathList;
        }

        /// <summary>
        /// Detects the parent child looper.
        /// </summary>
        /// <returns></returns>
        public imbTreeNode detectParentChildLooper()
        {
            List<IObjectWithPath> suspects = new List<IObjectWithPath>();

            foreach (imbTreeNode nd in Values)
            {
                suspects.Add(nd as IObjectWithPath);
            }

            IObjectWithPath looper = suspects.detectParentChildLoopReference();
            if (looper != null)
            {
                //String msg = "imbTreeNode - parent/child reference looper detected!";
                //msg += "imbTreeNode.name [" + name + "] --- path [" + path + "]";
                //logSystem.log(msg, logType.Warning);
            }

            return looper as imbTreeNode;
        }

        #region --- attributesToShow ------- List of attributes to show in XML conversion

        private static List<String> _attributesToShow;

        /// <summary>
        /// List of attributes to show in XML conversion
        /// </summary>
        public static List<String> attributesToShow
        {
            get
            {
                if (_attributesToShow == null)
                {
                    _attributesToShow = new List<String>();
                    _attributesToShow.Add("name");
                    _attributesToShow.Add("sourcePath");
                }
                return _attributesToShow;
            }
        }

        #endregion --- attributesToShow ------- List of attributes to show in XML conversion

        /// <summary>
        /// Makes the XML output using simple methods
        /// </summary>
        /// <param name="xmlReport">The XML report.</param>
        /// <param name="lineprefix">The lineprefix.</param>
        /// <returns></returns>
        public StringBuilder makeXmlSimple(StringBuilder xmlReport = null, String lineprefix = "")
        {
            if (xmlReport == null) xmlReport = new StringBuilder();

            String tng = this.nodeType().ToString(); // iTI.xmlTagNameForType;

            IObjectWithPath looper = detectParentChildLooper();

            if (looper != null)
            {
                xmlReport.AppendLine("WARNING: Loop child inherence at: " + this.path);

                return xmlReport;
            }

            String openLine = lineprefix + "<" + tng + " name=\"" + name + "\" sourcePath=\"" + sourcePath + "\" >";

            xmlReport.AppendLine(openLine);

            foreach (imbTreeNode nd in Values)
            {
                if (nd == this)
                {
                    xmlReport.AppendLine("WARNING: Loop child inherence at: " + this.path);
                }
                else
                {
                    nd.makeXmlSimple(xmlReport, lineprefix + "    ");
                }
            }

            if (!sourceContent.isNullOrEmptyString())
            {
                var lns = sourceContent.SplitSmart(Environment.NewLine);
                foreach (String ln in lns)
                {
                    xmlReport.AppendLine(lineprefix + ln);
                }

                xmlReport.AppendLine();
            }

            xmlReport.AppendLine(lineprefix + "</" + tng + ">");

            return xmlReport;
        }

        ///// <summary>
        ///// Pravi XML izvestaj o tree nodeu
        ///// </summary>
        ///// <param name="xmlReport"></param>
        ///// <returns></returns>
        //public reportXmlDocument makeXml(reportXmlDocument xmlReport = null)
        //{
        //    if (xmlReport == null)
        //        xmlReport = new reportXmlDocument("TreeNode report for: " + name, reportXmlFlag.insertXmlDeclaration,
        //                                          reportXmlFlag.escapeNodeContent);

        //    imbTypeInfo iTI = imbCore.resources.typology.imbTypology.getTypology(this); // this.getTypology();
        //    iTI.prepareForXmlEntity();

        //    String tng = this.nodeType().ToString(); // iTI.xmlTagNameForType;

        //    List<String> attribs = new List<string>();

        //    foreach (imbPropertyInfo kp in iTI.xmlAttributeProperty)
        //    {
        //        attribs.Add(kp.propertyRealName);
        //    }

        //    IObjectWithPath looper = detectParentChildLooper();

        //    if (looper != null)
        //    {
        //        xmlReport.AppendLine("WARNING: Loop child inherence at: " + this.path);

        //        return xmlReport;
        //    }

        //    String attLn = "";
        //    String vls = "";
        //    if (attribs.Any())
        //    {
        //        foreach (String kp in attribs)
        //        {
        //            vls = this.imbPropertyToString(kp, "");

        //            if (!vls.isNullOrEmptyString())
        //            {
        //                attLn = attLn.Append(kp + "=\"" + vls + "\"");
        //            }
        //        }

        //    }

        //    String _ntag = "<" + tng + "";

        //    _ntag = _ntag + " " + attLn; //.Append(attributes, " ");

        //        _ntag = _ntag.Append(">", " ");

        //    xmlReport.AppendLine(_ntag);

        //    foreach (imbTreeNode nd in Values)
        //    {
        //        if (nd == this)
        //        {
        //            xmlReport.AppendLine("WARNING: Loop child inherence at: " + this.path);

        //        }
        //        else
        //        {
        //            nd.makeXml(xmlReport);
        //        }
        //    }

        //    //if (!sourceContent.isNullOrEmptyString())
        //    //{
        //    //    xmlReport.AppendLine(sourceContent);
        //    //    xmlReport.AppendLine();
        //    //}

        //    xmlReport.AppendLine("</" + tng + ">");

        //    return xmlReport;
        //}

        #region IObjectWithIDandName Members

        /// <summary>
        /// Osnovni ID podatak
        /// </summary>
        public long id
        {
            get
            {
                long _id = -1;

                if (parent != null)
                {
                    if (parent != null)
                    {
                        if (parent.ContainsKey(this.name))
                        {
                            //__path = __path + ".items[" + parent.items.IndexOf(this) + "]";

                            _id = parent.imbGetIndexOf(this);
                        }
                    }
                }
                return _id;
            }
            set
            {
                // throw new NotImplementedException();
            }
        }

        #endregion IObjectWithIDandName Members

        #region IObjectWithParent Members

        Object IObjectWithParent.parent
        {
            get { return parent as Object; }
        }

        #endregion IObjectWithParent Members

        #region -----------  parent  -------  [Referenca prema parent objektu]

        private imbTreeNodeBranch _parent; // = new imbTreeNodeBranch();

        /// <summary>
        /// Referenca prema parent objektu
        /// </summary>
        // [XmlIgnore]
        [Category("imbTreeNode")]
        [DisplayName("parent")]
        [Description("Referenca prema parent objektu")]
        public imbTreeNodeBranch parent
        {
            get { return _parent; }
            set
            {
                // Boolean chg = (_parent != value);
                _parent = value;
                OnPropertyChanged("parent");
                //  OnPropertyChanged("name");
                OnPropertyChanged("id");
                // if (chg) {}
            }
        }

        #endregion -----------  parent  -------  [Referenca prema parent objektu]

        #region IObjectWithPath Members

        /// <summary>
        /// Putanja objekta
        /// </summary>
        public string path
        {
            get
            {
                if (_sourcePath.isNullOrEmpty())
                {
                    _sourcePath = this.getPathViaExtension(true);
                }
                return this.getPathViaExtension(true);
            }
        }

        #endregion IObjectWithPath Members

        /// <summary>
        /// Sklanja sve childrene koji nemaju childrene i nemaju sadrzaj
        /// </summary>
        public void removeEmptyChildren()
        {
            List<imbTreeNode> toRemove = new List<imbTreeNode>();
            foreach (imbTreeNode nd in this.Values)
            {
                if (nd.Count == 0)
                {
                    if (nd.sourceContent.Trim() == "")
                    {
                        toRemove.Add(nd);
                    }
                }
            }

            foreach (imbTreeNode nd in toRemove)
            {
                this.Remove(nd.keyHash);
            }

            foreach (imbTreeNode nd in this.Values)
            {
                nd.removeEmptyChildren();
            }
        }

        public void compressNode()
        {
            imbTreeNode ch = this;

            while (ch.Count == 1)
            {
                ch = ch.Values.imbFirstSafe();
            }

            if (ch != this)
            {
                replaceNode(ch);
            }
            else
            {
            }
        }

        public void replaceNode(imbTreeNode replacementNode)
        {
            var head = parent;
            if (parent != null)
            {
                head.Remove(this.keyHash);
                head.Add(replacementNode.keyHash, replacementNode);
            }
            else if (parent == null)
            {
            }
        }

        /// <summary>
        /// Detektuje vrstu nodea
        /// </summary>
        /// <returns></returns>
        public imbTreeNodeType nodeType()
        {
            if (type != imbTreeNodeType.unknown) return type;

            imbTreeNodeBranch this_imbTreeNodeBranch = (imbTreeNodeBranch)this;

            if (this_imbTreeNodeBranch.Count == 0)
            {
                return imbTreeNodeType.dry;
            }

            List<imbTreeNodeBranch> subBranches = this_imbTreeNodeBranch.allBranches();
            if (subBranches.Count == 0)
            {
                return imbTreeNodeType.end;
            }
            else
            {
#pragma warning disable CS0184 // The given expression is never of the provided ('imbTreeNodeLeaf') type
                if (this_imbTreeNodeBranch.Any(x => x is imbTreeNodeLeaf))
#pragma warning restore CS0184 // The given expression is never of the provided ('imbTreeNodeLeaf') type
                {
                    return imbTreeNodeType.leafed;
                }
            }

            if (this_imbTreeNodeBranch.parent.Count == 1)
            {
                if (this_imbTreeNodeBranch.Count == 1)
                {
                    if (this_imbTreeNodeBranch.First().Value is imbTreeNodeBranch)
                    {
                        return imbTreeNodeType.main;
                    }
                }
                else
                {
#pragma warning disable CS0184 // The given expression is never of the provided ('imbTreeNodeBranch') type
                    if (this_imbTreeNodeBranch.All(x => x is imbTreeNodeBranch))
#pragma warning restore CS0184 // The given expression is never of the provided ('imbTreeNodeBranch') type
                    {
                        return imbTreeNodeType.lateralFirst;
                    }
                }
            }

            foreach (imbTreeNodeBranch subs in subBranches)
            {
#pragma warning disable CS0184 // The given expression is never of the provided ('imbTreeNodeLeaf') type
                if (subs.Any(x => x is imbTreeNodeLeaf))
#pragma warning restore CS0184 // The given expression is never of the provided ('imbTreeNodeLeaf') type
                {
                    return imbTreeNodeType.lateralLast;
                }
            }

            return imbTreeNodeType.lateral;
        }

        //private CollectionChangedEventManager _changedEvent;
        ///// <summary>
        /////
        ///// </summary>
        //public CollectionChangedEventManager changedEvent
        //{
        //    get { return _changedEvent; }
        //    set { _changedEvent = value; }
        //}

        //public static Int32 tCount = 0;

        /// <summary>
        /// Postavlja osnovna podesavanja za node
        /// </summary>
        protected void _init()
        {
            //changedEvent += imbTreeNode_CollectionChanged;

            //r(imbTreeNode_CollectionChanged;
            //UID = imbStringGenerators.getRandomString(32);
        }

        private void imbTreeNode_CollectionChanged(object sender,
                                                   System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (this is imbTreeNodeBranch)
            {
                imbTreeNodeBranch parentBranch = this as imbTreeNodeBranch;

                switch (e.Action)
                {
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                        foreach (imbTreeNode item in e.NewItems)
                        {
                            item.parent = parentBranch;
                        }
                        break;

                    case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                        break;

                    case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                        foreach (imbTreeNode item in e.OldItems)
                        {
                            item.parent = null;
                        }
                        break;

                    case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                        foreach (imbTreeNode item in e.NewItems)
                        {
                            item.parent = parentBranch;
                        }
                        foreach (imbTreeNode item in e.OldItems)
                        {
                            item.parent = null;
                        }
                        break;

                    case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                        break;
                }
            }
            else
            {
                Exception ex = new ArgumentOutOfRangeException("imnCollectionMeta operations are forbiden to non branch tree object");
                throw ex;
            }
        }

        // public abstract Object value { get; set; }

        /// <summary>
        /// Preuzima podatke od prosledjenog nodea
        /// </summary>
        /// <param name="nodeToLearnFrom"></param>
        protected void learnFrom(imbTreeNode nodeToLearnFrom)
        {
            value = nodeToLearnFrom.value;
            sourcePath = nodeToLearnFrom.sourcePath;
            sourceContent = nodeToLearnFrom.sourceContent;
        }

        public void Add(imbTreeNode child)
        {
            if (!(this is imbTreeNodeBranch))
            {
                Exception ex = new ArgumentException("Can't add new branch to imbTreeNodeLeaf object", nameof(child));

                throw ex;
                return;
            }

            Add("", child);
        }

        public pathResolverResult resolvePath(String path, pathResolveFlag flags)
        {
            pathResolverResult output = this.resolvePathSegments(path, flags);
            return output;
        }

        public override string ToString()
        {
            return path;
        }

        IEnumerator IObjectWithChildSelector.GetEnumerator() => this.GetEnumerator();

        int IObjectWithChildSelector.indexOf(IObjectWithChildSelector child) => this.Values.imbGetIndexOf(child);

        int IObjectWithChildSelector.Count() => this.Count();

        public bool ContainsKey(string key)
        {
            return ((IDictionary<string, imbTreeNode>)items).ContainsKey(key);
        }

        public void Add(KeyValuePair<string, imbTreeNode> item)
        {
            Add(item.Key, item.Value);
            //((IDictionary<string, imbTreeNode>)items).Add(item);
        }

        private Int32 _lastNamingIteration = 0;

        /// <summary> </summary>
        public Int32 lastNamingIteration
        {
            get
            {
                return _lastNamingIteration;
            }
            protected set
            {
                _lastNamingIteration = value;
                OnPropertyChanged("lastNamingIteration");
            }
        }

        public void Add(String notApplied, imbTreeNode value)
        {
            // String sp = value.sourcePath;
            if (ContainsKey(value.keyHash))
            {
                // Remove(value.keyHash);

                value._keyHash = items.makeUniqueDictionaryName(value.keyHash, ref _lastNamingIteration, 2000);
            }
            else
            {
                // value.parent = this as imbTreeNodeBranch;
                //    ((IDictionary<string, imbTreeNode>)items).Add(value.keyHash, value);
            }

            value.parent = this as imbTreeNodeBranch;
            ((IDictionary<string, imbTreeNode>)items).Add(value.keyHash, value);
        }

        public String _keyHash = "";

        public String keyHash
        {
            get
            {
                if (_keyHash.isNullOrEmpty())
                {
                    _keyHash = md5.GetMd5Hash(sourcePath);
                }
                return _keyHash; //
            }
        }

        public bool Remove(string key)
        {
            return ((IDictionary<string, imbTreeNode>)items).Remove(key);
        }

        public bool TryGetValue(string key, out imbTreeNode value)
        {
            return ((IDictionary<string, imbTreeNode>)items).TryGetValue(key, out value);
        }

        public void Clear()
        {
            ((IDictionary<string, imbTreeNode>)items).Clear();
        }

        public bool Contains(KeyValuePair<string, imbTreeNode> item)
        {
            return ((IDictionary<string, imbTreeNode>)items).Contains(item);
        }

        public void CopyTo(KeyValuePair<string, imbTreeNode>[] array, int arrayIndex)
        {
            ((IDictionary<string, imbTreeNode>)items).CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, imbTreeNode> item)
        {
            return ((IDictionary<string, imbTreeNode>)items).Remove(item);
        }

        public IEnumerator<KeyValuePair<string, imbTreeNode>> GetEnumerator()
        {
            return ((IDictionary<string, imbTreeNode>)items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IDictionary<string, imbTreeNode>)items).GetEnumerator();
        }

        public bool Equals(imbTreeNode other)
        {
            return (this.path == other.path);
        }

        #region --- _nameBase ------- skracenica imena

        private String __nameBase = "";

        /// <summary>
        /// osnovno ime grane
        /// </summary>
        protected String _nameBase
        {
            get
            {
                if (String.IsNullOrEmpty(__nameBase))
                {
                    __nameBase = GetType().Name.imbGetAbbrevation(3, true);
                }
                return __nameBase;
            }
            set
            {
                __nameBase = value;
                OnPropertyChanged("name");
            }
        }

        #endregion --- _nameBase ------- skracenica imena

        #region -----------  name  -------  [ime objekta - ne jedinstveno]

        private String _name = "treeNode"; // = new String();

        /// <summary>
        /// ime objekta koje sadrzi broj pozicije na kojoj se trenutno nalazi - ime tipa i ID - ako je objekat deo glavnog niza
        /// </summary>
        // [XmlIgnore]
        [Category("contentElementBase")]
        [DisplayName("name")]
        [Description("ime objekta - ne jedinstveno")]
        public String name
        {
            get
            {
                _name = _nameBase;

                //long _id = id;

                //if (_id > -1)
                //{
                //    _name += "[" + _id.ToString() + "]";
                //}

                return _name;
            }
            set
            {
                Exception ex = new ExecutionEngineException("Can't change name of a tree node with direct access");
                throw ex;
                //var isb = new imbStringBuilder(0);
                //isb.AppendLine("imbTreeNode error");
                //isb.AppendPair("Target is: ", this.toStringSafe());
                //isb.AppendPair("Path is: ", path.toStringSafe());
                //devNoteManager.note(this, ex, isb.ToString(), "imbTreeNode", devNoteType.notIntendentUsage);
            }
        }

        #endregion -----------  name  -------  [ime objekta - ne jedinstveno]

        #region --- type ------- koji je node type poslednji put detektovan

        private imbTreeNodeType _type = imbTreeNodeType.unknown;

        /// <summary>
        /// koji je node type poslednji put detektovan
        /// </summary>
        public virtual imbTreeNodeType type
        {
            get
            {
                // prebacujemo na pasivnu dodelu tipa
                // if (_type == imbTreeNodeType.unknown) _type = nodeType();
                return _type;
            }
            set
            {
                _type = value;
                OnPropertyChanged("type");
            }
        }

        #endregion --- type ------- koji je node type poslednji put detektovan

        private String _description = "";
        private object logSystem;

        /// <summary>
        ///
        /// </summary>
        public String description
        {
            get { return _description; }
            set { _description = value; }
        }

        object IObjectWithRoot.root => root;

        public ICollection<string> Keys
        {
            get
            {
                return ((IDictionary<string, imbTreeNode>)items).Keys;
            }
        }

        public ICollection<imbTreeNode> Values
        {
            get
            {
                return ((IDictionary<string, imbTreeNode>)items).Values;
            }
        }

        public int Count
        {
            get
            {
                return ((IDictionary<string, imbTreeNode>)items).Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return ((IDictionary<string, imbTreeNode>)items).IsReadOnly;
            }
        }

        public imbTreeNode this[string key]
        {
            get
            {
                return ((IDictionary<string, imbTreeNode>)items)[key];
            }

            set
            {
                ((IDictionary<string, imbTreeNode>)items)[key] = value;
            }
        }

        object IObjectWithChildSelector.this[string childName] => this[childName];

        object IObjectWithChildSelector.this[int key] => items.imbGetItemAt(key);

        /*
        public void Add()
        {
            if (this is imbTreeNodeLeaf)
            {
            }
        }
        */
    }
}