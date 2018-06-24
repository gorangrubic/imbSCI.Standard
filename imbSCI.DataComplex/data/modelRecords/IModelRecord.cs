// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IModelRecord.cs" company="imbVeles" >
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
namespace imbSCI.DataComplex.data.modelRecords
{
    using imbSCI.Core.collection;
    using imbSCI.Core.interfaces;
    using imbSCI.Core.reporting;
    using imbSCI.Data.interfaces;
    using System;

    /// <summary>
    /// Data model objects -- with records of <see cref="imbFramework.tests.testDefinition"/> execution
    /// </summary>
    public interface IModelRecord : IAppendDataFieldsExtended, IAutosaveEnabled, ILogable, IConsoleControl //, ILogSerializableProvider<IModelRecordSerializable>
    {
        // void initiate(String __instanceID, String __testRunStamp);
        string UID { get; }

        string instanceID { get; }
        string testRunStamp { get; }
        string logContent { get; }
        int childIndexCurrent { get; }
        DateTime timeStart { get; }
        DateTime timeFinish { get; }
        ILogBuilder logBuilder { get; }

        modelRecordStateEnum state { get; }
        modelRecordRemarkFlags remarkFlags { get; set; }

        PropertyCollectionExtended AppendDataFields(PropertyCollectionExtended data, modelRecordFieldToAppendFlags whatToAppend);

        string startingThread { get; }
    }
}