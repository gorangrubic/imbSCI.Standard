// --------------------------------------------------------------------------------------------------------------------
// <copyright file="stringPresetList.cs" company="imbVeles" >
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
namespace imbSCI.Core.extensions.text
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using imbSCI.Data.collection;

    #region imbVeles using

    using System;

    #endregion imbVeles using

    public class stringPresetList
    {
        #region presetListElement enum

        public enum presetListElement
        {
            value,
            desc,
            all
        }

        #endregion presetListElement enum

        private static aceDictionaryCollection<String> _defaultPresets;

        #region imbObject Property <imbCollection> defaultPresets

        /// <summary>
        /// imbControl property defaultPresets tipa imbCollection
        /// </summary>
        internal static aceDictionaryCollection<String> defaultPresets
        {
            get
            {
                if (_defaultPresets == null)
                {
                    _defaultPresets = new aceDictionaryCollection<String>();
                    preparePresets();
                }

                return _defaultPresets;
            }
            set { _defaultPresets = value; }
        }

        #endregion imbObject Property <imbCollection> defaultPresets

        private static void preparePresets()
        {
            defaultPresets["#countryList#"] =
                "RS|Serbia;AF|Afghanistan;AL|Albania;DZ|Algeria;AS|American Samoa;AD|Andorra;AO|Angola;AI|Anguilla;AQ|Antarctica;AG|Antigua and Barbuda;AR|Argentina;AM|Armenia;AW|Aruba;AU|Australia;AT|Austria;AZ|Azerbaijan;BS|Bahamas;BH|Bahrain;BD|Bangladesh;BB|Barbados;BY|Belarus;BE|Belgium;BZ|Belize;BJ|Benin;BM|Bermuda;BT|Bhutan;BO|Bolivia;BA|Bosnia and Herzegovina;BW|Botswana;BV|Bouvet Island;BR|Brazil;IO|British Indian Ocean Territory;BN|Brunei Darussalam;BG|Bulgaria;BF|Burkina Faso;BI|Burundi;KH|Cambodia;CM|Cameroon;CA|Canada;CV|Cape Verde;KY|Cayman Islands;CF|Central African Republic;TD|Chad;CL|Chile;CN|China;CX|Christmas Island;CC|Cocos (Keeling) Islands;CO|Colombia;KM|Comoros;CG|Congo;CD|Congo, Democratic Republic;CK|Cook Islands;CR|Costa Rica;CI|Cote d'Ivoire;HR|Croatia;CY|Cyprus;CZ|Czech Republic;DK|Denmark;DJ|Djibouti;DM|Dominica;DO|Dominican Republic;TL|East Timor;EC|Ecuador;EG|Egypt;SV|El Salvador;GQ|Equatorial Guinea;ER|Eritrea;EE|Estonia;ET|Ethiopia;FK|Falkland Islands (Malvinas);FO|Faroe Islands;FJ|Fiji;FI|Finland;FR|France;GF|French Guiana;PF|French Polynesia;TF|French Southern Territories;GA|Gabon;GM|Gambia;GE|Georgia;DE|Germany;GH|Ghana;GI|Gibraltar;GR|Greece;GL|Greenland;GD|Grenada;GP|Guadeloupe;GU|Guam;GT|Guatemala;GN|Guinea;GW|Guinea-Bissau;GY|Guyana;HT|Haiti;HM|Heard and McDonald Islands;HN|Honduras;HK|Hong Kong;HU|Hungary;IS|Iceland;IN|India;ID|Indonesia;IQ|Iraq;IE|Ireland;IL|Israel;IT|Italy;JM|Jamaica;JP|Japan;JO|Jordan;KZ|Kazakhstan;KE|Kenya;KI|Kiribati;KW|Kuwait;KG|Kyrgyzstan;LA|Lao People's Democratic Republic;LV|Latvia;LB|Lebanon;LS|Lesotho;LR|Liberia;LY|Libya;LI|Liechtenstein;LT|Lithuania;LU|Luxembourg;MO|Macau;MK|Macedonia;MG|Madagascar;MW|Malawi;MY|Malaysia;MV|Maldives;ML|Mali;MT|Malta;MH|Marshall Islands;MQ|Martinique;MR|Mauritania;MU|Mauritius;YT|Mayotte;MX|Mexico;FM|Micronesia;MD|Moldova;MC|Monaco;MN|Mongolia;MS|Montserrat;MA|Morocco;MZ|Mozambique;NA|Namibia;NR|Nauru;NP|Nepal;NL|Netherlands;AN|Netherlands Antilles;NC|New Caledonia;NZ|New Zealand;NI|Nicaragua;NE|Niger;NG|Nigeria;NU|Niue;NF|Norfolk Island;MP|Northern Mariana Islands;NO|Norway;OM|Oman;PK|Pakistan;PW|Palau;PS|Palestinian Territory;PA|Panama;PG|Papua New Guinea;PY|Paraguay;PE|Peru;PH|Philippines;PN|Pitcairn;PL|Poland;PT|Portugal;PR|Puerto Rico;QA|Qatar;RE|Reunion;RO|Romania;RU|Russian Federation;RW|Rwanda;KN|Saint Kitts and Nevis;LC|Saint Lucia;VC|Saint Vincent and the Grenadines;WS|Samoa;SM|San Marino;ST|Sao Tome and Principe;SA|Saudi Arabia;SN|Senegal;CS|Serbia and Montenegro;SC|Seychelles;SL|Sierra Leone;SG|Singapore;SK|Slovakia;SI|Slovenia;SB|Solomon Islands;SO|Somalia;ZA|South Africa;GS|South Georgia and The South Sandwich Islands;KR|South Korea;ES|Spain;LK|Sri Lanka;SH|St. Helena;PM|St. Pierre and Miquelon;SR|Suriname;SJ|Svalbard and Jan Mayen Islands;SZ|Swaziland;SE|Sweden;CH|Switzerland;TW|Taiwan;TJ|Tajikistan;TZ|Tanzania;TH|Thailand;TG|Togo;TK|Tokelau;TO|Tonga;TT|Trinidad and Tobago;TN|Tunisia;TR|Turkey;TM|Turkmenistan;TC|Turks and Caicos Islands;TV|Tuvalu;UG|Uganda;UA|Ukraine;AE|United Arab Emirates;GB|United Kingdom;US|United States;UM|United States Minor Outlying Islands;UY|Uruguay;UZ|Uzbekistan;VU|Vanuatu;VA|Vatican;VE|Venezuela;VN|Viet Nam;VG|Virgin Islands (British);VI|Virgin Islands (U.S.);WF|Wallis and Futuna Islands;EH|Western Sahara;YE|Yemen;ZM|Zambia;ZW|Zimbabwe;";

            defaultPresets["#googleLangSearch#"] =
                "lang_af|Afrikaans;lang_ar|Arabic;lang_hy|Armenian;lang_be|Belarusian;lang_bg|Bulgarian;lang_ca|Catalan;lang_zh-CN|Chinese (Simplified);lang_zh-TW|Chinese (Traditional);lang_hr|Croatian;lang_cs|Czech;lang_da|Danish;lang_nl|Dutch;lang_en|English;lang_eo|Esperanto;lang_et|Estonian;lang_tl|Filipino;lang_fi|Finnish;lang_fr|French;lang_de|German;lang_el|Greek;lang_iw|Hebrew;lang_hu|Hungarian;lang_is|Icelandic;lang_id|Indonesian;lang_it|Italian;lang_ja|Japanese;lang_ko|Korean;lang_lv|Latvian;lang_lt|Lithuanian;lang_no|Norwegian;lang_fa|Persian;lang_pl|Polish;lang_pt|Portuguese;lang_ro|Romanian;lang_ru|Russian;lang_sr|Serbian;lang_sk|Slovak;lang_sl|Slovenian;lang_es|Spanish;lang_sw|Swahili;lang_sv|Swedish;lang_th|Thai;lang_tr|Turkish;lang_uk|Ukrainian;lang_vi|Vietnamese;";

            defaultPresets["#languageLong#"] =
                "bg|Bulgarian;hr|Croatian;en|English;fr|French;de|German;it|Italian;pl|Polish;sr|Serbian;sl|Slovenian;sk|Slovakian;af|Afrikaans;sq|Albanian;am|Amharic;ar|Arabic;hy|Armenian;az|Azerbaijani;eu|Basque;be|Belarusian;bn|Bengali;bh|Bihari;bs|Bosnian;br|Breton;bg|Bulgarian;km|Cambodian;ca|Catalan;zh-CN|Chinese (Simplified);zh-TW|Chinese (Traditional);co|Corsican;hr|Croatian;cs|Czech;da|Danish;nl|Dutch;en|English;eo|Esperanto;et|Estonian;fo|Faroese;tl|Filipino;fi|Finnish;fr|French;fy|Frisian;gl|Galician;ka|Georgian;de|German;el|Greek;gn|Guarani;gu|Gujarati;xx-hacker  Hacker;ha|Hausa;iw|Hebrew;hi|Hindi;hu|Hungarian;is|Icelandic;id|Indonesian;ia|Interlingua;ga|Irish;it|Italian;ja|Japanese;jw|Javanese;kn|Kannada;kk|Kazakh;rw|Kinyarwanda;rn|Kirundi;ko|Korean;ku|Kurdish;ky|Kyrgyz;lo|Laothian;la|Latin;lv|Latvian;ln|Lingala;lt|Lithuanian;mk|Macedonian;mg|Malagasy;ms|Malay;ml|Malayalam;mt|Maltese;mi|Maori;mr|Marathi;mo|Moldavian;mn|Mongolian;sr-ME|Montenegrin;ne|Nepali;no|Norwegian;nn|Norwegian (Nynorsk);oc|Occitan;or|Oriya;om|Oromo;ps|Pashto;fa|Persian;pl|Polish;pt-BR|Portuguese (Brazil);pt-PT|Portuguese (Portugal);pa|Punjabi;qu|Quechua;ro|Romanian;rm|Romansh;ru|Russian;gd|Scots Gaelic;sr|Serbian;sh|Serbo-Croatian;st|Sesotho;sn|Shona;sd|Sindhi;si|Sinhalese;sk|Slovak;sl|Slovenian;so|Somali;es|Spanish;su|Sundanese;sw|Swahili;sv|Swedish;tg|Tajik;ta|Tamil;tt|Tatar;te|Telugu;th|Thai;ti|Tigrinya;to|Tonga;tr|Turkish;tk|Turkmen;tw|Twi;ug|Uighur;uk|Ukrainian;ur|Urdu;uz|Uzbek;vi|Vietnamese;cy|Welsh;xh|Xhosa;yi|Yiddish;yo|Yoruba;zu|Zulu;";
            //try
            //{
            //    defaultPresets["#tldList#"] = imbDomainManager.getAllTLD();
            //}
            //catch
            //{
            //}
            defaultPresets["#htmlTags#"] =
                "!--...--|Specifies a comment;!DOCTYPE| Specifies the document type;a|Specifies a hyperlink;abbr|Specifies an abbreviation;address|Specifies an address element;area|Specifies an area inside an image map;article|Specifies an article;aside|Specifies content aside from the page content;audio|Specifies sound content;b|Specifies bold text;base|Specifies a base URL for all the links in a page;bdo|Specifies the direction of text display;blockquote|Specifies a long quotation;body|Specifies the body element;br|Inserts a single line button|Specifies a push button;canvas|Define graphics;caption|Specifies a table caption;cite|Specifies a citation;code|Specifies computer code text;col|Specifies attributes for table columns;colgroup|Specifies groups of table columns;command|Specifies a command;datalist|Specifies an autocomplete dropdown list;dd|Specifies a definition description;del|Specifies deleted text;details|Specifies details of an element;dfn|Defines a definition term;div|Specifies a section in a document;dl|Specifies a definition list;dt|Specifies a definition term;em|Specifies emphasized text;embed|Specifies external application or interactive content;eventsource|Specifies a target for events sent by a server;fieldset|Specifies a fieldset;figcaption|Specifies caption for the figure element.;figure|Specifies a group of media content, and their caption;footer|Specifies a footer for a section or page;form|Specifies a form;h1|Specifies a heading level 1;h2|Specifies a heading level 2;h3|Specifies a heading level 3;h4|Specifies a heading level 4;h5|Specifies a heading level 5;h6|Specifies a heading level 6;head|Specifies information about the document;header|Specifies a group of introductory or navigational aids, including hgroup elements;hgroup|Specifies a header for a section or page;hr|Specifies a horizontal rule;html|Specifies an html document;i|Specifies italic text;iframe|Specifies an inline sub window (frame);img|Specifies an image;input|Specifies an input field;ins|Specifies inserted text;kbd|Specifies keyboard text;keygen|Generates a key pair;label|Specifies a label for a form control;legend|Specifies a title in a fieldset;li|Specifies a list item;link|Specifies a resource reference;mark|Specifies marked text;map|Specifies an image map;menu|Specifies a menu list;meta|Specifies meta information;meter|Specifies measurement within a predefined range;nav|Specifies navigation links;noscript|Specifies a noscript section;object|Specifies an embedded object;ol|Specifies an ordered list;optgroup|Specifies an option group;option|Specifies an option in a drop-down list;output|Specifies some types of output;p|Specifies a paragraph;param|Specifies a parameter for an object;pre|Specifies preformatted text;progress|Specifies progress of a task of any kind;q|Specifies a short quotation;ruby|Specifies a ruby annotation (used in East Asian typography);rp|Used for the benefit of browsers that don't support ruby annotations;rt|Specifies the ruby text component of a ruby annotation.;samp|Specifies sample computer code;script|Specifies a script;section|Specifies a section;select|Specifies a selectable list;small|Specifies small text;source|Specifies media resources;span|Specifies a section in a document;strong|Specifies strong text;style|Specifies a style definition;sub|Specifies subscripted text;summary|Specifies a summary/caption for the  details> element;sup|Specifies superscripted text;table|Specifies a table;tbody|Specifies a table body;td|Specifies a table cell;textarea|Specifies a text area;tfoot|Specifies a table footer;th|Specifies a table header;thead|Specifies a table header;time|Specifies a date/time;title|Specifies the document title;tr|Specifies a table row;ul|Specifies an unordered list;var|Specifies a variable;video|Specifies a video;wbr|Specifies a line break opportunity for very long words and strings of text with no spaces.;";

            defaultPresets["#htmlCommonTags#"] = "a|Hyperlink;p|Paragraf;";

            String tmp =
                "@tagName|Name of xmlNode tag;@innerText|Node inner value;@innerXML|Node inner xml;@outerXML|Node outer xml;@allInnerText|All visible text inside node;@attLine|XML code line with attributes;";
            tmp +=
                "@hasChildren|If node has children;@hasRelatives|If node has relatives;@namespace|Namespace of node;@count|Children count;@attToCsv|attributes to CSV;@attToDList|Attributes to DefaultList format;";
            tmp +=
                "@xPath|xPath of node;@index|Index at parent;@JSON|JSON representation of node;@MPS|Multi Path Structure format;@valueNotNull|InnerText is not empty string;@attributeExists";
            defaultPresets["#nodeValueSource#"] = tmp;

            defaultPresets["#attributesCommon#"] =
                "href|Hiper link;src|Source;width|Width;height|Height;class|Class attribute;id|Id attribute;";

            defaultPresets["#attributes#"] =
                "abbr|abbr;accept-charset|accept-charset;accept|accept;accesskey|accesskey;action|action;align|align;alink|alink;alt|alt;archive|archive;axis|axis;background|background;bgcolor|bgcolor;border|border;cellpadding|cellpadding;cellspacing|cellspacing;char|char;charoff|charoff;charset|charset;checked|checked;cite|cite;class|class;classid|classid;clear|clear;code|code;codebase|codebase;codetype|codetype;color|color;cols|cols;colspan|colspan;compact|compact;content|content;coords|coords;data|data;datetime|datetime;declare|declare;defer|defer;dir|dir;disabled|disabled;enctype|enctype;face|face;for|for;frame|frame;frameborder|frameborder;headers|headers;height|height;href|href;hreflang|hreflang;hspace|hspace;http-equiv|http-equiv;id|id;ismap|ismap;label|label;lang|lang;language|language;link|link;longdesc|longdesc;marginheight|marginheight;marginwidth|marginwidth;maxlength|maxlength;media|media;method|method;multiple|multiple;name|name;nohref|nohref;noresize|noresize;noshade|noshade;nowrap|nowrap;object|object;onblur|onblur;onchange|onchange;onclick|onclick;ondblclick|ondblclick;onfocus|onfocus;onkeydown|onkeydown;onkeypress|onkeypress;onkeyup|onkeyup;onload|onload;onmousedown|onmousedown;onmousemove|onmousemove;onmouseout|onmouseout;onmouseover|onmouseover;onmouseup|onmouseup;onreset|onreset;onselect|onselect;onsubmit|onsubmit;onunload|onunload;profile|profile;prompt|prompt;readonly|readonly;rel|rel;rev|rev;rows|rows;rowspan|rowspan;rules|rules;scheme|scheme;scope|scope;scrolling|scrolling;selected|selected;shape|shape;size|size;span|span;src|src;standby|standby;start|start;style|style;summary|summary;tabindex|tabindex;target|target;text|text;title|title;type|type;usemap|usemap;valign|valign;value|value;valuetype|valuetype;version|version;vlink|vlink;vspace|vspace;width|width;";

            defaultPresets["#nodeSpliters#"] =
                "{nl}|new line; {dc}|dot comma; {dd}|dobule dot; {dcnl}|dot comma, newline; {tb}|tab;";

            defaultPresets["#indexing#"] =
                "1-5|Getting elements from index 1 to index 5; 2-4:7-9|Getting elements from 2 to 4 and from 7 to 9;3>|Getting elements from 3 to end;<5|Getting elements from start to 5;2nth|gets every second element;5nth|gets every fifth element;";

            defaultPresets["#htmlPhaserEngine#"] =
                defaultPresets["#htmlPhaserEngines#"] =
                "xmlDocument|Standard .NET XmlDocument phaser;systemHtml|Small and quick HTML2XML phaser, removes non-html tags*;htmlAgilityPack|Robust HTML2XML engine;htmlAgilitySafeMode|Maximum compatibility settings;JSON2XML|JSON object to XML representation;byPass|Load string data into input string and output string;";

            defaultPresets["#comparisonPolicy#"] =
                "exact|Totally same string;caseFree|Ignoring case differences;trimSpaceExact|Trims needle and source;trimSpaceCaseFree|Triming and case ignorant test;trimSpaceContainExact|Trimming and testing for substring;trimSpaceContainCaseFree|The most tolerant test*;containExact|Searching for exact substring;containCaseFree|Searching for substring ignoring case;length|same size test;overrideTrue|returnes true always;overrideFalse|returnes false always;atBeginningCaseFree|Testing for substring at beginning of the source;atEndCaseFree|Testing for substring at the end of the source;lengthMore|Length is more than needle - may be number of chars or length of string will be taken;lengthLess|Length is less or same than needle - may be number of chars or length of string will be taken;";

            defaultPresets["#elementType#"] =
                "all|whole input;c|chars;w|words;sg|space splited segments;ln|lines*;se|sentences;cs_|Custom separator place it after _ character;";

            defaultPresets["#comparisonOperators#"] =
                "=|Equal;==|Equal (same);>|Bigger;<|Smaller;<=|Smaller and equal;>=|Bigger and equal;<>|Not equal;";

            defaultPresets["#pathElements#"] =
                "d|Get directory part;f|Filename without extension;e|File extension only;fe|Filename with extension;h|Host - domain name with http;q|Query parameter;p|Url path before file name;qn|Query parameter by name;";

            defaultPresets["#numberFormats#"] =
                "BMK|Convert B, M and K multiplier notations to number;C|currency;D1|One digit;D3|Three digits;D5|Five digits;D10|Ten digits;F2|Fixed point decimals;F4|Fixed point decimals 2,0000;E|Exponent;P|Percent;X|Hexadecimal;clockToDec|Converting MM:SS clock time to decimal seconds;";

            defaultPresets["#numberSufix#"] =
                "$|Dollar;€|Euro;£|UK Phound;?|Number;%|Percent;‰|Promil;°|Angular point;¢|Cent;¥|Yen;";

            defaultPresets["#regex#"] = "\n|Matches a newline character;";

            defaultPresets["#urlShema#"] =
                "file|The resource is a file on the local computer. ;ftp|The resource is accessed through FTP. ;gopher|The resource is accessed through the Gopher protocol. ;http|The resource is accessed through HTTP. ;https|The resource is accessed through SSL-encrypted HTTP. ;ldap|The resource is accessed through the LDAP protocol. ;mailto|The resource is an e-mail address and accessed through the SMTP protocol. ;net.pipe|The resource is accessed through a named pipe. ;net.tcp|The resource is accessed from TCP endpoint. ;news|The resource is accessed through the NNTP protocol. ;nntp|The resource is accessed through the NNTP protocol. ;telnet|The resource is accessed through the TELNET protocol. ;uuid|The resource is accessed through a unique UUID endpoint name for communicating with a service. ;";

            defaultPresets["#urlMethod#"] = "GET|Get parameter encoding*;POST|Post parameter encoding;";

            defaultPresets["#alexaTags#"] = "ContactInfo|Contact parent node;DataUrl|Url with info on contact;";

            defaultPresets["#xPath#"] =
                "./author | All <author> elements within the current context. Note that this is equivalent to the expression in the next row.;author | All <author> elements within the current context.;first.name | All <first.name> elements within the current context.;/bookstore | The document element ( <bookstore>) of this document.;//author | All <author> elements in the document.;book[/bookstore/@specialty=@style] | All <book> elements whose style attribute value is equal to the specialty attribute value of the <bookstore> element at the root of the document.;author/first-name | All <first-name> elements that are children of an <author> element.;bookstore//title | All <title> elements one or more levels deep in the <bookstore> element (arbitrary descendants). Note that this is different from the expression in the next row.;bookstore/*/title | All <title> elements that are grandchildren of <bookstore> elements.;bookstore//book/excerpt//emph | All <emph> elements anywhere inside <excerpt> children of <book> elements, anywhere inside the <bookstore> element.;.//title | All <title> elements one or more levels deep in the current context. Note that this situation is essentially the only one in which the period notation is required.;author/* | All elements that are the children of <author> elements.;book/*/last-name | All <last-name> elements that are grandchildren of <book> elements.;";

            defaultPresets["#xPathExpression#"] =
                "book[last()] |The last <book> element of the current context node. ; book/author[last()] | The last <author> child of each <book> element of the current context node. ; (book/author)[last()] | The last <author> element from the entire set of <author> children of <book> elements of the current context node. ; book[excerpt] | All <book> elements that contain at least one <excerpt> element child. ; book[excerpt]/title | All <title> elements that are children of <book> elements that also contain at least one <excerpt> element child. ; book[excerpt]/author[degree] | All <author> elements that contain at least one <degree> element child, and that are children of <book> elements that also contain at least one <excerpt> element. ; book[author/degree] | All <book> elements that contain <author> children that in turn contain at least one <degree> child. ; author[degree][award] | All <author> elements that contain at least one <degree> element child and at least one <award> element child. ; author[degree and award] | All <author> elements that contain at least one <degree> element child and at least one <award> element child. ; author[(degree or award) and publication] | All <author> elements that contain at least one <degree> or <award> and at least one <publication> as the children ; author[degree and not(publication)] | All <author> elements that contain at least one <degree> element child and that contain no <publication> element children. ; author[not(degree or award) and publication] | All <author> elements that contain at least one <publication> element child and contain neither <degree> nor <award> element children. ; author[last-name = \"Bob\"] | All <author> elements that contain at least one <last-name> element child with the value Bob. ; author[last-name[1] = \"Bob\"] | All <author> elements where the first <last-name> child element has the value Bob. Note that this is equivalent to the expression in the next row. ; author[last-name [position()=1]= \"Bob\"] | All <author> elements where the first <last-name> child element has the value Bob. ; degree[@from != \"Harvard\"] | All <degree> elements where the from attribute is not equal to \"Harvard\". ; author[. = \"Matthew Bob\"] | All <author> elements whose value is Matthew Bob. ; book[position() &lt;= 3] | The first three books (1, 2, 3). ; author[not(last-name = \"Bob\")] | All <author> elements that do no contain <last-name> child elements with the value Bob. ; author[first-name = \"Bob\"] | All <author> elements that have at least one <first-name> child with the value Bob. ; author[* = \"Bob\"] | all author elements containing any child element whose value is Bob. ; author[last-name = \"Bob\" and first-name = \"Joe\"] | All <author> elements that has a <last-name> child element with the value Bob and a <first-name> child element with the value Joe. ; price[@intl = \"Canada\"] | All <price> elements in the context node which have an intl attribute equal to \"Canada\". ; degree[position() &lt; 3] | The first two <degree> elements that are children of the context node. ; p/text()[2] | The second text node in each <p> element in the context node. ; ancestor::book[1] | The nearest <book> ancestor of the context node. ; ancestor::book[author][1] | The nearest <book> ancestor of the context node and this <book> element has an <author> element as its child. ; ancestor::author[parent::book][1] | The nearest <author> ancestor in the current context and this <author> element is a child of a <book> element.;";
            defaultPresets["#xPathExpressions#"] = defaultPresets["#xPathExpression#"];
            defaultPresets["#xPathCommon#"] =
                "//*|All elements in document;//h:a|All links in document;//h:*[@src]|All elements having src attribute;//*[@*]|All elements having any attribute";

            defaultPresets["#mathComparators#"] =
                "or|logical OR;and|logical AND;=|Equal;==|Equal;!=|Not equal;<>|Not equal;>|More;>=|Mora or same;<|Less;<=|Less or same;";

            defaultPresets["#mathOperators#"] = "+|Plus;-|Minus;*|Multiply;%|Modulus;/|Divide;";

            defaultPresets["#mathFunctions#"] =
                "Abs|Returns the absolute value of a specified number Abs(-1) ;Acos|Returns the angle whose cosine is the specified number Acos(1) ;Asin|Returns the angle whose sine is the specified number Asin(0) ;Atan|Returns the angle whose tangent is the specified number Atan(0) ;Ceiling|Returns the smallest integer greater than or equal to the specified number Ceiling(1 5) ;Cos|Returns the cosine of the specified angle Cos(0) ;Exp|Returns e raised to the specified power Exp(0) ;Floor|Returns the largest integer less than or equal to the specified number Floor(1 5) ;IEEERemainder|Returns the remainder resulting from the division of a specified number by another specified number IEEERemainder(3 2) ;Log|Returns the logarithm of a specified number Log(1 10) ;Log10|Returns the base 10 logarithm of a specified number Log10(1) ;Max|Returns the larger of two specified numbers Max(1 2) ;Min|Returns the smaller of two numbers Min(1 2) ;Pow|Returns a specified number raised to the specified power Pow(3 2) ;Round|Rounds a value to the nearest integer or specified number of decimal places The mid number behaviour can be changed by using EvaluateOption RoundAwayFromZero during construction of the Expression object Round(3 222 2) ;Sign|Returns a value indicating the sign of a number Sign(-10) ;Sin|Returns the sine of the specified angle Sin(0) ;Sqrt|Returns the square root of a specified number Sqrt(4) ;Tan|Returns the tangent of the specified angle Tan(0) ;Truncate|Calculates the integral part of a number Truncate(1 7) ;";

            defaultPresets["#mathTests#"] =
                "if(3 % 1=1, 'true', 'false)|IF example;in(2+1,3,5,6)|Tests if value is in set of values;";

            defaultPresets["#mathPrimeri#"] = "($(1) + $(2)) / 1000|Sabira dve lokalne varijable i deli sa 1000;";

            defaultPresets["#seoScoreMode#"] =
                "sum|Summary of all ocurences in the list;max|Only the best possition is calculated;avg|Average possition is calculated;";

            defaultPresets["#seoCurveMode#"] =
                "imbSeo|imb inverse exponential distribution curve;linear|simple linear distribution;";

            defaultPresets["#seoLength#"] =
                "*40|imbSEO30 test - testing first 3 pages;*75|imbSEO60 test - Extended list for competition monitoring;200|Deep market scan;";

            defaultPresets["#seoKeywordRange#"] =
                "0-15|Calculating score for keywords of 0 row to 15th row;5>|From 5th row to end;2nth|Every second row;";

            defaultPresets["#seoFinalSum#"] =
                "fsum|Sum scores from all lists;favg|Average SEO score for set of keywords;";

            defaultPresets["#excelCellFormat#"] =
                "General| Default cell format;0| Whole number;0.00| Number with two decimal places;#,##0| Thousands without decimals;#,##0.00|Thousands with two decimals;0%| Percentage;0.00%| Percentage with two decimal places;0.00E+00| Cell format;# ?/?| Cell format;# ??/??| Cell format;d/m/yyyy| Date format - Serbia;d-mmm-yy| Date format;d-mmm| Date format;mmm-yy| Date format;h mm tt| Cell Date format;h mm ss tt| Cell Date format;H mm| Time format;H mm ss| Cell format;m/d/yyyy H mm| Cell format;#,##0 (# ##0)| Cell format;#,##0 [Red](#,##0)| Cell format;# ##0 00 (# ##0 00)| Cell format;# ##0 00 [Red](# ##0 00)| Cell format;mm ss| Cell format;[h] mm ss| Cell format;mmss 0| Cell format;##0.0E+0| Exp expression;@| Cell format;";

            defaultPresets["#DBtypes#"] =
                "String|Text value*;Boolean|True or false;Byte|Byte;Char|One character;DateTime|Date and Time;Decimal| ;Double| ;Int16| ;Int32|Natural number;Int64| ;SByte| ;Single| ;TimeSpan| ;UInt16| ;UInt32| ;UInt64| ;";

            defaultPresets["#DBAggregation#"] =
                "Skip|Skip ADO aggregation*;Sum|(Sum);Avg|(Average);Min|(Minimum);Max|(Maximum);Count|(Count);StDev|(Statistical standard deviation);Var|(Statistical variance);";

            defaultPresets["#scope#"] =
                "local|Local variable scope - only this script;global|Global variable scope - whole system;";

            defaultPresets["#includes#"] = "CH|include childs; CO|Childs only;OT|Only target*;";

            defaultPresets["#relativeMode#"] =
                "endNodes|Krajnje nodove;upToLevel|Do određenog nivoa u dubinu;downToLevel|Od kraja do određene dubine*;onLevel|Tačno na određenom novou;brothers|Braća;parentNodes|Selektuje parent nodes - do odredjene dubine u nazad.;";
            defaultPresets["#levels#"] = "1|Prvi nivo;2|Drugi nivo;3|Treći nivo;";
        }

        /// <summary>
        /// Vraća Default Preset list - prema zadatom presetName-u
        /// U većini slučajeva je bolje koristiti getSeparatedPresetLists - jer podržava više defaultList poziva
        /// </summary>
        /// <param name="presetName"></param>
        /// <returns>String u DefaultPresetList formatu</returns>
        internal static String getDefaultPreset(String presetName)
        {
            return defaultPresets[presetName];  //defaultPresets.getTypedItem<String>(presetName, imbCollection.loadType.keyIfNotFound);
        }

        /// <summary>
        /// Vraca element default liste koji ima neki needle
        /// </summary>
        /// <param name="defaultListString">lista</param>
        /// <param name="needle">needle. * znaci da je podrazumevani, {} da podrzava multiopts, $$$ da podrzava dinamicka polja</param>
        /// <param name="whatElement">value, desc ili all</param>
        /// <returns></returns>
        public static String getDefaultListElementWith(String defaultListString, String needle,
                                                       presetListElement whatElement = presetListElement.value)
        {
            List<String> output;

            output = getDefaultListElement(defaultListString, whatElement);

            foreach (String el in output)
            {
                if (el.Contains(needle))
                {
                    return el;
                }
            }
            return "";
        }

        /// <summary>
        /// V3: Za svaki #defaultPresetListCall# pravi poseban Dictionary item . Sve stavke koje nisu u ## liste odvaja u [0] element
        /// Koristiti: kada očekuješ više #defList1##defList2# poziva u stringu
        /// </summary>
        /// <param name="defaultListString">String linija sa svim default listama</param>
        /// <returns>Recnik sa svim listama. Key je naziv pod liste, Value je spisak default vrednosti </returns>
        public static Dictionary<String, String> getSeparatedPresetLists(String defaultListString)
        {
            Dictionary<String, String> output = new Dictionary<String, String>();
            String regPattern = "\\#(.*?)\\#";

            MatchCollection result = Regex.Matches(defaultListString, regPattern);

            String rootList = Regex.Replace(defaultListString, regPattern, "");
            if (!String.IsNullOrEmpty(rootList))
            {
                output.Add("Main defaults", rootList);
            }

            foreach (Match item in result)
            {
                String presetName = item.Value.Replace("#", "");

                if (output.ContainsKey(presetName))
                {
                    output[presetName] = getDefaultPreset(presetName);
                }
                else
                {
                    output.Add(presetName, getDefaultPreset(presetName));
                }
            }

            return output;
        }

        /// <summary>
        /// Pretvara #defaultListe# u defaultList format
        /// </summary>
        /// <param name="defaultList"></param>
        /// <returns></returns>
        public static String getProcessedPresetList(String defaultList)
        {
            String output = defaultList;

            if (String.IsNullOrEmpty(defaultList)) return "";

            String regPattern = "\\#(.*?)\\#";

            MatchCollection result = Regex.Matches(defaultList, regPattern);

            foreach (Match item in result)
            {
                String presetName = item.Value;
                output = output.Replace(presetName, getDefaultPreset(presetName));
            }

            return output;
        }

        /// <summary>
        /// Vraća listu stringova na osnovu DefaultList stringa
        /// </summary>
        /// <param name="defaultList">List u DefaultString formatu</param>
        /// <param name="element">elementi su: value, desc, all</param>
        /// <returns></returns>
        public static List<String> getDefaultListElement(String defaultList,
                                                         presetListElement element = presetListElement.value)
        {
            List<String> output = new List<string>();

            List<String> defVal = new List<String>();

            defVal.AddRange(defaultList.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
            foreach (String item in defVal)
            {
                String[] arr = item.Split("|".ToCharArray());
                switch (element)
                {
                    default:
                    case presetListElement.value:
                        output.Add(arr[0]);
                        break;

                    case presetListElement.desc:
                        output.Add(arr[1]);
                        break;

                    case presetListElement.all:
                        output.Add(item);
                        break;
                }
            }
            return output;
        }

        /// <summary>
        /// Vraća listu stringova na osnovu DefaultList stringa
        /// </summary>
        /// <param name="defaultList">List u DefaultString formatu</param>
        /// <param name="element">elementi su: value, desc, all</param>
        /// <returns></returns>
        public static Dictionary<String, String> getDefaultListDictionary(String defaultList)
        {
            Dictionary<String, String> output = new Dictionary<String, String>();

            List<String> defVal = new List<String>();

            defVal.AddRange(defaultList.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
            foreach (String item in defVal)
            {
                String[] arr = item.Split("|".ToCharArray());
                if (arr.Count() > 0)
                {
                    String vl = arr[0];
                    if (arr.Count() > 1)
                    {
                        vl = arr[1];
                    }
                    output.Add(arr[0], vl);
                }
            }
            return output;
        }

        /// <summary>
        /// Pravi String u DefaultList formatu iz niza vrednosti - ako je niz opisa null onda koristi default opis
        /// </summary>
        /// <param name="values">Lista sa vrednostima</param>
        /// <param name="desc">List sa opisima</param>
        /// <param name="defaultDescSuffix">Suffix da doda posle countera u descriptionu</param>
        /// <returns></returns>
        public static String getDefaultListString(List<String> values, List<String> desc = null,
                                                  String defaultDescSuffix = "preset value")
        {
            String output = "";
            Int32 c = 0;

            if (desc == null)
            {
                desc = new List<string>();
            }

            String tmpDesc = "";
            foreach (String val in values)
            {
                if (c < desc.Count())
                {
                    tmpDesc = desc[c];
                }
                else
                {
                    tmpDesc = "[" + c + "] " + defaultDescSuffix;
                }

                output = output + val + "|" + tmpDesc + ";";
                c++;
            }

            return output;
        }
    }
}