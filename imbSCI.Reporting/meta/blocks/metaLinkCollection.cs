// --------------------------------------------------------------------------------------------------------------------
// <copyright file="metaLinkCollection.cs" company="imbVeles" >
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
namespace imbSCI.Reporting.meta.blocks
{
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.interfaces;
    using imbSCI.Core.reporting.colors;
    using imbSCI.Data;
    using imbSCI.Data.enums.appends;
    using imbSCI.Data.interfaces;
    using imbSCI.Reporting.meta.collection;
    using imbSCI.Reporting.meta.page;
    using imbSCI.Reporting.script;
    using System.Collections;
    using System.ComponentModel;
    using System.Linq;

    /// <summary>
    /// Meta representation for group of links
    /// </summary>
    public class metaLinkCollection : MetaContainerNestedBase, IObjectWithDescription
    {
        public override void construct(object[] resources)
        {
            colors = acePaletteRole.colorA;
            width = blockWidth.full;
        }

        public override docScript compose(docScript script)
        {
            if (script == null) script = new docScript(name);
            script.x_scopeIn(this);

            foreach (metaLink link in links)
            {
                link.compose(script);
            }

            //  script.add(appendType.s_palette).arg(colors);

            //script.add(appendType.c_section, docScriptArguments.dsa_name, docScriptArguments.dsa_content, docScriptArguments.dsa_description, docScriptArguments.dsa_class_attribute, docScriptArguments.dsa_id_attribute).set(name, content, description, "header", "#" + name);

            //script.add(appendType.s_palette).arg(acePaletteRole.colorDefault);

            script.x_scopeOut(this);
            return script;
        }

        #region --- description ------- Description line about the links

        private string _description = "";

        /// <summary>
        /// Description line about the links
        /// </summary>
        public string description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                OnPropertyChanged("description");
            }
        }

        #endregion --- description ------- Description line about the links

        #region -----------  links  -------  [kolekcija linkova]

        /// <summary>
        /// kolekcija linkova
        /// </summary>
        // [XmlIgnore]
        [Category("metaLinkCollection")]
        [DisplayName("links")]
        [Description("kolekcija linkova")]
        internal metaCollection<metaLink> links { get; set; } = new metaCollection<metaLink>();

        #endregion -----------  links  -------  [kolekcija linkova]

        /// <summary>
        /// Adding internal link
        /// </summary>
        /// <param name="__target"></param>
        /// <param name="__parent"></param>
        /// <returns></returns>
        public metaLink AddLink(IMetaContentNested __target, string __description = "", int __priority = 100)
        {
            metaLink link = new metaLink();
            link.target = __target;
            link.parent = parent;
            link.priority = __priority;
            link.description = __description;

            if (__target is metaPage)
            {
                metaPage targetPage = __target as metaPage;
                link.title = targetPage.header.name;
                if (link.description.isNullOrEmptyString())
                {
                    link.description = targetPage.header.description;
                }
                //link.description = targetPage.
            }

            link.name = link.target.name.imbTitleCamelOperation(true);
            link.type = appendLinkType.link;

            links.Add(link);
            return link;
        }

        /// <summary>
        /// Adding external link
        /// </summary>
        /// <param name="__name"></param>
        /// <param name="__description"></param>
        /// <param name="__url"></param>
        /// <returns></returns>
        public metaLink AddLink(string __name, string __description, string __url, int __priority = 100)
        {
            metaLink link = new metaLink();
            link.type = appendLinkType.link;
            link.name = __name;
            link.url = __url;
            link.description = __description;
            link.parent = parent;
            link.priority = __priority;
            links.Add(link, parent);
            return link;
        }

        /// <summary>
        /// Adding on page anchor
        /// </summary>
        /// <param name="__name"></param>
        /// <param name="__description"></param>
        /// <returns></returns>
        public metaLink AddAnchor(string __name, string __description, int __priority = 100)
        {
            metaLink link = new metaLink();
            link.type = appendLinkType.anchor;
            link.name = __name;
            link.url = __name;
            link.description = __description;
            link.name = link.target.name.imbTitleCamelOperation(true);
            link.priority = __priority;
            links.Add(link, parent);

            return link;
        }

        /// <summary>
        /// Place of link inside the collection
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        public int indexOf(IMetaContentNested child)
        {
            return links.IndexOf(child as metaLink);
        }

        /// <summary>
        /// Number of links
        /// </summary>
        /// <returns></returns>
        public override int Count()
        {
            return links.Count();
        }

        /// <summary>
        /// Shorts all links by priority
        /// </summary>
        public override void sortChildren()
        {
            links.Sort();
        }

        #region -----------  type  -------  [type of this link collection]

        /// <summary>
        /// type of this link collection
        /// </summary>
        // [XmlIgnore]
        [Category("metaLinkCollection")]
        [DisplayName("type")]
        [Description("type of this link collection")]
        public metaLinkCollectionType type { get; set; } = metaLinkCollectionType.unknown;

        #endregion -----------  type  -------  [type of this link collection]

        /// <summary>
        /// Path from which this link collection point its links
        /// </summary>
        // [XmlIgnore]
        [Category("metaLinkCollection")]
        [DisplayName("rootRelativePath")]
        [Description("Path from which this link collection point its links")]
        public string rootRelativePath { get; set; }

        //public override int Count
        //{
        //    get
        //    {
        //        return items.Count;
        //    }
        //}

        ///// <summary>
        ///// Gets the <see cref="System.Object"/> with the specified name.
        ///// </summary>
        ///// <value>
        ///// The <see cref="System.Object"/>.
        ///// </value>
        ///// <param name="name">The name.</param>
        ///// <returns></returns>
        //public override object this[string name]
        //{
        //    get
        //    {
        //        return links.First(x => x.name == name);
        //    }
        //}

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public override IEnumerator GetEnumerator()
        {
            return links.GetEnumerator();
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
                return links[key];
            }
        }
    }
}