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
public abstract class reportModelContentBase : IObjectWithName
    {
        public reportRootModel root { get; set; }

        public reportStructureModel node { get; set; }

        public reportElementLevel elementLevel { get; protected set; } = reportElementLevel.document;


        public abstract void RenderPageContent(ITextRender output);

      
        /// <summary>
        /// Gets or sets the assigned identifier.
        /// </summary>
        /// <value>
        /// The assigned identifier.
        /// </value>
        public String path { get; set; }

        /// <summary>
        /// Title for humans
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public String name { get; set; } = "";


    }
}