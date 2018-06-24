using System;
using System.Linq;
using System.Collections.Generic;
using imbSCI.DataComplex.special;

namespace imbSCI.BibTex
{

    /// <summary>
    /// Preprocessing text from Bibtex file
    /// </summary>
    public class BibTexSourceProcessor
    {
        private Object _latex_lock = new Object();
        private translationTextTable _latex;

        /// <summary>
        /// LaTeX non-ascii characters excape translation table
        /// </summary>
        public translationTextTable latex
        {
            get
            {
                if (_latex == null)
                {
                    lock (_latex_lock)
                    {
                        if (_latex == null)
                        {
                            _latex = new translationTextTable();
                            _latex.Add("\\`{o}", "ò");
                            _latex.Add("\\'{o}", "ó");
                            _latex.Add("\\^{o}", "ô");
                            _latex.Add("\\\"{o}", "ö");
                            _latex.Add("\\H{o}", "ő");
                            _latex.Add("\\~{o}", "õ");
                            _latex.Add("\\c{c}", "ç");
                            _latex.Add("\\k{a}", "ą");
                            _latex.Add("\\l{}", "ł");
                            _latex.Add("\\={o}", "ō");
                            _latex.Add("\\b{o}", "o");
                            _latex.Add("\\.{o}", "ȯ");
                            _latex.Add("\\d{u}", "ụ");
                            _latex.Add("\\r{a}", "å");
                            _latex.Add("\\u{o}", "ŏ");
                            _latex.Add("\\v{s}", "š");
                            _latex.Add("\\t{oo}", "o͡o");
                            _latex.Add("\\o", "ø");

                            _latex.Add("\\%", "%");
                            _latex.Add("\\$", "$");
                            _latex.Add("\\{", "{");
                            _latex.Add("\\_", "_");
                            _latex.Add("\\P", "¶");
                            _latex.Add("\\ddag", "‡");
                            _latex.Add("\\textbar", "|");
                            _latex.Add("\\textgreater", ">");
                            _latex.Add("\\textendash", "–");
                            _latex.Add("\\texttrademark", "™");
                            _latex.Add("\\textexclamdown", "¡");
                            _latex.Add("\\textsuperscript{ a}", "a");
                            _latex.Add("\\pounds", "£");
                            _latex.Add("\\#", "#");
                            _latex.Add("\\&", "&");
                            _latex.Add("\\}", "}");
                            _latex.Add("\\S", "§");
                            _latex.Add("\\dag", "†");
                            _latex.Add("\\textbackslash", "\\");
                            _latex.Add("\\textless", "<");
                            _latex.Add("\\textemdash", "—");
                            _latex.Add("\\textregistered", "®");
                            _latex.Add("\\textquestiondown", "¿");
                            _latex.Add("\\textcircled{ a}", "ⓐ");
                            _latex.Add("\\copyright", "©");

                            _latex.Add("$\\backslash$", "\\");
                            _latex.Add("\\'{e}", "ë");
                            _latex.Add("\\'{i}", "ë");

                            // add here if any additional initialization code is required
                        }
                    }
                }
                return _latex;
            }
        }

    }

}