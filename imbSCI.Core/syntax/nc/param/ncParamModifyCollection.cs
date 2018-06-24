// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ncParamModifyCollection.cs" company="imbVeles" >
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
namespace imbSCI.Core.syntax.nc.param
{
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.syntax.nc.line;
    using imbSCI.Core.syntax.param;
    using imbSCI.Data.collection;
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Kolekcija sa ncParamModify objektima
    /// </summary>
    public class ncParamModifyCollection : aceCollection<ncParamModify>
    {
        public ncParamModifyCollection()
        {
        }

        /// <summary>
        /// Primenjuje sva modify podesavanja
        /// </summary>
        /// <param name="line">Linija NC koda</param>
        /// <param name="doLog">Da li vraca beleske</param>
        /// <returns>Beleske - ako je doLog=False onda je empty string</returns>
        public String applyToLine(ncLine line, Boolean doLog)
        {
            String debug = "";
            if (doLog) debug = debug.log("Before: " + line.ToString());

            foreach (ncParamModify pm in this)
            {
                //var ncp = line.getParamByKeyOrIndex(pm.parameterTarget);
                //if (ncp == null)
                //{
                //    //line.addToken(pm.
                //    //if (pm.doAddOnMissing)
                //    //{
                //    //    ncp = new ncParam();
                //    //    ncp.format = pm.modificationValue.getFormatFromExample(2);
                //    //    ncp.decValue = 0;

                //    //    if (pm.parIndex > -1)
                //    //    {
                //    //        ncp.index = pm.parIndex;
                //    //        line.addParam(ncp, pm.parIndex);
                //    //    } else
                //    //    {
                //    //        ncp.key = pm.parameterTarget;
                //    //        line.parameters.addParam(ncp);
                //    //    }
                //    //}
                //} else
                //{
                //    ncp.modify(pm.modificationType, pm.modificationValue);
                //    if (pm.doEnforceFormat)
                //    {
                //        ncp.setFormat(pm.modificationValue.getFormatFromExample(0));
                //    }
                //}
                //if (ncp == null)
                //{
                //    if (doLog) debug = debug.log("Line parameter not found [" + pm.parameterTarget+"]");
                //    break;
                //}
            }
            if (doLog) debug = debug.log("After: " + line.ToString());

            return debug;
        }

        /// <summary>
        /// Creates collection witn ncParamModify items
        /// </summary>
        /// <param name="nParams">How many ncParamModify items to setup automatically</param>
        public ncParamModifyCollection(Int32 nParams)
        {
            for (Int32 a = 1; a <= nParams; a++)
            {
                ncParamModify ncPM = new ncParamModify();
                ncPM.parameterTarget = a.ToString();
                ncPM.modificationType = paramValueModificationType.increase;
                ncPM.doEnforceFormat = true;
                ncPM.doAddOnMissing = false;
                ncPM.modificationValue = "0.000";
                Add(ncPM);
            }
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Kreira event koji obaveštava da je promenjen neki parametar
        /// </summary>
        /// <remarks>
        /// Neće biti kreiran event ako nije spremna aplikacija: imbSettingsManager.current.isReady
        /// </remarks>
        /// <param name="name"></param>
        public void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}