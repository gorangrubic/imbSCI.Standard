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
#pragma warning disable CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning disable CS1584 // XML comment has syntactically incorrect cref attribute 'imbSCI.Reporting.model.reportModelDataContentBase{imbSCI.Reporting.includes.reportIncludeFileCollection}'
    /// <summary>
    /// Set of files to attach
    /// </summary>
    /// <seealso cref="imbSCI.Reporting.model.reportModelDataContentBase{imbSCI.Reporting.includes.reportIncludeFileCollection}" />
    public class reportModelFileAttachment : reportModelDataContentBase<reportIncludeFileCollection>
#pragma warning restore CS1584 // XML comment has syntactically incorrect cref attribute 'imbSCI.Reporting.model.reportModelDataContentBase{imbSCI.Reporting.includes.reportIncludeFileCollection}'
#pragma warning restore CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
    {
        public override void RenderPageContent(ITextRender output)
        {
            throw new NotImplementedException();
        }
    }
}