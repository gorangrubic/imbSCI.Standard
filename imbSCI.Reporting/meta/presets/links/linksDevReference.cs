// --------------------------------------------------------------------------------------------------------------------
// <copyright file="linksDevReference.cs" company="imbVeles" >
//
// Copyright (C) 2018 imbVeles
//
// This program is free software: you can redistribute it and/or modify
// it under the +terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/.
// </copyright>
// <summary>
// Project: imbSCI.Reporting
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Reporting.meta.presets.links
{
    using imbSCI.Reporting.meta.blocks;
    using imbSCI.Reporting.meta.collection;

    /// <summary>
    /// List of reference manuals
    /// </summary>
    public class linksDevReference : metaLinkCollection
    {
        #region --- topic ------- what development domain is this

        /// <summary>
        /// what development domain is this
        /// </summary>
        public linksDevReferenceDomain topic { get; set; } = linksDevReferenceDomain.standard;

        #endregion --- topic ------- what development domain is this

        public linksDevReference(linksDevReferenceDomain topic)
        {
            loadTopic(topic);
        }

        internal void loadTopic(linksDevReferenceDomain topic)
        {
            type = metaLinkCollectionType.externalWeb;
            rootRelativePath = "\\imbVelesFrameworkDocs";
            switch (topic)
            {
                case linksDevReferenceDomain.standard:
                    AddLink("MSDN:APIs", "Microsoft Developer Network - APIs index page", @"https://msdn.microsoft.com/library");
                    AddLink(".NET library", "Microsoft .NET framework 4.0", @"https://msdn.microsoft.com/en-us/library/w0x726c2(v=vs.100).aspx");
                    AddLink("C# language", "C# language reference", @"https://msdn.microsoft.com/en-us/library/618ayhy6.aspx");
                    AddLink("HtmlAgilityPack", "HTML processing library", @"htmlagility.chm");
                    break;

                case linksDevReferenceDomain.web:
                    AddLink("JS Grid", "JS Grid table plug in", @"http://js-grid.com/demos/");
                    break;

                case linksDevReferenceDomain.reporting:
                    AddLink("PDFSharp", "Library for: text abstract, printing, RTF, png/jpg, meta files EMF, HTML... supports CMYK", @"http://www.pdfsharp.net/");
                    AddLink("Doddle", "Library for: OpenXML, PDF, HTML, CSV.", @"https://doddlereport.codeplex.com/");
                    AddLink("GRAV Markdown", "Learning resource for Markdown syntax", @"https://learn.getgrav.org/content/markdown");
                    AddLink("W3C CSS", "W3schools CSS reference", @"http://www.w3schools.com/tags/tag_head.asp");
                    AddLink("W3C HTML", "W3schools HTML reference", @"http://www.w3schools.com/html/default.asp");
                    AddLink("W3C JS", "W3schools JavaScript reference", @"http://www.w3schools.com/js/default.asp");
                    AddLink("W3C JQUERY", "W3schools JavaScript reference", @"http://www.w3schools.com/js/default.asp");
                    break;

                case linksDevReferenceDomain.semantic:
                    AddLink("RDF Sharp .NET", "Library for RDF and Ontologies", @"https://rdfsharp.codeplex.com/");
                    AddLink("dotNetRDF", "RDF, SPARQL and the Semantic Web", @"https://bitbucket.org/dotnetrdf/dotnetrdf/wiki/UserGuide/Working%20with%20Triple%20Stores");
                    break;

                case linksDevReferenceDomain.nlp:
                    break;

                case linksDevReferenceDomain.xml:
                    break;
            }
        }
    }
}