// --------------------------------------------------------------------------------------------------------------------
// <copyright file="graphWrapNode.cs" company="imbVeles" >
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
// Project: imbSCI.Data
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------

//using imbSCI.Core.interfaces;
//using aceCommonTypes.extensions.io;
//using aceCommonTypes.extensions.text;
//using aceCommonTypes.interfaces;

namespace imbSCI.Data.collection.graph
{
    using imbSCI.Data.interfaces;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <summary>
    /// Graph structure that wrapps custom object specified by {TItem}
    /// </summary>
    /// <typeparam name="TItem">The type of the item.</typeparam>
    /// <seealso cref="System.Collections.Generic.IEnumerable{aceCommonTypes.collection.nested.graphWrapNode{TItem}}" />
    /// <seealso cref="imbSCI.Core.interfaces.IObjectWithParent" />
    /// <seealso cref="imbSCI.Core.interfaces.IObjectWithPath" />
    /// <seealso cref="aceCommonTypes.interfaces.IObjectWithName" />
    /// <seealso cref="aceCommonTypes.interfaces.IObjectWithPathAndChildren" />
    [Serializable]
    public class graphWrapNode<TItem> : graphNodeBase, IGraphNode, IEnumerable, IEnumerable<graphWrapNode<TItem>>,
        IObjectWithParent, IObjectWithPath, IObjectWithName, IObjectWithPathAndChildren, IObjectWithTreeView where TItem : IObjectWithName
    {
        //public void OnBeforeSave()

        IEnumerator<graphWrapNode<TItem>> IEnumerable<graphWrapNode<TItem>>.GetEnumerator()
        {
            return mychildren.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return children.Values.GetEnumerator();
        }

        public IEnumerator<IObjectWithPathAndChildren> GetEnumerator()
        {
            return mychildren.Values.GetEnumerator();
        }

        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        /// <value>
        /// The children.
        /// </value>
        protected override IDictionary children
        {
            get
            {
                return mychildren;
            }
        }

        protected SortedList<String, graphWrapNode<TItem>> mychildren { get; set; } = new SortedList<string, graphWrapNode<TItem>>();

        //private ConcurrentDictionary<String, graphWrapNode<TItem>> _children = new ConcurrentDictionary<String, graphWrapNode<TItem>>();
        ///// <summary>
        ///// Gets or sets the children.
        ///// </summary>
        ///// <value>
        ///// The children.
        ///// </value>
        //protected ConcurrentDictionary<String, graphWrapNode<TItem>> mychildren
        //{
        //    get
        //    {
        //        return _children;
        //    }
        //    set { _children = value; }
        //}

        private TItem _item; // = "";

        /// <summary>
        /// The object associated with the graph
        /// </summary>
        [XmlIgnore]
        public TItem item
        {
            get { return _item; }
            protected set { _item = value; }
        }

        public void SetItem(TItem __item)
        {
            String __name = name;
            __item.name = __name;
            _item = __item;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is empty graph node, containing no wrapped item node.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is null node; otherwise, <c>false</c>.
        /// </value>
        public Boolean isNullNode
        {
            get
            {
                return (item == null);
            }
        }

        public graphWrapNode()
        {
        }

        protected graphWrapNode(String __name, graphWrapNode<TItem> __parent = null)
        {
            name = __name;
            parent = __parent;
        }

        protected graphWrapNode(TItem __item, graphWrapNode<TItem> __parent = null)
        {
            _item = __item;
            parent = __parent;
        }

        /// <summary>
        /// Gets or sets the <see cref="graphWrapNode{TItem}"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="graphWrapNode{TItem}"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public override IGraphNode this[String key]
        {
            get
            {
                return mychildren[key];
            }
            set
            {
                if (mychildren.ContainsKey(key))
                {
                    mychildren[key].item = value as graphWrapNode<TItem>;
                }
                else
                {
                    Add(value);
                }
            }
        }

        /// <summary>
        /// Ime koje je dodeljeno objektu
        /// </summary>
        public override string name
        {
            get
            {
                if (isNullNode)
                {
                    return base.name;
                }
                return item.name;
            }

            set
            {
                base.name = value;
            }
        }

        private graphWrapNode<TItem> _parent;

        /// <summary>
        /// Referenca prema parent objektu
        /// </summary>
        public override IGraphNode parent
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = (graphWrapNode<TItem>)value;
            }
        }

        public static implicit operator TItem(graphWrapNode<TItem> gr)
        {
            return gr.item;
        }

        /// <summary>
        /// Adds the specified item into graph structure
        /// </summary>
        /// <param name="__item">The item.</param>
        public virtual graphWrapNode<TItem> Add(TItem __item)
        {
            if (!mychildren.ContainsKey(__item.name))
            {
                var tkng = new graphWrapNode<TItem>(__item, this);

                children.Add(__item.name, tkng);
                return tkng;
            }
            return this[__item.name] as graphWrapNode<TItem>;
        }

        /// <summary>
        /// Adds the specified name.
        /// </summary>
        /// <param name="__name">The name.</param>
        /// <returns></returns>
        public virtual graphWrapNode<TItem> Add(String __name)
        {
            if (!mychildren.ContainsKey(__name))
            {
                var tkng = new graphWrapNode<TItem>(__name, this);
                children.Add(__name, tkng);
                tkng.parent = this;
                return tkng;
            }
            return this[__name] as graphWrapNode<TItem>;
        }

        /// <summary>
        /// Adds new node or nodes to correspond to specified path or name. <c>pathOrName</c> can be path like: path1\\path2\\path3
        /// </summary>
        /// <param name="pathWithName">Name of the path with.</param>
        /// <param name="__item">The item.</param>
        /// <returns></returns>
        public virtual graphWrapNode<TItem> Add(String pathWithName, TItem __item)
        {
            List<String> pathParts = pathWithName.SplitSmart(pathSeparator);
            graphWrapNode<TItem> head = this;

            foreach (String part in pathParts)
            {
                head = head.Add(part);
            }

            head.SetItem(__item);

            return head;
        }

        /// <summary>
        /// Adds the specified <c>newChild</c>, if its name is not already occupied
        /// </summary>
        /// <param name="newChild">The new child.</param>
        /// <returns></returns>
        public override bool Add(IGraphNode newChild)
        {
            if (children.Contains(newChild.name))
            {
                return false;
            }
            else
            {
                IGraphNode gn = newChild as IGraphNode;
                gn.parent = this;
                children.Add(gn.name, gn);
                return true;
            }
        }

        public const String SEPARATOR = "--> ";
        public const String SEPARATOR_CHILD = "|-> ";

        /// <summary>
        /// To the string path list.
        /// </summary>
        /// <returns></returns>
        public String ToStringPathList()
        {
            String output = "";
            var allLeafs = this.getAllLeafs();

            foreach (IObjectWithPathAndChildren pair in allLeafs)
            {
                output = output + pair.path + Environment.NewLine;
            }
            return output;
        }

        public virtual String forTreeview
        {
            get
            {
                return name;
            }
        }

        // public override IGraphNode this[string key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /*
        public String ToStringTreeview(String prefix = "", Boolean showType = false, Int32 gen = 0)
        {
            String output = forTreeview; // + "[l:" + level.ToString("D2") + "]";

            //if (showType) output += ":" + type.ToString();

            //Int32 l = 0;
            //foreach (var pair in children)
            //{
            //    l = Math.Max(pair.Value.forTreeview.Length, l);
            //}
            //l = l + 2;

            //output = output.PadRight(l);

            output = prefix + output;

            Boolean firstChild = true;
            //Int32 pad = Math.Max(output.Length, 20);

            String cPrefix = new String(' ', output.Length);

           // var thelast = ;

            foreach (var pair in children)
            {
                if (firstChild)
                {
                    String ch = pair.Value.ToStringTreeview(cPrefix + SEPARATOR, showType, gen + 1);
                    output = output + ch.removeStartsWith(cPrefix);
                    firstChild = false;
                }
                else
                {
                    output = output + Environment.NewLine;
                    output = output + pair.Value.ToStringTreeview(cPrefix + SEPARATOR_CHILD, showType, gen + 1);
                    if (pair.Value == children.Last().Value)
                    {
                        output = output + Environment.NewLine;
                    }
                }
            }
            return output;
        }*/

        /*
       /// <summary>
       /// Returns string representation of the graph
       /// </summary>
       /// <param name="prefix">The prefix.</param>
       /// <param name="showType">if set to <c>true</c> [show type].</param>
       /// <param name="gen">The gen.</param>
       /// <returns></returns>
       public String ToStringTreeviewAlt(String prefix = "", Boolean showType = false, Int32 gen = 0)
       {
           String output = "";

           List <graphWrapNode<TItem>> output = new List<graphWrapNode<TItem>>();
           List<graphWrapNode<TItem>> stack = new List<graphWrapNode<TItem>>();
           stack.Add(this);

           List<String> lines = new List<string>();

           while (stack.Any())
           {
               var n_stack = new List<graphWrapNode<TItem>>();

               Int32 l = 0;
               foreach (var pair in stack)
               {
                   l = Math.Max(pair.forTreeview.Length, l);
               }
               l = l + 2;

               foreach (graphWrapNode<TItem> child in stack)
               {
                   String oput = child.forTreeview;
                   oput = oput.PadRight(l);
                   if ()
                   {
                       output.Add(child);
                   }

                   stack.AddRange(child);
               }

               stack = n_stack;
           }

           //String  // + "[l:" + level.ToString("D2") + "]";

           //if (showType) output += ":" + type.ToString();

           output = prefix + output;

           Boolean firstChild = true;
           //Int32 pad = Math.Max(output.Length, 20);

           String cPrefix = new String(' ', output.Length);

           foreach (var pair in children)
           {
               if (firstChild)
               {
                   String ch = pair.Value.ToStringTreeview(cPrefix + SEPARATOR, showType, gen + 1);
                   output = output + ch.removeStartsWith(cPrefix);
                   firstChild = false;
               }
               else
               {
                   output = output + Environment.NewLine;
                   output = output + pair.Value.ToStringTreeview(cPrefix + SEPARATOR_CHILD, showType, gen + 1);
               }
           }

           return output;
       }
       */
    }
}