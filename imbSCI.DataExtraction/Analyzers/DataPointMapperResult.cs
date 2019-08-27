using HtmlAgilityPack;
using imbSCI.Core.files.folders;
using imbSCI.Data;
using imbSCI.DataExtraction.Analyzers.Data;
using imbSCI.DataExtraction.Analyzers.Templates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace imbSCI.DataExtraction.Analyzers
{
    /// <summary>
    /// Set of mapping blocks
    /// </summary>
    [Serializable]
    public class DataPointMapperResult
    {
        public void Load(folderNode folder, String filenamePrefix = "datapoints")
        {
            var bl = folder.findFiles(filenamePrefix + "*.xml", System.IO.SearchOption.TopDirectoryOnly);

            foreach (var block in bl)
            {
                var b = DataPointMapBlock.Load(block);
                MapBlocks.Add(b);
            }
        }


        public void Save(folderNode folder, String filenamePrefix = "datapoints", Boolean generateCode=true)
        {
            foreach (var block in MapBlocks)
            {
                String sufix = MapBlocks.IndexOf(block).ToString("D3");

                if (block.name.isNullOrEmpty()) block.name = sufix;

                String p = folder.pathFor(filenamePrefix + block.name + sufix + ".xml", imbSCI.Data.enums.getWritableFileMode.existing);
                block.Save(p);

                if (generateCode)
                {
                    p = folder.pathFor(filenamePrefix + block.name + sufix + ".cs", imbSCI.Data.enums.getWritableFileMode.existing);
                    var c = block.GetCode();

                    File.WriteAllText(p, c);

                    
                }
            }
        }

        public List<DataPointMapBlock> MapBlocks { get; set; } = new List<DataPointMapBlock>();

        public void SetRelativeXPaths()
        {
            foreach (DataPointMapBlock block in MapBlocks)
            {
                block.SetRelativeXPaths();

                
            }
        }

        public void SetLabel(List<HtmlNode> nodes)
        {
            foreach (DataPointMapBlock block in MapBlocks)
            {
                

                block.SetLabels(nodes);
            }
        }

        public Boolean IsValid
        {
            get
            {
                return MapBlocks.Count > 0;
            }
        }

        public DataPointMapperResult()
        {

        }
    }
}