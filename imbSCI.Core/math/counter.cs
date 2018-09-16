// --------------------------------------------------------------------------------------------------------------------
// <copyright file="counter.cs" company="imbVeles" >
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
    using imbSCI.Data.data;

    #region imbVeles using

    using System;

    #endregion imbVeles using

    /// <summary>
    /// Univerzalni brojac
    /// </summary>
    public class counter : dataBindableBase
    {
        public counter()
        {
        }

        public counter(String __title, Int32 __limit, Int32 __startIndex = 0, Boolean __isRunning = false)
        {
            setup(__title, __limit, __startIndex, __isRunning);
        }

        public void setup(String __title, Int32 __limit, Int32 __startIndex = 0, Boolean __isRunning = false)
        {
            index = __startIndex;
            title = __title;
            setup(__limit, __isRunning);
        }

        /// <summary>
        /// Ako je __limit = 0, onda ce isRunning biti false
        /// </summary>
        /// <param name="__limit"></param>
        /// <param name="__isRunning"></param>
        public void setup(Int32 __limit, Boolean __isRunning = true)
        {
            limit = __limit;
            isRunning = __isRunning;
            if (__limit == 0) isRunning = false;
        }

        public string signature()
        {
            return "Counter :: " + title + " [i:" + index + " / " + limit + "]";
        }

        /// <summary>
        /// Proverava da li je index dostigao limit.
        /// </summary>
        /// <param name="moveIndex">Za koliko da pomeri index</param>
        /// <returns>TRUE ako je limit dostignut</returns>
        public Boolean check(Int32 moveIndex = 1, Boolean onNotRunning = true)
        {
            if (isRunning)
            {
                index = index + moveIndex;
            }
            else
            {
                return onNotRunning;
            }

            if (index < limit)
            {
                callCounterChecked();
                return false;
            }
            else
            {
                callLimitReached();
                return true;
            }
        }

        /// <summary>
        /// Vraca index na dati index. vraca index koji je bio pre nego sto je resetovano
        /// </summary>
        /// <param name="indexToStart"></param>
        /// <returns></returns>
        public Int32 reset(Int32 indexToStart = 0)
        {
            Int32 indexBefore = index;
            index = indexToStart;
            callCounterReset(indexBefore);
            return indexBefore;
        }

        #region --- index ------- trenutna Index pozicija

        private Int32 _index = 0;

        /// <summary>
        /// trenutna Index pozicija
        /// </summary>
        public Int32 index
        {
            get { return _index; }
            set
            {
                _index = value;
                OnPropertyChanged("index");
            }
        }

        #endregion --- index ------- trenutna Index pozicija

        #region --- limit ------- postavljen limit

        private Int32 _limit = 10;

        /// <summary>
        /// postavljen limit
        /// </summary>
        public Int32 limit
        {
            get { return _limit; }
            set
            {
                _limit = value;
                OnPropertyChanged("limit");
            }
        }

        #endregion --- limit ------- postavljen limit

        #region Event Handlers: LimitReached - Doslo je do limita

        /// <summary>
        /// Proverava da li ima handler vec
        /// </summary>
        public Boolean onLimitReached_hasHandler
        {
            get { return (onLimitReached != null); }
        }

        /// <summary>
        /// Event invoker za LimitReached - ako je ovaj objekat uzrok dogadjaja onda moze i bez argumenata da se pozove
        /// </summary>
        /// <param name="sender">Objekat koji je pozvao izvrsavanje - ako je null smatrace da je ovaj objekat uzrok dogadjaja</param>
        /// <param name="args">Argumenti dogadjaja - ako je null postavlja da je unknown</param>
        public void callLimitReached()
        {
            counterEventArgs args = new counterEventArgs(counterEventType.limitReached, signature());
            if (onCounterChecked != null) onCounterChecked(this, args);
        }

        /// <summary>
        /// Event handler za LimitReached
        /// </summary>
        protected event counterEvent onLimitReached;

        /// <summary>
        /// Postavlja event handler za LimitReached (onLimitReached)
        /// </summary>
        public void onLimitReached_addHandler(counterEvent _onLimitReached)
        {
            if (!onLimitReached_hasHandler) onLimitReached += _onLimitReached;
        }

        #endregion Event Handlers: LimitReached - Doslo je do limita

        #region Event Handlers: CounterReset - Doslo je do resetovanja brojaca

        /// <summary>
        /// Proverava da li ima handler vec
        /// </summary>
        public Boolean onCounterReset_hasHandler
        {
            get { return (onCounterReset != null); }
        }

        /// <summary>
        /// Event invoker za CounterReset - ako je ovaj objekat uzrok dogadjaja onda moze i bez argumenata da se pozove
        /// </summary>
        /// <param name="sender">Objekat koji je pozvao izvrsavanje - ako je null smatrace da je ovaj objekat uzrok dogadjaja</param>
        /// <param name="args">Argumenti dogadjaja - ako je null postavlja da je unknown</param>
        public void callCounterReset(Int32 indexBeforeReset)
        {
            counterEventArgs args = new counterEventArgs(counterEventType.counterReset,
                                                         "Index before reset: " + indexBeforeReset + " :: " +
                                                         signature());
            if (onCounterChecked != null) onCounterChecked(this, args);
        }

        /// <summary>
        /// Event handler za CounterReset
        /// </summary>
        protected event counterEvent onCounterReset;

        /// <summary>
        /// Postavlja event handler za CounterReset (onCounterReset)
        /// </summary>
        public void onCounterReset_addHandler(counterEvent _onCounterReset)
        {
            if (!onCounterReset_hasHandler) onCounterReset += _onCounterReset;
        }

        #endregion Event Handlers: CounterReset - Doslo je do resetovanja brojaca

        #region Event Handlers: CounterChecked - Pozvan je counter check

        /// <summary>
        /// Proverava da li ima handler vec
        /// </summary>
        public Boolean onCounterChecked_hasHandler
        {
            get { return (onCounterChecked != null); }
        }

        /// <summary>
        /// Event invoker za CounterChecked - ako je ovaj objekat uzrok dogadjaja onda moze i bez argumenata da se pozove
        /// </summary>
        /// <param name="sender">Objekat koji je pozvao izvrsavanje - ako je null smatrace da je ovaj objekat uzrok dogadjaja</param>
        /// <param name="args">Argumenti dogadjaja - ako je null postavlja da je unknown</param>
        public void callCounterChecked()
        {
            counterEventArgs args = new counterEventArgs(counterEventType.counterChecked, signature());
            if (onCounterChecked != null) onCounterChecked(this, args);
        }

        /// <summary>
        /// Event handler za CounterChecked
        /// </summary>
        protected event counterEvent onCounterChecked;

        /// <summary>
        /// Postavlja event handler za CounterChecked (onCounterChecked)
        /// </summary>
        public void onCounterChecked_addHandler(counterEvent _onCounterChecked)
        {
            if (!onCounterChecked_hasHandler) onCounterChecked += _onCounterChecked;
        }

        #endregion Event Handlers: CounterChecked - Pozvan je counter check

        #region --- isRunning ------- da li je counter pokrenut? ako TRUE onda ce check pomeriti index, ako je FALSE onda se index ne mrda

        private Boolean _isRunning;

        /// <summary>
        /// da li je counter pokrenut? ako TRUE onda ce check pomeriti index, ako je FALSE onda se index ne mrda
        /// </summary>
        public Boolean isRunning
        {
            get { return _isRunning; }
            set
            {
                _isRunning = value;
                OnPropertyChanged("isRunning");
            }
        }

        #endregion --- isRunning ------- da li je counter pokrenut? ako TRUE onda ce check pomeriti index, ako je FALSE onda se index ne mrda

        #region --- title ------- naziv countera

        private String _title;

        /// <summary>
        /// naziv countera
        /// </summary>
        public String title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged("title");
            }
        }

        #endregion --- title ------- naziv countera
    }
}