using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.Core.style.css
{
    /// <summary>
    /// HTML tags, https://www.w3schools.com/tags/default.asp
    /// </summary>
    public enum htmlTagEnum
    {
        ///<summary>Anchor</summary>
        a,
        ///<summary>Defines an abbreviation or an acronym</summary>
        abbr,
        ///<summary>Not supported in HTML5. Use abbr instead.Defines an acronym</summary>
acronym,
        ///<summary>Defines contact information for the author/owner of a document</summary>
        address,
        ///<summary>Not supported in HTML5. Use embed or object instead.Defines an embedded applet</summary>
applet,
        ///<summary>Defines an area inside an image-map</summary>
        area,
        ///<summary>Defines an article</summary>
        article,
        ///<summary>Defines content aside from the page content</summary>
        aside,
        ///<summary>Defines sound content</summary>
        audio,
        ///<summary>Defines bold text</summary>
        b,
///<summary>Specifies the base URL/target for all relative URLs in a document</summary>
@base,
        ///<summary>Not supported in HTML5. Use CSS instead.Specifies a default color, size, and font for all text in a document</summary>
basefont,
        ///<summary>Isolates a part of text that might be formatted in a different direction from other text outside it</summary>
        bdi,
        ///<summary>Overrides the current text direction</summary>
        bdo,
        ///<summary>Not supported in HTML5. Use CSS instead.Defines big text</summary>
big,
        ///<summary>Defines a section that is quoted from another source</summary>
        blockquote,
        ///<summary>Defines the document's body</summary>
        body,
        ///<summary>Defines a single line break</summary>
        br,
        ///<summary>Defines a clickable button</summary>
        button,
        ///<summary>Used to draw graphics, on the fly, via scripting (usually JavaScript)</summary>
        canvas,
        ///<summary>Defines a table caption</summary>
        caption,
        ///<summary>Not supported in HTML5. Use CSS instead.Defines centered text</summary>
center,
        ///<summary>Defines the title of a work</summary>
        cite,
        ///<summary>Defines a piece of computer code</summary>
        code,
        ///<summary>Specifies column properties for each column within a colgroup element </summary>
        col,
        ///<summary>Specifies a group of one or more columns in a table for formatting</summary>
        colgroup,
        ///<summary>Links the given content with a machine-readable translation</summary>
        data,
        ///<summary>Specifies a list of pre-defined options for input controls</summary>
        datalist,
        ///<summary>Defines a description/value of a term in a description list</summary>
        dd,
        ///<summary>Defines text that has been deleted from a document</summary>
        del,
        ///<summary>Defines additional details that the user can view or hide</summary>
        details,
        ///<summary>Represents the defining instance of a term</summary>
        dfn,
        ///<summary>Defines a dialog box or window</summary>
        dialog,
        ///<summary>Not supported in HTML5. Use ul instead. Defines a directory list</summary>
dir,
        ///<summary>Defines a section in a document</summary>
        div,
        ///<summary>Defines a description list</summary>
        dl,
        ///<summary>Defines a term/name in a description list</summary>
        dt,
        ///<summary>Defines emphasized text </summary>
        em,
        ///<summary>Defines a container for an external (non-HTML) application</summary>
        embed,
        ///<summary>Groups related elements in a form</summary>
        fieldset,
        ///<summary>Defines a caption for a figure element</summary>
        figcaption,
        ///<summary>Specifies self-contained content</summary>
        figure,
        ///<summary>Not supported in HTML5. Use CSS instead.Defines font, color, and size for text</summary>
font,
        ///<summary>Defines a footer for a document or section</summary>
        footer,
        ///<summary>Defines an HTML form for user input</summary>
        form,
        ///<summary>Not supported in HTML5. Defines a window (a frame) in a frameset</summary>
frame,
        ///<summary>Not supported in HTML5. Defines a set of frames</summary>
frameset,
        ///<summary>Defines HTML headings</summary>
        h1,
        h2,
        h3,
        h4,
        h5,
        h6,
        ///<summary>Defines information about the document</summary>
        head,
        ///<summary>Defines a header for a document or section</summary>
        header,
        ///<summary>Defines a thematic change in the content</summary>
        hr,
        ///<summary>Defines the root of an HTML document</summary>
        html,
        ///<summary>Defines a part of text in an alternate voice or mood</summary>
        i,
        ///<summary>Defines an inline frame</summary>
        iframe,
        ///<summary>Defines an image</summary>
        img,
        ///<summary>Defines an input control</summary>
        input,
        ///<summary>Defines a text that has been inserted into a document</summary>
        ins,
        ///<summary>Defines keyboard input</summary>
        kbd,
        ///<summary>Defines a label for an input element</summary>
        label,
        ///<summary>Defines a caption for a fieldset element</summary>
        legend,
        ///<summary>Defines a list item</summary>
        li,
        ///<summary>Defines the relationship between a document and an external resource (most used to link to style sheets)</summary>
        link,
        ///<summary>Specifies the main content of a document</summary>
        main,
        ///<summary>Defines a client-side image-map</summary>
        map,
        ///<summary>Defines marked/highlighted text</summary>
        mark,
        ///<summary>Defines metadata about an HTML document</summary>
        meta,
        ///<summary>Defines a scalar measurement within a known range (a gauge)</summary>
        meter,
        ///<summary>Defines navigation links</summary>
        nav,
        ///<summary>Not supported in HTML5. Defines an alternate content for users that do not support frames</summary>
noframes,
        ///<summary>Defines an alternate content for users that do not support client-side scripts</summary>
        noscript,
///<summary>Defines an embedded object</summary>
@object,
        ///<summary>Defines an ordered list</summary>
        ol,
        ///<summary>Defines a group of related options in a drop-down list</summary>
        optgroup,
        ///<summary>Defines an option in a drop-down list</summary>
        option,
        ///<summary>Defines the result of a calculation</summary>
        output,
        ///<summary>Defines a paragraph</summary>
        p,
        ///<summary>Defines a parameter for an object</summary>
        param,
        ///<summary>Defines a container for multiple image resources</summary>
        picture,
        ///<summary>Defines preformatted text</summary>
        pre,
        ///<summary>Represents the progress of a task</summary>
        progress,
        ///<summary>Defines a short quotation</summary>
        q,
        ///<summary>Defines what to show in browsers that do not support ruby annotations</summary>
        rp,
        ///<summary>Defines an explanation/pronunciation of characters (for East Asian typography)</summary>
        rt,
        ///<summary>Defines a ruby annotation (for East Asian typography)</summary>
        ruby,
        ///<summary>Defines text that is no longer correct</summary>
        s,
        ///<summary>Defines sample output from a computer program</summary>
        samp,
        ///<summary>Defines a client-side script</summary>
        script,
        ///<summary>Defines a section in a document</summary>
        section,
        ///<summary>Defines a drop-down list</summary>
        select,
        ///<summary>Defines smaller text</summary>
        small,
        ///<summary>Defines multiple media resources for media elements (video and audio)</summary>
        source,
        ///<summary>Defines a section in a document</summary>
        span,
        ///<summary>Not supported in HTML5. Use del or s instead.Defines strikethrough text</summary>
strike,
        ///<summary>Defines important text</summary>
        strong,
        ///<summary>Defines style information for a document</summary>
        style,
        ///<summary>Defines subscripted text</summary>
        sub,
        ///<summary>Defines a visible heading for a details element</summary>
        summary,
        ///<summary>Defines superscripted text</summary>
        sup,
        ///<summary>Defines a container for SVG graphics</summary>
        svg,
        ///<summary>Defines a table</summary>
        table,
        ///<summary>Groups the body content in a table</summary>
        tbody,
        ///<summary>Defines a cell in a table</summary>
        td,
        ///<summary>Defines a template</summary>
        template,
        ///<summary>Defines a multiline input control (text area)</summary>
        textarea,
        ///<summary>Groups the footer content in a table</summary>
        tfoot,
        ///<summary>Defines a header cell in a table</summary>
        th,
        ///<summary>Groups the header content in a table</summary>
        thead,
        ///<summary>Defines a date/time</summary>
        time,
        ///<summary>Defines a title for the document</summary>
        title,
        ///<summary>Defines a row in a table</summary>
        tr,
        ///<summary>Defines text tracks for media elements (video and audio)</summary>
        track,
        ///<summary>Not supported in HTML5. Use CSS instead.Defines teletype text</summary>
tt,
        ///<summary>Defines text that should be stylistically different from normal text</summary>
        u,
        ///<summary>Defines an unordered list</summary>
        ul,
        ///<summary>Defines a variable</summary>
        var,
        ///<summary>Defines a video or movie</summary>
        video,
        ///<summary>Defines a possible line-break</summary>
        wbr


    }
}