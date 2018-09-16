// --------------------------------------------------------------------------------------------------------------------
// <copyright file="measure.cs" company="imbVeles" >
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
namespace imbSCI.Core.math.measureSystem.core
{
    using imbSCI.Core.enums;
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Core.math.measureSystem.enums;
    using imbSCI.Core.math.range;
    using imbSCI.Data;
    using imbSCI.Data.interfaces;
    using interfaces;

    using interfaces;
    using interfaces;
    using interfaces;
    using interfaces;
    using interfaces;

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <seealso cref="aceCommonTypes.interfaces.IAceMathMeasure" />
    /// <seealso cref="IObjectWithNameAndDescription" />
    /// <seealso cref="aceCommonTypes.interfaces.IValueWithFormat" />
    /// <seealso cref="aceCommonTypes.interfaces.IValueWithImportanceInfo" />
    /// <seealso cref="aceCommonTypes.interfaces.IValueWithRoleInfo" />
    /// <seealso cref="aceCommonTypes.interfaces.IValueWithToString" />
    /// <seealso cref="aceCommonTypes.interfaces.IValueWithUnitInfo" />
    public abstract class measure<TValue> : measureBase, IAceMathMeasure, IMeasureBasic where TValue : IComparable
    {
        private measureOperandList _calculateTasks = new measureOperandList();

        /// <summary>
        ///
        /// </summary>
        public measureOperandList calculateTasks
        {
            get { return _calculateTasks; }
            protected set { _calculateTasks = value; }
        }

        protected measure()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="measure{TValue}"/> class.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="system">The system.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="defaultBaseValue">The default base value.</param>
        /// <param name="levelsFromRoot">The levels from root.</param>
        protected measure(Enum role, measureSystemsEnum system, TValue defaultValue, TValue defaultBaseValue, Int32 levelsFromRoot = 0)
        {
            valueTypeGroup = valueType.getTypeGroup();

            info = measureSystemManager.getMeasureInfo(role, levelsFromRoot, system);
            setDefaultValue(defaultValue, defaultBaseValue);
        }

        public measureSystemRoleEntry getRoleEntry(Object input)
        {
            Tuple<Enum, String, String, String, String> tup = input as Tuple<Enum, String, String, String, String>;
            return info.system.GetOrMakeRole(tup, info.role);
        }

        public measureSystemUnitEntry getUnitEntry(Object input)
        {
            Tuple<Enum, String, String, String, String> tup = input as Tuple<Enum, String, String, String, String>;
            return info.system.GetOrMakeUnit(tup, info.unit);
        }

        public virtual TC getValue<TC>()
        {
            return primValue.imbConvertValueSafeTyped<TC>();

            //if (primValue is Double)
            //{
            //} else if (primValue is Decimal)
            //{
            //}
            //else if (primValue is Int32)
            //{
            //}
            //else if (primValue is Boolean)
            //{
            //}
            //else if (primValue.GetType().IsEnum)
            //{
            //} else if (primValue is String)
            //{
            //} else
            //{
            //    //
            //}
        }

        // public abstract IMeasureBasic calculate(IMeasureBasic second, operation op);

        public measureSystemUnitEntry setCustomUnitEntry()
        {
            if (info == null) info = new measureInfo();
            if (info.unit == null) info.unit = new measureSystemUnitEntry();

            return info.unit;
        }

        public measureSystemRoleEntry setCustomRoleEntry(String __letter = "", String __symbol = "", String __name = "")
        {
            if (info == null) info = new measureInfo();
            if (info.role == null) info.role = new measureSystemRoleEntry(__letter, __symbol, __name);

            return info.role;
        }

        //public virtual Decimal getValueAsDecimal()
        //{
        //    return primValue.imbToNumber<Decimal>();

        //}

        public void setAlarmExact(TValue _alarmValue, Boolean _alarmOn)
        {
            isAlarmTurnedOn = _alarmOn;
            alarmCriteria.setCriteriaExact(_alarmValue);
        }

        #region ----------- Boolean [ isAlarmTurnedOn ] -------  [If true it will use alarm range to fire alarm]

        private Boolean _isAlarmTurnedOn = false;

        /// <summary>
        /// If true it will use alarm range to fire alarm
        /// </summary>
        [Category("Switches")]
        [DisplayName("isAlarmTurnedOn")]
        [Description("If true it will use alarm range to fire alarm")]
        public Boolean isAlarmTurnedOn
        {
            get { return _isAlarmTurnedOn; }
            set { _isAlarmTurnedOn = value; }
        }

        #endregion ----------- Boolean [ isAlarmTurnedOn ] -------  [If true it will use alarm range to fire alarm]

        private rangeCriteria<TValue> _alarmCriteria = new rangeCriteria<TValue>();

        /// <summary>
        ///
        /// </summary>
        public rangeCriteria<TValue> alarmCriteria
        {
            get { return _alarmCriteria; }
            protected set { _alarmCriteria = value; }
        }

        public virtual bool isValueInAlarmRange
        {
            get
            {
                if (!isAlarmTurnedOn) return false;
                return alarmCriteria.testCriteria(primValue);
            }
        }

        private Boolean _isUnitOptEnabled = true;

        /// <summary>
        ///
        /// </summary>
        public Boolean doUnitOptimization
        {
            get { return _isUnitOptEnabled; }
            set { _isUnitOptEnabled = value; }
        }

        /// <summary>
        /// Gets a value indicating whether [do unit optimization].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [do unit optimization]; otherwise, <c>false</c>.
        /// </value>
        public virtual Boolean getUnitOptimization()
        {
            switch (valueTypeGroup)
            {
                default:
                case imbTypeGroup.boolean:
                case imbTypeGroup.enumeration:

                case imbTypeGroup.text:
                case imbTypeGroup.unknown:
                    return false;
                    break;

                case imbTypeGroup.instance:
                case imbTypeGroup.number:
                    return true;
                    break;
            }
            return false;
        }

        /// <summary>
        /// Converts to optimal unit level.
        /// </summary>
        public void convertToOptimalUnitLevel()
        {
            if (!doUnitOptimization) return;
            measureSystemUnitEntry optimalUnit = info.system.GetOptimalUnit(info.unit, GetFormatedValue().Length);
            if (optimalUnit != info.unit) convertToUnit(optimalUnit);
        }

        public abstract void convertToUnit(measureSystemUnitEntry targetUnit);

        //{
        //    double fd = info.unit.GetFactorDistance(targetUnit);
        //    primValue = primValue * fd;
        //}

        public void setDefaultValue(TValue defaultValue, TValue defaultBaseValue)
        {
            primValue = defaultValue;
            baseValue = defaultBaseValue;
            defValue = defaultValue;
            defaultValueAndUnit = GetFormatedValueAndUnit();
        }

        /// <summary>
        /// Returns a string that represents the meassure - according to
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public String ToString()
        {
            String form = GetFormatForValue(true);
            //String useForValueAndUnit = GetFormatForValue(true);
            var args = GetContent(info.unit.contentForValueAndUnitOutput);
            return String.Format(form, args.ToArray());
        }

        /// <summary>
        /// Checks the unit sufix.
        /// </summary>
        /// <returns></returns>
        protected String checkUnitSufix()
        {
            if (info.role != null)
            {
                return info.role.checkRoleUnitOverride(info.unit.unit);
            }
            return info.unit.unit;
        }

        /// <summary>
        /// Gets the informational content about this measure
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public List<Object> GetContent(measureToStringContent content)
        {
            List<Object> output = new List<Object>();
            if (content.HasFlag(measureToStringContent.value)) output.Add(info.unit.checkValueMap(primValue));
            if (content.HasFlag(measureToStringContent.unit)) output.Add(unit_sufix);
            if (content.HasFlag(measureToStringContent.name)) output.Add(info.unit.nameSingular);

            if (content.HasFlag(measureToStringContent.roleName)) output.Add(role_name);
            if (content.HasFlag(measureToStringContent.roleSymbol)) output.Add(role_symbol);
            if (content.HasFlag(measureToStringContent.roleLetter)) output.Add(role_letter);

            if (content.HasFlag(measureToStringContent.baseValue))
            {
                output.Add(info.unit.checkValueMap(baseValue));
            }
            return output;
        }

        /// <summary>
        /// To the string.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public String ToString(measureToStringContent content)
        {
            String output = "";

            String useForValue = GetFormatForValue();
            String useForValueAndUnit = GetFormatForValue(true);
            //String vl = "";
            //if (info.unit.isUsingBaseValue)
            //{
            //    vl = String.Format(info.unit.formatForValue, primValue);
            //}

            if (content.HasFlag(measureToStringContent.valueAndUnit))
            {
                return ToString();
            }

            if (content.HasFlag(measureToStringContent.roleName)) output = imbSciStringExtensions.add(output, role_name, " ");
            if (content.HasFlag(measureToStringContent.roleLetter)) output = imbSciStringExtensions.add(output, role_letter, " ");

            if (content.HasFlag(measureToStringContent.equalSign)) output = imbSciStringExtensions.add(output, "=", " ");
            if (content.HasFlag(measureToStringContent.isWord)) output = imbSciStringExtensions.add(output, "is", " ");

            if (content.HasFlag(measureToStringContent.value)) output = imbSciStringExtensions.add(output, String.Format(useForValue, info.unit.checkValueMap(primValue)), " ");

            if (content.HasFlag(measureToStringContent.unit)) output = imbSciStringExtensions.add(output, checkUnitSufix(), " ");

            if (content.HasFlag(measureToStringContent.baseValue))
            {
                output = imbSciStringExtensions.add(output, String.Format(info.unit.formatForValue, info.unit.checkValueMap(baseValue)), " ");
                output = imbSciStringExtensions.add(output, info.unit.unit, " ");
            }

            return output;
        }

        /// <summary>
        /// Gets the format for value or value-and-unit if forValueAndUnit is TRUE.
        /// </summary>
        /// <param name="forValueAndUnit">if set to <c>true</c> returns format for value and unit.</param>
        /// <returns>Format string</returns>
        public String GetFormatForValue(Boolean forValueAndUnit = false)
        {
            if (forValueAndUnit)
            {
                if (imbSciStringExtensions.isNullOrEmptyString(info.role.valueAndUnitFormatOverride))
                {
                    return info.unit.formatForValueAndUnit;
                }
                else
                {
                    return info.role.valueAndUnitFormatOverride;
                }
            }
            else
            {
                if (imbSciStringExtensions.isNullOrEmptyString(info.role.valueFormatOverride))
                {
                    return info.unit.formatForValue;
                }
                else
                {
                    return info.role.valueFormatOverride;
                }
            }
        }

        public string GetFormatedValue()
        {
            return ToString(measureToStringContent.value);
        }

        public string GetFormatedValueAndUnit()
        {
            return ToString(measureToStringContent.valueAndUnit);
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="obj" /> in the sort order. Zero This instance occurs in the same position in the sort order as <paramref name="obj" />. Greater than zero This instance follows <paramref name="obj" /> in the sort order.
        /// </returns>
        public int CompareTo(object obj)
        {
            TValue b2 = default(TValue);

            if (obj is measure<TValue>)
            {
                measure<TValue> obj_measure = (measure<TValue>)obj;
                b2 = obj_measure.primValue;
            }
            else if (obj is TValue)
            {
                TValue obj_TValue = (TValue)obj;
                b2 = obj_TValue;
            }
            else
            {
                return -1;
            }

            return primValue.CompareTo(b2);
        }

        public abstract IMeasureBasic calculate(IMeasure second, operation op);

        private measureInfo _info;

        /// <summary>
        ///
        /// </summary>
        public measureInfo info
        {
            get { return _info; }
            set { _info = value; }
        }

        //  public abstract object Clone();

        //  internal measureInfo info;

        private String _defaultValueAndUnit;

        /// <summary>
        /// string representation of default value with Unit
        /// </summary>
        public String defaultValueAndUnit
        {
            get { return _defaultValueAndUnit; }
            protected set { _defaultValueAndUnit = value; }
        }

        /// <summary>
        /// Type of <c>TValue</c>
        /// </summary>
        protected Type valueType
        {
            get { return typeof(TValue); }
        }

        private imbTypeGroup _valueTypeGroup;

        /// <summary>
        ///
        /// </summary>
        public imbTypeGroup valueTypeGroup
        {
            get { return _valueTypeGroup; }
            protected set { _valueTypeGroup = value; }
        }

        private TValue _defValue;

        /// <summary>
        ///
        /// </summary>
        public TValue defValue
        {
            get { return _defValue; }
            set { _defValue = value; }
        }

        private rangeValueBase<TValue> _valueRange;

        /// <summary>
        ///
        /// </summary>
        public rangeValueBase<TValue> valueRange
        {
            get { return _valueRange; }
            set { _valueRange = value; }
        }

        private TValue _primValue = default(TValue);

        /// <summary>
        ///
        /// </summary>
        public TValue primValue
        {
            get { return _primValue; }
            set
            {
                if (valueRange != null)
                {
                    _primValue = valueRange.checkValue(value);
                }
                else
                {
                    _primValue = value;
                }
            }
        }

        private TValue _baseValue = default(TValue);

        /// <summary>
        ///
        /// </summary>
        public TValue baseValue
        {
            get { return _baseValue; }
            set { _baseValue = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the current value is plural.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is value plural; otherwise, <c>false</c>.
        /// </value>
        public virtual Boolean isValuePlural
        {
            get
            {
                return true;
            }
        }

        public string formatForValue => info.unit.formatForValue;

        public string formatForValueAndUnit => info.unit.formatForValueAndUnit;

        //  private Boolean _isValueInAlarmRange = false;
        /// <summary>
        /// TRUE if current value is in alarmant value range
        /// </summary>
        //public virtual Boolean isValueInAlarmRange {
        //    get
        //    {
        //        return alarmCriteria.testCriteria(primValue);
        //    }
        //}

        /// <summary>
        /// TRUE if current value is same as default value specified with constructor
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is value default; otherwise, <c>false</c>.
        /// </value>
        public virtual Boolean isValueDefault
        {
            get
            {
                var tmp = GetFormatedValueAndUnit();
                return (defaultValueAndUnit == tmp);
            }
        }

        private dataPointImportance _relevance = dataPointImportance.normal;

        /// <summary>
        ///
        /// </summary>
        public virtual dataPointImportance relevance
        {
            get
            {
                if (isValueDefault) return dataPointImportance.none;
                if (isValueInAlarmRange) return dataPointImportance.alarm;

                return _relevance;
            }
            set { _relevance = value; }
        }

        /// <summary>
        /// Gets the name of the role.
        /// </summary>
        /// <value>
        /// The name of the role.
        /// </value>
        public string role_name
        {
            get { return info.role.name; }
            set { info.role.name = value; }
        }

        /// <summary>
        /// Gets the role symbol.
        /// </summary>
        /// <value>
        /// The role symbol.
        /// </value>
        public string role_symbol
        {
            get { return info.role.symbol; }
            set { info.role.symbol = value; }
        }

        /// <summary>
        /// Gets the role letter.
        /// </summary>
        /// <value>
        /// The role letter.
        /// </value>
        public string role_letter
        {
            get { return info.role.letter; }
            set { info.role.letter = value; }
        }

        public string unit_sufix => checkUnitSufix();

        public string unit_name
        {
            get
            {
                if (isValuePlural)
                {
                    return info.unit.namePlural;
                }
                else
                {
                    return info.unit.nameSingular;
                }
            }
        }

        //if (Convert.ToInt32(primValue) == 1)

        public static implicit operator TValue(measure<TValue> a1)
        {
            return a1.primValue;
        }

        public static bool operator <(measure<TValue> a1, measure<TValue> a2)
        {
            return (a1.primValue.CompareTo(a2.primValue) == -1);
        }

        public static bool operator >(measure<TValue> a1, measure<TValue> a2)
        {
            return (a1.primValue.CompareTo(a2.primValue) == 1);
        }

        public static bool operator <=(measure<TValue> a1, measure<TValue> a2)
        {
            return (a1.primValue.CompareTo(a2.primValue) < 1);
        }

        public static bool operator >=(measure<TValue> a1, measure<TValue> a2)
        {
            return (a1.primValue.CompareTo(a2.primValue) > -1);
        }

        public static bool operator ==(measure<TValue> a1, measure<TValue> a2)
        {
            return (a1.primValue.CompareTo(a2.primValue) == 0);
        }

        public static bool operator !=(measure<TValue> a1, measure<TValue> a2)
        {
            return (a1.primValue.CompareTo(a2.primValue) != 0);
        }

        public static bool operator <(measure<TValue> a1, TValue a2)
        {
            return (a1.primValue.CompareTo(a2) == -1);
        }

        public static bool operator >(measure<TValue> a1, TValue a2)
        {
            return (a1.primValue.CompareTo(a2) == 1);
        }

        public static bool operator <=(measure<TValue> a1, TValue a2)
        {
            return (a1.primValue.CompareTo(a2) < 1);
        }

        public static bool operator >=(measure<TValue> a1, TValue a2)
        {
            return (a1.primValue.CompareTo(a2) > -1);
        }

        public static bool operator ==(measure<TValue> a1, TValue a2)
        {
            return (a1.primValue.CompareTo(a2) == 0);
        }

        public static bool operator !=(measure<TValue> a1, TValue a2)
        {
            return (a1.primValue.CompareTo(a2) != 0);
        }

        public double unit_factor => info.unit.factor;

        public measureUnitType unit_type => info.system.measureType;

        public bool unit_isUsingBaseValue => info.unit.isUsingBaseValue;

        IRangeValue IMeasureBasic.valueRange => valueRange as IRangeValue;

        IRangeCriteria IMeasureBasic.alarmCriteria => alarmCriteria as IRangeCriteria;
    }
}