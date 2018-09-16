// --------------------------------------------------------------------------------------------------------------------
// <copyright file="conversionRuleSet.cs" company="imbVeles" >
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
using imbSCI.Core.files.folders;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.IO;

namespace imbSCI.Core.syntax.converter.core
{
    public class conversionRuleSet : List<conversionRule>
    {
        public conversionRule Add()
        {
            conversionRule rule = new conversionRule();
            Add(rule);
            return rule;
        }

        public void process()
        {
            var fileList = inputFolder.findFiles("*" + inputExtension, System.IO.SearchOption.AllDirectories);

            foreach (String fl in fileList)
            {
                String code = File.ReadAllText(fl);

                foreach (conversionRule r in this)
                {
                    if (r.multiline) code = r.ProcessLine(code);
                }

                List<String> codeLines = code.SplitSmart(Environment.NewLine, "", false, false);

                List<String> codeOutput = new List<string>();

                foreach (String l in codeLines)
                {
                    String lo = l;
                    foreach (conversionRule r in this)
                    {
                        if (!r.multiline)
                        {
                            lo = r.ProcessLine(lo);
                        }
                    }
                    codeOutput.Add(lo);
                }

                String fn = Path.GetFileNameWithoutExtension(fl);

                String directoryInfo = fl.removeEndsWith(Path.GetFileName(fl));
                String dir = directoryInfo.removeStartsWith(inputFolder.path);

                var of = outputFolder.Add(dir, "", "");

                String op = of.pathFor(fn.add(outputExtension, "."), Data.enums.getWritableFileMode.newOrExisting);

                File.WriteAllLines(op, codeOutput);
            }
        }

        public conversionRuleSet(folderNode _in, folderNode _out)
        {
            inputFolder = _in;
            outputFolder = _out;
        }

        public String inputExtension { get; set; } = ".java";

        public String outputExtension { get; set; } = ".cs";

        public folderNode inputFolder { get; set; }

        public folderNode outputFolder { get; set; }
    }
}