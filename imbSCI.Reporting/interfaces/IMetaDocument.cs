//namespace imbReportingCore.interfaces
//{
//    using System;
// using aceCommonTypes.core.exceptions;
//    using aceCommonTypes.reporting.format;
//    using aceCommonTypes.reporting.style.core;
//    using imbReportingCore.meta.blocks;
//    using imbReportingCore.meta.document;
//    using imbReportingCore.meta.page;
//    using imbReportingCore.script;

//    /// <summary>
//    ///
//    /// </summary>
//    /// <seealso cref="imbReportingCore.interfaces.IMetaComposeAndConstruct" />
//    /// <seealso cref="imbReportingCore.interfaces.IMetaContentNested" />
//    public interface IMetaDocument:IMetaComposeAndConstruct, IMetaContentNested
//    {
//        /// <summary>
//        /// Gets the theme.
//        /// </summary>
//        /// <value>
//        /// The theme.
//        /// </value>
//        styleTheme theme { get; }

//        /// <summary>
//        /// Common part of composing
//        /// </summary>
//        /// <param name="script">The script.</param>
//        /// <returns></returns>
//        docScript baseCompose(docScript script);

//        /// <summary>
//        /// Common part of construct
//        /// </summary>
//        /// <param name="resources">The resources.</param>
//        void baseConstruct(params object[] resources);

//        /// <summary>
//        /// Gets the document bottom line.
//        /// </summary>
//        /// <value>
//        /// The document bottom line.
//        /// </value>
//        String documentBottomLine { get; }
//        /// <summary>
//        /// Gets the document description.
//        /// </summary>
//        /// <value>
//        /// The document description.
//        /// </value>
//        String documentDescription { get; }
//        /// <summary>
//        /// Gets the document title.
//        /// </summary>
//        /// <value>
//        /// The document title.
//        /// </value>
//        String documentTitle { get; }

//        /// <summary>
//        /// Gets the header.
//        /// </summary>
//        /// <value>
//        /// The header.
//        /// </value>
//        metaHeader header { get; }

//        /// <summary>
//        /// Gets the footer.
//        /// </summary>
//        /// <value>
//        /// The footer.
//        /// </value>
//        metaFooter footer { get; }

//        /// <summary>
//        /// Pages that are part of this document
//        /// </summary>
//        /// <value>
//        /// The pages.
//        /// </value>
//        metaPageCollection pages { get; }
//    }

//}