using imbSCI.Core.extensions.data;
using imbSCI.Core.math.classificationMetrics;
using imbSCI.Data;
using System;
using System.Text;

namespace imbSCI.Reporting.charts
{
    /// <summary>
    /// Generates shortcode content for WordPress
    /// </summary>
    /// <remarks>
    /// This should be used with proper WordPress plugin for created shortcode. The plugin has to Base64 decode shortcode content. See below PHP example for proper WP plugin
    /// </remarks>
    /// <code>
    /// function jschart_shortcode( $atts, $content = null ) {
    /// $a = shortcode_atts(array(
    ///     'id' => 'element_id'), $atts );
    ///  $js = base64_decode($a['js']);
    /// 
    /// return '&lt;div id="' .$a['id']. '" >&lt;/div>&lt;script>'.$js.'&lt;/script>';
    /// }
    /// add_shortcode( 'jschart', 'jschart_shortcode' );
    /// </code>
    public class ShortcodeJSInsertTool
    {
        public String shortcodeName { get; set; } = "jschart";

        public Boolean UseGutenbergContainerBlock { get; set; } = true;

        public String GutenbergContainerBlock { get; set; } = "<!--wp:shortcode-->{0}<!--/wp:shortcode-->";

        public Int32 currentID { get; set; } = 0;

        public String GetShortCodeTemplate()
        {
            return "[" + shortcodeName + " id=\"{0}\" {1}]{2}[/" + shortcodeName + "]";
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }



        /// <summary>
        /// Creates WordPress shortcode code to embedd the specified js content. 
        /// </summary>
        /// <param name="js_content">JavaScript content to be enbedded. If the string contains {0}, it will be replaced with <c>custom_id</c></param>
        /// <param name="custom_id">DIV identifier, the custom value. Leave empty to have automatically assigned unique ID for created DIV</param>
        /// <param name="extendedAttributes">Optional pair of additional shortcode attributes</param>
        /// <returns>WP shortcode code. If <see cref="UseGutenbergContainerBlock"/> it will be wrapped in given gutenberg container, <see cref="GutenbergContainerBlock"/></returns>
        public String Create(String js_content, String custom_id = "", reportExpandedData extendedAttributes = null)
        {

            currentID++;

            if (custom_id.isNullOrEmpty()) custom_id = "block" + currentID.ToString("D6");


            if (js_content.Contains("{0}"))
            {
                js_content = js_content.Replace("{0}", custom_id);
                //js_content = String.Format(js_content, custom_id);
            }



            String attributes = "";

            if (extendedAttributes != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (reportExpandedDataPair pair in extendedAttributes)
                {
                    sb.Append(" " + pair.key + "=\"" + pair.value + "\"");
                }
                attributes = sb.ToString();
            }

            String content = Base64Encode(js_content);

            String template = GetShortCodeTemplate();
            String output = String.Format(template, custom_id, attributes, content);

            if (UseGutenbergContainerBlock)
            {
                output = String.Format(GutenbergContainerBlock, output);

            }
            return output;
        }

    }
}