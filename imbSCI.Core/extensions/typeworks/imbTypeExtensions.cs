// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbTypeExtensions.cs" company="imbVeles" >
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
namespace imbSCI.Core.extensions.typeworks
{
    using imbSCI.Core.enums;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.enumworks;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class imbTypeExtensions
    {
        /// <summary>
        /// Supports> String, Int32, any number via Double and Boolean
        /// </summary>
        /// <param name="exValue"></param>
        /// <param name="cnValue"></param>
        /// <returns></returns>
        public static Object sumValues(this Object exValue, Object cnValue)
        {
            Object result = cnValue;
            cnValue = cnValue.imbConvertValueSafe(exValue.GetType());
            if (exValue is String)
            {
                result = (String)(exValue as String) + (cnValue as String);
            }
            else if (exValue is Int32)
            {
                result = ((Int32)exValue) + ((Int32)cnValue);
            }
            else if (exValue.GetType().isNumber())
            {
                result = exValue.imbToNumber<Double>() + cnValue.imbToNumber<Double>();
            }
            else if (exValue is Boolean)
            {
                Boolean exb = exValue.imbToBoolean();
                Boolean cnb = cnValue.imbToBoolean();
                result = exb && cnb;
            }
            else
            {
            }
            return result;
        }

        //public static Boolean isBoolean(this Type t)
        //{
        //    if (t.Name.ToLower().StartsWith("bool")) return true;
        //    return false;
        //}

        //public static Boolean isText(this Type t)
        //{
        //    if (t.Name.ToLower().StartsWith("string")) return true;
        //    return false;
        //}

        /// <summary>
        /// Pravi instancu preko konstruktora kojem odgovaraju ovi parametri
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static Object getInstance(this Type t, Object[] settings = null)
        {
            Object output = null;

            if (settings == null) settings = new Object[0];
            if (t.isNumber())
            {
                output = 0;
            }
            else if (t.isBoolean())
            {
                output = false;
            }
            else if (t == typeof(String))
            {
                output = "";
            }
            else
            {
                output = Activator.CreateInstance(t, settings);
            }

            return output;
        }

        /// <summary>
        /// Returns type category string description: enum, array (for collections), object or type name if type is simple input.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static String toStringTypeTitle(this Type t)
        {
            String tname = t.Name;

            if (t.IsEnum)
            {
                tname = "enum";
            }
            else if (t.isCollection())
            {
                tname = "array";
            }
            else if (t.isSimpleInputEnough() || t.isToggleValue())
            {
                tname = t.Name;
            }
            else
            {
                tname = "object";
            }
            return tname;
        }

        ///// <summary>
        ///// Proverava da li je input null, ako je string onda ga proverava kao string ako je neki drugi objekat onda ga predvara u string pa proverava
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //public static Boolean isNullOrEmptyString(this Object input)
        //{
        //    if (input == null)
        //    {
        //        return true;
        //    }
        //    if (input is DBNull)
        //    {
        //        return true;
        //    }
        //    if (input is String)
        //    {
        //        return String.IsNullOrEmpty(input as String);
        //    }
        //    return false;
        //}

        public static T imbConvertValueSafeTyped<T>(this Object vl)
        {
            Type t = typeof(T);

            Object tmp = vl.imbConvertValueSafe(t);
            if (tmp == null) return default(T);
            try
            {
                if (tmp.GetType().isCompatibileWith(t))
                {
                    return (T)tmp;
                }
            }
            catch (Exception ex)
            {
                // vl.note(ex, "Converting [" + vl.ToString() + " (" + vl.GetType().Name + ") to " + t.Name);
            }
            return default(T);
        }

        /// <summary>
        /// 2014c> bezbedna konverzija vrednosti
        /// </summary>
        /// <param name="input">Objekat sa izvornom vrednosti</param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static Object imbConvertValueSafe(this Object input, Type targetType)
        {
            if (input == null)
            {
                return targetType.GetDefaultValue();
            }
            Object output = null;

            Type inputTi = input.GetType();
            Type outputTi = targetType;

            if (inputTi.isCompatibileWith(targetType))
            {
                return input;
            }

            switch (outputTi.getTypeGroup())
            {
                case imbTypeGroup.boolean:
                    output = input.imbToBoolean(false);
                    break;

                case imbTypeGroup.enumeration:
                    output = input.imbToEnumeration(targetType);
                    break;

                case imbTypeGroup.number:
                    output = input.imbToNumber(targetType);
                    break;

                case imbTypeGroup.text:
                    output = input.ToString();
                    break;

                case imbTypeGroup.instance:
                case imbTypeGroup.unknown:
                    if (targetType.IsValueType)
                    {
                        output = Convert.ChangeType(input, targetType);
                    }
                    else
                    {
                        output = input.imbToInstance(targetType);
                    }
                    break;
            }
            return output;

            /*

            String vT =
            String pT = targetType.Name.ToLower();

            if (!compareTypes(vlTt, targetType))
            {
                if (targetType.IsEnum)
                {
                    vl = targetType.getEnumByName(vl.ToString());
                }
                else
                {
                    Object old = vl;

                    switch (pT)
                    {
                        case "bool":
                        case "boolean":
                            if (vT.conv)
                            vl = Convert.ChangeType(vl, targetType);

                            vl = true;
                            break;

                        case "string":
                            vl = vl.ToString();
                            break;

                        case "int32":
                            Int32 br = 0;
                            Int32.TryParse(vl as String, out br);
                            vl = br;
                            break;

                        default:
                            vl = null;

                            logSystem.log("Target type not supported - null value set: " + pT.ToLower(),
                                          logType.FatalError);
                            break;
                    }

                    targetType.GetDefaultValue();
                }
            }
            return vl;
             * */
        }

        public static T imbToNumber<T>(this Object input, Type numberType = null)
        {
            Object output = input.imbToNumber(typeof(T));
            return (T)output;
        }

        /// <summary>
        /// Konvertuje u broj prema zadatom tipu
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Object imbToNumber(this Object input, Type numberType = null)
        {
            if (numberType == null) numberType = typeof(Int32);

            var outputTi = numberType; //.getTypology();

            if (input == null) return outputTi.GetDefaultValue();
            Object output = outputTi.GetDefaultValue();

            Type inputTi = input.GetType(); //.getTypology();
            String tmp = "";

            if (input is DBNull) input = numberType.GetDefaultValue();

            switch (inputTi.getTypeGroup())
            {
                case imbTypeGroup.boolean:
                    Int32 vl = -1;
                    Boolean bl = (Boolean)input;
                    if (bl)
                    {
                        vl = 1;
                    }
                    else
                    {
                        Int64 minval = Convert.ToInt64(outputTi.ReadStaticField("MinValue"));
                        if (minval < 0)
                        {
                            vl = -1;
                        }
                        else
                        {
                            vl = 0;
                        }
                    }
                    output = Convert.ChangeType(vl, numberType);
                    break;

                case imbTypeGroup.enumeration:
                    output = Convert.ChangeType((Int32)input, numberType);
                    break;

                case imbTypeGroup.number:
                case imbTypeGroup.instance:

                    if (input is IConvertible)
                    {
                        output = Convert.ChangeType(input, numberType);
                    }
                    else
                    {
                        // var isb = imbStringBuilder.start("input was not convertable error");
                        // isb.AppendLine("input : " + input.toStringSafe());

                        input = numberType.GetDefaultValue();
                        output = Convert.ChangeType(input, numberType);

                        // Exception exd = new aceGeneralException(isb.ToString());
                        // devNoteManager.note(input, exd, isb.ToString(), "input was not convertable", devNoteBehaviour.openDevNote, devNoteType.typology);
                    }

                    break;

                case imbTypeGroup.unknown:
                case imbTypeGroup.text:
                    String pharseInput = input.ToString().ToLower().Trim();

                    if (numberType == typeof(Decimal))
                    {
                        decimal fo;
                        Decimal.TryParse(pharseInput, out fo);
                        output = Convert.ChangeType(fo, numberType);
                    }
                    else
                    {
                        double fo;
                        Double.TryParse(pharseInput, out fo);
                        output = Convert.ChangeType(fo, numberType);
                    }
                    break;
            }
            return output;
        }

        /// <summary>
        /// Konvertuje u potrebni tip. Ukoliko postoji konstruktor sa parametrom koji je IsInstanceOfType(input) onda ce koristit njega
        /// </summary>
        /// <remarks>
        /// Vraca Null: Ako je targetType == null, input == null
        /// Vraca Input (nepromenjen): Ako je targetType.IsInstanceOfType(input)
        /// Ako postoji konstruktor> new targetType(input) (isti ili kompatibilan tip parametra, onda ce pozvati taj konstruktor). isti tip parametra ima prednost nad kompatibilnim
        /// Ako nema kompatibilnog konstruktora, pravi novu instancu i kopira istoimene propertije iz inputa
        /// </remarks>
        /// <param name="input"></param>
        /// <param name="targetType"></param>
        /// <returns>Instancu objekta</returns>
        public static Object imbToInstance(this Object input, Type targetType)
        {
            if (targetType == null) return null;

            Type outputTi = targetType;
            Object output = null;
            if (input == null) return output;

            if (targetType.IsInstanceOfType(input)) return input;

            MethodInfo iMI = null;
            ConstructorInfo oneParameter = outputTi.GetConstructors().Where(x => x.GetParameters().Count() == 1).imbFirstSafe();
            List<Object> inputPar = new List<object>();
            inputPar.Add(input);
            output = oneParameter.Invoke(inputPar.ToArray());

            /*
            if (oneParameter.Any())
            {
                iMI = oneParameter.imbFirstSafe(x => x.GetMethodBody()..GetParameters().Any(xp => xp.ParameterType == input));
                if (iMI == null)
                {
                    iMI =
                        oneParameter.imbFirstSafe(
                            x => x.methodBase.GetParameters().Any(xp => xp.ParameterType.IsInstanceOfType(input)));
                }
            }
            if (iMI == null)
            {
                output = outputTi.getInstance(makeInstanceMode.copyFromSource, input);
            }
            else
            {
                output = iMI.methodBase.Invoke(null, new Object[] {input});
            }*/
            return output;
        }

        /// <summary>
        /// Konvertuje bilo koju vrednost u Boolean
        /// </summary>
        /// <param name="input">Objekat razlicite vrednosti</param>
        /// <param name="defaultResult">Rezultat koji se vraca ako je null. Kontra od ovoga ce vratiti ako je u pitanju instanca</param>
        /// <returns>true ili false u skladu sa konverzijom</returns>
        public static Boolean imbToBoolean(this Object input, Boolean defaultResult = false)
        {
            if (input == null) return defaultResult;
            //bTypeInfo inputTi = input.getTypology();

            Type inputTi = input.GetType();

            String tmp = "";
            switch (inputTi.getTypeGroup())
            {
                case imbTypeGroup.boolean:
                    return (Boolean)input;
                    break;

                case imbTypeGroup.enumeration:
                    Object first = Enum.GetValues(inputTi).getFirstSafe();
                    return input != first;
                    break;

                case imbTypeGroup.instance:
                    return !defaultResult;
                    break;

                case imbTypeGroup.number:
                    Int32 numValue = 0;
                    if (input is Int32)
                    {
                        numValue = (Int32)input;
                    }
                    else
                    {
                        numValue = Convert.ToInt32(input);
                    }

                    return (numValue > 0);
                    break;

                case imbTypeGroup.unknown:
                case imbTypeGroup.text:
                    tmp = input.ToString().ToLower().Trim();
                    break;
            }

            switch (tmp)
            {
                case "y":
                case "yes":
                case "t":
                case "1":
                case "true":
                case "on":
                case "ok":
                case "accept":
                case "confirm":
                case "retry":
                case "+":
                    return true;
                    break;

                case "n":
                case "dbnull":
                case "null":
                case "no":
                case "f":
                case "0":
                case "-1":
                case "false":
                case "off":
                case "cancel":
                case "ignore":
                case "abort":
                case "-":
                    return false;
                    break;
            }

            return defaultResult;
        }

        internal static String compareAkaCheck(this String tn)
        {
            switch (tn.ToLower())
            {
                case "runtimetype":
                    return "type";
                    break;

                default:
                    return tn;
                    break;
            }
        }

        /// <summary>
        /// Da li su prosledjeni tipovi isti ili da li je jedan od tipova null
        /// </summary>
        /// <param name="typeA"></param>
        /// <param name="typeB"></param>
        /// <returns></returns>
        internal static Boolean compareTypes(Type typeA, Type typeB)
        {
            if (typeA == null)
            {
                if (typeB == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            String tnA = typeA.Name.ToLower().compareAkaCheck();
            String tnB = typeB.Name.ToLower().compareAkaCheck();

            return (tnA == tnB);
        }
    }
}