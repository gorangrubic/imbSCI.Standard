// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ncFileTool.cs" company="imbVeles" >
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
namespace imbSCI.Core.syntax.nc
{
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.files.backup;
    using imbSCI.Core.files.job;
    using imbSCI.Core.interfaces;
    using imbSCI.Core.syntax.nc.line;
    using imbSCI.Core.syntax.nc.param;
    using imbSCI.Core.syntax.param;
    using imbSCI.Data;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    //public class aceFileTool:aceFileToolBase
    //{
    //    /// <summary>
    //    /// The main constructor.
    //    /// </summary>
    //    /// <param name="__terminal">AceTerminal instance</param>
    //    public aceFileTool(IAceLogable __terminal) : base(__terminal)
    //    {
    //    }

    //    /// <summary>
    //    /// Returns string information about tool version
    //    /// </summary>
    //    /// <returns></returns>
    //    public override string getVersion()
    //    {
    //        return null;
    //    }

    //    /// <summary>
    //    /// Podesava default current job
    //    /// </summary>
    //    /// <param name="jobName"></param>
    //    public override void setDefaultCurrentJob(string jobName)
    //    {
    //    }

    //    public override string processFile(string filePath, bool doLog = false)
    //    {
    //       return null;
    //    }

    //    public override string processFiles(int startIndex = -1, bool doLog = false, int takeSize = 0, int defaultTakeSize = 0)
    //    {
    //        return null;
    //    }
    //}

    /// <summary>
    /// Alat sa svim logickim operacijama nad NC fajlovima
    /// </summary>
    public class ncFileTool : aceFileToolBase<ncFileJob<ncFile>>
    {
        public override string projectFolderPath
        {
            get
            {
                return "";
            }
        }

        public ncFileTool(IAceLogable __terminal) : base(null, __terminal)
        {
            terminal = __terminal;
        }

        public override String getVersion()
        {
            return "ncFileTool v1.2";
        }

        /// <summary>
        /// Podesava default current job
        /// </summary>
        /// <param name="jobName"></param>
        public override void setDefaultCurrentJob(String jobName)
        {
            currentJob = new ncFileJob<ncFile>();
            currentJob.name = jobName;

            currentJob.scanFiles.fileExtension = ".nc";
            // currentJob.scanFiles.path = "c:\\toppunch\\";
            currentJob.scanFiles.doIncludeSubFolders = true;
            currentJob.scanFiles.backup = backupPolicy.zipSelected;

            currentJob.lineSelector = new ncLineSelector(2);
            currentJob.lineSelector.commandCriteria = ncLineCommandPredefined.MOV;
            currentJob.lineSelector[0].commandCriteria = ncLineCommandPredefined.RIPOSIZIONA;
            currentJob.lineSelector[0].relativePosition = 1;
            currentJob.lineSelector[0].relativeType = ncLineRelativeCriteriaType.onExactPosition;
            currentJob.lineSelector[1].commandCriteria = ncLineCommandPredefined.any;
            currentJob.lineSelector[1].relativePosition = 0;
            currentJob.lineSelector[1].relativeType = ncLineRelativeCriteriaType.disabled;

            currentJob.paramModifiers = new ncParamModifyCollection(3);
            currentJob.paramModifiers[0].modificationType = paramValueModificationType.nochange;
            currentJob.paramModifiers[1].modificationType = paramValueModificationType.increase;
            currentJob.paramModifiers[1].modificationValue = "116.00";
            currentJob.paramModifiers[2].modificationType = paramValueModificationType.nochange;
        }

        public override String processFile(String filePath, Boolean doLog = false)
        {
            String debug = "Process file [" + filePath + "]";

            currentNCFile = new ncFile();
            currentNCFile.load(filePath);

            throw new NotImplementedException();

            List<ncLine> lines = null; //currentNCFile.ncLines.selectLinesBySelector(currentJob.lineSelector);

            String outputPath = "";
            String filename = Path.GetFileName(filePath);

            if (lines.Any())
            {
                outputPath = currentJob.scanFiles.outputPath.add(filename, "\\");

                if (doLog) debug = debug.log("lines selected: [" + lines.Count() + "]");

                foreach (ncLine line in lines)
                {
                    debug = debug.log(currentJob.paramModifiers.applyToLine(line, doLog));
                }
            }
            else
            {
                outputPath = currentJob.scanFiles.outputUnmatchPath.add(filename, "\\");
                if (doLog) debug = debug.log(filePath.add("contain no selectable lines! - not processed"));
            }
            currentNCFile.save(outputPath);
            return debug;
        }

        public override String processFiles(Int32 startIndex = -1, Boolean doLog = false, Int32 takeSize = 0, Int32 defaultTakeSize = 0)
        {
            // String debug = "Process files";

            if (startIndex == -1)
            {
                if (currentNCFile != null)
                {
                    startIndex = currentFileList.targetList.IndexOf(currentNCFile.path);
                }
            }
            if (startIndex == -1)
            {
                startIndex = 0;
            }

            if (takeSize < 1)
            {
                takeSize = currentJob.processTakeSize;
            }
            if (takeSize < 1)
            {
                takeSize = defaultTakeSize;
            }

            Int32 toIndex = startIndex + takeSize;
            toIndex = Math.Min(currentFileList.targetList.Count(), toIndex);
            startIndex = Math.Min(currentFileList.targetList.Count() - 1, startIndex);

            String debug = "Take files [" + startIndex + "] up to [" + toIndex + "]";
            debug = debug.log(this.scanAndBackup(false, true, doLog));

            for (Int32 a = startIndex; a < toIndex; a++)
            {
                String tmp = processFile(currentFileList.targetList[a], doLog);
                if (doLog)
                {
                    debug = debug.log(tmp);
                }
            }

            return debug;
        }
    }
}