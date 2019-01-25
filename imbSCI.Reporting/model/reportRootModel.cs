using imbSCI.Core.extensions.data;
using imbSCI.Core.files.folders;
using imbSCI.Core.reporting.render;
using imbSCI.Data.collection;
using imbSCI.Data.collection.graph;
using imbSCI.Data.enums;
using imbSCI.Data.extensions.data;
using imbSCI.Data.interfaces;
using imbSCI.Reporting.includes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace imbSCI.Reporting.model
{
public abstract class reportRootModel : reportModelContentBase
    {

        public reportStructureModel StructureModel { get; set; }

        public Relationships<reportModelContentBase, reportModelContentBase> relations { get; set; } = new Relationships<reportModelContentBase, reportModelContentBase>();

        public void Register(reportModelContentBase content, String path)
        {
            content.root = this;
            content.node = graphTools.ConvertPathToGraph<reportStructureModel>(StructureModel, path, true, StructureModel.pathSeparator, true);
            content.node.instance = content;
            //.ConvertPathToGraph

        }


        protected reportRootModel(folderNode _folder, String _name)
        {
            deployFolders(_folder, _name);
        }


        protected void deployFolders(folderNode _folder, String _name)
        {
            folder = _folder;
            name = _name;
            StructureModel = new reportStructureModel(name);
            node = StructureModel;

            folder_include = folder.Add("include", "include", "globally included resources");
        }

        public folderNode folder { get; set; }

        public folderNode folder_include { get; set; }


    }
}
