// --------------------------------------------------------------------------------------------------------------------
// <copyright file="graphDataPackage.cs" company="imbVeles" >
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
namespace imbSCI.Data.collection.graph
{
    using imbSCI.Data.data.package;
    using imbSCI.Data.interfaces;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// XML Serializable container for <see cref="graphWrapNode{TItem}" /> structure. Use <see cref="Store(graphWrapNode{TItem}, string)" /> to add node into package.
    /// </summary>
    /// <typeparam name="TItem">The type of the item.</typeparam>
    /// <seealso cref="imbSCI.Data.data.package.dataPackage{TItem, imbSCI.Data.collection.graph.graphWrapNode{TItem}}" />
    /// <seealso cref="imbSCI.Data.data.package.dataPackage{TItem, aceCommonTypes.collection.nested.graphWrapNode{TItem}}" />
    public class graphDataPackage<TItem> : dataPackage<TItem, graphWrapNode<TItem>> where TItem : class, IObjectWithName, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="graphDataPackage{TItem}"/> class.
        /// </summary>
        public graphDataPackage() : base()
        {
        }

        /// <summary>
        /// Stores the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="path">The path.</param>
        public void Store(graphWrapNode<TItem> input, String path)
        {
            List<graphWrapNode<TItem>> chld = new List<graphWrapNode<TItem>>();
            input.getAllChildren().ForEach(x => chld.Add((graphWrapNode<TItem>)x));

            AddDataItems(chld, true);

            this.SaveDataPackage(path);
        }

        /// <summary>
        /// Utility method that returns Unique path to the specified node
        /// </summary>
        /// <param name="wrapper">Object that holds the instance</param>
        /// <returns>
        /// ID
        /// </returns>
        protected override string GetDataPackageID(graphWrapNode<TItem> wrapper)
        {
            return wrapper.path;
        }

        /// <summary>
        /// Takes data item instance from the wrapper
        /// </summary>
        /// <param name="wrapper">Object that holds the instance</param>
        /// <returns>
        /// item that was held by the instance
        /// </returns>
        protected override TItem GetInstanceToPack(graphWrapNode<TItem> wrapper)
        {
            return wrapper.item;
        }
    }
}