// --------------------------------------------------------------------------------------------------------------------
// <copyright file="modelRecordSummaryBase.cs" company="imbVeles" >
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

    public abstract class modelRecordSummaryBase<TInstance, TSideInstance, TSideRecord> : modelRecordStandaloneBase<TInstance>, IModelRecordSummary<TSideInstance, TSideRecord>
        where TInstance : class, IObjectWithName, IObjectWithDescription, IObjectWithNameAndDescription
        //where TSideInstance : class, IObjectWithName, IObjectWithDescription, IObjectWithNameAndDescription
        where TSideRecord : modelRecordBase, IModelStandaloneRecord<TSideInstance>
    {
        public modelRecordSummaryBase(string __testRunStamp, TInstance __instance) : base(__testRunStamp, __instance)
        {
        }

        public override modelRecordMode VAR_RecordModeFlags
        {
            get
            {
                return modelRecordMode.multiStarter | modelRecordMode.obligationDataSet | modelRecordMode.obligationBuildSummaryStatistics | modelRecordMode.summaryScope;
            }
        }

        /// <summary>
        /// Finish this summary record
        /// </summary>
        public void summaryFinished()
        {
            _summaryFinished();
            _doOnRealFinish();
        }

        /// <summary>
        /// Boing to be executed before <see cref="modelRecordBase.datasetBuildOnFinish"/> and <see cref="modelRecordBase.datasetBuildOnFinishDefault"/>
        /// </summary>
        protected abstract void _summaryFinished();

        private modelSideRecordSetCollection<TSideInstance, TSideRecord> _sideRecordSets = new modelSideRecordSetCollection<TSideInstance, TSideRecord>();

        /// <summary> </summary>
        public modelSideRecordSetCollection<TSideInstance, TSideRecord> sideRecordSets
        {
            get
            {
                return _sideRecordSets;
            }
            protected set
            {
                _sideRecordSets = value;
                OnPropertyChanged("sideRecordSets");
            }
        }

        public override void _recordFinish()
        {
            base._recordFinish();
        }

        public virtual void AddSideRecord(TSideRecord __sideRecord)
        {
            sideRecordSets.AddRecord(__sideRecord.instance, __sideRecord);
        }

        //public abstract void storeSideRecord(TSideRecord __sideRecord);
    }
}