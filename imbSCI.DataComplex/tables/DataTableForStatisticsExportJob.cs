using System;

namespace imbSCI.DataComplex.tables
{
    using imbSCI.Core.data;
    using imbSCI.Core.files.folders;
    using System.Data;

    /// <summary>
    /// Temporary object - handling report export process
    /// </summary>
    public class DataTableForStatisticsExportJob
    {
        private DataTable source;
        private folderNode folder;
        private aceAuthorNotation notation;
        private String filenamePrefix;
        private Boolean disablePrimaryKey;

        public DataTableForStatisticsExportJob(DataTable _source, folderNode _folder, aceAuthorNotation _notation = null, string _filenamePrefix = "", bool _disablePrimaryKey = true)
        {
            source = _source;
            folder = _folder;
            notation = _notation;
            filenamePrefix = _filenamePrefix;
            disablePrimaryKey = _disablePrimaryKey;
        }

        public void Do()
        {
            source.GetReportAndSave(folder, notation, filenamePrefix, disablePrimaryKey, false);
        }
    }
}