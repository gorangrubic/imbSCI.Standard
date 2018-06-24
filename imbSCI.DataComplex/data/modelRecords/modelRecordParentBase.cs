// --------------------------------------------------------------------------------------------------------------------
// <copyright file="modelRecordParentBase.cs" company="imbVeles" >
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
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.interfaces;
    using imbSCI.Core.reporting;
    using imbSCI.Data.interfaces;
    using System.Collections.Generic;

    /// <summary>
    /// Base class for <see cref="IModelRecord"/> data model reports about an algorithm execution
    /// </summary>
    /// <seealso cref="aceCommonTypes.primitives.imbBindable" />
    /// <seealso cref="IAppendDataFields" />
    /// <seealso cref="IAppendDataFieldsExtended" />
    public abstract class modelRecordParentBase<TInstance, TChildInstance, TChildRecord> : modelRecordBase, IAppendDataFields, IAppendDataFieldsExtended,
        IModelParentRecord<TInstance, TChildInstance, TChildRecord>, IAutosaveEnabled, ILogable, IConsoleControl
    where TChildInstance : class, IObjectWithName, IObjectWithDescription, IObjectWithNameAndDescription
    where TChildRecord : modelRecordBase, IModelRecord, IModelStandalone
    {
        public modelRecordParentBase(string __testRunStamp, TInstance __instance) : base(__testRunStamp, __instance)
        {
            if (!__testRunStamp.isNullOrEmpty()) testRunStamp = __testRunStamp;
            instance = __instance;
            children = new instanceWithRecordCollection<TChildInstance, TChildRecord>(testRunStamp);
        }

        private TInstance _instance;

        /// <summary> </summary>
        public TInstance instance
        {
            get
            {
                return _instance;
            }
            protected set
            {
                _instance = value;
                OnPropertyChanged("instance");
            }
        }

        private IModelParentRecord _parent;

        /// <summary> </summary>
        public IModelParentRecord parent
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
                OnPropertyChanged("parent");
            }
        }

        private instanceWithRecordCollection<TChildInstance, TChildRecord> _children; // = new instanceWithRecordCollection<TChildInstance, TChildRecord>();

        /// <summary>The sub instance record collection. Collection index is -1 late to the real <see cref="childRecord"/> and <see cref="childInstance"/></summary>
        public instanceWithRecordCollection<TChildInstance, TChildRecord> children
        {
            get
            {
                return _children;
            }
            protected set
            {
                _children = value;
                OnPropertyChanged("children");
            }
        }

        IModelStandalone IModelParentRecord.childRecord => childRecord;

        /// <summary>
        /// Collects all first-level children and sends as <see cref="List{T}"/>
        /// </summary>
        /// <returns></returns>
        public List<TChildRecord> GetChildRecords()
        {
            List<TChildRecord> output = new List<TChildRecord>();
            foreach (KeyValuePair<TChildInstance, TChildRecord> pair in children)
            {
                output.Add(pair.Value);
            }
            return output;
        }

        private TChildRecord _childRecord;

        /// <summary> </summary>
        public TChildRecord childRecord
        {
            get
            {
                return _childRecord;
            }
            protected set
            {
                _childRecord = value;
                OnPropertyChanged("childRecord");
            }
        }

        object IModelParentRecord.childInstance => childInstance;

        private TChildInstance _childInstance;

        /// <summary> </summary>
        public TChildInstance childInstance
        {
            get
            {
                // return children.GetInstance(childRecord);
                return _childInstance;
            }
            protected set
            {
                _childInstance = value;
                OnPropertyChanged("childInstance");
            }
        }

        protected override void _recordStartHandle()
        {
            children.ResetIndex();
        }

        /// <summary>
        /// Starts the next child record - method for iterative call
        /// </summary>
        /// <remarks>Use only if the <c>children</c> collection is immutable and <c>instanceID</c></remarks>
        /// <returns></returns>
        public bool startNextChildRecord()
        {
            _finishChild();

            childRecord = children.GetCurrentRecord();
            childInstance = children.GetCurrentInstance();
            childRecord.parent = this;
            childRecord._recordStart(testRunStamp, childRecord.instanceID);
            log("SubRecord [" + childRecord.instanceID + "] started");

            return children.MoveNext();
        }

        private object getChildRecordLock = new object();

        /// <summary>
        /// Gets the child record - without starting it and without setting the <see cref="childRecord"/> nor <see cref="childInstance"/>. It is thread safe.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="__instanceID">The instance identifier.</param>
        /// <returns></returns>
        public TChildRecord getChildRecord(TChildInstance instance, string __instanceID)
        {
            lock (getChildRecordLock)
            {
                TChildRecord cRecord = null;
                cRecord = children.GetRecord(instance, true);
                cRecord.parent = this;
                return cRecord;
            }
        }

        /// <summary>
        /// Starts the child record -- creates instance, record and sets parent
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="__instanceID">The instance identifier.</param>
        public virtual void startChildRecord(TChildInstance instance, string __instanceID)
        {
            _finishChild();
            //if (instance == null) instance = new TChildInstance();

            childRecord = children.GetRecord(instance, true);
            childInstance = children.GetInstance(childRecord);
            childRecord.parent = this;

            childRecord._recordStart(testRunStamp, __instanceID);
            log("SubRecord [" + childRecord.instanceID + "] started");
        }

        private void _finishChild()
        {
            if (childRecord != null)
            {
                if (childRecord.state == modelRecordStateEnum.started)
                {
                    childRecord._recordFinish();

                    log("SubRecord [" + childRecord.instanceID + "] finished");
                }
            }
        }

        public virtual void finishChildRecord()
        {
            _finishChild();
        }

        /// <summary>
        /// Records the start. Make sure to call <see cref="_recordStart"/> at beginning of the method
        /// </summary>
        public abstract void recordStart(string __testRunStamp, string __instanceID, params object[] resources);

        /// <summary>
        /// Records the finish. Make sure to call <see cref="_recordFinish"/> at the end of the method
        /// </summary>
        public abstract void recordFinish(params object[] resources);
    }
}