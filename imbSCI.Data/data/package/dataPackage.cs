// --------------------------------------------------------------------------------------------------------------------
// <copyright file="dataPackage.cs" company="imbVeles" >
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
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace imbSCI.Data.data.package
{
    using imbSCI.Data.collection.nested;

    /// <summary>
    /// Serializable data structure package container - base class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class dataPackage<T, TWrapper> : dataPackageBase where TWrapper : class
        where T : class, new()
    {
        protected dataPackage()
        {
            xSer = new XmlSerializer(typeof(T));
        }

        //public static TPackage LoadDataPackage<TPackage>(String path) where TPackage: dataPackage<T, TWrapper>
        //{
        //}

        private aceConcurrentBag<dataItemContainer> _bagContent;

        /// <summary>
        /// Concurrent version of the <see cref="content"/>
        /// </summary>
        /// <value>
        /// The content of the bag.
        /// </value>
        private aceConcurrentBag<dataItemContainer> bagContent
        {
            get
            {
                if (_bagContent == null)
                {
                    _bagContent = new aceConcurrentBag<dataItemContainer>();
                    content.ForEach(x => _bagContent.Add(x));
                }
                return _bagContent;
            }
        }

        private XmlSerializer xSer;

        /// <summary>
        /// Makes or takes (unique or not) ID to assign to the item instance wrapped
        /// </summary>
        /// <param name="wrapper">Object that holds the instance</param>
        /// <returns>ID</returns>
        protected abstract String GetDataPackageID(TWrapper wrapper);

        /// <summary>
        /// Takes data item instance from the wrapper
        /// </summary>
        /// <param name="wrapper">Object that holds the instance</param>
        /// <returns>item that was held by the instance</returns>
        protected abstract T GetInstanceToPack(TWrapper wrapper);

        /// <summary>
        /// Serializes and stores an item into <see cref="content"/> collection. , before serialization it calls <see cref="IDataPackageItem.OnBeforeSave"/>
        /// </summary>
        /// <param name="wrapper">The wrapper to use for <see cref="dataItemContainer.id"/> and <see cref="dataItemContainer.instanceXml"/> creation</param>
        /// <param name="xmlSer">The XML serializer to use instead of its own.</param>
        /// <remarks>Makes the container using <see cref="GetInstanceToPack(TWrapper)"/> and <see cref="GetDataPackageID(TWrapper)"/>. Calls <see cref="IDataPackageItem.OnBeforeSave"/> between these two.</remarks>
        /// <returns></returns>
        protected virtual dataItemContainer AddDataItem(TWrapper wrapper, XmlSerializer xmlSer = null)
        {
            dataItemContainer iContainer = new dataItemContainer();
            T item = GetInstanceToPack(wrapper);

            if (item is IDataPackageItem)
            {
                IDataPackageItem item_IDataPackageItem = (IDataPackageItem)item;
                item_IDataPackageItem.OnBeforeSave();
            }

            iContainer.id = GetDataPackageID(wrapper);

            TextWriter writer = new StringWriter();
            if (xmlSer == null) xmlSer = xSer;

            xmlSer.Serialize(writer, item);
            writer.Close();

            iContainer.instanceXml = writer.ToString();

            content.Add(iContainer);
            return iContainer;
        }

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <param name="iContainer">The i container.</param>
        /// <returns></returns>
        protected virtual instanceItemContainer<T> GetItem(dataItemContainer iContainer)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(T));
            TextReader reader = new StringReader(iContainer.instanceXml);
            object obj = deserializer.Deserialize(reader);
            T output = (T)obj;
            reader.Close();

            return new instanceItemContainer<T>(iContainer.id, output);
        }

        /// <summary>
        /// Adds the data items.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="doInParaller">if set to <c>true</c> [do in paraller].</param>
        /// <returns></returns>
        protected aceConcurrentBag<dataItemContainer> AddDataItems(IEnumerable<TWrapper> items, Boolean doInParaller = true)
        {
            aceConcurrentBag<dataItemContainer> bag = new aceConcurrentBag<dataItemContainer>();

            if (doInParaller)
            {
                Parallel.ForEach(items, (i) =>
                {
                    bag.Add(AddDataItem(i));
                });
            }
            else
            {
                foreach (var i in items)
                {
                    bag.Add(AddDataItem(i));
                }
            }
            return bag;
        }

        /// <summary>
        /// Deserializes stored data
        /// </summary>
        /// <param name="doInParaller">if set to <c>true</c> [do in paraller].</param>
        /// <returns></returns>
        protected Dictionary<String, T> GetDataItems(Boolean doInParaller = true)
        {
            aceConcurrentBag<instanceItemContainer<T>> bag = new aceConcurrentBag<instanceItemContainer<T>>();
            Dictionary<String, T> output = new Dictionary<String, T>();

            if (doInParaller)
            {
                Parallel.ForEach(bagContent, (i) =>
                {
                    bag.Add(GetItem(i));
                });
            }
            else
            {
                foreach (var i in bagContent)
                {
                    bag.Add(GetItem(i));
                }
            }

            foreach (var i in bag)
            {
                output.Add(i.id, i.instance);
                if (i.instance is IDataPackageItem)
                {
                    IDataPackageItem item_IDataPackageItem = (IDataPackageItem)i.instance;
                    item_IDataPackageItem.OnLoaded();
                }
            }

            return output;
        }
    }
}