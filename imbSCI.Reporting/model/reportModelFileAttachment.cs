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
    /// <summary>
    /// Set of files to attach
    /// </summary>
    /// <seealso cref="imbSCI.Reporting.model.reportModelDataContentBase{imbSCI.Reporting.includes.reportIncludeFileCollection}" />
    public class reportModelFileAttachment : reportModelDataContentBase<reportIncludeFileCollection>
    {
        public override void RenderPageContent(ITextRender output)
        {
            throw new NotImplementedException();
        }
    }
}