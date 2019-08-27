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
    /// Provides access and storage mechanism for records collections
    /// </summary>
    /// <typeparam name="TCollection">The type of the collection.</typeparam>
    /// <typeparam name="TRecord">The type of the record.</typeparam>
    public class FileSystemRecordProvider<TCollection, TRecord> : RecordProvider<TCollection, TRecord>
        where TCollection : RecordsWithUIDCollection<TRecord>, IReferencedRecordsCollection, new()
        where TRecord : class, IRecordWithGetUID, new()
    {
        protected folderNode folder { get; set; }

        protected DirectoryInfo directory { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemRecordProvider{TCollection, TRecord}"/> class.
        /// </summary>
        /// <param name="storageFolder">Folder where collections are stored</param>
        /// <param name="record_prefix">Filename prefix for records</param>
        /// <param name="collection_prefix">Subdirectory name prefix for collections</param>
        public FileSystemRecordProvider(folderNode storageFolder, String record_prefix = "", String collection_prefix = "")
        {
            folder = storageFolder;
            directory = folder;
            if (record_prefix != "") RecordStoredNamePrefix = record_prefix;
            if (collection_prefix != "") CollectionStoredNamePrefix = collection_prefix;
            RecordType = typeof(TRecord);
            folder.generateReadmeFiles(null);
        }

        protected DirectoryInfo GetDirectoryForReference(string reference_uid, RecordProviderResponse response = null)
        {
            if (OperationMode == RecordProviderOperationMode.singleCollectionMode)
            {
                return directory;
            }

            if (OperationMode == RecordProviderOperationMode.multiCollectionMode && reference_uid.isNullOrEmpty())
            {
                throw new ArgumentOutOfRangeException(nameof(reference_uid), EXCEPTION_NOCOLLECTION_ID);
            }

            String needle = GetCollectionDirectoryName(reference_uid);
            foreach (DirectoryInfo d in directory.EnumerateDirectories())
            {
                if (d.Name == needle)
                {
                    if (response != null)
                    {
                        response.status |= RecordProviderResponseStatus.collectionFound;
                    }
                    return d;
                }
            }
            if (response != null)
            {
                response.status |= RecordProviderResponseStatus.collectionNotFound;
            }
            return null;
        }

        protected folderNode GetFolderForCollection(String collection_uid)
        {
            if (OperationMode == RecordProviderOperationMode.singleCollectionMode)
            {
                return folder;
            }

            if (OperationMode == RecordProviderOperationMode.multiCollectionMode && collection_uid.isNullOrEmpty())
            {
                throw new ArgumentOutOfRangeException(nameof(collection_uid), EXCEPTION_NOCOLLECTION_ID);
            }

            folderNode targetFolder = folder.Attach(GetCollectionDirectoryName(collection_uid), collection_uid, "Storage directory for collection records", false, true);
            return targetFolder;
        }

        
        protected String GetRecordFilename(string record_uid)
        {
            return RecordStoredNamePrefix + record_uid + ".xml";
        }

        protected String GetCollectionDirectoryName(string reference_uid)
        {
            return CollectionStoredNamePrefix + reference_uid;
        }

        protected String GetRecordUID(String filename)
        {
            String output = filename.removeStartsWith(RecordStoredNamePrefix);
            output = output.removeEndsWith(".xml");
            return output;
        }

        protected String GetCollectionUID(String directoryName)
        {
            String output = directoryName.removeStartsWith(CollectionStoredNamePrefix);
            return output;
        }

        protected FileInfo GetRecordFile(DirectoryInfo di, string record_uid)
        {
            var files = di.GetFiles(GetRecordFilename(record_uid));

            return files.FirstOrDefault();
        }

        protected TRecord LoadRecordFile(FileInfo file, RecordProviderResponse response = null)
        {
            String recordXml = File.ReadAllText(file.FullName);

            if (response != null)
            {
                if (recordXml.Contains(RecordType.Name))
                {
                    response.status |= RecordProviderResponseStatus.recordTypeNameFound;
                }
                else
                {
                    response.status |= RecordProviderResponseStatus.recordTypeNameNotFound;
                }
            }

            TRecord output = objectSerialization.ObjectFromXML<TRecord>(recordXml);

            if (response != null)
            {
                response.record = output;

                response.status |= RecordProviderResponseStatus.recordFound;
            }

            return output;
        }

        protected TCollection LoadCollection(DirectoryInfo target, RecordProviderResponse response = null)
        {
            TCollection output = new TCollection();

            if (target == null)
            {
                return output;
            }

            output.name = GetCollectionUID(target.Name);

            var recordFiles = target.GetFiles(GetRecordFilename("*"));

            foreach (var fi in recordFiles)
            {
                var r = LoadRecordFile(fi);
                if (r != null)
                {
                    output.AddOrReplace(r);
                }
            }

            if (response != null)
            {
                response.collection = output;
            }

            return output;
        }

        /// <summary>
        /// Loads entire collection, if found
        /// </summary>
        /// <param name="collection_uid">The collection uid.</param>
        /// <returns></returns>
        public override RecordProviderResponse GetCollection(string collection_uid = "")
        {
            if (OperationMode == RecordProviderOperationMode.multiCollectionMode && collection_uid.isNullOrEmpty())
            {
                throw new ArgumentOutOfRangeException(nameof(collection_uid), EXCEPTION_NOCOLLECTION_ID);
            }

            RecordProviderResponse response = new RecordProviderResponse();

            DirectoryInfo di = GetDirectoryForReference(collection_uid, response);

            LoadCollection(di, response);

            return response;
        }

        // protected void saveRecord(TRecord record, Dire)

        /// <summary>
        /// Saves the collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="delete_all_existing_records">if set to <c>true</c> [delete all existing records].</param>
        /// <param name="response">The response.</param>
        /// <returns></returns>
        public override RecordProviderResponse SaveCollection(TCollection collection, Boolean delete_all_existing_records = false, RecordProviderResponse response = null)
        {
            if (response == null) response = new RecordProviderResponse();

            folderNode targetFolder = GetFolderForCollection(collection.name); // folder.Attach(GetCollectionDirectoryName(collection.name), collection.name, "Storage directory for collection records", false, true);

            if (delete_all_existing_records)
            {
                var existing_records = targetFolder.findFiles(GetRecordFilename("*"));
                foreach (var file in existing_records)
                {
                    File.Delete(file);
                }
            }

            foreach (TRecord record in collection.items)
            {
                String filename = GetRecordFilename(record.GetUID());
                String path = targetFolder.pathFor(filename, Data.enums.getWritableFileMode.overwrite, "Serialized data for [" + record.GetType().Name + "]");

                objectSerialization.saveObjectToXML((TRecord)record, path);
            }

            if (response != null)
            {
                response.Path = targetFolder.path;
                response.status |= RecordProviderResponseStatus.saved;
            }

            return response;
        }

        /// <summary>
        /// Saves the record.
        /// </summary>
        /// <param name="collection_uid">The collection uid.</param>
        /// <param name="record">The record.</param>
        /// <param name="response">The response.</param>
        /// <returns></returns>
        public override RecordProviderResponse SaveRecord(TRecord record, string collection_uid = "", RecordProviderResponse response = null)
        {
            if (response == null) response = new RecordProviderResponse();

            if (OperationMode == RecordProviderOperationMode.multiCollectionMode && collection_uid.isNullOrEmpty())
            {
                throw new ArgumentOutOfRangeException(nameof(collection_uid), EXCEPTION_NOCOLLECTION_ID);
            }

            folderNode targetFolder = GetFolderForCollection(collection_uid);

            String filename = GetRecordFilename(record.GetUID());
            String path = targetFolder.pathFor(filename, Data.enums.getWritableFileMode.overwrite, "Serialized data for [" + record.GetType().Name + "]");

            objectSerialization.saveObjectToXML((TRecord)record, path);
            if (response != null)
            {
                response.Path = path;
                response.status |= RecordProviderResponseStatus.saved;
            }
            return response;
        }

       



        /// <summary>
        /// Gets the record.
        /// </summary>
        /// <param name="collection_uid">The collection uid.</param>
        /// <param name="record_uid">The record uid.</param>
        /// <returns></returns>
        public override RecordProviderResponse GetRecord(String record_uid, string collection_uid = "")
        {
            RecordProviderResponse response = new RecordProviderResponse();

            if (OperationMode == RecordProviderOperationMode.multiCollectionMode && collection_uid.isNullOrEmpty())
            {
                throw new ArgumentOutOfRangeException(nameof(collection_uid), EXCEPTION_NOCOLLECTION_ID);
            }

            DirectoryInfo di = GetDirectoryForReference(collection_uid, response);

            if (di == null) return response;

            FileInfo file = GetRecordFile(di, record_uid);

            if (file == null)
            {
                response.status = RecordProviderResponseStatus.recordNotFound;
                return response;
            }

            LoadRecordFile(file, response);

            return response;
        }
    }
}