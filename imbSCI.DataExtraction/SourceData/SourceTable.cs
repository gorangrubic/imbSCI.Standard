using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Core.files;
using imbSCI.Core.math.classificationMetrics;
using imbSCI.Core.reporting.zone;
using imbSCI.DataExtraction.MetaTables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.DataExtraction.SourceData
{
    /// <summary>
    /// Temporary table data structure to store raw data loaded from XML or HTML source
    /// </summary>
    [Serializable]
    public class SourceTable
    {

        public reportExpandedData ExpandedData { get; set; } = new reportExpandedData();

        public Boolean IsTransposed { get; set; } = false;

        /// <summary>
        /// Number of rows
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        [XmlIgnore]
        public Int32 Height
        {
            get
            {
                return Cells.Count;
            }
        }

        /// <summary>
        /// Number of cells per row
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        [XmlIgnore]
        public Int32 Width
        {
            get
            {
                if (!Cells.Any()) return 0;
                return Cells[0].Count;
            }
        }

        /// <summary>
        /// Gets the <see cref="SourceTableCell"/> at specified row <c>y</c> and column <c>x</c>
        /// </summary>
        /// <value>
        /// The <see cref="SourceTableCell"/>.
        /// </value>
        /// <param name="x">Column index</param>
        /// <param name="y">Row index</param>
        /// <returns></returns>
        public SourceTableCell this[Int32 x, Int32 y]
        {
            get
            {
                return GetCell(x, y);
            }
        }

        public List<SourceTableCell> GetRowAsCells(Int32 y = -1)
        {
            if (y == -1) y = 0;
            List<SourceTableCell> output = new List<SourceTableCell>();

            for (int i = 0; i < Cells[y].Count; i++)
            {
                output.Add(Cells[y][i]);
            }
            return output;
        }


        public List<String> GetRow(Int32 y=-1)
        {
            if ( y == -1)  y = 0;
            List<String> output = new List<string>();
            
            for (int i = 0; i < Cells[y].Count; i++)
            {
                output.Add(Cells[y][i].Value);
            }
            return output;
        }

        public List<String> GetColumn(Int32 x=-1)
        {
            if (x == -1) x = 0;

            List<String> output = new List<string>();
            
            for (int i = 0; i < Height; i++)
            {
                output.Add(Cells[i][x].Value);
            }
            return output;
        }

        /// <summary>
        /// Gets cell at given coordinates. If cell does not exist, returns null
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public SourceTableCell GetCell(Int32 x, Int32 y)
        {
            if (checkCoordinates(x, y))
            {
                SourceTableCell output = Cells[y][x];
                return output;
            }
            else
            {
                return null;
            }
        }

        public void SetCell(Int32 x, Int32 y, SourceTableCell cell)
        {
            if (checkCoordinates(x, y))
            {
                Cells[y][x] = cell;
                
            }
            else
            {
                
            }
        }


        /*
        public List<List<String>> GetContentBySelectZone(selectZone zone)
        {
            List<List<String>> output = new List<List<string>>();

            for (int i = 0; i < Height; i++)
            {
                if (i >= zone.y && i < zone.y + zone.height)
                {
                    List<String> row = new List<string>();
                    for (int j = 0; j < Width; j++)
                    {
                        SourceTableCell cell = GetCell(j, i);
                        row.Add(cell.Value);
                    }
                    output.Add(row);
                }
            }
            return output;
        }*/

        /// <summary>
        /// Gets the content cells.
        /// </summary>
        /// <param name="ByColumns">if set to <c>true</c> [by columns].</param>
        /// <returns></returns>
        public List<List<SourceTableCell>> GetContentCells(Boolean ByColumns=false)
        {
            var output = new List<List<SourceTableCell>>();
            if (ByColumns)
            {
                for (int i = 0; i < Width; i++)
                {
                    var row = new List<SourceTableCell>();
                    for (int j = 0; j < Height; j++) row.Add(GetCell(i, j));
                    output.Add(row);
                }
            }
            else
            {
                for (int i = 0; i < Height; i++)
                {
                    var row = new List<SourceTableCell>();
                    for (int j = 0; j < Width; j++) row.Add(GetCell(j, i));
                    output.Add(row);
                }
            }
            return output;
        }

        public List<List<String>> GetContentByColumns()
        {
            List<List<String>> output = new List<List<string>>();

            for (int i = 0; i < Width; i++)
            {
                List<String> column = new List<string>();
                for (int j = 0; j < Height; j++)
                {
                    SourceTableCell cell = GetCell(i, j);
                    column.Add(cell.Value);
                }
                output.Add(column);
            }
            return output;
        }

        public List<List<String>> GetContentByRows()
        {
            List<List<String>> output = new List<List<string>>();

            for (int i = 0; i < Height; i++)
            {
                List<String> row = new List<string>();
                for (int j = 0; j < Width; j++)
                {
                    SourceTableCell cell = GetCell(j, i);
                    row.Add(cell.Value);
                }
                output.Add(row);
            }
            return output;
        }

        /// <summary>
        /// Gets the value. If cell does not exist, returns default value for {T}
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x">The x - index of column</param>
        /// <param name="y">The y - index of row</param>
        /// <returns></returns>
        public T GetValue<T>(Int32 x, Int32 y)
        {
            SourceTableCell output = GetCell(x, y);
            if (output != null)
            {
                return output.Value.imbConvertValueSafeTyped<T>();
            }
            else
            {
                return default(T);
            }
        }

        protected Boolean checkCoordinates(Int32 x, Int32 y)
        {
            if (x < 0) return false;
            if (y < 0) return false;
            if (Cells.Any())
            {
                if (y < Cells.Count)
                {
                    if (x < Cells[0].Count)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// y,x matrix: [row][column]
        /// </summary>
        /// <value>
        /// The cells.
        /// </value>
        protected List<List<SourceTableCell>> Cells { get; set; } = new List<List<SourceTableCell>>();

        protected void Init(Int32 width, Int32 height)
        {
            Cells = new List<List<SourceTableCell>>();

            if (width < 1 || height < 1)
            {
                return;
                //
            }

            
            for (int i = 0; i < height; i++)
            {
                List<SourceTableCell> row = new List<SourceTableCell>();

                for (int j = 0; j < width; j++)
                {
                    SourceTableCell cell = new SourceTableCell();

                    row.Add(cell);
                }

                Cells.Add(row);
            }
        }

        [XmlIgnore]
        public Boolean IsValid
        {
            get
            {
                if (Height < 1 || Width < 1) return false;
                return true;
            }
        }

      

        public SourceTable MergeAsRows(SourceTable other, Boolean OnlyDistinct=false)
        {

            var thisRows = GetContentByRows();
            var otherRows = other.GetContentByRows();

            //var allRows = new List<List<String>>();
            //allRows.AddRange(thisRows);
            //allRows.AddRange(otherRows);

            //if (OnlyDistinct) allRows = TakeDistinct(allRows);

            //SourceTable output = new SourceTable(Width, allRows.Count);

            SourceTable output = SourceTableExtensions.Merge(new List<List<List<String>>>() { thisRows, otherRows }, false, OnlyDistinct); //new SourceTable(allColumns.Count, Height);


            //for (int i = 0; i < output.Height; i++)
            //{
            //    var row = allRows[i];
            //    //List<SourceTableCell> row = new List<SourceTableCell>();

            //    for (int j = 0; j < output.Width; j++)
            //    {
            //        if (j < row.Count())
            //        {
            //            output[j, i].Value = row[j];
            //        }
            //        //  SourceTableCell cell = new SourceTableCell();

            //        //row.Add(cell);
            //    }

            //    //Cells.Add(row);
            //}

            return output;

        }

        public SourceTable Transpose()
        {
            if (!IsTransposed)
            {
                SourceTable output = new SourceTable(Height, Width);
                output.IsTransposed = true;
                for (int i = 0; i < Width; i++)
                {

                    //List<SourceTableCell> row = new List<SourceTableCell>();

                    for (int j = 0; j < Height; j++)
                    {
                        output[j, i].Value = this[i, j].Value;
                        //  SourceTableCell cell = new SourceTableCell();

                        //row.Add(cell);
                    }

                    //Cells.Add(row);
                }
                return output;
            } else
            {
                return this;
            }
            
        }

        public SourceTable MergeAsColumns(SourceTable other, Boolean OnlyDistinct = false)
        {

            var thisColumns = GetContentByColumns();
            var otherColumns = other.GetContentByColumns();

            //var allColumns = new List<List<String>>();
            //allColumns.AddRange(thisColumns);
            //allColumns.AddRange(otherColumns);
            //if (OnlyDistinct) allColumns = TakeDistinct(allColumns);

            SourceTable output = SourceTableExtensions.Merge(new List<List<List<String>>>() { thisColumns, otherColumns }, true, OnlyDistinct);//  new SourceTable(allColumns.Count, Height);

            //for (int i = 0; i < output.Width; i++)
            //{
            //    var column = allColumns[i];
            //    //List<SourceTableCell> row = new List<SourceTableCell>();

            //    for (int j = 0; j < output.Height; j++)
            //    {
            //        if (j < column.Count)
            //        {
            //            output[i, j].Value = column[j];
            //        }
            //        //  SourceTableCell cell = new SourceTableCell();

            //        //row.Add(cell);
            //    }

            //    //Cells.Add(row);
            //}

            return output;

        }

       

        /// <summary>
        /// Gets simple datatable output, for diagnostic purposes
        /// </summary>
        /// <returns></returns>
        public DataTable GetDataTable()
        {
            DataTable output = new DataTable();
            List<DataColumn> dcl = new List<DataColumn>();

            for (int j = 0; j < Width; j++)
            {
                DataColumn dc = output.Columns.Add("dc" + j.ToString(), typeof(String));
                dcl.Add(dc);
            }

            for (int i = 0; i < Height; i++)
            {
                DataRow dr = output.NewRow();

                for (int i2 = 0; i2 < dcl.Count; i2++)
                {
                    var cl = this[i2, i];
                    if (cl != null)
                    {
                        if (cl.Value.isNullOrEmpty())
                        {
                            dr[dcl[i2]] = dcl[i2].DefaultValue;
                        } else
                        {
                            dr[dcl[i2]] = cl.Value;
                        }
                        
                    } else
                    {
                        dr[dcl[i2]] = dcl[i2].DefaultValue;
                    }
                    
                }

                output.Rows.Add(dr);
            }

            return output;
        }

        /// <summary>
        /// Saves the specified filepath.
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        public void Save(String filepath)
        {
            objectSerialization.saveObjectToXML(Cells, filepath);
        }

        public static SourceTable Load(String filepath)
        {
            var cells = objectSerialization.loadObjectFromXML<List<List<SourceTableCell>>>(filepath);
            SourceTable output = new SourceTable();
            output.Cells = cells;

            return output;
        }

        public SourceTable() { }

        public SourceTable(Int32 width, Int32 height)
        {
            Init(width, height);
        }
    }
}