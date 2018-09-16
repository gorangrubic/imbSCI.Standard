// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ncLineCriteria.cs" company="imbVeles" >
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
namespace imbSCI.Core.syntax.nc.line
{
    using imbSCI.Data.data;
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Kriterijum kojim bira liniju
    /// </summary>
    public class ncLineCriteria : dataBindableBase
    {
        #region -----------  commandCriteria  -------  [What command should line starts with: [custom] to use customCommand variable, [any] to return YES on any command]

        private ncLineCommandPredefined _commandCriteria = ncLineCommandPredefined.MOV; // = new ncLineCommandPredefined();

        /// <summary>
        /// What command should line starts with: [custom] to use customCommand variable, [any] to return YES on any command
        /// </summary>
        // [XmlIgnore]
        [Category("ncLineCriteria")]
        [DisplayName("Command")]
        [Description("What command should line starts with: [custom] to use Custom Command variable, [any] to return YES on any command")]
        public ncLineCommandPredefined commandCriteria
        {
            get
            {
                return _commandCriteria;
            }
            set
            {
                // Boolean chg = (_commandCriteria != value);
                _commandCriteria = value;
                OnPropertyChanged("commandCriteria");
                // if (chg) {}
            }
        }

        #endregion -----------  commandCriteria  -------  [What command should line starts with: [custom] to use customCommand variable, [any] to return YES on any command]

        #region -----------  customCommand  -------  [If Command is set to [custom] it will look at this value. Letter-case is ignored. If empty it will return YES on any command - same as Command = [any]]

        private String _customCommand = ""; // = new String();

        /// <summary>
        /// If Command is set to [custom] it will look at this value. Letter-case is ignored. If empty it will return YES on any command - same as Command = [any]
        /// </summary>
        // [XmlIgnore]
        [Category("ncLineCriteria")]
        [DisplayName("Custom Command")]
        [Description("If Command is set to [custom] it will look at this value. Letter-case is ignored. If empty it will return YES on any command - same as Command = [any]")]
        public String customCommand
        {
            get
            {
                return _customCommand;
            }
            set
            {
                // Boolean chg = (_customCommand != value);
                _customCommand = value;
                OnPropertyChanged("customCommand");
                // if (chg) {}
            }
        }

        #endregion -----------  customCommand  -------  [If Command is set to [custom] it will look at this value. Letter-case is ignored. If empty it will return YES on any command - same as Command = [any]]

        /// <summary>
        /// vraca command criteria - custom ili predefined
        /// </summary>
        /// <returns></returns>
        public String getCommandCriteria()
        {
            String output = "";
            if (commandCriteria == ncLineCommandPredefined.custom)
            {
                output = customCommand;
            }
            else
            {
                output = commandCriteria.ToString();
            }
            output = output.ToUpper().Trim();
            return output;
        }

        public virtual Int32 criteriaCount()
        {
            return 1;
        }

        /// <summary>
        /// Testira da li je ispunjen kriterijum komande.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="inverseLogic"></param>
        /// <returns></returns>
        public Boolean testLineCriteria(ncLine line, Boolean inverseLogic = false)
        {
            String lineCommand = line.command.ToUpper().Trim();
            Boolean output;
            if (commandCriteria == ncLineCommandPredefined.any)
            {
                output = true;
            }
            else
            {
                String command = getCommandCriteria();
                if (String.IsNullOrEmpty(command))
                {
                    output = true;
                }
                else
                {
                    output = lineCommand == command;
                }
            }

            if (inverseLogic) output = !output;
            return output;
        }
    }
}