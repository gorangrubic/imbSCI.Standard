// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeatMapModel.cs" company="imbVeles" >
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
namespace imbSCI.Core.math.range.matrix
{
    using imbSCI.Core.collection;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.extensions.table;
    using imbSCI.Core.math;

    //using imbSCI.Data.collection.nested;
    using imbSCI.Core.math.range.finder;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Xml.Serialization;

    /// <summary>
    /// Intensity matrix
    /// </summary>
    /// <seealso cref="System.Collections.Generic.List{System.Collections.Generic.List{System.Double}}" />
    public class HeatMapModel : List<List<Double>>
    {

        public DataTable GetDataTable(String name, String description)
        {
            DataTable table = new DataTable(name);
            table.SetDescription(description);


            foreach (String xk in xKeys)
            {
                DataColumn dc = table.Columns.Add(xk, typeof(Double));
                dc.SetFormat("F5");


            }

            foreach (String yk in yKeys)
            {
                var dr = table.NewRow();
                foreach (String xk in xKeys)
                {
                    dr[xk] = this[xk, yk];
                }
                table.Rows.Add(dr);
            }



            return table;
        }




        [XmlIgnore]
        public PropertyCollectionExtended properties { get; set; } = new PropertyCollectionExtended();

        private Boolean _deployed;

        /// <summary>
        ///
        /// </summary>
        protected Boolean deployed
        {
            get
            {
                return _deployed;
            }
            set { _deployed = value; }
        }

        // private Boolean deployed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeatMapModel"/> class.
        /// </summary>
        public HeatMapModel()
        {
        }

        /// <summary>
        /// Gets or sets the artificial maximum.
        /// </summary>
        /// <value>
        /// The artificial maximum.
        /// </value>
        public Double ArtificialMaximum { get; set; } = 0;

        public List<String> xKeys { get; set; } = new List<string>();
        public List<String> yKeys { get; set; } = new List<string>();

        /// <summary>
        /// Initializes square heat map matrix
        /// </summary>
        /// <param name="columnsAndRows">The columns and rows.</param>
        public HeatMapModel(IEnumerable<String> columnsAndRows)
        {
            foreach (String col in columnsAndRows)
            {
                xKeys.Add(col);
                yKeys.Add(col);
                var cl = new List<double>();

                foreach (String row in columnsAndRows)
                {
                    cl.Add(0);
                }

                this.Add(cl);
            }
            deployed = true;
        }

        /// <summary>
        /// Initializes rectanglural heat map matrix
        /// </summary>
        /// <param name="columns">The columns.</param>
        /// <param name="rows">The rows.</param>
        public void Deploy(IEnumerable<String> columns, IEnumerable<String> rows)
        {
            foreach (String col in columns)
            {
                xKeys.Add(col);
            }

            foreach (String col in rows)
            {
                yKeys.Add(col);
            }

            foreach (String col in columns)
            {
                var cl = new List<double>();
                foreach (String row in rows)
                {
                    cl.Add(0);
                }
                this.Add(cl);
            }
            deployed = true;
        }

        /// <summary>
        /// Creates the specified width.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="axisFormat">The axis format.</param>
        /// <returns></returns>
        public static HeatMapModel Create(Int32 width, Int32 height, String axisFormat = "D2")
        {
            return CreateRandom(width, height, 0, 0, axisFormat);
        }

        /// <summary>
        /// Creates randomized heat map, with specified x-y axis size.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <param name="axisFormat">The axis format.</param>
        /// <returns></returns>
        public static HeatMapModel CreateRandom(Int32 width, Int32 height, Int32 min, Int32 max, String axisFormat = "D2")
        {
            List<String> xKeys = new List<string>();
            List<String> yKeys = new List<string>();

            for (int i = 0; i < width; i++)
            {
                xKeys.Add(i.ToString(axisFormat));
            }

            for (int i = 0; i < height; i++)
            {
                yKeys.Add(i.ToString(axisFormat));
            }

            HeatMapModel model = new HeatMapModel();
            model.Deploy(xKeys, yKeys);
            model.Randomize(min, max);

            return model;
        }

        /// <summary>
        /// Gets the weight.
        /// </summary>
        /// <value>
        /// The weight.
        /// </value>
        [XmlIgnore]
        public Int32 weight
        {
            get
            {
                return xKeys.Count;
            }
        }

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        [XmlIgnore]
        public Int32 height
        {
            get
            {
                return yKeys.Count;
            }
        }

        /// <summary>
        /// Selection by string labels
        /// </summary>
        /// <value>
        /// The <see cref="Double"/>.
        /// </value>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public Double this[String x, String y]
        {
            get
            {
                Int32 _x = xKeys.IndexOf(x);
                Int32 _y = yKeys.IndexOf(y);
                if (_x < 0) return 0;
                if (_y < 0) return 0;

                return this[_x, _y];
            }
            set
            {
                Int32 _x = xKeys.IndexOf(x);
                Int32 _y = yKeys.IndexOf(y);
                if (_x < 0) return;
                if (_y < 0) return;

                this[_x, _y] = value;
            }
        }

        /// <summary>
        /// Makes sure that matrix cells up to of specified dimensions is instantiated the size.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public void AllocateSize(Int32 x, Int32 y)
        {
            if (!deployed) Deploy(xKeys, yKeys);

            while (x >= this.Count)
            {
                this.Add(new List<double>());
            }

            while (y >= this[x].Count)
            {
                this[x].Add(0);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Double"/> with the specified x.
        /// </summary>
        /// <value>
        /// The <see cref="Double"/>.
        /// </value>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public Double this[Int32 x, Int32 y]
        {
            get
            {
                AllocateSize(x, y);
                return this[x][y];
            }
            set
            {
                AllocateSize(x, y);

                this[x][y] = value;
            }
        }

        /// <summary>
        /// Randomizes the values writen in the matrix
        /// </summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        public void Randomize(Int32 min = 1, Int32 max = 100)
        {
            Random rnd = new Random();

            for (int x = 0; x < this.Count; x++)
            {
                for (int y = 0; y < this[x].Count; y++)
                {
                    Double vl = rnd.Next(min, max);//.GetRatio(max - min);
                    this[x][y] = vl;
                }
            }

            DetectMinMax();
        }

        public Double GetValueForScaleY(Int32 scaleStep, Int32 scaleSteps = -1)
        {
            if (scaleSteps == -1) scaleSteps = yKeys.Count;
            if (scaleStep == 0) return ranger.Minimum;

            if (scaleStep == scaleSteps) return ranger.Maximum;

            Double absValue = (ranger.Range.GetRatio(scaleSteps) * scaleStep) + ranger.Minimum;

            return absValue;
        }

        public Double GetValueForScaleX(Int32 scaleStep, Int32 scaleSteps = -1)
        {
            if (scaleSteps == -1) scaleSteps = xKeys.Count;
            if (scaleStep == 0) return ranger.Minimum;

            if (scaleStep == scaleSteps) return ranger.Maximum;

            Double absValue = (ranger.Range.GetRatio(scaleSteps) * scaleStep) + ranger.Minimum;

            return absValue;
        }

        public Double GetRatioForScale(Int32 scaleStep, Double floor = 0, Int32 scaleSteps = -1)
        {
            if (scaleSteps == -1) scaleSteps = xKeys.Count;

            Double val = scaleStep.GetRatio(scaleSteps);

            if (floor == 0)
            {
                return val;
            }

            val = val / (1 + floor);
            val = val + floor;
            if (val > 1) val = 1;

            //if (scaleStep == 0) return ranger.Minimum;

            //if (scaleStep == scaleSteps) return ranger.Maximum;

            //if (ranger == null) return absValue;

            //absValue = absValue - ranger.Minimum;
            //absValue = absValue.GetRatio(ranger.Range);// * ranger.Maximum;
            return val;
        }

        /// <summary>
        /// Gets the value that is
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="floor">The floor.</param>
        /// <returns></returns>
        public Double GetRatioValue(Int32 x, Int32 y, Double floor = 0)
        {
            Double val = this[x, y];
            if (ranger == null) return val;

            val = val - ranger.Minimum;
            val = val.GetRatio(ranger.Range);// * ranger.Maximum;
            if (floor == 0)
            {
                return val;
            }
            val = val / (1 + floor);
            val = val + floor;
            if (val > 1) val = 1;

            return val;
        }

        /// <summary>
        /// Gets or sets the ranger.
        /// </summary>
        /// <value>
        /// The ranger.
        /// </value>
        [XmlIgnore]
        public rangeFinder ranger { get; set; }

        /// <summary>
        /// Sets the ranger
        /// </summary>
        public void DetectMinMax()
        {
            ranger = new rangeFinder();

            if (ArtificialMaximum != 0) ranger.Learn(ArtificialMaximum);
            for (int x = 0; x < this.Count; x++)
            {
                for (int y = 0; y < this[x].Count; y++)
                {
                    ranger.Learn(this[x][y]);
                }
            }
        }
    }
}