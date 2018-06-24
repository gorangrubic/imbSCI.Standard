// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbAttribute.cs" company="imbVeles" >
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
namespace imbSCI.Core.attributes
{
    using imbSCI.Core.enums;
    using imbSCI.Core.extensions.enumworks;
    using imbSCI.Core.math.aggregation;
    using imbSCI.Core.math.measureSystem.core;
    using imbSCI.Core.math.range;
    using imbSCI.Core.style.color;
    using imbSCI.Data.enums.fields;

    #region imbVeles using

    using System;
    using System.Drawing;

    #endregion imbVeles using

    /// <summary>
    /// General imbSCI attribute, used for data annotation, report generation, documentation generation and UI auto-configuration
    /// </summary>
    [AttributeUsage(AttributeTargets.Class |
                    AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Property |
                    AttributeTargets.Enum | AttributeTargets.GenericParameter | AttributeTargets.All,
        AllowMultiple = true)]
    public class imbAttribute : Attribute
    {
        /// <summary>
        /// Drug parametar: prosleđena poruka
        /// Npr: ako je ime "link" onda se ovde može definisati naziv koji će se prikazivati
        /// Ako je * onda je default
        /// </summary>
        public string msg;

        /// <summary>
        /// Prvi parametar: ime oznake. Npr: Link znači da je property ustvari Linked Resource
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        public string name;

        public imbAttributeName nameEnum = imbAttributeName.undefined;

        public Object objMsg;

        public Object objExtra { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="imbAttribute"/> class.
        /// </summary>
        /// <param name="column_option">The column option.</param>
        /// <param name="setting_value">The setting value.</param>
        public imbAttribute(imbAttributeName _name, templateFieldDataTable column_option, Int32 setting_value)
        {
            nameEnum = imbAttributeName.DataTableExport;
            name = nameEnum.ToString();
            objExtra = setting_value;
            objMsg = column_option;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="imbAttribute"/> class.
        /// </summary>
        /// <param name="column_option">The column option.</param>
        /// <param name="setting_value">The setting value.</param>
        public imbAttribute(imbAttributeName _name, templateFieldDataTable column_option, String setting_value)
        {
            nameEnum = imbAttributeName.DataTableExport;
            name = nameEnum.ToString();
            objExtra = setting_value;
            objMsg = column_option;
        }

        ///// <summary>
        ///// Postavlja atribut u flag modu - samo ime
        ///// </summary>
        ///// <param name="_name"></param>
        //public imbAttribute(string _name)
        //{
        //    name = _name;
        //    msg = "No data";
        //}

        ///// <summary>
        ///// Postavlja atribut zajedno sa porukom
        ///// </summary>
        ///// <param name="_name">Ime podešavanja na koje se odnosi poruka</param>
        ///// <param name="_msg">Tekst poruke za podešavanje</param>
        //public imbAttribute(string _name, string _msg)
        //{
        //    name = _name;
        //    msg = _msg;
        //    nameEnum = (imbAttributeName) Enum.Parse(typeof(imbAttributeName), _name); // (imbAttributeName) imbEnumExtend.getEnumByName(typeof (imbAttributeName), _name);
        //}

        /// <summary>
        /// Konstruktor koji koristi Enumeraciju za ime podešavanja na koji se odnosi poruka
        /// </summary>
        /// <param name="_name">Na koje se podešavanje odnosi poruka</param>
        /// <param name="_msg">Vrednost podešavaja</param>
        public imbAttribute(imbAttributeName _name)
        {
            name = _name.ToString();
            nameEnum = _name;
        }

        /// <summary>
        /// Konstruktor koji koristi Enumeraciju za ime podešavanja na koji se odnosi poruka
        /// </summary>
        /// <param name="_name">Na koje se podešavanje odnosi poruka</param>
        /// <param name="_msg">Vrednost podešavaja</param>
        public imbAttribute(imbAttributeName _name, string _msg)
        {
            name = _name.ToString();
            nameEnum = _name;
            msg = _msg;
        }

        /// <summary>
        /// Defines one aspect of aggregation config
        /// </summary>
        /// <param name="aggregationAspect">The aggregation aspect to be declared.</param>
        /// <param name="types">The types to be set for the aspect</param>
        public imbAttribute(dataPointAggregationAspect aggregationAspect, dataPointAggregationType types)
        {
            nameEnum = imbAttributeName.reporting_aggregation;
            name = nameof(imbAttributeName.reporting_aggregation);
            objExtra = aggregationAspect;
            objMsg = types;
            msg = types.ToString();
        }

        public imbAttribute(imbAttributeName _name, Type _msg)
        {
            name = _name.ToString();
            nameEnum = _name;
            objMsg = _msg;
            msg = _msg.Name;
        }

        /// <summary>
        /// Defines calculation expression
        /// </summary>
        /// <param name="operandA">The operand a.</param>
        /// <param name="operaA">The opera a.</param>
        /// <param name="operandB">The operand b.</param>
        /// <param name="operaB">The opera b.</param>
        /// <param name="operandC">The operand c.</param>
        public imbAttribute(operation operaA, String operandA, operation operaAB, String operandB, operation operaBC = operation.none, String operandC = "")
        {
            imbAttributeName _name = imbAttributeName.measure_expression;
            name = _name.ToString();
            nameEnum = _name;

            measureOperandList oplist = new measureOperandList();
            if (operaA != operation.none) oplist.Add(0, operaA, operandA);
            if (operaAB != operation.none) oplist.Add(0, operaAB, operandB);
            if (operaBC != operation.none) oplist.Add(0, operaBC, operandC);
            objMsg = oplist;
        }

        public imbAttribute(imbAttributeName _name, Double _min, Double _max, Double _even, rangeCriteriaEnum _mode)
        {
            name = _name.ToString();
            nameEnum = _name;
            objMsg = new aceCriterionConfig(_min, _max, _even, _mode);
        }

        public imbAttribute(imbAttributeName _name, Double _min, Double _max, numberRangeModifyRule _mode)
        {
            name = _name.ToString();
            nameEnum = _name;
            objMsg = new aceRangeConfig(_min, _max, _mode);
        }

        //public imbAttribute(imbAttributeName _name, Enum _keyname, String letterOrSufix, String symbolOrName)
        //{
        //}

        public imbAttribute(imbAttributeName _name, Enum _keyname, String letterOrSufix, String symbolOrName, String formatA = "", String formatB = "")
        {
            name = _name.ToString();

            nameEnum = _name;

            Tuple<Enum, String, String, String, String> tup = new Tuple<Enum, String, String, String, String>(_keyname, letterOrSufix, symbolOrName, formatA, formatB);

            objMsg = tup;
        }

        /// <summary>
        /// Sets <c>_msg</c> color as hexadecimal value, to specified <c>imbAttributeName</c> attribue
        /// </summary>
        /// <param name="_name">Attribute value to be set</param>
        /// <param name="_msg">System color value to be set</param>
        public imbAttribute(imbAttributeName _name, Color _msg)
        {
            name = _name.ToString();
            nameEnum = _name;
            msg = ColorWorks.ColorToHex(_msg);
            objMsg = _msg;
        }

        /// <summary>
        /// Bilo sta za msg on ce to pretvoriti u String - zgodno kada treba navesti neki enum
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_msg"></param>
        public imbAttribute(imbAttributeName _name, Object _msg)
        {
            if (_msg == null)
            {
                msg = "";
            }
            else
            {
                Type msgType = _msg.GetType();

                if (msgType.IsEnum)
                {
                    msg = _msg.getEnumMemberPath(true);
                }
            }
            name = _name.ToString();
            nameEnum = _name;
            objMsg = _msg;
        }

        /// <summary>
        /// Konstruktor koji koristi Enumeraciju za ime podešavanja na koji se odnosi poruka
        /// </summary>
        /// <param name="_name">Na koje se podešavanje odnosi poruka</param>
        /// <param name="_msg">Vrednost podešavaja</param>
        public imbAttribute(imbAttributeName _name, Int32 _int)
        {
            nameEnum = _name;
            name = _name.ToString();
            msg = _int.ToString();
        }

        /// <summary>
        /// Bezbedan nacin da se dobije prava poruka - ako postoji vratice objMsg
        /// </summary>
        /// <returns></returns>
        public Object getMessage()
        {
            Object vl = msg;
            if (objMsg != null) vl = objMsg;
            return vl;
        }

        /*
        /// <summary>
        /// Konstruktor koji koristi Enumeraciju za ime podešavanja na koji se odnosi poruka
        /// </summary>
        /// <param name="_name">Na koje se podešavanje odnosi poruka</param>
        /// <param name="_msg">Vrednost podešavaja</param>
        public imbAttribute(imbKeyboardCommand _command)
        {
            name = _name.ToString();
            msg = _msg;
        }
        */
    }
}