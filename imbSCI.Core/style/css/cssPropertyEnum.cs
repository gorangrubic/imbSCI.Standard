using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.Core.style.css
{

    /// <summary>
    /// Enumeration of CSS properties, from: https://www.w3schools.com/cssref/
    /// </summary>
    public enum cssPropertyEnum
    {
        ///<summary>Specifies the alignment for items inside a flexible container</summary>
        align_items,
        ///<summary>Specifies the alignment for selected items inside a flexible container</summary>
        align_self,
        ///<summary>Resets all properties (except unicode-bidi and direction)</summary>
        all,
        ///<summary>A shorthand property for all the animation-* properties</summary>
        animation,
        ///<summary>Specifies a delay for the start of an animation</summary>
        animation_delay,
        ///<summary>Specifies whether an animation should be played forwards, backwards or in alternate cycles</summary>
        animation_direction,
        ///<summary>Specifies how long an animation should take to complete one cycle</summary>
        animation_duration,
        ///<summary>Specifies a style for the element when the animation is not playing (before it starts, after it ends, or both)</summary>
        animation_fill_mode,
        ///<summary>Specifies the number of times an animation should be played</summary>
        animation_iteration_count,
        ///<summary>Specifies a name for the @keyframes animation</summary>
        animation_name,
        ///<summary>Specifies whether the animation is running or paused</summary>
        animation_play_state,
        ///<summary>Specifies the speed curve of an animation</summary>
        animation_timing_function,
        ///<summary>Defines whether or not the back face of an element should be visible when facing the user</summary>
        backface_visibility,
        ///<summary>A shorthand property for all the background-* properties</summary>
        background,
        ///<summary>Sets whether a background image scrolls with the rest of the page, or is fixed</summary>
        background_attachment,
        ///<summary>Specifies the blending mode of each background layer (color/image)</summary>
        background_blend_mode,
        ///<summary>Defines how far the background (color or image) should extend within an element</summary>
        background_clip,
        ///<summary>Specifies the background color of an element</summary>
        background_color,
        ///<summary>Specifies one or more background images for an element</summary>
        background_image,
        ///<summary>Specifies the origin position of a background image</summary>
        background_origin,
        ///<summary>Specifies the position of a background image</summary>
        background_position,
        ///<summary>Sets if/how a background image will be repeated</summary>
        background_repeat,
        ///<summary>Specifies the size of the background images</summary>
        background_size,
        ///<summary>A shorthand property for border-width, border-style and border-color</summary>
        border,
        ///<summary>A shorthand property for border-bottom-width, border-bottom-style and border-bottom-color</summary>
        border_bottom,
        ///<summary>Sets the color of the bottom border</summary>
        border_bottom_color,
        ///<summary>Defines the radius of the border of the bottom-left corner</summary>
        border_bottom_left_radius,
        ///<summary>Defines the radius of the border of the bottom-right corner</summary>
        border_bottom_right_radius,
        ///<summary>Sets the style of the bottom border</summary>
        border_bottom_style,
        ///<summary>Sets the width of the bottom border</summary>
        border_bottom_width,
        ///<summary>Sets whether table borders should collapse into a single border or be separated</summary>
        border_collapse,
        ///<summary>Sets the color of the four borders</summary>
        border_color,
        ///<summary>A shorthand property for all the border-image-* properties</summary>
        border_image,
        ///<summary>Specifies the amount by which the border image area extends beyond the border box</summary>
        border_image_outset,
        ///<summary>Specifies whether the border image should be repeated, rounded or stretched</summary>
        border_image_repeat,
        ///<summary>Specifies how to slice the border image</summary>
        border_image_slice,
        ///<summary>Specifies the path to the image to be used as a border</summary>
        border_image_source,
        ///<summary>Specifies the width of the border image</summary>
        border_image_width,
        ///<summary>A shorthand property for all the border-left-* properties</summary>
        border_left,
        ///<summary>Sets the color of the left border</summary>
        border_left_color,
        ///<summary>Sets the style of the left border</summary>
        border_left_style,
        ///<summary>Sets the width of the left border</summary>
        border_left_width,
        ///<summary>A shorthand property for the four border-*-radius properties</summary>
        border_radius,
        ///<summary>A shorthand property for all the border-right-* properties</summary>
        border_right,
        ///<summary>Sets the color of the right border</summary>
        border_right_color,
        ///<summary>Sets the style of the right border</summary>
        border_right_style,
        ///<summary>Sets the width of the right border</summary>
        border_right_width,
        ///<summary>Sets the distance between the borders of adjacent cells</summary>
        border_spacing,
        ///<summary>Sets the style of the four borders</summary>
        border_style,
        ///<summary>A shorthand property for border-top-width, border-top-style and border-top-color</summary>
        border_top,
        ///<summary>Sets the color of the top border</summary>
        border_top_color,
        ///<summary>Defines the radius of the border of the top-left corner</summary>
        border_top_left_radius,
        ///<summary>Defines the radius of the border of the top-right corner</summary>
        border_top_right_radius,
        ///<summary>Sets the style of the top border</summary>
        border_top_style,
        ///<summary>Sets the width of the top border</summary>
        border_top_width,
        ///<summary>Sets the width of the four borders</summary>
        border_width,
        ///<summary>Sets the elements position, from the bottom of its parent element</summary>
        bottom,
        ///<summary>Sets the behavior of the background and border of an element at page-break, or, for in-line elements, at line-break.</summary>
        box_decoration_break,
        ///<summary>Attaches one or more shadows to an element</summary>
        box_shadow,
        ///<summary>Defines how the width and height of an element are calculated: should they include padding and borders, or not</summary>
        box_sizing,
        ///<summary>Specifies the page-, column-, or region-break behavior after the generated box</summary>
        break_after,
        ///<summary>Specifies the page-, column-, or region-break behavior before the generated box</summary>
        break_before,
        ///<summary>Specifies the page-, column-, or region-break behavior inside the generated box</summary>
        break_inside,
        ///<summary>Specifies the placement of a table caption</summary>
        caption_side,
        ///<summary>Specifies the color of the cursor (caret) in inputs, textareas, or any element that is editable</summary>
        caret_color,
        ///<summary>Specifies the character encoding used in the style sheet</summary>
        @charset,
        ///<summary>Specifies on which sides of an element floating elements are not allowed to float</summary>
        clear,
        ///<summary>Clips an absolutely positioned element</summary>
        clip,
        ///<summary>Sets the color of text</summary>
        color,
        ///<summary>Specifies the number of columns an element should be divided into</summary>
        column_count,
        ///<summary>Specifies how to fill columns, balanced or not</summary>
        column_fill,
        ///<summary>Specifies the gap between the columns</summary>
        column_gap,
        ///<summary>A shorthand property for all the column-rule-* properties</summary>
        column_rule,
        ///<summary>Specifies the color of the rule between columns</summary>
        column_rule_color,
        ///<summary>Specifies the style of the rule between columns</summary>
        column_rule_style,
        ///<summary>Specifies the width of the rule between columns</summary>
        column_rule_width,
        ///<summary>Specifies how many columns an element should span across</summary>
        column_span,
        ///<summary>Specifies the column width</summary>
        column_width,
        ///<summary>A shorthand property for column-width and column-count</summary>
        columns,
        ///<summary>Used with the :before and :after pseudo-elements, to insert generated content</summary>
        content,
        ///<summary>Increases or decreases the value of one or more CSS counters</summary>
        counter_increment,
        ///<summary>Creates or resets one or more CSS counters</summary>
        counter_reset,
        ///<summary>Specifies the mouse cursor to be displayed when pointing over an element</summary>
        cursor,
        ///<summary>Specifies the text direction/writing direction</summary>
        direction,
        ///<summary>Specifies how a certain HTML element should be displayed</summary>
        display,
        ///<summary>Specifies whether or not to display borders and background on empty cells in a table</summary>
        empty_cells,
        ///<summary>Defines effects (e.g. blurring or color shifting) on an element before the element is displayed</summary>
        filter,
        ///<summary>A shorthand property for the flex-grow, flex-shrink, and the flex-basis properties</summary>
        flex,
        ///<summary>Specifies the initial length of a flexible item</summary>
        flex_basis,
        ///<summary>Specifies the direction of the flexible items</summary>
        flex_direction,
        ///<summary>A shorthand property for the flex-direction and the flex-wrap properties</summary>
        flex_flow,
        ///<summary>Specifies how much the item will grow relative to the rest</summary>
        flex_grow,
        ///<summary>Specifies how the item will shrink relative to the rest</summary>
        flex_shrink,
        ///<summary>Specifies whether the flexible items should wrap or not</summary>
        flex_wrap,
///<summary>Specifies whether or not a box should float</summary>
@float,
        ///<summary>A shorthand property for the font-style, font-variant, font-weight, font-size/line-height, and the font-familyproperties</summary>
        font,
        ///<summary>A rule that allows websites to download and use fonts other than the "web-safe" fonts</summary>
        @font_face,
        ///<summary>Specifies the font family for text</summary>
        font_family,
        ///<summary>Allows control over advanced typographic features in OpenType fonts</summary>
        font_feature_settings,
        ///<summary>Allows authors to use a common name in font-variant-alternate for feature activated differently in OpenType</summary>
        @font_feature_values,
        ///<summary>Controls the usage of the kerning information (how letters are spaced)</summary>
        font_kerning,
        ///<summary>Controls the usage of language-specific glyphs in a typeface</summary>
        font_language_override,
        ///<summary>Specifies the font size of text</summary>
        font_size,
        ///<summary>Preserves the readability of text when font fallback occurs</summary>
        font_size_adjust,
        ///<summary>Selects a normal, condensed, or expanded face from a font family</summary>
        font_stretch,
        ///<summary>Specifies the font style for text</summary>
        font_style,
        ///<summary>Controls which missing typefaces (bold or italic) may be synthesized by the browser</summary>
        font_synthesis,
        ///<summary>Specifies whether or not a text should be displayed in a small-caps font</summary>
        font_variant,
        ///<summary>Controls the usage of alternate glyphs associated to alternative names defined in @font-feature-values</summary>
        font_variant_alternates,
        ///<summary>Controls the usage of alternate glyphs for capital letters</summary>
        font_variant_caps,
        ///<summary>Controls the usage of alternate glyphs for East Asian scripts (e.g Japanese and Chinese)</summary>
        font_variant_east_asian,
        ///<summary>Controls which ligatures and contextual forms are used in textual content of the elements it applies to</summary>
        font_variant_ligatures,
        ///<summary>Controls the usage of alternate glyphs for numbers, fractions, and ordinal markers</summary>
        font_variant_numeric,
        ///<summary>Controls the usage of alternate glyphs of smaller size positioned as superscript or subscript regarding the baseline of the font</summary>
        font_variant_position,
        ///<summary>Specifies the weight of a font</summary>
        font_weight,
        ///<summary>A shorthand property for the grid-template-rows, grid-template-columns, grid-template-areas, grid-auto-rows, grid-auto-columns, and the grid-auto-flow properties</summary>
        grid,
        ///<summary>Either specifies a name for the grid item, or this property is a shorthand property for the grid-row-start, grid-column-start, grid-row-end, and grid-column-end properties</summary>
        grid_area,
        ///<summary>Specifies a default column size</summary>
        grid_auto_columns,
        ///<summary>Specifies how auto-placed items are inserted in the grid</summary>
        grid_auto_flow,
        ///<summary>Specifies a default row size</summary>
        grid_auto_rows,
        ///<summary>A shorthand property for the grid-column-start and the grid-column-end properties</summary>
        grid_column,
        ///<summary>Specifies where to end the grid item</summary>
        grid_column_end,
        ///<summary>Specifies the size of the gap between columns</summary>
        grid_column_gap,
        ///<summary>Specifies where to start the grid item</summary>
        grid_column_start,
        ///<summary>A shorthand property for the grid-row-gap and grid-column-gap properties</summary>
        grid_gap,
        ///<summary>A shorthand property for the grid-row-start and the grid-row-end properties</summary>
        grid_row,
        ///<summary>Specifies where to end the grid item</summary>
        grid_row_end,
        ///<summary>Specifies the size of the gap between rows</summary>
        grid_row_gap,
        ///<summary>Specifies where to start the grid item</summary>
        grid_row_start,
        ///<summary>A shorthand property for the grid-template-rows, grid-template-columns and grid-areas properties</summary>
        grid_template,
        ///<summary>Specifies how to display columns and rows, using named grid items</summary>
        grid_template_areas,
        ///<summary>Specifies the size of the columns, and how many columns in a grid layout</summary>
        grid_template_columns,
        ///<summary>Specifies the size of the rows in a grid layout</summary>
        grid_template_rows,
        ///<summary>Specifies whether a punctuation character may be placed outside the line box</summary>
        hanging_punctuation,
        ///<summary>Sets the height of an element</summary>
        height,
        ///<summary>Sets how to split words to improve the layout of paragraphs</summary>
        hyphens,
        ///<summary>Gives a hint to the browser about what aspects of an image are most important to preserve when the image is scaled</summary>
        image_rendering,
        ///<summary>Allows you to import a style sheet into another style sheet</summary>
        @import,
        ///<summary>Defines whether an element must create a new stacking content</summary>
        isolation,
        ///<summary>Specifies the alignment between the items inside a flexible container when the items do not use all available space</summary>
        justify_content,
        ///<summary>Specifies the animation code</summary>
        @keyframes,
        ///<summary>Specifies the left position of a positioned element</summary>
        left,
        ///<summary>Increases or decreases the space between characters in a text</summary>
        letter_spacing,
        ///<summary>Specifies how/if to break lines</summary>
        line_break,
        ///<summary>Sets the line height</summary>
        line_height,
        ///<summary>Sets all the properties for a list in one declaration</summary>
        list_style,
        ///<summary>Specifies an image as the list-item marker</summary>
        list_style_image,
        ///<summary>Specifies the position of the list-item markers (bullet points)</summary>
        list_style_position,
        ///<summary>Specifies the type of list-item marker</summary>
        list_style_type,
        ///<summary></summary>
        M,
        ///<summary>Sets all the margin properties in one declaration</summary>
        margin,
        ///<summary>Sets the bottom margin of an element</summary>
        margin_bottom,
        ///<summary>Sets the left margin of an element</summary>
        margin_left,
        ///<summary>Sets the right margin of an element</summary>
        margin_right,
        ///<summary>Sets the top margin of an element</summary>
        margin_top,
        ///<summary>Sets the maximum height of an element</summary>
        max_height,
        ///<summary>Sets the maximum width of an element</summary>
        max_width,
        ///<summary>Sets the style rules for different media types/devices/sizes</summary>
        @media,
        ///<summary>Sets the minimum height of an element</summary>
        min_height,
        ///<summary>Sets the minimum width of an element</summary>
        min_width,
        ///<summary>Specifies how an element's content should blend with its direct parent background</summary>
        mix_blend_mode,
        ///<summary>Specifies how the contents of a replaced element should be fitted to the box established by its used height and width</summary>
        object_fit,
        ///<summary>Specifies the alignment of the replaced element inside its box</summary>
        object_position,
        ///<summary>Sets the opacity level for an element</summary>
        opacity,
        ///<summary>Sets the order of the flexible item, relative to the rest</summary>
        order,
        ///<summary>Sets the minimum number of lines that must be left at the bottom of a page when a page break occurs inside an element</summary>
        orphans,
        ///<summary>A shorthand property for the outline-width, outline-style, and the outline-color properties</summary>
        outline,
        ///<summary>Sets the color of an outline</summary>
        outline_color,
        ///<summary>Offsets an outline, and draws it beyond the border edge</summary>
        outline_offset,
        ///<summary>Sets the style of an outline</summary>
        outline_style,
        ///<summary>Sets the width of an outline</summary>
        outline_width,
        ///<summary>Specifies what happens if content overflows an element's box</summary>
        overflow,
        ///<summary>Specifies whether or not the browser may break lines within words in order to prevent overflow (when a string is too long to fit its containing box)</summary>
        overflow_wrap,
        ///<summary>Specifies whether or not to clip the left/right edges of the content, if it overflows the element's content area</summary>
        overflow_x,
        ///<summary>Specifies whether or not to clip the top/bottom edges of the content, if it overflows the element's content area</summary>
        overflow_y,
        ///<summary>A shorthand property for all the padding-* properties</summary>
        padding,
        ///<summary>Sets the bottom padding of an element</summary>
        padding_bottom,
        ///<summary>Sets the left padding of an element</summary>
        padding_left,
        ///<summary>Sets the right padding of an element</summary>
        padding_right,
        ///<summary>Sets the top padding of an element</summary>
        padding_top,
        ///<summary>Sets the page-break behavior after an element</summary>
        page_break_after,
        ///<summary>Sets the page-break behavior before an element</summary>
        page_break_before,
        ///<summary>Sets the page-break behavior inside an element</summary>
        page_break_inside,
        ///<summary>Gives a 3D-positioned element some perspective</summary>
        perspective,
        ///<summary>Defines at which position the user is looking at the 3D-positioned element</summary>
        perspective_origin,
        ///<summary>Defines whether or not an element reacts to pointer events</summary>
        pointer_events,
        ///<summary>Specifies the type of positioning method used for an element (static, relative, absolute or fixed)</summary>
        position,
        ///<summary>Sets the type of quotation marks for embedded quotations</summary>
        quotes,
        ///<summary>Defines if (and how) an element is resizable by the user</summary>
        resize,
        ///<summary>Specifies the right position of a positioned element</summary>
        right,
        ///<summary>Specifies whether to smoothly animate the scroll position in a scrollable box, instead of a straight jump</summary>
        scroll_behavior,
        ///<summary>Specifies the width of a tab character</summary>
        tab_size,
        ///<summary>Defines the algorithm used to lay out table cells, rows, and columns</summary>
        table_layout,
        ///<summary>Specifies the horizontal alignment of text</summary>
        text_align,
        ///<summary>Describes how the last line of a block or a line right before a forced line break is aligned when text-align is "justify"</summary>
        text_align_last,
        ///<summary>Specifies the combination of multiple characters into the space of a single character</summary>
        text_combine_upright,
        ///<summary>Specifies the decoration added to text</summary>
        text_decoration,
        ///<summary>Specifies the color of the text-decoration</summary>
        text_decoration_color,
        ///<summary>Specifies the type of line in a text-decoration</summary>
        text_decoration_line,
        ///<summary>Specifies the style of the line in a text decoration</summary>
        text_decoration_style,
        ///<summary>Specifies the indentation of the first line in a text-block</summary>
        text_indent,
        ///<summary>Specifies the justification method used when text-align is "justify"</summary>
        text_justify,
        ///<summary>Defines the orientation of the text in a line</summary>
        text_orientation,
        ///<summary>Specifies what should happen when text overflows the containing element</summary>
        text_overflow,
        ///<summary>Adds shadow to text</summary>
        text_shadow,
        ///<summary>Controls the capitalization of text</summary>
        text_transform,
        ///<summary>Specifies the position of the underline which is set using the text-decoration property</summary>
        text_underline_position,
        ///<summary>Specifies the top position of a positioned element</summary>
        top,
        ///<summary>Applies a 2D or 3D transformation to an element</summary>
        transform,
        ///<summary>Allows you to change the position on transformed elements</summary>
        transform_origin,
        ///<summary>Specifies how nested elements are rendered in 3D space</summary>
        transform_style,
        ///<summary>A shorthand property for all the transition-* properties</summary>
        transition,
        ///<summary>Specifies when the transition effect will start</summary>
        transition_delay,
        ///<summary>Specifies how many seconds or milliseconds a transition effect takes to complete</summary>
        transition_duration,
        ///<summary>Specifies the name of the CSS property the transition effect is for</summary>
        transition_property,
        ///<summary>Specifies the speed curve of the transition effect</summary>
        transition_timing_function,
        ///<summary>Used together with the direction property to set or return whether the text should be overridden to support multiple languages in the same document</summary>
        unicode_bidi,
        ///<summary>Specifies whether the text of an element can be selected</summary>
        user_select,
        ///<summary>Sets the vertical alignment of an element</summary>
        vertical_align,
        ///<summary>Specifies whether or not an element is visible</summary>
        visibility,
        ///<summary>Specifies how white-space inside an element is handled</summary>
        white_space,
        ///<summary>Sets the minimum number of lines that must be left at the top of a page when a page break occurs inside an element</summary>
        widows,
        ///<summary>Sets the width of an element</summary>
        width,
        ///<summary>Specifies how words should break when reaching the end of a line</summary>
        word_break,
        ///<summary>Increases or decreases the space between words in a text</summary>
        word_spacing,
        ///<summary>Allows long, unbreakable words to be broken and wrap to the next line</summary>
        word_wrap,
        ///<summary>Specifies whether lines of text are laid out horizontally or vertically</summary>
        writing_mode,
        ///<summary>Sets the stack order of a positioned element</summary>
        z_index


    }
}