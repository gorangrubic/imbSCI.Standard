// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbDataExecutor.cs" company="imbVeles" >
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
namespace imbSCI.Core.process
{
    #region imbVeles using

    using imbSCI.Core.extensions.text;
    using imbSCI.Core.files;
    using System;
    using System.Collections.Generic;
    using System.Data;

    #endregion imbVeles using

    /// <summary>
    /// PREVAZIDJENO
    /// </summary>
    public static class imbDataExecutor
    {
        /// <summary>
        /// 2013a> bezbedno vraca string verziju prosledjene vrednosti - Kreira stringValue
        /// </summary>
        /// <param name="input">Neki objekat koji treba da konvertuje</param>
        /// <returns></returns>
        public static String getStringValue(this Object input)
        {
            String output = "";

            if (input == null)
            {
                return "";
            }

            Type iType = input.GetType();

            try
            {
                if (iType.IsClass)
                {
                    output = objectSerialization.ObjectToXML(input);
                    //(String)
                    //imbSerialization.serializeValue(input, imbSerializationMode.JSON,
                    //                                imbCoreManager.supportedAsChildren.ToArray());
                }
                else
                {
                    output = input.ToString();
                }
            }
            catch (Exception ex)
            {
                output = "";
            }

            return output;
        }

        public static Boolean isBooleanLogic(dataLogic input)
        {
            switch (input)
            {
                case dataLogic.OR:
                case dataLogic.AND:
                    return true;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Privremeni string wrapper
        /// </summary>
        /// <param name="logic"></param>
        /// <param name="operandA"></param>
        /// <param name="operandB"></param>
        /// <returns></returns>
        public static String executeLogic(dataLogic logic, String operandA, String operandB)
        {
            return executeLogic(logic, operandA as Object, operandB as Object) as String;
        }

        /// <summary>
        /// Izvrsava jednostavnu logiku - i prosledjuje kompleksnoj ako je potrebno
        /// </summary>
        /// <param name="logic"></param>
        /// <param name="operandA">Domacin, target</param>
        /// <param name="operandB">Nova vrednost</param>
        /// <returns></returns>
        public static Object executeLogic(dataLogic logic, Object operandA, Object operandB)
        {
            Object output = operandA;

            String strA = "";
            String strB = "";

            switch (logic)
            {
                case dataLogic.set:
                    return operandB;
                    break;

                case dataLogic.setIfEmpty:
                    if (operandA == null)
                    {
                        return operandB;
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(operandA.ToString()))
                        {
                            return operandB;
                        }
                        else
                        {
                            return operandA;
                        }
                    }
                    break;

                default:
                    break;
            }

            return output;
        }

        /// <summary>
        /// Vraća NCalc string oblik logike
        /// </summary>
        /// <param name="logic"></param>
        /// <returns></returns>
        public static String getStringOperator(dataLogic logic)
        {
            switch (logic)
            {
                case dataLogic.OR:
                    return "||";

                case dataLogic.AND:
                    return "&&";

                default:
                case dataLogic.plus:
                    return "+";

                case dataLogic.minus:
                    return "-";

                case dataLogic.divide:
                    return "/";

                case dataLogic.combine:
                    return "*";
            }
        }

        public static Type getRecommandedType(dataLogic logic, Type defaultType)
        {
            switch (logic)
            {
                case dataLogic.OR:
                case dataLogic.AND:
                    return typeof(Boolean);

                case dataLogic.plus:
                case dataLogic.minus:
                    return typeof(Int32);

                case dataLogic.divide:
                case dataLogic.combine:
                    return typeof(Double);

                default:
                    return defaultType;
                    break;
            }
        }

        public static Double Compute(String mathExpression)
        {
            return (Double)new DataTable().Compute(mathExpression, null);
        }

        public static Object executeComplexLogic(dataLogic logic, Object operandA, Object operandB)
        {
            Object output = null;

            Type typeA = operandA.GetType();
            Type typeB = operandB.GetType();

            Object safeA = operandA.getDataTypeSafe(typeA);
            Object safeB = operandB.getDataTypeSafe(typeB);

            typeA = safeA.GetType();
            typeB = safeB.GetType();

            String typeNameA = typeA.Name;
            String nOp = getStringOperator(logic);

            switch (typeNameA)
            {
                case "Int32":
                case "Double":
                case "Decimal":
                case "Float":
                case "Boolean":
                    try
                    {
                        output = Compute(safeA.ToString() + nOp + safeB.ToString());
                    }
                    catch
                    {
                        output = operandB;
                    }
                    break;

                case "String":
                    switch (logic)
                    {
                        default:
                        case dataLogic.plus:
                            output = safeA.ToString() + safeB.ToString();
                            break;

                        case dataLogic.minus:
                            output = safeB.ToString() + safeA.ToString();
                            break;
                    }

                    break;
            }

            if (output != null) return output;

            if (typeA != typeB) return null;

            return output;
        }

        /// <summary>
        /// Obradjuje podatak iz ucitanog stringa
        /// </summary>
        public static String getTypeName(String loadLine)
        {
            String output = "";
            if (loadLine.IndexOf("[type:") == 0)
            {
                loadLine = loadLine.Replace("[type:", "");
                loadLine = loadLine.Substring(0, loadLine.IndexOf("]"));
                output = loadLine;
            }
            else
            {
                output = "String";
            }
            return output;
        }

        public static String getDataOnly(String loadLine)
        {
            int len = loadLine.IndexOf("]");
            if (len < 0)
            {
                return loadLine;
            }
            else
            {
                String output = loadLine.Substring(0, len + 1);
                output = loadLine.Replace(output, "");
                return output;
            }
        }

        public static List<Object> toObjectList(List<String> input)
        {
            List<Object> output = new List<object>();
            foreach (String child in input) output.Add((Object)child);
            return output;
        }

        public static List<Object> toObjectList(List<Int32> input)
        {
            List<Object> output = new List<object>();
            foreach (Int32 child in input) output.Add((Object)child);
            return output;
        }

        public static List<Object> toObjectList(List<Double> input)
        {
            List<Object> output = new List<object>();
            foreach (Double child in input) output.Add((Object)child);
            return output;
        }

        public static List<Object> toObjectList(List<Boolean> input)
        {
            List<Object> output = new List<object>();
            foreach (Boolean child in input) output.Add((Object)child);
            return output;
        }

        public static List<String> toStringList(List<Object> input)
        {
            List<String> output = new List<String>();
            foreach (Object child in input) output.Add((String)child);
            return output;
        }

        public static List<Int32> toInt32List(List<Object> input)
        {
            List<Int32> output = new List<Int32>();
            foreach (Object child in input) output.Add((Int32)child);
            return output;
        }

        public static List<Double> toDoubleList(List<Object> input)
        {
            List<Double> output = new List<Double>();
            foreach (Object child in input) output.Add((Double)child);
            return output;
        }

        public static List<Boolean> toBooleanList(List<Object> input)
        {
            List<Boolean> output = new List<Boolean>();
            foreach (Object child in input) output.Add((Boolean)child);
            return output;
        }

        public static List<Object> combineArray(List<Object> tmpListString, List<Object> tmpListStringB, dataLogic logic)
        {
            List<Object> output = tmpListString;
            switch (logic)
            {
                case dataLogic.plus:
                    foreach (Object child in tmpListStringB) tmpListString.Add(child);
                    output = tmpListString;
                    break;

                case dataLogic.minus:
                    foreach (Object child in tmpListStringB) tmpListString.Remove(child);
                    output = tmpListString;
                    break;

                case dataLogic.combine:
                    List<Object> combined = new List<Object>();
                    foreach (Object child in tmpListString)
                    {
                        if (!combined.Contains(child))
                        {
                            combined.Add(child);
                        }
                    }
                    foreach (Object child in tmpListStringB)
                    {
                        if (!combined.Contains(child))
                        {
                            combined.Add(child);
                        }
                    }
                    output = combined;
                    break;

                case dataLogic.divide:
                    List<Object> tmpCombined = combineArray(tmpListString, tmpListStringB, dataLogic.combine);
                    foreach (Object child in tmpCombined) tmpListString.Remove(child);
                    output = tmpListString;
                    break;
            }
            return output;
        }

        /// <summary>
        ///2013a> Proverava da li je prosledjena ista vrednost
        /// </summary>
        /// <param name="__valA"></param>
        /// <param name="__valB"></param>
        /// <param name="__targetType"></param>
        /// <returns></returns>
        public static Boolean isSameValue(Object __valA, Object __valB, Type __targetType = null)
        {
            Boolean output = false;
            Object __valBtyped = null;
            String __valBstr = "";
            String __valAstr = "";

            if (__valA == null)
            {
                if (__valB == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (__valB == null)
                {
                    return false;
                }
            }

            if (__targetType == null) __targetType = __valA.GetType();

            String tn = __targetType.Name;

            if (__targetType.IsPrimitive)
            {
                switch (tn)
                {
                    case "Boolean":
                        output = ((Boolean)__valA == (Boolean)__valB);
                        break;

                    case "Int16":
                        output = ((Int16)__valA == (Int16)__valB);
                        break;

                    case "Int32":
                        output = ((Int32)__valA == (Int32)__valB);
                        break;

                    case "Int64":
                        output = ((Int64)__valA == (Int64)__valB);
                        break;

                    case "Char":
                        output = ((Char)__valA == (Char)__valB);
                        break;

                    case "Double":
                        output = ((Double)__valA == (Double)__valB);
                        break;

                    case "Byte":
                        output = ((Byte)__valA == (Byte)__valB);
                        break;

                    default:
                        output = (__valA == __valB);
                        break;
                }
            }
            else
            {
                switch (tn)
                {
                    case "String":
                        output = ((String)__valA == (String)__valB);
                        break;

                    default:
                        if (__valA.GetHashCode() == __valB.GetHashCode())
                        {
                            return true;
                        }
                        else
                        {
                            try
                            {
                                output = (__valA == Convert.ChangeType(__valB, __targetType));
                            }
                            catch (Exception ex)
                            {
                                __valBstr = imbDataExecutor.getStringValue(__valB);
                                __valAstr = imbDataExecutor.getStringValue(__valA);
                                output = (__valBstr == __valAstr);
                            }
                        }
                        /*

                          * */
                        break;
                }
            }

            return output;
        }
    }
}