using imbSCI.Core.data;
using imbSCI.Core.files;
using imbSCI.Core.files.folders;
using imbSCI.DataComplex.tables;
using imbSCI.DataExtraction.MetaTables;
using imbSCI.DataExtraction.MetaTables.Core;
using imbSCI.DataExtraction.SourceData;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml.Serialization;

namespace imbSCI.DataExtraction.Extractors.Task
{


    [Serializable]
    public class TableExtractionChain
    {

        public static List<TableExtractionChain> CreateChains(IEnumerable<SourceTable> sourceTables)
        {
            List<TableExtractionChain> output = new List<TableExtractionChain>();

            foreach (var s in sourceTables)
            {
                output.Add(new TableExtractionChain()
                {
                    source = s
                });
            }

            return output;
        }


        private static Object _authorInfo_lock = new Object();
        private static aceAuthorNotation _authorInfo;
        /// <summary>
        /// static and autoinitiated object
        /// </summary>
        public static aceAuthorNotation authorInfo
        {
            get
            {
                if (_authorInfo == null)
                {
                    lock (_authorInfo_lock)
                    {

                        if (_authorInfo == null)
                        {
                            _authorInfo = new aceAuthorNotation();
                            
                            // add here if any additional initialization code is required
                        }
                    }
                }
                return _authorInfo;
            }
        }


        public void Publish(folderNode outputFolder,ExtractionResultPublishFlags flags, String name_prefix="")
        {
            source.Publish(outputFolder, flags, name_prefix, authorInfo);
            meta.Publish(outputFolder, flags, name_prefix, authorInfo);
            

            if (data!=null)
            {
                var metap = outputFolder.pathFor(name_prefix + "data.xml", imbSCI.Data.enums.getWritableFileMode.autoRenameThis, "Exported source table");
               if (flags.HasFlag(ExtractionResultPublishFlags.dataTableSerialization)) objectSerialization.saveObjectToXML(data, metap);

                String fl = Path.GetFileNameWithoutExtension(metap);

              if (flags.HasFlag(ExtractionResultPublishFlags.dataTableExcel)) data.GetReportAndSave(outputFolder, authorInfo, fl);
            }

            
        }

        [XmlIgnore]
        public SourceTable source { get; set; }

        [XmlIgnore]
        public MetaTable meta { get; set; }

        public DataTable data { get; set; }

        public string name { get; set; }

        public TableExtractionChain()
        {

        }
    }
}