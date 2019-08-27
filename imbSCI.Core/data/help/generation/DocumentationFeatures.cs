using imbSCI.Core.attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace imbSCI.Core.data.help.generation
{
    public class DocumentationFeatures
    {
        /// <summary>
        /// Gets or sets the snippet options.
        /// </summary>
        /// <value>
        /// The snippet options.
        /// </value>
        public SnippetOptions snippetOptions {get;set;} = new SnippetOptions();


        public ExtensionMethodsOptions extensionsOptions { get; set; } = new ExtensionMethodsOptions();


        public SourceCodeOptions sourceOptions { get; set; } = new SourceCodeOptions();
    }
}