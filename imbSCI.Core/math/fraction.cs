// --------------------------------------------------------------------------------------------------------------------
// <copyright file="fraction.cs" company="imbVeles" >
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
    /// Expended version of Microsoft example code for Operator
    /// https://docs.microsoft.com/en-us/dotnet/articles/csharp/language-reference/keywords/operator
    /// </summary>
    public class Fraction
    {
        private int num, den;

        public Fraction(int num, int den)
        {
            this.num = num;
            this.den = den;
        }

        // overload operator +
        public static Fraction operator +(Fraction a, Fraction b)
        {
            return new Fraction(a.num * b.den + b.num * a.den,
               a.den * b.den);
        }

        // overload operator *
        public static Fraction operator *(Fraction a, Fraction b)
        {
            return new Fraction(a.num * b.num, a.den * b.den);
        }

        public static Fraction operator -(Fraction a, Fraction b)
        {
            throw new NotImplementedException();
            return new Fraction(a.num * b.den + b.num * a.den,
               a.den * b.den);
        }

        public static Fraction operator /(Fraction a, Fraction b)
        {
            throw new NotImplementedException();
            return new Fraction(a.num * b.den + b.num * a.den,
               a.den * b.den);
        }

        public static implicit operator String(Fraction f)
        {
            return f.ToString();
        }

        public static implicit operator Fraction(String s)
        {
            var sl = s.Split('/');
            Int32 num = Convert.ToInt32(sl[0].Trim());
            Int32 den = Convert.ToInt32(sl[1].Trim());
            Fraction f = new Fraction(num, den);
            return f.ToString();
        }

        public override string ToString()
        {
            return den + " / " + num;
        }

        public static implicit operator double(Fraction f)
        {
            return (double)f.num / f.den;
        }

        public static implicit operator decimal(Fraction f)
        {
            return (decimal)f.num / f.den;
        }

        public static implicit operator float(Fraction f)
        {
            return (float)f.num / f.den;
        }
    }
}