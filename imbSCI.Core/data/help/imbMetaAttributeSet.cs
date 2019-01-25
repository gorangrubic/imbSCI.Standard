using imbSCI.Core.attributes;
using imbSCI.Core.files.folders;
using imbSCI.Core.reporting.render;
using imbSCI.Data.collection.nested;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace imbSCI.Core.data.help
{

    public class imbMetaAttributeSet : aceEnumListSet<imbMetaAttributeEnum, imbMetaAttribute>
    {



        /// <summary>
        /// Selects all imbMeta attributes from member info
        /// </summary>
        /// <param name="mi">The mi.</param>
        public void Set(MemberInfo mi)
        {
            var att = mi.GetCustomAttributes(true);
            foreach (var at in att)
            {
                if (at is imbMetaAttribute meta)
                {
                    Add(meta.type, meta);
                }
            }
        }

    }

}