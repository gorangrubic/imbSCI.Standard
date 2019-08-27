// --------------------------------------------------------------------------------------------------------------------
// <copyright file="numberRange.cs" company="imbVeles" >
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

    using imbSCI.Core.enums;
    using imbSCI.Data;
    using System;


    /// <summary>
    /// Scaling scalar numbers
    /// </summary>
    public static class numberRange
    {
        /// <summary>
        /// Održava vrednost u zadatom rasponu
        /// </summary>
        /// <param name="old">Postojeća vrednost</param>
        /// <param name="delta">Razlika kojom pravimo korekciju</param>
        /// <param name="max">Maksimalna vrednost</param>
        /// <param name="rule">Pravilo kojim menjamo vrednost</param>
        /// <param name="op">Operator koji se primenjuje</param>
        /// <returns>
        /// Broj koji je sigurno u rasponu
        /// </returns>
        /// <seealso cref="aceCommonTypes.extensions.imbValueChangers.checkRange(int,int,int,bool)"/>
        public static float rangeChange(this float old, float delta, float max, numberRangeModifyRule rule, String op = "+")
        {
            Double tmpOut = Convert.ToDouble(old);
            Double tmpMax = Convert.ToDouble(max);
            Double tmpDelta = Convert.ToDouble(delta);

            return (float)rangeChange(tmpOut, tmpDelta, tmpMax, rule, op);
        }

        public static Decimal rangeChange(this Decimal old, Decimal delta, Decimal max, numberRangeModifyRule rule, String op = "+")
        {
            Double tmpOut = Convert.ToDouble(old);
            Double tmpMax = Convert.ToDouble(max);
            Double tmpDelta = Convert.ToDouble(delta);

            return (Decimal)rangeChange(tmpOut, tmpDelta, tmpMax, rule, op);
        }

        public static Int32 compute(this operation op, Int32 a, Int32 b)
        {
            return calculate(a, op, b);
        }

        public static Double compute(this operation op, Double a, Double b)
        {
            return calculate(a, op, b);
        }


        /// <summary>
        /// Calculates the specified oper.
        /// </summary>
        /// <param name="ad">The ad.</param>
        /// <param name="oper">The oper.</param>
        /// <param name="bd">The bd.</param>
        /// <returns></returns>
        public static Decimal calculate(this Decimal ad, operation oper, Decimal bd = 1)
        {
            Double a = Convert.ToDouble(ad);
            Double b = Convert.ToDouble(bd);
            return Convert.ToDecimal(calculate(a, oper, b));
        }

        /// <summary>
        /// Calculates the specified operation.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="operation">The operation.</param>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        public static Double calculate(this Double a, operation operation, Double b = 1)
        {
            switch (operation)
            {
                case operation.avg:
                    return (a + b) / 2;
                    break;

                case operation.cos:
                    return a * Math.Cos(angle.ToRadians(b));
                    break;

                case operation.diagonal:
                    return Math.Sqrt(a * a + b * b);
                    break;

                case operation.division:
                    return a / b;
                    break;

                case operation.divisionNatural:
                    return Math.Floor(a / b);
                    break;

                case operation.max:
                    return Math.Max(a, b);
                    break;

                case operation.min:
                    return Math.Min(a, b);
                    break;

                case operation.minus:
                    return a - b;
                    break;

                case operation.modulo:
                    return a % b;
                    break;

                case operation.multiplication:
                    return a * b;
                    break;

                case operation.none:
                    return a;
                    break;

                case operation.plus:
                    return a + b;
                    break;

                case operation.assign:
                    return b;
                    break;

                case operation.power:
                    return Math.Pow(a, b);
                    break;

                case operation.quantize:
                    return Math.Floor(a / b) * b;
                    break;

                case operation.round:
                    return Math.Round(a, Convert.ToInt32(b));
                    break;

                case operation.sin:
                    return a * Math.Sin(b);
                    break;

                case operation.sqroot:
                    return Math.Sqrt(a);
                    break;

                case operation.square:
                    return a * a;
                    break;

                case operation.abs:
                    return Math.Abs(a);
                    break;

                case operation.exp:
                    return Math.Exp(a);
                    break;

                case operation.floor:
                    return Math.Floor(a);
                    break;

                case operation.ceil:
                    return Math.Ceiling(a);
                    break;

                case operation.pi:
                    return Math.PI * a;
                    break;

                case operation.log:
                    return Math.Log(a);
                    break;

                case operation.isSame:
                    if (a.CompareTo(b) == 0) return 1;
                    return 0;
                    break;

                case operation.what4th:
                    return Math.Ceiling((a / b) / 0.25);
                    break;

                case operation.what10th:
                    return Math.Ceiling((a / b) / 0.10);
                    break;

                case operation.whatHalf:
                    return Math.Ceiling((a / b) / 0.50);
                    break;

                case operation.what5th:
                    return Math.Ceiling((a / b) / 0.20);
                    break;

                case operation.what3rd:
                    return Math.Ceiling((a / b) / 0.33);
                    break;

                case operation.compare:
                    //if ( == 0) return 1;
                    return a.CompareTo(b);
                    break;

                case operation.log10:
                    return a * a;
                    break;

                default:
                    return a;
                    break;
            }
        }

        /// <summary>
        /// Calculates the specified operation.
        /// </summary>
        /// <param name="a">if set to <c>true</c> [a].</param>
        /// <param name="operation">The operation.</param>
        /// <param name="b">if set to <c>true</c> [b].</param>
        /// <returns></returns>
        public static Boolean calculate(this Boolean a, operation operation, Boolean b)
        {
            Boolean c = false;
            switch (operation)
            {
                case operation.plus:
                    return a && b;
                    break;

                case operation.minus:
                    return a && !b;
                    break;

                case operation.multiplication:
                    return a || b;
                    break;

                case operation.division:
                    return a || !b;
                    break;

                case operation.assign:
                    return a;
                    break;

                default:
                    break;
            }
            return c;
        }

        /// <summary>
        /// Calculates the specified operation.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="operation">The operation.</param>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        public static String calculate(this String a, operation operation, String b = "")
        {
            String c = "";
            switch (operation)
            {
                case operation.assign:
                    return a;
                    break;

                case operation.max:
                    if (a.Length > b.Length)
                    {
                        return a;
                    }
                    else if (b.Length > a.Length)
                    {
                        return b;
                    }
                    return a;
                    break;

                case operation.min:
                    if (a.Length < b.Length)
                    {
                        return a;
                    }
                    else if (b.Length < a.Length)
                    {
                        return b;
                    }
                    return a;
                    break;

                case operation.compare:
                    return a.CompareTo(b).ToString();
                    break;

                case operation.plus:
                    return imbSciStringExtensions.add(a, b);
                    break;

                case operation.minus:
                    return a.Replace(b, "");
                    break;

                case operation.division:
                case operation.multiplication:

                    break;

                default:
                    return a;
                    break;
            }
            return c;
        }

        public static Int32 calculate(this Int32 a, operation operation, Int32 b = 0)
        {
            Int32 c = 0;
            switch (operation)
            {
                case operation.avg:
                    return (a + b) / 2;
                    break;

                case operation.cos:
                    return Convert.ToInt32(a * Math.Cos(angle.ToRadians(b)));
                    break;

                case operation.diagonal:
                    return Convert.ToInt32(Math.Sqrt(a * a + b * b));
                    break;

                case operation.division:
                    return a / b;
                    break;

                case operation.divisionNatural:

                    Math.DivRem(a, b, out c);
                    return c;
                    break;

                case operation.max:
                    return Math.Max(a, b);
                    break;

                case operation.min:
                    return Math.Min(a, b);
                    break;

                case operation.minus:
                    return a - b;
                    break;

                case operation.modulo:
                    return a % b;
                    break;

                case operation.multiplication:
                    return a * b;
                    break;

                case operation.none:
                    return a;
                    break;

                case operation.plus:
                    return a + b;
                    break;

                case operation.assign:
                    return b;
                    break;

                case operation.power:
                    return Convert.ToInt32(Math.Pow(a, b));
                    break;

                case operation.quantize:
                    Math.DivRem(a, b, out c);
                    return Convert.ToInt32(c * b);
                    break;

                case operation.round:
                    return a;
                    break;

                case operation.sin:
                    return Convert.ToInt32(a * Math.Sin(b));
                    break;

                case operation.sqroot:
                    return Convert.ToInt32(Math.Sqrt(a));
                    break;

                case operation.square:
                    return a * a;
                    break;

                case operation.abs:
                    return Math.Abs(a);
                    break;

                case operation.exp:
                    return Convert.ToInt32(Math.Exp(a));
                    break;

                case operation.floor:
                    return a;
                    break;

                case operation.ceil:
                    return a;
                    break;

                case operation.pi:
                    return Convert.ToInt32(Math.PI * a);
                    break;

                case operation.log:
                    return Convert.ToInt32(Math.Log(a));
                    break;

                case operation.isSame:
                    if (a.CompareTo(b) == 0) return 1;
                    return 0;
                    break;

                case operation.what4th:
                    return Convert.ToInt32(Math.Ceiling((a / b) / 0.25));
                    break;

                case operation.what10th:
                    return Convert.ToInt32(Math.Ceiling((a / b) / 0.10));
                    break;

                case operation.whatHalf:
                    return Convert.ToInt32(Math.Ceiling((a / b) / 0.50));
                    break;

                case operation.what5th:
                    return Convert.ToInt32(Math.Ceiling((a / b) / 0.20));
                    break;

                case operation.what3rd:
                    return Convert.ToInt32(Math.Ceiling((a / b) / 0.33));
                    break;

                case operation.compare:
                    //if ( == 0) return 1;
                    return a.CompareTo(b);
                    break;

                case operation.log10:
                    return a * a;
                    break;

                default:
                    return a;
                    break;
            }
        }

        public static operation fromString(String op)
        {
            switch (op)
            {
                case "+":
                    return operation.plus;
                    break;

                case "-":
                    return operation.minus;
                    break;

                case "/":
                    return operation.division;
                    break;

                case "*":
                    return operation.multiplication;
                    break;

                case "%":
                    return operation.modulo;
                    break;

                case "":
                    return operation.modulo;
                    break;

                default:
                    return operation.none;
                    break;
            }
        }

        public static Double rangeChange(this Double old, Double delta, Double max, numberRangeModifyRule rule, String op = "+", Double min = 0)
        {
            Double tmpOut = old;
            Double tmpMax = max;
            Double tmpDelta = delta;

            Boolean doCalc = false;

            switch (rule)
            {
                case numberRangeModifyRule.clipToMax:
                case numberRangeModifyRule.loop:
                case numberRangeModifyRule.bounce:
                    doCalc = true;
                    break;

                default:
                    break;
            }

            if (doCalc)
            {
                switch (op)
                {
                    case "+":
                        tmpOut = old + delta;
                        break;

                    case "-":
                        tmpOut = old - delta;
                        break;

                    case "/":
                        tmpOut = old / delta;
                        break;

                    case "*":
                        tmpOut = old * delta;
                        break;

                    case "%":
                        tmpOut = old % delta;
                        break;

                    default:
                        throw new NotImplementedException();
                        //Expression exp = new Expression(old.ToString() + op + delta.ToString());
                        //tmpOut = Double.Parse(exp.Evaluate().ToString());
                        break;
                }
            }

            switch (rule)
            {
                case numberRangeModifyRule.clipToMax:

                    if (tmpOut > tmpMax) tmpOut = tmpMax;
                    break;

                case numberRangeModifyRule.loop:
                    //  tmpOut = Double.Parse(exp.Evaluate().ToString());
                    if (tmpOut > tmpMax) tmpOut = tmpOut % tmpMax;
                    break;

                case numberRangeModifyRule.set:
                    tmpOut = tmpDelta;
                    break;

                case numberRangeModifyRule.bounce:
                    if (tmpOut > tmpMax)
                    {
                        Double t = tmpMax - tmpOut;

                        tmpOut = tmpMax + t;
                        if (tmpOut < min) tmpOut = min;
                        //   tmpOut = tmpOut  tmpMax;
                    }
                    break;

                case numberRangeModifyRule.bypass:
                default:
                    tmpOut = old;
                    break;
            }

            //if (tmpOut < min) tmpOut = min;

            return tmpOut;
        }

        public static Int32 rangeChange(this Int32 old, Int32 delta, Int32 max, numberRangeModifyRule rule,
                                        String op = "+")
        {
            return (Int32)rangeChange((float)old, (float)delta, (float)max, rule, op);
        }
    }
}