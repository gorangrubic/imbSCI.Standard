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
    /// Operation mode
    /// </summary>
    public enum RecordProviderOperationMode
    {
        /// <summary>
        /// The single collection mode: only one collection is subject of the provider, serving its records
        /// </summary>
        singleCollectionMode,
        /// <summary>
        /// The multi collection mode: provider serves multiple collections, with records in each one
        /// </summary>
        multiCollectionMode
    }
}