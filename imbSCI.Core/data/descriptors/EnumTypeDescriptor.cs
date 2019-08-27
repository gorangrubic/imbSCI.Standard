using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace imbSCI.Core.data.descriptors
{
public class EnumTypeDescriptor<T>:IEnumTypeDescriptor
    {
        public EnumTypeDescriptor()
        {
            Deploy();
        }

        Dictionary<String, Enum> StringToEnum = new Dictionary<string, Enum>();
        Dictionary<Int32, Enum> Int32ToEnum = new Dictionary<Int32, Enum>();
        Dictionary<T, Enum> TypeToEnum = new Dictionary<T, Enum>();

        Dictionary<Enum, String> EnumToString = new Dictionary<Enum, string>();
        Dictionary<Enum, Int32> EnumToInt32 = new Dictionary<Enum, Int32>();
        Dictionary<Enum, T> EnumToType = new Dictionary<Enum, T>();

        Dictionary<String, T> StringToType = new Dictionary<string, T>();
        Dictionary<Int32, T> Int32ToType = new Dictionary<int, T>();

        Dictionary<T, String> TypeToString = new Dictionary<T, string>();
        Dictionary<T, Int32> TypeToInt32 = new Dictionary<T, int>();
        private T _min;
        private T _max;

        protected void Deploy()
        {
         
            var array = Enum.GetValues(typeof(T));

            foreach (Enum en in array)
            {
                String ens = en.ToString();
                Int32 eni = Convert.ToInt32(en);

                T ent = (T)Enum.ToObject(typeof(T), eni);


                EnumToString.Add(en, ens);
                EnumToInt32.Add(en, eni);
                EnumToType.Add(en, ent);
                StringToEnum.Add(ens, en);
                Int32ToEnum.Add(eni, en);
                TypeToEnum.Add(ent, en);
                StringToType.Add(ens, ent);
                TypeToString.Add(ent, ens);

                Int32ToType.Add(eni, ent);
                TypeToInt32.Add(ent, eni);

                if (eni > MaxInt)
                {
                    MaxInt = eni;
                    Max = ent;
                }

                if (eni < MinInt)
                {
                    MinInt = eni;
                    Min = ent;
                }
            }

            foreach (Enum en in array)
            {

                 var enumComponents = en.getEnumListFromFlags();
                if (enumComponents.Count == 1)
                {
                    BaseFlags.Add(EnumToType[en]);
                } else
                {
                    CompositeFlags.Add(EnumToType[en]);
                }
            }

         
        }

        public Type EnumType { get; protected set; } = typeof(T);

        public Enum FromName(String value)
        {
            return StringToEnum[value];
        }

        public Enum FromInt32(Int32 value)
        {
            return Int32ToEnum[value];
        }

        public Enum FromTypedValue(T value)
        {
            return TypeToEnum[value];
        }

        public T ByEnum(Enum value)
        {
            return EnumToType[value];
        }


        public T ByName(String value)
        {
            return StringToType[value];
        }

        public T ByInt32(Int32 value)
        {
            return Int32ToType[value];
        }

        public Int32 MinInt { get; protected set; } = Int32.MaxValue;
        public Int32 MaxInt  { get; protected set; } = Int32.MinValue;

        public T Min
        {
            get { return _min; }
            set { _min = value; }
        }

        public T Max
        {
            get { return _max; }
            set { _max = value; }
        }

        /// <summary>
        /// Enum members that are base flags
        /// </summary>
        /// <value>
        /// The base flags.
        /// </value>
        public List<T> BaseFlags { get; protected set; } = new List<T>();

        /// <summary>
        /// Enum members that contain other flag enums 
        /// </summary>
        /// <value>
        /// The composite flags.
        /// </value>
        public List<T> CompositeFlags { get; protected set; } = new List<T>();



    }
}