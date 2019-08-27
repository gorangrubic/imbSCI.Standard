using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.Core.style.css
{
public enum cssSelectorEnum
    {
        ///<summary><c>$$after</c> Insert something after the content of each <p> element</summary>
        d_after,
        ///<summary><c>$$before</c> Insert something before the content of each <p> element</summary>
        d_before,
///<summary><c>:checked</c> Selects every checked <input> element</summary>
@checked,
///<summary><c>:default</c> Selects the default <input> element</summary>
@default,
        ///<summary><c>:disabled</c> Selects every disabled <input> element</summary>
        disabled,
        ///<summary><c>:empty</c> Selects every <p> element that has no children (including text nodes)</summary>
        empty,
        ///<summary><c>:enabled</c> Selects every enabled <input> element</summary>
        enabled,
        ///<summary><c>:first-child</c> Selects every <p> element that is the first child of its parent</summary>
        first_child,
        ///<summary><c>$$first-letter</c> Selects the first letter of every <p> element</summary>
        d_first_letter,
        ///<summary><c>$$first-line</c> Selects the first line of every <p> element</summary>
        d_first_line,
        ///<summary><c>:first-of-type</c> Selects every <p> element that is the first <p> element of its parent</summary>
        first_of_type,
        ///<summary><c>:focus</c> Selects the input element which has focus</summary>
        focus,
        ///<summary><c>:hover</c> Selects links on mouse over</summary>
        hover,
        ///<summary><c>:in-range</c> Selects input elements with a value within a specified range</summary>
        in_range,
        ///<summary><c>:indeterminate</c> Selects input elements that are in an indeterminate state</summary>
        indeterminate,
        ///<summary><c>:invalid</c> Selects all input elements with an invalid value</summary>
        invalid,
        ///<summary><c>:lang(language)</c> Selects every <p> element with a lang attribute equal to "it" (Italian)</summary>
        lang,
        ///<summary><c>:last-child</c> Selects every <p> element that is the last child of its parent</summary>
        last_child,
        ///<summary><c>:last-of-type</c> Selects every <p> element that is the last <p> element of its parent</summary>
        last_of_type,
        ///<summary><c>:link</c> Selects all unvisited links</summary>
        link,
        ///<summary><c>:not(selector)</c> Selects every element that is not a <p> element</summary>
        not,
        ///<summary><c>:nth-child(n)</c> Selects every <p> element that is the second child of its parent</summary>
        nth_child,
        ///<summary><c>:nth-last-child(n)</c> Selects every <p> element that is the second child of its parent, counting from the last child</summary>
        nth_last_child,
        ///<summary><c>:nth-last-of-type(n)</c> Selects every <p> element that is the second <p> element of its parent, counting from the last child</summary>
        nth_last_of_type,
        ///<summary><c>:nth-of-type(n)</c> Selects every <p> element that is the second <p> element of its parent</summary>
        nth_of_type,
        ///<summary><c>:only-of-type</c> Selects every <p> element that is the only <p> element of its parent</summary>
        only_of_type,
        ///<summary><c>:only-child</c> Selects every <p> element that is the only child of its parent</summary>
        only_child,
        ///<summary><c>:optional</c> Selects input elements with no "required" attribute</summary>
        optional,
        ///<summary><c>:out-of-range</c> Selects input elements with a value outside a specified range</summary>
        out_of_range,
        ///<summary><c>$$placeholder</c> Selects input elements with placeholder text</summary>
        d_placeholder,
        ///<summary><c>:read-only</c> Selects input elements with the "readonly" attribute specified</summary>
        read_only,
        ///<summary><c>:read-write</c> Selects input elements with the "readonly" attribute NOT specified</summary>
        read_write,
        ///<summary><c>:required</c> Selects input elements with the "required" attribute specified</summary>
        required,
        ///<summary><c>:root</c> Selects the document's root element</summary>
        root,
        ///<summary><c>$$selection</c> Selects the portion of an element that is selected by a user</summary>
        d_selection,
        ///<summary><c>:target</c> Selects the current active #news element (clicked on a URL containing that anchor name)</summary>
        target,
        ///<summary><c>:valid</c> Selects all input elements with a valid value</summary>
        valid,
        ///<summary><c>:visited</c> Selects all visited links</summary>
        visited



    }
}