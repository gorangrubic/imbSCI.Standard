using System;

namespace imbSCI.Reporting.wordpress.blocks.prototype
{
    public abstract class MultiItemContentBlockBase : ReportContentBlockBase
    {
        /// <summary>
        /// Template for a item
        /// </summary>
        /// <value>
        /// The item template.
        /// </value>
        public String itemTemplate { get; set; }
            = "<div class=\"kt-btn-wrap kt-btn-wrap-0\">" +
            "<a class=\"kt-button kt-btn-0-action kt-btn-size-standard kt-btn-style-basic kt-btn-svg-show-always kt-btn-has-text-false kt-btn-has-svg-false\" href=\"{0}\">" +
            "<span class=\"kt-btn-inner-text\">{1}</span></a></div>";

    }
}