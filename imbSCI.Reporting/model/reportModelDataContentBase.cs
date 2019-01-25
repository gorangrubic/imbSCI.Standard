using imbSCI.Core.extensions.data;
using imbSCI.Core.files.folders;
using imbSCI.Core.reporting.render;
using imbSCI.Data.collection;
using imbSCI.Data.collection.graph;
using imbSCI.Data.enums;
using imbSCI.Data.extensions.data;
using imbSCI.Data.interfaces;
using imbSCI.Reporting.includes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace imbSCI.Reporting.model
{
public abstract class reportModelDataContentBase<T> : reportModelContentBase where T : new()
    {

        protected T data { get; set; }

        protected reportModelDataContentBase()
        {

        }

        protected void SetData(T _data)
        {
            if (_data == null) _data = new T();
            data = _data;
        }
        protected reportModelDataContentBase(T _data)
        {
            SetData(_data);
        }


    }
}