using imbSCI.Core.data;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Core.reporting.format;
using imbSCI.Core.reporting.render.converters;
using imbSCI.Core.reporting.render.core;
using imbSCI.Core.reporting.zone;
using imbSCI.Data;
using imbSCI.Data.enums;
using imbSCI.Data.enums.appends;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace imbSCI.Core.reporting.render.builders
{
/// <summary>
    /// 
    /// </summary>
    public interface ICodeRender
    {
        void AppendUsing(List<String> namespaces);

        void OpenClass(string classname, string accessLevel = "public", String description = "");

        void CloseClass();

        void OpenNamespace(string namespaceName);

        void OpenMethod(CodeMethodBlockInfo methodInfo);

        void CloseMethod();

        void AppendProperty(settingsPropertyEntry property, PropertyAppendType type, PropertyAppendFlags flags);

        void CloseNamespace();
    }
}