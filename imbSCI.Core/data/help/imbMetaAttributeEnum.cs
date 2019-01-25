using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imbSCI.Core.data.help
{

    /// <summary>
    /// Meta attribute type
    /// </summary>
    public enum imbMetaAttributeEnum
    {

        /// <summary>
        /// Attaches external example to a member
        /// </summary>
        AttachExample,

        /// <summary>
        /// Attaches external markdown content to a member 
        /// </summary>
        AttachContent,

        /// <summary>
        /// Defines a static web link
        /// </summary>
        AttachLink,

        /// <summary>
        /// Defines API / Blog search link
        /// </summary>
        AddSearchLink,

        /// <summary>
        /// Defines a property as Plugin / Command console execution context
        /// </summary>
        AddContextProperty,

        /// <summary>
        /// Defines a property as Project instance
        /// </summary>
        AddProjectProperty,

        /// <summary>
        /// Adds inline example, related to the member
        /// </summary>
        AddExampleInLine,

    }

}