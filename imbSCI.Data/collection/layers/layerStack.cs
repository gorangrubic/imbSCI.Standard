// --------------------------------------------------------------------------------------------------------------------
// <copyright file="layerStack.cs" company="imbVeles" >
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
// Project: imbSCI.Data
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Data.collection.layers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

#pragma warning disable CS1574 // XML comment has cref attribute 'imbBindable' that could not be resolved
    /// <summary>
    /// Contains multiple <see cref="layerCollection"/>
    /// </summary>
    /// <seealso cref="aceCommonTypes.primitives.imbBindable" />
    public class layerStack : IEnumerable<layerCollection>
#pragma warning restore CS1574 // XML comment has cref attribute 'imbBindable' that could not be resolved
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="layerStack"/> class.
        /// </summary>
        public layerStack()
        {
        }

        /// <summary>
        /// Pushes the specified items, and returns the items refused (already inside - or - by other criterion)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input">The input.</param>
        /// <param name="layer_id">The layer identifier.</param>
        /// <param name="leakToLower">if set to <c>true</c> it will try to Push the items into next lower layer.</param>
        /// <returns></returns>
        public List<T> Push<T>(IEnumerable<T> input, Int32 layer_id, Boolean leakToLower = false) where T : class
        {
            List<T> output = new List<T>();

            output = this[layer_id].Push(input);

            if (leakToLower)
            {
                while (output.Any() && layer_id < Count)
                {
                    layer_id++;

                    output = this[layer_id].Push<T>(output);
                }
            }

            return output;
        }

        /// <summary>
        /// Last layer pull
        /// </summary>
        /// <value>
        /// The layer identifier.
        /// </value>
        public Int32 layer_id { get; set; } = 0;

        /// <summary>
        /// Pulls the content from the best (topmost) layer and removes taken elements.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pullLimit">The pull limit.</param>
        /// <returns></returns>
        public List<T> Pull<T>(Int32 pullLimit = -1, Boolean takeFromLower = false)
        {
            List<T> output = new List<T>();
            layer_id = 0;

            while (this[layer_id].isEmpty && layer_id < Count)
            {
                layer_id++;

                if (layer_id == Count) return output;
            }

            if (pullLimit == -1) pullLimit = this[layer_id].Count();
            if (pullLimit > this[layer_id].Count()) pullLimit = this[layer_id].Count();

            output.AddRange(this[layer_id].Pull<T>(pullLimit));

            if (takeFromLower)
            {
                while (output.Count() < pullLimit && layer_id < Count)
                {
                    layer_id++;
                    output.AddRange(this[layer_id].Pull<T>((pullLimit - output.Count())));

                    if (layer_id == Count) return output;
                }
            }

            return output;
        }

        /// <summary>
        /// Pulls all elements in the collections
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> PullAll<T>()
        {
            List<T> output = new List<T>();

            foreach (layerCollection layer in layers)
            {
                var pulled = layer.PullAll<T>();
                foreach (var p in pulled)
                {
                    if (!output.Contains(p)) output.Add(p);
                }
                //output.AddRangeUnique(layer.PullAll<T>());
            }

            return output;
        }

        /// <summary>
        /// Clears all layers from any instances
        /// </summary>
        public void Clear()
        {
            foreach (layerCollection layer in layers)
            {
                layer.Clear();
            }
            layer_id = 0;
        }

        /// <summary>
        /// Gets the count of layers
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public Int32 Count
        {
            get
            {
                return layers.Count;
            }
        }

        /// <summary>
        /// Gets the total count of all elements in the layers
        /// </summary>
        /// <value>
        /// The count all.
        /// </value>
        public Int32 CountAll
        {
            get
            {
                Int32 output = 0;

                foreach (layerCollection layer in layers)
                {
                    output += layer.Count;
                }

                return output;
            }
        }

        /// <summary>
        /// Gets the <see cref="layerCollection"/> with the specified identifier.
        /// </summary>
        /// <value>
        /// The <see cref="layerCollection"/>.
        /// </value>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public layerCollection this[Int32 id]
        {
            get
            {
                if (id >= layers.Count()) id = layers.Count() - 1;
                if (id < 0) id = 0;
                return layers[id];
            }
        }

        /// <summary>
        /// Gets the <see cref="layerCollection"/> with the specified name.
        /// </summary>
        /// <value>
        /// The <see cref="layerCollection"/>.
        /// </value>
        /// <param name="__name">The name.</param>
        /// <returns></returns>
        public layerCollection this[String __name]
        {
            get
            {
                return layers.First(x => x.name == __name);
            }
        }

        public layerCollection Surface
        {
            get
            {
                return this[0];
            }
        }

        public layerCollection Deepest
        {
            get
            {
                return this[layers.Count() - 1];
            }
        }

        /// <summary>
        /// Adds the layer - defines new layer within the stack
        /// </summary>
        /// <param name="__name">The name.</param>
        /// <param name="__description">The description.</param>
        /// <returns></returns>
        public layerCollection AddLayer(String __name, String __description)
        {
            layerCollection layer = new layerCollection(__name, __description, this, layers.Count());
            layers.Add(layer);
            return layer;
        }

        /// <summary>
        /// Where is the specified <c>item</c>, returns -1 if it is not in the layer stack
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public Int32 WhereIs(Object item)
        {
            Int32 output = -1;

            Int32 i = 0;
            foreach (layerCollection layer in layers)
            {
                if (layer.Contains(item)) return i;
                i++;
            }
            return output;
        }

        /// <summary>
        /// Removes the specified item. Returns <c>false</c> if the specified <c>item</c> was not found in any layer of the stack
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public Boolean Remove(Object item)
        {
            foreach (layerCollection layer in layers)
            {
                if (layer.Contains(item))
                {
                    return layer.Remove(item);
                }
            }
            return false;
        }

        private List<layerCollection> _layers = new List<layerCollection>();

        /// <summary> </summary>
        protected List<layerCollection> layers
        {
            get
            {
                return _layers;
            }
            set
            {
                _layers = value;
                //OnPropertyChanged("layers");
            }
        }

        public IEnumerator<layerCollection> GetEnumerator()
        {
            return ((IEnumerable<layerCollection>)layers).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<layerCollection>)layers).GetEnumerator();
        }
    }
}