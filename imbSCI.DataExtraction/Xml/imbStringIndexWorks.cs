namespace imbSCI.DataExtraction.Xml
{
    using imbSCI.DataExtraction.Xml.enums;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.extensions.typeworks;

    #region imbVeles using

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    //using aceCommonTypes.extensions;

    #endregion imbVeles using

    /// <summary>
    /// Indeksne operacije nad stringom
    /// </summary>
    public static class imbStringIndexWorks
    {
        public const string sep = ", ";
        public static Regex keyReg = new Regex("\\((.*?)\\)");

        /// <summary>
        /// 2014C> prosiruje listu kljuceva za varijante
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="lowLetters"></param>
        /// <param name="capLetters"></param>
        /// <param name="removeDouble"></param>
        /// <returns></returns>
        public static T extendKeys<T>(this T input, Boolean lowLetters = false, Boolean capLetters = false,
                                      Boolean removeDouble = true) where T : class, IEnumerable<string>
        {
            var output = new List<string>();
            if (lowLetters || capLetters)
            {
                var extended = new List<string>();
                foreach (String key in input)
                {
                    if (!(removeDouble && extended.Contains(key)))
                    {
                        extended.Add(key);
                        if (lowLetters) extended.AddUnique(key.ToLower());
                        if (capLetters) extended.AddUnique(key.ToUpper());
                    }
                }
                output = extended;
            }
            else
            {
                return input;
            }

            return output as T;
        }

        /// <summary>
        /// Sve "clanove" objekata u nizu dodaje. Ako je limit -1, onda nema limita. Ako je displayProperty "" onda prikazuje sam objekat prikazuje sa .ToString().
        /// </summary>
        /// <param name="input">Niz objekata</param>
        /// <param name="limit">Koliko članova najviše da prikaže? Ako je limit -1, onda nema limita. Ako prekine niz koristiće endSufix na kraju</param>
        /// <param name="displayProperty">Koji property da prikaže od selektovanog objekta. Podržane su putanje (prop1.prop2.prop3), ako je "" onda će prikazati sam objekat preko .ToString()</param>
        /// <param name="spliter">Znak ili string koji stavlja između elemenata niza</param>
        /// <param name="endSufix">Znak ili string koji stavlja ako prekine niz usled prekoračenja Limita</param>
        /// <param name="eachItemPrefix">Dodaje ispred svakog itema</param>
        /// <param name="eachItemSufix">Dodaje posle svakog itema</param>
        /// <returns>Sređen string</returns>
        public static String imbMembersToStringLine(this IEnumerable<object> input, Int32 limit = -1,
                                                    String displayProperty = "", String spliter = ",",
                                                    String endSufix = "...", Boolean insertOrdinalNumber = false,
                                                    String eachItemPrefix = "", String eachItemSufix = "")
        {
            String output = "";
            int i = 0;
            String ordinalInsert = "";

            if (limit == -1) limit = 10000;

            foreach (Object cls in input)
            {
                i++;
                if ((i < limit))
                {
                    String itemName = "";

                    if (insertOrdinalNumber)
                    {
                        ordinalInsert = i.ToString() + ": ";
                    }
                    else
                    {
                        ordinalInsert = "";
                    }

                    if (cls == null)
                    {
                        itemName = "[null]";
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(displayProperty))
                        {
                            itemName = cls.ToString();
                        }
                        else
                        {
                            itemName = imbTypologyPropertyGetSet.imbPropertyToString(cls, displayProperty);
                        }
                    }

                    if (cls != input.Last())
                    {
                        output += ordinalInsert + eachItemPrefix + itemName + eachItemSufix + spliter;
                    }
                    else
                    {
                        output += ordinalInsert + eachItemPrefix + itemName + eachItemSufix;
                    }
                }
                else if (i == limit)
                {
                    output += endSufix;
                }
                else
                {
                    break;
                }
            }
            return output;
        }

        /// <summary>
        /// Vraća prvu vrednost u listi strigova - može se doradi i na univerzalni nivo
        /// </summary>
        /// <param name="input"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static String imbGetFirstValue(this IEnumerable<string> input, String def)
        {
            String output = "";
            if (input != null)
            {
                if (input.Count() > 0)
                {
                    output = input.First(); //   [0];
                }
            }
            if (String.IsNullOrEmpty(output))
            {
                output = def;
            }
            return output;
        }

        ///// <summary>
        ///// Obrađuju string podatak korišćenjem imbIndexOps metoda
        ///// </summary>
        ///// <param name="indexes">Indeksna instrukcija</param>
        ///// <param name="input">Ulazni string</param>
        ///// <param name="operation">Tip operacije</param>
        ///// <returns>Rezultat</returns>
        //public static String imbIndexOps(this String input, String indexes, indexOps operation = indexOps.pass)
        //{
        //    return
        //        filterStringRange(indexes.rangeStringToIndexList(), input.getStringLineList(StringSplitOptions.None)).
        //            toCsvInLine(Environment.NewLine);
        //}

        ///// <summary>
        ///// Obrađuju string podatak korišćenjem imbIndexOps metoda
        ///// </summary>
        ///// <param name="indexes">Indeksi</param>
        ///// <param name="input">Ulazni string</param>
        ///// <param name="operation">Tip operacije</param>
        ///// <returns>Rezultat</returns>
        //public static String imbIndexOps(this String input, List<int> indexes, indexOps operation = indexOps.pass)
        //{
        //    return filterStringRange(indexes, input.getStringLineList(StringSplitOptions.None)).toCsvInLine(Environment.NewLine);
        //}

        ///// <summary>
        ///// Vraca string koji sadrži samo linije koje imaju needle
        ///// </summary>
        ///// <param name="input">Ulazni string</param>
        ///// <param name="needle">String koji se traži</param>
        ///// <param name="inverseTest">Da li da invertuje razultate</param>
        ///// <returns>Samo linije koje imaju needle</returns>
        //public static String imbLinesWithNeedle(this String input, String needle, Boolean inverseTest = false)
        //{
        //    return
        //        input.getStringLineList(StringSplitOptions.RemoveEmptyEntries, needle, inverseTest).toCsvInLine(
        //            Environment.NewLine);
        //}

        /*
        public static List<T> filterRange<T>(String indexes, List<T> input, indexOps operation = indexOps.pass)
            where T : IRelatedCollectionItem
        {
            if (input == null) return new List<T>();
            List<Int32> indexList = indexes.rangeStringToIndexList();

            return filterRange<T>(indexList, input, operation);
        }

        /// <summary>
        /// Uzima elemente koji su navedeni u listi
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="indexes"></param>
        /// <param name="input"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public static List<T> filterRange<T>(List<Int32> indexes, List<T> input, indexOps operation = indexOps.pass)
            where T : IRelatedCollectionItem
        {
            if (input == null) return new List<T>();
            // Int32 ti = Convert.ToInt32(x.id);
            switch (operation)
            {
                default:
                case indexOps.pass:
                    return input.Where(x => indexes.Contains(Convert.ToInt32(x.id))).ToList();
                    break;

                case indexOps.remove:
                    return input.Where(x => !indexes.Contains(Convert.ToInt32(x.id))).ToList();
                    break;
            }
        }
        */

        ///// <summary>
        ///// Obrađuju string podatak korišćenjem imbIndexOps metoda
        ///// </summary>
        ///// <param name="indexes">Indeksna instrukcija</param>
        ///// <param name="input">Ulazni string</param>
        ///// <param name="operation">Tip operacije</param>
        ///// <returns>Rezultat</returns>
        //public static List<string> filterStringRange(String indexes, List<string> input,
        //                                             indexOps operation = indexOps.pass)
        //{
        //    var ind = indexes.rangeStringToIndexList(input.Count);

        //    List<String> output = new List<string>();

        //    foreach (Int32 i in ind) output.Add(input[i]);

        //    return output;
        //}

        public static List<T> imbIndexOps<T>(this IList<T> source, List<Int32> indexMap, indexOps ops = indexOps.pass)
        {
            List<T> output = new List<T>();

            foreach (Int32 i in indexMap) output.Add(source[i]);

            return output;
        }

        ///// <summary>
        ///// Obrađuju string podatak korišćenjem imbIndexOps metoda
        ///// </summary>
        ///// <param name="indexes">Lista indeksa</param>
        ///// <param name="input">Ulazni string</param>
        ///// <param name="operation">Tip operacije</param>
        ///// <returns>Rezultat</returns>
        //public static List<string> filterStringRange(List<int> indexes, List<string> input,
        //                                             indexOps operation = indexOps.pass)
        //{
        //    return input.imbIndexOps(indexes, operation);
        //}

        ///// <summary>
        ///// HELPER Konvertuje range line u listu indexa. Format:  3-5,8 , 2-8, 9, *
        ///// </summary>
        ///// <remarks>
        /////  ako postoji 3>, to znači od 3 do kraja, ako stoji manje 3 onda kontra
        ///// </remarks>
        ///// <param name="rangeLine">Linija npr> 1=5, 8, 12-20, 3 </param>
        ///// <returns>Listu sa indeksima</returns>
        //public static List<int> rangeStringToIndexList(this String rangeLine, Int32 indMax = 100)
        //{
        //    List<int> output = new List<int>();

        //    List<string> slogovi = imbStringCommonTools.multiOpsInputProcessing(rangeLine);

        //    //string[] slogovi = rangeLine.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

        //    Int32 v;
        //    foreach (String item in slogovi)
        //    {
        //        if (item.Contains("*"))
        //        {
        //            for (v = 0; v < indMax; v++)
        //            {
        //                output.Add(v);
        //            }
        //            break;
        //        }

        //        if (item.Contains("-"))
        //        {
        //            try
        //            {
        //                string[] limits = item.Split("-".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

        //                if (limits.Length > 1)
        //                {
        //                    Int32 start = imbStringFormats.getInt32Safe(limits[0]);
        //                    //Int32.Parse(_numOnly.Replace(limits[0], "", 100));
        //                    Int32 end = imbStringFormats.getInt32Safe(limits[1]);
        //                    //Int32.Parse(_numOnly.Replace(limits[1], "", 100));
        //                    if (end > indMax)
        //                    {
        //                        end = indMax;
        //                    }
        //                    for (v = start; v < end + 1; v++)
        //                    {
        //                        output.Add(v);
        //                    }
        //                }
        //            }
        //            catch
        //            {
        //                throw;
        //                // new aceGeneralException("NLP processing error", null, output, "rangeStringToIndexList()");
        //            }
        //        }
        //        else if (item.Contains(">"))
        //        {
        //            Int32 start = Int32.Parse(imbStringCommonTools._numOnly.Replace(item, "", 100));
        //            Int32 end = indMax;
        //            for (v = start; v < end; v++)
        //            {
        //                output.Add(v);
        //            }
        //        }
        //        else if (item.Contains("<"))
        //        {
        //            Int32 end = Int32.Parse(imbStringCommonTools._numOnly.Replace(item, "", 100));
        //            if (end > indMax)
        //            {
        //                end = indMax;
        //            }
        //            Int32 start = 0;
        //            for (v = start; v < end; v++)
        //            {
        //                output.Add(v);
        //            }
        //        }
        //        else if (item.Contains("nth"))
        //        {
        //            Int32 step = imbStringFormats.getInt32Safe(item, 2);
        //            Int32 a;
        //            for (a = 0; a < indMax; a = a + step)
        //            {
        //                output.Add(a);
        //            }
        //        }
        //        else
        //        {
        //            output.Add(imbStringFormats.getInt32Safe(item));
        //        }
        //    }

        //    List<int> uniqueOutput = new List<int>();
        //    foreach (Int32 index in output)
        //    {
        //        if (!uniqueOutput.Contains(index))
        //        {
        //            uniqueOutput.Add(index);
        //        }
        //    }

        //    return uniqueOutput;
        //}

        /*
        /// <summary>
        /// Vraca Relation Keys niz
        /// </summary>
        /// <param name="relObject"></param>
        /// <returns></returns>
        public static List<String> getRelationKeyList(this IRelatedObject relObject)
        {
            List<String> output = new List<string>();

            if (String.IsNullOrEmpty(relObject.value)) return output;

            MatchCollection mc = keyReg.Matches(relObject.value);
            foreach (Match m in mc)
            {
                output.Add(m.Value);
            }
            if (output.Count == 0)
            {
                output.Add(relObject.value);
            }
            return output;
        }

    */

        /*
    /// <summary>
    /// Pretvara listu kljuceva u string
    /// </summary>
    /// <param name="relObject"></param>
    /// <param name="keyList">Tip podatka prosledjenih kljuceva</param>
    /// <returns></returns>
    public static String getStringFromKeys(this IList keyList, imbTypeInfo iTI = null)
    {
        String output = "";
        Int32 ind = 0;
        Int32 lim = keyList.Count - 1;
        String q = "";
        if (iTI == null) q = keyList.GetType().getTypology().genericSubType.getTypology().getQuote();
        if (iTI != null) q = iTI.getQuote();
        foreach (var k in keyList)
        {
            if (k != null)
            {
                output += "" + q + k.ToString() + q + "";

                if (ind < lim) output += sep;
            }
            ind++;
        }

        return output;
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="input"></param>
    /// <param name="iTI">Tip propertija ! ne host objekta</param>
    /// <returns></returns>
    public static List<T> getKeysFromString<T>(this String input, imbTypeInfo iTI = null)
    {
        var output = new List<T>();
        if (String.IsNullOrEmpty(input)) return output;

        //Int32 ind = 0;
        Type tt = typeof (Int32);
        String q = "";
        if (iTI != null)
        {
            q = iTI.getQuote();
        }
        else
        {
            q = typeof(T).getTypology().getQuote();
        }
        String[] keyList = input.Split(new[] {sep}, StringSplitOptions.RemoveEmptyEntries);
        if (!Enumerable.Any(keyList)) keyList[0] = input;
        //Int32 lim = keyList.Count - 1;

        foreach (var k in keyList)
        {
            String tm = k.Trim();
            T vl = (T)imbTypeExtensions.imbConvertValueSafe(tm, tt); // tm.imbConvertValueSafe(tt);
            output.Add(vl);
        }
        return output;
    }

    /// <summary>
    /// Vraca listu targetPropPi vrednosti [obicno je> uniqueColumnKey]
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="relObject"></param>
    /// <param name="relItems"></param>
    /// <returns></returns>
    public static List<T> extractRelationKeyList<T>(this IRelatedObject relObject, IList relItems)
    {
        List<T> output = new List<T>();
        foreach (Object rci in relItems)
        {
            Object vl = rci.imbGetPropertySafe(relObject.targetPropPi, null, false);
            output.Add((T) imbTypeExtensions.imbConvertValueSafe(vl, relObject.targetPropPi.type));
        }
        return output;
    }

    public static List<T> extractRelationItems<T, TK>(this IRelatedObject relObject, IList<TK> keyList)
        where T : class, IRelatedCollectionItem, new()
    {
        List<T> output = new List<T>();

        //relObject.targetCollection.selectItems<T,TK>(keyList as List<TK>, entity.enums.selectItemsMode.sqlWhere, relObject.targetPropPi, entity.enums.selectItemsResultType.managed, queryWhereOperator.OR, true, null, -1);

        //output = relObject.targetCollection.selectItems<IRelatedCollectionItem, >(keyList, relObject.targetPropPi, null);
        return output;
    }
    */
    }
}