using imbSCI.Core.files.folders;
using imbSCI.Core.reporting.render;
using imbSCI.Core.reporting.render.builders;
using imbSCI.DataComplex.tables;
using System.Data;

namespace imbSCI.Reporting.delivery
{
    public class builderForFiles : builderForHtml, ITextRender
    {

        public builderForFiles(folderNode _folder)
        {
            folder = _folder;
        }
        public folderNode folder { get; set; }

        public override void AppendTable(DataTable table, bool doThrowException = true)
        {
            table.GetReportAndSave(folder, null, "");

        }

    }

    /*
    public class builderForGutenberg : imbStringBuilderBase, ITextRender
    {
        // private converterBase _converter;

        public override converterBase converter
        {
            get
            {
                if (_converter == null) _converter = new converterForBootstrap3();
                return _converter;
            }
        }

        public object addDocument(string name, bool scopeToNew = true, getWritableFileMode mode = getWritableFileMode.autoRenameExistingOnOtherDate, reportOutputFormatName format = reportOutputFormatName.none)
        {
            return null;
        }

        public object addPage(string name, bool scopeToNew = true, getWritableFileMode mode = getWritableFileMode.autoRenameThis, reportOutputFormatName format = reportOutputFormatName.none)
        {
            String c = "<!-- wp:nextpage --><!--nextpage--><!-- /wp:nextpage -->";
            _AppendLine(c);

            return c;
        }

        /// <summary>
        /// Appends the image tag/call.
        /// </summary>
        /// <param name="imageSrc">Source url/path of the image</param>
        /// <param name="imageAltText">The image alt text.</param>
        /// <param name="imageRef">Caption below the image</param>
        public override void AppendImage(string imageSrc, string imageAltText, string imageRef)
        {
            String c = "<!-- wp:media-text {\"mediaId\":13,\"mediaType\":\"image\"} --><div class=\"wp-block-media-text alignwide\"><figure class=\"wp-block-media-text__media\">";
            c += "<img src=\"" + imageSrc + "\" alt =\"" + imageAltText + "\" class=\"wp-image-13\"/></figure><div class=\"wp-block-media-text__content\">";

            c += "<p class=\"has-large-font-size\">" + imageRef + "</p><!-- /wp:paragraph --></div></div><!-- /wp:media-text -->";
            _AppendLine(c);
        }

        public override void AppendLabel(string content, bool isBreakLine = true, object comp_style = null)
        {
            String c = "";


            c = "<label>" + content + "</label>";
            if (isBreakLine)
            {
                c = "<p>" + c + "</p>";
            }

            _AppendLine(c);

        }

        public override void AppendPair(string key, object value, bool breakLine = true, string between = ": ")
        {
            String c = "<strong>" + key + "</strong>";

            c += between;
            c += "<var>" + value.toStringSafe("") + "</var>";


            _AppendLine(c);

        }

        public override void AppendPlaceholder(object fieldName, bool breakLine = false)
        {
            base.AppendPlaceholder(fieldName, breakLine);
        }

        public override void AppendLink(string url, string name, string caption = "", appendLinkType linkType = appendLinkType.unknown)
        {
            String c = "<a href=\"" + url + "\">" + name + "</a>";

            if (!caption.isNullOrEmpty())
            {
                c += "<label>" + caption + "</label>" + c;
            }

            _AppendLine(c);

        }



        public override void AppendList(IEnumerable<object> content, bool isOrderedList = false)
        {
            StringBuilder b = new StringBuilder();
            String list_tag = "ul";
            if (isOrderedList)
            {
                list_tag = "ol";
            }

            b.AppendLine("<!-- wp:list -->");

            b.AppendLine("<" + list_tag + ">");

            imbStringHTMLExtensions.HtmlList(content, isOrderedList, b, 0);

            b.AppendLine("</" + list_tag + ">");

            b.AppendLine("<!-- /wp:list -->");


            _AppendLine(b.ToString());


            //base.AppendList(content, isOrderedList);
        }

        public override void AppendLine(string content = "")
        {
            base.AppendLine(content);
        }

        public override void AppendTable(DataTable table, bool doThrowException = true)
        {
            String htmlTable = imbStringHTMLExtensions.htmlTable(table, "wp-block-table is-style-stripes", true);
            String c = "<!-- wp:table {\"className\":\" is-style-stripes\"} -->" + htmlTable + "<!-- /wp:paragraph -->";
            _AppendLine(c);
        }

        public override object AppendParagraph(string content, bool fullWidth = false)
        {
            String c = "<!-- wp:paragraph --><p>" + content + "</p><!-- /wp:paragraph -->";
            _AppendLine(c);

            return c;
        }

        public override object AppendCite(string content)
        {
            String c = "<!-- wp:quote --><blockquote class=\"wp-block-quote\"><p>" + content + "</p></blockquote><!-- /wp:quote -->";
            _AppendLine(c);

            return c;
        }

        public override object AppendHeading(string content, int level = 1)
        {
            String c = "<!-- wp:heading --><h" + level.ToString() + "> " + content + " </h" + level.ToString() + "><!-- /wp:heading -->";
            _AppendLine(c);

            return c;
        }

        public override void AppendHorizontalLine()
        {
            String c = "<!-- wp:separator --><hr class=\"wp-block-separator\"/><!-- /wp:separator -->";
            _AppendLine(c);
        }

        public void AppendLine()
        {
            String c = "<!-- wp:separator --><hr class=\"wp-block-separator\"/><!-- /wp:separator -->";
            _AppendLine(c);
        }

        public override void AppendMath(string mathFormula, string mathFormat = "asciimath")
        {
            throw new NotImplementedException();
        }

        public override void AppendPanel(string content, string comp_heading = "", string comp_description = "", object comp_style = null)
        {
            String c = "<!--wp:uagb / section { \"block_id\":\"10768f65-ae2e-468a-8483-c5ebce93530d\"} --><section class=\"wp-block-uagb-section uagb-section__wrap uagb-section__background-undefined\" id=\"uagb-section-10768f65-ae2e-468a-8483-c5ebce93530d\">";
            c += "<div class=\"uagb-section__overlay\"></div><div class=\"uagb-section__inner-wrap\">";

            _AppendLine(c);

            AppendHeading(comp_heading, 4);

            AppendParagraph(content);

            c = "</section><!-- /wp:uagb/section -->";
            _AppendLine(c);
        }

        public FileInfo savePage(string name, reportOutputFormatName format = reportOutputFormatName.none)
        {
            return null;
        }
    }*/
}
