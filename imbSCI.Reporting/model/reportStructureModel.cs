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
public class reportStructureModel : graphNodeCustom
    {
        public reportStructureModel(String _name)
        {
            name = _name;
            pathSeparator = Path.DirectorySeparatorChar.ToString();
        }

        public reportModelContentBase instance { get; set; }

        public reportStructureModel()
        {

        }

        protected override bool doAutorenameOnExisting => false;

        protected override bool doAutonameFromTypeName => false;
    }
}