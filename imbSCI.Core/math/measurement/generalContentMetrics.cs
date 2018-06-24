// --------------------------------------------------------------------------------------------------------------------
// <copyright file="generalContentMetrics.cs" company="imbVeles" >
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
// Project: imbSCI.Core
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Core.math.measurement
{
    using imbSCI.Core.attributes;
    using imbSCI.Core.enums;
    using imbSCI.Core.math.measureSystem;
    using imbSCI.Core.math.range;
    using System.ComponentModel;

    /// <summary>
    /// Page content metrics
    /// </summary>
    /// <seealso cref="measureSetBase" />
    public class generalContentMetrics : measureSetBase
    {
        public generalContentMetrics() : base("Page content metrics", "Tokenized content statistics and relationship between the most variant parameters.")
        {
        }

        /// <summary>
        /// HNusspel recognized d
        /// </summary>
        [DisplayName("urlAddress")]
        [Description("URL address of page loaded")]
        [imb(imbAttributeName.measure_displayGroup, "Elements")]
        [imb(imbAttributeName.measure_displayGroupDescripton, "Elements statictics")]
        [imb(imbAttributeName.measure_metaModelName, "urlAddress")]
        [imb(imbAttributeName.measure_metaModelPrefix, "FV01")]
        [imb(imbAttributeName.measure_optimizeUnit, true)]
        [imb(imbAttributeName.measure_calcGroup, 0)]
        [imb(imbAttributeName.measure_setAlarm, 0, 1, 0, rangeCriteriaEnum.exactEven)]
        public numericCountMeasure urlAddress => this["Elements", "urlAddress"] as numericCountMeasure;

        //-----------------------------------------------------------------------------------//

        /// <summary>
        /// Blocks on the page - count
        /// </summary>
        [imb(imbAttributeName.measure_displayGroup, "Elements")]
        [imb(imbAttributeName.measure_displayGroupDescripton, "Content structure elements statictics")]
        [DisplayName("Blocks")]
        [Description("Blocks on the page - count")]
        [imb(imbAttributeName.viewPriority, 20)]
        [imb(imbAttributeName.measure_calcGroup, 0)]
        [imb(imbAttributeName.measure_setAlarm, 0, 1, 0, rangeCriteriaEnum.exactEven)]
        public numericCountMeasure blocks => this["Elements", "blocks"] as numericCountMeasure;

        [DisplayName("Paragraphs")]
        [Description("Total number of tokens in the structure")]
        [imb(imbAttributeName.measure_optimizeUnit, true)]
        [imb(imbAttributeName.measure_setAlarm, 0, 1, 0, rangeCriteriaEnum.exactEven)]
        public numericCountMeasure paragraph => this["Elements", "paragraph"] as numericCountMeasure;

        [DisplayName("Sentences")]
        [Description("Total number of sentences in the structure")]
        [imb(imbAttributeName.measure_optimizeUnit, true)]
        [imb(imbAttributeName.measure_calcGroup, 0)]
        [imb(imbAttributeName.measure_setAlarm, 0, 1, 0, rangeCriteriaEnum.exactEven)]
        public numericCountMeasure sentences => this["Elements", "sentences"] as numericCountMeasure;

        [DisplayName("Tokens")]
        [Description("Total number of tokens in the structure")]
        [imb(imbAttributeName.measure_optimizeUnit, true)]
        [imb(imbAttributeName.measure_setAlarm, 0, 1, 0, rangeCriteriaEnum.exactEven)]
        public numericCountMeasure tokens => this["Elements", "tokens"] as numericCountMeasure;

        /// <summary>
        /// Number of HNusspel recongized words for the test language {{{lang_eName}}} ({{{lang_iso}}}
        /// </summary>
        [DisplayName("hnusspel_language_Match")]
        [Description("Number of HNusspel recongized words for the test language {{{lang_eName}}} ({{{lang_iso}}}")]
        [imb(imbAttributeName.measure_displayGroup, "Language")]
        [imb(imbAttributeName.measure_displayGroupDescripton, "Results of language tests against the tokens")]
        [imb(imbAttributeName.measure_optimizeUnit, true)]
        [imb(imbAttributeName.measure_calcGroup, 0)]
        [imb(imbAttributeName.measure_setAlarm, 0, 1, 0, rangeCriteriaEnum.exactEven)]
        public numericCountMeasure hnusspel_language_Match => this["Elements", "hnusspel_language_Match"] as numericCountMeasure;

        /// <summary>
        /// Normal words of a language
        /// </summary>
        [DisplayName("language_word_count")]
        [Description("Normal words of a language")]
        [imb(imbAttributeName.measure_optimizeUnit, true)]
        [imb(imbAttributeName.measure_calcGroup, 0)]
        [imb(imbAttributeName.measure_setAlarm, 0, 1, 0, rangeCriteriaEnum.exactEven)]
        public numericCountMeasure language_word_count => this["language_word_count"] as numericCountMeasure;

        /// <summary>
        /// Ratio: number of [words hnusspel recognized] / [total word count]
        /// </summary>
        [DisplayName("hnusspel_word_ratio")]
        [Description("imbEntity kolekcija")]
        [imb(imbAttributeName.measure_optimizeUnit, true)]
        [imb(imbAttributeName.measure_calcGroup, 1)]
        [imb(operation.assign, nameof(hnusspel_language_Match), operation.division, nameof(language_word_count))]
        public percentMeasure hnusspel_word_ratio => this["hnusspel_word_ratio"] as percentMeasure;

        // ------------------------------------------------------------------------------------------------------------------ //

        /// <summary>
        /// Number of links pointing to another TLD
        /// </summary>
        [DisplayName("linkOutCount")]
        [Description("Number of links pointing to another TLD")]
        [imb(imbAttributeName.measure_displayGroup, "Links")]
        [imb(imbAttributeName.measure_displayGroupDescripton, "Links statictics")]
        [imb(imbAttributeName.measure_optimizeUnit, true)]
        [imb(imbAttributeName.measure_calcGroup, 0)]
        [imb(imbAttributeName.measure_setAlarm, 0, 1, 0, rangeCriteriaEnum.exactEven)]
        [imb(imbAttributeName.measure_metaModelName, "linkOutCount")]
        [imb(imbAttributeName.measure_metaModelPrefix, "FV01")]
        public numericCountMeasure linkOutCount => this["Links", "linkOutCount"] as numericCountMeasure;

        /// <summary>
        /// Number of links leading to pages inside the same TLD
        /// </summary>
        [DisplayName("linkInCount")]
        [Description("Number of links leading to pages inside the same TLD or pointing to an arnchor on the page")]
        [imb(imbAttributeName.measure_metaModelName, "linkInCount")]
        [imb(imbAttributeName.measure_metaModelPrefix, "FV02")]
        [imb(imbAttributeName.measure_optimizeUnit, true)]
        [imb(imbAttributeName.measure_calcGroup, 0)]
        [imb(imbAttributeName.measure_setAlarm, 0, 1, 0, rangeCriteriaEnum.exactEven)]
        public numericCountMeasure linkInCount => this["Links", "linkInCount"] as numericCountMeasure;

        /// <summary>
        /// Total number of links on page, any kind of (including: on-page-anchor-links, inner pages and out-of-the-TLD links, excluding: multimedia srcs,  css, js and other source includes)
        /// </summary>
        [DisplayName("linkInOutCount")]
        [Description("Total number of links on page, any kind of (including: on-page-anchor-links, inner pages and out-of-the-TLD links, excluding: multimedia srcs,  css, js and other source includes)")]
        [imb(imbAttributeName.measure_metaModelName, "linkInOutCount")]
        [imb(imbAttributeName.measure_metaModelPrefix, "FV03")]
        [imb(imbAttributeName.measure_optimizeUnit, true)]
        [imb(imbAttributeName.measure_calcGroup, 1)]
        [imb(imbAttributeName.measure_setAlarm, 0, 1, 0, rangeCriteriaEnum.exactEven)]
        [imb(operation.assign, nameof(linkOutCount), operation.plus, nameof(linkInCount))]
        public numericCountMeasure linkInOutCount => this["Links", "linkInOutCount"] as numericCountMeasure;

        /// <summary>
        /// Inner links with title having hnuspell-test positive words
        /// </summary>
        [DisplayName("linkWithKnownWords")]
        [Description("Inner links with title having hnuspell-test positive words")]
        [imb(imbAttributeName.measure_displayGroup, "Links")]
        [imb(imbAttributeName.measure_metaModelName, "linkWithKnownWords")]
        [imb(imbAttributeName.measure_metaModelPrefix, "FV05")]
        [imb(imbAttributeName.measure_optimizeUnit, true)]
        [imb(imbAttributeName.measure_calcGroup, 0)]
        [imb(imbAttributeName.measure_setAlarm, 0, 1, 0, rangeCriteriaEnum.exactEven)]
        public numericCountMeasure linkWithKnownWords => this["Links", "linkWithKnownWords"] as numericCountMeasure;

        //--------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Title Sentence total Count
        /// </summary>
        [DisplayName("Titles Count")]
        [Description("Total on-page number of sentences flaged as Title Sentence")]
        [imb(imbAttributeName.measure_displayGroup, "Element analysis")]
        [imb(imbAttributeName.measure_displayGroupDescripton, "Figures derived after first-stage linear analysis algorithm.")]
        [imb(imbAttributeName.measure_metaModelName, "sentenceTitleCount")]
        [imb(imbAttributeName.measure_metaModelPrefix, "FV21")]
        [imb(imbAttributeName.measure_optimizeUnit, true)]
        [imb(imbAttributeName.measure_calcGroup, 0)]
        [imb(imbAttributeName.measure_setAlarm, 0, 1, 0, rangeCriteriaEnum.exactEven)]
        public numericCountMeasure sentenceTitleCount => this["Element analysis", "sentenceTitleCount"] as numericCountMeasure;

        /// <summary>
        /// Normal sentence total count
        /// </summary>
        [DisplayName("Normals Count")]
        [Description("Total on-page number of sentences flaged as Normal Sentence")]
        [imb(imbAttributeName.measure_metaModelName, "sentenceNormalCount")]
        [imb(imbAttributeName.measure_metaModelPrefix, "FV22")]
        [imb(imbAttributeName.measure_optimizeUnit, true)]
        [imb(imbAttributeName.measure_calcGroup, 0)]
        [imb(imbAttributeName.measure_setAlarm, 0, 1, 0, rangeCriteriaEnum.exactEven)]
        public numericCountMeasure sentenceNormalCount => this["Element analysis", "sentenceNormalCount"] as numericCountMeasure;

        /// <summary>
        /// Ratio: [links count (any kind of)] / [Normal sentence count]
        /// </summary>
        [DisplayName("linkPerSentenceNormal")]
        [Description("Ratio: [links count (any kind of)] / [Normal sentence count]")]
        [imb(imbAttributeName.measure_metaModelName, "linkPerSentenceNormal")]
        [imb(imbAttributeName.measure_metaModelPrefix, "FR21")]
        [imb(imbAttributeName.measure_optimizeUnit, true)]
        [imb(imbAttributeName.measure_calcGroup, 1)]
        [imb(imbAttributeName.measure_setAlarm, 0, 1, 0, rangeCriteriaEnum.exactEven)]
        [imb(operation.assign, nameof(linkInOutCount), operation.division, nameof(sentenceNormalCount))]
        public numericCountMeasure linkPerSentenceNormal => this["Element analysis", "linkPerSentenceNormal"] as numericCountMeasure;

        /// <summary>
        /// Ratio: [word count (any kind of)] / [Normal sentence count]
        /// </summary>
        [DisplayName("wordPerSentenceNormal")]
        [Description("Ratio: [word count (confirmed by hnusspel)] / [Normal sentence count] ")]
        [imb(imbAttributeName.measure_metaModelName, "wordPerSentenceNormal")]
        [imb(imbAttributeName.measure_metaModelPrefix, "FV22")]
        [imb(imbAttributeName.measure_optimizeUnit, true)]
        [imb(imbAttributeName.measure_calcGroup, 1)]
        [imb(imbAttributeName.measure_setAlarm, 0, 1, 0, rangeCriteriaEnum.exactEven)]
        [imb(operation.assign, nameof(language_word_count), operation.division, nameof(sentenceNormalCount))]
        public numericCountMeasure wordPerSentenceNormal => this["Element analysis", "wordPerSentenceNormal"] as numericCountMeasure;

        /// <summary>
        /// Ratio: [Title sentence count)] / [Normal sentence count]
        /// </summary>
        [DisplayName("titlePerSentenceNormal")]
        [Description("Ratio: [Title sentence count)] / [Normal sentence count]")]
        [imb(imbAttributeName.measure_metaModelName, "titlePerSentenceNormal")]
        [imb(imbAttributeName.measure_metaModelPrefix, "FV23")]
        [imb(imbAttributeName.measure_optimizeUnit, true)]
        [imb(imbAttributeName.measure_calcGroup, 1)]
        [imb(imbAttributeName.measure_setAlarm, 0, 1, 0, rangeCriteriaEnum.exactEven)]
        [imb(operation.assign, nameof(sentenceTitleCount), operation.division, nameof(sentenceNormalCount))]
        public numericCountMeasure titlePerSentenceNormal => this["Element analysis", "titlePerSentenceNormal"] as numericCountMeasure;

        /// <summary>
        /// Count of normal sentences in the block with the highest count of normal sentences
        /// </summary>
        [DisplayName("Sent. normal in HSNC Block")]
        [Description("Count of normal sentences in the block with the highest count of normal sentences. HSNC Block is the block with Highest SentenceNormal Count")]
        [imb(imbAttributeName.measure_metaModelName, "sentenceNormalCountInHSNCBlock")]
        [imb(imbAttributeName.measure_metaModelPrefix, "FV24")]
        [imb(imbAttributeName.measure_optimizeUnit, true)]
        [imb(imbAttributeName.measure_calcGroup, 0)]
        [imb(imbAttributeName.measure_setAlarm, 0, 1, 0, rangeCriteriaEnum.exactEven)]
        public numericCountMeasure sentenceNormalCountInHSNCBlock => this["Element analysis", "sentenceNormalCountInHSNCBlock"] as numericCountMeasure;

        /// <summary>
        /// Count of inner links in the block with the highest count of inner links
        /// </summary>
        [DisplayName("Links in HILC Block")]
        [Description("Count of inner links in the block with the highest count of inner links. HILC Block is the block with the Highest InnerLink Count")]
        [imb(imbAttributeName.measure_metaModelName, "linkInCountInHILCBlock")]
        [imb(imbAttributeName.measure_metaModelPrefix, "FV25")]
        [imb(imbAttributeName.measure_optimizeUnit, true)]
        [imb(imbAttributeName.measure_calcGroup, 0)]
        [imb(imbAttributeName.measure_setAlarm, 0, 1, 0, rangeCriteriaEnum.exactEven)]
        public numericCountMeasure linkInCountInHILCBlock => this["Elements", "linkInCountInHILCBlock"] as numericCountMeasure;
    }
}