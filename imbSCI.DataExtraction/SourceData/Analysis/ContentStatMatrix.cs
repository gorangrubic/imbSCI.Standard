using imbSCI.Core.extensions.text;
using imbSCI.Core.math;
using imbSCI.Core.math.range.frequency;
using imbSCI.Core.reporting.zone;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using imbSCI.Data.primitives;

namespace imbSCI.DataExtraction.SourceData.Analysis
{
    public class ContentStatMatrix
    {
        public void Transpose()
        {
            var tmp = column_stats.ToList();
            column_stats = row_stats;
            row_stats = tmp;
        }


        internal CellContentStats total { get; set; } = new CellContentStats();
        public List<CellContentStats> row_stats { get; protected set; } = new List<CellContentStats>();
        public List<CellContentStats> column_stats { get; protected set; } = new List<CellContentStats>();


        Dictionary<String, CellContentInfo> registry = new Dictionary<string, CellContentInfo>();
        protected String GetRegistryKey(Int32 x, Int32 y)
        {
            return x.ToString("D5") + ":" + y.ToString("D5");
        }

        protected void SetToRegistry(Int32 x, Int32 y,  CellContentInfo info)
        {
            String key = GetRegistryKey(x, y);
            if (!registry.ContainsKey(key)) registry.Add(key, info);
        }

        protected CellContentInfo GetFromRegistry(Int32 x, Int32 y)
        {
             String key = GetRegistryKey(x, y);
            if (registry.ContainsKey(key))
            {
               return registry[key];
            } else
            {
                return null;
            }
        }

        public ContentStatMatrix(Int32 rows, Int32 columns)
        {
            for (int i = 0; i < rows; i++)
            {
                CellContentStats stat = new CellContentStats();
                row_stats.Add(stat);
            }

            for (int i = 0; i < columns; i++)
            {
                CellContentStats stat = new CellContentStats();
                column_stats.Add(stat);
            }
        }

        public void Assign(CellContentInfo info, Int32 x, Int32 y)
        {
            SetToRegistry(x, y, info);
            row_stats[y].Assign(info);
            column_stats[x].Assign(info);
            total.Assign(info);
        }

        public CellContentInfo Get(Int32 x, Int32 y)
        {
            return GetFromRegistry(x, y);
        }

        protected void SetZoneCornerUL(coordinateXY zone)
        {
            for (int x = 0; x < column_stats.Count; x++)
            {
                for (int y = 0; y < row_stats.Count; y++)
                {
                    CellContentInfo info = Get(x, y);
                    if (info != null)
                    {
                        if (info.type == total.dominantType)
                        {
                            zone.x = x;
                            zone.y = y;
                            return;
                        }
                    }
                }

            }
        }

        //protected void SetZoneCornerDR(coordinateXY zone)
        //{
        //    for (int x = column_stats.Count - 1; x >= 0; x--)
        //    {
        //        for (int y = row_stats.Count - 1; y >= 0; y--)
        //        {
        //            CellContentInfo info = Get(x, y);
        //            if (info != null)
        //            {
        //                if (info.type == total.dominantType)
        //                {
        //                    zone.width = x -zone.x;
        //                    zone.height = y - zone.y;
        //                    return;
        //                }
        //            }
        //        }

        //    }
        //}

        public coordinateXY GetValueZone()
        {
            coordinateXY output = new coordinateXY();

           // SetZoneCornerUL(output);
            
            return output;
        }

        public void Compute()
        {
            foreach (var r in row_stats)
            {
                r.Compute();
            }

            foreach (var c in column_stats)
            {
                c.Compute();
            }

            total.Compute();
        }
    }
}