// --------------------------------------------------------------------------------------------------------------------
// <copyright file="angle.cs" company="imbVeles" >
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
namespace imbSCI.Core.math
{
    using System;

    /// <summary>
    /// Holds an angle as both radians and degrees, with conversions
    /// </summary>
    public class angle : IConvertible
    {
        #region constructors

        /// <summary>
        /// Blank constructor. Remember to manually set some data to the properties!
        /// </summary>
        public angle()
        {
        }

        public angle(String input)
        {
            if (input == "*")
            {
                isUndefined = true;
            }
            else
            {
            }
        }

        //public static explicit operator double(angle d)  // implicit digit to byte conversion operator
        //{
        //    //System.Console.WriteLine("conversion occurred");
        //    return d.degrees;  // implicit conversion
        //}

        #region --- isUndefined ------- Is angle value left undefined

        private Boolean _isUndefined;

        /// <summary>
        /// Is angle value left undefined
        /// </summary>
        public Boolean isUndefined
        {
            get
            {
                return _isUndefined;
            }
            set
            {
                _isUndefined = value;
                // OnPropertyChanged("isUndefined");
            }
        }

        #endregion --- isUndefined ------- Is angle value left undefined

        /// <summary>
        /// Construct an angle with either a degree or a radian value
        /// </summary>
        /// <param name="input">angle value</param>
        /// <param name="angleType">Specify whether input is in radians or degrees</param>
        public angle(double input, Type angleType = angle.Type.Radians)
        {
            if (angleType == Type.Degrees)
            {
                Degrees = input;
            }
            else if (angleType == Type.Radians)
            {
                Radians = input;
            }
        }

        /// <summary>
        /// Quickly construct a common angle
        /// </summary>
        /// <param name="preset">Choose angle</param>
        public angle(angle.Preset preset)
        {
            if (preset == angle.Preset.Deg0) { Degrees = 0; }
            else if (preset == Preset.Deg180) { Degrees = 180; }
            else if (preset == Preset.Deg360) { Degrees = 360; }
            else if (preset == Preset.Rad2Pi) { Radians = twoPi; }
            else if (preset == Preset.RadPi) { Radians = Pi; }
            else Radians = 0;
        }

        #endregion constructors

        #region properties

        private double degrees;

        /// <summary>
        /// angle in degrees
        /// </summary>
        public double Degrees
        {
            get { return degrees; }
            set
            {
                degrees = value;
                radians = ToRadians(value);
                updateFixedangles();
            }
        }

        private double radians;

        /// <summary>
        /// angle in radians between -pi to pi
        /// </summary>
        public double Radians
        {
            get { return radians; }
            set
            {
                radians = value;
                degrees = ToDegrees(value);
                updateFixedangles();
            }
        }

        private double radians2pi;

        /// <summary>
        /// angle in radians, modified to fall between 0 and 2pi. Read-only.
        /// </summary>
        public double Radians2pi
        {
            get { return radians2pi; }
        }

        private double degrees360;

        /// <summary>
        /// Value in degrees between 0 and 360. Read-only.
        /// </summary>
        public double Degrees360
        {
            get { return degrees360; }
        }

        #endregion properties

        #region enums

        public enum Type { Radians, Degrees }

        /// <summary>
        /// Presets for quick setup of angle constructor
        /// </summary>
        public enum Preset { Deg0, Deg180, Deg360, RadPi, Rad2Pi }

        #endregion enums

        #region constants

        private const double twoPi = 2 * Math.PI;
        private const double Pi = Math.PI;

        #endregion constants

        #region methods

        /// <summary>
        /// Convert an input angle of degrees to radians. Does not affect any properties in this class - set properties instead.
        /// </summary>
        /// <param name="val">Input in degrees</param>
        /// <returns>Value in radians</returns>
        public static double ToRadians(double val)
        {
            return val / (180 / Pi);
        }

        /// <summary>
        /// Convert an input angle of radians into degrees. Does not affect any properties in this class - set properties instead.
        /// </summary>
        /// <param name="val">Input in radians</param>
        /// <returns>Value in degrees</returns>
        public static double ToDegrees(double val)
        {
            return val * (180 / Pi);
        }

        /// <summary>
        /// Fixes an angle to between 0 and 360 or 2pi.
        /// </summary>
        /// <param name="val">Input angle</param>
        /// <param name="type">Specify whether the input angle is radians or degrees</param>
        /// <returns>The angle, fixed to between 0 and 360 or 0 and 2pi</returns>
        public static double FixAngle(double val, Type type)
        {
            if (type == Type.Radians)
            {
                //-2pi to 0 to between 0 and 2pi
                if (val < 0)
                {
                    return 2 * Math.PI - (Math.Abs(val) % (2 * Math.PI));
                }
                //over 2pi to between 0 and 2pi
                else if (val > 2 * Math.PI)
                {
                    return val % (2 * Math.PI);
                }
                //else it's fine, return it back
                else
                {
                    return val;
                }
            }
            else if (type == Type.Degrees)
            {
                //-360 to 0 to between 0 and 360
                if (val < 0)
                {
                    return 360 - (Math.Abs(val) % 360);
                }
                //over 360 to between 0 and 360
                else if (val > 360)
                {
                    return val % 360;
                }
                //else it's fine, return it back
                else
                {
                    return val;
                }
            }
            else return -1; //something's gone wrong
        }

        /// <summary>
        /// Looks at the radians and degrees properties, and updates their respective fixed angles
        /// </summary>
        private void updateFixedangles()
        {
            radians2pi = FixAngle(radians, Type.Radians);
            degrees360 = FixAngle(degrees, Type.Degrees);
        }

#pragma warning disable CS1570 // XML comment has badly formed XML -- 'The character(s) '3' cannot be used at this location.'
#pragma warning disable CS1570 // XML comment has badly formed XML -- 'An identifier was expected.'
#pragma warning disable CS1570 // XML comment has badly formed XML -- 'An identifier was expected.'
#pragma warning disable CS1570 // XML comment has badly formed XML -- 'The character(s) '2' cannot be used at this location.'
        /// <summary>
        /// Copies Radians2pi to Radians, and Degrees360 to Degrees. (I.e. fixes radians to 0<2pi and degrees to 0<360)
        /// </summary>
        public void FixAngles()
#pragma warning restore CS1570 // XML comment has badly formed XML -- 'The character(s) '2' cannot be used at this location.'
#pragma warning restore CS1570 // XML comment has badly formed XML -- 'An identifier was expected.'
#pragma warning restore CS1570 // XML comment has badly formed XML -- 'An identifier was expected.'
#pragma warning restore CS1570 // XML comment has badly formed XML -- 'The character(s) '3' cannot be used at this location.'
        {
            updateFixedangles();
            Radians = Radians2pi; //this also calls Degrees
        }

        #endregion methods

        #region operators

        /// <summary>
        /// Returns the sum of two angles
        /// </summary>
        /// <param name="a1">First angle</param>
        /// <param name="a2">Second angle</param>
        /// <returns>An angle constructed from the radian sum of the input angles</returns>
        public static angle operator +(angle a1, angle a2)
        {
            return new angle(a1.degrees + a2.degrees, Type.Degrees);
        }

        /// <summary>
        /// Returns the difference between two angles
        /// </summary>
        /// <param name="a1">First angle</param>
        /// <param name="a2">Second angle</param>
        /// <returns>An angle constructed from the value that is the first angle minus the second angle</returns>
        public static angle operator -(angle a1, angle a2)
        {
            return new angle(a1.degrees - a2.degrees, Type.Degrees);
        }

        /// <summary>
        /// Returns the exact division between two angles (i.e. how many times does the second angle fit into the first)
        /// </summary>
        /// <param name="a1">Numerator angle</param>
        /// <param name="a2">Dedominator angle</param>
        /// <returns>A new angle constructed from the value that is the first angle in radians divided by the second angle in radians</returns>
        public static angle operator /(angle a1, angle a2)
        {
            return new angle(a1.degrees / a2.degrees, Type.Degrees);
        }

        public static angle operator *(angle a, double d)
        {
            return new angle(a.degrees * d, Type.Degrees);
        }

        public static angle operator *(double d, angle a)
        {
            return new angle(a.degrees * d, Type.Degrees);
        }

        public static angle operator /(angle a, double d)
        {
            return new angle(a.degrees / d, Type.Degrees);
        }

        public static bool operator <(angle a1, angle a2)
        {
            if (a1.degrees < a2.degrees) return true;
            else return false;
        }

        public static bool operator >(angle a1, angle a2)
        {
            if (a1.degrees > a2.degrees) return true;
            else return false;
        }

        public static bool operator <=(angle a1, angle a2)
        {
            if (a1.degrees <= a2.degrees) return true;
            else return false;
        }

        public static bool operator >=(angle a1, angle a2)
        {
            if (a1.degrees >= a2.degrees) return true;
            else return false;
        }

        public static implicit operator double(angle angleobj)
        {
            return angleobj.degrees;
        }

        public static implicit operator string(angle a)
        {
            return a.ToString();
        }

        #endregion operators

        #region overrides

        public override string ToString()
        {
            if (isUndefined)
            {
                return "*";
            }
            return degrees.ToString("#0.00000") + "";
        }

        public TypeCode GetTypeCode()
        {
            return TypeCode.Decimal;
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            return degrees >= 180;
        }

        public char ToChar(IFormatProvider provider)
        {
            return Convert.ToChar((Int32)degrees);
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public byte ToByte(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public short ToInt16(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public int ToInt32(IFormatProvider provider)
        {
            return (Int32)degrees;
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public long ToInt64(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public float ToSingle(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public double ToDouble(IFormatProvider provider)
        {
            return Degrees;
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            return (decimal)Degrees;
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public string ToString(IFormatProvider provider)
        {
            return ToString();
        }

        public object ToType(System.Type conversionType, IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        #endregion overrides
    }
}