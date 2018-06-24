// --------------------------------------------------------------------------------------------------------------------
// <copyright file="testLabelingSettings.cs" company="imbVeles" >
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
// Project: imbSCI.DataComplex
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
using imbSCI.Data.data;
using System;

namespace imbSCI.DataComplex.tests
{
    #region imbVELES USING

    using System.ComponentModel;

    #endregion imbVELES USING

    /// <summary>
    /// 2013c: LowLevel resurs
    /// </summary>
    public class testLabelingSettings : imbBindable
    {
        private Int32 _sampleBlockOrdinalNumber = 1; //default(Int32); // = new Int32();

        /// <summary>
        /// Sample block that is subject of test run
        /// </summary>
        [Category("testLabelingSettings")]
        [DisplayName("sampleBlockOrdinalNumber")]
        [Description("Sample block that is subject of test run")]
        public Int32 sampleBlockOrdinalNumber
        {
            get
            {
                return _sampleBlockOrdinalNumber;
            }
            set
            {
                _sampleBlockOrdinalNumber = value;
                OnPropertyChanged("sampleBlockOrdinalNumber");
            }
        }

        #region -----------  separator  -------  [Separator koji se stavlja izmedju elemenata RunStampa]

        private String _separator = "_"; // = new String();

        /// <summary>
        /// Separator koji se stavlja izmedju elemenata RunStampa
        /// </summary>
        // [XmlIgnore]
        [Category("testLabelingSettings")]
        [DisplayName("separator")]
        [Description("Separator koji se stavlja izmedju elemenata RunStampa")]
        public String separator
        {
            get { return _separator; }
            set
            {
                // Boolean chg = (_separator != value);
                _separator = value;
                OnPropertyChanged("separator");
                // if (chg) {}
            }
        }

        #endregion -----------  separator  -------  [Separator koji se stavlja izmedju elemenata RunStampa]

        #region ----------- Boolean [ doAbrevateTitle ] -------  [Da li da skracuje naslov testa]

        private Boolean _doAbbrevateTitle = true;

        /// <summary>
        /// Da li da skracuje naslov testa
        /// </summary>
        [Category("Switches")]
        [DisplayName("doAbbrevateTitle")]
        [Description("Da li da skracuje naslov testa")]
        public Boolean doAbbrevateTitle
        {
            get { return _doAbbrevateTitle; }
            set
            {
                _doAbbrevateTitle = value;
                OnPropertyChanged("doAbbrevateTitle");
            }
        }

        #endregion ----------- Boolean [ doAbrevateTitle ] -------  [Da li da skracuje naslov testa]

        #region -----------  titleAbbrevationLength  -------  [Broj karaktera - do koje duzine treba da bude skracen naziv testa]

        private Int32 _titleAbbrevationLength = 4; // = new Int32();

        /// <summary>
        /// Broj karaktera - do koje duzine treba da bude skracen naziv testa
        /// </summary>
        // [XmlIgnore]
        [Category("Test Labeling")]
        [DisplayName("titleAbbrevationLength")]
        [Description("Broj karaktera - do koje duzine treba da bude skracen naziv testa")]
        public Int32 titleAbbrevationLength
        {
            get { return _titleAbbrevationLength; }
            set
            {
                // Boolean chg = (_titleAbbrevationLength != value);
                _titleAbbrevationLength = value;
                OnPropertyChanged("titleAbbrevationLength");
                // if (chg) {}
            }
        }

        #endregion -----------  titleAbbrevationLength  -------  [Broj karaktera - do koje duzine treba da bude skracen naziv testa]

        #region -----------  testCountDigitCount  -------  [Koliko cifara da odvoji za ispis: default je 3, sto znaci> 005 za peti test po redu]

        private Int32 _testCountDigitCount = 3; // = new Int32();

        /// <summary>
        /// Koliko cifara da odvoji za ispis: default je 3, sto znaci> 005 za peti test po redu
        /// </summary>
        // [XmlIgnore]
        [Category("Test Labeling")]
        [DisplayName("testCountDigitCount")]
        [Description("Koliko cifara da odvoji za ispis: default je 3, sto znaci> 005 za peti test po redu")]
        public Int32 testCountDigitCount
        {
            get { return _testCountDigitCount; }
            set
            {
                // Boolean chg = (_testCountDigitCount != value);
                _testCountDigitCount = value;
                OnPropertyChanged("testCountDigitCount");
                // if (chg) {}
            }
        }

        #endregion -----------  testCountDigitCount  -------  [Koliko cifara da odvoji za ispis: default je 3, sto znaci> 005 za peti test po redu]

        #region -----------  insertYear  -------  [Da li na kraju potpisa testa da ubaci godinu]

        private Boolean _insertYear = true; // = new Boolean();

        /// <summary>
        /// Da li na kraju potpisa testa da ubaci godinu
        /// </summary>
        // [XmlIgnore]
        [Category("Test Labeling")]
        [DisplayName("insertYear")]
        [Description("Da li na kraju potpisa testa da ubaci godinu")]
        public Boolean insertYear
        {
            get { return _insertYear; }
            set
            {
                // Boolean chg = (_insertYear != value);
                _insertYear = value;
                OnPropertyChanged("insertYear");
                // if (chg) {}
            }
        }

        #endregion -----------  insertYear  -------  [Da li na kraju potpisa testa da ubaci godinu]

        #region -----------  insertMonthTreeLetters  -------  [Da li da ubacuje prva tri slova od naziva meseca]

        private Boolean _insertMonthTreeLetters = true; // = new String();

        /// <summary>
        /// Da li da ubacuje prva tri slova od naziva meseca
        /// </summary>
        // [XmlIgnore]
        [Category("Test Labeling")]
        [DisplayName("insertMonthTreeLetters")]
        [Description("Da li da ubacuje prva tri slova od naziva meseca")]
        public Boolean insertMonthTreeLetters
        {
            get { return _insertMonthTreeLetters; }
            set
            {
                // Boolean chg = (_insertMonthTreeLetters != value);
                _insertMonthTreeLetters = value;
                OnPropertyChanged("insertMonthTreeLetters");
                // if (chg) {}
            }
        }

        #endregion -----------  insertMonthTreeLetters  -------  [Da li da ubacuje prva tri slova od naziva meseca]

        #region -----------  insertDayInMonth  -------  [Da li da ubacuje dan u mesecu u potpis testa?]

        private Boolean _insertDayInMonth = false; // = new String();

        /// <summary>
        /// Da li da ubacuje dan u mesecu u potpis testa?
        /// </summary>
        // [XmlIgnore]
        [Category("Test Labeling")]
        [DisplayName("insertDayInMonth")]
        [Description("Da li da ubacuje dan u mesecu u potpis testa?")]
        public Boolean insertDayInMonth
        {
            get { return _insertDayInMonth; }
            set
            {
                // Boolean chg = (_insertDayInMonth != value);
                _insertDayInMonth = value;
                OnPropertyChanged("insertDayInMonth");
                // if (chg) {}
            }
        }

        #endregion -----------  insertDayInMonth  -------  [Da li da ubacuje dan u mesecu u potpis testa?]
    }
}