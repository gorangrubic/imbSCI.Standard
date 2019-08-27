using imbSCI.BusinessData.Core;
using imbSCI.Core.files;
using imbSCI.Core.files.folders;
using imbSCI.Data;
using System;
using System.IO;
using System.Linq;

namespace imbSCI.BusinessData.Storage
{
/// <summary>
    /// Base for a record provider implementation
    /// </summary>
    /// <typeparam name="TCollection">The type of the collection.</typeparam>
    /// <typeparam name="TRecord">The type of the record.</typeparam>
    public abstract class RecordProvider<TCollection, TRecord>
        where TCollection : RecordsWithUIDCollection<TRecord>, IReferencedRecordsCollection, new()
        where TRecord : class, IRecordWithGetUID, new()
    {
        protected Type RecordType { get; set; }

        public RecordProviderOperationMode OperationMode { get; set; } = RecordProviderOperationMode.multiCollectionMode;

        public const String EXCEPTION_NOCOLLECTION_ID = "Collection reference must be specified when provider is in multi collection mode (OperationMode = RecordProviderOperationMode.multiCollectionMode)";

        public abstract RecordProviderResponse GetCollection(string collection_uid = "");

        public abstract RecordProviderResponse GetRecord(String record_uid, string collection_uid = "");

        public abstract RecordProviderResponse SaveRecord(TRecord record, string collection_uid = "", RecordProviderResponse response = null);

        public abstract RecordProviderResponse SaveCollection(TCollection collection, Boolean delete_all_existing_records = false, RecordProviderResponse response = null);
        
        public String RecordStoredNamePrefix { get; set; } = "rec_";
        public String CollectionStoredNamePrefix { get; set; } = "col_";


        /// <summary>
        /// Loads entire collection, under specified <c>collection_uid</c>
        /// </summary>
        /// <param name="collection_uid">The collection uid.</param>
        /// <returns></returns>
        public TCollection LoadCollection(String collection_uid)
        {
            var response = GetCollection(collection_uid);
            
            var output = response.collection as TCollection;
            if (output == null)
            {
                output = new TCollection();
            }
            return output;
        }

        /// <summary>
        /// Loads a record, designated by specifeid <c>record_uid</c> and <c>collection_uid</c>
        /// </summary>
        /// <param name="record_uid">The record uid.</param>
        /// <param name="collection_uid">The collection uid.</param>
        /// <returns>Loaded record</returns>
        public TRecord LoadRecord(String record_uid, string collection_uid = "")
        {
            var response = GetRecord(record_uid, collection_uid);
            return response.record as TRecord;
        }
    }
}