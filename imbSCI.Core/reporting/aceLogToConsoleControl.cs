// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceLogToConsoleControl.cs" company="imbVeles" >
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
using imbSCI.Core.extensions.text;
using imbSCI.Data;

namespace imbSCI.Core.reporting
{
    using imbSCI.Core.extensions.data;
    using imbSCI.Data.collection.special;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Helper class that contains list of IConsoleControls that have permission to write to console
    /// </summary>
    public class aceLogToConsoleControl
    {
        private Dictionary<IConsoleControl, String> _prefixRegistar = new Dictionary<IConsoleControl, String>();

        /// <summary> </summary>
        protected virtual Dictionary<IConsoleControl, String> prefixRegistar
        {
            get
            {
                return _prefixRegistar;
            }
            set
            {
                _prefixRegistar = value;
            }
        }

        protected circularSelector<ConsoleColor> colorSelector = new circularSelector<ConsoleColor>(ConsoleColor.White, ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Yellow, ConsoleColor.Magenta, ConsoleColor.Gray, ConsoleColor.DarkCyan);

        private Dictionary<IConsoleControl, ConsoleColor> _colorRegistar = new Dictionary<IConsoleControl, ConsoleColor>();

        /// <summary>Registar of color-to-source pairs</summary>
        protected virtual Dictionary<IConsoleControl, ConsoleColor> colorRegistar
        {
            get
            {
                return _colorRegistar;
            }
            set
            {
                _colorRegistar = value;
            }
        }

        /// <summary>
        /// Sets as only output - removes all the rest
        /// </summary>
        /// <param name="loger">The loger.</param>
        public virtual void setAsOnlyOutput(IConsoleControl loger, String prefix = "")
        {
            colorRegistar.Clear();
            prefixRegistar.Clear();
            allowList.Clear();
            setAsOutput(loger, prefix);
        }

        /// <summary>
        /// Sets the log file path
        /// </summary>
        /// <param name="path">The path.</param>
        public virtual void setLogFileWriter(String path = null)
        {
            if (_logWritter != null)
            {
                _logWritter.Close();
                logFileWriteOn = false;
            }

            if (path == null)
            {
                if (_logWritter != null)
                {
                    logFileWriteOn = false;
                }
            }
            else
            {
                _logWritter = File.AppendText(path);
                logFileWriteOn = true;
            }
        }

        public Boolean logFileWriteOn { get; set; } = false;

        private TextWriter _logWritter;

        #region imbObject Property <TextWriter> logWritter

        /// <summary>
        /// imbControl property logWritter tipa TextWriter
        /// </summary>
        public TextWriter logWritter
        {
            get
            {
                if (_logWritter == null)
                {
                    String p = "diagnostic\\log.md";
                    // p.getWritableFile(getWritableFileMode.autoRenameExistingToOldOnce);
                    _logWritter = File.AppendText(p);
                }

                return _logWritter;
            }
            set
            {
                _logWritter = value;
            }
        }

        #endregion imbObject Property <TextWriter> logWritter

        private Object setAsOutputLock = new Object();

        /// <summary>
        /// Makes sure that <c>loggerA</c> and <c>loggerB</c> have different colors assigned for their outputs to the Console buffer.
        /// </summary>
        /// <param name="loggerA">The logger a.</param>
        /// <param name="loggerB">The logger b.</param>
        /// <returns>true if the loggers have different colors, false if one of loggers is not in the current output or if otherwise failed to update color of <c>loggerB</c></returns>
        public virtual Boolean makeSureHaveDifferentColors(IConsoleControl loggerA, IConsoleControl loggerB)
        {
            if (!colorRegistar.ContainsKey(loggerA)) return false;
            if (!colorRegistar.ContainsKey(loggerB)) return false;

            if (colorRegistar[loggerA] != colorRegistar[loggerB]) return true;

            Int32 limit = 10;
            while (colorRegistar[loggerA] == colorRegistar[loggerB])
            {
                limit--;
                colorRegistar[loggerB] = colorSelector.next(1);
                if (limit < 0) return false;
            }

            return true;
        }

#pragma warning disable CS1574 // XML comment has cref attribute 'log(string)' that could not be resolved
        /// <summary>
        /// Allows access to the console output bugger to the <c>logger</c> and sets its color and prefix. If you call this method on logger that was already set to output, it will result in changed color
        /// </summary>
        /// <param name="logger">The logger assigned to the console output</param>
        /// <param name="prefix">The prefix that will appear in front of each <see cref="IAceLogable.log(string)"/> call. It will update it if already registered prefix exist</param>
        /// <param name="preferedColor">If not <see cref="ConsoleColor.Black"/> - it will assign specified color to the logger. Otherwise, if <see cref="ConsoleColor.Black"/> is specified, it will assign automatically a color from the <see cref="colorSelector"/></param>
        public virtual void setAsOutput(IConsoleControl logger, String prefix = "", ConsoleColor preferedColor = ConsoleColor.Black)
#pragma warning restore CS1574 // XML comment has cref attribute 'log(string)' that could not be resolved
        {
            lock (setAsOutputLock)
            {
                if (!colorRegistar.ContainsKey(logger))
                {
                    if (preferedColor != ConsoleColor.Black)
                    {
                        colorRegistar.Add(logger, preferedColor);
                    }
                    else
                    {
                        colorRegistar.Add(logger, colorSelector.next());
                    }
                }
                else
                {
                    if (preferedColor != ConsoleColor.Black)
                    {
                        colorRegistar[logger] = preferedColor;
                    }
                    else
                    {
                        colorRegistar[logger] = colorSelector.next();
                    }
                }

                if (!allowList.Contains(logger))
                {
                    allowList.Add(logger);
                }
                if (!prefix.isNullOrEmpty())
                {
                    prefix = prefix.toWidthExact(8, " ") + ": ";
                }

                if (!prefixRegistar.ContainsKey(logger))
                {
                    prefixRegistar.Add(logger, prefix);
                }
                else
                {
                    prefixRegistar[logger] = prefix;
                }

                logger.VAR_AllowInstanceToOutputToConsole = true;
            }
        }

        /// <summary>
        /// Replaces the two loggers in the console output
        /// </summary>
        /// <param name="oldLogger">The old loger.</param>
        /// <param name="newLogger">The new loger.</param>
        public virtual void replaceOutput(IConsoleControl oldLogger, IConsoleControl newLogger)
        {
            removeFromOutput(oldLogger);
            setAsOutput(newLogger);
        }

        private IConsoleControl lastLogerToWrite = null;

        private Boolean _convertToDos = true;

        /// <summary>
        /// IT will convert all characters sent to output into compatibile DOS encoding
        /// </summary>
        public Boolean convertToDos
        {
            get { return _convertToDos; }
            set { _convertToDos = value; }
        }

        private toDosCharactersMode _encodeMode = toDosCharactersMode.toCleanChars;

        /// <summary>
        ///
        /// </summary>
        public toDosCharactersMode encodeMode
        {
            get { return _encodeMode; }
            set { _encodeMode = value; }
        }

        /// <summary>
        /// Preprocesses the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="loger">The loger.</param>
        /// <returns></returns>
        protected virtual List<String> preprocessMessage(String message, IConsoleControl loger)
        {
            List<String> lines = new List<string>();
            foreach (String line in message.breakLines(false))
            {
                String li = line;
                //if (convertToDos) li = li.toDosCharacters(toDosCharactersMode.toCleanChars);
                if (!String.IsNullOrWhiteSpace(li))
                    lines.Add(li.ensureStartsWith(prefixRegistar[loger]));
            }
            return lines;
        }

        private Object writeToLogWritterLock = new Object();

        /// <summary>
        /// The highlight regex - used for all console output
        /// </summary>
        private static Regex reg_highlight = new Regex(@" _([\w\s\.\\_\-\:\-,\%\?\!\^\\$#~]*)_ ");

        /// <summary>
        /// Writes to console.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="loger">The loger.</param>
        /// <param name="breakline">if set to <c>true</c> [breakline].</param>
        /// <param name="altColor">Color of the alt.</param>
        public virtual void writeToConsole(String message, IConsoleControl loger, Boolean breakline, Int32 altColor = -1)
        {
            lock (writeToLogWritterLock)
            {
                if (!checkOutputPermission(loger)) return;

                try
                {
                    if (loger != lastLogerToWrite) Console.WriteLine("--");

                    Console.ForegroundColor = colorRegistar[loger];

                    if (altColor > 0)
                    {
                        Console.ForegroundColor = colorSelector.getAlternative(Console.ForegroundColor, altColor);
                    }

                    if (breakline)
                    {
                        List<String> lines = preprocessMessage(message, loger);

                        foreach (String ln in lines)
                        {
                            writeLine(ln, loger);
                            //Thread.Sleep(1);

                            //if (logFileWriteOn) logWritter.WriteLine(ln);
                        }
                    }
                    else
                    {
                        writeLine(message, loger);
                    }

                    lastLogerToWrite = loger;

                    Console.ForegroundColor = ConsoleColor.White;
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        protected virtual void writeLine(String ln, IConsoleControl loger)
        {
            if (reg_highlight.IsMatch(ln))
            {
                var mchs = reg_highlight.Matches(ln);

                var splits = reg_highlight.Split(ln);

                Int32 i = -1;
                foreach (String sp in splits)
                {
                    Console.ForegroundColor = colorRegistar[loger];

                    if (i == 1)
                    {
                        Console.ForegroundColor = colorSelector.getAlternative(Console.ForegroundColor, i);
                    }

                    i = -i;

                    //Thread.Sleep(1);

                    //if (logFileWriteOn) logWritter.WriteLine(ln);

                    if (sp == Enumerable.Last<string>(splits))
                    {
                        Console.WriteLine(sp);
                    }
                    else
                    {
                        Console.Write(sp);
                    }
                }
            }
            else
            {
                Console.WriteLine(ln);
            }

            if (logFileWriteOn) logWritter.Write(ln);
        }

        /// <summary>
        /// TRUE: when <see cref="allowList"/> is empty. Then someone else may print out
        /// </summary>
        /// <value>
        ///   <c>true</c> if [no one on output now]; otherwise, <c>false</c>.
        /// </value>
        public Boolean noOneOnOutputNow
        {
            get
            {
                return !allowList.Any();
            }
        }

        /// <summary>
        /// Checks if this loger has output permission. Checks only the globaly defined list, not members of <see cref="IConsoleControl"/>
        /// </summary>
        /// <param name="loger">The loger.</param>
        /// <returns></returns>
        public virtual Boolean checkOutputPermission(IConsoleControl loger)
        {
            try
            {
                return allowList.Contains(loger);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured during permission check");
                return true;
            }
        }

        private Object removeFromOutputLock = new Object();

        /// <summary>
        /// Removes this loger from output permission
        /// </summary>
        /// <param name="loger">The loger.</param>
        public virtual void removeFromOutput(IConsoleControl loger)
        {
            //writeToConsole(loger.GetType().Name + ": went off", loger, true);
            lock (removeFromOutputLock)
            {
                if (colorRegistar.ContainsKey(loger)) colorRegistar.Remove(loger); //, colorSelector.next());
                if (allowList.Contains(loger))
                {
                    allowList.Remove(loger);
                    loger.VAR_AllowInstanceToOutputToConsole = false;
                }
            }
        }

        private List<IConsoleControl> _allowList = new List<IConsoleControl>();

        /// <summary> </summary>
        protected List<IConsoleControl> allowList
        {
            get
            {
                return _allowList;
            }
            set
            {
                _allowList = value;
            }
        }
    }
}