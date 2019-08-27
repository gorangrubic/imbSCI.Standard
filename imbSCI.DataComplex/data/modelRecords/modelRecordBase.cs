// --------------------------------------------------------------------------------------------------------------------
// <copyright file="modelRecordBase.cs" company="imbVeles" >
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
    using imbSCI.Core.data;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.interfaces;
    using imbSCI.Core.math;
    using imbSCI.Core.reporting;
    using imbSCI.Core.reporting.format;
    using imbSCI.Data;
    using imbSCI.Data.interfaces;
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Threading;

    public abstract class modelRecordBase : modelDataSet, IAppendDataFields, IAppendDataFieldsExtended, IModelRecord, IAutosaveEnabled, ILogable, IConsoleControl, IObjectWithName//, ILogSerializableProvider<IModelRecordSerializable>
    {
        public string name
        {
            get { return instanceID; }
            set
            {
                // << --- no set
            }
        }

        /// <summary>
        /// Dumps its data into Serializable tween
        /// </summary>
        /// <param name="output">The output.</param>
        public virtual void SetLogSerializable(IModelRecordSerializable output)
        {
            output.modelUID = UID;

            var data = AppendDataFields(null, modelRecordFieldToAppendFlags.all);
            output.modelDataFieldDamp = data.ToString();
            output.modelIndexCurrent = childIndexCurrent;
            output.modelInstanceID = instanceID;
            output.modelLogContent = logBuilder.GetContent();
            output.modelNote = "";
            output.modelRunStamp = testRunStamp;
            output.modelStartingThread = startingThread;
            output.modelState = state;
            output.modelTimeFinish = timeFinish;
            output.modelTimeStart = timeStart;
            output.modelClassName = GetType().Name;
        }

        private int _childIndexCurrent = 0; //= new Int32();

        /// <summary> </summary>
        public int childIndexCurrent
        {
            get
            {
                return _childIndexCurrent;
            }
            protected set
            {
                _childIndexCurrent = value;
                OnPropertyChanged("childIndexCurrent");
            }
        }

        public abstract modelRecordMode VAR_RecordModeFlags { get; }

        /// <summary>
        ///
        /// </summary>
        public bool VAR_AllowInstanceToOutputToConsole { get; set; }

        /// <summary>
        /// IT WILL NOT ATTACH <c>__instance</c> into <c>instance</c> property. Reads meta data about the instance and does some preparations.
        /// </summary>
        /// <param name="__instance">The instance.</param>
        internal modelRecordBase(string runstamp, object __instance)
        {
            _modelRecordCommonSetup();
            learnAboutInstance(__instance);
        }

        //protected modelRecordBase()
        //{
        //    _modelRecordCommonSetup();
        //}

        private void _modelRecordCommonSetup()
        {
            _modelRecordGlobalCount.Add(true);
            id_global = modelRecordGlobalCount + 1;
            timeStart = DateTime.Now;
            timeFinish = DateTime.Now;
            iTI = new settingsMemberInfoEntry(this);
        }

        /// <summary>
        /// Gets a value indicating whether the instance should be registered for autosave call on application close
        /// </summary>
        /// <value>
        /// <c>true</c> if [variable register for autosave]; otherwise, <c>false</c>.
        /// </value>
        public virtual bool VAR_RegisterForAutosave { get { return true; } }

        /// <summary>
        /// Gets the variable filename prefix to be used
        /// </summary>
        /// <value>
        /// The variable filename prefix.
        /// </value>
        public virtual string VAR_FilenamePrefix { get { return "record_" + GetType().Name; } }

        /// <summary>
        /// Gets the variable filename extension.
        /// </summary>
        /// <value>
        /// The variable filename extension.
        /// </value>
        public virtual string VAR_FilenameExtension { get { return "log"; } }

        /// <summary>
        /// Gets the variable folder path for autosave.
        /// </summary>
        /// <value>
        /// Path (from app. root) to store the record on autosave. Also used as default on regular save call.
        /// </value>
        public virtual string VAR_FolderPathForAutosave { get { return "diagnostic\\records"; } }

        /// <summary>
        /// The root base of filename (without extension) for autosave.
        /// </summary>
        /// <value>
        /// The variable filename base.
        /// </value>
        /// <remarks>
        /// This should be an abstract property in abstract base classes
        /// </remarks>
        public virtual string VAR_FilenameBase
        {
            get
            {
                string fn = "";
                fn = fn.add(id_global.ToString("#00"), "_");
                if (instanceID.isNullOrEmpty())
                {
                    fn += "_for_" + instanceID;
                }
                else
                {
                    fn += "_for_unknown_instance";
                }
                return fn;
            }
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void log(string message)
        {
            logBuilder.log(message);
        }

        /// <summary>
        /// The model record global count
        /// </summary>
        private static ConcurrentBag<bool> _modelRecordGlobalCount = new ConcurrentBag<bool>();

        /// <summary>
        /// Gets the model record global count.
        /// </summary>
        /// <value>
        /// The model record global count.
        /// </value>
        public static long modelRecordGlobalCount
        {
            get
            {
                return _modelRecordGlobalCount.Count();
            }
        }

        /// <summary>
        /// TRUE: it sill throw exceptions on iregular condition
        /// </summary>
        public const bool DO_EXCEPTIONS = false;

        /// <summary>
        /// Global universal ID based on number of instances
        /// </summary>
        protected long id_global { get; set; } = 0;

        /// <summary>
        ///
        /// </summary>
        protected settingsMemberInfoEntry iTI { get; set; }

        /// <summary>
        /// Learns the about related instance/key object
        /// </summary>
        /// <param name="instanceOrKey">The instance or key.</param>
        internal void learnAboutInstance(object instanceOrKey)
        {
            instanceTypeInfo = new settingsMemberInfoEntry(instanceOrKey);

            PropertyEntry pe = new PropertyEntry(instanceTypeInfo.displayName, instanceOrKey);

            instanceOrKeyData = new PropertyCollectionExtended();
            instanceOrKeyData.AppendVertically(pe);
        }

        /// <summary>
        ///
        /// </summary>
        protected PropertyCollectionExtended instanceOrKeyData { get; set; }

        /// <summary>
        /// Type information about instance that this <see cref="IModelRecord"/> describes. Has to be trough <see cref="learnAboutInstance(object)"/>
        /// </summary>
        protected settingsMemberInfoEntry instanceTypeInfo { get; set; }

        /// <summary>
        ///
        /// </summary>
        public modelRecordStateEnum state { get; protected set; }

        /// <summary>
        /// Remarks about algorithm execution and/or results
        /// </summary>
        public modelRecordRemarkFlags remarkFlags { get; set; }

        private TimeSpan _duration = new TimeSpan();

        /// <summary> </summary>
        public TimeSpan duration
        {
            get
            {
                return _duration;
            }
            protected set
            {
                _duration = value;
                OnPropertyChanged("duration");
            }
        }

        private int _startCallCount = 0;

        /// <summary>
        /// Number of start calls
        /// </summary>
        public int startCallCount
        {
            get
            {
                return _startCallCount;
            }
            protected set
            {
                _startCallCount = value;
                OnPropertyChanged("startCallCount");
            }
        }

#pragma warning disable CS1574 // XML comment has cref attribute 'recordStart' that could not be resolved
        /// <summary>
        /// The moment of <see cref="recordStart"/> call.
        /// </summary>
        public DateTime timeStart { get; protected set; }
#pragma warning restore CS1574 // XML comment has cref attribute 'recordStart' that could not be resolved

        private DateTime _timeFinish;

#pragma warning disable CS1574 // XML comment has cref attribute 'recordFinish' that could not be resolved
        /// <summary>
        /// The moment of <see cref="recordFinish"/> call.
        /// </summary>
        /// <value>
        /// The time finish.
        /// </value>
        public DateTime timeFinish
#pragma warning restore CS1574 // XML comment has cref attribute 'recordFinish' that could not be resolved
        {
            get
            {
                return _timeFinish;
            }
            protected set
            {
                _timeFinish = value;
                OnPropertyChanged("timeFinish");
            }
        }

        /// <summary>
        /// Auto-generated UID based on <see cref="instanceID"/>, typesignature and start time
        /// </summary>
        public string UID { get; protected set; } = "";

        /// <summary>
        /// Human readable instanceID before generation of UID
        /// </summary>
        public string instanceID { get; set; } = "";

        /// <summary>
        /// Runstamp of test being recorded
        /// </summary>
        public string testRunStamp { get; set; }

        /// <summary>
        /// Gets the content of the log. Make sure that <see cref="state"/> is <see cref="modelRecordStateEnum.finished"/> before trying to get the log content
        /// </summary>
        /// <value>
        /// The content of the log.
        /// </value>
        /// <exception cref="Exception">The record [" + instanceID + ":" + this.GetType().Name + "] is not finished. Call finish method to have log content available. (UID:" + UID + ")</exception>
        public string logContent
        {
            get
            {
                if (state != modelRecordStateEnum.finished)
                {
                    string msg = "The record [" + instanceID + ":" + GetType().Name + "] is not finished. Call finish method to have log content available. (UID:" + UID + ")";
                    //if (DO_EXCEPTIONS)
                    //{
                    //    throw new dataException(msg); // )
                    //}
                    //else
                    //{
                    logBuilder.AppendHeading(msg);
                    logBuilder.AppendLine("::: this is probably autosave call on *UnhandledException* Application level event :::");
                    //}
                }
                return _logContent;
            }
        }

        /// <summary>
        /// Appends the data fields.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="whatToAppend">The what to append.</param>
        /// <returns></returns>
        public virtual PropertyCollectionExtended AppendDataFields(PropertyCollectionExtended data, modelRecordFieldToAppendFlags whatToAppend)
        {
            if (data == null) data = new PropertyCollectionExtended();
            if (whatToAppend.HasFlag(modelRecordFieldToAppendFlags.identification))
            {
                data.Add(modelRecordColumnEnum.iid, instanceID, "Instance ID", "Human readable identification of instance covered by the record");
                if (instanceTypeInfo != null)
                {
                    data.Add(modelRecordColumnEnum.instance, instanceTypeInfo.displayName, "Instance type", "Type name of the instance followed by this record");
                    data.Add(modelRecordColumnEnum.instancedesc, instanceTypeInfo.description, "Instance info", "Notes on the instance type");
                }
                data.Add(modelRecordColumnEnum.uid, UID, "UID", "Universal code-ID of the record instance");
                data.Add(modelRecordColumnEnum.recordstate, state, "Record state", "State flag for this record instance");
                data.Add(modelRecordColumnEnum.runstamp, testRunStamp, "Run stamp", "Identification code of the experiment test run instance");
                data.Add(modelRecordColumnEnum.recordtype, iTI.displayName, "Record type", "Name of the record class");
                data.Add(modelRecordColumnEnum.recorddesc, iTI.description, "Record info", "Notes about applied record class");
            }
            if (whatToAppend.HasFlag(modelRecordFieldToAppendFlags.modelRecordCommonData))
            {
                string duration_str = timeFinish.Subtract(timeStart).TotalMilliseconds.getTimeSecString();
                if (VAR_RecordModeFlags.HasFlag(modelRecordMode.singleStarter))
                {
                    data.Add(modelRecordColumnEnum.start, timeStart.ToShortTimeString(), "Start", "Start of the record / the instance run");
                    data.Add(modelRecordColumnEnum.finish, timeFinish.ToShortTimeString(), "Finish", "Time of the instance execution finished");
                }
                else if (VAR_RecordModeFlags.HasFlag(modelRecordMode.multiStarter))
                {
                    data.Add(modelRecordColumnEnum.sessions, timeFinish.ToShortTimeString(), "Sessions", "Number of the record start-finish sessions");
                }
                if (VAR_RecordModeFlags.HasFlag(modelRecordMode.starter))
                {
                    data.Add(modelRecordColumnEnum.duration, duration_str, "Duration", "Time span of the instance run, in seconds.");
                }
                data.Add(modelRecordColumnEnum.remarks, remarkFlags.ToStringEnumSmart(), "Remarks", "Remark flags about the instance execution");
            }
            if (whatToAppend.HasFlag(modelRecordFieldToAppendFlags.modelRecordInstanceData))
            {
                data.AddRange(instanceOrKeyData, false, false, false);
            }
            //if (whatToAppend.HasFlag(modelRecordFieldToAppendFlags.modelRecordLogData))
            //{
            //    logBuilder.AppendDataFields(data);
            //}

            return data;
        }

        private string _startingThread = "";

        /// <summary> </summary>
        public string startingThread
        {
            get
            {
                return _startingThread;
            }
            protected set
            {
                _startingThread = value;
                OnPropertyChanged("startingThread");
            }
        }

        /// <summary>
        /// Records the start.
        /// </summary>
        /// <param name="__testRunStamp">The test run stamp.</param>
        /// <param name="__instanceID">The instance identifier.</param>
        /// <param name="">The .</param>
        public virtual void _recordStart(string __testRunStamp, string __instanceID)
        {
            instanceID = __instanceID;

            if (instanceID.isNullOrEmpty()) instanceID = __testRunStamp + "_" + timeStart.ToString() + id_global + "_" + GetHashCode().ToString("D4");

            UID = md5.GetMd5Hash(instanceID);

            if (VAR_RecordModeFlags.HasFlag(modelRecordMode.nonStarter)) throw new InvalidOperationException("This record is nonStarter - should be started nor finished");

            testRunStamp = __testRunStamp;

            var laststate = state;
            state = modelRecordStateEnum.started;
            timeStart = DateTime.Now;

            //if (VAR_AllowAutoOutputToConsole)  aceLog.consoleControl.setAsOutput(this, VAR_LogPrefix);

            //dataSet = new DataSet(instanceID + testRunStamp);

            logBuilder.log("Record (" + iTI.displayName + ") for `**" + __testRunStamp + "**` started as: `**" + instanceID + "**`  _(UID:" + UID + ")_ ");

            if (laststate == modelRecordStateEnum.initiated)
            {
                logBuilder.log("Record (" + iTI.displayName + ") for `**" + __testRunStamp + "**` started after [init call] as: `**" + instanceID + "**`  _(UID:" + UID + ")_ ");
                if (VAR_RecordModeFlags.HasFlag(modelRecordMode.obligationStartBeforeInit))
                {
                    throw new InvalidOperationException("Obligation " + nameof(modelRecordMode.obligationStartBeforeInit) + " not respected");
                }
            }
            else if (laststate == modelRecordStateEnum.notStarted)
            {
                if (VAR_RecordModeFlags.HasFlag(modelRecordMode.obligationInitBeforeStart))
                {
                    throw new InvalidOperationException("Obligation " + nameof(modelRecordMode.obligationInitBeforeStart) + " not respected");
                }
                logBuilder.log("Record (" + iTI.displayName + ") for `**" + __testRunStamp + "**` started before [init call] as: `**" + instanceID + "**`  _(UID:" + UID + ")_ ");
            }
            else if (laststate == modelRecordStateEnum.started)
            {
                if (VAR_RecordModeFlags.HasFlag(modelRecordMode.singleStarter))
                {
                    string threadName = Thread.CurrentThread.ManagedThreadId.ToString().add(Thread.CurrentThread.Name);
                    string msgSufix = "";
                    if (threadName == startingThread)
                    {
                        msgSufix = " - by the same thread [" + threadName + "]";
                    }
                    else
                    {
                        msgSufix = " - by thread [" + startingThread + "]. This is [" + threadName + "]";
                    }

                    var axe = new InvalidOperationException("This record was already started [" + __instanceID + "] [" + testRunStamp + "] " + msgSufix);

                    throw axe;
                }
            }
            startCallCount++;

            startingThread = Thread.CurrentThread.ManagedThreadId.ToString().add(Thread.CurrentThread.Name);
            _recordStartHandle();
        }

        protected abstract void _recordStartHandle();

#pragma warning disable CS1574 // XML comment has cref attribute 'dataCollectionExtendedList' that could not be resolved
        /// <summary>
        /// Override this method with instructions to update <see cref="dataCollectionExtendedList"/>
        /// </summary>
        public abstract void datasetBuildOnFinish();
#pragma warning restore CS1574 // XML comment has cref attribute 'dataCollectionExtendedList' that could not be resolved

        /// <summary>
        /// Default dataset build - intended for mid-level class overload
        /// </summary>
        public virtual void datasetBuildOnFinishDefault()
        {
            if (reallyFinished)
            {
                throw new InvalidOperationException("This record already created finish calculations");
            }
            reallyFinished = true;
            if (dataCollectionExtendedList == null) dataCollectionExtendedList = new PropertyCollectionExtendedList();

            dataCollectionExtendedList.Add(AppendDataFields(null, modelRecordFieldToAppendFlags.modelRecordCommonData), modelRecordDataSetCategoriesEnum.master_record);
            dataCollectionExtendedList.Add(AppendDataFields(null, modelRecordFieldToAppendFlags.identification), modelRecordDataSetCategoriesEnum.master_log_info);
        }

        /// <summary>
        /// Records the finish.
        /// </summary>
        public virtual void _recordFinish()
        {
            if (state == modelRecordStateEnum.notStarted) throw new InvalidOperationException("This record was never started nor initiated!!!");

            if (VAR_RecordModeFlags.HasFlag(modelRecordMode.nonStarter)) throw new InvalidOperationException("This record is nonStarter - should be started nor finished");

            state = modelRecordStateEnum.finished;
            timeFinish = DateTime.Now;

            duration.Add(timeFinish.Subtract(timeStart));

            logBuilder.log("Record finished: `**" + instanceID + "**`  _(UID:" + UID + ")_ after: [" + duration.TotalSeconds.getTimeSecString() + "]");

            // if (VAR_AllowAutoOutputToConsole) aceLog.consoleControl.removeFromOutput(this); //setAsOutput(this, VAR_LogPrefix);
            // TODO: Reimplement centralized loging

            _logContent = logBuilder.ContentToString(true, reportOutputFormatName.textMdFile);

            if (VAR_RecordModeFlags.HasFlag(modelRecordMode.singleStarter))
            {
                _doOnRealFinish();
            }
        }

        private bool _reallyFinished = false;

        /// <summary> </summary>
        public bool reallyFinished
        {
            get
            {
                return _reallyFinished;
            }
            protected set
            {
                _reallyFinished = value;
                OnPropertyChanged("reallyFinished");
            }
        }

        private object FinishLock = new object();

#pragma warning disable CS1574 // XML comment has cref attribute 'dataException' that could not be resolved
#pragma warning disable CS1574 // XML comment has cref attribute 'aceObligationException' that could not be resolved
        /// <summary>
        /// Builds data sets --- calls <see cref="datasetBuildOnFinishDefault"/> and after that <see cref="datasetBuildOnFinish"/>
        /// </summary>
        /// <exception cref="aceCommonTypes.core.exceptions.dataException">DataSet is empty for record [" + instanceID + "] in test run [" + testRunStamp + "] - null - DataSet not populated</exception>
        /// <exception cref="aceCommonTypes.core.exceptions.aceObligationException"></exception>
        protected void _doOnRealFinish(bool callBuilds = false)
#pragma warning restore CS1574 // XML comment has cref attribute 'aceObligationException' that could not be resolved
#pragma warning restore CS1574 // XML comment has cref attribute 'dataException' that could not be resolved
        {
            lock (FinishLock)
            {
                if (state == modelRecordStateEnum.finished) return;
                if (VAR_RecordModeFlags.HasFlag(modelRecordMode.callDataSetBuildOnFinish) || callBuilds)
                {
                    // datasetBuildOnFinishDefault();
                    //datasetBuildOnFinish();

                    //if (dataSet.Tables.Count == 0)
                    //{
                    //    if (VAR_RecordModeFlags.HasFlag(modelRecordMode.obligationDataSet))
                    //    {
                    //        throw new dataException("DataSet is empty for record [" + instanceID + "] in test run [" + testRunStamp + "]", null, this, "DataSet not populated");
                    //    }
                    //}

                    //if (VAR_RecordModeFlags.HasFlag(modelRecordMode.obligationBuildSummaryStatistics))
                    //{
                    //    if (!dataSet.Tables.Contains(DATANAME_Summary)) throw new aceObligationException(this, modelRecordMode.obligationBuildSummaryStatistics);
                    //}
                }
            }
        }
    }
}