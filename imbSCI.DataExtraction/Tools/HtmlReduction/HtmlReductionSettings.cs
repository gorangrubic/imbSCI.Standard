using imbSCI.Core.files;
using imbSCI.Core.math.classificationMetrics;
using imbSCI.Core.reporting;
using System;
using System.Collections.Generic;
using System.IO;

namespace imbSCI.DataExtraction.Tools.HtmlReduction
{
public class HtmlReductionSettings 
    {
        public HtmlReductionSettings()
        {
        }


        public Boolean InsertReductionSignature { get; set; } = true;

        public Boolean IsEnabled { get; set; } = true;
        public Boolean RemoveComments { get; set; } = true;
        public Boolean ReduceEmptySpace { get; set; } = true;

        public Boolean RebuildHtml { get; set; } = true;

        public static HtmlReductionSettings GetDefaultReductionSettings()
        {
            HtmlReductionSettings output = new HtmlReductionSettings();
            output.tagsToRemove.Add("link");
            output.tagsToRemove.Add("style");
            output.tagsToRemove.Add("script");
            output.tagsToRemove.Add("comment");
            output.tagsToRemove.Add("noscript");
            output.tagsToRemove.Add("meta");
            output.tagsToRemove.Add("img");
            output.tagsToRemove.Add("svg");
            output.tagsToRemove.Add("clippath");
            output.tagsToRemove.Add("defs");
            output.tagsToRemove.Add("iframe");
            output.tagsToRemove.Add("hr");

            output.emptyTagsToRemove.Add("i");
            output.emptyTagsToRemove.Add("b");
            output.emptyTagsToRemove.Add("p");

            output.emptyTagsToRemove.Add("div");
            output.emptyTagsToRemove.Add("span");




            output.attributesToRemove.Add("target");
            output.attributesToRemove.Add("rel");
            output.attributesToRemove.Add("style");
            output.attributesToRemove.Add("alt");
            output.attributesToRemove.Add("onclick");

            output.tagNameReplacement.Add("em", "b");
            output.tagNameReplacement.Add("strong", "b");

            output.attributeWithValueToRemove.Add("href", "javascript:void(0)");

            output.tagsToWrapout.Add("b");
            output.tagsToWrapout.Add("span");
            output.tagsToWrapout.Add("small");
            output.tagsToWrapout.Add("p");
            output.tagsToWrapout.Add("i");

            output.tagsToForceMultilineFormat.Add("#document");
            output.tagsToForceMultilineFormat.Add("body");
            output.tagsToForceMultilineFormat.Add("html");
            output.tagsToForceMultilineFormat.Add("head");
            output.tagsToForceMultilineFormat.Add("section");
            output.tagsToForceMultilineFormat.Add("header");
            output.tagsToForceMultilineFormat.Add("footer");
            output.tagsToForceMultilineFormat.Add("aside");
            output.tagsToForceMultilineFormat.Add("article");
            output.tagsToForceMultilineFormat.Add("main");
            output.tagsToForceMultilineFormat.Add("table");

            output.tagsToRemoveAllAttributes.Add("html");
            output.tagsToRemoveAllAttributes.Add("body");
            output.tagsToRemoveAllAttributes.Add("head");
            output.tagsToRemoveAllAttributes.Add("main");
            

            return output;
        }

        public reportExpandedData tagNameReplacement { get; set; } = new reportExpandedData();

        public reportExpandedData attributeWithValueToRemove { get; set; } = new reportExpandedData();

        public List<String> tagsToWrapout { get; set; } = new List<string>();

        public List<String> tagsToRemove { get; set; } = new List<string>();

        public List<String> tagsToRemoveAllAttributes { get; set; } = new List<string>();

        public List<String> tagsToForceMultilineFormat { get; set; } = new List<string>();

        public List<String> emptyTagsToRemove { get; set; } = new List<string>();

        public List<String> attributesToRemove { get; set; } = new List<string>();
    }
}