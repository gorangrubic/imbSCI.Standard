using System;

namespace imbSCI.BusinessData.Storage
{
    [Flags]
    public enum RecordProviderResponseStatus
    {
        none = 0,
        notFound = 1,
        Found = 2,
        Record = 4,
        Collection = 8,
        Error = 16,
        RecordTypeName = 32,
        recordFound = Record | Found,
        collectionFound = Collection | Found,
        recordNotFound = Record | notFound,
        collectionNotFound = Collection | notFound,
        recordTypeNameFound = 64,

        /// <summary>
        /// Name of Record Type was not found in loaded XML file
        /// </summary>
        recordTypeNameNotFound = 128,

        saved = 256
    }
}