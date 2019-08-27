using imbSCI.Data.collection.nested;
using imbSCI.Core.extensions.enumworks;
using System;
using System.Collections.Generic;
using System.Text;
using imbSCI.Data;
using imbSCI.Core.math;
using System.Linq;

namespace imbSCI.Core.style.color
{
     [Flags]
        public enum DemoFlags
        {
            none=0,
            flagA=1,
            flagB=2,
            flagC=4,
            flagD=8,
            flagAB = flagA | flagB,
            flagCD = flagC | flagD
        }

    public class ColorEnumDictionary<T>: aceEnumDictionary<T, String>
    {
       


          public ColorEnumDictionary(String hexColorStart, String hexColorEnd, ColorGradientFunction functions=ColorGradientFunction.HueAToB)
        {
            ColorGradient g = new ColorGradient(hexColorStart, hexColorEnd, functions);
            Deploy(g);
        }

        public ColorEnumDictionary(String hexColorStart)
        {
            ColorGradient g = new ColorGradient(hexColorStart);
            Deploy(g);
        }


        public ColorEnumDictionary(ColorGradient _gradient)
        {

            Deploy(_gradient);
        }

        protected void Deploy(ColorGradient _gradient)
        {
            gradient = _gradient;
           // Colors = new aceEnumDictionary<T, string>();

             var dict = imbSciEnumExtensions.getEnumDictionary<T>();

            var compositeEnumFlags = imbSciEnumExtensions.getEnumList<T>();
            var baseEnumFlags = imbSciEnumExtensions.getBaseFlags<T>();

            for (int i = 0; i < baseEnumFlags.Count; i++)
            {
                var c = gradient.GetHexColor(i.GetRatio(baseEnumFlags.Count));
                //Colors[baseEnumFlags[i]] = c;

                this[baseEnumFlags[i]] = c;
                var ce = compositeEnumFlags.FirstOrDefault(x => x.ToString() == baseEnumFlags[i].ToString()); //.Remove(baseEnumFlags[i]);
                compositeEnumFlags.Remove(ce);
            }

            foreach (T composite_enum in compositeEnumFlags)
            {
                Double rt = 0.5;
                Enum c_e = dict[composite_enum.ToString()];
                if (Convert.ToInt32(c_e) == 0)
                {

                }
                else
                {

                    List<Double> posList = new List<double>();
                    var enumComponents = dict[composite_enum.ToString()].getEnumListFromFlags(); //  composite_enum.getEnumListFromFlags<T>();
                    foreach (var e in enumComponents)
                    {
                        Double r = 0.5;
                        if (baseEnumFlags.Any(x => x.ToString() == e.ToString()))
                        {
                            var ce = baseEnumFlags.First(x => x.ToString() == e.ToString());
                            r = baseEnumFlags.IndexOf(ce).GetRatio(baseEnumFlags.Count);
                        }
                        posList.Add(r);
                    }

                    rt = posList.Average();
                }
                
                var c = gradient.GetHexColor(rt);

               // T value = (T)Enum.ToObject(typeof(T), composite_enum.ToInt32());


                this[composite_enum] = c;
            }


            //gradient.GetHexColor()
            
        }

        public ColorGradient gradient { get; set; }

        //public  Colors { get; set; } = null;

    }
}
