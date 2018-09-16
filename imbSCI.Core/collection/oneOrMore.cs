// --------------------------------------------------------------------------------------------------------------------
// <copyright file="oneOrMore.cs" company="imbVeles" >
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
namespace imbSCI.Core.collection
{
    using imbSCI.Core.extensions.typeworks;

    #region imbVeles using

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    #endregion imbVeles using

    /// <summary>
    /// 2014c> napredna kolekcija kojoj moze da se pristupa kao da je jedan objekat u pitanju - tada se vidi samo prvi item. Obavestava o promenama u kolekciji
    /// </summary>
    /// <remarks>
    /// Po defaultu ne kreira sam prvi item.
    /// doAutocreateFirst se podesava kroz konstruktor
    /// </remarks>
    /// <example>
    /// oneOrMoreCollection(object) item;
    ///
    /// item = object; // ovo postavlja prvi objekat u kolekciju
    ///
    /// object tmp = item; // ovo vraca prvi objekat u nizu, ako jos nije instanciran oneOrMoreCollection
    /// </example>
    /// <typeparam name="T">Objekat koji se smesta u kolekciju</typeparam>
    public class oneOrMore<T> : ObservableCollection<T>, IOneOrMore // where T : class
    {
        #region --- type ------- informacije o tipu koji je smesten i jedini dozvoljen u kolekciji

        private Type _iTi;

        /// <summary>
        /// informacije o tipu koji je smesten i jedini dozvoljen u kolekciji
        /// </summary>
        public Type iTI
        {
            get { return _iTi; }
            set { _iTi = value; }
        }

        #endregion --- type ------- informacije o tipu koji je smesten i jedini dozvoljen u kolekciji

        #region ----------- Boolean [ doAutocreateFirst ] -------  [Da li da automatski napravi prvi item]

        private Boolean _doAutocreateFirst = false;

        /// <summary>
        /// Da li da automatski napravi prvi item - definise se samo pri instanciranju, ostavljeno kao field cisto ako budem menjao
        /// </summary>
        private Boolean doAutocreateFirst
        {
            get { return _doAutocreateFirst; }
            set { _doAutocreateFirst = value; }
        }

        #endregion ----------- Boolean [ doAutocreateFirst ] -------  [Da li da automatski napravi prvi item]

        /// <summary>
        /// Moze da se definise da li dodaje prvi element ili ne
        /// </summary>
        /// <param name="__doAutocreateFirst"></param>
        public oneOrMore(Boolean __doAutocreateFirst)
        {
            deploy();
            doAutocreateFirst = __doAutocreateFirst;
        }

        /// <summary>
        /// Napravice prvi element ako postoji parametarless konstruktor
        /// </summary>
        public oneOrMore()
        {
            deploy();
        }

        public oneOrMore(params T[] content)
        {
            foreach (T t in content)
            {
                Add(t);
            }

            deploy();
        }

        /// <summary>
        /// direktan pristup prvom itemu
        /// </summary>
        public T Value
        {
            get { return getOrMakeFirst(); }
            set { setFirst(value); }
        }

        /// <summary>
        /// Da li ima vise od jednog elementa
        /// </summary>
        public Boolean isMany
        {
            get { return (Count > 1); }
        }

        #region IOneOrMore Members

        Object IOneOrMore.Value
        {
            get { return getOrMakeFirst(); }
            set { setFirst((T)value); }
        }

        /// <summary>
        /// TRUE kad nema nista u kolekciji, ali je inicijalizovana
        /// </summary>
        public Boolean isNothing
        {
            get { return !(Count > 0); }
        }

        /// <summary>
        /// Kontra od isNothing - tj. da ima nesto u sebi
        /// </summary>
        public Boolean isSomething
        {
            get { return (Count > 0); }
        }

        #endregion IOneOrMore Members

        #region --- defaultValue ------- podrazumevana vrednost

        /// <summary>
        /// podrazumevana vrednost
        /// </summary>
        protected T defaultValue
        {
            get { return this.GetDefaultValue<T>(); }
        }

        #endregion --- defaultValue ------- podrazumevana vrednost

        // public static oneOrMore<TI> makeOneOrMore<TI>(params TI)

        private void deploy()
        {
            //  iTI = typeof (T).getTypology();
            //doAutocreateFirst = iTI.hasParametarlessConstructor;
        }

        public static implicit operator T(oneOrMore<T> conteiner)
        {
            if (conteiner == null) conteiner = new oneOrMore<T>();
            return conteiner.getOrMakeFirst();
        }

        /// <summary>
        /// Pravi novi kontejner
        /// </summary>
        /// <param name="newItem"></param>
        /// <returns></returns>
        public static implicit operator oneOrMore<T>(T newItem)
        {
            var conteiner = new oneOrMore<T>();
            conteiner.setFirst(newItem);
            return conteiner;
        }

        /// <summary>
        /// Dodaje novi ili prvi clan u kolekciju - a ako je kolekcija null onda je instancira
        /// </summary>
        /// <param name="col"></param>
        /// <param name="newItem"></param>
        /// <returns></returns>
        public static oneOrMore<T> operator +(oneOrMore<T> col, T newItem)
        {
            if (col == null)
            {
                col = new oneOrMore<T>();
            }
            if (!col.Contains(newItem)) col.Add(newItem);
            return col;
        }

        /// <summary>
        /// Dodaje sve elemente iz druge kolekcije
        /// </summary>
        /// <param name="col"></param>
        /// <param name="newItem"></param>
        /// <returns></returns>
        public static oneOrMore<T> operator +(oneOrMore<T> col, IEnumerable<T> newItem)
        {
            if (col == null)
            {
                col = new oneOrMore<T>();
            }
            foreach (T t in newItem)
            {
                if (!col.Contains(t)) col.Add(t);
            }
            return col;
        }

        /// <summary>
        /// Oduzima sve clanove iz prosledjene kolekcije
        /// </summary>
        /// <param name="col"></param>
        /// <param name="newItem"></param>
        /// <returns></returns>
        public static oneOrMore<T> operator -(oneOrMore<T> col, IEnumerable<T> newItem)
        {
            if (col == null)
            {
                col = new oneOrMore<T>();
            }
            foreach (T t in newItem)
            {
                if (col.Contains(t)) col.Remove(t);
            }
            return col;
        }

        /// <summary>
        /// Sklanja item iz operatora
        /// </summary>
        /// <param name="col"></param>
        /// <param name="newItem"></param>
        /// <returns></returns>
        public static oneOrMore<T> operator -(oneOrMore<T> col, T newItem)
        {
            if (col == null)
            {
                col = new oneOrMore<T>();
            }
            else
            {
                col.Remove(newItem);
            }

            return col;
        }

        /// <summary>
        /// javni metod za pristup prvom itemu - za neposredan pristup koristiti property Value
        /// </summary>
        /// <returns></returns>
        public T First()
        {
            return getOrMakeFirst();
        }

        /// <summary>
        /// vraca prvi item ili ga stvara ako tip ima parametarless konstruktor
        /// </summary>
        /// <returns></returns>
        private T getOrMakeFirst()
        {
            if (isSomething)
            {
                return this[0];
            }
            else
            {
                if (!doAutocreateFirst) return defaultValue;

                /*
                if (iTI.hasParametarlessConstructor)
                {
                    Add((T) iTI.getInstance(makeInstanceMode.newItem));
                    return this.First();
                }
                else
                {
                    var cTI = this.getTypology();
                    this.note(devNoteType.metaCollectionApplication,
                              "doAutocreateFirst is on, but T " + iTI.typeSignature() +
                              " has no parameterless constructor. Find and remove call to constructor: " +
                              cTI.typeSignature() + "(true)", cTI.typeSignature());
                    return defaultValue;
                }*/
            }
            return defaultValue;
        }

        private void setFirst(T newItem)
        {
            if (this.Count == 0)
            {
                this.Add(newItem);
            }
            else if (this.Count > 0)
            {
                this[0] = newItem;
            }
        }
    }
}