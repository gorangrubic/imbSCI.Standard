// --------------------------------------------------------------------------------------------------------------------
// <copyright file="bagOfWords.cs" company="imbVeles" >
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
namespace imbSCI.DataComplex.special
{
    using imbSCI.Core.enums;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.text;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    /// <summary>
    /// Words are stored in First capital letter variation
    /// </summary>
    public class bagOfWords : instanceCountCollection<string>, IComparable, ICloneable
    {
        /// <summary>
        /// Calculates the specified second.
        /// </summary>
        /// <param name="second">The second.</param>
        /// <param name="op">The op.</param>
        public void calculate(bagOfWords second, operation op)
        {
            switch (op)
            {
                case operation.assign:
                    Clear();
                    AddInstanceRange(second);
                    break;

                case operation.plus:
                    AddInstanceRange(second);
                    break;

                case operation.minus:
                    Remove(second);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Calculates the specified second.
        /// </summary>
        /// <param name="second">The second.</param>
        /// <param name="op">The op.</param>
        public void calculate(object second, operation op)
        {
            if (second is bagOfWords)
            {
                calculate(second as bagOfWords, op);
                return;
            }

            if (second is string)
            {
                calculate(new bagOfWords(second.toStringSafe()), op);
                return;
            }
        }

        /// <summary>
        /// Calculates the specified second.
        /// </summary>
        /// <param name="second">The second.</param>
        /// <param name="op">The op.</param>
        public void calculate(string second, operation op)
        {
            calculate(new bagOfWords(second), op);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="bagOfWords"/> class.
        /// </summary>
        public bagOfWords()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="bagOfWords"/> class.
        /// </summary>
        /// <param name="input">The input.</param>
        public bagOfWords(params string[] input)
        {
            List<string> inp = collectionExtensions.getFlatList<string>(input);

            inp.ForEach(x => Add(x));
        }

        /// <summary>
        /// Gets or sets the count/score <see cref="Int32"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="Int32"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public int this[string key]
        {
            get
            {
                key = key.ToLower();
                return base[key];
            }

            set
            {
                key = key.ToLower();
                base[key] = value;
            }
        }

        /// <summary>
        /// Builds an property collection with limited or all entries (-1 for limitless)
        /// </summary>
        /// <param name="limit">The limit.</param>
        /// <returns></returns>
        public PropertyCollection ToPropertyCollection(int limit = 10)
        {
            PropertyCollection output = new PropertyCollection();

            List<KeyValuePair<string, int>> srt = collectionExtensions.toSortedKeyValueList<string>(items);
            if (limit > 0)
            {
                var srtt = srt.Take(limit);
                srt.Clear();
                srt.AddRange(srtt);
            }
            foreach (var pair in srt)
            {
                output.Add(pair.Key, pair.Value);
            }
            return output;
        }

        /// <summary>
        /// Adds words to banned list: the same words will be ignored in any future Add call
        /// </summary>
        /// <param name="words">The words.</param>
        public void AddBan(params string[] words)
        {
            List<string> wordList = collectionExtensions.getFlatList<string>(words);
            foreach (string wr in wordList)
            {
                bannedWords.AddUnique(wr);
            }
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public new void Add(string key, object value)
        {
            key = key.ToLower();
            if (bannedWords.Contains(key)) return;

            key = key.ToLower();
            AddInstance(key, value);
        }

        /// <summary>
        /// Reduces the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public new void Reduce(string key, int value)
        {
            List<string> itms = key.ToLower().getStringTokens();

            foreach (string item in itms)
            {
                base.Reduce(item, value);
            }
        }

        /// <summary>
        /// Removes the specified word or all words from the line.
        /// </summary>
        /// <param name="wordOrLine">The word or line.</param>
        public new void Remove(string wordOrLine)
        {
            List<string> itms = wordOrLine.ToLower().getStringTokens();

            foreach (string item in itms)
            {
                if (items.ContainsKey(item))
                {
                    base.Remove(item);
                }
            }
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public new void Add(string lineOrWord)
        {
            List<string> itms = lineOrWord.ToLower().getStringTokens();

            foreach (string item in itms)
            {
                if (bannedWords.Contains(item)) return;

                if (items.ContainsKey(item))
                {
                    items[item]++;
                }
                else
                {
                    items.Add(item, 1);
                }
            }
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public object Clone()
        {
            bagOfWords output = new bagOfWords();
            foreach (var pair in items)
            {
                output.Add(pair.Key, pair.Value);
            }
            return output;
        }

        //protected Dictionary<String, Int32> words = new Dictionary<string, int>();

        /// <summary>
        /// Words banned from the bag
        /// </summary>
        public List<string> bannedWords { get; protected set; } = new List<string>();
    }
}