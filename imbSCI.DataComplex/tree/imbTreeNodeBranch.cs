// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbTreeNodeBranch.cs" company="imbVeles" >
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

namespace imbSCI.DataComplex.tree
{
    using imbSCI.Core.attributes;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.reporting.render;
    using imbSCI.DataComplex.path;

    #region imbVeles using

    using System;

    #endregion imbVeles using

    /// <summary>
    /// Grana koja ne poseduje podatak
    /// </summary>
    [imb(imbAttributeName.xmlNodeTypeName, "branch")]
    [Serializable]
    public class imbTreeNodeBranch : imbTreeNode
    {
        public imbTreeNodeBranch(String nodeBaseName)
        {
            _nameBase = nodeBaseName;
            _init();
        }

        /// <summary>
        /// Pravi novi treenode koji koristi ime tipa
        /// </summary>
        public imbTreeNodeBranch()
        {
            _nameBase = GetType().Name;
            _init();
        }

        public void Add(imbTreeNode child)
        {
            child.parent = this as imbTreeNodeBranch;

            base.Add(child.name, child);
        }

        /// <summary>
        /// Dodaje novi "list" -- krajnji element u strukturi koji obicno nosi i vrednost -- pravi strukturu koja je neophodna da bi ova grana profukncionisala
        /// </summary>
        /// <param name="leafNameOfPath">Putanja se prosledjuje u sourcePath</param>
        /// <param name="value"></param>
        /// <param name="report"></param>
        /// <returns></returns>
        public imbTreeNodeLeaf AddNewLeaf(String leafNameOfPath, Object value, ITextRender report = null,
                                          String __sourceContent = "")
        {
            String __sourcePath = leafNameOfPath;

            pathSegments psq = new pathSegments();
            String branchName = "";

            psq.deployPath(leafNameOfPath, pathResolveFlag.autorenameCollectionIndexer);
            String leafName = psq.lastSegment.needle; //leafNameOfPath;

            if (psq.Count > 1)
            {
                psq.Remove(psq.lastSegment);
                branchName = psq.ToString();
            }

            imbTreeNodeBranch head = this;

            if (!String.IsNullOrEmpty(branchName))
            {
                head = AddNewBranch(branchName, report);
            }

            if (head == null)
            {
                head = this;
            }

            if (report != null)
            {
                //report.AppendLine("Adding new leaf: " + leafName + "  inside: " + head.path);
            }

            imbTreeNodeLeaf newLeaf = new imbTreeNodeLeaf(leafName, value);
            newLeaf.sourcePath = __sourcePath;
            newLeaf.sourceContent = __sourceContent;
            head.Add(newLeaf);

            return newLeaf;
        }

        /// <summary>
        /// Pravi novu granu - ili strukturu pod grana -- ako je prosledjena putanja umesto obicnog imena
        /// </summary>
        /// <param name="branchName"></param>
        /// <returns></returns>
        public imbTreeNodeBranch AddNewBranch(String branchNameOrPath, ITextRender report = null)
        {
            sourcePath = branchNameOrPath;
            pathResolverResult query = this.resolvePath(branchNameOrPath, pathResolveFlag.autorenameCollectionIndexer);
            imbTreeNodeBranch head = null;

            switch (query.type)
            {
                case pathResolverResultType.foundOne:
                case pathResolverResultType.foundMany:
                    //head = query.nodeFound. as imbTreeNodeBranch;
                    head = query.nodeFound.imbFirstSafe() as imbTreeNodeBranch;
                    /*
					Exception ex = new aceGeneralException("Branch is already found at: " + branchNameOrPath);

					 var isb = new imbStringBuilder(0);
					 isb.AppendLine("Can't add new branch on place where the one already exists error");
					 isb.AppendPair("Target is: ", this.toStringSafe());
					 devNoteManager.note(this, ex, isb.ToString(), "Can't add new branch on place where the one already exists", devNoteType.unknown);
	*/
                    break;

                default:
                case pathResolverResultType.nothingFound:

                case pathResolverResultType.folderFoundButItemMissing:
                case pathResolverResultType.folderFoundButFoldersMissing:
                    imbTreeNode nd = query.nodeFound.imbFirstSafe() as imbTreeNode;
                    if (nd is imbTreeNodeLeaf)
                    {
                        head = nd.parent;
                        imbTreeNodeBranch newBranch = new imbTreeNodeBranch(nd.name);
                        newBranch.learnFrom(nd);

                        head.Remove(nd.keyHash);
                        head.Add(newBranch);
                        head = newBranch;

                        if (report != null)
                        {
                            //report.AppendPair("replacing existing Leaf node with branch one", head.name);
                        }
                    }
                    else
                    {
                        head = nd as imbTreeNodeBranch;
                    }

                    if (head == null)
                    {
                        head = this;
                    }
                    foreach (pathSegment ps in query.missing)
                    {
                        imbTreeNodeBranch newBranch = new imbTreeNodeBranch(ps.needle);
                        head.Add(newBranch);

                        if (report != null)
                        {
                            //report.AppendPair("Add new node to [" + head.path + "] : ", newBranch.name);
                        }
                        head = newBranch;
                    }
                    break;
            }

            return head;
        }

        ///// <summary>
        ///// Objekat koji je dodeljen nodeu
        ///// </summary>
        //public override object value
        //{
        //    get { return null; }
        //    set
        //    {
        //        Exception ex = new aceGeneralException("This is imbTreeNodeBranch - can-t have object set ");

        //        var isb = new imbStringBuilder(0);
        //        isb.AppendLine("Cant set value for branch");
        //        isb.AppendPair("Target is: ", this.toStringSafe());
        //        isb.AppendPair("Value is: ", value.toStringSafe());
        //        devNoteManager.note(this, ex, isb.ToString(), "Forbiden application", devNoteType.notIntendentUsage);

        //    }
        //}
    }
}