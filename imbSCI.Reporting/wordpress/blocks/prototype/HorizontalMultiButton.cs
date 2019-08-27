using imbSCI.Core.reporting.render;
using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.Reporting.wordpress.blocks.prototype
{
    public class HorizontalMultiButton : MultiItemContentBlockBase
    {
        public HorizontalMultiButton()
        {
            Deploy();
        }

        public override void Deploy()
        {
            innerTemplate = "<div class=\"wp-block-kadence-advancedbtn kt-btn-align-center kt-btn-tablet-align-inherit kt-btn-mobile-align-inherit kt-btns-wrap kt-btns_d6230c-57 kt-force-btn-fullwidth\">" +
            "{0}</div>";

            itemTemplate = "<div class=\"kt-btn-wrap kt-btn-wrap-0\">" +
            "<a class=\"kt-button kt-btn-0-action kt-btn-size-standard kt-btn-style-basic kt-btn-svg-show-always kt-btn-has-text-false kt-btn-has-svg-false\" href=\"{0}\">" +
            "<span class=\"kt-btn-inner-text\">{1}</span></a></div>";
        }


        public String Render<T>(IEnumerable<T> items, Func<T, String> ButtonLabel, Func<T, String> ButtonLink)
        {
            StringBuilder sb = new StringBuilder();

            foreach (T item in items)
            {
                String label = ButtonLabel(item);
                String link = ButtonLink(item);

                sb.AppendFormat(itemTemplate, link, label);
            }


            String content = WrapInTemplate(innerTemplate, sb.ToString());
            content = WrapInTemplate(outerTemplate, content);
            return content;
        }

        public void RenderTo<T>(ITextRender builder, IEnumerable<T> items, Func<T, String> ButtonLabel, Func<T, String> ButtonLink)
        {
            String content = Render<T>(items, ButtonLabel, ButtonLink);
            builder.AppendDirect(content);
        }
    }
}
