// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbStringBuilder.cs" company="imbVeles" >
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

//namespace imbSCI.Core.reporting
//{
//    #region imbVeles using

//    using System;
//    using System.Collections;
//    using System.Collections.Generic;
//    using System.Data;
//    using System.IO;
//    using System.Linq;
//    using System.Reflection;
//    using System.Xml.Serialization;
//    using imbSCI.Core.reporting.format;
//    using imbSCI.Core.reporting.render;
//    using imbSCI.Core.reporting.render.converters;
//    using imbSCI.Core.reporting.render.core;
//    using imbSCI.Data.enums.reporting;
//    using imbSCI.Data;
//    using imbSCI.Core.extensions.typeworks;
//    using imbSCI.Data.enums.appends;
//    using imbSCI.Core.extensions.text;
//    using imbSCI.Core.extensions;
//    using imbSCI.Data.enums;
//    using imbSCI.Data.data.maps.datamap;
//    using imbSCI.Data.interfaces;

//    #endregion

//    /// <summary>
//    /// Napredni konstruktor stringa
//    /// </summary>
//    public class imbStringBuilder : imbStringBuilderBase, ITextRender
//    {
//        /// <summary>
//        /// konstruktor koji postavlja tabLevel, podrazumevani tab level je 2
//        /// </summary>
//        /// <param name="__tabLevel"></param>
//    public imbStringBuilder(Int32 __tabLevel) : base(__tabLevel)
//        {
//        }

//        /// <summary>
//        /// konstruktor sa podrazumevanim tabLevelom (2)
//        /// </summary>
//        public imbStringBuilder()
//        {
//            //tabInsert = tab.Repeat(tabLevel);
//        }

//        /// <summary>
//        /// Argumenti koji su bili null do sada
//        /// </summary>
//        [XmlIgnore]
//        public List<String> failedArguments
//        {
//            get { return _failedArguments; }
//            set
//            {
//                _failedArguments = value;
//                OnPropertyChanged("failedArguments");
//            }
//        }

//        public override converterBase converter
//        {
//            get
//            {
//                throw new NotImplementedException();
//            }
//        }

//        /// <summary>
//        /// Pravi development note ako se desilo da neki provereni argument bude null -- nakon toga resetuje isArgumentFailed i failedArguments listu
//        /// </summary>
//        /// <param name="caller">Objekat koji prosledjuje kao caller</param>
//        /// <returns>TRUE ako je bacio note, FALSE ako nije</returns>
//        public Boolean noteOnFailAndReset(Object caller)
//        {
//            if (isArgumentFailed)
//            {
//                //String args = failedArguments.Join(", ");

//                //ArgumentNullException ex = new ArgumentNullException(args,
//                //                                                     "Arguments was null - tested by imbStringBuilder");

//                //StackTrace st = new StackTrace(true);

//                //caller.note(this.ToString(), ex, devNoteType.argumentFail, st.GetFrame(0));

//                //isArgumentFailed = false;
//                //failedArguments.Clear();

//                return true;
//            }
//            return false;
//        }

//        /// <summary>
//        /// Instancira novi bilder i pravi poruku
//        /// </summary>
//        /// <param name="message"></param>
//        /// <returns></returns>
//        public static imbStringBuilder start(String message)
//        {
//            imbStringBuilder iSB = new imbStringBuilder(0);
//            iSB.AppendLine(message);
//            return iSB;
//        }

//        /// <summary>
//        /// Formira logBlock na osnovu naslova i trenutnog sadrzaja StringBuildera
//        /// </summary>
//        /// <param name="title">Naslov koji se dodeljuje bloku</param>
//        /// <returns>logBlock</returns>
//        /// <summary>
//        /// Checks if value is isNullOrEmptyString() -- if yes Appends log message and returns TRUE, if no returns FALSE (everything is ok)
//        /// </summary>
//        /// <param name="value">Value of the property</param>
//        /// <param name="propertyName">Name of the property</param>
//        /// <param name="customMessage">Some additional message to write</param>
//        /// <returns>If value is null returns TRUE. Ako je true onda se upisuje u isArgumentFailed property na samom imbStringBuilder-u</returns>
//        public Boolean AppendIfNull(Object value, String propertyName, IObjectWithName propertyHost = null,
//            String customMessage = "")
//        {
//            if (imbSciStringExtensions.isNullOrEmptyString(value))
//            {
//                open(htmlTagName.div, htmlClassForReport.member);
//                if (propertyHost != null)
//                    Append((htmlTagName) htmlTagName.span, "at object[" + propertyHost.name + ":" + propertyHost.toStringSafe() + "] ");
//                AppendPair(propertyName, " is null or empty", htmlTagName.span, htmlClassForReport.itemName.ToString(),
//                    htmlClassForReport.member.ToString(), true, htmlTagName.span, false);
//                if (!String.IsNullOrEmpty(customMessage)) Append((htmlTagName) htmlTagName.p, customMessage);

//                close();
//                failedArguments.Add(propertyName);
//                isArgumentFailed = true;
//                return true;
//            }
//            return false;
//        }

//        /// <summary>
//        /// Dodaje sadrzaj u novu liniju, i pomera tab
//        /// </summary>
//        /// <param name="content">Tekstualni sadrzaj koji se prikazuje</param>
//        /// <param name="tabChange"></param>
//        public void AppendLine(String content, Int32 tabChange, htmlTagName tag = htmlTagName.none,
//            String htmlClass = "", Boolean isHtmlContent = false)
//        {
//            tabLevel += tabChange;
//            tag = reportOutputFormatTools.checkForDefaultTag(tag, reportOutputRoles.appendLine);

//            AppendLine(content, tag, htmlClass, "", isHtmlContent);
//        }

//        public void AppendPairs(Object target, String title, params String[] propertiesToReport)
//        {
//            open((htmlTagName) htmlTagName.div, title.imbHtmlEncode(), (htmlClassForReport) htmlClassForReport.reportBlockContainer);
//            AppendLine("Properties of: " + title, htmlTagName.h1);

//            if (target == null)
//            {
//                AppendLine((string) "Source is null", (htmlTagName) htmlTagName.p, (htmlClassForReport) htmlClassForReport.note, htmlIdForReport.none, false);
//                return;
//            }

//            AppendLine("Type: " + target.GetType().Name, htmlTagName.h1);
//            AppendPairs(target, htmlTagName.dt, htmlClassForReport.itemName, htmlTagName.dt,
//                htmlClassForReport.variables, htmlTagName.dd, htmlTagName.dl, propertiesToReport);

//            close();
//        }

//        /// <summary>
//        ///
//        /// </summary>
//        /// <param name="key"></param>
//        /// <param name="value"></param>
//        /// <param name="tag"></param>
//        /// <param name="keyClass"></param>
//        /// <param name="valueClass"></param>
//        /// <param name="insertId"></param>
//        /// <param name="containerTag"></param>
//        /// <param name="isHtmlContent"></param>
//        public void AppendPair(String key, String value, htmlTagName tag, String keyClass, String valueClass,
//            Boolean insertId = true, htmlTagName containerTag = htmlTagName.p,
//            Boolean isHtmlContent = false)
//        {
//            tag = reportOutputFormatTools.checkForDefaultTag(tag, reportOutputRoles.appendPair);
//            containerTag = reportOutputFormatTools.checkForDefaultTag(containerTag, reportOutputRoles.appendPairContainer);

//            String id_key = "";
//            String id_value = "";
//            if (insertId)
//            {
//                id_key = key + "_itemname";
//                id_value = key + "_value";
//            }
//            String keyHtml = key.WrapHtml(tag, keyClass, id_key, isHtmlContent);
//            String valHtml = value.WrapHtml(tag, valueClass, id_value, isHtmlContent);

//            String output = keyHtml + valHtml;
//            AppendLine(output, containerTag, "", "", true);
//            // Append(containerTag, output);
//            /*

//            Append(keyHtml);
//            Append(valHtml); */
//        }

//        public void AppendPairLine(String key, String value, Boolean isHtmlContent = false)
//        {
//            AppendPair(key, value, isHtmlContent);
//            AppendLine();
//        }

//        /// <summary>
//        /// Adds key/value pair - supports non HTML content as well
//        /// </summary>
//        /// <param name="key"></param>
//        /// <param name="value"></param>
//        /// <param name="isHtmlContent"></param>
//        public void AppendPair(String key, String value, Boolean isHtmlContent = false)
//        {
//            if (isHtmlContent)
//            {
//                AppendPair(key,
//                    value,
//                    htmlTagName.span,
//                    htmlClassForReport.itemName.ToString(),
//                    key.ToLower().Trim(),
//                    true,
//                    htmlTagName.p,
//                    isHtmlContent);
//            }
//            else
//            {
//                Append(imbSciStringExtensions.add(key, value, ": "));
//            }
//        }

//        public void AppendPair(string v1, string description, htmlTagName p1, htmlClassForReport itemName,
//            htmlClassForReport note, bool v2 = false, htmlTagName p2 = htmlTagName.p)
//        {
//            open(p1, description.imbHtmlEncode(), itemName);
//            if (v2)
//            {
//                AppendLine(v1, p2, itemName, htmlIdForReport.none);
//            }
//            else
//            {
//                AppendLine(v1, p2, note, htmlIdForReport.none);
//            }
//            close();

//            //this.AppendPair(v1, description, p1, itemName, note, p2, p2);
//            // throw new NotImplementedException();
//        }

//        public void AppendPair(string v, string name, htmlTagName span, htmlClassForReport itemName,
//            htmlClassForReport variables)
//        {
//            open(span, itemName);
//            Append(name);
//            close();

//            open(span, variables);
//            Append(v);
//            close();

//            //throw new NotImplementedException();
//        }

//        public void AppendPairs(Object target, htmlTagName tagItem, htmlClassForReport classItem, htmlTagName tagValue,
//            htmlClassForReport classValue, htmlTagName tagPair, htmlTagName tagMain,
//            String[] propertiesToReport)
//        {
//            Type iti = target.GetType();

//            List<String> propList = new List<string>();
//            if (propertiesToReport.Any())
//            {
//                var props = iti.GetProperties();
//                foreach (var pi in props)
//                {
//                    propList.Add(pi.Name);
//                }
//                propertiesToReport = propList.ToArray();
//            }

//            open(tagMain, iti.Name, (htmlClassForReport) htmlClassForReport.reportBlockContainer);
//            foreach (String prop in propertiesToReport)
//            {
//                PropertyInfo pi = iti.GetProperty(prop);
//                if (pi != null)
//                {
//                    open(tagPair, pi.Name, (htmlClassForReport) htmlClassForReport.member);

//                    AppendPair(pi.Name, target.imbGetPropertySafe(pi, "").toStringSafe(), tagItem, classItem.ToString(),
//                        classValue.ToString(), false, htmlTagName.none, true);

//                    close();
//                }
//                else
//                {
//                    AppendPair(prop, "not declared in: " + iti.Name, tagItem, classItem.ToString(),
//                        classValue.ToString(), false,
//                        htmlTagName.p, true);
//                }
//            }
//            close();
//        }

//        public void AppendLine(String content, htmlTagName tag, htmlClassForReport htmlClass, htmlIdForReport htmlId,
//            Boolean isHtmlContent = false)
//        {
//            tag = reportOutputFormatTools.checkForDefaultTag(tag, reportOutputRoles.appendLine);
//            AppendLine(content, tag, (string) htmlClass.ToString(), htmlId.ToString(), isHtmlContent);
//        }

//        /// <summary>
//        /// Dodaje liniju, ako postoji nesto. Ako nema nista onda je preskace
//        /// </summary>
//        /// <param name="content"></param>
//        public void AppendLine(String content, htmlTagName tag = htmlTagName.none, String htmlClass = "",
//            String htmlId = "", Boolean isHtmlContent = false)
//        {
//            tag = reportOutputFormatTools.checkForDefaultTag(tag, reportOutputRoles.appendLine);
//            String output = content.WrapHtml(tag, htmlClass, htmlId, isHtmlContent);

//            _AppendLine(output);
//        }

//        /// <summary>
//        /// Dodaje naslovnu liniju (prefix) i opisni niz za prosledjenu kolekciju
//        /// </summary>
//        /// <remarks>
//        /// Komanda je radi isto što i <c>Append{T}()</c>, samo dodaje još jedan red za naslov - prosleđen u <c>prefix</c> parametru.
//        /// </remarks>
//        /// <typeparam name="T">Tip objekata u kolekciji</typeparam>
//        /// <param name="input">Kolekcija koja treba da se popise</param>
//        /// <param name="toString">Funkcija za ispis svakog unosa</param>
//        /// <param name="prefix">Naslovna linija - na pocetku prikaza kolekcije</param>
//        /// <param name="format">Formatiranje / šablon za svaki unos</param>
//        /// <example>
//        /// <para>Lista svih članova <c>methodsForConstructor</c> kolekcije - u podrazumevanom formatu.</para>
//        /// <para>Na početku liste je ubačen naslov: <c>prefix</c></para>
//        /// <code>
//        /// sb.AppendLine(methodsForConstructor, x => x.name, "Constructor methods: ");
//        /// </code>
//        /// </example>
//        /// <example>
//        /// <para>Lista svih članova <c>methodsForConstructor</c> kolekcije.</para>
//        /// <para>Dat je format za višelinijsku listu</para>
//        /// <code>
//        /// sb.AppendLine(methodsForStatic, x => x.name, "Static methods: ", "- {0}"+Environment.NewLine);
//        /// </code>
//        /// </example>
//        /// <example>
//        /// <para>Lista prvih 5 članova <c>methodsForConstructor</c> kolekcije.</para>
//        /// <para>Ispred svake linije će biti ubačen <c>prefix</c> i primenjen dati format.</para>
//        /// <code>
//        /// sb.AppendLine(methodsForStatic, x => x.name, "Static methods: ", "{0}", 5, "... and {0} items more.");
//        /// </code>
//        /// </example>
//        /// <seealso cref="Append{T}"/>
//        public void AppendLine<T>(IEnumerable<T> input, Func<T, Object> toString, String prefix = "",
//            String format = "{0}, ", Int32 limit = 0,
//            String limitSufixFormat = "... and {0} more.")
//        {
//            if (input.Count() == 0) return;
//            if (!String.IsNullOrEmpty(prefix)) AppendLine(prefix, 1);
//            Append(input, toString, "", format, limit, limitSufixFormat);
//            prevTabLevel();
//        }

//        protected void _withAttributes(String nodeName, String attributes, Boolean inline, Boolean compact)
//        {
//            String _ntag = "<" + nodeName + "";

//            _ntag = _ntag.Append(attributes, " ");

//            if (compact)
//            {
//                _ntag = _ntag.Append("/>", " ");
//                AppendLine(_ntag);
//            }
//            else
//            {
//                _ntag = _ntag.Append(">", " ");
//                if (inline)
//                {
//                    Append(_ntag);
//                }
//                else
//                {
//                    AppendLine(_ntag);
//                }
//                openedTags.Add(nodeName, nodeName, _ntag);
//                //openedTags.Push(nodeName);
//            }
//        }

//        public void AppendClosedTagWithAttributes(String nodeName, String attributes, Boolean inline = false)
//        {
//            _withAttributes(nodeName, attributes, inline, true);
//        }

//        public void openWithAttributes(String nodeName, String attributes, Boolean inline = false)
//        {
//            _withAttributes(nodeName, attributes, inline, false);
//        }

//        /// <summary>
//        /// Otvara tag kojem dodeljuje attribute iz parova
//        /// </summary>
//        /// <param name="nodeName"></param>
//        /// <param name="attributes"></param>
//        public void open(String nodeName, IValuePairs attributes, Boolean inline = false, Boolean compact = false)
//        {
//            String _ntag = "";
//            foreach (KeyValuePair<string, object> att in attributes)
//            {
//                String vl = att.Value.toStringSafe();
//                String key = att.Key;
//                if (!String.IsNullOrEmpty(vl))
//                {
//                    _ntag = _ntag.Append(key + "=\"" + vl + "\"", " ");
//                }
//            }
//            _withAttributes(nodeName, _ntag, inline, compact);
//        }

//        //public void open(String tag)
//        //{
//        //    AppendLine(tag.openTag("", ""));
//        //    openTags.Push(tag);
//        //}

//        //public void open(String tag, String htmlClass, String htmlId)
//        //{
//        //    AppendLine(tag.openTag(htmlClass, htmlId));

//        //    openedTags.Push(tag);
//        //}

//        public void open(String nodeName, Object item, params String[] propertiesForAttributes)
//        {
//            open(nodeName, item, propertiesForAttributes.ToList());
//        }

//        public void open(String nodeName, Object item, List<String> propertiesForAttributes, Boolean compact = false)
//        {
//            String attLn = "";
//            String vls = "";
//            if (propertiesForAttributes.Any())
//            {
//                foreach (String kp in propertiesForAttributes)
//                {
//                    vls = item.imbPropertyToString(kp, "");
//                    //attribs.Add(kp.Name);
//                    //item.imbGetPropertySafe(kp)

//                    if (!imbSciStringExtensions.isNullOrEmptyString(vls))
//                    {
//                        attLn = attLn.Append(kp + "=\"" + vls + "\"");
//                    }
//                }
//                //attLn = attLn.Append(kp.Name + "=\"" + vls + "\"");

//                // open(nodeName, attLn, true, true);
//            }
//            _withAttributes(nodeName, attLn, false, compact);
//        }

//        public void open(htmlTagName tag, String htmlClass, String htmlId)
//        {
//            tag = reportOutputFormatTools.checkForDefaultTag(tag, reportOutputRoles.container);
//            AppendLine(imbStringReporting.openTag((string) tag.ToString(), htmlClass, htmlId));
//            openedTags.Push(tag.ToString());
//        }

//        public void open(htmlTagName tag, String htmlId, htmlClassForReport htmlClass = htmlClassForReport.note)
//        {
//            tag = reportOutputFormatTools.checkForDefaultTag(tag, reportOutputRoles.container);
//            AppendLine(imbStringReporting.openTag((string) tag.ToString(), (object) htmlClass, htmlId));
//            openedTags.Push(tag.ToString());
//        }

//        public void open(htmlTagName tag, htmlClassForReport htmlClass,
//            htmlIdForReport htmlId = htmlIdForReport.none)
//        {
//            tag = reportOutputFormatTools.checkForDefaultTag(tag, reportOutputRoles.container);
//            AppendLine(imbStringReporting.openTag(tag, htmlClass, htmlId));
//            openedTags.Push(tag.ToString());
//        }

//        public void open(htmlTagName tag)
//        {
//            AppendLine(imbStringReporting.openTag(tag, "", ""));
//            openedTags.Push(tag.ToString());
//        }

//        ///// <summary>
//        ///// Dodaje HTML zatvaranje taga -- zatvara poslednji koji je otvoren
//        ///// </summary>
//        ///// <remarks>
//        ///// Ako je prosledjeni tag none onda zatvara poslednji tag koji je otvoren.
//        ///// </remarks>
//        ///// <param name="tag"></param>
//        //public override void close(String tag = "none")
//        //{
//        //    //tag = tag.checkForDefaultTag(reportOutputRoles.container);

//        //    if (tag == "none")
//        //    {
//        //        if (openedTags.Any())
//        //        {
//        //            tag = openedTags.Pop();
//        //        }
//        //        else
//        //        {
//        //            tag = "error";
//        //        }
//        //    }

//        //    if (tag != "none")
//        //    {
//        //        AppendLine(tag.closeTag());
//        //    }
//        //}

//        ///// <summary>
//        ///// Zatvara sve tagove koji su trenutno otvoreni
//        ///// </summary>
//        //public override void closeAll()
//        //{
//        //    String tag = "none"; // htmlTagName.none;

//        //    Int32 c = openedTags.Count;

//        //    for (int i = 0; i < c; i++)
//        //    {
//        //        openedTags.Pop();

//        //        tag =
//        //        AppendLine(tag.closeTag());
//        //    }
//        //}

//        /// <summary>
//        /// Dodaje instrukciju preloma. Podrazumevano se odnosi na Breakline
//        /// </summary>
//        /// <param name="_flags"></param>
//        public virtual void AppendBreak(params appendBreakFlag[] _flags)
//        {
//            foreach (appendBreakFlag fl in _flags)
//            {
//                switch (fl)
//                {
//                    case appendBreakFlag.breakLine:
//                        Append(newLineString);
//                        break;
//                    case appendBreakFlag.breakPage:
//                        closeAll();
//                        break;
//                    case appendBreakFlag.breakSection:
//                        close();
//                        break;
//                }
//            }
//        }

//        /// <summary>
//        /// Renders HTML table
//        /// </summary>
//        /// <param name="datatable"></param>
//        /// <param name="styleClass"></param>
//        /// <param name="idPrefix"></param>
//        /// <param name="fields"></param>
//        /// <param name="_limitRows"></param>
//        /// <param name="_flags"></param>
//        public virtual void AppendTable(IEnumerable datatable, String styleClass = "", String idPrefix = "",
//            IEnumerable<String> fields = null, Int32 _limitRows = -1,
//            params appendTableFlag[] _flags)
//        {
//            if (String.IsNullOrEmpty(idPrefix)) idPrefix = "Table";
//            List<String> fieldList = new List<string>();
//            if (fields != null)
//            {
//                fieldList.AddRange(fields.ToList());
//            }

//            open(htmlTagName.table);

//            open(htmlTagName.tr);

//            foreach (String field in fieldList)
//            {
//                open(htmlTagName.th);
//                Append(field);
//                close();
//            }

//            close();

//            foreach (Object datarow in datatable)
//            {
//                open(htmlTagName.tr);
//                foreach (String field in fieldList)
//                {
//                    open(htmlTagName.td);
//                    try
//                    {
//                        Append(datarow.imbGetPropertySafe(field, "").ToString());
//                    }
//                    catch (Exception ex)
//                    {
//                        Append("-error-");
//                    }
//                    close();
//                }
//                close();
//            }

//            close();

//            // throw new NotImplementedException();
//            //DataTable table = datatable.createDataTable(idPrefix, null);

////            AppendTable(table, styleClass, idPrefix, headingMetaData, _limitRows, _flags);
//        }

//        /// <summary>
//        /// Dodaje tabelu u izvestaj
//        /// </summary>
//        /// <param name="datatable"></param>
//        /// <param name="styleClass">osnovni text builder koristi ovo kao separator, XML/HTML generatori ovo koriste kao ime css klase ili stila cele tabele</param>
//        /// <param name="idPrefix">osnovni text builder ovo koristi kao insert za title, a XML/HTML generatori ovo dodaju u ID attribut</param>
//        /// <param name="headingMetaData"></param>
//        /// <param name="_flags"></param>
//        public virtual void AppendTable(DataTable datatable, String styleClass = "", String idPrefix = "",
//            IValuePairs headingMetaData = null, Int32 _limitRows = -1,
//            params appendTableFlag[] flags)
//        {
//            if (String.IsNullOrEmpty(styleClass)) styleClass = ", ";

//            if (datatable == null) return;

//            if (flags.Contains(appendTableFlag.addTableNameAsTitle)) AppendLine(datatable.TableName);

//            if (flags.Contains(appendTableFlag.insertHeader))
//            {
//                String columnHeader = "";
//                foreach (DataColumn cl in datatable.Columns)
//                {
//                    columnHeader = null;

//                    if (headingMetaData == null)
//                    {
//                        columnHeader = cl.ColumnName;
//                    }
//                    else
//                    {
//                        if (headingMetaData.ContainsKey(cl.ColumnName))
//                        {
//                            columnHeader = headingMetaData[cl.ColumnName].toStringSafe();
//                        }
//                        else
//                        {
//                            columnHeader = null;
//                        }
//                    }

//                    if (flags.Contains(appendTableFlag.headerToUpper))
//                    {
//                        columnHeader = columnHeader.ToUpper();
//                    }

//                    if (columnHeader != null)
//                    {
//                        Append(columnHeader, styleClass);
//                    }
//                }
//            }

//            AppendBreak();

//            foreach (DataRow rw in datatable.Rows)
//            {
//                foreach (DataColumn cl in datatable.Columns)
//                {
//                    String vl = null;
//                    if (headingMetaData == null)
//                    {
//                        vl = rw[cl, DataRowVersion.Current].toStringSafe();
//                    }
//                    else
//                    {
//                        if (headingMetaData.ContainsKey(cl.ColumnName))
//                        {
//                            vl = rw[cl, DataRowVersion.Current].toStringSafe();
//                        }
//                        else
//                        {
//                            vl = null;
//                        }
//                    }

//                    if (vl != null)
//                    {
//                        Append(vl, styleClass);
//                    }
//                }
//            }
//        }

//        public void AppendLink(String linkSrc, String linkTitle, htmlClassForReport htmlClass, String prefixTitle = "",
//            htmlTagName prefixTag = htmlTagName.none, String uriScheme = @"file:///")
//        {
//            AppendLink(linkSrc, linkTitle, (string) htmlClass.ToString(), prefixTitle, prefixTag, uriScheme);
//        }

//        public void AppendLink(String linkSrc, String linkTitle = "", String htmlClass = "", String prefixTitle = "",
//            htmlTagName prefixTag = htmlTagName.none, String uriScheme = @"file:///")
//        {
//            if (String.IsNullOrEmpty(linkTitle)) linkTitle = linkSrc;

//            if (!String.IsNullOrEmpty(prefixTitle))
//            {
//                Append(prefixTag, prefixTitle);
//            }

//            String _linkTag = "<a href=\"" + uriScheme + linkSrc + "\" ";
//            if (!String.IsNullOrEmpty(htmlClass)) _linkTag += " class=\"" + htmlClass + "\" ";
//            _linkTag += ">" + linkTitle + "</a>";
//            _Append(_linkTag);
//        }

//        /// <summary>
//        /// Dodaje niz tagova
//        /// </summary>
//        /// <param name="tag"></param>
//        /// <param name="items"></param>
//        public void Append(htmlTagName tag, params Object[] items)
//        {
//            tag = reportOutputFormatTools.checkForDefaultTag(tag, reportOutputRoles.appendInline);
//            htmlClassForReport currentClass = htmlClassForReport.none;
//            htmlTagName currentTag = tag;
//            foreach (Object it in items)
//            {
//                if (it is String)
//                {
//                    Append(it as string, "", currentTag, imbStringExtensions.ToStringEnumSmart(currentClass));
//                }
//                if (it is htmlTagName)
//                {
//                    currentTag = (htmlTagName) it;
//                }
//                if (it is htmlClassForReport)
//                {
//                    currentClass = (htmlClassForReport) it;
//                }
//            }
//        }

//        /// <summary>
//        /// U trenutnu liniju ubacuje string uokviren u html tag sa klasom i id-om
//        /// </summary>
//        /// <param name="isb"></param>
//        /// <param name="tag"></param>
//        /// <param name="_class"></param>
//        /// <param name="_id"></param>
//        public void Append(String isb, htmlTagName tag, htmlClassForReport _class = htmlClassForReport.none,
//            htmlIdForReport _id = htmlIdForReport.none)
//        {
//            tag = reportOutputFormatTools.checkForDefaultTag(tag, reportOutputRoles.appendInline);
//            //String input = isb.WrapHtml(tag, _class, _id);
//            _Append(input);
//        }

//        /// <summary>
//        /// Dodaje prosledjen tekst na kraj trenutne linije
//        /// </summary>
//        /// <param name="isb">Tekst koji treba da se ubaci</param>
//        public void Append(String isb, String separator = "", htmlTagName tag = htmlTagName.none, String htmlClass = "")
//        {
//            tag = reportOutputFormatTools.checkForDefaultTag(tag, reportOutputRoles.appendInline);
//            //if (isb == null)
//            //{
//            //    isb = "";
//            //}
//            if (sb.Length > 0)
//            {
//                _Append(separator);
//            }
//            if (!String.IsNullOrEmpty(isb))
//            {
//                //String input = isb.WrapHtml(tag, htmlClass);
//                //_Append(input);
//            }
//        }

//        /// <summary>
//        /// Dodaje kompletan tekst postojeceg imbStringBuilder objekta
//        /// </summary>
//        /// <remarks>
//        /// Ako je <c>isb</c> null - onda se nista ne menja
//        /// </remarks>
//        /// <param name="isb">Postojeci imbStringBuilder objekat.</param>
//        public void Append(ITextRender isb)
//        {
//            if (isb == null) return;
//            sb.Append((object) isb.ToString());
//        }

//        /// <summary>
//        /// Dodaje opisnu liniju za ceo niz prosledjenih objekata
//        /// </summary>
//        /// <typeparam name="T">Tip objekata u kolekciji</typeparam>
//        /// <param name="input">Kolekcija koja treba da se popise</param>
//        /// <param name="toString">Funkcija za ispis svakog unosa</param>
//        /// <param name="prefix">Prefix koji se dodaje pre svake linije</param>
//        /// <param name="format">Formatiranje / šablon za svaki unos</param>
//        /// <param name="limit">Opisace kolekciju do n-tog clana a onda dodaje limitSufix i prekida niz. Ako je 0 onda je iskljuceno limitiranje</param>
//        /// <param name="limitSufixFormat">Format limitSufix-a koji se dodaje na kraj unosa ako je dostignut limit</param>
//        /// <example>
//        /// <para>Lista svih članova <c>methodsForConstructor</c> kolekcije - u podrazumevanom formatu.</para>
//        /// <code>
//        /// sb.Append(methodsForConstructor, x => x.name, "");
//        /// </code>
//        /// </example>
//        /// <example>
//        /// <para>Lista svih članova <c>methodsForConstructor</c> kolekcije.</para>
//        /// <para>Dat je format za višelinijsku listu</para>
//        /// <code>
//        /// sb.Append(methodsForStatic, x => x.name, "", "- {0}"+Environment.NewLine);
//        /// </code>
//        /// </example>
//        /// <example>
//        /// <para>Lista prvih 5 članova <c>methodsForConstructor</c> kolekcije.</para>
//        /// <code>
//        /// sb.Append(methodsForStatic, x => x.name, "", "{0}", 5, "... and {0} items more.");
//        /// </code>
//        /// </example>
//        public void Append<T>(IEnumerable<T> input, Func<T, Object> toString, String prefix = "",
//            String format = "{0}, ", Int32 limit = 0, String limitSufixFormat = "... and {0} more.")
//        {
//            if (input.Count() == 0) return;
//            String ln = "";
//            Int32 c = 0;
//            if (limit < 1) limit = input.Count();
//            foreach (T obj in input)
//            {
//                if (!String.IsNullOrEmpty(prefix))
//                {
//                    ln += prefix;
//                }
//                ln += String.Format(format, toString(obj).ToString());
//                c++;
//                if (c > limit)
//                {
//                    ln += String.Format(limitSufixFormat, input.Count() - limit);
//                    break;
//                }
//            }
//            AppendLine(ln);
//        }

//        public FileInfo savePage(string name, reportOutputFormatName format = reportOutputFormatName.none)
//        {
//            throw new NotImplementedException();
//        }

//        public object addDocument(string name, bool scopeToNew = true, getWritableFileMode mode = getWritableFileMode.autoRenameExistingOnOtherDate, reportOutputFormatName format = reportOutputFormatName.none)
//        {
//            throw new NotImplementedException();
//        }

//        public object addPage(string name, bool scopeToNew = true, getWritableFileMode mode = getWritableFileMode.autoRenameThis, reportOutputFormatName format = reportOutputFormatName.none)
//        {
//            throw new NotImplementedException();
//        }

//        public override void AppendImage(string imageSrc, string imageAltText, string imageRef)
//        {
//            throw new NotImplementedException();
//        }

//        public override void AppendMath(string mathFormula, string mathFormat = "asciimath")
//        {
//            throw new NotImplementedException();
//        }

//        public override void AppendLabel(string content, bool isBreakLine = true, object comp_style = null)
//        {
//            throw new NotImplementedException();
//        }

//        public override void AppendPanel(string content, string comp_heading = "", string comp_description = "", object comp_style = null)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}