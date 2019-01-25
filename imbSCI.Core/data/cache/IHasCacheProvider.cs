using imbSCI.Core.data.systemWatch;
using imbSCI.Core.extensions.io;
using imbSCI.Core.reporting.render;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace imbSCI.Core.data.cache
{
    /// <summary>
    /// Classes that use cache provider
    /// </summary>
    public interface IHasCacheProvider
    {
        void SetCacheProvider(CacheServiceProvider _CacheProvider);
    }
}