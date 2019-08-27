using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace imbSCI.Core.reporting.render.builders
{
    public class builderForStreamReader<T> where T : ITextRender, IConsoleControl, new()
    {

        public Int32 CycleTick { get; set; } = 3000;

        public Boolean CycleOn { get; set; } = true;

        public void Start()
        {
            Thread cycleThread = new Thread(ReadCycle);
        }

        public void ReadCycle()
        {

            while (CycleOn)
            {
                Thread.Sleep(CycleTick);


                String ln = sourceStream.ReadToEnd();

                if (ln.isNullOrEmpty())
                {
                    output.AppendLine(ln);
                }

            }
        }


        public builderForStreamReader(StreamReader _source)
        {
            sourceStream = _source;
            output = new T();
            


        }
        public T output { get; set; }

        public StreamReader sourceStream { get; set; }
    }
}
