// --------------------------------------------------------------------------------------------------------------------
// <copyright file="rangeCriteria.cs" company="imbVeles" >
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
namespace imbSCI.Core.math.range
{
    using imbSCI.Core.attributes;
    using imbSCI.Core.interfaces;
    using System;

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="imbSCI.Core.interfaces.IRangeCriteria" />
    public class rangeCriteria<T> : IRangeCriteria where T : IComparable
    {
        void IRangeCriteria.setCriteriaExact(IComparable even) => this.setCriteriaExact((T)even);

        void IRangeCriteria.setCriteria(IComparable even, bool trueIfMore) => this.setCriteria((T)even, trueIfMore);

        void IRangeCriteria.setCriteria(IComparable min, IComparable max, bool trueIfInside) => this.setCriteria((T)min, (T)max, trueIfInside);

        bool IRangeCriteria.testCriteria(IComparable testValue) => this.testCriteria((T)testValue);

        /// <summary>
        /// Initializes a new instance of the <see cref="rangeCriteria{T}"/> in disabled mode. It will return <c>false</c> on any test value before <see cref="setCriteria(T, bool)"/>  or <see cref="setCriteria(T, T, bool)"/> was called
        /// </summary>
        public rangeCriteria()
        {
            mode = rangeCriteriaEnum.none;
        }

        public void setCriteria(aceCriterionConfig config)
        {
            if (config == null)
            {
                isDisabled = true;
                return;
            }
            isDisabled = false;
            max = (T)config.max;
            min = (T)config.min;
            even = (T)config.even;
            mode = config.mode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="rangeCriteria{T}"/> class in MinMax range mode. It will return <c>true</c> if test value is between or equal to Min and Max values.
        /// </summary>
        /// <param name="min">The minimum value of range. The range test will return <c>true</c> against exactly the same value.</param>
        /// <param name="max">The maximum value of range. The range test will return <c>true</c> against exactly the same value.</param>
        /// <param name="trueIfInside">if set to <c>false</c> the range test is inversed.</param>
        public rangeCriteria(T min, T max, Boolean trueIfInside = true)
        {
            setCriteria(min, max, trueIfInside);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="rangeCriteria{T}"/> class in more or less mode. It will return <c>true</c> if test value is greater or equal than <c>even</c> (or lower in case <c>trueIfMore</c> was false)
        /// </summary>
        /// <param name="even">The even value to test against</param>
        /// <param name="trueIfMore">if set to <c>false</c> the test will return <c>true</c> for lower than <c>even</c> test value</param>
        public rangeCriteria(T even, Boolean trueIfMore = true)
        {
            setCriteria(even, trueIfMore);
        }

        public void setCriteriaExact(T even)
        {
            this.even = even;
            mode = rangeCriteriaEnum.exactEven;
        }

        /// <summary>
        /// Sets the criteria.
        /// </summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <param name="trueIfInside">if set to <c>true</c> [true if inside].</param>
        public void setCriteria(T min, T max, Boolean trueIfInside = true)
        {
            if (min.CompareTo(max) > -1)
            {
                this.min = max;
                this.max = min;
            }
            else
            {
                this.min = min;
                this.max = max;
            }
            if (trueIfInside)
            {
                mode = rangeCriteriaEnum.insideMinMax;
            }
            else
            {
                mode = rangeCriteriaEnum.outsideMinMax;
            }
        }

        /// <summary>
        /// Sets the criteria.
        /// </summary>
        /// <param name="even">The even.</param>
        /// <param name="trueIfMore">if set to <c>true</c> [true if more].</param>
        public void setCriteria(T even, Boolean trueIfMore = true)
        {
            this.even = even;

            if (trueIfMore)
            {
                mode = rangeCriteriaEnum.moreThenEven;
            }
            else
            {
                mode = rangeCriteriaEnum.lessThenEven;
            }
        }

        private rangeCriteriaEnum _mode = rangeCriteriaEnum.insideMinMax;

        /// <summary>
        ///
        /// </summary>
        protected rangeCriteriaEnum mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        //public rangeCriteria()

        private T _min;

        /// <summary>
        ///
        /// </summary>
        public T min
        {
            get { return _min; }
            set { _min = value; }
        }

        private T _even;

        /// <summary>
        ///
        /// </summary>
        public T even
        {
            get { return _even; }
            set { _even = value; }
        }

        private T _max;

        /// <summary>
        ///
        /// </summary>
        public T max
        {
            get { return _max; }
            set { _max = value; }
        }

        protected Boolean insideRange(T testValue)
        {
            return ((testValue.CompareTo(max) < 1) && (testValue.CompareTo(min) > -1));
        }

        protected Boolean isBelowMin(T testValue)
        {
            return (testValue.CompareTo(min) == -1);
        }

        protected Boolean isOverOrOnMax(T testValue)
        {
            return (testValue.CompareTo(max) > -1);
        }

        private Boolean _isDisabled = false;

        /// <summary>
        /// if <c>True</c> test result is always <c>False</c>
        /// </summary>
        public Boolean isDisabled
        {
            get { return _isDisabled; }
            set { _isDisabled = value; }
        }

        bool IRangeCriteria.isDisabled
        {
            get
            {
                return isDisabled;
            }
            set
            {
                isDisabled = value;
            }
        }

        /// <summary>
        /// Tests the value against criteria.
        /// </summary>
        /// <param name="testValue">The test value.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public Boolean testCriteria(T testValue)
        {
            if (isDisabled) return true;

            switch (mode)
            {
                case rangeCriteriaEnum.none:
                    return false;
                    break;

                case rangeCriteriaEnum.lessThenEven:
                    return (testValue.CompareTo(even) == -1);
                    break;

                case rangeCriteriaEnum.moreThenEven:
                    return (testValue.CompareTo(even) > -1);
                    break;

                case rangeCriteriaEnum.insideMinMax:
                    return insideRange(testValue);

                    break;

                case rangeCriteriaEnum.outsideMinMax:
                    return !insideRange(testValue);
                    break;

                case rangeCriteriaEnum.exactEven:
                    return (testValue.CompareTo(even) == 0);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        //public numberRangeCriteria()
    }
}