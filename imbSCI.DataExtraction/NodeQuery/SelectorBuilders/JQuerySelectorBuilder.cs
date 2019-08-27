using imbSCI.Core.extensions.io;
using imbSCI.Core.extensions.text;
using imbSCI.Data;
using imbSCI.Data.extensions;
using imbSCI.Data.extensions.data;
using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.DataExtraction.NodeQuery.SelectorBuilders
{
    public class JQuerySelectorBuilder
    {
        public JQuerySelectorBuilder()
        {
        }

        public List<String> elements = new List<string>();

        public String ToString()
        {
            StringBuilder sb = new StringBuilder();

            String output = "$(\"";

            String el = "";
            foreach (String element in elements)
            {
                el = el.add(element, ", ");
            }

            output += el + "\")";
            return output;
        }

        public JQuerySelectorBuilder AddIDName(String input)
        {
            input = input.Trim();

            input = input.ensureStartsWith("#");
            elements.Add(input);
            return this;
        }

        public JQuerySelectorBuilder AddClassName(String input)
        {
            input = input.Trim();

            input = input.ensureStartsWith(".");
            elements.Add(input);
            return this;
        }

        public JQuerySelectorBuilder AddTagName(String input)
        {
            input = input.Trim();
            elements.Add(input);
            return this;
        }
    }
}