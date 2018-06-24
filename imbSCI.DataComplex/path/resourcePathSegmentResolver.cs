// --------------------------------------------------------------------------------------------------------------------
// <copyright file="resourcePathSegmentResolver.cs" company="imbVeles" >
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
namespace imbSCI.DataComplex.path
{
    #region imbVeles using

    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Data.interfaces;
    using imbSCI.DataComplex.tree;
    using System;
    using System.Collections;
    using System.Linq;

    //using aceCommonTypes.extensions;

    #endregion imbVeles using

    public static class resourcePathSegmentResolver
    {
        internal static pathResolverResult resolvePathSegments(this object source, string path,
                                                                pathResolveFlag _flagList)
        {
            string _path = path;

            //  pathResolveFlags flags = _flagList;

            pathSegments pathSeg = new pathSegments(_path, _flagList);

            pathResolverResult output = resolvePathSegments(source, pathSeg, _flagList);

            return output;
        }

        /// <summary>
        /// Izvrsava kolekciju path segmenata nad prosledjenim objektom
        /// </summary>
        /// <param name="source"></param>
        /// <param name="segments"></param>
        /// <param name="_flagList"></param>
        /// <returns></returns>
        internal static pathResolverResult resolvePathSegments(this object source, pathSegments segments,
                                                               pathResolveFlag _flagList)
        {
            pathResolveFlag flags = _flagList;

            pathResolverResult output = new pathResolverResult();

            output.parent = source as IObjectWithPath;

            output.segments = segments;
            output.path = segments.path;

            object result = source;
            object lastResult = source;

            output.missing = new pathSegments();

            pathSegment ps = segments.First();
            int i = 0;
            do
            {
                ps = segments[i];
                result = result.resolvePathSegment(ps, flags);
                if (result == null)
                {
                    output.missing = segments.getSegmentsAfterThis(ps, true);
                    break;
                }
                else
                {
                    lastResult = result;
                }
                i++;
            } while (i < segments.Count);

            if (lastResult != null)
            {
                output.nodeFound.Add(lastResult);
            }

            //foreach (pathSegment ps in segments)
            //{
            //    if (result != null)
            //    {
            //        lastResult = result;

            //    } else
            //    {
            //        output.missing = segments.getSegmentsAfterThis(ps, true);

            //        break;
            //    }
            //}*/
            //if (result != null)
            //{
            //} else if (lastResult != null)
            //{
            //    output.nodeFound.Add(lastResult);
            //}
            return output;
        }

        /// <summary>
        /// 2014> resava kompleksne putanje
        /// </summary>
        /// <param name="source">Izvorni objekat. Moze biti imbTypeInfo - onda ce uvek vracati imbPropertyInfo</param>
        /// <param name="pathSegment">Putanja ili deo putanje koji treba da se resi</param>
        /// <param name="returnPropertyInfo">Vraca imbPropertyInfo umesto vrednosti - vazi za> @ $ . : , </param>
        /// <returns></returns>
        internal static object resolvePathSegment(this object source, pathSegment segment, pathResolveFlag flagList)
        {
            object output = null;

            //imbPropertyInfo iPI = null;

            //imbTypeInfo iTI = null;

            bool returnPropertyInfo = flagList.HasFlag(pathResolveFlag.returnPropertyInfo);

            //if (source is Type)
            //{
            //    Type st = source as Type;

            //    source = st.getTypology();
            //}
            //if (source is imbTypeInfo)
            //{
            //    iTI = source as imbTypeInfo;
            //}
            //else if (source is imbPropertyInfo)
            //{
            //    iPI = source as imbPropertyInfo;
            //    iTI = iPI.propertyTypeInfo;
            //}
            //else
            //{
            //    iTI = source.getTypology();
            //}

            //if (iTI == null)
            //{
            //}

            //imbProjectResource resource = null;

            //if (source is imbProjectResource)
            //{
            //    resource = source as imbProjectResource;
            //}

            switch (segment.prefix)
            {
                case imbProjectResourceBase.prefix_PROPERTYINFO:
                    returnPropertyInfo = true;
                    break;

                case imbProjectResourceBase.prefix_PROPERTY_OF_GENERIC:
                    //returnPropertyInfo = true;

                    //var gen = iTI.deepGenericSubTypeSearch(true);
                    //if (gen.Any())
                    //{
                    //    iTI = gen.First().getTypology();
                    //}

                    break;
            }

            if (!segment.isPrefixSupported)
            {
                throw new ArgumentOutOfRangeException("Path segment prefix not supported !!! [" + segment.prefix +
                                          "] - returning source object");
                return source;
            }

            switch (segment.prefix)
            {
                default:

                    break;

                case imbProjectResourceBase.prefix_CHILD_PATH:
                    //if (resource != null)
                    //{
                    //    return resource.children.First(x => x.title == segment.needle);
                    //}
                    //else
                    //{
                    //    if (source is IImbCollections)
                    //    {
                    //        // IImbCollections _col = source as IImbCollections;
                    //        output = executeCollectionQuery(source, segment.needle);
                    //    }
                    //}
                    break;

                case imbProjectResourceBase.prefix_TYPEBYNAME:

                    //if (source == null)
                    //{
                    //    //output = new settingsEntriesForObject(); //imbTypology.getTypology(segment.needle);
                    //}
                    //else
                    //{
                    //    if (string.IsNullOrEmpty(segment.needle))
                    //    {
                    //        output = source.GetType();
                    //    }
                    //    else
                    //    {
                    //        Type t = iTI.type.GetNestedType(segment.needle);
                    //        if (t != null) output = t.getTypology();
                    //    }
                    //}
                    break;

                case imbProjectResourceBase.prefix_OPERATIONENUM:

                    //#region OPERATION ENUMERATION

                    //if (source == null)
                    //{
                    //    output = imbCoreManager.enumerations.getElementSafe(segment.needle);
                    //    //   if (output != null) output = output.getTypology();
                    //}
                    //else
                    //{
                    //    if (string.IsNullOrEmpty(segment.needle))
                    //    {
                    //        if (source is imbEnumMemberInfo)
                    //        {
                    //            imbEnumMemberInfo iEM = source as imbEnumMemberInfo;
                    //            iEM.executeOperation(null, null, null);
                    //            output = iEM;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (source is imbTypeInfo)
                    //        {
                    //            if (iTI.isEnum)
                    //            {
                    //                imbEnumMemberInfo iEM = iTI.findEnumerationMember(segment.needle);
                    //                if (iEM != null)
                    //                {
                    //                    iEM.executeOperation(null, null, null);
                    //                }
                    //                output = iEM;
                    //            }
                    //            else
                    //            {
                    //                Type t = iTI.type.GetNestedType(segment.needle);
                    //                if (t != null) output = t.getTypology();
                    //            }
                    //        }
                    //        else
                    //        {
                    //            imbEnumMemberInfo iEM = null;
                    //            foreach (var et in iTI.operationEnumTypes)
                    //            {
                    //                foreach (var etm in et.enumMembers)
                    //                {
                    //                    if (etm.name == segment.needle)
                    //                    {
                    //                        iEM = etm;
                    //                        break;
                    //                    }
                    //                }
                    //                if (iEM != null)
                    //                {
                    //                    break;
                    //                }
                    //            }
                    //            if (iEM != null) iEM.executeOperation(source, null, null);
                    //            output = iEM;
                    //        }
                    //    }
                    //}

                    //#endregion

                    break;

                case imbProjectResourceBase.prefix_COLLECTION_INDEX_ACCESS:
                    output = executeCollectionQuery(source, segment.needle);
                    break;

                case imbProjectResourceBase.prefix_COLLECTION_QUERY:
                    output = executeCollectionQuery(source, segment.needle);
                    break;

                case imbProjectResourceBase.prefix_NOPARENT_PATH:
                    output = source;
                    break;

                case imbProjectResourceBase.prefix_GLOBAL_RESOURCE:

                    // output = globalResources.resolveGlobalResourcePath(segment.needle);
                    break;

                case imbProjectResourceBase.prefix_STRING_LIST:
                    // output = segment.needle.convertPresetListToTree(segment.needle, "big05b");
                    break;

                case imbProjectResourceBase.prefix_LINKED_PROPERTY_PATH:
                    //if (resource != null)
                    //{
                    //    resource.imbResourceLib.finalDeploy();
                    //    iPI = iTI.linkedResource.imbFirstSafe(x => (x.propertyRealName == segment.needle));
                    //    if (returnPropertyInfo)
                    //    {
                    //        output = iPI;
                    //    }
                    //    else
                    //    {
                    //        if (iPI != null) output = source.imbGetPropertySafe(iPI, null);
                    //    }
                    //}
                    //else
                    //{
                    //    throw new aceGeneralException(
                    //        "Prefix for linked property path is supported only for imbProjectResource objects! This object is: " +
                    //        source.getTypeSignature());
                    //}

                    break;

                case imbProjectResourceBase.prefix_PROJECT_PATH:
                    //  output = imbCoreManager.managerInstance.__myProjectObject;
                    break;

                case imbProjectResourceBase.prefix_INTEGRATED_PATH:

                    //#region INTEGRATED resource $

                    //var props = iTI.getPropertyInfoByRelationTypes(resourceRelationTypes.integratedResource,
                    //                                               resourceRelationTypes.integratedEntityCollection);
                    //if (props.Any())
                    //{
                    //    iPI = props.imbFirstSafe(x => x.propertyRealName == segment.needle);

                    //    if (returnPropertyInfo)
                    //    {
                    //        output = iPI;
                    //    }
                    //    else
                    //    {
                    //        if (iPI == null)
                    //        {
                    //            // nije pronadjen property -- pokusati preko object name-a
                    //            var _values = source.getPropertyValues(props);
                    //            foreach (var r in _values)
                    //            {
                    //                if (r.Value is IObjectWithName)
                    //                {
                    //                    IObjectWithName r_IObjectWithName = (IObjectWithName) r.Value;
                    //                    if (segment.needle.compareStrings(r_IObjectWithName.name,
                    //                                                      stringMatchPolicy.similar))
                    //                    {
                    //                        output = r_IObjectWithName;
                    //                        break;
                    //                    }
                    //                }
                    //                else if (r.Key.compareStrings(segment.needle, stringMatchPolicy.similar))
                    //                {
                    //                    output = r.Value;
                    //                    break;
                    //                }
                    //            }
                    //        }
                    //        else
                    //        {
                    //            output = source.imbGetPropertySafe(iPI);
                    //        }
                    //    }
                    //}

                    //#endregion

                    break;

                case imbProjectResourceBase.prefix_PROPERTYINFO:
                case imbProjectResourceBase.prefix_PROPERTY_OF_GENERIC:
                case imbProjectResourceBase.prefix_PROPERTY_PATH:
                    //if (iTI != null)
                    //{
                    //    if (iTI.isEnum)
                    //    {
                    //        output = iTI.findEnumerationMember(segment.needle);
                    //    }
                    //    else
                    //    {
                    //        iPI = iTI.searchForProperty(segment.needle);
                    //        if (iPI != null)
                    //        {
                    //            if (returnPropertyInfo)
                    //            {
                    //                output = iPI;
                    //            }
                    //            else
                    //            {
                    //                output = source.imbGetPropertySafe(iPI, null);
                    //            }
                    //        }
                    //    }
                    //}
                    break;
            }
            return output;
        }

        /// <summary>
        /// 2014c > izvrsava deo putanje koji se odnosi na upit nad kolekcijom
        /// </summary>
        /// <param name="source"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        internal static object executeCollectionQuery(object source, string query)
        {
            string customKey = "";
            string fullQuery = query;

            if (string.IsNullOrEmpty(query)) return source;

            if (query.Contains('='))
            {
                var qr = query.Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                customKey = qr.First();
                query = qr.Last();
            }

            object queryValue = null;
            if (query.Contains('"'))
            {
                // string key
                queryValue = query.Trim('"');
            }
            else
            {
                // queryValue = imbTypeExtensions.imbConvertValueSafeTyped<int>(query);
            }

            if (string.IsNullOrEmpty(customKey))
            {
                if (source is imbTreeNode)
                {
                    var tn = source as imbTreeNode;
                    return tn[query];
                }
                //else if (source is ICollectionWithKeyProperty)
                //{
                //    ICollectionWithKeyProperty source_ICollectionWithKeyProperty = (ICollectionWithKeyProperty) source;
                //    return source_ICollectionWithKeyProperty[query];
                //}
                else if (source is IList)
                {
                    var slist = source as IList;
                    return slist[imbTypeExtensions.imbConvertValueSafeTyped<int>(queryValue)];
                }
                else if (source is IDictionary)
                {
                    var sdict = source as IDictionary;
                    return sdict[queryValue];
                }
                else
                {
                }
            }
            else
            {
                //if (source is IRelationEnabledCollection)
                //{
                //    IRelationEnabledCollection rel = source as IRelationEnabledCollection;
                //    return rel.selectItem(fullQuery, 0);
                //}
                //else if (source is IImbCollections)
                //{
                //    var col = source as IImbCollections;
                //    return col.selectItem(fullQuery);
                //}
            }

            return source;
        }
    }
}