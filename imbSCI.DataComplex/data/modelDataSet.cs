// --------------------------------------------------------------------------------------------------------------------
// <copyright file="modelDataSet.cs" company="imbVeles" >
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
namespace imbSCI.DataComplex.data
{
    using imbSCI.Core.collection;
    using imbSCI.Core.interfaces;
    using imbSCI.Core.reporting;
    using imbSCI.Core.reporting.render.builders;
    using imbSCI.Data.data;
    using System;
    using System.Data;

    public abstract class modelDataSet : imbBindable, IAppendDataFields, IAppendDataFieldsExtended, ILogable, IConsoleControl
    {
        /// <summary>
        ///
        /// </summary>
        public bool VAR_AllowInstanceToOutputToConsole { get; set; }

        public abstract bool VAR_AllowAutoOutputToConsole { get; }

        public abstract string VAR_LogPrefix { get; }

        /// <summary>
        /// Summary statistics inside <see cref="dataCollectionExtendedList"/> and <see cref="dataSet"/>
        /// </summary>
        public const string DATANAME_Summary = "summary";

        /// <summary>
        /// Children statistics inside <see cref="dataCollectionExtendedList"/> and <see cref="dataSet"/>
        /// </summary>
        public const string DATANAME_Children = "children";

        /// <summary>
        /// Instance statistics inside <see cref="dataCollectionExtendedList"/> and <see cref="dataSet"/>
        /// </summary>
        public const string DATANAME_Instance = "instance";

        private PropertyCollectionExtendedList _dataCollectionExtendedList = new PropertyCollectionExtendedList();

        /// <summary>DataField sets stored at record Finish call</summary>
        public PropertyCollectionExtendedList dataCollectionExtendedList
        {
            get
            {
                return _dataCollectionExtendedList;
            }
            protected set
            {
                _dataCollectionExtendedList = value;
                OnPropertyChanged("dataset");
            }
        }

        /// <summary>
        /// Appends its data points into new or existing property collection
        /// </summary>
        /// <param name="data">Property collection to add data into</param>
        /// <returns>Updated or newly created property collection</returns>
        public abstract PropertyCollectionExtended AppendDataFields(PropertyCollectionExtended data = null);

        /// <summary>
        /// Appends its data points into new or existing property collection
        /// </summary>
        /// <param name="data">Property collection to add data into</param>
        /// <returns>
        /// Updated or newly created property collection
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        PropertyCollection IAppendDataFields.AppendDataFields(PropertyCollection data = null) => (PropertyCollection)AppendDataFields(data as PropertyCollectionExtended);

        public void log(string message)
        {
            ((ILogable)logBuilder).log(message);
        }

        protected ILogBuilder _log = new builderForLogBase(); // new ILogBuilder();

        /// <summary>
        /// Log creator
        /// </summary>
        public ILogBuilder logBuilder
        {
            get
            {
                return _log;
            }
            protected set { _log = value; }
        }

        protected string _logContent;
    }
}