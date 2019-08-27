using imbSCI.BusinessData.Core;

namespace imbSCI.BusinessData.Storage
{
    public class RecordProviderResponse
    {
        public RecordProviderResponse()
        {
        }

        public RecordProviderResponseStatus status = RecordProviderResponseStatus.none;

        public IRecordWithGetUID record;

        public IRecordsWithUIDCollection collection;

        /// <summary>
        /// Relevant path information (filepath, directory path, URI or other kind of location description
        /// </summary>
        /// <value>
        /// The filepath.
        /// </value>
        public string Path { get; set; } = "";
    }
}