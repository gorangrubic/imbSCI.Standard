// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IModelRecordSerializable.cs" company="imbVeles" >
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
    using System;

    public interface IModelRecordSerializable //:ILogSerializable
    {
        string modelClassName { get; set; }
        string modelUID { get; set; }
        string modelInstanceID { get; set; }
        string modelRunStamp { get; set; }
        string modelLogContent { get; set; }
        int modelIndexCurrent { get; set; }
        DateTime modelTimeStart { get; set; }
        DateTime modelTimeFinish { get; set; }

        /// <summary>
        /// Special string note about parameters not available at <see cref="modelRecordBase"/> level
        /// </summary>
        /// <value>
        /// The model note.
        /// </value>
        string modelNote { get; set; }

        modelRecordStateEnum modelState { get; set; }
        modelRecordRemarkFlags modelRemarkFlags { get; set; }

        string modelDataFieldDamp { get; set; }

        //PropertyCollectionExtended AppendDataFields(PropertyCollectionExtended data, modelRecordFieldToAppendFlags whatToAppend);

        string modelStartingThread { get; set; }
    }
}