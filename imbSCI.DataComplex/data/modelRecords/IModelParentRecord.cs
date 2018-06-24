// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IModelParentRecord.cs" company="imbVeles" >
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
    using imbSCI.Data.interfaces;

    //public interface IModelParentRecord

    public interface IModelParentRecord : IModelStandalone
    {
        IModelStandalone childRecord { get; }

        object childInstance { get; }

        void finishChildRecord();

        void recordStart(string __testRunStamp, string __instanceID, params object[] resources);

        void recordFinish(params object[] resources);
    }

    //public interface IModelParentRecord<TChildInstance, TChildRecord>:IModelStandalone
    //{
    //    //instanceWithRecordCollection<TChildInstance, TChildRecord> children { get; }
    //    TChildRecord childRecord { get; }

    //    TChildInstance childInstance { get; }

    //    void finishChildRecord();
    //    void startChildRecord(TChildInstance instance, String __instanceID);
    //    void recordStart(String __testRunStamp, String __instanceID, params Object[] resources);
    //    void recordFinish(params Object[] resources);
    //}

    public interface IModelParentRecord<TInstance, TChildInstance, TChildRecord> : IModelStandaloneRecord<TInstance>, IModelParentRecord
        where TChildInstance : class, IObjectWithName, IObjectWithDescription, IObjectWithNameAndDescription
        where TChildRecord : modelRecordBase, IModelRecord
    {
        instanceWithRecordCollection<TChildInstance, TChildRecord> children { get; }
        TChildRecord childRecord { get; }
        TChildInstance childInstance { get; }
        TInstance instance { get; }

        void finishChildRecord();

        void startChildRecord(TChildInstance instance, string __instanceID);

        void recordStart(string __testRunStamp, string __instanceID, params object[] resources);

        void recordFinish(params object[] resources);
    }
}