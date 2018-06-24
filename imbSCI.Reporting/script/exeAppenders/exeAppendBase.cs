// --------------------------------------------------------------------------------------------------------------------
// <copyright file="exeAppendBase.cs" company="imbVeles" >
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
namespace imbSCI.Reporting.script.exeAppenders
{
    using imbSCI.Core.collection;
    using imbSCI.Core.reporting.render;
    using imbSCI.Data.data;
    using imbSCI.DataComplex.data.modelRecords;
    using imbSCI.DataComplex.diagram.core;
    using System.Collections.Generic;
    using System.Data;

    /// <summary>
    /// exeAppend is custom-code module injectable in <see cref="docScript"/> execution pipeline.
    /// </summary>
    /// <seealso cref="IExeAppend" />
    public abstract class exeAppendBase : imbBindable, IExeAppend
    {
        /// <summary>
        /// Name for this instance - opional, used for content creation
        /// </summary>
        public string name { get; set; } = "";

        /// <summary>
        /// Human-readable description of object instance - optional used for content cretion
        /// </summary>
        public string description { get; set; } = "";

        private IModelRecord _record;

        /// <summary> </summary>
        public IModelRecord record
        {
            get
            {
                return _record;
            }
            set
            {
                _record = value;
                OnPropertyChanged("record");
            }
        }

        /// <summary>
        /// The implementation requres parametarless constructor to be only one
        /// </summary>
        public exeAppendBase()
        {
        }

        /// <summary>
        ///
        /// </summary>
        protected Dictionary<string, PropertyCollectionExtended> staticData { get; set; } = new Dictionary<string, PropertyCollectionExtended>();

        /// <summary>
        /// Gets or sets the data set.
        /// </summary>
        /// <value>
        /// The data set.
        /// </value>
        protected DataSet dataSet { get; set; } = new DataSet();

        /// <summary>
        /// Sets the data.
        /// </summary>
        /// <param name="_data">The data.</param>
        /// <returns></returns>
        public IExeAppend setData(DataTable _data)
        {
            dataSet.Tables.Add(_data);
            return this;
        }

        /// <summary>
        /// Sets the data.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="_data">The data.</param>
        /// <returns></returns>
        public IExeAppend setData(string key, PropertyCollectionExtended _data)
        {
            staticData.Add(key, _data);
            return this;
        }

        /// <summary>
        /// Executes the <see cref="exeAppendBase"/> instance
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="render">The render.</param>
        public abstract IExeAppend execute(IRenderExecutionContext context, ITextRender render);
    }
}