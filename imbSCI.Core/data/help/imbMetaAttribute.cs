using imbSCI.Core.attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace imbSCI.Core.data.help
{



    /// <summary>
    /// Defines additional meta information for documentation generation
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class |
                AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true)]
    public class imbMetaAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="imbMetaAttribute"/> class.
        /// </summary>
        public imbMetaAttribute() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="imbMetaAttribute"/> class.
        /// </summary>
        /// <param name="_type">The type.</param>
        /// <param name="_path">The path.</param>
        /// <param name="_description">The description.</param>
        public imbMetaAttribute(imbMetaAttributeEnum _type, String _path, String _description, String _caption="")
        {
            path = _path;
            description = _description;
            caption = _caption;
            type = _type;
        }

        /// <summary>
        /// Path leading to external content. For search link, it must contain {0} place holder
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        public String path { get; set; } = "";

        public String caption { get; set; } = "";

        /// <summary>
        /// Additional description
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public String description { get; set; } = "";

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public imbMetaAttributeEnum type { get; set; } = imbMetaAttributeEnum.AttachContent;

    }
}
