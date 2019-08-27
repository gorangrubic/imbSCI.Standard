using imbSCI.Core.reporting;
using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.Core.data.providers
{
    /// <summary>
    /// Interface for type provider classes. <see cref="UniversalTypeProvider{TInterface}"/>
    /// </summary>
    public interface ITypeProvider
    {
        void Prepare(ILogBuilder logger);

        Type GetTypeByName(String classname);
        
    }
}
