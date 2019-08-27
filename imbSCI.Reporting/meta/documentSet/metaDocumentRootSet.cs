// --------------------------------------------------------------------------------------------------------------------
// <copyright file="metaDocumentRootSet.cs" company="imbVeles" >
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
    using imbSCI.Core.interfaces;
    using imbSCI.Data;
    using imbSCI.Data.collection.nested;
    using imbSCI.Data.enums;
    using imbSCI.Data.interfaces;
    using imbSCI.Reporting.meta.page;
    using imbSCI.Reporting.script;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Root set of the report - adds index.html outside its own folder
    /// </summary>
    /// <seealso cref="imbSCI.Reporting.meta.documentSet.metaDocumentSet" />
    public abstract class metaDocumentRootSet : metaDocumentSet, IObjectWithRoot
    {
        object IObjectWithRoot.root
        {
            get
            {
                return this;
            }
        }

        public override metaPage indexPage
        {
            get
            {
                return pageRootIndex;
            }
        }

        /// <summary>
        /// Regs the path get.
        /// </summary>
        /// <typeparam name="TReg">The type of the reg.</typeparam>
        /// <param name="__path">The path in format $$$reportElementLevel:\\analyticReport  .</param>
        /// <returns></returns>
        public TReg regPathGet<TReg>(string __path) where TReg : class, IMetaContentNested
        {
            if (__path.Contains(":"))
            {
                var prt = __path.SplitSmart(":");
            }
            reportElementLevel level = typeof(TReg).GetElementLevel();

            if (regPathCache[level].ContainsKey(__path))
            {
                return regPathCache[level][__path] as TReg;
            }
            if (logger != null) logger.log("regPathGet with level[" + level.ToString() + "] failed on [" + __path + "]");
            return this as TReg;
        }

        public static Regex ELEMENTPATH_PATH = new Regex(@"\:([\w\\]+)\$\$\$");
        public static Regex ELEMENTPATH_ELEMENT = new Regex(@"\$\$\$([\w\\]+)\:");

        public IMetaContentNested regPathGet(string __elementPath)
        {
            string __path = "";
            reportElementLevel level = __elementPath.GetReportElementPathAndLevel(out __path);

            if (regPathCache[level].ContainsKey(__path))
            {
                IMetaContentNested result = regPathCache[level][__path] as IMetaContentNested;
                return result;
            }
            if (logger != null) logger.log("regPathGet with level[" + level.ToString() + "] failed on [" + __path + "]");
            return this;
        }

        /// <summary>
        /// Regs the path get.
        /// </summary>
        /// <param name="__path">The path.</param>
        /// <param name="level">The level.</param>
        /// <returns></returns>
        public IMetaContentNested regPathGet(string __path, reportElementLevel level)
        {
            if (regPathCache[level].ContainsKey(__path))
            {
                return regPathCache[level][__path] as IMetaContentNested;
            }
            if (logger != null) logger.log("regPathGet with level[" + level.ToString() + "] failed on [" + __path + "]");
            return this;
        }

#pragma warning disable CS1574 // XML comment has cref attribute 'aceReportException' that could not be resolved
        /// <summary>
        /// Regs the path.
        /// </summary>
        /// <param name="metaItem">The meta item.</param>
        /// <param name="theParentRegMoment">if set to <c>true</c> [the parent reg moment].</param>
        /// <exception cref="aceReportException">
        /// null - null - regPath issue
        /// or
        /// null - null - regPath issue
        /// </exception>
        public void regPath(IMetaContentNested metaItem, bool theParentRegMoment)
#pragma warning restore CS1574 // XML comment has cref attribute 'aceReportException' that could not be resolved
        {
            reportElementLevel level = metaItem.elementLevel;
            if (regPathCache[level].ContainsKey(metaItem.path))
            {
                if (regPathCache[level].ContainsValue(metaItem))
                {
                    if (regPathCache[level][metaItem.path] == metaItem)
                    {
                    }
                    else
                    {
                        metaItem.name = metaItem.name + "sub";
                        regPath(metaItem, theParentRegMoment);
                        // throw new aceReportException(metaItem.path + " is reserved by other element! " + metaItem.name, null, null, "regPath issue");
                    }
                }
                else
                {
                    metaItem.name = metaItem.name + "in";
                    regPath(metaItem, theParentRegMoment);
                }
            }
            else
            {
                regPathCache[level][metaItem.path] = metaItem;
            }
        }

        private aceEnumDictionarySet<reportElementLevel, string, IMetaContentNested> _regPathCache = new aceEnumDictionarySet<reportElementLevel, string, IMetaContentNested>();

        /// <summary>
        /// Gets or sets the reg path cache.
        /// </summary>
        /// <value>
        /// The reg path cache.
        /// </value>
        protected aceEnumDictionarySet<reportElementLevel, string, IMetaContentNested> regPathCache
        {
            get
            {
                return _regPathCache;
            }
            set
            {
                _regPathCache = value;
                OnPropertyChanged("regPathCache");
            }
        }

        /// <summary>
        /// Gets or sets the path string cache.
        /// </summary>
        /// <value>
        /// The path string cache.
        /// </value>
        protected Dictionary<IMetaContentNested, string> pathStrCache { get; set; } = new Dictionary<IMetaContentNested, string>();

        /// <summary>
        /// Gets or sets the path cache.
        /// </summary>
        /// <value>
        /// The path cache.
        /// </value>
        protected Dictionary<string, IMetaContentNested> pathCache { get; set; } = new Dictionary<string, IMetaContentNested>();

        /// <summary>
        /// Gets or sets the index of the page root.
        /// </summary>
        /// <value>
        /// The index of the page root.
        /// </value>
        public metaCustomizedSimplePage pageRootIndex { get; protected set; }

        public override void baseConstruct(object[] resources)
        {
            pageRootIndex.construct(resources);
            pageRootIndex.parent = this;
            base.baseConstruct(resources);
        }

        /// <summary>
        /// The compose call with root index inclusion out of report folder
        /// </summary>
        /// <param name="script">The script.</param>
        /// <returns></returns>
        public override docScript compose(docScript script)
        {
            if (script == null) script = new docScript(name);

            pageRootIndex.compose(script);

            script.x_scopeIn(this);

            baseCompose(script);

            script.x_scopeOut(this);
            return script;
        }

        /// <summary>
        /// Collects data trough this meta node and its children
        /// </summary>
        /// <param name="data">PropertyCollectionDictionary to fill in</param>
        /// <returns>
        /// New or updated Dictionary
        /// </returns>
        public override PropertyCollectionDictionary collect(PropertyCollectionDictionary data = null)
        {
            data = base.collect(data);

            return data;
        }
    }
}